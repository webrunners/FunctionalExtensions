using System;
using FunctionalExtensions.Lambda;

namespace FunctionalExtensions.FluentOption
{
    public class Intermediate2<T1, T2> : IIntermediate2<T1, T2>
    {
        private readonly Func<Option<T1>> _f1;
        private readonly Func<Option<T2>> _f2;

        public Intermediate2(Func<Option<T1>> f1, Func<Option<T2>> f2)
        {
            _f1 = f1;
            _f2 = f2;
        }

        public IIntermediate3<T1, T2, TResult> From<TResult>(Func<Option<TResult>> f3)
        {
            return new Intermediate3<T1, T2, TResult>(_f1, _f2, f3);
        }

        public IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, Option<TResult>> selector)
        {
            var result = Fun.Create(() => _f1().Bind(v1 => _f2().Bind(v2 => selector(v1, v2))));
            return new Intermediate1<TResult>(result);
        }

        public Option<T2> Result()
        {
            return _f1().Bind(x => _f2());
        }

        public IIntermediate1<TResult> Select<TResult>(Func<T1, T2, TResult> selector)
        {
            var result = Fun.Create(() => _f1().Bind(v1 => _f2().Select(v2 => selector(v1, v2))));
            return new Intermediate1<TResult>(result);
        }
    }
}