using System;
using System.Collections.Generic;
using FunctionalExtensions.Currying;
using FunctionalExtensions.Linq;
using FunctionalExtensions.Transform;
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
            var result = 5.ToOption().Match(x => x, () => 0);

            Assert.That(result, Is.EqualTo(5));

            String s = null;
            var option2 = s.ToOption();
            Assert.That(option2.Match(x => false, () => true), Is.True);
        }

        [Test]
        public void Null_Tests()
        {
            var opt1 = Option.Some<string>(null);
            Assert.That(opt1, Is.EqualTo(Option.None<string>()));

            var opt2 = Option.Return<string>(null);
            Assert.That(opt2, Is.EqualTo(Option.None<string>()));
        }

        [Test]
        public void Equals_Test()
        {
            Assert.That(Option.Some(42), Is.EqualTo(Option.Some(42)));
            Assert.That(Option.Some(42) == Option.Some(42));
            Assert.That(Option.Some(42) != Option.Some(24));
            Assert.That(Option.None<int>() != Option.Some(42));

            Assert.That(Option.None<int>() == Option.None<int>());
            Assert.That(!Option.None<int>().Equals(Option.None<string>()));

            Assert.That(null != Option.None<int>());
            Assert.That(Option.None<int>() != null);
            Assert.That(new Option<int>() == Option.None<int>());
            Assert.That(new Option<int>().Tag, Is.EqualTo(OptionType.None));
            Assert.That(Option.Some<string>(null), Is.EqualTo(Option.None<string>()));
        }

        [Test]
        public void Monad_Laws_Test()
        {
            var k = Fun.Create((int x) => Option.Some(x * 2));

            // Left Identity
            Assert.That(Option.Return(42).Bind(k), Is.EqualTo(k(42)));

            // Right Identity
            var m = Option.Some(42);
            Assert.That(m.Bind(Option.Return), Is.EqualTo(m));

            // Associativity
            var h = Fun.Create((int x) => Option.Some(x + 1));
            Assert.That(
                m.Bind(x => k(x).Bind(h)),
                Is.EqualTo(m.Bind(k).Bind(h)));
        }

        [Test]
        public void Applicative_Test()
        {
            var add = Fun.Create((int x, int y) => x + y).Curry();

            var result1 = add.ToOption().Apply(Option.Some(3)).Apply(Option.Some(5));
            Assert.That(result1, Is.EqualTo(Option.Some(8)));

            var result2 = add.ToOption().Apply(Option.None<int>()).Apply(Option.Some(5));
            Assert.That(result2, Is.EqualTo(Option.None<int>()));

            Assert.That(Option.Some(3).Select(add).Apply(Option.Some(5)), Is.EqualTo(Option.Some(8)));
            Assert.That(Option.Some(5).Select(add(3)), Is.EqualTo(Option.Some(8)));

            var multiply = Fun.Create((int x, int y, int z) => x * y * z).Curry();

            var multResult = multiply.ToOption().Apply(Option.Some(1)).Apply(Option.Some(2)).Apply(Option.Some(3));
            Assert.That(multResult, Is.EqualTo(Option.Some(6)));

            // order matters
            var divide = Fun.Create((decimal x, decimal y) => x / y).Curry();
            Assert.That(divide.ToOption().Apply(Option.Some(1m)).Apply(Option.Some(2m)), Is.EqualTo(Option.Some(0.5m)));
            Assert.That(divide.ToOption().Apply(Option.Some(2m)).Apply(Option.Some(1m)), Is.EqualTo(Option.Some(2m)));
        }

        [Test]
        public void Applicative_ToOption_Test()
        {
            var divide = Fun.Create((decimal x, decimal y) => Division.Divide(x, y));

            Assert.That(divide.Curry().ToOption().Apply(Option.Some(1m)).Apply(Option.Some(2m)), Is.EqualTo(Option.Some(0.5m)));
            Assert.That(divide.Curry().ToOption().Apply(Option.None<decimal>()).Apply(Option.Some(2m)), Is.EqualTo(Option.None<decimal>()));
            Assert.That(divide.Curry().ToOption().Apply(Option.Some(1m)).Apply(Option.None<decimal>()), Is.EqualTo(Option.None<decimal>()));
            Assert.That(divide.Curry().ToOption().Apply(Option.Some(1m)).Apply(Option.Some(0m)), Is.EqualTo(Option.None<decimal>()));
        }

        [Test]
        public void ReturnOption_OnExceptionNone_Test()
        {
            var divide = Fun.Create((decimal x, decimal y) => x / y)
                .ReturnOption()
                .OnExceptionNone()
                .Curry();

            Assert.That(divide.ToOption().Apply(Option.Some(1m)).Apply(Option.Some(2m)), Is.EqualTo(Option.Some(0.5m)));
            Assert.That(divide.ToOption().Apply(Option.None<decimal>()).Apply(Option.Some(2m)), Is.EqualTo(Option.None<decimal>()));
            Assert.That(divide.ToOption().Apply(Option.Some(1m)).Apply(Option.None<decimal>()), Is.EqualTo(Option.None<decimal>()));
            Assert.That(divide.ToOption().Apply(Option.Some(1m)).Apply(Option.Some(0m)), Is.EqualTo(Option.None<decimal>()));
        }

        [Test]
        public void Applicative_Laws_fmap_Test()
        {
            var add = new Func<int, Func<int, int>>(x1 => x2 => x1 + x2);
            var add3 = add(3);

            // fmap
            // fmap f x = pure f <*> x 
            var x = Option.Some(42);
            Assert.That(
                x.Select(add3), // fmap f x
                Is.EqualTo(Option.Return(add3).Apply(x))); // pure f <*> x
        }

        [Test]
        public void Applicative_Laws_Identity_Test()
        {
            // Identity
            // pure id <*> v = v
            var v = Option.Some(42);
            Assert.That(
                Option.Return(Operators.Id<int>()).Apply(v), // pure id <*> v
                Is.EqualTo(v)); // v
        }

        [Test, Ignore]
        public void Applicative_Laws_Composition_Test()
        {
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
        }

        [Test]
        public void Applicative_Laws_Homomorphism_Test()
        {
            // pure f <*> pure x = pure (f x)
            var f = Fun.Create((int i) => i + 3);
            const int x = 5;
            var lhs = Option.Return(f).Apply(Option.Return(x));
            var rhs = Option.Return(f(x));
            Assert.That(lhs, Is.EqualTo(rhs));
        }

        [Test, Ignore]
        public void Applicative_Laws_Interchange_Test()
        {
            // u <*> pure y = pure ($ y) <*> u 
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

        [Test]
        public void DefaultIfNone_Test()
        {
            var x = Option.None<int>().DefaultIfNone(-1);
            Assert.That(x, Is.EqualTo(-1));

            var y = Option.Some(42).DefaultIfNone(-1);
            Assert.That(y, Is.EqualTo(42));

            var f = Fun.Create((int i) => i%2 == 0 ? "foo" : null).ReturnOption();
            Assert.That(f(3), Is.EqualTo(Option.None<string>()));
        }

        [Test]
        public void Parse_Test()
        {
            var parseInt = Fun.Create((string s) => Int32.Parse(s)).ReturnOption().OnExceptionNone();
            var i = parseInt("sdfs");

            Assert.That(i, Is.EqualTo(Option.None<int>()));
        }

        [Test]
        public void IsSome_IsNone_Test()
        {
            var some = Option.Return(42);
            var none = Option.None<int>();

            Assert.That(some.IsSome);
            Assert.That(!none.IsSome);  

            Assert.That(!some.IsNone);
            Assert.That(none.IsNone);
        }

        [Test]
        public void ToString_Test()
        {
            Assert.That(Option.Return(42).ToString(), Is.EqualTo("Some(42)"));
            Assert.That(Option.None<int>().ToString(), Is.EqualTo("None<Int32>"));
            Assert.That(Option.None<string>().ToString(), Is.EqualTo("None<String>"));
        }

        [Test]
        public void FromNullable_Test()
        {
            Assert.That(((int?) 5).ToOption(), Is.EqualTo(Option.Some(5)));
            Assert.That(((int?) null).ToOption(), Is.EqualTo(Option.None<int>()));
        }
    }
}