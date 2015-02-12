using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtensions.Validation
{
    [Obsolete("The Validation framework will be moved to another package.")]
    public class Failure<T> : IEnumerable<T>
    {
        private readonly List<T> _errors = new List<T>();

        internal Failure(T message)
        {
            _errors.Add(message);
        }

        internal Failure(IEnumerable<T> messages)
        {
            _errors.AddRange(messages);
        }

        internal void AddError(T message)
        {
            _errors.Add(message);
        }

        public bool HasErrors()
        {
            return _errors.Any();
        }

        public static Failure<T> Merge(Failure<T> errors1, Failure<T> errors2)
        {
            return Failure.Create(errors1.Concat(errors2));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class Failure
    {
        public static Failure<T> Create<T>(IEnumerable<T> messages)
        {
            return new Failure<T>(messages);
        }

        public static Failure<T> Create<T>(T messages)
        {
            return new Failure<T>(messages);
        }
    }
}