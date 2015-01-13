namespace FunctionalExtensions
{
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
}