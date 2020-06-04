using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TobyMeehan.Sql.Extensions;

namespace TobyMeehan.Sql.QueryBuilder
{
    public partial class SqlQuery<T> : ISqlQuery<T>
    {
        private ISqlQuery<T> Where(LambdaExpression expression)
        {
            return Clone(SqlClause.Join(" ", new SqlClause("WHERE"), SqlExpression.FromExpression(expression, ref _parameterCount)));
        }

        public ISqlQuery<T> Where(Expression<Predicate<T>> expression) => Where(expression);

        public ISqlQuery<T> Where<TForeign>(Expression<Func<T, TForeign, bool>> expression) => Where(expression);


        private ISqlQuery<T> Join<TLeft, TRight>(string joinType, Expression<Func<TLeft, TRight, bool>> expression)
        {
            string tableName = typeof(TRight).GetSqlName() ?? nameof(TRight);

            return Clone(SqlClause.Join(" ",
                new SqlClause($"{joinType} JOIN {tableName} ON"),
                SqlExpression.FromExpression(expression, ref _parameterCount)));
        }

        public ISqlQuery<T> InnerJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression) => Join("INNER", expression);

        public ISqlQuery<T> InnerJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) => Join("INNER", expression);

        public ISqlQuery<T> LeftJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression) => Join("LEFT", expression);

        public ISqlQuery<T> LeftJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) => Join("LEFT", expression);

        public ISqlQuery<T> RightJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression) => Join("RIGHT", expression);

        public ISqlQuery<T> RightJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) => Join("RIGHT", expression);

        public ISqlQuery<T> FullJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression) => Join("FULL OUTER", expression);

        public ISqlQuery<T> FullJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) => Join("FULL OUTER", expression);
    }
}
