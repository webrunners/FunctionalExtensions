using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public static class ValidateWithErrorType<TError>
    {
        public static IIntermediate1<T, TError> That<T>(T instance) where T : class
        {
            return new FluentValidator<T, TError>(instance);
        }
    }
}