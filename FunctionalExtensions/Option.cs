using System;
using System.Collections.Generic;

namespace FunctionalExtensions
{
    /// <summary>
    /// Union type with two cases: Some and None
    /// </summary>
    /// <typeparam name="T">Type of the contained value</typeparam>
    public struct Option<T>
    {
        private readonly OptionType _tag;
        private readonly T _value;

        internal Option(T value)
        {
            _value = value;
            _tag = value != null ? OptionType.Some : OptionType.None;
        }

        internal Option(Unit unit)
        {
            _value = default(T);
            _tag = OptionType.None;
        }

        /// <summary>
        /// Specifies wether the option is Some or None
        /// </summary>
        public OptionType Tag { get { return _tag; } }

        public bool IsNone { get { return Tag == OptionType.None; } }

        public bool IsSome { get { return Tag == OptionType.Some; } }

        internal bool MatchNone()
        {
            return Tag == OptionType.None;
        }

        internal bool MatchSome(out T value)
        {
            value = Tag == OptionType.Some ? _value : default(T);
            return Tag == OptionType.Some;
        }

        /// <summary>
        /// Pattern matching for the option type
        /// </summary>
        /// <typeparam name="TResult">Type of the return value</typeparam>
        /// <param name="onSome">Function that will be invoked if the option is Some</param>
        /// <param name="onNone">Function that will be invoked if the option is None</param>
        /// <returns>The result of the invoked function</returns>
        public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone)
        {
            return Tag == OptionType.Some ? onSome(_value) : onNone();
        }

        public override int GetHashCode()
        {
            return Tag == OptionType.Some ? EqualityComparer<T>.Default.GetHashCode(_value) : EqualityComparer<T>.Default.GetHashCode(default(T));
        }

        private bool EqualsOption(Option<T> other)
        {
            return Tag == OptionType.Some && other.Tag == OptionType.Some && Equals(_value, other._value)
                || Tag == OptionType.None && other.Tag == OptionType.None;
        }

        public override bool Equals(object obj)
        {
            return obj == null && Tag == OptionType.None
                || obj is Option<T> && EqualsOption((Option<T>)obj);
        }

        public static bool operator ==(Option<T> a, Option<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Option<T> a, Option<T> b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return Tag == OptionType.Some ? String.Format("Some({0})", _value) : String.Format("None<{0}>", typeof(T).Name);
        }
    }

    /// <summary>
    /// Union type with two cases: Some and None
    /// </summary>
    public static class Option
    {
        /// <summary>
        /// Returns None
        /// </summary>
        /// <typeparam name="T">Type of the contained value</typeparam>
        /// <returns>None</returns>
        public static Option<T> None<T>()
        {
            return new Option<T>(Unit.Value);
        }

        /// <summary>
        /// Returns Some&lt;T&gt;
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="value">The contained value</param>
        /// <returns>Some</returns>
        public static Option<T> Some<T>(T value)
        {
            return new Option<T>(value);
        }

        /// <summary>
        /// Returns Some&lt;T&gt;
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="value">The contained value</param>
        /// <returns>Some</returns>
        public static Option<T> Return<T>(T value)
        {
            return Some(value);
        }
    }
}
