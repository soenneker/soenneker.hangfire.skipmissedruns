using Soenneker.Tests.HostedUnit;

namespace Soenneker.Hangfire.SkipMissedRuns.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class SkipMissedRunsAttributeTests : HostedUnitTest
{
    public SkipMissedRunsAttributeTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {
    }
}
