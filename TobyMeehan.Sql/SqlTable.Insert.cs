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
                $"({string.Join(", ", GetParameterValues(parameters))})";

            return sql;
        }

        public virtual int Insert(object value)
        {
            return _connection.Execute(GetInsertQuery(value), value);
        }

        public virtual Task<int> InsertAsync(object value)
        {
            return _connection.ExecuteAsync(GetInsertQuery(value), value);
        }
    }
}
