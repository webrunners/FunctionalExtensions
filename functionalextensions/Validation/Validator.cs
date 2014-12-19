using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtensions.Validation
{
    public static class Validator
    {
        public static Choice<T, Errors<TError>> NotNull<T, TError>(T value, TError err) where T : class
        {
            return value != null ? Result.Ok<T, TError>(value) : Result.Error<T, TError>(err);
        }

        public static Choice<T, Errors<TError>> NotEqual<T, TError>(T value, T pattern, TError err) where T : class
        {
            return !value.Equals(pattern) ? Result.Ok<T, TError>(value) : Result.Error<T, TError>(err);
        }

        public static Choice<string, Errors<TError>> NotNullOrEmpty<TError>(string value, TError err)
        {
            return !String.IsNullOrEmpty(value) ? Result.Ok<string, TError>(value) : Result.Error<string, TError>(err);
        }

        public static Func<T, Choice<T, Errors<TError>>> Create<T, TError>(Predicate<T> pred, TError err)
        {
            return x => pred(x) ? Result.Ok<T, TError>(x) : Result.Error<T, TError>(err);
        }

        public static Func<IEnumerable<T>, Choice<IEnumerable<T>, Errors<TError>>> EnumerableValidator<T, TError>(Func<T, Choice<T, Errors<TError>>> validateOrder)
        {
            var zero = Choice.NewChoice1Of2<IEnumerable<T>, Errors<TError>>(new List<T>());

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
