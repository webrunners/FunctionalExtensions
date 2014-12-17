using System;

namespace FunctionalExtensions
{
    public enum OptionType
    {
        Some, None
    }

    public abstract class Option<T>
    {
        private readonly OptionType _tag;

        protected Option(OptionType tag)
        {
            _tag = tag;
        }

        public OptionType Tag { get { return _tag; } }

        internal bool MatchNone()
        {
            return Tag == OptionType.None;
        }

        internal bool MatchSome(out T value)
        {
            value = Tag == OptionType.Some ? ((Some<T>)this).Value : default(T);
            return Tag == OptionType.Some;
        }

        public void Match(Action<T> onSome, Action onNone)
        {
            if (Tag == OptionType.Some)
                onSome(((Some<T>)this).Value);
            else
                onNone();
        }

        public Choice<T, T2> ToChoice<T2>(T2 value)
        {
            if (Tag == OptionType.Some)
            {
                T some;
                MatchSome(out some);
                return Choice.NewChoice1Of2<T, T2>(some);
            }
            else
                return Choice.NewChoice2Of2<T, T2>(value);
        }
    }

    internal class None<T> : Option<T>
    {
        public None() : base(OptionType.None) { }
    }

    internal class Some<T> : Option<T>
    {
        public Some(T value)
            : base(OptionType.Some)
        {
            _value = value;
        }
        private readonly T _value;
        public T Value { get { return _value; } }
    }

    public static class Option
    {
        public static Option<T> None<T>()
        {
            return new None<T>();
        }
        public static Option<T> Some<T>(T value)
        {
            return new Some<T>(value);
        }
    }

    public static class OptionExtensions
    {
        public static Option<TResult> Map<T, TResult>(this Option<T> source, Func<T, TResult> selector)
        {
            T value;
            return source.MatchSome(out value) ? Option.Some(selector(value)) : Option.None<TResult>();
        }

        public static Option<TResult> Bind<T, TResult>(this Option<T> source, Func<T, Option<TResult>> selector)
        {
            T value;
            return source.MatchSome(out value) ? selector(value) : Option.None<TResult>();
        }

        public static Option<TResult> Select<T, TResult>(this Option<T> source, Func<T, TResult> selector)
        {
            return source.Map(selector);
        }

        public static Option<TResult> SelectMany<TSource, TValue, TResult>(this Option<TSource> source, Func<TSource, Option<TValue>> valueSelector, Func<TSource, TValue, TResult> resultSelector)
        {
            return source.Bind(s => valueSelector(s).Map(v => resultSelector(s, v)));
        }
    }
}
