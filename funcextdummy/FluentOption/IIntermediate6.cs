using System;

namespace FunctionalExtensions.FluentOption
{
    public interface IIntermediate6<T1, T2, T3, T4, T5, T6> : IResult<T6>
    {
        IIntermediate7<T1, T2, T3, T4, T5, T6, TResult> From<TResult>(Func<Option<TResult>> f7);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, T5, T6, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> selector);
    }
}