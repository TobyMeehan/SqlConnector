using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TobyMeehan.Sql.Extensions;

namespace TobyMeehan.Sql.QueryBuilder
{
    /// <summary>
    /// Class representing a complete SQL query.
    /// </summary>
    public class SqlQuery<T> : SqlQueryBase<T, SqlQuery<T>>
    {
        protected override SqlQuery<T> Clone(SqlQuery<T> source)
        {
            return new SqlQuery<T>();
        }
    }
}
