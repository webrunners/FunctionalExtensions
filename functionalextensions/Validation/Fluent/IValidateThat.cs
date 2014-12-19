namespace FunctionalExtensions.Validation.Fluent
{
    public interface IValidateThat<T, TError> where T : class
    {
        IChain<T, TError> IsNotNull(TError err);
    }
}
