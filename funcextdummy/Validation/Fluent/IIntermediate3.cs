using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public interface IIntermediate3<T, TError>
    {
        IIntermediate2<T, TError> Fulfills(Predicate<T> predicate, TError error, Func<Exception, TError> onException = null);
        IIntermediate2<T, TError> Fulfills(Predicate<T> predicate, Func<T, TError> onError, Func<Exception, TError> onException = null);
    }
}