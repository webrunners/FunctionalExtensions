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

        public IIntermediate2<T, TError> IsNotNull(TError error)
        {
            try
            {
                var selection = _selector(_instance);
                return new FluentValidator<T, TError>(_instance,
                    from x in _result
                    join y in Validator.NotNull(selection, error) on 1 equals 1
                    select _instance);
            }
            catch (Exception)
            {
                return new FluentValidator<T, TError>(_instance, _result);
            }
        }

        public IIntermediate2<T, TError> Fulfills(Predicate<TResult> predicate, TError error)
        {
            try
            {
                var selection = _selector(_instance);
                return selection == default(TResult)
                    ? new FluentValidator<T, TError>(_instance, _result)
                    : new FluentValidator<T, TError>(_instance,
                        from x in _result
                        join y in Validator.Create(predicate, error)(selection) on 1 equals 1
                        select _instance);
            }
            catch (Exception)
            {
                return new FluentValidator<T, TError>(_instance, _result);
            }
        }
    }
}