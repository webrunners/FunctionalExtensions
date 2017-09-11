using Xunit;

namespace FunctionalExtensions.Tests
{
    public class OperatorsTests
    {
        [Fact]
        public void Id_Test()
        {
            Assert.Equal(42, Operators.Id(42));
            Assert.Equal(42, Operators.Id<int>()(42));
        }
    }
}
