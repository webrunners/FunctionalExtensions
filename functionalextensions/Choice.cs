using System;

namespace FunctionalExtensions
{
    public enum ChoiceType { Choice1Of2, Choice2Of2 };

    public abstract class Choice<T1, T2>
    {
        private readonly ChoiceType _tag;

        protected Choice(ChoiceType tag)
        {
            _tag = tag;
        }

        public ChoiceType Tag { get { return _tag; } }

        internal bool MatchChoice1Of2(out T1 value)
        {
            value = Tag == ChoiceType.Choice1Of2 ? ((Choice1Of2<T1, T2>)this).Value : default(T1);
            return Tag == ChoiceType.Choice1Of2;
        }

        internal bool MatchChoice2Of2(out T2 value)
        {
            value = Tag == ChoiceType.Choice2Of2 ? ((Choice2Of2<T1, T2>)this).Value : default(T2);
            return Tag == ChoiceType.Choice2Of2;
        }

        public void Match(Action<T1> onChoice1Of2, Action<T2> onChoice2Of2)
        {
            if (Tag == ChoiceType.Choice1Of2)
                onChoice1Of2(((Choice1Of2<T1, T2>)this).Value);
            else
                onChoice2Of2(((Choice2Of2<T1, T2>)this).Value);
        }
    }

    internal class Choice1Of2<T1, T2> : Choice<T1, T2>
    {
        public Choice1Of2(T1 value)
            : base(ChoiceType.Choice1Of2)
        {
            _value = value;
        }
        private readonly T1 _value;

        public T1 Value { get { return _value; } }
    }

    internal class Choice2Of2<T1, T2> : Choice<T1, T2>
    {
        public Choice2Of2(T2 value)
            : base(ChoiceType.Choice2Of2)
        {
            _value = value;
        }
        private readonly T2 _value;

        public T2 Value { get { return _value; } }
    }

    public static class Choice
    {
        public static Choice<T1, T2> NewChoice1Of2<T1, T2>(T1 value)
        {
            return new Choice1Of2<T1, T2>(value);
        }

        public static Choice<T1, T2> NewChoice2Of2<T1, T2>(T2 value)
        {
            return new Choice2Of2<T1, T2>(value);
        }
    }

    public static class ChoiceExtensions
    {
        public static Choice<TResult, T2> Map<T1, T2, TResult>(this Choice<T1, T2> source, Func<T1, TResult> selector)
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
            return source.Map(selector);
        }

        public static Choice<TResult, T2> SelectMany<TSource, TValue, T2, TResult>(this Choice<TSource, T2> source, Func<TSource, Choice<TValue, T2>> valueSelector, Func<TSource, TValue, TResult> resultSelector)
        {
            return source.Bind(s => valueSelector(s).Map(v => resultSelector(s, v)));
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
