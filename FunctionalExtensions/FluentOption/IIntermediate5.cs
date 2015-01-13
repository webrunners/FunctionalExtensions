using System;

namespace FunctionalExtensions.FluentOption
{
    public interface IIntermediate5<T1, T2, T3, T4, T5> : IResult<T5>
    {
        IIntermediate6<T1, T2, T3, T4, T5, TResult> From<TResult>(Func<Option<TResult>> f6);
        IIntermediate1<TResult> Bind<TResult>(Func<T1, T2, T3, T4, T5, Option<TResult>> selector);
        IIntermediate1<TResult> Select<TResult>(Func<T1, T2, T3, T4, T5, TResult> selector);
    }
}