using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtensions.Validation
{
    public static class Validator
    {
        public static Choice<T, Failure<TError>> NotNull<T, TError>(T value, TError err) where T : class
        {
            return value != default(T) ? Result.Success<T, TError>(value) : Result.Failure<T, TError>(err);
        }

        public static Choice<T, Failure<TError>> NotEqual<T, TError>(T value, T pattern, TError err) where T : class
        {
            return !value.Equals(pattern) ? Result.Success<T, TError>(value) : Result.Failure<T, TError>(err);
        }

        public static Choice<T, Failure<TError>> Equal<T, TError>(T value, T pattern, TError err) where T : class
        {
            return value.Equals(pattern) ? Result.Success<T, TError>(value) : Result.Failure<T, TError>(err);
        }

        public static Choice<string, Failure<TError>> NotNullOrEmpty<TError>(string value, TError err)
        {
            return !String.IsNullOrEmpty(value) ? Result.Success<string, TError>(value) : Result.Failure<string, TError>(err);
        }

        public static Func<T, Choice<T, Failure<TError>>> Create<T, TError>(Predicate<T> pred, TError err)
        {
            return x => pred(x) ? Result.Success<T, TError>(x) : Result.Failure<T, TError>(err);
        }

        public static Func<IEnumerable<T>, Choice<IEnumerable<T>, Failure<TError>>> EnumerableValidator<T, TError>(Func<T, Choice<T, Failure<TError>>> validateOrder)
        {
            var zero = Choice.NewChoice1Of2<IEnumerable<T>, Failure<TError>>(new List<T>());

            return x => x
                .Select(validateOrder)
                .Aggregate(zero,
                    (e, c) =>
                        from a in e
                        join b in c on 1 equals 1
                        select a.Concat(new[] { b }));
        }
    }
}
