namespace FunctionalExtensions.Validation
{
    public static class Result
    {
        public static Choice<T, Failures<TError>> Success<T, TError>(T value)
        {
            return Choice.NewChoice1Of2<T, Failures<TError>>(value);
        }

        public static Choice<T, Failures<TError>> Failure<T, TError>(TError message)
        {
            return Choice.NewChoice2Of2<T, Failures<TError>>(new Failures<TError>(message));
        }
    }
}
