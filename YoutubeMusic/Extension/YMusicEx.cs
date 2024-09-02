using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeMusic.Extension
{
    public class YMusicEx
    {
        public static async Task DisableNavigationBarAsync(bool disabled)
        {
            string visible = disabled ? "hidden" : "visible";
            string script = $@"document.querySelector('ytmusic-nav-bar').style.visibility = '{visible}';";
            await Config.browser.EvaluateScriptAsync(script);
        }

        public static async Task DisableDrawerAsync(bool disabled)
        {
            string visible = disabled ? "hidden" : "visible";
            string script = $@"document.querySelector('tp-yt-app-drawer').style.visibility = '{visible}';";
            await Config.browser.EvaluateScriptAsync(script);
        }

        public static async Task DisablePaperAsync(bool disabled)
        {
            string visible = disabled ? "hidden" : "visible";
            string script = $@"document.querySelector('.toggle-player-page-button').style.visibility = '{visible}';";
            await Config.browser.EvaluateScriptAsync(script);
        }

        public static async Task DisableScrollAsync(bool disabled)
        {
            string overflowValue = disabled ? "hidden" : "auto";
            string script = $"document.body.style.overflow = '{overflowValue}';";
            await Config.browser.EvaluateScriptAsync(script);
        }

        public static async Task DisableMenuAsync(bool disabled)
        {
            string visible = disabled ? "hidden" : "visible";
            string pointer = disabled ? "none" : "auto";
            string script = $@"
            (function() {{
                var middleControls = document.querySelector('.menu.style-scope.ytmusic-player-bar');
                if (middleControls) {{
                    middleControls.style.visibility = '{visible}';
                    middleControls.style.pointerEvents = '{pointer}';
                }}
            }})();";

            await Config.browser.EvaluateScriptAsync(script);
        }

        public static async Task DisableCloseFullScreen(bool disabled)
        {
            string pointer = disabled ? "none" : "auto";
            string script = $@"
            (function() {{
                var middleControls = document.querySelector('.exit-fullscreen-button');
                if (middleControls) {{
                    middleControls.style.pointerEvents = '{pointer}';
                }}
            }})();";

            await Config.browser.EvaluateScriptAsync(script);
        }

        public static async Task DisablePlayerBarPaper(bool disabled)
        {
            string script = $@"
            (function() {{
                var playerBar = document.querySelector('ytmusic-player-bar');
                if (playerBar) {{
                    // 핸들러 함수를 변수로 저장
                    var handler = function(event) {{
                        if (!event.target.closest('tp-yt-paper-icon-button') && !event.target.closest('button') && !event.target.closest('span')) {{
                            event.stopPropagation();
                            event.preventDefault();
                        }}
                    }};

                    // 핸들러를 전역 변수로 저장하여 나중에 참조 가능하게 함
                    if (!window.playerBarHandler) {{
                        window.playerBarHandler = handler;
                    }}

                    if ({(disabled ? "true" : "false")}) {{
                        // 클릭 이벤트 차단
                        playerBar.addEventListener('click', window.playerBarHandler, true);
                    }} else {{
                        // 클릭 이벤트 복원
                        playerBar.removeEventListener('click', window.playerBarHandler, true);
                    }}
                }}
            }})();";

            await Config.browser.EvaluateScriptAsync(script);
        }

        public static async Task<string> GetZIndexAsync(string selector)
        {
            string script = $@"
            (function() {{
                var element = document.querySelector('{selector}');
                return element ? window.getComputedStyle(element).zIndex : 'auto';
            }})();";

            var response = await Config.browser.EvaluateScriptAsync(script);
            return response.Success ? response.Result?.ToString() : "auto";
        }

        public static async Task SetZIndexAsync(string selector, string zIndexValue)
        {
            string script = $@"
            (function() {{
                var element = document.querySelector('{selector}');
                if (element) {{
                    element.style.zIndex = '{zIndexValue}';
                }}
            }})();";

            await Config.browser.EvaluateScriptAsync(script);
        }

        public static async Task ChangeZIndexAsync(bool disabled)
        {
            if (disabled)
            {
                Config.navBarZ = await GetZIndexAsync("ytmusic-nav-bar");
                Config.playerBarZ = await GetZIndexAsync("ytmusic-player-bar");
                await SetZIndexAsync("ytmusic-nav-bar", Config.playerBarZ);
                await SetZIndexAsync("ytmusic-player-bar", Config.navBarZ);
            }
            else
            {
                await SetZIndexAsync("ytmusic-nav-bar", Config.navBarZ);
                await SetZIndexAsync("ytmusic-player-bar", Config.playerBarZ);
            }
        }

        public static async Task PaperButtonToFullScreen(bool disabled)
        {
            string mode = disabled ? "setAttribute" : "removeAttribute";
            string script = $@"
            (function() {{
                var playerBar = document.querySelector('ytmusic-player-bar');
                if (playerBar) {{
                    playerBar.{mode}('player-fullscreened', '');
                }}
            }})();";

            await Config.browser.EvaluateScriptAsync(script);
        }

        public static async Task<bool> IsVideoPlay()
        {
            var script = @"
            (function() {
                var element = document.querySelector('ytmusic-player-bar');
                if (element) {
                    var isVideo = element.hasAttribute('is-video');
                    var visibility = window.getComputedStyle(element).visibility;
                    return {
                        isVideo: isVideo,
                        visible: visibility === 'visible'
                    };
                } else {
                    return { isVideo: false, visible: false };
                }
            })();";

            var response = await Config.browser.EvaluateScriptAsync(script);
            if (response.Success && response.Result is IDictionary<string, object> result)
            {
                bool isVideo = (bool)result["isVideo"];
                bool visible = (bool)result["visible"];
                if (isVideo && visible) return true; //Video playing
            }

            return false;
        }

        public static async Task<bool> IsMusicPlay()
        {
            var script = @"
            (function() {
                var element = document.querySelector('ytmusic-player-bar');
                if (element) {
                    var isVideo = element.hasAttribute('is-video');
                    var visibility = window.getComputedStyle(element).visibility;
                    return {
                        isVideo: isVideo,
                        visible: visibility === 'visible'
                    };
                } else {
                    return { isVideo: false, visible: false };
                }
            })();";

            var response = await Config.browser.EvaluateScriptAsync(script);
            if (response.Success && response.Result is IDictionary<string, object> result)
            {
                bool isVideo = (bool)result["isVideo"];
                bool visible = (bool)result["visible"];
                if (!isVideo && visible) return true; //Music playing
            }

            return false;
        }

        public static async Task<bool> IsPlayerBar() => await IsMusicPlay() || await IsVideoPlay();

        public static async Task InjectMouseMoveScriptAsync()
        {
            string script = @"
            document.addEventListener('mousemove', function(event) {
                CefSharp.PostMessage({ x: event.clientX, y: event.clientY });
            });";

            await Config.browser.GetMainFrame().EvaluateScriptAsync(script);
        }

    }
}
