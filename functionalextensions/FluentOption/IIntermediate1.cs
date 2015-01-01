using System;

namespace FunctionalExtensions.FluentOption
{
    public interface IIntermediate1<T1> : IResult<T1>
    {
        IIntermediate2<T1, TResult> From<TResult>(Func<Option<TResult>> f2);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, TResult> selector);
    }
}