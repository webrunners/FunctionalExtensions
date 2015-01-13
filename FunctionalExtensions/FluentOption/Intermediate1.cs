using System;

namespace FunctionalExtensions.FluentOption
{
    public class Intermediate1<T1> : IIntermediate1<T1>
    {
        private readonly Func<Option<T1>> _f1;

        public Intermediate1(Func<Option<T1>> f)
        {
            _f1 = f;
        }

        public IIntermediate2<T1, TResult> From<TResult>(Func<Option<TResult>> f2)
        {
            return new Intermediate2<T1, TResult>(_f1, f2);
        }

        public IIntermediate1<TResult> Bind<TResult>(Func<T1, Option<TResult>> selector)
        {
            return new Intermediate1<TResult>(() => _f1().Bind(selector));
        }

        public Option<T1> Result()
        {
            return _f1();
        }


        public IIntermediate1<TResult> Select<TResult>(Func<T1, TResult> selector)
        {
            return new Intermediate1<TResult>(() => _f1().Select(selector));
        }
    }
}