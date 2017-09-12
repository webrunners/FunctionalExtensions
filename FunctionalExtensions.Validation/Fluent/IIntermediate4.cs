using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public interface IIntermediate4<T, out TResult, TError>
    {
        IIntermediate2A<T, TError> IsNotNull(TError error);
        IIntermediate2A<T, TError> Fulfills(Predicate<TResult> predicate, Func<TResult, TError> onError, Func<Exception, TError> onException = null);
        IIntermediate2A<T, TError> Fulfills(Predicate<TResult> predicate, TError error, Func<Exception, TError> onException = null);
    }
}