namespace FunctionalExtensions
{
    internal class None<T> : Option<T>
    {
        public None() : base(OptionType.None) { }
    }
}