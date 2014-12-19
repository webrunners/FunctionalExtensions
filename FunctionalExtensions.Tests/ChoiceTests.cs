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
            var result = 0.0m;
            var d1 = Fun.Create(() => Choice.NewChoice1Of2<decimal, string>(2.5m));
            var d2 = Fun.Create(() => Choice.NewChoice1Of2<decimal, string>(2.5m));

            var callbackSome = Act.Create((decimal x) => result = x);

            var callbackNone = new Action<string>(x => Assert.Fail());

            ValidationWithChoiceMonad(d1, d2, callbackSome, callbackNone);
            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void ValidationWithChoiceMonad_RainyDay_Test()
        {
            var d1 = Fun.Create(() => Choice.NewChoice1Of2<decimal, string>(2.5m));
            var ex1 = Fun.Create(() => Choice.NewChoice2Of2<decimal, string>("Error1"));
            var ex2 = Fun.Create(() => Choice.NewChoice2Of2<decimal, string>("Error2"));
            var zero = Fun.Create(() => Choice.NewChoice1Of2<decimal, string>(0.0m));

            var error = String.Empty;
            Action<string> callbackNone = s => error = s;
            Action<decimal> callbackSome = x => Assert.Fail();

            ValidationWithChoiceMonad(d1, ex2, callbackSome, callbackNone);
            Assert.That(error, Is.EqualTo("Error2"));

            ValidationWithChoiceMonad(ex1, d1, callbackSome, callbackNone);
            Assert.That(error, Is.EqualTo("Error1"));

            ValidationWithChoiceMonad(d1, zero, callbackSome, callbackNone);
            Assert.That(error, Is.EqualTo("Cannot Devide by zero."));
        }

        private static void ValidationWithChoiceMonad(Func<Choice<decimal, string>> readdecimal1, Func<Choice<decimal, string>> readdecimal2, Action<decimal> callBackSome, Action<string> callBackNone)
        {
            (
                from v1 in readdecimal1()
                from v2 in readdecimal2()
                from devisionResult in Division.Divide(v1, v2).ToChoice("Cannot Devide by zero.")
                select devisionResult*100
            )
                .Match(callBackSome, callBackNone);
        }
    }
}
