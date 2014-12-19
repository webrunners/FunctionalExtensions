namespace FunctionalExtensions.Validation
{
    public static class Result
    {
        public static Choice<T, Errors<TError>> Ok<T, TError>(T value)
        {
            return Choice.NewChoice1Of2<T, Errors<TError>>(value);
        }

        public static Choice<T, Errors<TError>> Error<T, TError>(TError message)
        {
            return Choice.NewChoice2Of2<T, Errors<TError>>(new Errors<TError>(message));
        }
    }
}
