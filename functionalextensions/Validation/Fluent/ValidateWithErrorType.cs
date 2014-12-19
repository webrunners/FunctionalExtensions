namespace FunctionalExtensions.Validation.Fluent
{
    public static class ValidateWithErrorType<TError>
    {
        public static IValidateThat<T, TError> That<T>(T instance) where T : class
        {
            return new ValidateThat<T, TError>(instance);
        }
    }
}