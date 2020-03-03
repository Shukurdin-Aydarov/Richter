namespace KernelModeConstructs
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    public sealed class SingleApplicationInstance
    {
        public static void Run()
        {
            using(new Semaphore(0, 1, "MyApp", out var createdNew))
            {
                if (createdNew)
                {
                    // This thread created the kernel object so no other instance of this
                    // application must be running. Run the rest of the application here...
                }
                else
                {
                    // This thread opened an existing kernel object with the same string name;
                    // another instance of this application must be running now.
                    // There is nothing to do in here, let's just return from Main to terminate
                    // this second instance of the application.
                }
            }
        }
    }
}
