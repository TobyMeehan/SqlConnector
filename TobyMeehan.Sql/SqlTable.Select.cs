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
        private ExecutableSqlQuery<T> GetSelectQuery() => GetSelectQuery("*");

        private ExecutableSqlQuery<T> GetSelectQuery(params string[] columns)
        {
            return _queryFactory.Executable<T>()
                .Select();
        }

        public virtual IEnumerable<T> Select()
        {
            return GetSelectQuery().Query();
        }

        public virtual Task<IEnumerable<T>> SelectAsync()
        {
            return GetSelectQuery().QueryAsync();
        }

        public virtual IEnumerable<T> Select(params string[] columns)
        {
            return GetSelectQuery(columns).Query();
        }

        public virtual Task<IEnumerable<T>> SelectAsync(params string[] columns)
        {
            return GetSelectQuery(columns).QueryAsync();
        }

        public virtual IEnumerable<T> SelectBy(Expression<Predicate<T>> expression)
        {
            return GetSelectQuery().Where(expression).Query();
        }

        public virtual IEnumerable<T> SelectBy<TForeign>(Expression<Func<T, TForeign, bool>> expression)
        {
            return GetSelectQuery().Where(expression).Query();
        }

        public virtual IEnumerable<T> SelectBy(Expression<Predicate<T>> expression, params string[] columns)
        {
            return GetSelectQuery(columns).Where(expression).Query();
        }

        public virtual IEnumerable<T> SelectBy<TForeign>(Expression<Func<T, TForeign, bool>> expression, params string[] columns)
        {
            return GetSelectQuery(columns).Where(expression).Query();
        }

        public virtual Task<IEnumerable<T>> SelectByAsync(Expression<Predicate<T>> expression)
        {
            return GetSelectQuery().Where(expression).QueryAsync();
        }

        public virtual Task<IEnumerable<T>> SelectByAsync<TForeign>(Expression<Func<T, TForeign, bool>> expression)
        {
            return GetSelectQuery().Where(expression).QueryAsync();
        }

        public virtual Task<IEnumerable<T>> SelectByAsync(Expression<Predicate<T>> expression, params string[] columns)
        {
            return GetSelectQuery(columns).Where(expression).QueryAsync();
        }

        public virtual Task<IEnumerable<T>> SelectByAsync<TForeign>(Expression<Func<T, TForeign, bool>> expression, params string[] columns)
        {
            return GetSelectQuery(columns).Where(expression).QueryAsync();
        }
    }
}
