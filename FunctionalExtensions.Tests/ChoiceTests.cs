using System;
using System.Globalization;
using FunctionalExtensions.Lambda;
using Xunit;

namespace FunctionalExtensions.Tests
{
    public class ChoiceTests
    {
        [Fact]
        public void ValidationWithChoiceMonad_HappyDay_Test()
        {
            var d1 = Fun.Create(() => Choice.NewChoice1Of2<decimal, string>(2.5m));
            var d2 = Fun.Create(() => Choice.NewChoice1Of2<decimal, string>(2.5m));

            var callbackSome = Fun.Create((decimal x) => x.ToString("#.00", CultureInfo.InvariantCulture));
            var callbackNone = Fun.Create((string x) => string.Empty);

            Assert.Equal("100.00", ValidationWithChoiceMonad(d1, d2, callbackSome, callbackNone));
        }

        [Fact]
        public void ValidationWithChoiceMonad_RainyDay_Test()
        {
            var d1 = Fun.Create(() => Choice.NewChoice1Of2<decimal, string>(2.5m));
            var ex1 = Fun.Create(() => Choice.NewChoice2Of2<decimal, string>("Error1"));
            var ex2 = Fun.Create(() => Choice.NewChoice2Of2<decimal, string>("Error2"));
            var zero = Fun.Create(() => Choice.NewChoice1Of2<decimal, string>(0.0m));

            var callbackNone = Fun.Create((string s) => s);
            var callbackSome = Fun.Create((decimal x) => string.Empty);

            Assert.Equal("Error2", ValidationWithChoiceMonad(d1, ex2, callbackSome, callbackNone));
            Assert.Equal("Error1", ValidationWithChoiceMonad(ex1, d1, callbackSome, callbackNone));
            Assert.Equal("Cannot divide by zero.", ValidationWithChoiceMonad(d1, zero, callbackSome, callbackNone));
        }

        [Fact]
        public void Monad_Laws_Test()
        {
            var k = Fun.Create((string x) => Choice.NewChoice1Of2<string, int>(x.ToUpper()));

            // Left Identity
            var choice1 = Choice.Return<string, int>("hello").Bind(k);
            var choice2 = k("hello");

            Assert.True(ChoiceEquals(choice1, choice2));

            // Right Identity
            var m = Choice.Return<string, int>("hello");
            var choice4 = m.Bind(Choice.Return<string, int>);

            Assert.True(ChoiceEquals(m, choice4));

            // Associativity
            var h = Fun.Create((string x) => Choice.NewChoice1Of2<string, int>(x + x));

            var choice5 = m.Bind(x1 => k(x1).Bind(h));
            var choice6 = m.Bind(k).Bind(h);

            Assert.True(ChoiceEquals(choice5, choice6));
        }

        [Fact]
        public void ToString_Test()
        {
            Assert.Equal("Choice1Of2<String, Int32>(42)", Choice.NewChoice1Of2<string, int>("42").ToString());
            Assert.Equal("Choice2Of2<String, Int32>(42)", Choice.NewChoice2Of2<string, int>(42).ToString());
            Assert.Equal("Choice1Of2<String, Int32>()", new Choice<string, int>().ToString());
        }

        private static bool ChoiceEquals<T1, T2>(Choice<T1, T2> first, Choice<T1, T2> second)
        {
            return first.Match(
                onChoice1Of2: f1 => second.Match(
                    onChoice1Of2: s1 => f1.Equals(s1),
                    onChoice2Of2: s2 => false
                ),
                onChoice2Of2: f2 => false
            );
        }

        private static string ValidationWithChoiceMonad(
            Func<Choice<decimal, string>> readdecimal1,
            Func<Choice<decimal, string>> readdecimal2,
            Func<decimal, string> onSome,
            Func<string, string> onNone
        )
        {
            return (
                    from v1 in readdecimal1()
                    from v2 in readdecimal2()
                    from divisionResult in Division.Divide(v1, v2).ToChoice("Cannot divide by zero.")
                    select divisionResult * 100
                )
                .Match(onSome, onNone);
        }

    }
}
