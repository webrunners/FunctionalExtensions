using System;

namespace FunctionalExtensions.Pipeline
{
    public static class ObjectExtensions
    {
        public static TResult Pipeline<T, TResult>(this T source, Func<T, TResult> f)
        {
            return f(source);
        }
    }
}
