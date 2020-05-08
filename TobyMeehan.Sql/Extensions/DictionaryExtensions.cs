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
            return (ExpandoObject)dictionary;
        }
    }
}
