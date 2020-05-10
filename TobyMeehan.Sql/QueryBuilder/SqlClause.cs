using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace TobyMeehan.Sql.QueryBuilder
{
    public class SqlClause
    {
        public string Sql { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        public static SqlClause FromSql(string sql)
        {
            return new SqlClause
            {
                Sql = sql
            };
        }

        public static SqlClause FromParameter(int count, object value)
        {
            return new SqlClause
            {
                Parameters = { { count.ToString(), value } },
                Sql = $"@{count}"
            };
        }

        public static SqlClause FromCollection(ref int countStart, IEnumerable<object> values)
        {
            var parameters = new Dictionary<string, object>();
            var sql = new StringBuilder("(");

            foreach (var value in values)
            {
                parameters.Add(countStart.ToString(), value);
                sql.Append($"@{countStart}");
                countStart++;
            }

            if (sql.Length == 1)
            {
                sql.Append("null,");
            }

            sql[sql.Length - 1] = ')';

            return new SqlClause
            {
                Parameters = parameters,
                Sql = sql.ToString()
            };
        }

        public static SqlClause Concat(string @operator, SqlClause operand)
        {
            return new SqlClause
            {
                Parameters = operand.Parameters,
                Sql = $"({@operator} {operand.Sql}"
            };
        }

        public static SqlClause Concat(SqlClause left, string @operator, SqlClause right)
        {
            return new SqlClause
            {
                Parameters = left.Parameters.Union(right.Parameters).ToDictionary(x => x.Key, x => x.Value),
                Sql = $"({left.Sql} {@operator} {right.Sql})"
            };
        }
    }
}
