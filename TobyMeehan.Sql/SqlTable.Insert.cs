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
        private ExecutableSqlQuery<T> GetInsertQuery(object values)
        {
            return _queryFactory.Executable<T>()
                .Insert(values);
        }

        public virtual int Insert(object values)
        {
            return GetInsertQuery(values).Execute();
        }

        public virtual Task<int> InsertAsync(object values)
        {
            return GetInsertQuery(values).ExecuteAsync();
        }
    }
}
