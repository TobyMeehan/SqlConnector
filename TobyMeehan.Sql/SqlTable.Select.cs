using Dapper;
using System;
using System.Collections.Generic;
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
        private string GetSelectQuery() => GetSelectQuery("*");

        private string GetSelectQuery(params string[] columns)
        {
            return new SqlQuery<T>()
                .Select(columns)
                .AsSql();
        }

        public virtual IEnumerable<T> Select()
        {
            return _connection.Query<T>(GetSelectQuery());
        }

        public virtual Task<IEnumerable<T>> SelectAsync()
        {
            return _connection.QueryAsync<T>(GetSelectQuery());
        }

        public virtual IEnumerable<T> Select(params string[] columns)
        {
            return _connection.Query<T>(GetSelectQuery(columns));
        }

        public virtual Task<IEnumerable<T>> SelectAsync(params string[] columns)
        {
            return _connection.QueryAsync<T>(GetSelectQuery(columns));
        }

        private string GetSelectByQuery(Expression<Predicate<T>> expression, out object parameters, params string[] columns)
        {
            return new SqlQuery<T>()
                .Select(columns)
                .Where(expression)
                .AsSql(out parameters);
        }

        private string GetSelectByQuery<TForeign>(Expression<Func<T, TForeign, bool>> expression, out object parameters, params string[] columns)
        {
            return new SqlQuery<T>()
                .Select(columns)
                .Where(expression)
                .AsSql(out parameters);
        }

        public virtual IEnumerable<T> SelectBy(Expression<Predicate<T>> expression)
        {
            return _connection.Query<T>(GetSelectByQuery(expression, out object parameters, "*"), parameters);
        }

        public virtual IEnumerable<T> SelectBy<TForeign>(Expression<Func<T, TForeign, bool>> expression)
        {
            return _connection.Query<T>(GetSelectByQuery(expression, out object parameters, "*"), parameters);
        }

        public virtual IEnumerable<T> SelectBy(Expression<Predicate<T>> expression, params string[] columns)
        {
            return _connection.Query<T>(GetSelectByQuery(expression, out object parameters, columns), parameters);
        }

        public virtual IEnumerable<T> SelectBy<TForeign>(Expression<Func<T, TForeign, bool>> expression, params string[] columns)
        {
            return _connection.Query<T>(GetSelectByQuery(expression, out object parameters, columns), parameters);
        }

        public virtual Task<IEnumerable<T>> SelectByAsync(Expression<Predicate<T>> expression)
        {
            return _connection.QueryAsync<T>(GetSelectByQuery(expression, out object parameters, "*"), parameters);
        }

        public virtual Task<IEnumerable<T>> SelectByAsync<TForeign>(Expression<Func<T, TForeign, bool>> expression)
        {
            return _connection.QueryAsync<T>(GetSelectByQuery(expression, out object parameters, "*"), parameters);
        }

        public virtual Task<IEnumerable<T>> SelectByAsync(Expression<Predicate<T>> expression, params string[] columns)
        {
            return _connection.QueryAsync<T>(GetSelectByQuery(expression, out object parameters, columns), parameters);
        }

        public virtual Task<IEnumerable<T>> SelectByAsync<TForeign>(Expression<Func<T, TForeign, bool>> expression, params string[] columns)
        {
            return _connection.QueryAsync<T>(GetSelectByQuery(expression, out object parameters, columns), parameters);
        }
    }
}
