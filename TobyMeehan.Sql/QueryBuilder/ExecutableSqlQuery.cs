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

        #region Synchronous Queries

        /// <summary>
        /// Executes the SQL as a query.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Query()
        {
            using (_connection)
            {
                return _connection.Query<T>(AsSql(out object param), param); 
            }
        }

        /// <summary>
        /// Executes the SQL as a multi-mapping query with 2 input types.
        /// </summary>
        /// <param name="map">Function to map row types to return type.</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T1>(Func<T, T1, T> map)
        {
            using (_connection)
            {
                return _connection.Query(AsSql(out object param), map, param);
            }
        }

        /// <summary>
        /// Executes the SQL as a multi-mapping query with 3 input types.
        /// </summary>
        /// <param name="map">Function to map row types to return type.</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T1, T2>(Func<T, T1, T2, T> map)
        {
            using (_connection)
            {
                return _connection.Query(AsSql(out object param), map, param);
            }
        }

        /// <summary>
        /// Executes the SQL as a multi-mapping query with 4 input types.
        /// </summary>
        /// <param name="map">Function to map row types to return type.</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T1, T2, T3>(Func<T, T1, T2, T3, T> map)
        {
            using (_connection)
            {
                return _connection.Query(AsSql(out object param), map, param);
            }
        }

        /// <summary>
        /// Executes the SQL as a multi-mapping query with 5 input types.
        /// </summary>
        /// <param name="map">Function to map row types to return type.</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T1, T2, T3, T4>(Func<T, T1, T2, T3, T4, T> map)
        {
            using (_connection)
            {
                return _connection.Query(AsSql(out object param), map, param);
            }
        }

        /// <summary>
        /// Executes the SQL as a multi-mapping query with 6 input types.
        /// </summary>
        /// <param name="map">Function to map row types to return type.</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T1, T2, T3, T4, T5>(Func<T, T1, T2, T3, T4, T5, T> map)
        {
            using (_connection)
            {
                return _connection.Query(AsSql(out object param), map, param);
            }
        }

        /// <summary>
        /// Executes the SQL as a multi-mapping query with 7 input types.
        /// </summary>
        /// <param name="map">Function to map row types to return type.</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T1, T2, T3, T4, T5, T6>(Func<T, T1, T2, T3, T4, T5, T6, T> map)
        {
            using (_connection)
            {
                return _connection.Query(AsSql(out object param), map, param);
            }
        }

        #endregion

        #region Async Queries

        /// <summary>
        /// Asynchronously executes the SQL as a query.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync()
        {
            using (_connection)
            {
                return await _connection.QueryAsync<T>(AsSql(out object param), param);
            }
        }

        /// <summary>
        /// Asynchronously executes the SQL as a multi-mapping query with 2 input types.
        /// </summary>
        /// <param name="map">Function to map row types to return type.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T1>(Func<T, T1, T> map)
        {
            using (_connection)
            {
                return await _connection.QueryAsync(AsSql(out object param), map, param);
            }
        }

        /// <summary>
        /// Asynchronously executes the SQL as a multi-mapping query with 3 input types.
        /// </summary>
        /// <param name="map">Function to map row types to return type.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T1, T2>(Func<T, T1, T2, T> map)
        {
            using (_connection)
            {
                return await _connection.QueryAsync(AsSql(out object param), map, param);
            }
        }

        /// <summary>
        /// Asynchronously executes the SQL as a multi-mapping query with 4 input types.
        /// </summary>
        /// <param name="map">Function to map row types to return type.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T1, T2, T3>(Func<T, T1, T2, T3, T> map)
        {
            using (_connection)
            {
                return await _connection.QueryAsync(AsSql(out object param), map, param);
            }
        }

        /// <summary>
        /// Asynchronously executes the SQL as a multi-mapping query with 5 input types.
        /// </summary>
        /// <param name="map">Function to map row types to return type.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T1, T2, T3, T4>(Func<T, T1, T2, T3, T4, T> map)
        {
            using (_connection)
            {
                return await _connection.QueryAsync(AsSql(out object param), map, param);
            }
        }

        /// <summary>
        /// Asynchronously executes the SQL as a multi-mapping query with 6 input types.
        /// </summary>
        /// <param name="map">Function to map row types to return type.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T1, T2, T3, T4, T5>(Func<T, T1, T2, T3, T4, T5, T> map)
        {
            using (_connection)
            {
                return await _connection.QueryAsync(AsSql(out object param), map, param);
            }
        }

        /// <summary>
        /// Asynchronously executes the SQL as a multi-mapping query with 7 input types.
        /// </summary>
        /// <param name="map">Function to map row types to return type.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T1, T2, T3, T4, T5, T6>(Func<T, T1, T2, T3, T4, T5, T6, T> map)
        {
            using (_connection)
            {
                return await _connection.QueryAsync(AsSql(out object param), map, param);
            }
        }

        #endregion

        /// <summary>
        /// Executes the SQL.
        /// </summary>
        public int Execute()
        {
            using (_connection)
            {
                return _connection.Execute(AsSql(out object parameters), parameters);
            }
        }

        /// <summary>
        /// Executes the SQL asynchronously.
        /// </summary>
        /// <returns></returns>
        public Task<int> ExecuteAsync()
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
