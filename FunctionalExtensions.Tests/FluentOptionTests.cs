using FunctionalExtensions.FluentOption;
using FunctionalExtensions.Lambda;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtensions.Tests
{
    [TestFixture]
    public class FluentOptionTests
    {
        //[Test]
        public void Test1()
        {
            var readInt = Fun.Create(() => Option.Some(1));

            OptionMonad
                .From(readInt)
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(1)),
                    () => Assert.Fail());
        }

        //[Test]
        public void Test2()
        {
            var divide = Fun.Create((int i) =>
            {
                try { return Option.Some(1m / i); }
                catch (DivideByZeroException) { return Option.None<decimal>(); }
            });

            var readInt = Fun.Create(() => Option.Some(2));

            OptionMonad
                .From(readInt)
                .From(divide)
                .Result()
                .Select(x => x * 100)
                .Match(
                    x => Assert.That(x, Is.EqualTo(50m)),
                    () => Assert.Fail());
        }

        //[Test]
        public void Test3()
        {
            var divide = Fun.Create((int a, int b) =>
            {
                try { return Option.Some((decimal) a / b); }
                catch (DivideByZeroException) { return Option.None<decimal>(); }
            });

            var readInt1 = Fun.Create(() => Option.Some(10));
            var readInt2 = Fun.Create(() => Option.Some(40));

            OptionMonad
                .From(readInt1)
                .From(readInt2)
                .From(divide)
                .Result()
                .Select(x => x * 100)
                .Match(
                    x => Assert.That(x, Is.EqualTo(25m)),
                    () => Assert.Fail());
        }
    }
}   
