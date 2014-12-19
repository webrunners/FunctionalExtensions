using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public interface IValidateThat<T> where T : class
    {
        IChain<T> IsNotNull(string err);
        IChain<T> Fulfills(Func<T, bool> pred, string err);
    }

    public interface IValidateThatMember<T, out TMember>
        where T : class
        where TMember : class
    {
        IChain<T> IsNotNull(string err);
        IChain<T> Fulfills(Func<TMember, bool> pred, string err);
    }

    public interface IChain<T> where T : class
    {
        IValidateThat<T> And { get; }
        IValidateThatMember<T, TResult> AndMember<TResult>(Func<T, TResult> selector) where TResult : class;
        Choice<T, Errors> Result { get; }
    }

    public static class Validate
    {
        public static IValidateThat<T> That<T>(T instance) where T : class
        {
            return new ValidateThat<T>(instance);
        }
    }

    internal class ValidateThat<T> : IValidateThat<T> where T : class
    {
        private readonly T _instance;

        internal ValidateThat(T instance)
        {
            _instance = instance;
            Result = new Choice1Of2<T, Errors>(_instance);
        }

        public IChain<T> IsNotNull(string err)
        {
            Result = Validator.NotNull(_instance, err);
            return new Chain<T>(this);
        }

        public IChain<T> Fulfills(Func<T, bool> pred, string err)
        {
            if (_instance == default(T))
            {
                return new Chain<T>(this);
            }
            if (!pred(_instance))
            {
                Result =
                    from x in Result
                    join y in Choice.NewChoice2Of2<T, Errors>(new Errors(err)) on 1 equals 1
                    select _instance;
            }
            return new Chain<T>(this);
        }

        internal T Instance
        {
            get { return _instance; }
        }

        internal Choice<T, Errors> Result { get; set; }
    }

    internal class Chain<T> : IChain<T> where T : class
    {
        private readonly ValidateThat<T> _validator;

        public Chain(ValidateThat<T> validator)
        {
            _validator = validator;
        }

        public IValidateThat<T> And
        {
            get { return _validator; }
        }

        public IValidateThatMember<T, TResult> AndMember<TResult>(Func<T, TResult> selector)
            where TResult : class
        {
            return _validator.Instance == default(T)
                ? new ValidateThatMember<T, TResult>(_validator, default(TResult))
                : new ValidateThatMember<T, TResult>(_validator, selector(_validator.Instance));
        }

        public Choice<T, Errors> Result
        {
            get
            {
                return _validator.Result;
            }
        }
    }

    internal class ValidateThatMember<T, TMember> : IValidateThatMember<T, TMember> where T : class
        where TMember : class
    {
        private readonly ValidateThat<T> _validator;
        private readonly TMember _member;

        public ValidateThatMember(ValidateThat<T> validator, TMember member)
        {
            _validator = validator;
            _member = member;
        }

        public IChain<T> IsNotNull(string err)
        {
            if (_validator.Instance == default(T))
            {
                return new Chain<T>(_validator);
            }
            if (_member == default(TMember))
            {
                _validator.Result =
                    from x in _validator.Result
                    join y in Choice.NewChoice2Of2<T, Errors>(new Errors(err)) on 1 equals 1
                    select _validator.Instance;
            }
            return new Chain<T>(_validator);
        }

        public IChain<T> Fulfills(Func<TMember, bool> pred, string err)
        {
            if (_member == default(TMember) || _validator.Instance == default(T))
            {
                return new Chain<T>(_validator);
            }
            if (!pred(_member))
            {
                _validator.Result =
                    from x in _validator.Result
                    join y in Choice.NewChoice2Of2<T, Errors>(new Errors(err)) on 1 equals 1
                    select _validator.Instance;
            }
            return new Chain<T>(_validator);
        }
    }
}
