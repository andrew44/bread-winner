﻿using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using SamplesShared;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 1000;

            var pool = WorkerPoolExample.CreatePool(
                false,
                new TimeSpan(0, 0, 0, 30),
                new TimeSpan(0, 0, 0, 10),
                100);
            var tokenSource = new CancellationTokenSource();
            pool.Start(tokenSource.Token);

            while (Console.ReadKey().KeyChar != 'q')
            {
            }

            tokenSource.Cancel();
        }
    }
}
