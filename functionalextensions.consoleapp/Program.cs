using FunctionalExtensions.Attributes;
using FunctionalExtensions.Extensions;
using FunctionalExtensions.FluentOption;
using FunctionalExtensions.Lambda;
using FunctionalExtensions.Validation;
using System;
using System.Linq;

namespace FunctionalExtensions.ConsoleApp
{
    class Program
    {
        private static void Main()
        {
            var result = Fun.Create((decimal x, decimal y) => x/y)
                .ReturnOption()
                .OnExceptionNone()
                .Curry()
                .Bind(ReadDecimal())
                .Apply(ReadDecimal())
                .Match(
                    x => String.Format("Result = {0}", x.ToString("F")),
                    () => "An error occurred.");

            Console.WriteLine(result);
        }

        private static void ValidationWithFluentOption()
        {
            Console.WriteLine("Enter two (floating point) numbers:");

            var result = OptionMonad
                .From(ReadDecimal)
                .From(ReadDecimal)
                .Bind(Divide)
                .Result()
                .Match(
                    x => String.Format("Result = {0}", x.ToString("F")),
                    () => "An error occurred.");

            Console.WriteLine(result);
        }

        enum Error
        {
            [EnumDisplayName("Cannot parse 1st input.")]
            CannotNotParse1StInput,
            [EnumDisplayName("Cannot parse 2st input.")]
            CannotNotParse2NdInput,
            [EnumDisplayName("Cannot divide by zero.")]
            CannotDivideByZero
        }

        private static void ValidationWithResult()
        {
            Console.WriteLine("Enter a (floating point) number:");

            (
                from v1 in ReadDecimal().ToChoice(Failure.Create(Error.CannotNotParse1StInput))
                join v2 in ReadDecimal().ToChoice(Failure.Create(Error.CannotNotParse2NdInput)) on 1 equals 1
                from result in Divide(v1, v2).ToChoice(Failure.Create(Error.CannotDivideByZero))
                select result
            )
                .Match(
                    x => Console.WriteLine("Result = {0}", x),
                    err => err.Errors.ToList().ForEach(x => Console.WriteLine(x.GetDisplayName())));
        }


        private static void ValidationWithOption()
        {
            Console.WriteLine("Enter two (floating point) numbers:");

            var output = 
                (
                    from v1 in ReadDecimal()
                    from v2 in ReadDecimal()
                    from result in Divide(v1, v2)
                    select result*100
                )
                    .Match(
                        x => String.Format("Result = {0} %", x.ToString("F")),
                        () => "An error occurred.");

            Console.WriteLine(output);
        }

        private static void ValidationWithChoice()
        {
            Console.WriteLine("Enter two (floating point) numbers:");

            (
                from v1 in ReadDecimal().ToChoice("Could not parse input.")
                from v2 in ReadDecimal().ToChoice("Could not parse input.")
                from devisionResult in Divide(v1, v2).ToChoice("Cannot Divide by zero.")
                select devisionResult*100
            )
                .Match(
                    x => Console.WriteLine("Result = {0} %", x.ToString("F")),
                    Console.WriteLine);
        }

        private static Option<decimal> Divide(decimal a, decimal b)
        {
            try { return Option.Some(a / b); }
            catch (DivideByZeroException) { return Option.None<decimal>(); }
        }

        private static Option<decimal> ReadDecimal()
        {
            decimal i;
            return decimal.TryParse(Console.ReadLine(), out i) ? Option.Some(i) : Option.None<decimal>();
        }
    }
}
