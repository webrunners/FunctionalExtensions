using System;

namespace FunctionalExtensions
{
    public class Validator<T> where T : class
    {
        private readonly T _instance;

        internal Validator(T instance)
        {
            _instance = instance;
        }

        public Continuation<T> IsNotNull(string err)
        {
            Result = Validation.NonNull(_instance, err);
            return new Continuation<T>(this, Result);
        }

        internal T Instance
        {
            get { return _instance; }
        }

        public Choice<T, Errors> Result { get; internal set; }
    }

    public class Continuation<T> where T : class
    {
        private readonly Validator<T> _validator;
        private readonly Choice<T, Errors> _result;

        public Continuation(Validator<T> validator, Choice<T, Errors> result)
        {
            _validator = validator;
            _result = result;
        }

        public MemberValidator<T, TResult> And<TResult>(Func<T, TResult> selector)
            where TResult : class
        {
            return _validator.Instance == default(T)
                ? new MemberValidator<T, TResult>(_validator, default(TResult))
                : new MemberValidator<T, TResult>(_validator, selector(_validator.Instance));
        }

        public Choice<T, Errors> Result
        {
            get
            {
                return _validator.Result;
            }
        }
    }

    public class MemberValidator<T, TMember>
        where T : class
        where TMember : class
    {
        private readonly Validator<T> _validator;
        private readonly TMember _member;

        public MemberValidator(Validator<T> validator, TMember member)
        {
            _validator = validator;
            _member = member;
        }

        public Continuation<T> IsNotNull(string err)
        {
            if (_validator.Instance == default(T))
            {
                return new Continuation<T>(_validator, _validator.Result);
            }
            if (_member == default(TMember))
            {
                _validator.Result =
                    from x in _validator.Result
                    join y in Choice.NewChoice2Of2<T, Errors>(new Errors(err)) on 1 equals 1
                    select _validator.Instance;
            }
            return new Continuation<T>(_validator, _validator.Result);
        }

        public Continuation<T> Fulfills(Func<TMember, bool> pred, string err)
        {
            if (_member == default(TMember) || _validator.Instance == default(T))
            {
                return new Continuation<T>(_validator, _validator.Result);
            }
            if (!pred(_member))
            {
                _validator.Result =
                    from x in _validator.Result
                    join y in Choice.NewChoice2Of2<T, Errors>(new Errors(err)) on 1 equals 1
                    select _validator.Instance;
            }
            return new Continuation<T>(_validator, _validator.Result);
        }
    }

    public static class Validate
    {
        public static Validator<T> That<T>(T instance) where T : class
        {
            return new Validator<T>(instance);
        }
    }
}
