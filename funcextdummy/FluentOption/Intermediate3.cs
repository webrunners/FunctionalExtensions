using FunctionalExtensions.Lambda;
using System;

namespace FunctionalExtensions.FluentOption
{
    public class Intermediate3<T1, T2, T3> : IIntermediate3<T1, T2, T3>
    {
        private readonly Func<Option<T1>> _f1;
        private readonly Func<Option<T2>> _f2;
        private readonly Func<Option<T3>> _f3;

        public Intermediate3(Func<Option<T1>> f1, Func<Option<T2>> f2, Func<Option<T3>> f3)
        {
            _f1 = f1;
            _f2 = f2;
            _f3 = f3;
        }

        public Option<T3> Result()
        {
            return _f1().Bind(x => _f2()).Bind(x => _f3());
        }

        public IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, Option<TResult>> selector)
        {
            var result = Fun.Create(() => _f1()
                .Bind(v1 => _f2()
                    .Bind(v2 => _f3()
                        .Bind(v3 => selector(v1, v2, v3)))));

            return new Intermediate1<TResult>(result);
        }

        public IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, TResult> selector)
        {
            var result = Fun.Create(() => _f1()
                .Bind(v1 => _f2()
                    .Bind(v2 => _f3()
                        .Select(v3=> selector(v1, v2, v3)))));

            return new Intermediate1<TResult>(result);
        }

        public IIntermediate4<T1, T2, T3, TResult> From<TResult>(Func<Option<TResult>> f4)
        {
            return new Intermediate4<T1, T2, T3, TResult>(_f1, _f2, _f3, f4);
        }
    }
}
