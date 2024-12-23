using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Hangfire.SkipMissedRuns.Tests;

[Collection("Collection")]
public class SkipMissedRunsAttributeTests : FixturedUnitTest
{
    public SkipMissedRunsAttributeTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public void Default()
    {
    }
}
