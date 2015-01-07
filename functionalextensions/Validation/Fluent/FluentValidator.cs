using System;

namespace FunctionalExtensions.Validation.Fluent
{
    internal class FluentValidator<T, TError> : IIntermediate1<T, TError>, IIntermediate2<T, TError>, IIntermediate3<T, TError>, IIntermediate2A<T, TError> where T : class
    {
        private readonly T _instance;
        private readonly Choice<T, Failures<TError>> _result;

        internal FluentValidator(T instance)
        {
            _instance = instance;
            _result = new Choice1Of2<T, Failures<TError>>(_instance);
        }

        internal FluentValidator(T instance, Choice<T, Failures<TError>> result)
        {
            _instance = instance;
            _result = result;
        }

        public IIntermediate2<T, TError> IsNotNull(TError err)
        {
            return new FluentValidator<T, TError>(_instance,
                from x in Result
                join y in Validator.NotNull(_instance, err) on 1 equals 1
                select _instance);
        }

        public IIntermediate2<T, TError> Fulfills(Predicate<T> pred, TError err, Func<Exception, TError> onException = null)
        {
            return Fulfills(pred, x => err, onException);
        }

        public IIntermediate2<T, TError> Fulfills(Predicate<T> predicate, Func<T, TError> onError, Func<Exception, TError> onException = null)
        {
            try
            {
                var validationResult = Validator.Create(predicate, onError(_instance))(_instance);

                return new FluentValidator<T, TError>(_instance,
                        from x in Result
                        join y in validationResult on 1 equals 1
                        select _instance);
            }
            catch (Exception ex)
            {
                return onException == null
                    ? new FluentValidator<T, TError>(_instance, _result)
                    : new FluentValidator<T, TError>(_instance,
                        from x in Result
                        join y in Validation.Result.Failure<T, TError>(onException(ex)) on 1 equals 1
                        select _instance);
            }
        }

        public Choice<T, Failures<TError>> Result
        {
            get { return _result; }
        }

        public IIntermediate3<T, TError> And
        {
            get { return this; }
        }

        public IIntermediate4<T, TResult, TError> AndSelect<TResult>(Func<T, TResult> selector) where TResult : class
        {
            return new FluentSelectionValidator<T, TResult, TError>(_instance, selector, Result);
        }
    }
}