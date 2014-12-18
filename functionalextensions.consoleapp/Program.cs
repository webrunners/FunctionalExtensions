using FunctionalExtensions.Validation;
using System;
using System.Linq;

namespace FunctionalExtensions.ConsoleApp
{
    class Program
    {
        private static void Main(string[] args)
        {
            ValidationAppF();
        }

        private static void ValidationAppF()
        {
            Console.WriteLine("Enter a (floating point) number:");

            (
                from v1 in ReadDouble().ToChoice(new Errors("Could not parse input."))
                join v2 in ReadDouble().ToChoice(new Errors("Could not parse input.")) on 1 equals 1
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
                from v1 in ReadDouble()
                from v2 in ReadDouble()
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
                from v1 in ReadDouble().ToChoice("Could not parse input.")
                from v2 in ReadDouble().ToChoice("Could not parse input.")
                from devisionResult in Divide(v1, v2).ToChoice("Cannot Divide by zero.")
                select devisionResult*100
            )
                .Match(
                    x => Console.WriteLine("Result = {0} %", x.ToString("F")),
                    Console.WriteLine);
        }

        public static Option<double> Divide(double a, double b)
        {
            return b == 0 ? Option.None<double>() : Option.Some(a / b);
        }

        public static Option<Double> ReadDouble()
        {
            double i;
            return Double.TryParse(Console.ReadLine(), out i) ? Option.Some(i) : Option.None<double>();
        }
    }
}
