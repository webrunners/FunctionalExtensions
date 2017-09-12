namespace FunctionalExtensions.Validation.Fluent
{
    public interface IIntermediate1<T, TError>
    {
        IIntermediate2<T, TError> IsNotNull(TError error);
    }
}