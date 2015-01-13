using System;
using FunctionalExtensions.Lambda;

namespace FunctionalExtensions.FluentOption
{
    public class Intermediate5<T1, T2, T3, T4, T5> : IIntermediate5<T1, T2, T3, T4, T5>
    {
        private readonly Func<Option<T1>> _f1;
        private readonly Func<Option<T2>> _f2;
        private readonly Func<Option<T3>> _f3;
        private readonly Func<Option<T4>> _f4;
        private readonly Func<Option<T5>> _f5;

        public Intermediate5(
            Func<Option<T1>> f1,
            Func<Option<T2>> f2,
            Func<Option<T3>> f3,
            Func<Option<T4>> f4,
            Func<Option<T5>> f5)
        {
            _f1 = f1;
            _f2 = f2;
            _f3 = f3;
            _f4 = f4;
            _f5 = f5;
        }

        public Option<T5> Result()
        {
            return _f1()
                .Bind(x => _f2())
                .Bind(x => _f3())
                .Bind(x => _f4())
                .Bind(x => _f5());
        }

        public IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, T5, Option<TResult>> selector)
        {
            var result = Fun.Create(() => _f1()
                .Bind(v1 => _f2()
                    .Bind(v2 => _f3()
                        .Bind(v3 => _f4()
                            .Bind(v4 => _f5()
                                .Bind(v5 => selector(v1, v2, v3, v4, v5)))))));

            return new Intermediate1<TResult>(result);
        }

        public IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            var result = Fun.Create(() => _f1()
                .Bind(v1 => _f2()
                    .Bind(v2 => _f3()
                        .Bind(v3 => _f4()
                            .Bind(v4 => _f5()
                                .Select(v5 => selector(v1, v2, v3, v4, v5)))))));

            return new Intermediate1<TResult>(result);
        }

        public IIntermediate6<T1, T2, T3, T4, T5, TResult> From<TResult>(Func<Option<TResult>> f6)
        {
            return new Intermediate6<T1, T2, T3, T4, T5, TResult>(_f1, _f2, _f3, _f4, _f5, f6);
        }
    }
}