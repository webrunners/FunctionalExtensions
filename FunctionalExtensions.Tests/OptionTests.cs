using System;
using System.Collections.Generic;
using FunctionalExtensions.Currying;
using FunctionalExtensions.Linq;
using FunctionalExtensions.Transform;
using FunctionalExtensions.Lambda;
using Xunit;

namespace FunctionalExtensions.Tests
{
    public class OptionTests
    {
        [Fact]
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
            Assert.Equal(100, result);
        }

        [Fact]
        public void ValidationWithOptionMonad_RainyDay_Test()
        {
            var isCalled = false;
            var d1 = Fun.Create(() => Option.Some(2.5m));
            var zero = Fun.Create(() => Option.Some(0.0m));
            var ex = Fun.Create(Option.None<decimal>);

            var callbackNone = Fun.Create(() => isCalled = true);

            var callbackSome = Fun.Create((decimal x) =>
            {
                Assert.True(false);
                return false;
            });

            isCalled = false;
            ValidationWithOptionMonad(d1, ex, Division.Divide, callbackSome, callbackNone);
            Assert.True(isCalled);

            isCalled = false;
            ValidationWithOptionMonad(ex, d1, Division.Divide, callbackSome, callbackNone);
            Assert.True(isCalled);

            isCalled = false;
            ValidationWithOptionMonad(d1, zero, Division.Divide, callbackSome, callbackNone);
            Assert.True(isCalled);
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

            Assert.True(result);
        }

        [Fact]
        public void ToOption_Test()
        {
            var result = 5.ToOption().Match(x => x, () => 0);

            Assert.Equal(5, result);

            string s = null;
            var option2 = s.ToOption();
            Assert.True(option2.Match(x => false, () => true));
        }

        [Fact]
        public void Null_Tests()
        {
            var opt1 = Option.Some<string>(null);
            Assert.Equal(Option.None<string>(), opt1);

            var opt2 = Option.Return<string>(null);
            Assert.Equal(Option.None<string>(), opt2);
        }

        [Fact]
        public void Equals_Test()
        {
            Assert.Equal(Option.Some(42), Option.Some(42));
            Assert.True(Option.Some(42) == Option.Some(42));
            Assert.True(Option.Some(42) != Option.Some(24));
            Assert.True(Option.None<int>() != Option.Some(42));

            Assert.True(Option.None<int>() == Option.None<int>());
            Assert.True(!Option.None<int>().Equals(Option.None<string>()));

            string s = null;
            Assert.True(s == Option.None<string>());
            Assert.True(null == Option.None<string>());
            Assert.True(Option.None<string>() == s);
            Assert.True(Option.None<string>() == null);
            Assert.True(new Option<int>() == Option.None<int>());
            Assert.Equal(OptionType.None, new Option<int>().Tag);
            Assert.Equal(Option.None<string>(), Option.Some<string>(null));

            Assert.Equal(42, Option.Some(42));
            Assert.True(Option.Some(42) == 42);
            Assert.True(Option.Some(42) != 24);
            Assert.True(Option.None<int>() != 42);
            Assert.True(42 == Option.Some(42));
            Assert.True(42 != Option.Some(24));
            Assert.True(42 != Option.None<int>());
        }

        [Fact]
        public void Monad_Laws_Test()
        {
            var k = Fun.Create((int x) => Option.Some(x * 2));

            // Left Identity
            Assert.Equal(k(42), Option.Return(42).Bind(k));

            // Right Identity
            var m = Option.Some(42);
            Assert.Equal(m, m.Bind(Option.Return));

            // Associativity
            var h = Fun.Create((int x) => Option.Some(x + 1));
            Assert.Equal(
                m.Bind(k).Bind(h),
                m.Bind(x => k(x).Bind(h))
            );
        }

        [Fact]
        public void Applicative_Test()
        {
            var add = Fun.Create((int x, int y) => x + y).Curry();

            var result1 = add.ToOption().Apply(Option.Some(3)).Apply(Option.Some(5));
            Assert.Equal(Option.Some(8), result1);

            var result2 = add.ToOption().Apply(Option.None<int>()).Apply(Option.Some(5));
            Assert.Equal(result2, Option.None<int>());

            Assert.Equal(Option.Some(3).Select(add).Apply(Option.Some(5)), Option.Some(8));
            Assert.Equal(Option.Some(5).Select(add(3)), Option.Some(8));

            var multiply = Fun.Create((int x, int y, int z) => x * y * z).Curry();

            var multResult = multiply.ToOption().Apply(Option.Some(1)).Apply(Option.Some(2)).Apply(Option.Some(3));
            Assert.Equal(Option.Some(6), multResult);

            // order matters
            var divide = Fun.Create((decimal x, decimal y) => x / y).Curry();
            Assert.Equal(Option.Some(0.5m), divide.ToOption().Apply(Option.Some(1m)).Apply(Option.Some(2m)));
            Assert.Equal(Option.Some(2m), divide.ToOption().Apply(Option.Some(2m)).Apply(Option.Some(1m)));
        }

