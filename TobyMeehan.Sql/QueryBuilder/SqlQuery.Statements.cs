using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TobyMeehan.Sql.Extensions;

namespace TobyMeehan.Sql.QueryBuilder
{
    public partial class SqlQuery<T> : ISqlQuery<T>
    {
        public ISqlQuery<T> Select() => Select("*");

        public ISqlQuery<T> Select(params string[] columns)
        {
            return Clone(new SqlClause($"SELECT {string.Join(", ", columns)} FROM {_tableName}"), 0);
        }

        public ISqlQuery<T> Insert(object values)
        {
            var properties = values.GetType().GetProperties().Select(x => x.Name);

            int i = _parameterCount;

            SqlClause sql = SqlClause.Join("",
                new SqlClause($"INSERT INTO {_tableName} ("),
                SqlClause.Join(", ", properties.Select(x => new SqlClause(x))),
                new SqlClause(") VALUES "),
                SqlClause.FromCollection(ref i, values.ToDictionary().Select(x => x.Value)));

            return Clone(sql, 0);
        }

        public ISqlQuery<T> Update(object values)
        {
            var properties = values.GetType().GetProperties();

            int i = _parameterCount;

            SqlClause updateClause = new SqlClause($"UPDATE {_tableName} SET");

            List<SqlClause> clauses = new List<SqlClause>();

            foreach (var item in properties)
            {
                clauses.Add(SqlClause.Join(" = ", new SqlClause(item.Name), SqlClause.FromParameter(i, item.GetValue(values))));
                i++;
            }

            return Clone(SqlClause.Join(" ", updateClause, SqlClause.Join(", ", clauses)), 0);
        }

        public ISqlQuery<T> Delete()
        {
            return Clone(new SqlClause($"DELETE FROM {_tableName}"), 0);
        }
    }
}
