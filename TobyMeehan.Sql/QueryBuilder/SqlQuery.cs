using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TobyMeehan.Sql.Extensions;

namespace TobyMeehan.Sql.QueryBuilder
{
    /// <summary>
    /// Class representing a complete SQL query.
    /// </summary>
    public class SqlQuery<T>
    {
        private readonly string _tableName;

        /// <summary>
        /// Creates a new instance of the SqlQuery class.
        /// </summary>
        public SqlQuery()
        {
            _tableName = typeof(T).GetSqlName() ?? typeof(T).Name;
        }

        private SqlQuery(SqlQuery<T> sqlQuery, SqlClause addition, int? index = null)
        {
            _tableName = typeof(T).GetSqlName() ?? typeof(T).Name;
            _clauses = sqlQuery._clauses;
            _parameters = sqlQuery._parameters;

            if (index == null)
            {
                _clauses.Add(addition);
            }
            else
            {
                _clauses.Insert(index.Value, addition);
            }

            _parameters = _parameters.Union(addition.Parameters).ToDictionary(x => x.Key, x => x.Value);

            _parameterCount = _parameters.Count + 1;
        }

        private List<SqlClause> _clauses = new List<SqlClause>();
        private int _parameterCount = 1;
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();

        /// <summary>
        /// Executes the query only on records matching the provided expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SqlQuery<T> Where(Expression<Predicate<T>> expression)
        {
            return new SqlQuery<T>(this, SqlClause.Join(" ", new SqlClause("WHERE"), SqlExpression.FromExpression(expression.Body, ref _parameterCount)));
        }

        //private SqlQuery<T> Join<TJoin>(string joinType, Expression<Func<T, TJoin, bool>> expression)
        //{
        //    return Join<T, TJoin>(joinType, expression);
        //}

        private SqlQuery<T> Join<TLeft, TRight>(string joinType, Expression<Func<TLeft, TRight, bool>> expression)
        {
            Type leftTable = typeof(TLeft);
            Type rightTable = typeof(TRight);

            string leftTableName = leftTable.GetSqlName() ?? leftTable.Name;
            string rightTableName = rightTable.GetSqlName() ?? rightTable.Name;

            return new SqlQuery<T>(this, SqlClause.Join(" ",
                new SqlClause($"{joinType} JOIN {rightTableName} ON"),
                SqlExpression.FromExpression(expression.Body, ref _parameterCount)));
        }

        /// <summary>
        /// Selects records with matching values in both tables.
        /// </summary>
        /// <typeparam name="TJoin">Table to join with.</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SqlQuery<T> InnerJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression) => Join("INNER", expression);
        /// <summary>
        /// Selects records with matching values in both tables.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SqlQuery<T> InnerJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) => Join("INNER", expression);
        /// <summary>
        /// Selects any matched records from the right table.
        /// </summary>
        /// <typeparam name="TJoin">Table to join with.</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SqlQuery<T> LeftJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression) => Join("LEFT", expression);
        /// <summary>
        /// Selects any matched records from the right table.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SqlQuery<T> LeftJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) => Join("LEFT", expression);
        /// <summary>
        /// Selects any matched records from the left table.
        /// </summary>
        /// <typeparam name="TJoin">Table to join with.</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SqlQuery<T> RightJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression) => Join("RIGHT", expression);
        /// <summary>
        /// Selects any matched records from the left table.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SqlQuery<T> RightJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) => Join("RIGHT", expression);
        /// <summary>
        /// Selects all records with a match in either table.
        /// </summary>
        /// <typeparam name="TJoin">Table to join with.</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SqlQuery<T> FullJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression) => Join("FULL OUTER", expression);
        /// <summary>
        /// Selects all records with a match in either table.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SqlQuery<T> FullJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression) => Join("FULL OUTER", expression);

        /// <summary>
        /// Selects all columns from the table.
        /// </summary>
        /// <returns></returns>
        public SqlQuery<T> Select() => Select("*");
        /// <summary>
        /// Selects the provided columns from the table.
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public SqlQuery<T> Select(params string[] columns)
        {
            return new SqlQuery<T>(this, new SqlClause($"SELECT {string.Join(", ", columns)} FROM {_tableName}"), 0);
        }

        /// <summary>
        /// Inserts a new record into the table.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlQuery<T> Insert(object values)
        {
            var properties = values.GetType().GetProperties().Select(x => x.Name);

            int i = _parameterCount;

            SqlClause sql = SqlClause.Join("",
                new SqlClause($"INSERT INTO {_tableName} ("),
                SqlClause.Join(", ", properties.Select(x => new SqlClause(x))),
                new SqlClause(") VALUES "),
                SqlClause.FromCollection(ref i, values.ToDictionary().Select(x => x.Value)));

            return new SqlQuery<T>(this, sql, 0);
        }

        /// <summary>
        /// Updates records in the table.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlQuery<T> Update(object values)
        {
            var properties = values.GetType().GetProperties();

            int i = _parameterCount;

            SqlClause updateClause = new SqlClause($"UPDATE {_tableName} SET");

            List<SqlClause> clauses = new List<SqlClause>();

            foreach (var item in properties)
            {
                clauses.Add(SqlClause.Join(" = ", new SqlClause(item.Name), SqlClause.FromParameter(i, item.GetValue(values))));
                i++;
            }

            return new SqlQuery<T>(this, SqlClause.Join(" ", updateClause, SqlClause.Join(", ", clauses)), 0);
        }

        /// <summary>
        /// Deletes records from the table.
        /// </summary>
        /// <returns></returns>
        public SqlQuery<T> Delete()
        {
            return new SqlQuery<T>(this, new SqlClause($"DELETE FROM {_tableName}"), 0);
        }

        /// <summary>
        /// Gets the SQL query as a string.
        /// </summary>
        /// <returns></returns>
        public string AsSql()
        {
            return string.Join(" ", _clauses.Select(x => x.Sql));
        }

        /// <summary>
        /// Gets the SQL query as a string and outputs the generated parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string AsSql(out Dictionary<string, object> parameters)
        {
            parameters = _parameters;

            return AsSql();
        }

        /// <summary>
        /// Gets the SQL query as a string and outputs the generated parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string AsSql(out object parameters)
        {
            parameters = new DynamicParameters(_parameters);

            return AsSql();
        }
    }
}
