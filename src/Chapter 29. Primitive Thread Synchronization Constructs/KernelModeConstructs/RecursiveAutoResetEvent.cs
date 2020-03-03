namespace KernelModeConstructs
{
    using System;
    using System.Threading;

    internal sealed class RecursiveAutoResetEvent
    {
        private readonly AutoResetEvent lockEvent = new AutoResetEvent(true);
        private int owningThreadId = 0;
        private int recursionCount = 0;

        public void Enter()
        {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;

            if (owningThreadId == currentThreadId)
            {
                recursionCount++;
                return;
            }

            lockEvent.WaitOne();
            owningThreadId = currentThreadId;
            recursionCount = 1;
        }

        public void Leave()
        {
            if (owningThreadId != Thread.CurrentThread.ManagedThreadId)
                throw new InvalidOperationException();

            recursionCount--;
            if (recursionCount == 0)
            {
                owningThreadId = 0;
                lockEvent.Set();
            }
        }
    }
}
