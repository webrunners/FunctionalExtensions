using FunctionalExtensions.Lambda;
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
        IIntermediate2<T1, TResult> From<TResult>(Func<Option<TResult>> f);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, TResult> selector);
    }

    public interface IIntermediate2<T1, T2> : IResult<T2>
    {
        IIntermediate3<T1, T2, TResult> From<TResult>(Func<Option<TResult>> f);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, TResult> selector);
    }

    public interface IIntermediate3<T1, T2, T3> : IResult<T3>
    {

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
    }
}
