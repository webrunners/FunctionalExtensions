using System;

namespace FunctionalExtensions.Validation.Fluent
{
    internal class ValidateThatMember<T, TMember, TError> : IValidateThatMember<T, TMember, TError>
        where T : class
        where TMember : class
    {
        private readonly ValidateThat<T, TError> _validator;
        private readonly TMember _member;

        public ValidateThatMember(ValidateThat<T, TError> validator, TMember member)
        {
            _validator = validator;
            _member = member;
        }

        public IChain<T, TError> IsNotNull(TError err)
        {
            if (_validator.Instance == default(T))
            {
                return new Chain<T, TError>(_validator);
            }
            if (_member == default(TMember))
            {
                _validator.Result =
                    from x in _validator.Result
                    join y in Choice.NewChoice2Of2<T, Errors<TError>>(new Errors<TError>(err)) on 1 equals 1
                    select _validator.Instance;
            }
            return new Chain<T, TError>(_validator);
        }

        public IChain<T, TError> Fulfills(Func<TMember, bool> pred, TError err)
        {
            if (_member == default(TMember) || _validator.Instance == default(T))
            {
                return new Chain<T, TError>(_validator);
            }
            if (!pred(_member))
            {
                _validator.Result =
                    from x in _validator.Result
                    join y in Choice.NewChoice2Of2<T, Errors<TError>>(new Errors<TError>(err)) on 1 equals 1
                    select _validator.Instance;
            }
            return new Chain<T, TError>(_validator);
        }
    }
}
