using System;

namespace FunctionalExtensions.FluentOption
{
    public interface IIntermediate8<T1, T2, T3, T4, T5, T6, T7, T8> : IResult<T8>
    {
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector);
    }
}