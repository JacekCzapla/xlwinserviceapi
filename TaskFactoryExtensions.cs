using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;

namespace XLWinServiceAPI
{
    public static class TaskFactoryExtensions
    {
        private static readonly TaskScheduler _staScheduler = new StaTaskScheduler(numberOfThreads: 1);

        public static Task<TResult> StartNewSta<TResult>(this TaskFactory factory, Func<TResult> action)
        {
            return factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, _staScheduler);
        }
    }
}
