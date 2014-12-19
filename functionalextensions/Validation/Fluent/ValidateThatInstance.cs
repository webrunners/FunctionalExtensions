using System;

namespace FunctionalExtensions.Validation.Fluent
{
    internal class ValidateThatInstance<T, TError> : IValidateThatInstance<T, TError> where T : class
    {
        private readonly T _instance;

        internal ValidateThatInstance(T instance)
        {
            _instance = instance;
            Result = new Choice1Of2<T, Errors<TError>>(_instance);
        }

        public IChain<T, TError> IsNotNull(TError err)
        {
            Result =
                from x in Result
                join y in Validator.NotNull(_instance, err) on 1 equals 1
                select _instance;

            return new Chain<T, TError>(this);
        }

        public IValidateThatMember<T, TResult, TError> Member<TResult>(Func<T, TResult> selector)
            where TResult : class
        {
            return Instance == default(T)
                ? new ValidateThatMember<T, TResult, TError>(this, default(TResult))
                : new ValidateThatMember<T, TResult, TError>(this, selector(Instance));
        }

        public IChain<T, TError> Fulfills(Predicate<T> pred, TError err)
        {
            if (_instance == default(T))
            {
                return new Chain<T, TError>(this);
            }

            Result =
                from x in Result
                join y in Validator.Create(pred, err)(_instance) on 1 equals 1
                select _instance;

            return new Chain<T, TError>(this);
        }

        public IChain<T, TError> IsEqualTo(T pattern, TError err)
        {
            if (_instance == default(T))
            {
                return new Chain<T, TError>(this);
            }

            Result =
                from x in Result
                join y in Validator.Equal(_instance, pattern, err) on 1 equals 1
                select _instance;

            return new Chain<T, TError>(this);
        }

        internal T Instance
        {
            get { return _instance; }
        }

        internal Choice<T, Errors<TError>> Result { get; set; }
    }
}