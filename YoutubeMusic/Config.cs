using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoutubeMusic
{
    public class Config
    {
        public static ChromiumWebBrowser browser;
        public static string settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "YouTubeMusicApp");
        public static string cachePath = Path.Combine(settingsPath, "Cache");

        public static bool IsSimpleMode = false;
        public static bool IsLoadEnd = false;
        public static string navBarZ;
        public static string playerBarZ;

        public static CefSettings GetCefSettings()
        {
            string userLanguage = CultureInfo.CurrentCulture.Name; // Example: "ko-KR"
            var settings = new CefSettings
            {
                Locale = userLanguage, // Example: "ko-KR"
                AcceptLanguageList = userLanguage + $",{userLanguage.Substring(0, 2)}", // Example: "ko-KR,ko"
                CachePath = cachePath,
            };

            settings.CefCommandLineArgs.Add("disable-infobars");
            settings.CefCommandLineArgs.Add("disable-extensions");
            settings.CefCommandLineArgs.Add("disable-plugins");
            settings.CefCommandLineArgs.Add("disable-speech-api");
            settings.CefCommandLineArgs.Add("disable-popup-blocking");
            settings.CefCommandLineArgs.Add("disable-logging");

            settings.CefCommandLineArgs.Add("no-first-run");
            settings.CefCommandLineArgs.Add("disable-application-cache", "1");

            //settings.CefCommandLineArgs.Add("js-flags", "--max-old-space-size=128"); //Low(unstable) = 96, average(Sometimes unstable) = 128
            //settings.CefCommandLineArgs.Add("disk-cache-size", "10485760"); // 10MB (Don't limit cache size, maintain performance)

            return settings;
        }

    }
}
