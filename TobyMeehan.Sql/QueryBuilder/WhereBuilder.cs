using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace TobyMeehan.Sql.QueryBuilder
{
    public class WhereBuilder : IWhereBuilder
    {
        private readonly IDbNameResolver _nameResolver;

        public WhereBuilder(IDbNameResolver nameResolver)
        {
            _nameResolver = nameResolver;
        }

        public SqlClause ToSql<T>(Expression<Predicate<T>> expression)
        {
            var i = 1;

            return Evaluate(ref i, expression.Body, isUnary: true);
        }

        private SqlClause Evaluate(ref int i, Expression expression, bool isUnary = false, string prefix = null, string suffix = null)
        {
            if (expression is UnaryExpression unary)
            {
                return SqlClause.Concat(NodeTypeToString(unary.NodeType), Evaluate(ref i, unary.Operand, true));
            }

            if (expression is BinaryExpression body)
            {
                return SqlClause.Concat(Evaluate(ref i, body.Left), NodeTypeToString(body.NodeType), Evaluate(ref i, body.Right));
            }

            if (expression is ConstantExpression constant)
            {
                var value = constant.Value;

                if (value is int)
                {
                    return SqlClause.FromSql(value.ToString());
                }

                if (value is string)
                {
                    value = prefix + (string)value + suffix;
                }

                if (value is bool && isUnary)
                {
                    return SqlClause.Concat(SqlClause.FromParameter(i++, value), "=", SqlClause.FromSql("1"));
                }

                return SqlClause.FromParameter(i++, value);
            }

            if (expression is MemberExpression member)
            {
                if (member.Member is PropertyInfo property)
                {
                    string column = _nameResolver.ResolveColumn(property.Name);

                    if (isUnary && member.Type == typeof(bool))
                    {
                        return SqlClause.Concat(Evaluate(ref i, expression), "=", SqlClause.FromParameter(i++, true));
                    }

                    return SqlClause.FromSql(column);
                }

                if (member.Member is FieldInfo)
                {
                    var value = GetValue(member);

                    if (value is string)
                    {
                        value = prefix + (string)value + suffix;
                    }

                    return SqlClause.FromParameter(i++, value);
                }

                throw new ArgumentException(nameof(expression), "Expression does not refer to a property or field.");
            }

            if (expression is MethodCallExpression methodCall)
            {
                if (methodCall.Method == typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) }))
                {
                    return SqlClause.Concat(Evaluate(ref i, methodCall.Object), "LIKE", Evaluate(ref i, methodCall.Arguments.First(), prefix: "%", suffix: "%"));
                }

                if (methodCall.Method == typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) }))
                {
                    return SqlClause.Concat(Evaluate(ref i, methodCall.Object), "LIKE", Evaluate(ref i, methodCall.Arguments.First(), suffix: "%"));
                }

                if (methodCall.Method == typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) }))
                {
                    return SqlClause.Concat(Evaluate(ref i, methodCall.Object), "LIKE", Evaluate(ref i, methodCall.Arguments.First(), prefix: "%"));
                }

                if (methodCall.Method.Name == nameof(string.Contains))
                {
                    Expression collection;
                    Expression property;

                    if (methodCall.Method.IsDefined(typeof(ExtensionAttribute)) && methodCall.Arguments.Count == 2)
                    {
                        collection = methodCall.Arguments[0];
                        property = methodCall.Arguments[1];
                    }
                    else if (!methodCall.Method.IsDefined(typeof(ExtensionAttribute)) && methodCall.Arguments.Count == 1)
                    {
                        collection = methodCall.Object;
                        property = methodCall.Arguments[0];
                    }
                    else
                    {
                        throw new ArgumentException(nameof(expression), $"Unsupported method call '{methodCall.Method.Name}'");
                    }

                    var values = (IEnumerable<object>)GetValue(collection);
                    return SqlClause.Concat(Evaluate(ref i, property), "IN", SqlClause.FromCollection(ref i, values));
                }

                throw new ArgumentException(nameof(expression), $"Unsupported method call '{methodCall.Method.Name}'");
            }

            throw new ArgumentException(nameof(expression), $"Unsupported expression '{expression.GetType().Name}'");
        }

        private static object GetValue(Expression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            return getterLambda.Compile();
        }

        private static string NodeTypeToString(ExpressionType nodeType)
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
