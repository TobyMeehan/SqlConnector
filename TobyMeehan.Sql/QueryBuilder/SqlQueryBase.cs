using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TobyMeehan.Sql.Extensions;

namespace TobyMeehan.Sql.QueryBuilder
{
    public abstract class SqlQueryBase<T, TDerived> where TDerived : SqlQueryBase<T, TDerived>
    {
        protected virtual TDerived Clone(SqlClause addition, int? index = null)
        {
            TDerived derived = Clone(this as TDerived);

            derived.Clauses = this.Clauses;
            derived.Parameters = this.Parameters;

            if (index == null)
            {
                derived.Clauses.Add(addition);
            }
            else
            {
                derived.Clauses.Insert(index.Value, addition);
            }

            derived.Parameters = derived.Parameters.Union(addition.Parameters).ToDictionary(x => x.Key, x => x.Value);

            derived.ParameterCount = derived.Parameters.Count + 1;

            return derived;
        }

        protected abstract TDerived Clone(TDerived source);

        protected string TableName => typeof(T).GetSqlName() ?? typeof(T).Name;
        protected List<SqlClause> Clauses = new List<SqlClause>();
        protected int ParameterCount = 1;
        protected Dictionary<string, object> Parameters = new Dictionary<string, object>();

        private TDerived Where(Expression expression)
        {
            return Clone(SqlClause.Join(" ", new SqlClause("WHERE"), SqlExpression.FromExpression(expression, ref ParameterCount)));
        }

        /// <summary>
        /// Executes the query only on records matching the provided expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TDerived Where(Expression<Predicate<T>> expression) => Where(expression.Body);

        /// <summary>
        /// Executes the query only on records matching the provided expression, using columns from a foreign table.
        /// </summary>
        /// <typeparam name="TForeign"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TDerived Where<TForeign>(Expression<Func<T, TForeign, bool>> expression) => Where(expression.Body);

        private TDerived Join<TLeft, TRight>(string joinType, Expression<Func<TLeft, TRight, bool>> expression)
        {
            Type leftTable = typeof(TLeft);
            Type rightTable = typeof(TRight);

            string leftTableName = leftTable.GetSqlName() ?? leftTable.Name;
            string rightTableName = rightTable.GetSqlName() ?? rightTable.Name;

            return Clone(SqlClause.Join(" ",
                new SqlClause($"{joinType} JOIN {rightTableName} ON"),
                SqlExpression.FromExpression(expression.Body, ref ParameterCount)));
        }

        /// <summary>
        /// Selects records with matching values in both tables.
        /// </summary>
        /// <typeparam name="TJoin">Table to join with.</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TDerived InnerJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression) => Join("INNER", expression);
        /// <summary>
        /// Selects records with matching values in both tables.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TDerived InnerJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) => Join("INNER", expression);
        /// <summary>
        /// Selects any matched records from the right table.
        /// </summary>
        /// <typeparam name="TJoin">Table to join with.</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TDerived LeftJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression) => Join("LEFT", expression);
        /// <summary>
        /// Selects any matched records from the right table.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TDerived LeftJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) => Join("LEFT", expression);
        /// <summary>
        /// Selects any matched records from the left table.
        /// </summary>
        /// <typeparam name="TJoin">Table to join with.</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TDerived RightJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression) => Join("RIGHT", expression);
        /// <summary>
        /// Selects any matched records from the left table.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TDerived RightJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) => Join("RIGHT", expression);
        /// <summary>
        /// Selects all records with a match in either table.
        /// </summary>
        /// <typeparam name="TJoin">Table to join with.</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TDerived FullJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression) => Join("FULL OUTER", expression);
        /// <summary>
        /// Selects all records with a match in either table.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TDerived FullJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) => Join("FULL OUTER", expression);

        /// <summary>
        /// Selects all columns from the table.
        /// </summary>
        /// <returns></returns>
        public TDerived Select() => Select("*");
        /// <summary>
        /// Selects the provided columns from the table.
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public TDerived Select(params string[] columns)
        {
            return Clone(new SqlClause($"SELECT {string.Join(", ", columns)} FROM {TableName}"), 0);
        }

        /// <summary>
        /// Inserts a new record into the table.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public TDerived Insert(object values)
        {
            var properties = values.GetType().GetProperties().Select(x => x.Name);

            int i = ParameterCount;

            SqlClause sql = SqlClause.Join("",
                new SqlClause($"INSERT INTO {TableName} ("),
                SqlClause.Join(", ", properties.Select(x => new SqlClause(x))),
                new SqlClause(") VALUES "),
                SqlClause.FromCollection(ref i, values.ToDictionary().Select(x => x.Value)));

            return Clone(sql, 0);
        }

        /// <summary>
        /// Updates records in the table.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public TDerived Update(object values)
        {
            var properties = values.GetType().GetProperties();

            int i = ParameterCount;

            SqlClause updateClause = new SqlClause($"UPDATE {TableName} SET");

            List<SqlClause> clauses = new List<SqlClause>();

            foreach (var item in properties)
            {
                clauses.Add(SqlClause.Join(" = ", new SqlClause(item.Name), SqlClause.FromParameter(i, item.GetValue(values))));
                i++;
            }

            return Clone(SqlClause.Join(" ", updateClause, SqlClause.Join(", ", clauses)), 0);
        }

        /// <summary>
        /// Deletes records from the table.
        /// </summary>
        /// <returns></returns>
        public TDerived Delete()
        {
            return Clone(new SqlClause($"DELETE FROM {TableName}"), 0);
        }

        /// <summary>
        /// Gets the SQL query as a string.
        /// </summary>
        /// <returns></returns>
        public string AsSql()
        {
            return string.Join(" ", Clauses.Select(x => x.Sql));
        }

        /// <summary>
        /// Gets the SQL query as a string and outputs the generated parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string AsSql(out Dictionary<string, object> parameters)
        {
            parameters = Parameters;

            return AsSql();
        }

        /// <summary>
        /// Gets the SQL query as a string and outputs the generated parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string AsSql(out object parameters)
        {
            parameters = new DynamicParameters(Parameters);

            return AsSql();
        }
    }
}
