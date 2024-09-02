using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoutubeMusic
{
    internal static class Program
    {
        private static readonly string MutexName = "Global\\YoutubeMusicDesktopApp";
        private static Mutex _mutex;

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isNewInstance;
            _mutex = new Mutex(true, MutexName, out isNewInstance);
            if (!isNewInstance) return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Cef.Initialize(Config.GetCefSettings());
            Config.browser = new ChromiumWebBrowser("https://music.youtube.com/") { Dock = DockStyle.Fill };

            Application.Run(new Form1());
            _mutex.ReleaseMutex();
        }
    }
}
