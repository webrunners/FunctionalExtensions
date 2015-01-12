using System;

namespace FunctionalExtensions
{
    public static class FuncExtensions
    {
        public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            return x1 => x2 => func(x1, x2);
        }

        public static Func<T1, Func<T2, Func<T3, TResult>>> Curry<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        {
            return x1 => x2 => x3 => func(x1, x2, x3);
        }

        public static Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> Curry<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func)
        {
            return x1 => x2 => x3 => x4 => func(x1, x2, x3, x4);
        }

        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TResult>>>>> Curry<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func)
        {
            return x1 => x2 => x3 => x4 => x5 => func(x1, x2, x3, x4, x5);
        }

        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, TResult>>>>>> Curry<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func)
        {
            return x1 => x2 => x3 => x4 => x5 => x6 => func(x1, x2, x3, x4, x5, x6);
        }

        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, TResult>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
        {
            return x1 => x2 => x3 => x4 => x5 => x6 => x7 => func(x1, x2, x3, x4, x5, x6, x7);
        }

        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, TResult>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
        {
            return x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => func(x1, x2, x3, x4, x5, x6, x7, x8);
        }
    }
}
