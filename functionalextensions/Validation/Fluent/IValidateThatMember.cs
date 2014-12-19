using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public interface IValidateThatMember<T, out TMember, TError> : IValidateThat<T, TError>
        where T : class
        where TMember : class
    {
        IChain<T, TError> Fulfills(Predicate<TMember> pred, TError err);
    }
}