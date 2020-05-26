using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TobyMeehan.Sql.QueryBuilder;

namespace TobyMeehan.Sql.Extensions
{
    public static class ReflectionExtensions
    {
        public static string GetSqlName(this MemberInfo memberInfo)
        {
            var attribute = Attribute.GetCustomAttribute(memberInfo, typeof(SqlNameAttribute)) as SqlNameAttribute;

            return attribute?.Name;
        }
    }
}
