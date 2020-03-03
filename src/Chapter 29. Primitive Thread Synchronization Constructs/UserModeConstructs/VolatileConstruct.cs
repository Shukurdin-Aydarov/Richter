namespace UserModeConstructs
{
    using System;
    using System.Threading;

    internal sealed class ThreadsSharingData
    {
        private int flag = 0;
        private int val = 0;

        public void Thread1()
        {
            // Note: 5 must be written to val before 1 is written to flag
            val = 5;
            Volatile.Write(ref flag, 1);
        }

        public void Thread2()
        {
            // Note: val must be read after flag is read 
            if (System.Threading.Volatile.Read(ref flag) == 1)
                Console.WriteLine(val);
        }
    }

    internal sealed class ThreadSharedDataVolitileKeyWord
    {
        private int val = 0;
        private volatile int flag = 0;

        public void Thread1()
        {
            // Note: 5 must be written to val before 1 is written to flag
            val = 5;
            System.Threading.Volatile.Write(ref flag, 1);
        }

        public void Thread2()
        {
            // Note: val must be read after flag is read 
            if (System.Threading.Volatile.Read(ref flag) == 1)
                Console.WriteLine(val);
        }
    }
}
