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
        private readonly T _value;

        internal Option(T value)
        {
            _value = value;
            Tag = value != null ? OptionType.Some : OptionType.None;
        }

        internal Option(Unit unit)
        {
            _value = default(T);
            Tag = OptionType.None;
        }

        /// <summary>
        /// Specifies wether the option is Some or None
        /// </summary>
        public OptionType Tag { get; }

        public bool IsNone => Tag == OptionType.None;
        public bool IsSome => Tag == OptionType.Some;

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

        public bool Equals(Option<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        public bool Equals(T other)
        {
            return EqualityComparer<T>.Default.Equals(_value, other);
        }

        public override bool Equals(object obj)
        {
            return obj == null && Tag == OptionType.None
                || obj is T && Equals((T) obj)
                || obj is Option<T> && Equals((Option<T>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(_value);
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

        public static implicit operator Option<T>(T value)
        {
            return value.ToOption();
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
