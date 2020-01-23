using System;

namespace MemoryPressure
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            //Causes infrequent GCs 
            MemoryPressureDemo(0);

            //10MB. Causes frequent GCs
            MemoryPressureDemo(10 * 1024 * 1024);

            Console.ReadKey();
        }

        private static void MemoryPressureDemo(int size)
        {
            Console.WriteLine();
            Console.WriteLine("Size={0}", size);

            for (var i = 0; i < 15; i++)
                new BigNativeResource(size);

            GC.Collect();
        }
    }
}
