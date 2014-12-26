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
    }
}
