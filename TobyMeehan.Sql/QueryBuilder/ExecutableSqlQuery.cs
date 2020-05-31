using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace TobyMeehan.Sql.QueryBuilder
{
    public class ExecutableSqlQuery<T> : SqlQueryBase<T, ExecutableSqlQuery<T>>, IDisposable
    {
        private readonly IDbConnection _connection;

        public ExecutableSqlQuery(IDbConnection connection)
        {
            _connection = connection;
        }

        protected override ExecutableSqlQuery<T> Clone(ExecutableSqlQuery<T> source)
        {
            return new ExecutableSqlQuery<T>(source._connection);
        }

        /// <summary>
        /// Executes the SQL as a query.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Query()
        {
            using (_connection)
            {
                return _connection.Query<T>(AsSql(out object parameters), parameters); 
            }
        }
        
        /// <summary>
        /// Executes the SQL as a query asynchronously.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<T>> QueryAsync()
        {
            using (_connection)
            {
                return _connection.QueryAsync<T>(AsSql(out object parameters), parameters);
            }
        }

        /// <summary>
        /// Executes the SQL.
        /// </summary>
        public void Execute()
        {
            using (_connection)
            {
                _connection.Execute(AsSql(out object parameters), parameters);
            }
        }

        /// <summary>
        /// Executes the SQL asynchronously.
        /// </summary>
        /// <returns></returns>
        public Task ExecuteAsync()
        {
            using (_connection)
            {
                return _connection.ExecuteAsync(AsSql(out object parameters), parameters);
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
