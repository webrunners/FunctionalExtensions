namespace FunctionalExtensions
{
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
}