using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.Extensions;
using TobyMeehan.Sql.QueryBuilder;

namespace TobyMeehan.Sql
{
    public partial class SqlTable<T> : ISqlTable<T>
    {
        private ISqlQuery<T> GetDeleteQuery(Expression<Predicate<T>> expression)
        {
            return new SqlQuery<T>()
                .Delete()
                .Where(expression);
        }

        public virtual int Delete(Expression<Predicate<T>> expression)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.Execute(GetDeleteQuery(expression));
            }
        }

        public virtual Task<int> DeleteAsync(Expression<Predicate<T>> expression)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.ExecuteAsync(GetDeleteQuery(expression));
            }
        }
    }
}
