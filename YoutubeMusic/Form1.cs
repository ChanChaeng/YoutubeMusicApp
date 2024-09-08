using CefSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using YoutubeMusic.Extension;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace YoutubeMusic
{
    public partial class Form1 : Form
    {
        public static Form Instance;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            Instance = this;
        }

        // Form Border
        protected override void OnPaint(PaintEventArgs e)
        {
            int BORDER_SIZE = 4;
            Color color = Color.FromArgb(255, 33, 33, 33);
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                                         color, BORDER_SIZE, ButtonBorderStyle.Solid,
                                         color, BORDER_SIZE, ButtonBorderStyle.Solid,
                                         color, BORDER_SIZE, ButtonBorderStyle.Solid,
                                         color, BORDER_SIZE, ButtonBorderStyle.Solid);
            base.OnPaint(e);
        }

        // Minimize from taskbar
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x20000; // WS_MINIMIZEBOX
                cp.ClassStyle |= 0x8; // CS_DBLCLKS
                return cp;
            }
        }

        // Form Resize
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTLEFT = 10, HTRIGHT = 11, HTTOP = 12, HTTOPLEFT = 13,
                      HTTOPRIGHT = 14, HTBOTTOM = 15, HTBOTTOMLEFT = 16, HTBOTTOMRIGHT = 17;

            if (m.Msg == WM_NCHITTEST)
            {
                var cursorPos = this.PointToClient(Cursor.Position);
                const int thickness = 30;
                const int areaSize = 10;

                if (cursorPos.Y < FormEx.GetTitleBarHeight()) m.Result = IntPtr.Zero;
                else if(cursorPos.X >= this.ClientSize.Width - thickness) // Right edge
                {
                    m.Result = cursorPos.Y >= this.ClientSize.Height - areaSize ? (IntPtr)HTBOTTOMRIGHT : (IntPtr)HTRIGHT;
                }
                else if (cursorPos.Y >= this.ClientSize.Height - thickness) m.Result = (IntPtr)HTBOTTOM;
                else base.WndProc(ref m);
                return;
            }

            base.WndProc(ref m);
        }

        private void InitUI()
        {
            int titleHeight = FormEx.GetTitleBarHeight();
            TitlePanel.Height = titleHeight;

            // Rectangle Button
            BackButton.Size = new Size(titleHeight + 10, titleHeight);
            SimpleModeButton.Size = new Size(titleHeight + 10, titleHeight);

            // Square Button
            Minimize_Button.Size = new Size(titleHeight, titleHeight);
            MaximizeButton.Size = new Size(titleHeight, titleHeight);
            ExitButton.Size = new Size(titleHeight, titleHeight);

            // Form Resize and CenterStart
            FormEx.ResizeFormWithResolution(scale: 0.8f);
        }

        private EventHandler<FrameLoadEndEventArgs> handler = null;
        private async void YoutubeMusic_Load(object sender, EventArgs e)
        {
            InitUI();

            // browser initialization is in Program.cs
            BrowserPanel.Controls.Add(Config.browser);

            // Covers the white background with a background color while waiting for the page to load
            Panel overlayPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = this.BackColor
            };
            this.Controls.Add(overlayPanel);
            overlayPanel.BringToFront();

            handler = async (_, args) =>
            {
                // Remove event handler
                Config.browser.FrameLoadEnd -= handler;

                this.Invoke(new Action(() =>
                {
                    Controls.Remove(overlayPanel);
                    overlayPanel.Dispose();
                }));

                await YMusicEx.InjectMouseMoveScriptAsync(); // Injection Mouse Move Event Listener
                MemoryManager.Start(MemoryManagerMode.Low);
            };
            Config.browser.FrameLoadEnd += handler;
            Config.browser.JavascriptMessageReceived += Browser_JavascriptMessageReceived; // Mouse Move Receive

            login_check_timer.Enabled = !CheckLoginStatus();
        }

        private void Browser_JavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {
            var message = e.Message as IDictionary<string, object>;
            if (message == null) return;

            int mouseX = Convert.ToInt32(message["x"]);
            int mouseY = Convert.ToInt32(message["y"]);

            // Choose whether to show the title bar based on the mouse position in full screen mode
            Invoke((MethodInvoker)delegate ()
            {
                int titleHeight = FormEx.GetTitleBarHeight();
                if (mouseY <= titleHeight)
                {
                    TitlePanel.Height = titleHeight;
                    SpacePanel.Visible = true;
                    TitlePanel.Visible = true;
                }
                else if (this.WindowState == FormWindowState.Maximized)
                {
                    TitlePanel.Visible = false;
                }
            });
        }

        private void ClearDirectory(string path)
        {
            if (Directory.Exists(path)) Directory.Delete(path, true);
        }
        private async Task ClearCache() => await Config.browser.GetDevToolsClient().Network.ClearBrowserCacheAsync();
        private async void YoutubeMusic_FormClosing(object sender, FormClosingEventArgs e)
        {
            MemoryManager.Stop();
            await ClearCache();
            Cef.Shutdown();

            ClearDirectory(Config.cachePath + "\\component_crx_cache");
            ClearDirectory(Config.cachePath + "\\GraphiteDawnCache");
            ClearDirectory(Config.cachePath + "\\GrShaderCache");
            ClearDirectory(Config.cachePath + "\\ShaderCache");

            string userData = Path.Combine(Config.cachePath, "Default");
            ClearDirectory(userData + "\\blob_storage");
            ClearDirectory(userData + "\\Cache");
            ClearDirectory(userData + "\\Code Cache");
            ClearDirectory(userData + "\\DawnGraphiteCache");
            ClearDirectory(userData + "\\DawnWebGPUCache");
            ClearDirectory(userData + "\\GCM Store");
            ClearDirectory(userData + "\\GPUCache");
            ClearDirectory(userData + "\\Local Storage");
            ClearDirectory(userData + "\\Session Storage");
            ClearDirectory(userData + "\\Shared Dictionary");
            ClearDirectory(userData + "\\WebStorage");
        }

        private bool CheckLoginStatus()
        {
            var cookies = Cef.GetGlobalCookieManager().VisitAllCookiesAsync().Result;
            return cookies.Where(cookie => cookie.Name.Contains("LOGIN_INFO")).ToList().Any();
        }

        private void login_check_timer_Tick(object sender, EventArgs e)
        {
            if (CheckLoginStatus())
            {
                Config.browser.Load("https://music.youtube.com/");
                login_check_timer.Enabled = false;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            Application.Exit();
        }

        private void MaximizeButton_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            if (Config.IsSimpleMode) return;
            this.WindowState = this.WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        }

        private void Minimize_Button_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            this.WindowState = FormWindowState.Minimized;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            Config.browser.Back();
        }

        private async Task ChangeSimpleMode(bool enbled)
        {
            //stuttering effect by first reducing the form size before hiding the elements
            await YMusicEx.ChangeZIndexAsync(enbled);
            if (enbled) this.Size = new Size(1160, 90 + TitlePanel.Height);

            await YMusicEx.DisableDrawerAsync(enbled);
            await YMusicEx.DisableScrollAsync(enbled);

            await YMusicEx.PaperButtonToFullScreen(enbled);
            await YMusicEx.DisableCloseFullScreen(enbled);
            await YMusicEx.DisablePlayerBarPaper(enbled);
            await YMusicEx.DisableMenuAsync(enbled);

            if (this.WindowState == FormWindowState.Maximized) this.WindowState = FormWindowState.Normal;
            ToggleFullScreen(false);

            if (!enbled) FormEx.SimpleToOriginResolution(0.8f);
        }

        private async void SimpleModeButton_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            if (!await YMusicEx.IsPlayerBar()) return;
            if (!Config.IsSimpleMode && await YMusicEx.IsVideoPlay()) return;

            Config.IsSimpleMode = !Config.IsSimpleMode;
            await ChangeSimpleMode(Config.IsSimpleMode);
            SimpleModeButton.BackgroundImage = Config.IsSimpleMode ? Properties.Resources.ZoomIn_White_320 : Properties.Resources.ZoomOut_White_320;
        }

        private void ToggleFullScreen(bool isMaximized)
        {
            TitlePanel.Visible = !isMaximized;
            TitlePanel.Height = isMaximized ? 0 : FormEx.GetTitleBarHeight();
            SpacePanel.Visible = !isMaximized;
            SpacePanel.Height = isMaximized ? 0 : 1;
            this.Padding = isMaximized ? new Padding(0, 0, 0, 0) : new Padding(1, 1, 1, 1);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ToggleFullScreen(this.WindowState == FormWindowState.Maximized);
        }

        private void TitlePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) TitleBar.Move(Handle);
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            if (!Config.IsSimpleMode) InitUI();
        }
    }
}
