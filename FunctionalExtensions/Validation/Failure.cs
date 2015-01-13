using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtensions.Validation
{
    public class Failure<T>
    {
        private readonly List<T> _errors = new List<T>();

        public IReadOnlyCollection<T> Errors
        {
            get { return _errors; }
        } 

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
            return Failure.Create(errors1.Errors.Concat(errors2.Errors));
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