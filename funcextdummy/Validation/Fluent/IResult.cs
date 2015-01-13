namespace FunctionalExtensions.Validation.Fluent
{
    public interface IResult<T, TError>
    {
        Choice<T, Failure<TError>> Result { get; }
    }
}
