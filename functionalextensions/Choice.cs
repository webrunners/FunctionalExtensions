using System;

namespace FunctionalExtensions
{
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

        public static Choice<T1, T2> Unit<T1, T2>(T1 value)
        {
            return new Choice1Of2<T1, T2>(value);
        }
    }
}
