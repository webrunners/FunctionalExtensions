using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public interface IIntermediate4<T, out TResult, TError>
    {
        IIntermediate2<T, TError> IsNotNull(TError error);
        IIntermediate2<T, TError> Fulfills(Predicate<TResult> predicate, TError error);
    }
}