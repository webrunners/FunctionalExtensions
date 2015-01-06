using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public interface IIntermediate4<T, out TResult, TError>
    {
        IIntermediate2A<T, TError> IsNotNull(TError error);

        IIntermediate2A<T, TError> Fulfills(
            Predicate<TResult> predicate, 
            Func<TResult, TError> error, 
            Func<NullReferenceException, TError> onNullReferenceException = null,
            Func<ArgumentOutOfRangeException, TError> onArgumentOutOfRangeException = null);

        IIntermediate2A<T, TError> Fulfills(
            Predicate<TResult> predicate,
            TError error, 
            Func<NullReferenceException, TError> onNullReferenceException = null,
            Func<ArgumentOutOfRangeException, TError> onArgumentOutOfRangeException = null);
    }
}