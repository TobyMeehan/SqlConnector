using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.Extensions;
using TobyMeehan.Sql.QueryBuilder;

namespace TobyMeehan.Sql
{
    public partial class SqlTable<T> : ISqlTable<T>
    {
        private ISqlQuery<T> GetSelectQuery() => GetSelectQuery("*");

        private ISqlQuery<T> GetSelectQuery(params string[] columns)
        {
            return new SqlQuery<T>()
                .Select();
        }

        public virtual IEnumerable<T> Select()
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.Query(GetSelectQuery());
            }
        }

        public virtual Task<IEnumerable<T>> SelectAsync()
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.QueryAsync(GetSelectQuery());
            }
        }

        public virtual IEnumerable<T> Select(params string[] columns)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.Query(GetSelectQuery(columns));
            }
        }

        public virtual Task<IEnumerable<T>> SelectAsync(params string[] columns)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.QueryAsync(GetSelectQuery(columns));
            }
        }

        public virtual IEnumerable<T> SelectBy(Expression<Predicate<T>> expression)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.Query(GetSelectQuery().Where(expression));
            }
        }

        public virtual IEnumerable<T> SelectBy<TForeign>(Expression<Func<T, TForeign, bool>> expression)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.Query(GetSelectQuery().Where(expression));
            }
        }

        public virtual IEnumerable<T> SelectBy(Expression<Predicate<T>> expression, params string[] columns)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.Query(GetSelectQuery(columns).Where(expression));
            }
        }

        public virtual IEnumerable<T> SelectBy<TForeign>(Expression<Func<T, TForeign, bool>> expression, params string[] columns)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.Query(GetSelectQuery(columns).Where(expression));
            }
        }

        public virtual Task<IEnumerable<T>> SelectByAsync(Expression<Predicate<T>> expression)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.QueryAsync(GetSelectQuery().Where(expression));
            }
        }

        public virtual Task<IEnumerable<T>> SelectByAsync<TForeign>(Expression<Func<T, TForeign, bool>> expression)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.QueryAsync(GetSelectQuery().Where(expression));
            }
        }

        public virtual Task<IEnumerable<T>> SelectByAsync(Expression<Predicate<T>> expression, params string[] columns)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.QueryAsync(GetSelectQuery(columns).Where(expression));
            }
        }

        public virtual Task<IEnumerable<T>> SelectByAsync<TForeign>(Expression<Func<T, TForeign, bool>> expression, params string[] columns)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.QueryAsync(GetSelectQuery(columns).Where(expression));
            }
        }
    }
}
