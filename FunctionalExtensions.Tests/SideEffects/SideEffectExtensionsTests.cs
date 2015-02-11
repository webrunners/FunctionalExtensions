using FunctionalExtensions.SideEffects;
using NFluent;
using NUnit.Framework;

namespace FunctionalExtensions.Tests.SideEffects
{
    [TestFixture]
    public class SideEffectExtensionsTests
    {
        [Test]
        public void Do1_Test()
        {
            var o = Option.Some(5);
            var isCalled = false;
            o.Do(() => isCalled = true);

            Check.That(isCalled);
        }
    }
}
