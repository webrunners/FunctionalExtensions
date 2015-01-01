﻿using FunctionalExtensions.Lambda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtensions.FluentOption
{
    public static class OptionMonad
    {
        public static IIntermediate1<TResult> From<TResult>(Func<Option<TResult>> f)
        {
            return new Intermediate1<TResult>(f);
        }
    }

    public interface IResult<T>
    {
        Option<T> Result();
    }

    public interface IIntermediate1<T1> : IResult<T1>
    {
        IIntermediate2<T1, TResult> From<TResult>(Func<Option<TResult>> f2);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, TResult> selector);
    }

    public interface IIntermediate2<T1, T2> : IResult<T2>
    {
        IIntermediate3<T1, T2, TResult> From<TResult>(Func<Option<TResult>> f3);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, TResult> selector);
    }

    public interface IIntermediate3<T1, T2, T3> : IResult<T3>
    {
        IIntermediate4<T1, T2, T3, TResult> From<TResult>(Func<Option<TResult>> f4);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, TResult> selector);
    }

    public interface IIntermediate4<T1, T2, T3, T4> : IResult<T4>
    {
        IIntermediate5<T1, T2, T3, T4, TResult> From<TResult>(Func<Option<TResult>> f5);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, TResult> selector);
    }

    public interface IIntermediate5<T1, T2, T3, T4, T5> : IResult<T5>
    {
        IIntermediate6<T1, T2, T3, T4, T5, TResult> From<TResult>(Func<Option<TResult>> f6);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, T5, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, T5, TResult> selector);
    }

    public interface IIntermediate6<T1, T2, T3, T4, T5, T6> : IResult<T6>
    {
        IIntermediate7<T1, T2, T3, T4, T5, T6, TResult> From<TResult>(Func<Option<TResult>> f7);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, T5, T6, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> selector);
    }

    public interface IIntermediate7<T1, T2, T3, T4, T5, T6, T7> : IResult<T7>
    {
        IIntermediate8<T1, T2, T3, T4, T5, T6, T7, TResult> From<TResult>(Func<Option<TResult>> f8);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector);
    }

    public interface IIntermediate8<T1, T2, T3, T4, T5, T6, T7, T8> : IResult<T8>
    {
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector);
    }

    public class Intermediate4<T1, T2, T3, T4> : IIntermediate4<T1, T2, T3, T4>
    {
        private readonly Func<Option<T1>> _f1;
        private readonly Func<Option<T2>> _f2;
        private readonly Func<Option<T3>> _f3;
        private readonly Func<Option<T4>> _f4;

        public Intermediate4(
            Func<Option<T1>> f1,
            Func<Option<T2>> f2,
            Func<Option<T3>> f3,
            Func<Option<T4>> f4)
        {
            _f1 = f1;
            _f2 = f2;
            _f3 = f3;
            _f4 = f4;
        }

        public Option<T4> Result()
        {
            return _f1()
                .Bind(x => _f2())
                .Bind(x => _f3())
                .Bind(x => _f4());
        }

        public IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, Option<TResult>> selector)
        {
            var result = Fun.Create(() => _f1()
                .Bind(v1 => _f2()
                    .Bind(v2 => _f3()
                        .Bind(v3 => _f4()
                            .Bind(v4 => selector(v1, v2, v3, v4))))));

            return new Intermediate1<TResult>(result);
        }

        public IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, TResult> selector)
        {
            var result = Fun.Create(() => _f1()
                .Bind(v1 => _f2()
                    .Bind(v2 => _f3()
                        .Bind(v3 => _f4()
                            .Select(v4 => selector(v1, v2, v3, v4))))));

            return new Intermediate1<TResult>(result);
        }

        public IIntermediate5<T1, T2, T3, T4, TResult> From<TResult>(Func<Option<TResult>> f6)
        {
            return new Intermediate5<T1, T2, T3, T4, TResult>(_f1, _f2, _f3, _f4, f6);
        }
    }

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

    public class Intermediate6<T1, T2, T3, T4, T5, T6> : IIntermediate6<T1, T2, T3, T4, T5, T6>
    {
        private readonly Func<Option<T1>> _f1;
        private readonly Func<Option<T2>> _f2;
        private readonly Func<Option<T3>> _f3;
        private readonly Func<Option<T4>> _f4;
        private readonly Func<Option<T5>> _f5;
        private readonly Func<Option<T6>> _f6;

        public Intermediate6(
            Func<Option<T1>> f1,
            Func<Option<T2>> f2,
            Func<Option<T3>> f3,
            Func<Option<T4>> f4,
            Func<Option<T5>> f5,
            Func<Option<T6>> f6)
        {
            _f1 = f1;
            _f2 = f2;
            _f3 = f3;
            _f4 = f4;
            _f5 = f5;
            _f6 = f6;
        }

        public Option<T6> Result()
        {
            return _f1()
                .Bind(x => _f2())
                .Bind(x => _f3())
                .Bind(x => _f4())
                .Bind(x => _f5())
                .Bind(x => _f6());
        }

        public IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, T5, T6, Option<TResult>> selector)
        {
            var result = Fun.Create(() => _f1()
                .Bind(v1 => _f2()
                    .Bind(v2 => _f3()
                        .Bind(v3 => _f4()
                            .Bind(v4 => _f5()
                                .Bind(v5 => _f6()
                                    .Bind(v6 => selector(v1, v2, v3, v4, v5, v6))))))));

            return new Intermediate1<TResult>(result);
        }

        public IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            var result = Fun.Create(() => _f1()
                .Bind(v1 => _f2()
                    .Bind(v2 => _f3()
                        .Bind(v3 => _f4()
                            .Bind(v4 => _f5()
                                .Bind(v5 => _f6()
                                    .Select(v6 => selector(v1, v2, v3, v4, v5, v6))))))));

            return new Intermediate1<TResult>(result);
        }

        public IIntermediate7<T1, T2, T3, T4, T5, T6, TResult> From<TResult>(Func<Option<TResult>> f7)
        {
            return new Intermediate7<T1, T2, T3, T4, T5, T6, TResult>(_f1, _f2, _f3, _f4, _f5, _f6, f7);
        }
    }

    public class Intermediate7<T1, T2, T3, T4, T5, T6, T7> : IIntermediate7<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly Func<Option<T1>> _f1;
        private readonly Func<Option<T2>> _f2;
        private readonly Func<Option<T3>> _f3;
        private readonly Func<Option<T4>> _f4;
        private readonly Func<Option<T5>> _f5;
        private readonly Func<Option<T6>> _f6;
        private readonly Func<Option<T7>> _f7;

        public Intermediate7(
            Func<Option<T1>> f1,
            Func<Option<T2>> f2,
            Func<Option<T3>> f3,
            Func<Option<T4>> f4,
            Func<Option<T5>> f5,
            Func<Option<T6>> f6,
            Func<Option<T7>> f7)
        {
            _f1 = f1;
            _f2 = f2;
            _f3 = f3;
            _f4 = f4;
            _f5 = f5;
            _f6 = f6;
            _f7 = f7;
        }

        public Option<T7> Result()
        {
            return _f1()
                .Bind(x => _f2())
                .Bind(x => _f3())
                .Bind(x => _f4())
                .Bind(x => _f5())
                .Bind(x => _f6())
                .Bind(x => _f7());
        }

        public IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, Option<TResult>> selector)
        {
            var result = Fun.Create(() => _f1()
                .Bind(v1 => _f2()
                    .Bind(v2 => _f3()
                        .Bind(v3 => _f4()
                            .Bind(v4 => _f5()
                                .Bind(v5 => _f6()
                                    .Bind(v6 => _f7()
                                        .Bind(v7 => selector(v1, v2, v3, v4, v5, v6, v7)))))))));

            return new Intermediate1<TResult>(result);
        }

        public IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            var result = Fun.Create(() => _f1()
                .Bind(v1 => _f2()
                    .Bind(v2 => _f3()
                        .Bind(v3 => _f4()
                            .Bind(v4 => _f5()
                                .Bind(v5 => _f6()
                                    .Bind(v6 => _f7()
                                        .Select(v7 => selector(v1, v2, v3, v4, v5, v6, v7)))))))));

            return new Intermediate1<TResult>(result);
        }

        public IIntermediate8<T1, T2, T3, T4, T5, T6, T7, TResult> From<TResult>(Func<Option<TResult>> f8)
        {
            return new Intermediate8<T1, T2, T3, T4, T5, T6, T7, TResult>(_f1, _f2, _f3, _f4, _f5, _f6, _f7, f8);
        }
    }

    public class Intermediate8<T1, T2, T3, T4, T5, T6, T7, T8> : IIntermediate8<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        private readonly Func<Option<T1>> _f1;
        private readonly Func<Option<T2>> _f2;
        private readonly Func<Option<T3>> _f3;
        private readonly Func<Option<T4>> _f4;
        private readonly Func<Option<T5>> _f5;
        private readonly Func<Option<T6>> _f6;
        private readonly Func<Option<T7>> _f7;
        private readonly Func<Option<T8>> _f8;

        public Intermediate8(
            Func<Option<T1>> f1,
            Func<Option<T2>> f2,
            Func<Option<T3>> f3,
            Func<Option<T4>> f4,
            Func<Option<T5>> f5,
            Func<Option<T6>> f6,
            Func<Option<T7>> f7,
            Func<Option<T8>> f8)
        {
            _f1 = f1;
            _f2 = f2;
            _f3 = f3;
            _f4 = f4;
            _f5 = f5;
            _f6 = f6;
            _f7 = f7;
            _f8 = f8;
        }

        public Option<T8> Result()
        {
            return _f1()
                .Bind(x => _f2())
                .Bind(x => _f3())
                .Bind(x => _f4())
                .Bind(x => _f5())
                .Bind(x => _f6())
                .Bind(x => _f7())
                .Bind(x => _f8());
        }

        public IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, Option<TResult>> selector)
        {
            var result = Fun.Create(() => _f1()
                .Bind(v1 => _f2()
                    .Bind(v2 => _f3()
                        .Bind(v3 => _f4()
                            .Bind(v4 => _f5()
                                .Bind(v5 => _f6()
                                    .Bind(v6 => _f7()
                                        .Bind(v7 => _f8()
                                            .Bind(v8 => selector(v1, v2, v3, v4, v5, v6, v7, v8))))))))));

            return new Intermediate1<TResult>(result);
        }

        public IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector)
        {
            var result = Fun.Create(() => _f1()
                .Bind(v1 => _f2()
                    .Bind(v2 => _f3()
                        .Bind(v3 => _f4()
                            .Bind(v4 => _f5()
                                .Bind(v5 => _f6()
                                    .Bind(v6 => _f7()
                                        .Bind(v7 => _f8()
                                            .Select(v8 => selector(v1, v2, v3, v4, v5, v6, v7, v8))))))))));

            return new Intermediate1<TResult>(result);
        }
    }

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
