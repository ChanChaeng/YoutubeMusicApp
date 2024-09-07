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
        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
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

                if (cursorPos.Y < GetTitleBarHeight()) m.Result = IntPtr.Zero;
                else if(cursorPos.X >= this.ClientSize.Width - thickness) // Right edge
                {
                    m.Result = cursorPos.Y >= this.ClientSize.Height - areaSize ? (IntPtr)HTBOTTOMRIGHT : (IntPtr)HTRIGHT;
                }
                else if (cursorPos.Y >= this.ClientSize.Height - thickness) m.Result = (IntPtr)HTBOTTOM;
                else m.Result = IntPtr.Zero;

                return;
            }

            base.WndProc(ref m);
        }

        private void SimpleToOriginResolution(float scale)
        {
            // 현재 폼의 원래 크기 및 위치를 저장
            var originalSize = this.Size;
            var originalLocation = this.Location;

            // 메인 모니터 해상도 가져오기
            var primaryScreen = Screen.PrimaryScreen;
            var resolution = primaryScreen.Bounds;

            // 새로운 크기 계산
            int newWidth = (int)(resolution.Width * scale);
            int newHeight = (int)(resolution.Height * scale);

            // 폼의 새 크기가 모니터 해상도를 초과하지 않도록 조정
            newWidth = Math.Min(newWidth, resolution.Width);
            newHeight = Math.Min(newHeight, resolution.Height);

            // 폼의 크기를 조정
            this.Size = new Size(newWidth, newHeight);

            // 크기 조정 후 위치를 조정하여 타이틀바의 좌측 상단이 고정되도록 설정
            this.Location = new Point(originalLocation.X, originalLocation.Y);

            // 추가적으로, 폼이 화면 밖으로 나가지 않도록 보정
            var formBounds = new Rectangle(this.Location, this.Size);
            if (!resolution.Contains(formBounds))
            {
                int newX = Math.Max(resolution.X, Math.Min(resolution.Right - this.Width, this.Location.X));
                int newY = Math.Max(resolution.Y, Math.Min(resolution.Bottom - this.Height, this.Location.Y));
                this.Location = new Point(newX, newY);
            }
        }

        private void ResizeFormWithResolution(float scale)
        {
            // 현재 폼의 원래 크기 및 위치를 저장
            var originalSize = this.Size;
            var originalLocation = this.Location;

            //메인 모니터 해상도 가져오기
            var currentScreen = Screen.FromControl(this); // 'this'는 현재 폼을 나타냄
            var resolution = currentScreen.Bounds; // 해상도 정보
            //var primaryScreen = Screen.PrimaryScreen;
            //var resolution = primaryScreen.Bounds;

            // 새로운 크기 계산
            int newWidth = (int)(resolution.Width * scale);
            int newHeight = (int)(resolution.Height * scale);
            this.Size = new Size(newWidth, newHeight);

            // 크기 조정 후 위치를 원래대로 조정
            int offsetX = (originalSize.Width - this.Width) / 2;
            int offsetY = (originalSize.Height - this.Height) / 2;
            this.Location = new Point(originalLocation.X + offsetX, originalLocation.Y + offsetY);
        }

        private float GetResolutionScaleFactor()
        {
            float baseScreenWidth = 1920;
            float currentScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            return currentScreenWidth / baseScreenWidth;
        }

        private int GetTitleBarHeight()
        {
            int titleBarHeight = SystemInformation.CaptionHeight;
            return (int)(titleBarHeight * 1.25f);
        }

        private void InitUI()
        {
            int titleHeight = GetTitleBarHeight();
            TitlePanel.Height = titleHeight;

            // Rectangle Button
            BackButton.Size = new Size(titleHeight + 10, titleHeight);
            SimpleModeButton.Size = new Size(titleHeight + 10, titleHeight);

            // Square Button
            Minimize_Button.Size = new Size(titleHeight, titleHeight);
            MaximizeButton.Size = new Size(titleHeight, titleHeight);
            ExitButton.Size = new Size(titleHeight, titleHeight);

            // Form Resize and CenterStart
            ResizeFormWithResolution(scale: 0.8f);
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
                MemoryManager.StartMemoryManagement(MemoryManagerMode.Low);
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
                int titleHeight = GetTitleBarHeight();
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

            if (!enbled) SimpleToOriginResolution(0.8f);
        }

        private async void SimpleModeButton_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            if (await YMusicEx.IsPlayerBar())
            {
                if (!Config.IsSimpleMode && await YMusicEx.IsVideoPlay()) return;

                Config.IsSimpleMode = !Config.IsSimpleMode;
                await ChangeSimpleMode(Config.IsSimpleMode);

                if (Config.IsSimpleMode) SimpleModeButton.BackgroundImage = Properties.Resources.ZoomIn_White_320;
                else SimpleModeButton.BackgroundImage = Properties.Resources.ZoomOut_White_320;
            }
        }

        private void ToggleFullScreen(bool isMaximized)
        {
            if (isMaximized)
            {
                TitlePanel.Visible = false;
                TitlePanel.Height = 0;
                SpacePanel.Visible = false;
                SpacePanel.Height = 0;
                this.Padding = new Padding(0, 0, 0, 0);
            }
            else
            {
                TitlePanel.Visible = true;
                TitlePanel.Height = GetTitleBarHeight();
                SpacePanel.Visible = true;
                SpacePanel.Height = 1;
                this.Padding = new Padding(1, 1, 1, 1);
            }
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
