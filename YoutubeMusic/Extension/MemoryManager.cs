using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeMusic.Extension
{
    public class MemoryManager
    {
        public static void LimitMemoryUsage(Process process, long maxMemoryMB)
        {
            IntPtr handle = OpenProcess(PROCESS_SET_QUOTA | PROCESS_QUERY_INFORMATION, false, process.Id);

            if (handle == IntPtr.Zero) return;

            try
            {
                IntPtr maxMemoryBytes = new IntPtr(maxMemoryMB * 1024 * 1024);
                SetProcessWorkingSetSize(handle, maxMemoryBytes, maxMemoryBytes);
            }
            finally
            {
                CloseHandle(handle);
            }
        }

        private const uint PROCESS_SET_QUOTA = 0x0100;
        private const uint PROCESS_QUERY_INFORMATION = 0x0400;

        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr process, IntPtr minimumWorkingSetSize, IntPtr maximumWorkingSetSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

    }
}
