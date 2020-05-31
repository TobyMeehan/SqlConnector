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
        private ExecutableSqlQuery<T> GetUpdateQuery(Expression<Predicate<T>> expression, object columns)
        {
            return _queryFactory.Executable<T>()
                .Update(columns)
                .Where(expression);
        }

        public virtual int Update(Expression<Predicate<T>> expression, object value)
        {
            return GetUpdateQuery(expression, value).Execute();
        }

        public virtual Task<int> UpdateAsync(Expression<Predicate<T>> expression, object value)
        {
            return GetUpdateQuery(expression, value).ExecuteAsync();
        }
    }
}
