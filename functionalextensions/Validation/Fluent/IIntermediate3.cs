using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public interface IIntermediate3<T, TError>
    {
        IIntermediate2<T, TError> Fulfills(Predicate<T> predicate, TError error);
    }
}