using System;
using System.Collections.Generic;
using System.Text;

namespace TobyMeehan.Sql.QueryBuilder
{
    public class QueryMap<T>
    {
        public QueryMap(Func<object[], T> map, params Type[] types)
        {
            Types = types;
            Map = map;
        }

        public Type[] Types { get; set; }

        public Func<object[], T> Map { get; set; }
    }
}
