using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public class FluentValidator<T> where T : class
    {
        private readonly T _instance;

        internal FluentValidator(T instance)
        {
            _instance = instance;
        }

        public Continuation<T> IsNotNull(string err)
        {
            Result = Validator.NotNull(_instance, err);
            return new Continuation<T>(this);
        }

        internal T Instance
        {
            get { return _instance; }
        }

        public Choice<T, Errors> Result { get; internal set; }
    }

    public class Continuation<T> where T : class
    {
        private readonly FluentValidator<T> _validator;

        public Continuation(FluentValidator<T> validator)
        {
            _validator = validator;
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
        private readonly FluentValidator<T> _validator;
        private readonly TMember _member;

        public MemberValidator(FluentValidator<T> validator, TMember member)
        {
            _validator = validator;
            _member = member;
        }

        public Continuation<T> IsNotNull(string err)
        {
            if (_validator.Instance == default(T))
            {
                return new Continuation<T>(_validator);
            }
            if (_member == default(TMember))
            {
                _validator.Result =
                    from x in _validator.Result
                    join y in Choice.NewChoice2Of2<T, Errors>(new Errors(err)) on 1 equals 1
                    select _validator.Instance;
            }
            return new Continuation<T>(_validator);
        }

        public Continuation<T> Fulfills(Func<TMember, bool> pred, string err)
        {
            if (_member == default(TMember) || _validator.Instance == default(T))
            {
                return new Continuation<T>(_validator);
            }
            if (!pred(_member))
            {
                _validator.Result =
                    from x in _validator.Result
                    join y in Choice.NewChoice2Of2<T, Errors>(new Errors(err)) on 1 equals 1
                    select _validator.Instance;
            }
            return new Continuation<T>(_validator);
        }
    }

    public static class Validate
    {
        public static FluentValidator<T> That<T>(T instance) where T : class
        {
            return new FluentValidator<T>(instance);
        }
    }
}
