using Dapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.Extensions;

namespace TobyMeehan.Sql
{
    public partial class SqlTable<T> : ISqlTable<T>
    {
        private string GetDeleteQuery(Expression<Predicate<T>> expression, out object parameters)
        {
            var whereClause = _whereBuilder.ToSql(expression);

            string sql = $"DELETE FROM {TableName} WHERE {whereClause}";

            parameters = whereClause.Parameters.ToDynamic();

            return sql;
        }

        public virtual int Delete(Expression<Predicate<T>> expression)
        {
            return _connection.Execute(GetDeleteQuery(expression, out object parameters), parameters);
        }

        public virtual Task<int> DeleteAsync(Expression<Predicate<T>> expression)
        {
            return _connection.ExecuteAsync(GetDeleteQuery(expression, out object parameters), parameters);
        }
    }
}
