using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public interface IValidateThatInstance<T, TError> : IValidateThat<T, TError> where T : class
    {
        IChain<T, TError> Fulfills(Predicate<T> pred, TError err);
        IChain<T, TError> IsEqualTo(T pattern, TError err);
        IValidateThatMember<T, TMember, TError> Member<TMember>(Func<T, TMember> selector) where TMember : class;
    }
}