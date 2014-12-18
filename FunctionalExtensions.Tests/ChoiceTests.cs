using System;
using NUnit.Framework;
using FunctionalExtensions.Lambda;

namespace FunctionalExtensions.Tests
{
    [TestFixture]
    public class ChoiceTests
    {
        [Test]
        public void ValidationWithChoiceMonad_HappyDay_Test()
        {
            var result = 0.0;
            var d1 = Fun.Create(() => Choice.NewChoice1Of2<double, string>(2.5));
            var d2 = Fun.Create(() => Choice.NewChoice1Of2<double, string>(2.5));

            var callbackSome = new Action<double>(x => result = x);

            var callbackNone = new Action<string>(x => Assert.Fail());

            ValidationWithChoiceMonad(d1, d2, callbackSome, callbackNone);
            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void ValidationWithChoiceMonad_RainyDay_Test()
        {
            var d1 = Fun.Create(() => Choice.NewChoice1Of2<double, string>(2.5));
            var ex1 = Fun.Create(() => Choice.NewChoice2Of2<double, string>("Error1"));
            var ex2 = Fun.Create(() => Choice.NewChoice2Of2<double, string>("Error2"));
            var zero = Fun.Create(() => Choice.NewChoice1Of2<double, string>(0.0));

            var error = String.Empty;
            Action<string> callbackNone = s => error = s;
            Action<double> callbackSome = x => Assert.Fail();

            ValidationWithChoiceMonad(d1, ex2, callbackSome, callbackNone);
            Assert.That(error, Is.EqualTo("Error2"));

            ValidationWithChoiceMonad(ex1, d1, callbackSome, callbackNone);
            Assert.That(error, Is.EqualTo("Error1"));

            ValidationWithChoiceMonad(d1, zero, callbackSome, callbackNone);
            Assert.That(error, Is.EqualTo("Cannot Devide by zero."));
        }

        private static void ValidationWithChoiceMonad(Func<Choice<double, string>> readDouble1, Func<Choice<double, string>> readDouble2, Action<double> callBackSome, Action<string> callBackNone)
        {
            (
                from v1 in readDouble1()
                from v2 in readDouble2()
                from devisionResult in Divide(v1, v2).ToChoice("Cannot Devide by zero.")
                select devisionResult*100
            )
                .Match(callBackSome, callBackNone);
        }

        private static Option<double> Divide(double a, double b)
        {
            return b == 0 ? Option.None<double>() : Option.Some(a / b);
        }
    }
}
