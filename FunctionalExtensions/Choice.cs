using System;

namespace FunctionalExtensions
{
    public struct Choice<T1, T2>
    {
        private readonly ChoiceType _tag;
        private readonly T1 _choice1Of2;
        private readonly T2 _choice2Of2;

        internal Choice(T1 choice1Of2)
        {
            if(choice1Of2 == null)
                throw new ArgumentNullException(nameof(choice1Of2));
            _choice1Of2 = choice1Of2;
            _choice2Of2 = default(T2);
            _tag = ChoiceType.Choice1Of2;
        }

        internal Choice(T2 choice2Of2)
        {
            if (choice2Of2 == null)
                throw new ArgumentNullException(nameof(choice2Of2));
            _choice2Of2 = choice2Of2;
            _choice1Of2 = default(T1);
            _tag = ChoiceType.Choice2Of2;
        }

        public ChoiceType Tag { get { return _tag; } }

        internal bool MatchChoice1Of2(out T1 value)
        {
            value = Tag == ChoiceType.Choice1Of2 ? _choice1Of2 : default(T1);
            return Tag == ChoiceType.Choice1Of2;
        }

        internal bool MatchChoice2Of2(out T2 value)
        {
            value = Tag == ChoiceType.Choice2Of2 ? _choice2Of2 : default(T2);
            return Tag == ChoiceType.Choice2Of2;
        }

        [Obsolete("Match is deprecated, please use overload of Match: TResult Match<TResult>(Func<T1, TResult> onChoice1Of2, Func<T2, TResult> onChoice2Of2) instead.")]
        public void Match(Action<T1> onChoice1Of2, Action<T2> onChoice2Of2)
        {
            if (Tag == ChoiceType.Choice1Of2)
                onChoice1Of2(_choice1Of2);
            else
                onChoice2Of2(_choice2Of2);
        }

        public TResult Match<TResult>(Func<T1, TResult> onChoice1Of2, Func<T2, TResult> onChoice2Of2)
        {
            return Tag == ChoiceType.Choice1Of2 
                ? onChoice1Of2(_choice1Of2) 
                : onChoice2Of2(_choice2Of2);
        }

        public override string ToString()
        {
            var t1T2 = string.Format("<{0}, {1}>", typeof (T1).Name, typeof (T2).Name);
            return Tag == ChoiceType.Choice1Of2 ? String.Format("Choice1Of2{1}({0})", _choice1Of2, t1T2) : String.Format("Choice2Of2{1}({0})", _choice2Of2, t1T2);
        }
    }

    public static class Choice
    {
        public static Choice<T1, T2> NewChoice1Of2<T1, T2>(T1 value)
        {
            return new Choice<T1, T2>(value);
        }

        public static Choice<T1, T2> NewChoice2Of2<T1, T2>(T2 value)
        {
            return new Choice<T1, T2>(value);
        }

        public static Choice<T1, T2> Return<T1, T2>(T1 value)
        {
            return new Choice<T1, T2>(value);
        }
    }
}
