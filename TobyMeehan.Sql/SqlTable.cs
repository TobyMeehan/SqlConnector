using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TobyMeehan.Sql.QueryBuilder;

namespace TobyMeehan.Sql
{
    public partial class SqlTable<T> : ISqlTable<T>
    {
        private readonly IDbConnection _connection;
        private readonly IDbNameResolver _nameResolver;
        private readonly IWhereBuilder _whereBuilder;

        public SqlTable(IDbConnection connection, IDbNameResolver nameResolver, IWhereBuilder whereBuilder)
        {
            _connection = connection;
            _nameResolver = nameResolver;
            _whereBuilder = whereBuilder;
        }

        private string TableName => _nameResolver.ResolveTable(typeof(T));
    }
}
