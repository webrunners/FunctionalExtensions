using FunctionalExtensions.Lambda;
using FunctionalExtensions.Pipeline;
using Xunit;

namespace FunctionalExtensions.Tests
{
    public class PipelineTests
    {
        [Fact]
        public void Pipeline_Test()
        {
            var inc = Fun.Create((int x) => x + 1);

            var result = 1
                .Pipeline(inc)
                .Pipeline(inc)
                .Pipeline(x => x + 1);

            Assert.Equal(4, result);
        }
    }
}
