using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryPressure
{
    internal class BigNativeResource
    {
        private readonly int size;

        public BigNativeResource(int size)
        {
            this.size = size;

            if (this.size > 0) GC.AddMemoryPressure(size);

            Console.WriteLine("BigNativeResource create.");
        }

        ~BigNativeResource()
        {
            if (size > 0) GC.RemoveMemoryPressure(size);

            Console.WriteLine("BigNativeResource destroy.");
        }
    }
}
