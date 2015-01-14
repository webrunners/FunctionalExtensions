using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtensions.Linq
{
    public static class OptionLinqExtensions
    {
        public static Option<TResult> SelectMany<TSource, TValue, TResult>(this Option<TSource> source, Func<TSource, Option<TValue>> valueSelector, Func<TSource, TValue, TResult> resultSelector)
        {
            return source.Bind(s => valueSelector(s).Select(v => resultSelector(s, v)));
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
    }
}