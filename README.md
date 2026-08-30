[![](https://img.shields.io/nuget/v/Soenneker.Hangfire.SkipMissedRuns.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Hangfire.SkipMissedRuns/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.hangfire.skipmissedruns/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.hangfire.skipmissedruns/actions/workflows/publish-package.yml)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.hangfire.skipmissedruns/build-and-test.yml?style=for-the-badge&label=build)](https://github.com/soenneker/soenneker.hangfire.skipmissedruns/actions/workflows/build-and-test.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Hangfire.SkipMissedRuns.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Hangfire.SkipMissedRuns/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.hangfire.skipmissedruns/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.hangfire.skipmissedruns/actions/workflows/codeql.yml)

# Soenneker.Hangfire.SkipMissedRuns

Prevents stale occurrences of recurring Hangfire jobs from being enqueued after a scheduler outage, server restart, or other delay. Fresh occurrences continue normally.

## Installation

```bash
dotnet add package Soenneker.Hangfire.SkipMissedRuns
```

## Apply it to a recurring job

```csharp
using Soenneker.Hangfire.SkipMissedRuns;

public sealed class RefreshCatalogJob
{
    [SkipMissedRuns]
    public Task Run(CancellationToken cancellationToken)
    {
        return RefreshCatalog(cancellationToken);
    }
}
```

Register the recurring job normally:

```csharp
RecurringJob.AddOrUpdate<RefreshCatalogJob>(
    "refresh-catalog",
    job => job.Run(CancellationToken.None),
    Cron.Hourly);
```

The default tolerance is 60 seconds. Supply a larger tolerance when modest scheduler delays should still execute:

```csharp
[SkipMissedRuns(maxDelaySeconds: 300)]
public Task Run(CancellationToken cancellationToken) => ...;
```

During job creation, the filter reads the recurring job's stored `NextExecution` value. If that scheduled time is older than the configured tolerance, creation is canceled. If the invocation is not a recurring job or its scheduling metadata is unavailable, the filter leaves it unchanged.

Apply the attribute only to recurring job methods. It does not deduplicate jobs, cancel an occurrence that was already enqueued, or change retry behavior after a job starts.
