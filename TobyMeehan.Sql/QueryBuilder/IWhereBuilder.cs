using System;
using System.Linq.Expressions;

namespace TobyMeehan.Sql.QueryBuilder
{
    public interface IWhereBuilder
    {
        SqlClause ToSql<T>(Expression<Predicate<T>> expression);
    }
}