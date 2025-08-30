﻿using Hangfire.States;
using Hangfire.Storage;

namespace Oikono.Infrastructure;

public class QueueFilter : IApplyStateFilter
{
    private readonly string _defaultQueue;

    public QueueFilter(string defaultQueue = "oikono")
    {
        _defaultQueue = defaultQueue;
    }

    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        if (context.NewState is EnqueuedState enqueueState)
        {
            enqueueState.Queue = _defaultQueue;
        }
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        // No action needed
    }
}