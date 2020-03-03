namespace UserModeConstructs
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class MultiWebRequest 
    {
        private readonly AsyncCoordintor coordintor = new AsyncCoordintor();
        public readonly Dictionary<string, object> servers = new Dictionary<string, object>
        {
            { "http://Wintellect.com/", null },
            { "http://Microsoft.com/", null },
            { "http://1.1.1.1/", null }
        };

        public MultiWebRequest(int timeout)
        {
            var httpClient = new HttpClient();
            coordintor.AboutToBegin(servers.Count);

            foreach(var s in servers.Keys)
            {
                httpClient.GetByteArrayAsync(s)
                    .ContinueWith(t => ComputeResult(s, t));
            }

            coordintor.AllDone(AllDone, timeout);
        }

        private void AllDone(CoordinationStatus status) 
        {
            switch (status)
            {
                case CoordinationStatus.Cancel:
                    Console.WriteLine("Operation canceled.");
                    break;
                case CoordinationStatus.Timeout:
                    Console.WriteLine("Operation timed-out.");
                    break;
                case CoordinationStatus.AllDone:
                    Console.WriteLine("Operation completed; results below:");
                    foreach (var server in servers)
                    {
                        Console.Write("{0} ", server.Key);
                        Object result = server.Value;
                        if (result is Exception)
                        {
                            Console.WriteLine("failed due to {0}.", result.GetType().Name);
                        }
                        else
                        {
                            Console.WriteLine("returned {0:N0} bytes.", result);
                        }
                    }
                    break;
            }
        }
        
        private void ComputeResult(string server, Task<byte[]> task)
        {
            if (task.Exception != null)
            {
                servers[server] = task.Exception;
            }
            else
            {
                // Process I/O completion here on thread pool thread(s)
                // Put your own compute-intensive algorithm here...
                servers[server] = task.Result.Length;
            }
            coordintor.JustEnded();
        }
    }

    internal sealed class AsyncCoordintor
    {
        private int statusReported = 0;
        private int operationCount = 1;
        private Action<CoordinationStatus> callback;
        private Timer timer;

        /// <summary>
        ///     This method MUST be called BEFORE initiating an operation. 
        /// </summary>
        /// <param name="opsToAdd">Operation count to add.</param>
        public void AboutToBegin(int opsToAdd)
        {
            Interlocked.Add(ref operationCount, opsToAdd);
        }

        /// <summary>
        ///     This method MUST be called AFTER an operation’s result has been processed.
        /// </summary>
        public void JustEnded()
        {
            if (Interlocked.Decrement(ref operationCount) == 0)
                ReportStatus(CoordinationStatus.AllDone);
        }

        /// <summary>
        ///     This method MUST be called AFTER initiating ALL operations.
        /// </summary>
        /// <param name="callback">Call when timeout, cancel or all operation done.</param>
        /// <param name="timeout">Operation timeout.</param>
        public void AllDone(Action<CoordinationStatus> callback, int timeout = Timeout.Infinite)
        {
            this.callback = callback;
            if (timeout != Timeout.Infinite)
                timer = new Timer(TimeExpired, null, timeout, Timeout.Infinite);

            JustEnded();
        }

        /// <summary>
        ///     Cancel all operation.
        /// </summary>
        public void Cancel()
        {
            ReportStatus(CoordinationStatus.Cancel);
        }

        private void ReportStatus(CoordinationStatus status) 
        {
            if (Interlocked.Exchange(ref statusReported, 1) == 0)
                ReportStatus(status);
        }

        private void TimeExpired(object state)
        {
            ReportStatus(CoordinationStatus.Timeout);
        }
    }

    internal enum CoordinationStatus
    {
        AllDone,
        Timeout,
        Cancel
    }

    internal struct SimpleSpinLock
    {
        private int inUse;

        public void Enter()
        {
            while(true)
            {
                // Always set resource to in-use
                // When this thread changes it from not in-use, return
                if (Interlocked.Exchange(ref inUse, 1) == 0) return;
                // Black magic goes here...
                // System.Threading.SpinWait is Black magic
            }
        }

        public void Leave()
        {
            // Set resource to not in-use
            Volatile.Write(ref inUse, 0);
        }
    }

    internal sealed class ThreadDelays
    {
        static void Go()
        {
            Thread.Sleep(1); // To sleep 
            Thread.Sleep(0); // This tells the system that the calling thread relinquishes the remainder of its current time-slice, and it forces the system to schedule another thread.
            Thread.Yield();  // The Yield method exists in order to give a thread of equal or lower priority that is starving for CPU time a chance to run. 
            Thread.SpinWait(0); // A thread can force itself to pause, allowing a hyperthreaded CPU to switch to its other thread by calling Thread’s SpinWait method. 
        }
    }
}
