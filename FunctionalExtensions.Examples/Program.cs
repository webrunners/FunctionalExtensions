using System;
using System.Linq;
using EnumExtensions.Attributes;
using EnumExtensions.Extensions;
using FunctionalExtensions.Currying;
using FunctionalExtensions.Lambda;
using FunctionalExtensions.SideEffects;
using FunctionalExtensions.Validation;

namespace FunctionalExtensions.Examples
{
    class Program
    {
        private static void Main()
        {
            Console.WriteLine("Option Applicative:");
            OptionApplicative();
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Option Monad:");
            OptionMonad();
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Option Monad (Fluent Interface):");
            OptionFluentInterface();
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Choice Monad:");
            ChoiceMonad();
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Choice Applicative:");
            ChoiceApplicative();
            Console.WriteLine(Environment.NewLine);
        }

        private static void OptionApplicative()
        {
            var result = Fun.Create((decimal x, decimal y) => x/y)
                .ReturnOption()
                .OnExceptionNone()
                .Curry()
                .ToOption()
                .Do(() => Console.Write("Enter a flaoting point number: "))
                .Apply(ReadDecimal())
                .Do(() => Console.Write("Enter a flaoting point number: "))
                .Apply(ReadDecimal())
                .Select(x => x * 100)
                .Match(
                    x => String.Format("Result = {0} %", x.ToString("F")),
                    () => "An error occurred.");

            Console.WriteLine(result);
        }

        private static void OptionMonad()
        {
            Console.WriteLine("Enter two (floating point) numbers:");

            var output =
                (
                    from v1 in ReadDecimal()
                    from v2 in ReadDecimal()
                    from result in Divide(v1, v2)
                    select result * 100
                )
                    .Match(
                        x => String.Format("Result = {0} %", x.ToString("F")),
                        () => "An error occurred.");

            Console.WriteLine(output);
        }

        private static void OptionFluentInterface()
        {
            Console.WriteLine("Enter two (floating point) numbers:");

            var result = FluentOption.OptionMonad
                .From(ReadDecimal)
                .From(ReadDecimal)
                .Bind(Divide)
                .Result()
                .Match(
                    x => String.Format("Result = {0}", x.ToString("F")),
                    () => "An error occurred.");

            Console.WriteLine(result);
        }

        private static void ChoiceMonad()
        {
            Console.WriteLine("Enter two (floating point) numbers:");

            (
                from v1 in ReadDecimal().ToChoice("Could not parse input.")
                from v2 in ReadDecimal().ToChoice("Could not parse input.")
                from devisionResult in Divide(v1, v2).ToChoice("Cannot Divide by zero.")
                select devisionResult * 100
            )
                .Match(
                    x => Console.WriteLine("Result = {0} %", x.ToString("F")),
                    Console.WriteLine);
        }

        private static void ChoiceApplicative()
        {
            Console.WriteLine("Enter two (floating point) number:");
            
            (
                from v1 in ReadDecimal().ToChoice(Failure.Create(Error.CannotNotParse1StInput))
                join v2 in ReadDecimal().ToChoice(Failure.Create(Error.CannotNotParse2NdInput)) on 1 equals 1
                from result in Divide(v1, v2).ToChoice(Failure.Create(Error.CannotDivideByZero))
                select result
            )
                .Match(
                    x => Console.WriteLine("Result = {0}", x),
                    errors => errors.ToList().ForEach(x => Console.WriteLine(x.GetDisplayName())));
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

    enum Error
    {
        [EnumDisplayName("Cannot parse 1st input.")]
        CannotNotParse1StInput,
        [EnumDisplayName("Cannot parse 2st input.")]
        CannotNotParse2NdInput,
        [EnumDisplayName("Cannot divide by zero.")]
        CannotDivideByZero
    }
}
