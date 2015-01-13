using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public interface IIntermediate2A<T, TError> : IResult<T, TError>
    {
        IIntermediate4<T, TResult, TError> AndSelect<TResult>(Func<T, TResult> selector) where TResult : class;
    }
}