        [Fact]
        public void Applicative_ToOption_Test()
        {
            var divide = Fun.Create((decimal x, decimal y) => Division.Divide(x, y));

            Assert.Equal(Option.Some(0.5m), divide.Curry().ToOption().Apply(Option.Some(1m)).Apply(Option.Some(2m)));
            Assert.Equal(Option.None<decimal>(), divide.Curry().ToOption().Apply(Option.None<decimal>()).Apply(Option.Some(2m)));
            Assert.Equal(Option.None<decimal>(), divide.Curry().ToOption().Apply(Option.Some(1m)).Apply(Option.None<decimal>()));
            Assert.Equal(Option.None<decimal>(), divide.Curry().ToOption().Apply(Option.Some(1m)).Apply(Option.Some(0m)));
        }

        [Fact]
        public void ReturnOption_OnExceptionNone_Test()
        {
            var divide = Fun.Create((decimal x, decimal y) => x / y)
                .ReturnOption()
                .OnExceptionNone()
                .Curry();

            Assert.Equal(Option.Some(0.5m), divide.ToOption().Apply(Option.Some(1m)).Apply(Option.Some(2m)));
            Assert.Equal(Option.None<decimal>(), divide.ToOption().Apply(Option.None<decimal>()).Apply(Option.Some(2m)));
            Assert.Equal(Option.None<decimal>(), divide.ToOption().Apply(Option.Some(1m)).Apply(Option.None<decimal>()));
            Assert.Equal(Option.None<decimal>(), divide.ToOption().Apply(Option.Some(1m)).Apply(Option.Some(0m)));
        }

        [Fact]
        public void Applicative_Laws_fmap_Test()
        {
            var add = new Func<int, Func<int, int>>(x1 => x2 => x1 + x2);
            var add3 = add(3);

            // fmap
            // fmap f x = pure f <*> x 
            var x = Option.Some(42);
            Assert.Equal(
                x.Select(add3), // fmap f x
                Option.Return(add3).Apply(x)); // pure f <*> x
        }

        [Fact]
        public void Applicative_Laws_Identity_Test()
        {
            // Identity
            // pure id <*> v = v
            var v = Option.Some(42);
            Assert.Equal(
                Option.Return(Operators.Id<int>()).Apply(v), // pure id <*> v
                v
            ); // v
        }

        [Fact(Skip="Because")]
        public void Applicative_Laws_Composition_Test()
        {
            // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
        }

        [Fact]
        public void Applicative_Laws_Homomorphism_Test()
        {
            // pure f <*> pure x = pure (f x)
            var f = Fun.Create((int i) => i + 3);
            const int x = 5;
            var lhs = Option.Return(f).Apply(Option.Return(x));
            var rhs = Option.Return(f(x));
            Assert.Equal(lhs, rhs);
        }

        [Fact(Skip="Because")]
        public void Applicative_Laws_Interchange_Test()
        {
            // u <*> pure y = pure ($ y) <*> u 
        }

        [Fact]
        public void FirstOrOptionTest()
        {
            var list = new List<int>();

            Assert.True(list.FirstOrOption().Match(x => false, () => true));

            list.AddRange(new[] { 1, 2, 3 });

            Assert.True(list.FirstOrOption(x => x > 3).Match(x => false, () => true));
            Assert.Equal(3, list.FirstOrOption(x => x == 3).Match(x => x, () => -1));
        }

        [Fact]
        public void SingleOrOptionTest()
        {
            var list = new List<int>();

            Assert.True(list.SingleOrOption().Match(x => false, () => true));

            list.AddRange(new[] { 1, 1, 2, 3 });

            Assert.True(list.SingleOrOption(x => x > 3).Match(x => false, () => true));
            Assert.Equal(3, list.SingleOrOption(x => x == 3).Match(x => x, () => -1));
            Assert.True(list.SingleOrOption(x => x == 1).Match(x => false, () => true));
        }

        [Fact]
        public void DefaultIfNone_Test()
        {
            var x = Option.None<int>().DefaultIfNone(-1);
            Assert.Equal(-1, x);

            var y = Option.Some(42).DefaultIfNone(-1);
            Assert.Equal(42, y);

            var f = Fun.Create((int i) => i%2 == 0 ? "foo" : null).ReturnOption();
            Assert.Equal(Option.None<string>(), f(3));
        }

        [Fact]
        public void Parse_Test()
        {
            var parseInt = Fun.Create((string s) => Int32.Parse(s)).ReturnOption().OnExceptionNone();
            var i = parseInt("sdfs");

            Assert.Equal(Option.None<int>(), i);
        }

        [Fact]
        public void IsSome_IsNone_Test()
        {
            var some = Option.Return(42);
            var none = Option.None<int>();

            Assert.True(some.IsSome);
            Assert.True(!none.IsSome);  

            Assert.True(!some.IsNone);
            Assert.True(none.IsNone);
        }

        [Fact]
        public void ToString_Test()
        {
            Assert.Equal("Some(42)", Option.Return(42).ToString());
            Assert.Equal("None<Int32>", Option.None<int>().ToString());
            Assert.Equal("None<String>", Option.None<string>().ToString());
        }

        [Fact]
        public void FromNullable_Test()
        {
            Assert.Equal(Option.Some(5), ((int?) 5).ToOption());
            Assert.Equal(Option.None<int>(), ((int?) null).ToOption());
        }

        [Fact]
        public void Implicit_Operator_Test()
        {
            Option<int> i = 6;
            Assert.Equal(Option.Some(6), i);

            Option<string> s = "hello";
            Assert.Equal(Option.Some("hello"), s);

            s = null;
            Assert.Equal(Option.None<string>(), s);
        }
    }
}