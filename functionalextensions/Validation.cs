using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtensions
{
    public static class Validation
    {
        public static Choice<T, Errors> NonNull<T>(T value, string err) where T : class
        {
            return value == null ? Result.Error<T>(err) : Result.Ok(value);
        }

        public static Choice<T, Errors> NotEqual<T>(T value, T pattern, string err) where T : class
        {
            return value.Equals(pattern) ? Result.Error<T>(err) : Result.Ok(value);
        }

        public static Choice<string, Errors> IsNullOrEmpty(string value, string err)
        {
            return String.IsNullOrEmpty(value) ? Result.Error<string>(err) : Result.Ok(value);
        }

        public static Func<T, Choice<T, Errors>> Validator<T>(Predicate<T> pred, string err)
        {
            return x => pred(x) ? Result.Ok(x) : Result.Error<T>(err);
        }

        public static Func<IEnumerable<T>, Choice<IEnumerable<T>, Errors>> EnumerableValidator<T>(Func<T, Choice<T, Errors>> validateOrder)
        {
            var zero = Choice.NewChoice1Of2<IEnumerable<T>, Errors>(new List<T>());

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
