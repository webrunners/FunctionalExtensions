using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public interface IIntermediate2<T, TError> : IResult<T, TError>
    {
        IIntermediate3<T, TError> And { get; }
        IIntermediate4<T, TResult, TError> AndSelect<TResult>(Func<T, TResult> selector) where TResult : class;
    }
}