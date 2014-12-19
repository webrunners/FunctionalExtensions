using FunctionalExtensions.Attributes;
using FunctionalExtensions.Extensions;
using FunctionalExtensions.Validation;
using System;
using System.Linq;

namespace FunctionalExtensions.ConsoleApp
{
    class Program
    {
        private static void Main()
        {
            ValidationWithOptionMonad();
            Console.WriteLine();

            ValidationWithChoiceMonad();
            Console.WriteLine();

            ValidationAppF();
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

        private static void ValidationAppF()
        {
            Console.WriteLine("Enter a (floating point) number:");

            (
                from v1 in ReadDecimal().ToChoice(new Errors<Error>(Error.CannotNotParse1StInput))
                join v2 in ReadDecimal().ToChoice(new Errors<Error>(Error.CannotNotParse2NdInput)) on 1 equals 1
                from result in Divide(v1, v2).ToChoice(new Errors<Error>(Error.CannotDivideByZero))
                select result
                )
                .Match(
                    x => Console.WriteLine("Result = {0}", x),
                    err => err.Get.ToList().ForEach(x => Console.WriteLine(x.GetDisplayName())));
        }


        private static void ValidationWithOptionMonad()
        {
            Console.WriteLine("Enter two (floating point) numbers:");

            (
                from v1 in ReadDecimal()
                from v2 in ReadDecimal()
                from result in Divide(v1, v2)
                select result*100
            )
                .Match(
                    x => Console.WriteLine("Result = {0} %", x.ToString("F")),
                    () => Console.WriteLine("An error occurred."));
        }

        private static void ValidationWithChoiceMonad()
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

        internal static Option<decimal> Divide(decimal a, decimal b)
        {
            try { return Option.Some(a / b); }
            catch (DivideByZeroException) { return Option.None<decimal>(); }
        }

        public static Option<decimal> ReadDecimal()
        {
            decimal i;
            return decimal.TryParse(Console.ReadLine(), out i) ? Option.Some(i) : Option.None<decimal>();
        }
    }
}
