using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandleCollection
{
    internal static class Program
    {
        static void Main()
        {
            HandleCollectorDemo();

            Console.ReadKey();
        }

        private static void HandleCollectorDemo()
        {
            for (var i = 0; i < 15; i++) new LimitedResource();

            GC.Collect();
        }
    }
}
