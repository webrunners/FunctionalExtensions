using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtensions
{
    public static class OptionExtensions
    {
        public static Option<TResult> Bind<T, TResult>(this Option<T> source, Func<T, Option<TResult>> selector)
        {
            T value;
            return source.MatchSome(out value) ? selector(value) : Option.None<TResult>();
        }

        public static Option<TResult> Select<T, TResult>(this Option<T> source, Func<T, TResult> selector)
        {
            T value;
            return source.MatchSome(out value) ? Option.Some(selector(value)) : Option.None<TResult>();
        }

        public static Option<TResult> SelectMany<TSource, TValue, TResult>(this Option<TSource> source, Func<TSource, Option<TValue>> valueSelector, Func<TSource, TValue, TResult> resultSelector)
        {
            return source.Bind(s => valueSelector(s).Select(v => resultSelector(s, v)));
        }

        public static Option<T> ToOption<T>(this T value)
        {
            return value != null ? Option.Some(value) : Option.None<T>();
        }

        public static Option<T> FirstOrOption<T>(this ICollection<T> source, Predicate<T> predicate = null)
        {
            return predicate != null
                ? source.Any(x => predicate(x)) ? source.First(x => predicate(x)).ToOption() : Option.None<T>()
                : source.Any() ? source.First().ToOption() : Option.None<T>();
        }

        public static Option<T> SingleOrOption<T>(this ICollection<T> source, Predicate<T> predicate = null)
        {
            return predicate != null
                ? source.Count(x => predicate(x)) == 1 ? source.Single(x => predicate(x)).ToOption() : Option.None<T>()
                : source.Count() == 1 ? source.Single().ToOption() : Option.None<T>();
        }

        public static Option<TResult> Apply<T, TResult>(this Option<Func<T, TResult>> func, Option<T> opt)
        {
            return opt.Bind(o => func.Select(s => s(o)));
        }

        public static Option<TResult> Apply<T, TResult>(this Option<Func<T, Option<TResult>>> func, Option<T> opt)
        {
            return opt.Bind(o => func.Bind(s => s(o)));
        }
    }

    public static class FunctionExtensions
    {
        public static Option<Func<TResult1, TResult2>> Select<T, TResult1, TResult2>(this Func<T, Func<TResult1, TResult2>> func, Option<T> opt)
        {
            return opt.Select(func);
        }

        public static Option<TResult> Select<T, TResult>(this Func<T, TResult> func, Option<T> opt)
        {
            return opt.Select(func);
        }

        public static Option<TResult> Bind<T, TResult>(this Func<T, Option<TResult>> func, Option<T> opt)
        {
            return opt.Bind(func);
        }

        public static Option<Func<TResult1, Option<TResult2>>> Bind<T, TResult1, TResult2>(this Func<T, Func<TResult1, Option<TResult2>>> func, Option<T> opt)
        {
            return opt.Select(func);
        }
    }

    public static class FunctionResultTransformExtensions
    {
        public static Func<T, Option<TResult>> ReturnOption<T, TResult>(this Func<T, TResult> func)
        {
            return x => func(x).ToOption();
        }

        public static Func<T1, T2, Option<TResult>> ReturnOption<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            return (x, y) => func(x, y).ToOption();
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
            return (x, y) =>
            {
                try
                {
                    return func(x, y);
                }
                catch (Exception)
                {
                    return Option.None<TResult>();
                }
            };
        }
    }
}