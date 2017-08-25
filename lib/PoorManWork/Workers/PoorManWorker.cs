﻿using System;
using System.Threading;

namespace PoorManWork
{
    public abstract class PoorManWorker : IPoorManWorker
    {
        protected Thread WrappedThread { get; set; }

        public void Start(CancellationToken cancellationToken)
        {
            if (cancellationToken != CancellationToken.None)
            {
                cancellationToken.Register(Stop);
            }

            WrappedThread = new Thread(GetThreadAction(cancellationToken))
            {
                IsBackground = true
            };

            WrappedThread.Start();
        }

        private ThreadStart GetThreadAction(CancellationToken cancellationToken)
        {
            return () =>
            {
                try
                {
                    Loop(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    // Cancellation was requested
                }
            };
        }

        protected abstract void Loop(CancellationToken cancellationToken);

        private void Stop()
        {
            if (!WrappedThread.Join(1000))
            {
                WrappedThread.Abort();
            }
        }
    }
}