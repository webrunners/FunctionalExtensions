using NUnit.Framework;

namespace FunctionalExtensions.Tests
{
    [TestFixture]
    public class OperatorsTests
    {
        [Test]
        public void Id_Test()
        {
            Assert.That(Operators.Id(42), Is.EqualTo(42));
            Assert.That(Operators.Id<int>()(42), Is.EqualTo(42));
        }
    }
}
