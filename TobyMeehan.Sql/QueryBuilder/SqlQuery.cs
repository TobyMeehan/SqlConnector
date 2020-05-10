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
    public class SqlQuery
    {
        private readonly string _tableName;

        /// <summary>
        /// Creates a new instance of the SqlQuery class, with the provided table name.
        /// </summary>
        /// <param name="tableName"></param>
        public SqlQuery(string tableName)
        {
            _tableName = tableName;
        }

        private SqlQuery(SqlQuery sqlQuery, SqlClause addition, int? index = null)
        {
            _tableName = sqlQuery._tableName;
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
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SqlQuery Where<T>(Expression<Predicate<T>> expression)
        {
            return new SqlQuery(this, new WhereSqlClause<T>(expression, ref _parameterCount));
        }

        /// <summary>
        /// Selects all columns from the table.
        /// </summary>
        /// <returns></returns>
        public SqlQuery Select() => Select("*");
        /// <summary>
        /// Selects the provided columns from the table.
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public SqlQuery Select(params string[] columns)
        {
            return new SqlQuery(this, new SqlClause($"SELECT {string.Join(", ", columns)} FROM {_tableName}"), 0);
        }

        /// <summary>
        /// Inserts a new record into the table.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlQuery Insert(object values)
        {
            var properties = values.GetType().GetProperties().Select(x => x.Name);

            int i = _parameterCount;

            SqlClause sql = SqlClause.Join("",
                new SqlClause($"INSERT INTO {_tableName} ("),
                SqlClause.Join(", ", properties.Select(x => new SqlClause(x))),
                new SqlClause(") VALUES "),
                SqlClause.FromCollection(ref i, values.ToDictionary().Select(x => x.Value)));

            return new SqlQuery(this, sql, 0);
        }

        /// <summary>
        /// Updates records in the table.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlQuery Update(object values)
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

            return new SqlQuery(this, SqlClause.Join(" ", updateClause, SqlClause.Join(", ", clauses)), 0);
        }

        /// <summary>
        /// Deletes records from the table.
        /// </summary>
        /// <returns></returns>
        public SqlQuery Delete()
        {
            return new SqlQuery(this, new SqlClause($"DELETE FROM {_tableName}"), 0);
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
            parameters = _parameters.ToDynamic();

            return AsSql();
        }
    }
}
