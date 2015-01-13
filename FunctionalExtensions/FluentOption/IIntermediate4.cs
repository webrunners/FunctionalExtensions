using System;

namespace FunctionalExtensions.FluentOption
{
    public interface IIntermediate4<T1, T2, T3, T4> : IResult<T4>
    {
        IIntermediate5<T1, T2, T3, T4, TResult> From<TResult>(Func<Option<TResult>> f5);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, TResult> selector);
    }
}