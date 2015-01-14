using System;
using FunctionalExtensions.Currying;

namespace FunctionalExtensions.Transform
{
    public static class FunctionResultTransformExtensions
    {
        public static Func<T, Option<TResult>> ReturnOption<T, TResult>(this Func<T, TResult> func)
        {
            return x => Option.Return(func(x));
        }

        public static Func<T1, T2, Option<TResult>> ReturnOption<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            return (x, y) => Option.Return(func(x, y));
        }

        public static Func<T, Option<TResult>> OnExceptionNone<T, TResult>(this Func<T, Option<TResult>> func)
        {
            return x =>
            {
                try
                {
                    return func(x);
                }
                catch (Exception)
                {
                    return Option.None<TResult>();
                }
            };
        }

        public static Func<T1, T2, Option<TResult>> OnExceptionNone<T1, T2, TResult>(this Func<T1, T2, Option<TResult>> func)
        {
            return (x, y) => func.Curry()(x).OnExceptionNone()(y);
        }

        public static Func<T1, T2, T3, Option<TResult>> OnExceptionNone<T1, T2, T3, TResult>(this Func<T1, T2, T3, Option<TResult>> func)
        {
            return (x1, x2, x3) => func.Curry()(x1)(x2).OnExceptionNone()(x3);
        }
    }
}
