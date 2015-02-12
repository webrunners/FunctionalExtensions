using System;

namespace FunctionalExtensions.Validation.Fluent
{
    internal class FluentSelectionValidator<T, TResult, TError> : IIntermediate4<T, TResult, TError>
        where T : class
        where TResult : class
    {
        private readonly T _instance;
        private readonly Func<T, TResult> _selector;
        private readonly Choice<T, Failure<TError>> _result;

        public FluentSelectionValidator(T instance, Func<T, TResult> selector, Choice<T, Failure<TError>> result)
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

        public IIntermediate2A<T, TError> Fulfills(Predicate<TResult> predicate, Func<TResult, TError> onError, Func<Exception, TError> onException = null)
        {
            try
            {
                var selection = _selector(_instance);
                var validationResult = Validator.Create(predicate, onError(selection))(selection);

                return new FluentValidator<T, TError>(_instance,
                    from x in _result
                    join y in validationResult on 1 equals 1
                    select _instance);
            }
            catch (Exception ex)
            {
                return onException == null
                    ? new FluentValidator<T, TError>(_instance, _result)
                    : new FluentValidator<T, TError>(_instance,
                        from x in _result
                        join y in Result.Failure<T, TError>(onException(ex)) on 1 equals 1
                        select _instance);
            }
        }

        public IIntermediate2A<T, TError> Fulfills(Predicate<TResult> predicate, TError error, Func<Exception, TError> onException = null)
        {
            return Fulfills(predicate, x => error, onException);
        }
    }
}