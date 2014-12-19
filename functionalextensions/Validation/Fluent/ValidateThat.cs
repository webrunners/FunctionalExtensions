using System;

namespace FunctionalExtensions.Validation.Fluent
{
    internal class ValidateThat<T, TError> : IValidateThat<T, TError> where T : class
    {
        private readonly T _instance;

        internal ValidateThat(T instance)
        {
            _instance = instance;
            Result = new Choice1Of2<T, Errors<TError>>(_instance);
        }

        public IChain<T, TError> IsNotNull(TError err)
        {
            Result = Validator.NotNull(_instance, err);
            return new Chain<T, TError>(this);
        }

        public IChain<T, TError> Fulfills(Func<T, bool> pred, TError err)
        {
            if (_instance == default(T))
            {
                return new Chain<T, TError>(this);
            }
            if (!pred(_instance))
            {
                Result =
                    from x in Result
                    join y in Choice.NewChoice2Of2<T, Errors<TError>>(new Errors<TError>(err)) on 1 equals 1
                    select _instance;
            }
            return new Chain<T, TError>(this);
        }

        internal T Instance
        {
            get { return _instance; }
        }

        internal Choice<T, Errors<TError>> Result { get; set; }
    }
}