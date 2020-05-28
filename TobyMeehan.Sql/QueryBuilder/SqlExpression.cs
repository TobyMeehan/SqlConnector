using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TobyMeehan.Sql.Extensions;

namespace TobyMeehan.Sql.QueryBuilder
{
    class SqlExpression : SqlClause    
    {
        public static SqlClause FromExpression(Expression expression, ref int parameterCount)
        {
            return Evaluate(ref parameterCount, expression, true);
        }

        private static SqlClause Evaluate(ref int i, Expression expression, bool isUnary = false)
        {
            if (expression is UnaryExpression unary)
            {
                return Concat(DotNetNodeToSql(unary.NodeType), Evaluate(ref i, unary.Operand, true));
            }

            if (expression is BinaryExpression body)
            {
                return Concat(Evaluate(ref i, body.Left), DotNetNodeToSql(body.NodeType), Evaluate(ref i, body.Right));
            }

            if (expression is ConstantExpression constant)
            {
                var value = constant.Value;

                if (value is int)
                {
                    return new SqlClause(value.ToString());
                }

                if (value is string)
                {
                    value = (string)value;
                }

                if (value is bool && isUnary)
                {
                    return Concat(FromParameter(i++, value), "=", new SqlClause("1"));
                }

                return FromParameter(i++, value);
            }

            if (expression is MemberExpression member)
            {
                if (member.Member is PropertyInfo property)
                {
                    string columnName = property.GetSqlName() ?? property.Name;
                    string tableName = member.Expression.Type.GetSqlName() ?? member.Expression.Type.Name; // property.ReflectedType is not used due to inheritance problems

                    if (isUnary && member.Type == typeof(bool))
                    {
                        return Concat(Evaluate(ref i, expression), "=", FromParameter(i++, true));
                    }

                    return new SqlClause($"{tableName}.{columnName}");
                }

                if (member.Member is FieldInfo)
                {
                    var value = GetExpressionValue(member);

                    if (value is string)
                    {
                        value = (string)value;
                    }

                    return FromParameter(i++, value);
                }

                throw new ArgumentException(nameof(expression), "Expression does not refer to a property or field.");
            }

            if (expression is MethodCallExpression method)
            {
                var value = GetMethodExpressionValue(method);

                if (value is string)
                {
                    value = (string)value;
                }

                return FromParameter(i++, value);
            }

            throw new ArgumentException(nameof(expression), $"Unsupported expression '{expression.GetType().Name}'");
        }

        private static object GetExpressionValue(Expression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            return getterLambda.Compile().Invoke();
        }

        private static object GetMethodExpressionValue(MethodCallExpression expression)
        {
            return Expression.Lambda(expression).Compile().DynamicInvoke();
        }

        private static string DotNetNodeToSql(ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.Add:
                    return "+";
                case ExpressionType.And:
                    return "&";
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.ExclusiveOr:
                    return "^";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.Multiply:
                    return "*";
                case ExpressionType.Negate:
                    return "-";
                case ExpressionType.Not:
                    return "NOT";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Or:
                    return "|";
                case ExpressionType.OrElse:
                    return "OR";
                case ExpressionType.Subtract:
                    return "-";
            }

            throw new ArgumentOutOfRangeException(nameof(nodeType), $"Unsupported node type.");
        }
    }
}
