using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeMusic.Extension
{
    public class TitleBar
    {
        public static void Move(IntPtr hWnd)
        {
            ReleaseCapture();
            SendMessage(hWnd, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;
    }
}
