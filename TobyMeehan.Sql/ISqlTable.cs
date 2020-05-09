using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TobyMeehan.Sql
{
    /// <summary>
    /// Interface representing a generic SQL table.
    /// </summary>
    /// <typeparam name="T">Type representing table records.</typeparam>
    public interface ISqlTable<T>
    {
        /// <summary>
        /// Gets all records from the table.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Select();

        /// <summary>
        /// Gets all records from the table.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Select(params string[] columns);

        /// <summary>
        /// Gets all records from the table that match the provided conditions.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<T> SelectBy(Expression<Predicate<T>> expression);

        /// <summary>
        /// Gets all records from the table that match the provided conditions.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<T> SelectBy(Expression<Predicate<T>> expression, params string[] columns);

        /// <summary>
        /// Gets all records from the table asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> SelectAsync();

        /// <summary>
        /// Gets all records from the table asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> SelectAsync(params string[] columns);

        /// <summary>
        /// Gets all records from the table that match the provided conditions asynchronously.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> SelectByAsync(Expression<Predicate<T>> expression);

        /// <summary>
        /// Gets all records from the table that match the provided conditions asynchronously.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> SelectByAsync(Expression<Predicate<T>> expression, params string[] columns);

        /// <summary>
        /// Inserts the provided values into the table.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int Insert(object value);

        /// <summary>
        /// Inserts the provided values into the table asynchronously.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<int> InsertAsync(object value);

        /// <summary>
        /// Updates the records matching the provided expression to the provided values.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        int Update(Expression<Predicate<T>> expression, object value);

        /// <summary>
        /// Updates the records matching the provided expression to the provided values asynchronously.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Predicate<T>> expression, object value);

        /// <summary>
        /// Removes the records matching the provided expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        int Delete(Expression<Predicate<T>> expression);

        /// <summary>
        /// Removes the records matching the provided expression asynchronously.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Predicate<T>> expression);
    }
}
