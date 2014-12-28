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
            return null;
        }
    }

    public interface IResult<T>
    {
        Option<T> Result();
    }

    public interface IIntermediate1<T1> : IResult<T1>
    {
        IIntermediate2<T1, TResult> From<TResult>(Func<Option<TResult>> f);
        IIntermediate2<T1, TResult> From<TResult>(Func<T1, Option<TResult>> f);
    }

    public interface IIntermediate2<T1, T2> : IResult<T2>
    {
        IIntermdiate3<T1, T2, TResult> From<TResult>(Func<T1, T2, Option<TResult>> f);
    }

    public interface IIntermdiate3<T1, T2, T3> : IResult<T3>
    {

    }
}
