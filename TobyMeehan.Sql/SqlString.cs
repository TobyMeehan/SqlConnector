using System;
using System.Collections.Generic;
using System.Text;

namespace TobyMeehan.Sql
{
    /// <summary>
    /// String that is not parameterised when generating SQL queries.
    /// </summary>
    public class SqlString
    {
        public SqlString(string value)
        {
            _value = value;
        }

        private string _value;

        public static implicit operator string(SqlString s) => s._value;
        public static implicit operator SqlString(string s) => new SqlString(s);

        public override string ToString() => _value;
    }
}
