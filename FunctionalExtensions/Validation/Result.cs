using System;

namespace FunctionalExtensions.Validation
{
    [Obsolete("The Validation framework will be moved to another package.")]
    public static class Result
    {
        public static Choice<T, Failure<TError>> Success<T, TError>(T value)
        {
            return Choice.NewChoice1Of2<T, Failure<TError>>(value);
        }

        public static Choice<T, Failure<TError>> Failure<T, TError>(TError message)
        {
            return Choice.NewChoice2Of2<T, Failure<TError>>(Validation.Failure.Create(message));
        }
    }
}
