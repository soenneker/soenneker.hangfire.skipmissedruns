using System;
using System.Collections.Generic;
using Hangfire.Client;
using Hangfire.Common;

namespace Soenneker.Hangfire.SkipMissedRuns;

/// <summary>
/// Cancels creation of a recurring job occurrence when its scheduled execution time is older than the allowed delay.
/// </summary>
/// <remarks>Apply this attribute only to recurring Hangfire job methods.</remarks>
public sealed class SkipMissedRunsAttribute : JobFilterAttribute, IClientFilter
{
    private readonly TimeSpan _maxDelay;

    /// <summary>
    /// Creates a filter with the maximum permitted scheduling delay.
    /// </summary>
    /// <param name="maxDelaySeconds">The age, in seconds, after which a recurring occurrence is skipped. Negative values are treated as zero.</param>
    public SkipMissedRunsAttribute(int maxDelaySeconds = 60)
    {
        _maxDelay = TimeSpan.FromSeconds(Math.Max(0, maxDelaySeconds));
    }

    /// <summary>
    /// Cancels stale recurring occurrences before Hangfire creates them.
    /// </summary>
    /// <param name="filterContext">Filter Context for the on creating operation.</param>
    public void OnCreating(CreatingContext filterContext)
    {
        if (!filterContext.Parameters.TryGetValue("RecurringJobId", out object? recurringJobId))
            return;

        // the job being created looks like a recurring job instance.

        Dictionary<string, string>? recurringJob = filterContext.Connection.GetAllEntriesFromHash($"recurring-job:{recurringJobId}");

        if (recurringJob == null || !recurringJob.TryGetValue("NextExecution", out string? nextExecution))
            return;

        DateTime utcNow = DateTime.UtcNow;

        // the next execution time of a recurring job is updated AFTER the job instance creation,
        // so at the moment it still contains the scheduled execution time from the previous run.
        DateTime scheduledTime = JobHelper.DeserializeDateTime(nextExecution);

        // Check if the job is created later than expected
        // and if it was created from the scheduler.

        // For now we don't want ANY old jobs to be scheduled
        if (utcNow > scheduledTime && utcNow - scheduledTime > _maxDelay)
        {
            filterContext.Canceled = true;
        }
    }

    /// <summary>
    /// Performs no post-creation work.
    /// </summary>
    /// <param name="filterContext">Filter Context for the on created operation.</param>
    public void OnCreated(CreatedContext filterContext)
    {
        // required for base
    }
}
