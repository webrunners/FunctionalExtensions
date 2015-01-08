using System;
using System.Collections.Generic;
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
            var result = 0.0m;
            var d1 = Fun.Create(() => Option.Some(2.5m));
            var d2 = Fun.Create(() => Option.Some(2.5m));

            var callbackSome = Fun.Create((decimal x) =>
            {
                result = x;
                return true;
            });

            var callbackNone = Fun.Create(() => false);

            ValidationWithOptionMonad(d1, d2, Division.Divide, callbackSome, callbackNone);
            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void ValidationWithOptionMonad_RainyDay_Test()
        {
            var isCalled = false;
            var d1 = Fun.Create(() => Option.Some(2.5m));
            var zero = Fun.Create(() => Option.Some(0.0m));
            var ex = Fun.Create(Option.None<decimal>);

            var callbackNone = Fun.Create(() => isCalled = true);

            var callbackSome = Fun.Create((decimal x) =>
            {
                Assert.Fail();
                return false;
            });

            isCalled = false;
            ValidationWithOptionMonad(d1, ex, Division.Divide, callbackSome, callbackNone);
            Assert.That(isCalled, Is.EqualTo(true));

            isCalled = false;
            ValidationWithOptionMonad(ex, d1, Division.Divide, callbackSome, callbackNone);
            Assert.That(isCalled, Is.EqualTo(true));

            isCalled = false;
            ValidationWithOptionMonad(d1, zero, Division.Divide, callbackSome, callbackNone);
            Assert.That(isCalled, Is.EqualTo(true));
        }

        private static void ValidationWithOptionMonad(Func<Option<decimal>> readdecimal1, Func<Option<decimal>> readdecimal2, Func<decimal, decimal, Option<decimal>> divide, Func<decimal, bool> callBackSome, Func<bool> callBackNone)
        {
            var result = 
            (
                from v1 in readdecimal1()
                from v2 in readdecimal2()
                from x in divide(v1, v2)
                select x * 100
            )
                .Match(callBackSome, callBackNone);

            Assert.That(result, Is.True);
        }

        [Test]
        public void ToOption_Test()
        {
            var result = 5.ToOption()
                .Match(x => x, () => 0);

            Assert.That(result, Is.EqualTo(5));

            String s = null;

            var option2 = s.ToOption();

            Assert.That(option2.Match(x => false, () => true), Is.True);
        }

        [Test]
        public void Equals_Test()
        {
            Assert.That(Option.Some(42), Is.EqualTo(Option.Some(42)));
            Assert.That(Option.Some(42) == Option.Some(42));
            Assert.That(Option.Some(42) != Option.Some(24));
            Assert.That(Option.None<int>() != Option.Some(42));

            Option<int> nullOption1 = null;
            Option<int> nullOption2 = null;

            Assert.That(nullOption1 == nullOption2);

            Assert.That(Option.None<int>() == Option.None<int>());
            Assert.That(!Option.None<int>().Equals(Option.None<string>()));

            Assert.That(null == Option.None<int>());
            Assert.That(Option.None<int>() == null);
        }

        [Test]
        public void Monad_Laws_Test()
        {
            var k = Fun.Create((int x) => Option.Some(x * 2));

            // Left Identity
            Assert.That(42.ToOption().Bind(k), Is.EqualTo(k(42)));

            // Right Identity
            var m = Option.Some(42);
            Assert.That(m.Bind(x => x.ToOption()), Is.EqualTo(m));

            // Associativity
            var h = Fun.Create((int x) => Option.Some(x + 1));
            Assert.That(
                m.Bind(x => k(x).Bind(h)),
                Is.EqualTo(m.Bind(k).Bind(h)));
        }

        [Test]
        public void FirstOrOptionTest()
        {
            var list = new List<int>();

            Assert.That(list.FirstOrOption().Match(x => false, () => true), Is.True);

            list.AddRange(new[] { 1, 2, 3 });

            Assert.That(list.FirstOrOption(x => x > 3).Match(x => false, () => true), Is.True);
            Assert.That(list.FirstOrOption(x => x == 3).Match(x => x, () => -1), Is.EqualTo(3));
        }

        [Test]
        public void SingleOrOptionTest()
        {
            var list = new List<int>();

            Assert.That(list.SingleOrOption().Match(x => false, () => true), Is.True);

            list.AddRange(new[] { 1, 1, 2, 3 });

            Assert.That(list.SingleOrOption(x => x > 3).Match(x => false, () => true), Is.True);
            Assert.That(list.SingleOrOption(x => x == 3).Match(x => x, () => -1), Is.EqualTo(3));
            Assert.That(list.SingleOrOption(x => x == 1).Match(x => false, () => true), Is.True);
        }
    }
}
