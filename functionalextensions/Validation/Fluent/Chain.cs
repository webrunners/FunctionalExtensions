using System;

namespace FunctionalExtensions.Validation.Fluent
{
    internal class Chain<T, TError> : IChain<T, TError> where T : class
    {
        private readonly ValidateThat<T, TError> _validator;

        public Chain(ValidateThat<T, TError> validator)
        {
            _validator = validator;
        }

        public IValidateThat<T, TError> And
        {
            get { return _validator; }
        }

        public IValidateThatMember<T, TResult, TError> AndMember<TResult>(Func<T, TResult> selector)
            where TResult : class
        {
            return _validator.Instance == default(T)
                ? new ValidateThatMember<T, TResult, TError>(_validator, default(TResult))
                : new ValidateThatMember<T, TResult, TError>(_validator, selector(_validator.Instance));
        }

        public Choice<T, Errors<TError>> Result
        {
            get
            {
                return _validator.Result;
            }
        }
    }
}