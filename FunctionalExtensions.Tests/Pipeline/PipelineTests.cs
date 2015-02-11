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
            var inc = Fun.Create((int x) => x + 1);

            var result = 1
                .Pipeline(inc)
                .Pipeline(inc)
                .Pipeline(x => x + 1);

            Check.That(result).IsEqualTo(4);
        }
    }
}
