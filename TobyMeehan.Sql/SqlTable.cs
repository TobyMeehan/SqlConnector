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
        private readonly QueryFactory _queryFactory;

        public SqlTable(QueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
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
