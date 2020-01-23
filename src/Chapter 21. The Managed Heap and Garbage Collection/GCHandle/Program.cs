using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCHandle
{
    internal static class Program
    {
        static void Main()
        {
            Go();

            Console.ReadKey();
        }

        unsafe public static void Go()
        {
            for (var x = 0; x < 10000; x++) new object();

            IntPtr originalMmeoryAddress;
            var bytes = new byte[1000];

            fixed(byte* pbytes = bytes) { originalMmeoryAddress = (IntPtr)pbytes; }

            GC.Collect();

            fixed(byte* pbytes = bytes)
            {
                Console.WriteLine("The Byte[] did{0} move during the GC",
                    (originalMmeoryAddress == (IntPtr) pbytes) ? " not" : null);
            }
        }
    }
}
