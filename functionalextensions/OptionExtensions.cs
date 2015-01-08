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
    }
}