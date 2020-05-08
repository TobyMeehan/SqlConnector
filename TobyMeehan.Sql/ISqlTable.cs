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
    }
}
