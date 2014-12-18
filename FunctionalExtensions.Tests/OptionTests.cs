using System;
using NUnit.Framework;
using FunctionalExtensions.Lambda;

namespace FunctionalExtensions.Tests
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void ValidationWithOptionMonad_HappyDay_Test()
        {
            var result = 0.0;
            var d1 = Fun.Create(() => Option.Some(2.5));
            var d2 = Fun.Create(() => Option.Some(2.5));

            var callbackSome = Act.Create((double x) => result = x);

            var callbackNone = Act.Create(Assert.Fail);

            ValidationWithOptionMonad(d1, d2, Divide, callbackSome, callbackNone);
            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void ValidationWithOptionMonad_RainyDay_Test()
        {
            var isCalled = false;
            var d1 = Fun.Create(() => Option.Some(2.5));
            var zero = Fun.Create(() => Option.Some(0.0));
            var ex = Fun.Create(() => Option.None<double>());

            Action callbackNone = () => isCalled = true;
            var callbackSome = Act.Create((double x) => Assert.Fail());

            isCalled = false;
            ValidationWithOptionMonad(d1, ex, Divide, callbackSome, callbackNone);
            Assert.That(isCalled, Is.EqualTo(true));

            isCalled = false;
            ValidationWithOptionMonad(ex, d1, Divide, callbackSome, callbackNone);
            Assert.That(isCalled, Is.EqualTo(true));

            isCalled = false;
            ValidationWithOptionMonad(d1, zero, Divide, callbackSome, callbackNone);
            Assert.That(isCalled, Is.EqualTo(true));
        }

        private static void ValidationWithOptionMonad(Func<Option<double>> readDouble1, Func<Option<double>> readDouble2, Func<double, double, Option<Double>> divide, Action<double> callBackSome, Action callBackNone)
        {
            (
                from v1 in readDouble1()
                from v2 in readDouble2()
                from result in divide(v1, v2)
                select result*100
            )
                .Match(callBackSome, callBackNone);
        }

        private static Option<double> Divide(double a, double b)
        {
            return b == 0 ? Option.None<double>() : Option.Some(a / b);
        }
    }
}
