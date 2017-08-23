﻿using System;
using System.Threading;

namespace PoorManWork
{
    public interface IPoorManWorkFacade
    {
        void AddConsumers(int n);

        void AddProducer(EventWaitHandle workArrived, Func<CancellationToken, IPoorManWorkItem[]> workFactoryMethod);

        void AddProducer(PoorManPulser pulser, Func<CancellationToken, IPoorManWorkItem[]> workFactoryMethod);

        void Start();

        void Stop();
    }
}