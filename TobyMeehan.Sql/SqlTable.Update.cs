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
        private ISqlQuery<T> GetUpdateQuery(Expression<Predicate<T>> expression, object columns)
        {
            return new SqlQuery<T>()
                .Update(columns)
                .Where(expression);
        }

        public virtual int Update(Expression<Predicate<T>> expression, object value)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.Execute(GetUpdateQuery(expression, value));
            }
        }

        public virtual Task<int> UpdateAsync(Expression<Predicate<T>> expression, object value)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.ExecuteAsync(GetUpdateQuery(expression, value));
            }
        }
    }
}
