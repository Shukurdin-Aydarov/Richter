using System;

using CompilerServices = System.Runtime.CompilerServices;

namespace ConditionalWeakTable
{
    internal static class GCWacher
    {
        private static readonly CompilerServices.ConditionalWeakTable<object, GCCompleted<string>> weakTable =
            new CompilerServices.ConditionalWeakTable<object, GCCompleted<string>>();

        public static T GCWatch<T>(this T obj, string tag)
        {
            weakTable.Add(obj, new GCCompleted<string>(tag));

            return obj;
        }
    }

    internal class GCCompleted<T>
    {
        private readonly T tag;

        public GCCompleted(T tag)
        {
            this.tag = tag;
        }

        public override string ToString()
        {
            return tag.ToString();
        }

        ~GCCompleted()
        {
            Console.WriteLine("GC'd: {0}", tag);
        }
    }
}
