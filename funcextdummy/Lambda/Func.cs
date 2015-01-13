using System;

namespace FunctionalExtensions.Lambda
{
    public static class Fun
    {
        public static Func<T> Create<T>(Func<T> del)
        {
            return del;
        }

        public static Func<T, TResult> Create<T, TResult>(Func<T, TResult> del)
        {
            return del;
        }

        public static Func<T1, T2, TResult> Create<T1, T2, TResult>(Func<T1, T2, TResult> del)
        {
            return del;
        }

        public static Func<T1, T2, T3, TResult> Create<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> del)
        {
            return del;
        }
    }
}
