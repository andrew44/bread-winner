using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using PoorManWork;

namespace Api
{
    public class DummyWorkFactory : IPoorManWorkFactory
    {
        private readonly WorkAvailableRepo _workAvailableRepo;

        public DummyWorkFactory(WorkAvailableRepo workAvailableRepo)
        {
            _workAvailableRepo = workAvailableRepo;
        }

        public IPoorManWorkItem[] Create(CancellationToken cancellationToken)
        {
            if (!_workAvailableRepo.IsWorkAvailable())
            {
                return null;
            }

            if (cancellationToken.WaitHandle.WaitOne(1000))
            {
                return null;
            }

            var rand = new Random();
            var synchronizer = new PoorManWorkBatchSynchronizer(3);
            var workItems = new[]
            {
                new SyncedDummyWorkItem(rand.Next(), synchronizer, cancellationToken),
                new SyncedDummyWorkItem(rand.Next(), synchronizer, cancellationToken),
                new SyncedDummyWorkItem(rand.Next(), synchronizer, cancellationToken)
            };

            Debug.WriteLine(
                $"Producer {Thread.CurrentThread.ManagedThreadId} has created " +
                $"{workItems[0].Id}, {workItems[1].Id}, {workItems[2].Id}");

            return workItems.Cast<IPoorManWorkItem>().ToArray();
        }
    }
}