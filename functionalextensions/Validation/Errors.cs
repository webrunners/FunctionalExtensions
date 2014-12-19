using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtensions.Validation
{
    public class Errors<T>
    {
        private readonly List<T> _errors = new List<T>();

        public IReadOnlyCollection<T> Get
        {
            get { return _errors; }
        } 

        public Errors(T message)
        {
            _errors.Add(message);
        }

        private Errors(IEnumerable<T> messages)
        {
            _errors.AddRange(messages);
        }

        public static Errors<T> Create(IEnumerable<T> messages)
        {
            return new Errors<T>(messages);
        }

        public void AddError(T message)
        {
            _errors.Add(message);
        }

        public bool HasErrors()
        {
            return _errors.Any();
        }

        public static Errors<T> Merge(Errors<T> errors1, Errors<T> errors2)
        {
            return Create(errors1.Get.Concat(errors2.Get));
        }
    }
}