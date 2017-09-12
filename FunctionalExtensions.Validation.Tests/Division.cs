using System;

namespace FunctionalExtensions.Validation.Tests
{
    public static class Division
    {
        internal static Option<decimal> Divide(decimal a, decimal b)
        {
            try
            {
                return Option.Some(a / b);
            }
            catch (DivideByZeroException)
            {
                return Option.None<decimal>();
            }
        }
    }
}
