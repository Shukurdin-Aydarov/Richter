namespace KernelModeConstructs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
    public class SimpleWaitLock : IDisposable
    {
        private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(true); // Initially free

        public void Enter()
        {
            autoResetEvent.WaitOne();
        }

        public void Leave()
        {
            autoResetEvent.Set();
        }

        public void Dispose() => autoResetEvent.Dispose();
    }

    public static class SimpleWaitLockTest
    {
        public static void Go()
        {
            var x = 0;
            const int iterations = 10000000; 
            
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < iterations; i++)
                x++;

            Console.WriteLine("Incrementing x: {0:N0} ms", sw.ElapsedMilliseconds);

            // How long does it take to increment x 10 million times
            // adding the overhead of calling a method that does nothing?
            sw.Restart();
            for (Int32 i = 0; i < iterations; i++)
            {
                M(); x++; M();
            }
            Console.WriteLine("Incrementing x in M: {0:N0} ms", sw.ElapsedMilliseconds);

            // How long does it take to increment x 10 million times
            // adding the overhead of calling an uncontended SimpleSpinLock?
            SpinLock sl = new SpinLock(false);
            sw.Restart();
            for (Int32 i = 0; i < iterations; i++)
            {
                Boolean taken = false; sl.Enter(ref taken); x++; sl.Exit();
            }
            Console.WriteLine("Incrementing x in SpinLock: {0:N0} ms", sw.ElapsedMilliseconds);
            // How long does it take to increment x 10 million times
            // adding the overhead of calling an uncontended SimpleWaitLock?
            using (SimpleWaitLock swl = new SimpleWaitLock())
            {
                sw.Restart();
                for (Int32 i = 0; i < iterations; i++)
                {
                    swl.Enter(); x++; swl.Leave();
                }
                Console.WriteLine("Incrementing x in SimpleWaitLock: {0:N0} ms", sw.ElapsedMilliseconds);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void M() { /* This method does nothing but return */ }
    } 

    public sealed class SimpleWaitLockOnSemaphor
    {
        private readonly Semaphore semaphore;

        public SimpleWaitLockOnSemaphor(int maxCuncurrent)
        {
            semaphore = new Semaphore(maxCuncurrent, maxCuncurrent);
        }

        public void Enter()
        {
            // Block in kernel until resource available
            semaphore.WaitOne();
        }

        public void Leave()
        {
            // Let another thread access the resource
            semaphore.Release(1);
        }
    }
}
