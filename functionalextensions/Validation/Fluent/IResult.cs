namespace FunctionalExtensions.Validation.Fluent
{
    public interface IResult<T, TError>
    {
        Choice<T, Failures<TError>> Result { get; }
    }
}
