using FunctionalExtensions.SideEffects;
using Xunit;

namespace FunctionalExtensions.Tests
{
    public class SideEffectExtensionsTests
    {
        [Fact]
        public void Do1_Test()
        {
            var o = Option.Some(5);
            var isCalled = false;
            o.Do(() => isCalled = true);

            Assert.True(isCalled);
        }
    }
}
