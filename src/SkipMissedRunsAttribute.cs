using System;
using System.Collections.Generic;
using Hangfire.Client;
using Hangfire.Common;

namespace Soenneker.Hangfire.SkipMissedRuns;

/// <summary>
/// Ensures the hangfire runner doesn't execute this job if time has passed since it's scheduled execution
/// </summary>
/// <remarks>Don't add this as an attribute to a method unless it's a hangfire -RECURRING- job</remarks>
public class SkipMissedRunsAttribute : JobFilterAttribute, IClientFilter
{
    /// <summary>
    /// The cutoff point in which we determine a job is 'old'. (1 minute)
    /// </summary>
    private const int _maxDelayMs = 60000;

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
        if (utcNow > scheduledTime.AddMilliseconds(_maxDelayMs)) // && IsCreatedFromRecurringJobScheduler()
        {
            filterContext.Canceled = true;
        }
    }

    public void OnCreated(CreatedContext filterContext)
    {
        // required for base
    }
}