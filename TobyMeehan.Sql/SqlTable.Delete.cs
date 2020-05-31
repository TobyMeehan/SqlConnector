using Dapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.Extensions;
using TobyMeehan.Sql.QueryBuilder;

namespace TobyMeehan.Sql
{
    public partial class SqlTable<T> : ISqlTable<T>
    {
        private ExecutableSqlQuery<T> GetDeleteQuery(Expression<Predicate<T>> expression)
        {
            return _queryFactory.Executable<T>()
                .Delete()
                .Where(expression);
        }

        public virtual int Delete(Expression<Predicate<T>> expression)
        {
            return GetDeleteQuery(expression).Execute();
        }

        public virtual Task<int> DeleteAsync(Expression<Predicate<T>> expression)
        {
            return GetDeleteQuery(expression).ExecuteAsync();
        }
    }
}
