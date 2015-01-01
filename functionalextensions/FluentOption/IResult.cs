namespace FunctionalExtensions.FluentOption
{
    public interface IResult<T>
    {
        Option<T> Result();
    }
}