using System;

namespace FunctionalExtensions.Validation.Fluent
{
    public static class Validate
    {
        public static IIntermediate1<T, string> That<T>(T instance) where T : class
        {
            return new FluentValidator<T, string>(instance);
        }
    }
}