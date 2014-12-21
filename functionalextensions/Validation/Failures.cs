using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtensions.Validation
{
    public class Failures<T>
    {
        private readonly List<T> _errors = new List<T>();

        public IReadOnlyCollection<T> Errors
        {
            get { return _errors; }
        } 

        public Failures(T message)
        {
            _errors.Add(message);
        }

        private Failures(IEnumerable<T> messages)
        {
            _errors.AddRange(messages);
        }

        public static Failures<T> Create(IEnumerable<T> messages)
        {
            return new Failures<T>(messages);
        }

        public void AddError(T message)
        {
            _errors.Add(message);
        }

        public bool HasErrors()
        {
            return _errors.Any();
        }

        public static Failures<T> Merge(Failures<T> errors1, Failures<T> errors2)
        {
            return Create(errors1.Errors.Concat(errors2.Errors));
        }
    }
}