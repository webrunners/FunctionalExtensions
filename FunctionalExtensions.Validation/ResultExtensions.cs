using System;

namespace FunctionalExtensions.Validation
{
    public static class ResultExtensions
    {
        public static Choice<Tuple<T1First, T1Second>, Failure<TError>> Merge<T1First, T1Second, TError>(this Choice<T1First, Failure<TError>> first, Choice<T1Second, Failure<TError>> second)
        {
            return first.Match(
                onChoice1Of2: first1 =>
                    second.Match(
                        onChoice1Of2: second1 => Choice.NewChoice1Of2<Tuple<T1First, T1Second>, Failure<TError>>(Tuple.Create(first1, second1)),
                        onChoice2Of2: second2 => Choice.NewChoice2Of2<Tuple<T1First, T1Second>, Failure<TError>>(second2)
                    ),
                onChoice2Of2: first2 =>
                    second.Match(
                        onChoice1Of2: second1 => Choice.NewChoice2Of2<Tuple<T1First, T1Second>, Failure<TError>>(first2),
                        onChoice2Of2: second2 => Choice.NewChoice2Of2<Tuple<T1First, T1Second>, Failure<TError>>(Failure<TError>.Merge(first2, second2))
                    )
            );
        }

        public static Choice<TResult, Failure<TError>> Join<TOuter, TInner, TKey, TResult, TError>(
            this Choice<TOuter, Failure<TError>> outer,
            Choice<TInner, Failure<TError>> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            return outer.Merge(inner).Select(tup => resultSelector(tup.Item1, tup.Item2));
        }
    }
}