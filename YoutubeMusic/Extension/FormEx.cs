using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoutubeMusic.Extension
{
    public class FormEx
    {
        public static void SimpleToOriginResolution(float scale)
        {
            // 현재 폼의 원래 크기 및 위치를 저장
            var originalSize = Form1.Instance.Size;
            var originalLocation = Form1.Instance.Location;

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
            Form1.Instance.Size = new Size(newWidth, newHeight);

            // 크기 조정 후 위치를 조정하여 타이틀바의 좌측 상단이 고정되도록 설정
            Form1.Instance.Location = new Point(originalLocation.X, originalLocation.Y);

            // 추가적으로, 폼이 화면 밖으로 나가지 않도록 보정
            var formBounds = new Rectangle(Form1.Instance.Location, Form1.Instance.Size);
            if (!resolution.Contains(formBounds))
            {
                int newX = Math.Max(resolution.X, Math.Min(resolution.Right - Form1.Instance.Width, Form1.Instance.Location.X));
                int newY = Math.Max(resolution.Y, Math.Min(resolution.Bottom - Form1.Instance.Height, Form1.Instance.Location.Y));
                Form1.Instance.Location = new Point(newX, newY);
            }
        }

        public static void ResizeFormWithResolution(float scale)
        {
            // 현재 폼의 원래 크기 및 위치를 저장
            var originalSize = Form1.Instance.Size;
            var originalLocation = Form1.Instance.Location;

            //폼이 위치한 현재 모니터 해상도 가져오기
            var currentScreen = Screen.FromControl(Form1.Instance);
            var resolution = currentScreen.Bounds;

            // 새로운 크기 계산
            int newWidth = (int)(resolution.Width * scale);
            int newHeight = (int)(resolution.Height * scale);
            Form1.Instance.Size = new Size(newWidth, newHeight);

            // 크기 조정 후 위치를 원래대로 조정
            int offsetX = (originalSize.Width - Form1.Instance.Width) / 2;
            int offsetY = (originalSize.Height - Form1.Instance.Height) / 2;
            Form1.Instance.Location = new Point(originalLocation.X + offsetX, originalLocation.Y + offsetY);
        }

        public static float GetResolutionScaleFactor()
        {
            float baseScreenWidth = 1920;
            float currentScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            return currentScreenWidth / baseScreenWidth;
        }

        public static int GetTitleBarHeight()
        {
            int titleBarHeight = SystemInformation.CaptionHeight;
            return (int)(titleBarHeight * 1.25f);
        }
    }
}
