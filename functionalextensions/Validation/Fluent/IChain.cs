using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public interface IChain<T, TError> where T : class
    {
        IValidateThatInstance<T, TError> And { get; }
        IValidateThatMember<T, TResult, TError> AndMember<TResult>(Func<T, TResult> selector) where TResult : class;
        Choice<T, Errors<TError>> Result { get; }
    }
}