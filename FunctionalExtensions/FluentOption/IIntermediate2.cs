using System;

namespace FunctionalExtensions.FluentOption
{
    public interface IIntermediate2<T1, T2> : IResult<T2>
    {
        IIntermediate3<T1, T2, TResult> From<TResult>(Func<Option<TResult>> f3);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, TResult> selector);
    }
}