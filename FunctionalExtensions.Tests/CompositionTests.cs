using System;
using System.Collections;
using System.Collections.Generic;
using FunctionalExtensions.Currying;
using FunctionalExtensions.Lambda;
using NFluent;
using NUnit.Framework;

namespace FunctionalExtensions.Tests
{
    [TestFixture]
    public class CompositionTests
    {
        [Test]
        public void Compose_Test()
        {
            var f = new Func<string, IEnumerable<string>>(s => s.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
            var g = new Func<string, string>(s => s.ToUpper());

            var h = f.Compose(g);

            var result = h("foo bar");

            Check.That(result).ContainsExactly("FOO", "BAR");

            var inc = new Func<int, int>(i => i + 1);
            var compose = inc.Compose(inc);
            Check.That(compose(1)).IsEqualTo(3);
        }

        [Test]
        public void Compose_2_Test()
        {
            var multiply = new Func<int, int, int>((x, y) => x*y);
            var inc = new Func<int, int>(i => i + 1);

            var compose = multiply.Compose(inc);

            var result = compose(7, 8);
            Check.That(result).IsEqualTo(64);
        }

        [Test]
        public void Compose_3_Test()
        {
            var multiply3 = new Func<int, int, int, int>((x, y, z) => x * y * z);
            var inc = new Func<int, int>(i => i + 1);

            var compose = multiply3.Compose(inc);

            var result = compose(1, 2, 3);
            Check.That(result).IsEqualTo(12);
        }

        [Test]
        public void Test()
        {
            var inc = new Func<int, int>(i => i + 1);
            var multiply = new Func<int, int, int>((x, y) => x*y);

            var c = Fun.Create((int i) => multiply.Curry()(i));

            var compose = inc.Compose(multiply);


        }
    }
}
