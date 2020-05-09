using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TobyMeehan.Sql
{
    public partial class SqlTable<T> : ISqlTable<T>
    {
        private string GetInsertQuery(object parameters)
        {
            var properties = parameters.GetType().GetProperties().Select(x => x.Name);

            string sql = $"INSERT INTO {TableName} " +
                $"({string.Join(", ", properties)})" +
                $" VALUES " +
                $"({string.Join(", ", GetValues(parameters))})";

            return sql;
        }

        private IEnumerable<string> GetValues(object obj)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.GetValue(obj) is SqlString s)
                {
                    yield return s;
                }
                else
                {
                    yield return $"@{property.Name}";
                }
            }
        }

        public int Insert(object value)
        {
            return _connection.Execute(GetInsertQuery(value), value);
        }

        public Task<int> InsertAsync(object value)
        {
            return _connection.ExecuteAsync(GetInsertQuery(value), value);
        }
    }
}
