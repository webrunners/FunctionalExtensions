using System;

namespace FunctionalExtensions.Validation
{
    public static class ResultExtensions
    {
        public static Choice<Tuple<T1First, T1Second>, Failures<TError>> Merge<T1First, T1Second, TError>(this Choice<T1First, Failures<TError>> first, Choice<T1Second, Failures<TError>> second)
        {
            T1First choie1Of21;
            Failures<TError> choice2Of22;

            if (first.MatchChoice1Of2(out choie1Of21))
            {
                T1Second choice1Of22;
                if (second.MatchChoice1Of2(out choice1Of22))
                {
                    return Choice.NewChoice1Of2<Tuple<T1First, T1Second>, Failures<TError>>(Tuple.Create(choie1Of21, choice1Of22));
                }

                if (second.MatchChoice2Of2(out choice2Of22))
                {
                    return Choice.NewChoice2Of2<Tuple<T1First, T1Second>, Failures<TError>>(choice2Of22);
                }

                throw new InvalidOperationException("second (:Choice) has no value.");
            }

            Failures<TError> choice2Of21;
            if (!first.MatchChoice2Of2(out choice2Of21))
                throw new InvalidOperationException("first (:Choice) has no value.");

            return Choice.NewChoice2Of2<Tuple<T1First, T1Second>, Failures<TError>>(second.MatchChoice2Of2(out choice2Of22) 
                ? Failures<TError>.Merge(choice2Of21, choice2Of22) 
                : choice2Of21);
        }

        public static Choice<TResult, Failures<TError>> Join<TOuter, TInner, TKey, TResult, TError>(
            this Choice<TOuter, Failures<TError>> outer,
            Choice<TInner, Failures<TError>> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            return outer.Merge(inner).Select(tup => resultSelector(tup.Item1, tup.Item2));
        }
    }
}