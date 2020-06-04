using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using TobyMeehan.Sql.Extensions;
using TobyMeehan.Sql.QueryBuilder;

namespace TobyMeehan.Sql
{
    public partial class SqlTable<T> : ISqlTable<T>
    {
        private readonly Func<IDbConnection> _connection;

        public SqlTable(Func<IDbConnection> connection)
        {
            _connection = connection;
        }

        private string GetParameterValue(PropertyInfo property, object obj)
        {
            if (property.GetValue(obj) is SqlString s)
            {
                return s;
            }
            else
            {
                return $"@{property.Name}";
            }
        }

        private IEnumerable<string> GetParameterValues(object obj)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                yield return GetParameterValue(property, obj);
            }
        }
    }
}
