using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public interface IValidateThatMember<T, out TMember, TError>
        where T : class
        where TMember : class
    {
        IChain<T, TError> IsNotNull(TError err);
        IChain<T, TError> Fulfills(Func<TMember, bool> pred, TError err);
    }
}