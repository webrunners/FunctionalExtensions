using System;
using NUnit.Framework;

namespace FunctionalExtensions.Tests
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void ValidationWithOptionMonad_HappyDay_Test()
        {
            var result = 0.0;
            var d1 = Lambda.Create(() => Option.Some(2.5));
            var d2 = Lambda.Create(() => Option.Some(2.5));

            var callbackSome = Lambda.Create((double x) => result = x);

            var callbackNone = Lambda.Create(Assert.Fail);

            // happy day
            ValidationWithOptionMonad(d1, d2, Devide, callbackSome, callbackNone);
            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void ValidationWithOptionMonad_RainyDay_Test()
        {
            var isCalled = false;
            var d1 = Lambda.Create(() => Option.Some(2.5));
            var zero = Lambda.Create(() => Option.Some(0.0));
            var ex = Lambda.Create(() => Option.None<double>());

            // errors
            Action callbackNone = () => isCalled = true;
            Action<double> callbackSome = x => Assert.Fail();

            isCalled = false;
            ValidationWithOptionMonad(d1, ex, Devide, callbackSome, callbackNone);
            Assert.That(isCalled, Is.EqualTo(true));

            isCalled = false;
            ValidationWithOptionMonad(ex, d1, Devide, callbackSome, callbackNone);
            Assert.That(isCalled, Is.EqualTo(true));

            isCalled = false;
            ValidationWithOptionMonad(d1, zero, Devide, callbackSome, callbackNone);
            Assert.That(isCalled, Is.EqualTo(true));
        }

        private static void ValidationWithOptionMonad(Func<Option<double>> readDouble1, Func<Option<double>> readDouble2, Func<double, double, Option<Double>> devide, Action<double> callBackSome, Action callBackNone)
        {
            (
                from v1 in readDouble1()
                from v2 in readDouble2()
                from result in devide(v1, v2)
                select result*100
            )
                .Match(callBackSome, callBackNone);
        }

        private static Option<double> Devide(double a, double b)
        {
            return b == 0 ? Option.None<double>() : Option.Some(a / b);
        }
    }
}
