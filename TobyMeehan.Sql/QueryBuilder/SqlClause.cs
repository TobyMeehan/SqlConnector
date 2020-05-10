using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace TobyMeehan.Sql.QueryBuilder
{
    class SqlClause
    {
        public SqlClause() { }

        public SqlClause(string sql)
        {
            Sql = sql;
        }

        public SqlClause(string sql, Dictionary<string, object> parameters)
        {
            Sql = sql;
            Parameters = parameters;
        }

        public string Sql { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        public static SqlClause FromParameter(int count, object value)
        {
            return new SqlClause($"@{count}", new Dictionary<string, object> { { count.ToString(), value } });
        }

        public static SqlClause FromCollection(ref int countStart, IEnumerable<object> values)
        {
            var parameters = new Dictionary<string, object>();
            var sql = new StringBuilder("(");

            foreach (var value in values)
            {
                if (value is SqlString s)
                {
                    sql.Append($"{s},");
                }
                else
                {
                    parameters.Add(countStart.ToString(), value);
                    sql.Append($"@{countStart},");
                    countStart++;
                }
            }

            if (sql.Length == 1)
            {
                sql.Append("null,");
            }

            sql[sql.Length - 1] = ')';

            return new SqlClause(sql.ToString(), parameters);
        }

        public static SqlClause Join(string separator, params SqlClause[] clauses)
        {
            return Join(separator, clauses.AsEnumerable());
        }

        public static SqlClause Join(string separator, IEnumerable<SqlClause> clauses)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            foreach (var clause in clauses)
            {
                parameters = parameters.Union(clause.Parameters).ToDictionary(x => x.Key, x => x.Value);
            }

            string sql = string.Join(separator, clauses.Select(x => x.Sql));

            return new SqlClause(sql, parameters);
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
