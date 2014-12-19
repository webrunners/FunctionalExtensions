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

        private static void ValidationAppF()
        {
            Console.WriteLine("Enter a (floating point) number:");

            (
                from v1 in ReadDecimal().ToChoice(new Errors("Could not parse 1st input."))
                join v2 in ReadDecimal().ToChoice(new Errors("Could not parse 2nd input.")) on 1 equals 1
                from result in Divide(v1, v2).ToChoice(new Errors("Cannot devide by zero."))
                select result
                )
                .Match(
                    x => Console.WriteLine("Result = {0}", x),
                    err => err.Messages.ToList().ForEach(Console.WriteLine));
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
