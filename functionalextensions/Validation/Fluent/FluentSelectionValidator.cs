using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public class FluentSelectionValidator<T, TResult, TError> : IIntermediate4<T, TResult, TError>
        where T : class
        where TResult : class
    {
        private readonly T _instance;
        private readonly Func<T, TResult> _selector;
        private readonly Choice<T, Failures<TError>> _result;

        public FluentSelectionValidator(T instance, Func<T, TResult> selector, Choice<T, Failures<TError>> result)
        {
            _instance = instance;
            _selector = selector;
            _result = result;
        }

        public IIntermediate2A<T, TError> IsNotNull(TError error)
        {
            try
            {
                var selection = _selector(_instance);
                var validationResult = Validator.NotNull(selection, error);
                return new FluentValidator<T, TError>(_instance,
                    from x in _result
                    join y in validationResult on 1 equals 1
                    select _instance);
            }
            catch (NullReferenceException)
            {
                return new FluentValidator<T, TError>(_instance, _result);
            }
        }

        public IIntermediate2A<T, TError> Fulfills(Predicate<TResult> predicate, Func<TResult, TError> error, Func<NullReferenceException, TError> onNullReferenceException = null, Func<ArgumentOutOfRangeException, TError> onArgumentOutOfRangeException  = null)
        {
            try
            {
                return ValidateSelection(predicate, error(_selector(_instance)));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return onArgumentOutOfRangeException == null
                    ? new FluentValidator<T, TError>(_instance, _result)
                    : new FluentValidator<T, TError>(_instance,
                        from x in _result
                        join y in Result.Failure<T, TError>(onArgumentOutOfRangeException(ex)) on 1 equals 1
                        select _instance);
            }
            catch (NullReferenceException ex)
            {
                return onNullReferenceException == null
                    ? new FluentValidator<T, TError>(_instance, _result)
                    : new FluentValidator<T, TError>(_instance,
                        from x in _result
                        join y in Result.Failure<T, TError>(onNullReferenceException(ex)) on 1 equals 1
                        select _instance);
            }
        }

        public IIntermediate2A<T, TError> Fulfills(Predicate<TResult> predicate, TError error, Func<NullReferenceException, TError> onNullReferenceException = null, Func<ArgumentOutOfRangeException, TError> onArgumentOutOfRangeException = null)
        {
            return Fulfills(predicate, x => error, onNullReferenceException, onArgumentOutOfRangeException);
        }

        private IIntermediate2A<T, TError> ValidateSelection(Predicate<TResult> predicate, TError error)
        {
            var selection = _selector(_instance);
            var validationResult = Validator.Create(predicate, error)(selection);
            return new FluentValidator<T, TError>(_instance,
                from x in _result
                join y in validationResult on 1 equals 1
                select _instance);
        }
    }
}