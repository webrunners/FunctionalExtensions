using FunctionalExtensions.Lambda;
using FunctionalExtensions.Pipeline;
using NFluent;
using NUnit.Framework;

namespace FunctionalExtensions.Tests.Pipeline
{
    [TestFixture]
    public class PipelineTests
    {
        [Test]
        public void Pipeline_Test()
        {
            var i = 1;

            var inc = Fun.Create((int x) => x + 1);

            var result = i
                .Pipeline(inc)
                .Pipeline(inc)
                .Pipeline(inc);

            Check.That(result).IsEqualTo(4);
        }
    }
}
