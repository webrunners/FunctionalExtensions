using System;

namespace FunctionalExtensions
{
    public static class ChoiceExtensions
    {
        public static Choice<TResult, T2> Bind<T1, T2, TResult>(this Choice<T1, T2> source, Func<T1, Choice<TResult, T2>> selector)
        {
            T1 value1;
            if (source.MatchChoice1Of2(out value1))
            {
                return selector(value1);
            }

            T2 value2;
            if (source.MatchChoice2Of2(out value2))
            {
                return Choice.NewChoice2Of2<TResult, T2>(value2);
            }

            throw new InvalidOperationException("source (:Choice) has no value.");
        }

        public static Choice<TResult, T2> Select<T1, T2, TResult>(this Choice<T1, T2> source, Func<T1, TResult> selector)
        {
            T1 value1;
            if (source.MatchChoice1Of2(out value1))
            {
                return Choice.NewChoice1Of2<TResult, T2>(selector(value1));
            }

            T2 value2;
            if (source.MatchChoice2Of2(out value2))
            {
                return Choice.NewChoice2Of2<TResult, T2>(value2);
            }

            throw new InvalidOperationException("source (:Choice) has no value.");
        }

        public static Choice<TResult, T2> SelectMany<TSource, TValue, T2, TResult>(this Choice<TSource, T2> source, Func<TSource, Choice<TValue, T2>> valueSelector, Func<TSource, TValue, TResult> resultSelector)
        {
            return source.Bind(s => valueSelector(s).Select(v => resultSelector(s, v)));
        }

        public static Choice<T, T2> ToChoice<T, T2>(this Option<T> source, T2 value)
        {
            if (source.Tag != OptionType.Some) return Choice.NewChoice2Of2<T, T2>(value);

            T some;
            source.MatchSome(out some);
            return Choice.NewChoice1Of2<T, T2>(some);
        }
    }
}