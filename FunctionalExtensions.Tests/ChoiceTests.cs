using System;
using FunctionalExtensions.Lambda;
using Xunit;

namespace FunctionalExtensions.Tests
{
    public class ChoiceTests
    {
        [Fact]
        public void ValidationWithChoiceMonad_HappyDay_Test()
        {
            var result = 0.0m;
            var d1 = Fun.Create(() => Choice.NewChoice1Of2<decimal, string>(2.5m));
            var d2 = Fun.Create(() => Choice.NewChoice1Of2<decimal, string>(2.5m));

            var callbackSome = Act.Create((decimal x) => result = x);

            var callbackNone = new Action<string>(x => Assert.True(false));

            ValidationWithChoiceMonad(d1, d2, callbackSome, callbackNone);
            Assert.Equal(100, result);
        }

        [Fact]
        public void ValidationWithChoiceMonad_RainyDay_Test()
        {
            var d1 = Fun.Create(() => Choice.NewChoice1Of2<decimal, string>(2.5m));
            var ex1 = Fun.Create(() => Choice.NewChoice2Of2<decimal, string>("Error1"));
            var ex2 = Fun.Create(() => Choice.NewChoice2Of2<decimal, string>("Error2"));
            var zero = Fun.Create(() => Choice.NewChoice1Of2<decimal, string>(0.0m));

            var error = String.Empty;
            Action<string> callbackNone = s => error = s;
            Action<decimal> callbackSome = x => Assert.True(false);

            ValidationWithChoiceMonad(d1, ex2, callbackSome, callbackNone);
            Assert.Equal("Error2", error);

            ValidationWithChoiceMonad(ex1, d1, callbackSome, callbackNone);
            Assert.Equal("Error1", error);

            ValidationWithChoiceMonad(d1, zero, callbackSome, callbackNone);
            Assert.Equal("Cannot Devide by zero.", error);
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

        private static bool ChoiceEquals<T1, T2>(Choice<T1, T2> choice5, Choice<T1, T2> choice6)
        {
            var b = false;
            choice5.Match(
                x1 => choice6.Match(
                    x2 => b = x1.Equals(x2),
                    _ => { }),
                _ => { });
            return b;
        }
    }
}
