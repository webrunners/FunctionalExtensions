namespace FunctionalExtensions
{
    internal class Some<T> : Option<T>
    {
        public Some(T value)
            : base(OptionType.Some)
        {
            _value = value;
        }
        private readonly T _value;
        public T Value { get { return _value; } }
    }
}