using System;

namespace ConditionalWeakTable
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var obj = new object().GCWatch("My object created at " + DateTime.Now);
            
            GC.Collect();
            GC.KeepAlive(obj);

            obj = null;

            GC.Collect(); // We'll see the GC notification sometime after this line
            Console.ReadKey();
        }
    }
}
