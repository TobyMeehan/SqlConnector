using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TobyMeehan.Sql.QueryBuilder
{
    /// <summary>
    /// Factory for SQL queries.
    /// </summary>
    public class QueryFactory
    {
        private readonly Func<IDbConnection> _connectionFactory;

        public QueryFactory(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Returns a simple SQL query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public SqlQuery<T> Query<T>() => new SqlQuery<T>();

        /// <summary>
        /// Returns an SQL query which can be executed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ExecutableSqlQuery<T> Executable<T>() => new ExecutableSqlQuery<T>(_connectionFactory());
    }
}