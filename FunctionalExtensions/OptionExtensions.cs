using System;

namespace FunctionalExtensions
{
    public static class OptionExtensions
    {
        public static Option<TResult> Bind<T, TResult>(this Option<T> source, Func<T, Option<TResult>> selector)
        {
            return source.Match(
                onSome: selector,
                onNone: Option.None<TResult>
            );
        }

        // fmap
        public static Option<TResult> Select<T, TResult>(this Option<T> source, Func<T, TResult> selector)
        {
            return source.Match(
                onSome: value => Option.Some(selector(value)),
                onNone: Option.None<TResult>
            );
        }

        public static Option<TResult> Apply<T, TResult>(this Option<Func<T, TResult>> func, Option<T> opt)
        {
            return opt.Bind(o => func.Select(s => s(o)));
        }

        public static Option<TResult> Apply<T, TResult>(this Option<Func<T, Option<TResult>>> func, Option<T> opt)
        {
            return opt.Bind(o => func.Bind(s => s(o)));
        }

        public static Option<T> ToOption<T>(this T value)
        {
            return new Option<T>(value);
        }

        public static T DefaultIfNone<T>(this Option<T> source, T defaultValue)
        {
            return source.Match(x => x, () => defaultValue);
        }

        public static Option<T> ToOption<T>(this T? value) where T : struct
        {
            return value.HasValue ? Option.Some(value.Value) : Option.None<T>();
        }
    }
}