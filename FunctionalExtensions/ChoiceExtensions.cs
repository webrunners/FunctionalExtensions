using System;

namespace FunctionalExtensions
{
    public static class ChoiceExtensions
    {
        public static Choice<TResult, T2> Bind<T1, T2, TResult>(this Choice<T1, T2> source, Func<T1, Choice<TResult, T2>> selector)
        {
            return source.Match(
                onChoice1Of2: selector,
                onChoice2Of2: Choice.NewChoice2Of2<TResult, T2>
            );
        }

        public static Choice<TResult, T2> Select<T1, T2, TResult>(this Choice<T1, T2> source, Func<T1, TResult> selector)
        {
            return source.Match(
                onChoice1Of2: value => Choice.NewChoice1Of2<TResult, T2>(selector(value)),
                onChoice2Of2: value => Choice.NewChoice2Of2<TResult, T2>(value)
            );
        }

        public static Choice<TResult, T2> SelectMany<TSource, TValue, T2, TResult>(this Choice<TSource, T2> source, Func<TSource, Choice<TValue, T2>> valueSelector, Func<TSource, TValue, TResult> resultSelector)
        {
            return source.Bind(s => valueSelector(s).Select(v => resultSelector(s, v)));
        }

        public static Choice<T, T2> ToChoice<T, T2>(this Option<T> source, T2 value)
        {
            return source.Match(
                onSome: Choice.NewChoice1Of2<T, T2>,
                onNone: () => Choice.NewChoice2Of2<T, T2>(value)
            );
        }
    }
}