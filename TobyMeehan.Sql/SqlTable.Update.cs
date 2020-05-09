using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.Extensions;

namespace TobyMeehan.Sql
{
    public partial class SqlTable<T> : ISqlTable<T>
    {
        private string GetUpdateQuery(Expression<Predicate<T>> expression, object columns, out object parameters)
        {
            var properties = columns.GetType().GetProperties();
            var whereClause = _whereBuilder.ToSql(expression);

            string sql = $"UPDATE {TableName} SET " +
                $"{string.Join(", ", properties.Select(x => $"{_nameResolver.ResolveColumn(x.Name)} = {GetParameterValue(x, columns)}"))}" +
                $" WHERE " +
                $"{whereClause.Sql}";

            foreach (var item in columns.ToDictionary())
            {
                whereClause.Parameters.Add(item.Key, item.Value);
            }

            parameters = whereClause.Parameters.ToDynamic();

            return sql;
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
