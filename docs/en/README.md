# Youtube Music for Desktop
- Uses more memory than the official app
- No useful additional features  
- Uses CEF (Chromium Embedded Framework) instead of implementing the functionality directly
- Created using .NET Framework 4.8 WinForm
<br><br>

### Reason for existence
When I use Edge instead of Chrome with Playwright, it conflicts with YouTube Music App which is PWA-based  
so I wanted to listen to music while doing that work, so I created this program in a quick and easy way
<br><br>

### Preview
![Preview](https://github.com/ChanChaeng/YoutubeMusicApp/blob/master/img/YoutubeMusicApp.png)  
<br><br>

### Simple Mode (Music Listening Mode)
![SimpleMode](https://github.com/ChanChaeng/YoutubeMusicApp/blob/master/img/YoutubeMusicSimpleMode.png)  
<br><br>

### Full Screen
![FullScreen](https://github.com/ChanChaeng/YoutubeMusicApp/blob/master/img/YoutubeMusicFullScreen.gif)  
<br><br>

### Memory Usage
![MemoryUsage](https://github.com/ChanChaeng/YoutubeMusicApp/blob/master/img/YoutubeMusicMemory.png)  
- Minimum - 10~20/MB
- Average - 30~40/MB
- Maximum - 100~250/MB (intermittent memory usage increases)
- I implemented the function simply by referring to [Firemin](https://github.com/Sylfaen2/Firemin)
<br><br>

### Delete cache and cookies (Delete login session)
- `C:\Users\UserName\AppData\Local\YouTubeMusicApp\Cache` Delete folder
