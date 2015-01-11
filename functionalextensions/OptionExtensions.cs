using System;
using System.Collections;
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

        public static Option<TResult> Apply<T, TResult>(this Option<T> source,
            Option<Func<T, TResult>> other)
        {
            return source.Bind(s => other.Select(o => o(s)));
        }

        public static Option<TResult> Apply<T, TResult>(this Option<Func<T, TResult>> source,
            Option<T> other)
        {
            return other.Bind(o => source.Select(s => s(o)));
        }

        public static Option<Func<TResult1, TResult2>> Select<T, TResult1, TResult2>(this Func<T, Func<TResult1, TResult2>> f, Option<T> optValue)
        {
            return optValue.Select(f);
        }

        public static Option<TResult> Select<T, TResult>(this Func<T, TResult> f, Option<T> optValue)
        {
            return optValue.Select(f);
        }

        public static Option<Func<T, TResult>> Select<T, TResult>(this Func<T, TResult> f)
        {
            return Option.Some(f);
        }
    }
}