using System;
using System.Collections.Generic;
using System.Text;

namespace TobyMeehan.Sql
{
    /// <summary>
    /// Interface for classes which resolve database names.
    /// </summary>
    public interface IDbNameResolver
    {
        /// <summary>
        /// Converts the provided type to a table name.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string Resolve(Type type);
    }
}
