﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TobyMeehan.Sql.Extensions;

namespace TobyMeehan.Sql.QueryBuilder
{
    /// <summary>
    /// Class representing a complete SQL query.
    /// </summary>
    public class SqlQuery
    {
        private readonly string _tableName;

        public SqlQuery(string tableName)
        {
            _tableName = tableName;
        }

        private SqlQuery(SqlQuery sqlQuery, SqlClause addition, int? index = null)
        {
            _tableName = sqlQuery._tableName;
            _clauses = sqlQuery._clauses;
            _parameters = sqlQuery._parameters;

            if (index == null)
            {
                _clauses.Add(addition);
            }
            else
            {
                _clauses.Insert(index.Value, addition);
            }

            foreach (var param in addition.Parameters)
            {
                _parameters.Add(param.Key, param.Value);
            }
        }

        private List<SqlClause> _clauses = new List<SqlClause>();
        private int _parameterCount = 1;
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public SqlQuery Where<T>(Expression<Predicate<T>> expression)
        {
            return new SqlQuery(this, new WhereSqlClause<T>(expression, ref _parameterCount));
        }

        public SqlQuery Select() => Select("*");
        public SqlQuery Select(params string[] columns)
        {
            return new SqlQuery(this, SqlClause.FromSql($"SELECT {string.Join(", ", columns)} FROM {_tableName}"), 0);
        }

        public string AsSql()
        {
            return string.Join(" ", _clauses.Select(x => x.Sql));
        }

        public string AsSql(out Dictionary<string, object> parameters)
        {
            parameters = _parameters;

            return AsSql();
        }

        public string AsSql(out object parameters)
        {
            parameters = _parameters.ToDynamic();

            return AsSql();
        }
    }
}