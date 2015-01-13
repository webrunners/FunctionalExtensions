using System;

namespace FunctionalExtensions.FluentOption
{
    public interface IIntermediate3<T1, T2, T3> : IResult<T3>
    {
        IIntermediate4<T1, T2, T3, TResult> From<TResult>(Func<Option<TResult>> f4);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, TResult> selector);
    }
}