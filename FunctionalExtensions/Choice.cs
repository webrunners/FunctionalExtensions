using System;

namespace FunctionalExtensions
{
    public struct Choice<T1, T2>
    {
        private readonly T1 _choice1Of2;
        private readonly T2 _choice2Of2;

        internal bool IsChoice1Of2 => Tag == ChoiceType.Choice1Of2;
        internal bool IsChoice2Of2 => Tag == ChoiceType.Choice2Of2;

        internal Choice(T1 choice1Of2)
        {
            if(choice1Of2 == null)
                throw new ArgumentNullException(nameof(choice1Of2));
            _choice1Of2 = choice1Of2;
            _choice2Of2 = default(T2);
            Tag = ChoiceType.Choice1Of2;
        }

        internal Choice(T2 choice2Of2)
        {
            if (choice2Of2 == null)
                throw new ArgumentNullException(nameof(choice2Of2));
            _choice2Of2 = choice2Of2;
            _choice1Of2 = default(T1);
            Tag = ChoiceType.Choice2Of2;
        }

        public ChoiceType Tag { get; }

        [Obsolete("Match is deprecated, please use overload of Match: TResult Match<TResult>(Func<T1, TResult> onChoice1Of2, Func<T2, TResult> onChoice2Of2) instead.")]
        public void Match(Action<T1> onChoice1Of2, Action<T2> onChoice2Of2)
        {
            if (IsChoice1Of2)
                onChoice1Of2(_choice1Of2);
            else
                onChoice2Of2(_choice2Of2);
        }

        public TResult Match<TResult>(Func<T1, TResult> onChoice1Of2, Func<T2, TResult> onChoice2Of2)
        {
            return IsChoice1Of2 
                ? onChoice1Of2(_choice1Of2) 
                : onChoice2Of2(_choice2Of2);
        }

        public override string ToString()
        {
            var t1T2 = $"<{typeof(T1).Name}, {typeof(T2).Name}>";
            return IsChoice1Of2 
                ? $"Choice1Of2{t1T2}({_choice1Of2})"
                : $"Choice2Of2{t1T2}({_choice2Of2})";
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
