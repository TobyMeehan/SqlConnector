using System;
using System.Collections.Generic;
using System.Text;

namespace TobyMeehan.Sql.Extensions
{
    static class ObjectExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (var property in obj.GetType().GetProperties())
            {
                dict.Add(property.Name, property.GetValue(obj));
            }

            return dict;
        }
    }
}
