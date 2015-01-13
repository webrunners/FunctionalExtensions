using System;
using System.Collections.Generic;

namespace FunctionalExtensions
{
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

        public OptionType Tag { get { return _tag; } }

        internal bool MatchNone()
        {
            return Tag == OptionType.None;
        }

        internal bool MatchSome(out T value)
        {
            value = Tag == OptionType.Some ? _value : default(T);
            return Tag == OptionType.Some;
        }

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
    }

    public static class Option
    {
        public static Option<T> None<T>()
        {
            return new Option<T>(Unit.Value);
        }
        public static Option<T> Some<T>(T value)
        {
            return new Option<T>(value);
        }

        public static Option<T> Return<T>(T value)
        {
            return Some(value);
        }
    }
}
