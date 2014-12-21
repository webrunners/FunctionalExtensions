using System;

namespace FunctionalExtensions.Validation.Fluent
{
    internal class FluentValidator<T, TError> : IIntermediate1<T, TError>, IIntermediate2<T, TError>, IIntermediate3<T, TError> where T : class
    {
        private readonly T _instance;

        internal FluentValidator(T instance)
        {
            _instance = instance;
            Result = new Choice1Of2<T, Failures<TError>>(_instance);
        }

        internal FluentValidator(T instance, Choice<T, Failures<TError>> result)
        {
            _instance = instance;
            Result = result;
        }

        public IIntermediate2<T, TError> IsNotNull(TError err)
        {
            return new FluentValidator<T, TError>(_instance,
                from x in Result
                join y in Validator.NotNull(_instance, err) on 1 equals 1
                select _instance);
        }

        public IIntermediate2<T, TError> Fulfills(Predicate<T> pred, TError err)
        {
            return _instance == default(T)
                ? this
                : new FluentValidator<T, TError>(_instance,
                    from x in Result
                    join y in Validator.Create(pred, err)(_instance) on 1 equals 1
                    select _instance);
        }

        public Choice<T, Failures<TError>> Result { get; private set; }

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