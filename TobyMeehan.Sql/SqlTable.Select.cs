﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TobyMeehan.Sql.Extensions;

namespace TobyMeehan.Sql
{
    public partial class SqlTable<T> : ISqlTable<T>
    {
        private string GetSelectQuery() => GetSelectQuery("*");

        private string GetSelectQuery(params string[] columns)
        {
            string target = string.Join(",", columns);

            return $"SELECT {target} FROM {TableName}";
        }

        public virtual IEnumerable<T> Select()
        {
            return _connection.Query<T>(GetSelectQuery());
        }

        public virtual Task<IEnumerable<T>> SelectAsync()
        {
            return _connection.QueryAsync<T>(GetSelectQuery());
        }

        public virtual IEnumerable<T> Select(params string[] columns)
        {
            return _connection.Query<T>(GetSelectQuery(columns));
        }

        public virtual Task<IEnumerable<T>> SelectAsync(params string[] columns)
        {
            return _connection.QueryAsync<T>(GetSelectQuery(columns));
        }

        private string SelectBy(Expression<Predicate<T>> expression, out object parameters, params string[] columns)
        {
            var whereClause = _whereBuilder.ToSql(expression);

            parameters = whereClause.Parameters.ToDynamic();

            return $"{GetSelectQuery(columns)} WHERE {whereClause.Sql}";
        }

        public virtual IEnumerable<T> SelectBy(Expression<Predicate<T>> expression)
        {
            return _connection.Query<T>(SelectBy(expression, out object parameters, "*"), parameters);
        }

        public virtual IEnumerable<T> SelectBy(Expression<Predicate<T>> expression, params string[] columns)
        {
            return _connection.Query<T>(SelectBy(expression, out object parameters, columns), parameters);
        }

        public virtual Task<IEnumerable<T>> SelectByAsync(Expression<Predicate<T>> expression)
        {
            return _connection.QueryAsync<T>(SelectBy(expression, out object parameters, "*"), parameters);
        }

        public virtual Task<IEnumerable<T>> SelectByAsync(Expression<Predicate<T>> expression, params string[] columns)
        {
            return _connection.QueryAsync<T>(SelectBy(expression, out object parameters, columns), parameters);
        }
    }
}