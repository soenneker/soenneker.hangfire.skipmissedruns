[![](https://img.shields.io/nuget/v/Soenneker.Hangfire.SkipMissedRuns.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Hangfire.SkipMissedRuns/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.hangfire.skipmissedruns/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.hangfire.skipmissedruns/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Hangfire.SkipMissedRuns.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Hangfire.SkipMissedRuns/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.hangfire.skipmissedruns/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.hangfire.skipmissedruns/actions/workflows/codeql.yml)

# Soenneker.Hangfire.SkipMissedRuns

Ensures the hangfire runner doesn't execute this job if time has passed since it's scheduled execution.

## Install

```bash
dotnet add package Soenneker.Hangfire.SkipMissedRuns
```

## Quick start

```csharp
using Soenneker.Hangfire.SkipMissedRuns;

public sealed class Request
{
    [SkipMissedRuns]
    public string? Value { get; init; }
}
```

Ensures the hangfire runner doesn't execute this job if time has passed since it's scheduled execution.

## What you get

- `SkipMissedRunsAttribute` — Ensures the hangfire runner doesn't execute this job if time has passed since it's scheduled execution.

## Important behavior

- `SkipMissedRunsAttribute`: Don't add this as an attribute to a method unless it's a hangfire -RECURRING- job.
