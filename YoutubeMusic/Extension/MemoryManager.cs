using CefSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YoutubeMusic.Extension
{
    public enum MemoryManagerMode : int
    {
        Low = 45,
        Default = 70,
        Performance = 100
    }

    public class MemoryManager
    {
        private static CancellationTokenSource _memoryCts;
        private static long _maxMemoryMB;
        private static int _checkInterval = 500;
        private static double _memoryThreshold = 0.8;

        public static async void Start(MemoryManagerMode mode)
        {
            _memoryCts = new CancellationTokenSource();

            _ = ManageMemoryAsync(Process.GetCurrentProcess(), maxMemoryMB: 10, _memoryCts.Token);

            Process[] subprocesses = Process.GetProcessesByName("CefSharp.BrowserSubprocess");
            foreach (var subprocess in subprocesses)
            {
                _ = ManageMemoryAsync(subprocess, maxMemoryMB: (int)mode, _memoryCts.Token);
            }

            // GC calls and browser memory cleanup
            while (!_memoryCts.Token.IsCancellationRequested)
            {
                try { await Task.Delay(30000, _memoryCts.Token); }
                catch { break; }

                Config.browser.ExecuteScriptAsync("window.gc();");

                long memoryUsage = Process.GetCurrentProcess().WorkingSet64;
                if (memoryUsage > (100 * 1024 * 1024)) // If it is more than 100MB call GC
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }

        public static void Stop()
        {
            _memoryCts?.Cancel();
            _memoryCts?.Dispose();
        }

        private static async Task ManageMemoryAsync(Process process, long maxMemoryMB, CancellationToken cancellationToken)
        {
            _maxMemoryMB = maxMemoryMB;

            while (!cancellationToken.IsCancellationRequested)
            {
                long currentMemoryMB = process.WorkingSet64 / (1024 * 1024);
                double memoryUsageRatio = (double)currentMemoryMB / _maxMemoryMB;

                if (memoryUsageRatio > _memoryThreshold)
                {
                    LimitMemoryUsage(process, _maxMemoryMB);
                    EmptyWorkingSet(process.Handle);

                    if (memoryUsageRatio > 0.95)
                    {
                        process.PriorityClass = ProcessPriorityClass.BelowNormal;
                        _checkInterval = 250; // Check more frequently
                    }
                }
                else
                {
                    process.PriorityClass = ProcessPriorityClass.Normal;
                    _checkInterval = 1000; // Check less frequently
                }

                await Task.Delay(_checkInterval, cancellationToken);
                _checkInterval = 500;
            }
        }

        private static void LimitMemoryUsage(Process process, long maxMemoryMB)
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

        [DllImport("psapi.dll")]
        private static extern int EmptyWorkingSet(IntPtr hwProc);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

    }
}
