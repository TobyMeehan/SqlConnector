using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace TobyMeehan.Sql.Extensions
{
    static class DictionaryExtensions
    {
        public static dynamic ToDynamic(this IDictionary<string, object> dictionary)
        {
            ICollection<KeyValuePair<string, object>> obj = new ExpandoObject();

            foreach (var item in dictionary)
            {
                obj.Add(item);
            }

            return obj;
        }
    }
}
