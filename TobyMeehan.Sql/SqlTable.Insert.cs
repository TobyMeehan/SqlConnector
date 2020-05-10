using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.QueryBuilder;

namespace TobyMeehan.Sql
{
    public partial class SqlTable<T> : ISqlTable<T>
    {
        private string GetInsertQuery(object values)
        {
            return new SqlQuery(TableName)
                .Insert(values)
                .AsSql();
        }

        public virtual int Insert(object values)
        {
            return _connection.Execute(GetInsertQuery(values), values);
        }

        public virtual Task<int> InsertAsync(object values)
        {
            return _connection.ExecuteAsync(GetInsertQuery(values), values);
        }
    }
}
