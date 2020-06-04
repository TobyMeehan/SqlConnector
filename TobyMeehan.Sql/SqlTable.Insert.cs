using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.QueryBuilder;

namespace TobyMeehan.Sql
{
    public partial class SqlTable<T> : ISqlTable<T>
    {
        private ISqlQuery<T> GetInsertQuery(object values)
        {
            return new SqlQuery<T>()
                .Insert(values);
        }

        public virtual int Insert(object values)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.Execute(GetInsertQuery(values));
            }
        }

        public virtual Task<int> InsertAsync(object values)
        {
            using (IDbConnection connection = _connection.Invoke())
            {
                return connection.ExecuteAsync(GetInsertQuery(values));
            }
        }
    }
}
