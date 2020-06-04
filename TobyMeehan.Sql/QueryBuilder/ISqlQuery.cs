using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TobyMeehan.Sql.QueryBuilder
{
    public interface ISqlQuery
    {
        /// <summary>
        /// Gets the string SQL query.
        /// </summary>
        /// <returns></returns>
        string ToSql();

        /// <summary>
        /// Gets the string SQL query and outputs the generated parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string ToSql(out IDictionary<string, object> parameters);

        /// <summary>
        /// Gets the string SQL query and outputs the generated parameters as a Dapper DynamicParameter object.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string ToSql(out object parameters);
    }

    public interface ISqlQuery<T> : ISqlQuery
    {
        /// <summary>
        /// Executes the query only on records matching the provided expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISqlQuery<T> Where(Expression<Predicate<T>> expression);

        /// <summary>
        /// Executes the query only on records matching the provided expression, using columns from a foreign table.
        /// </summary>
        /// <typeparam name="TForeign"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISqlQuery<T> Where<TForeign>(Expression<Func<T, TForeign, bool>> expression);

        /// <summary>
        /// Selects records with matching values in both tables.
        /// </summary>
        /// <typeparam name="TJoin">Table to join with.</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISqlQuery<T> InnerJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression);

        /// <summary>
        /// Selects records with matching values in both tables.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISqlQuery<T> InnerJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression);

        /// <summary>
        /// Selects any matched records from the right table.
        /// </summary>
        /// <typeparam name="TJoin">Table to join with.</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISqlQuery<T> LeftJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression);

        /// <summary>
        /// Selects any matched records from the right table.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISqlQuery<T> LeftJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression);

        /// <summary>
        /// Selects any matched records from the left table.
        /// </summary>
        /// <typeparam name="TJoin">Table to join with.</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISqlQuery<T> RightJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression);

        /// <summary>
        /// Selects any matched records from the left table.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISqlQuery<T> RightJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression);

        /// <summary>
        /// Selects all records with a match in either table.
        /// </summary>
        /// <typeparam name="TJoin">Table to join with.</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISqlQuery<T> FullJoin<TJoin>(Expression<Func<T, TJoin, bool>> expression);

        /// <summary>
        /// Selects all records with a match in either table.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ISqlQuery<T> FullJoin<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression);


        /// <summary>
        /// Selects all columns from the table.
        /// </summary>
        /// <returns></returns>
        ISqlQuery<T> Select();

        /// <summary>
        /// Selects the provided columns from the table.
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        ISqlQuery<T> Select(params string[] columns);

        /// <summary>
        /// Inserts a new record into the table.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        ISqlQuery<T> Insert(object values);

        /// <summary>
        /// Updates records in the table.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        ISqlQuery<T> Update(object values);

        /// <summary>
        /// Deletes records from the table.
        /// </summary>
        /// <returns></returns>
        ISqlQuery<T> Delete();


        /// <summary>
        /// Specifies the mapping for the query with 2 input types.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        ISqlQuery<T> Map<T1>(Func<T, T1, T> map);

        /// <summary>
        /// Specifies the mapping for the query with 3 input types.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        ISqlQuery<T> Map<T1, T2>(Func<T, T1, T2, T> map);

        /// <summary>
        /// Specifies the mapping for the query with 4 input types.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        ISqlQuery<T> Map<T1, T2, T3>(Func<T, T1, T2, T3, T> map);

        /// <summary>
        /// Specifies the mapping for the query with 5 input types.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        ISqlQuery<T> Map<T1, T2, T3, T4>(Func<T, T1, T2, T3, T4, T> map);

        /// <summary>
        /// Specifies the mapping for the query with 6 input types.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        ISqlQuery<T> Map<T1, T2, T3, T4, T5>(Func<T, T1, T2, T3, T4, T5, T> map);

        /// <summary>
        /// Specifies the mapping for the query with 7 input types.
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        ISqlQuery<T> Map<T1, T2, T3, T4, T5, T6>(Func<T, T1, T2, T3, T4, T5, T6, T> map);

        QueryMap<T> QueryMap { get; set; }
    }
}
