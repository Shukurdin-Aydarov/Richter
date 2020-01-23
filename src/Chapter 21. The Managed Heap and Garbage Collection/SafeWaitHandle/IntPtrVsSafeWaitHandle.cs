using System;
using System.Runtime.InteropServices;

namespace SafeWaitHandle
{
    internal static class IntPtrVsSafeWaitHandle
    {
        [DllImport("Kernel32", CharSet = CharSet.Unicode, EntryPoint="CreateEvent")]
        private static extern IntPtr CreateEventAsIntPrt( // <- this prototype is not robust
            IntPtr pSecurityAttribute,
            bool manualReset,
            bool initialSate,
            string name);

        [DllImport("Kernel32", CharSet = CharSet.Unicode, EntryPoint = "CreateEvent")]
        private static extern Microsoft.Win32.SafeHandles.SafeWaitHandle CreateEventAsSafeWaitHandle( // <- is robust
            IntPtr pSecurityAttribute,
            bool manualReset,
            bool initialSate,
            string name);

        public static (IntPtr ptr, Microsoft.Win32.SafeHandles.SafeWaitHandle safeWait) Create()
        {
            var handle = CreateEventAsIntPrt(IntPtr.Zero, false, false, null);
            var swh = CreateEventAsSafeWaitHandle(IntPtr.Zero, false, false, null);

            return (handle, swh);
        }
    }
}
