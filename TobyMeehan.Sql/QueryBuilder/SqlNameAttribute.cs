using System;
using System.Collections.Generic;
using System.Text;

namespace TobyMeehan.Sql.QueryBuilder
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class, Inherited = false)]
    public class SqlNameAttribute : Attribute
    {
        public SqlNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
