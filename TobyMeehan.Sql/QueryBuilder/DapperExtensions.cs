using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TobyMeehan.Sql.QueryBuilder
{
    public static class DapperExtensions
    {
        /// <summary>
        /// Executes the provided SQL query and returns resulting rows.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(this IDbConnection connection, ISqlQuery<T> query)
        {
            string sql = query.ToSql(out object param);

            if (query.QueryMap == null)
            {
                return connection.Query<T>(sql, param);
            }
            else
            {
                return connection.Query(sql, query.QueryMap.Types, query.QueryMap.Map, param);
            }
        }

        /// <summary>
        /// Executes the provided SQL query and returns resulting rows asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection connection, ISqlQuery<T> query)
        {
            string sql = query.ToSql(out object param);

            if (query.QueryMap == null)
            {
                return connection.QueryAsync<T>(sql, param);
            }
            else
            {
                return connection.QueryAsync(sql, query.QueryMap.Types, query.QueryMap.Map, param);
            }
        }

        /// <summary>
        /// Executes the provided SQL query.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static int Execute(this IDbConnection connection, ISqlQuery query)
        {
            return connection.Execute(query.ToSql(out object param), param);
        }

        /// <summary>
        /// Executes the provided SQL query asynchronously.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Task<int> ExecuteAsync(this IDbConnection connection, ISqlQuery query)
        {
            return connection.ExecuteAsync(query.ToSql(out object param), param);
        }
    }
}
