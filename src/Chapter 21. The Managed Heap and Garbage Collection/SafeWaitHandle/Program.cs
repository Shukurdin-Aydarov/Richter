using System;
using System.Threading;

namespace SafeWaitHandle
{
    internal static class Program
    {
        static void Main()
        {
            while(true)
            {
                var thread = new Thread(_ =>
                {
                    // Memory leak with IntPtr when thread aborted
                    var (ptr, swh) = IntPtrVsSafeWaitHandle.Create();

                    Console.WriteLine("IntPtr = {0}, SafeWaitHandle = {1}", ptr, swh);
                });

                thread.Start();

                Thread.Sleep(500);

                thread.Abort();
            }
        }
    }
}
