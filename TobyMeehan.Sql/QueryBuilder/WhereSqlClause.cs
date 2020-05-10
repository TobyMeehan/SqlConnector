using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace TobyMeehan.Sql.QueryBuilder
{
    public class WhereSqlClause<T> : SqlClause
    {
        public WhereSqlClause(Expression<Predicate<T>> expression, ref int parameterCount)
        {
            SqlClause child = Evaluate(ref parameterCount, expression.Body, true);

            Sql = $"WHERE {child.Sql}";
            Parameters = child.Parameters;
        }

        private SqlClause Evaluate(ref int i, Expression expression, bool isUnary = false)
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
                    string column = property.Name;

                    if (isUnary && member.Type == typeof(bool))
                    {
                        return Concat(Evaluate(ref i, expression), "=", FromParameter(i++, true));
                    }

                    return new SqlClause(column);
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

            throw new ArgumentException(nameof(expression), $"Unsupported expression '{expression.GetType().Name}'");
        }

        private static object GetExpressionValue(Expression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            return getterLambda.Compile().Invoke();
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
