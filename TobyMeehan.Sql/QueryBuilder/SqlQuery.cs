using Dapper;
using System.Collections.Generic;
using System.Linq;
using TobyMeehan.Sql.Extensions;

namespace TobyMeehan.Sql.QueryBuilder
{
    public partial class SqlQuery<T> : ISqlQuery<T>
    {
        private ISqlQuery<T> Clone(SqlClause addition, int? index = null)
        {
            SqlQuery<T> clone = new SqlQuery<T>
            {
                _clauses = _clauses,
                _parameters = _parameters,
                QueryMap = QueryMap
            };

            if (index == null)
            {
                clone._clauses.Add(addition);
            }
            else
            {
                clone._clauses.Insert(index.Value, addition);
            }

            clone._parameters = clone._parameters.Union(addition.Parameters).ToDictionary(x => x.Key, x => x.Value);

            clone._parameterCount = clone._parameters.Count + 1;

            return clone;
        }

        private ISqlQuery<T> Clone(QueryMap<T> map)
        {
            return new SqlQuery<T>
            {
                _clauses = _clauses,
                _parameters = _parameters,
                QueryMap = map
            };
        }

        private string _tableName => typeof(T).GetSqlName() ?? nameof(T);
        private List<SqlClause> _clauses = new List<SqlClause>();
        private int _parameterCount = 1;
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public QueryMap<T> QueryMap { get; set; }

        public string ToSql()
        {
            return string.Join(" ", _clauses.Select(x => x.Sql));
        }

        public string ToSql(out IDictionary<string, object> parameters)
        {
            parameters = _parameters;

            return ToSql();
        }

        public string ToSql(out object parameters)
        {
            parameters = new DynamicParameters(_parameters);

            return ToSql();
        }
    }
}
