using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtensions.Validation
{
    public static class Result
    {
        public static Choice<T, Errors> Ok<T>(T value)
        {
            return Choice.NewChoice1Of2<T, Errors>(value);
        }

        public static Choice<T, Errors> Error<T>(string message)
        {
            return Choice.NewChoice2Of2<T, Errors>(new Errors(message));
        }
    }

    public class Errors
    {
        private readonly List<string> _errors = new List<string>();

        public IReadOnlyCollection<string> Messages
        {
            get { return _errors; }
        } 

        public Errors(string message)
        {
            _errors.Add(message);
        }

        private Errors(IEnumerable<string> messages)
        {
            _errors.AddRange(messages);
        }

        public static Errors Create(IEnumerable<string> messages)
        {
            return new Errors(messages);
        }

        public void AddError(string message)
        {
            _errors.Add(message);
        }

        public bool HasErrors()
        {
            return _errors.Any();
        }

        public static Errors Merge(Errors errors1, Errors errors2)
        {
            return Create(errors1.Messages.Concat(errors2.Messages));
        }
    }

    public static class ResultExtensions
    {
        public static Choice<Tuple<T1First, T1Second>, Errors> Merge<T1First, T1Second>(this Choice<T1First, Errors> first, Choice<T1Second, Errors> second)
        {
            T1First choie1Of21;
            Errors choice2Of22;

            if (first.MatchChoice1Of2(out choie1Of21))
            {
                T1Second choice1Of22;
                if (second.MatchChoice1Of2(out choice1Of22))
                {
                    return Choice.NewChoice1Of2<Tuple<T1First, T1Second>, Errors>(Tuple.Create(choie1Of21, choice1Of22));
                }

                if (second.MatchChoice2Of2(out choice2Of22))
                {
                    return Choice.NewChoice2Of2<Tuple<T1First, T1Second>, Errors>(choice2Of22);
                }

                throw new InvalidOperationException("second (:Choice) has no value.");
            }

            Errors choice2Of21;
            if (!first.MatchChoice2Of2(out choice2Of21))
                throw new InvalidOperationException("first (:Choice) has no value.");

            return Choice.NewChoice2Of2<Tuple<T1First, T1Second>, Errors>(second.MatchChoice2Of2(out choice2Of22) 
                ? Errors.Merge(choice2Of21, choice2Of22) 
                : choice2Of21);
        }

        public static Choice<TResult, Errors> Join<TOuter, TInner, TKey, TResult>(
            this Choice<TOuter, Errors> outer,
            Choice<TInner, Errors> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            return outer.Merge(inner).Select(tup => resultSelector(tup.Item1, tup.Item2));
        }
    }
}
