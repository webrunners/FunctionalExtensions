using System;

namespace FunctionalExtensions
{
    public static class CompositionExtensions
    {
        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T2, T3> f, Func<T1, T2> g)
        {
            return x => f(g(x));
        }

        public static Func<T1, T2, T4> Compose<T1, T2, T3, T4>(this Func<T3, T4> f, Func<T1, T2, T3> g)
        {
            throw new NotImplementedException();
            //return (x1, x2) => f(g(x));
        }

        public static Func<T1, T3, T4> Compose<T1, T2, T3, T4>(this Func<T2, T3, T4> f, Func<T1, T2> g)
        {
            return (x1, x2) => f(g(x1), x2);
        }

        public static Func<T1, T3, T4, T5> Compose<T1, T2, T3, T4, T5>(this Func<T2, T3, T4, T5> f, Func<T1, T2> g)
        {
            return (x1, x2, x3) => f(g(x1), x2, x3);
        }
    }
}
