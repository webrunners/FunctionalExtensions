using System;
using System.Collections.Generic;

namespace FunctionalExtensions
{
    public abstract class Option<T>
    {
        private readonly OptionType _tag;

        protected Option(OptionType tag)
        {
            _tag = tag;
        }

        public OptionType Tag { get { return _tag; } }

        internal bool MatchNone()
        {
            return Tag == OptionType.None;
        }

        internal bool MatchSome(out T value)
        {
            value = Tag == OptionType.Some ? ((Some<T>)this).Value : default(T);
            return Tag == OptionType.Some;
        }

        public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone)
        {
            return Tag == OptionType.Some ? onSome(((Some<T>) this).Value) : onNone();
        }

        public override int GetHashCode()
        {
            return Tag == OptionType.Some ? EqualityComparer<T>.Default.GetHashCode(((Some<T>)this).Value) : EqualityComparer<T>.Default.GetHashCode(default(T));
        }

        private bool EqualsOption(Option<T> other)
        {
            return Tag == OptionType.Some && other.Tag == OptionType.Some && Equals(((Some<T>)this).Value, ((Some<T>)other).Value)
                || Tag == OptionType.None && other.Tag == OptionType.None;
        }

        public override bool Equals(object obj)
        {
            return obj == null && Tag == OptionType.None
                || obj is Option<T> && EqualsOption((Option<T>)obj);
        }

        public static bool operator ==(Option<T> a, Option<T> b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if ((object)a == null)
                return b.Equals(a);
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
            return new None<T>();
        }
        public static Option<T> Some<T>(T value)
        {
            return new Some<T>(value);
        }
    }
}
