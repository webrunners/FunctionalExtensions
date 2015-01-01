using System;

namespace FunctionalExtensions.FluentOption
{
    public interface IIntermediate7<T1, T2, T3, T4, T5, T6, T7> : IResult<T7>
    {
        IIntermediate8<T1, T2, T3, T4, T5, T6, T7, TResult> From<TResult>(Func<Option<TResult>> f8);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector);
    }
}