using FunctionalExtensions.FluentOption;
using FunctionalExtensions.Lambda;
using NUnit.Framework;
using System;

namespace FunctionalExtensions.Tests.FluentOption
{
    [TestFixture]
    public class FluentOptionTests
    {
        [Test]
        public void From_Result_Test1()
        {
            var readInt = Fun.Create(() => Option.Some(1));

            OptionMonad
                .From(readInt)
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(1)),
                    Assert.Fail);
        }

        [Test]
        public void From_Result_Test2()
        {
            var readInt1 = Fun.Create(() => Option.Some(1));
            var readInt2 = Fun.Create(() => Option.Some(2));

            OptionMonad
                .From(readInt1)
                .From(readInt2)
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(2)),
                    Assert.Fail);

            OptionMonad
                .From(Option.None<int>)
                .From(readInt2)
                .Result()
                .Match(
                    x => Assert.Fail(),
                    Assert.Pass);
        }

        [Test]
        public void From_Result_Test3()
        {
            var readInt1 = Fun.Create(() => Option.Some(1));
            var readInt2 = Fun.Create(() => Option.Some(2));
            var readInt3 = Fun.Create(() => Option.Some(3));

            OptionMonad
                .From(readInt1)
                .From(readInt2)
                .From(readInt3)
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(3)),
                    Assert.Fail);

            OptionMonad
                .From(Option.None<decimal>)
                .From(readInt2)
                .From(readInt3)
                .Result()
                .Match(
                    x => Assert.Fail(),
                    Assert.Pass);
        }

        [Test]
        public void Select_Test()
        {
            var readInt1 = Fun.Create(() => Option.Some(5));
            var readInt2 = Fun.Create(() => Option.Some(7));

            var add = Fun.Create((int a, int b) => a + b);

            OptionMonad
                .From(readInt1)
                .Select(x => add(x, 1))
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(6)),
                    Assert.Fail);

            OptionMonad
                .From(readInt1)
                .From(readInt2)
                .Select(add)
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(12)),
                    Assert.Fail);

            OptionMonad
                .From(readInt1)
                .From(Option.None<int>)
                .Select(add)
                .Result()
                .Match(
                    x => Assert.Fail(),
                    Assert.Pass);
        }

        [Test]
        public void Bind_Test()
        {
            var divide = Fun.Create((int i) =>
            {
                try { return Option.Some(1m / i); }
                catch (DivideByZeroException) { return Option.None<decimal>(); }
            });

            var readInt = Fun.Create(() => Option.Some(2));

            OptionMonad
                .From(readInt)
                .Bind(divide)
                .Result()
                .Select(x => x * 100)
                .Match(
                    x => Assert.That(x, Is.EqualTo(50m)),
                    Assert.Fail);
        }

        [Test]
        public void Bind_Test2()
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
                .Bind(divide)
                .Result()
                .Select(x => x * 100)
                .Match(
                    x => Assert.That(x, Is.EqualTo(25m)),
                    Assert.Fail);

            OptionMonad
                .From(readInt1)
                .From(() => Option.Some(0))
                .Bind(divide)
                .Result()
                .Select(x => x * 100)
                .Match(
                    x => Assert.Fail(),
                    Assert.Pass);
        }

        [Test]
        public void Multiple_Combinations_Test()
        {
            OptionMonad
                .From(() => Option.Some(1))
                .Bind(x => Option.Some(x + 1))
                .Bind(x => Option.Some(x + 1))
                .From(() => Option.Some(1))
                .Select((x, y) => x + y)
                .From(() => Option.Some(1))
                .Bind((x, y) => Option.Some(x + y))
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(5)),
                    Assert.Fail);

            OptionMonad
                .From(() => Option.Some(1))
                .Bind(x => Option.Some(x + 1))
                .Bind(x => Option.Some(x + 1))
                .From(() => Option.Some(1))
                .Select((x, y) => x + y)
                .From(() => Option.Some(1))
                .Bind((x, y) => Option.Some(x + y))
                .From(Option.None<string>)
                .Select((x, s) => Option.Some(s + x))
                .Result()
                .Match(
                    x => Assert.Fail(),
                    Assert.Pass);

            OptionMonad
                .From(() => Option.Some(1))
                .Bind(x => Option.Some(x + 1))
                .Bind(x => Option.Some(x + 1))
                .Bind(x => Option.Some(x + 1))
                .Bind(x => Option.Some(x + 1))
                .Bind(x => Option.Some(x + 1))
                .Bind(x => Option.Some(x + 1))
                .Bind(x => Option.Some(x + 1))
                .Bind(x => Option.Some(x.ToString()))
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo("8")),
                    Assert.Fail);
        }

        [Test]
        public void From_8x_Test()
        {
            OptionMonad
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .Bind((x1, x2, x3, x4, x5, x6, x7, x8) => Option.Some(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8))
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(8)),
                    Assert.Fail);

            OptionMonad
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .Select((x1, x2, x3, x4, x5, x6, x7, x8) => x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8)
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(8)),
                    Assert.Fail);
        }

        [Test]
        public void From_7x_Test()
        {
            OptionMonad
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .Bind((x1, x2, x3, x4, x5, x6, x7) => Option.Some(x1 + x2 + x3 + x4 + x5 + x6 + x7))
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(7)),
                    Assert.Fail);

            OptionMonad
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .Select((x1, x2, x3, x4, x5, x6, x7) => x1 + x2 + x3 + x4 + x5 + x6 + x7)
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(7)),
                    Assert.Fail);
        }

        [Test]
        public void From_6x_Test()
        {
            OptionMonad
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .Bind((x1, x2, x3, x4, x5, x6) => Option.Some(x1 + x2 + x3 + x4 + x5 + x6))
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(6)),
                    Assert.Fail);

            OptionMonad
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .Select((x1, x2, x3, x4, x5, x6) => x1 + x2 + x3 + x4 + x5 + x6)
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(6)),
                    Assert.Fail);
        }

        [Test]
        public void From_5x_Test()
        {
            OptionMonad
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .Bind((x1, x2, x3, x4, x5) => Option.Some(x1 + x2 + x3 + x4 + x5))
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(5)),
                    Assert.Fail);

            OptionMonad
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .Select((x1, x2, x3, x4, x5) => x1 + x2 + x3 + x4 + x5)
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(5)),
                    Assert.Fail);
        }

        [Test]
        public void From_4x_Test()
        {
            OptionMonad
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .Bind((x1, x2, x3, x4) => Option.Some(x1 + x2 + x3 + x4))
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(4)),
                    Assert.Fail);

            OptionMonad
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .From(() => Option.Some(1))
                .Select((x1, x2, x3, x4) => x1 + x2 + x3 + x4)
                .Result()
                .Match(
                    x => Assert.That(x, Is.EqualTo(4)),
                    Assert.Fail);
        }
    }
}   
