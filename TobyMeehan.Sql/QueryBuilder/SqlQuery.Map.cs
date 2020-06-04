using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TobyMeehan.Sql.QueryBuilder
{
    public partial class SqlQuery<T> : ISqlQuery<T>
    {
        public ISqlQuery<T> Map<T1>(Func<T, T1, T> map)
        {
            Func<object[], T> func = x => map((T)x[0], (T1)x[1]);

            return Clone(new QueryMap<T>(func, typeof(T), typeof(T1)));
        }

        public ISqlQuery<T> Map<T1, T2>(Func<T, T1, T2, T> map)
        {
            Func<object[], T> func = x => map((T)x[0], (T1)x[1], (T2)x[2]);

            return Clone(new QueryMap<T>(func, typeof(T), typeof(T1), typeof(T2)));
        }

        public ISqlQuery<T> Map<T1, T2, T3>(Func<T, T1, T2, T3, T> map)
        {
            Func<object[], T> func = x => map((T)x[0], (T1)x[1], (T2)x[2], (T3)x[3]);

            return Clone(new QueryMap<T>(func, typeof(T), typeof(T1), typeof(T2), typeof(T3)));
        }

        public ISqlQuery<T> Map<T1, T2, T3, T4>(Func<T, T1, T2, T3, T4, T> map)
        {
            Func<object[], T> func = x => map((T)x[0], (T1)x[1], (T2)x[2], (T3)x[3], (T4)x[4]);

            return Clone(new QueryMap<T>(func, typeof(T), typeof(T1), typeof(T2), typeof(T3), typeof(T4)));
        }

        public ISqlQuery<T> Map<T1, T2, T3, T4, T5>(Func<T, T1, T2, T3, T4, T5, T> map)
        {
            Func<object[], T> func = x => map((T)x[0], (T1)x[1], (T2)x[2], (T3)x[3], (T4)x[4], (T5)x[5]);

            return Clone(new QueryMap<T>(func, typeof(T), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)));
        }

        public ISqlQuery<T> Map<T1, T2, T3, T4, T5, T6>(Func<T, T1, T2, T3, T4, T5, T6, T> map)
        {
            Func<object[], T> func = x => map((T)x[0], (T1)x[1], (T2)x[2], (T3)x[3], (T4)x[4], (T5)x[5], (T6)x[6]);

            return Clone(new QueryMap<T>(func, typeof(T), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)));
        }
    }
}
