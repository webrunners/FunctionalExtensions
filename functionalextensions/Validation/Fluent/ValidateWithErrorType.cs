namespace FunctionalExtensions.Validation.Fluent
{
    public static class ValidateWithErrorType<TError>
    {
        public static IValidateThatInstance<T, TError> That<T>(T instance) where T : class
        {
            return new ValidateThatInstance<T, TError>(instance);
        }
    }
}