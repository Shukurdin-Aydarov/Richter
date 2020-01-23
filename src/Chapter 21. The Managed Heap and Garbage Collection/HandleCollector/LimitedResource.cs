using System;
using NativeCollector = System.Runtime.InteropServices.HandleCollector;

namespace HandleCollection
{
    internal sealed class LimitedResource
    {
        private static readonly NativeCollector collector = new NativeCollector("LimitedResource", 2);

        public LimitedResource()
        {
            collector.Add();

            Console.WriteLine("LimitedResource create. Count={0}", collector.Count);
        }

        ~LimitedResource()
        {
            collector.Remove();

            Console.WriteLine("LimitedResource destroy. Count={0}", collector.Count);
        }
    }
}
