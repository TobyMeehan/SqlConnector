using System;
using System.Collections.Generic;
using System.Text;

namespace TobyMeehan.Sql
{
    public interface IDbNameResolver
    {
        string ResolveTable(Type type);

        string ResolveColumn(string propertyName);
    }
}
