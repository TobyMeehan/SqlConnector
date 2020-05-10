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
        private string GetUpdateQuery(Expression<Predicate<T>> expression, object columns, out object parameters)
        {
            return new SqlQuery(TableName)
                .Update(columns)
                .Where(expression)
                .AsSql(out parameters);
        }

        public virtual int Update(Expression<Predicate<T>> expression, object value)
        {
            return _connection.Execute(GetUpdateQuery(expression, value, out object parameters), parameters);
        }

        public virtual Task<int> UpdateAsync(Expression<Predicate<T>> expression, object value)
        {
            return _connection.ExecuteAsync(GetUpdateQuery(expression, value, out object parameters), parameters);
        }
    }
}
