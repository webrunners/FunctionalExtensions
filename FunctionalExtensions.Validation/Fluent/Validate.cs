using System;

namespace FunctionalExtensions.Validation.Fluent
{
    [Obsolete("The Validation framework will be moved to another package.")]
    public static class Validate
    {
        public static IIntermediate1<T, string> That<T>(T instance) where T : class
        {
            return new FluentValidator<T, string>(instance);
        }
    }
}