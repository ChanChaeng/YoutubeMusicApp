# Youtube Music PC용
- 공식앱 보다 더 많은 메모리 사용량
- 유용한 추가 기능 없음
- 직접 기능을 구현한 것이 아닌 CEF 사용
- 닷넷 프레임워크 4.8 WinForm으로 제작됨
<br><br>

### Language
[English](docs/en/README.md)  
<br><br>

### 존재 이유
Playwright로 크롬 대신 엣지를 사용할 때 PWA기반인 유튜브 뮤직이랑 충돌해서  
해당 작업을 하는 동안 노래가 듣고 싶었기 때문에 빠르고 간단한 방법으로 이 프로그램을 만들었습니다  
<br><br>

### 미리보기
![Preview](https://github.com/ChanChaeng/YoutubeMusicApp/blob/master/img/YoutubeMusicApp.png)  
<br><br>

### 심플 모드 (음악 감상 모드)
![SimpleMode](https://github.com/ChanChaeng/YoutubeMusicApp/blob/master/img/YoutubeMusicSimpleMode.png)  
<br><br>

### 전체화면 (풀스크린)
![FullScreen](https://github.com/ChanChaeng/YoutubeMusicApp/blob/master/img/YoutubeMusicFullScreen.gif)  
<br><br>

### 메모리 사용량
![MemoryUsage](https://github.com/ChanChaeng/YoutubeMusicApp/blob/master/img/YoutubeMusicMemory.png)  
- 경량 모드 - 프로세스당 최대 45/MB 제한
- 기본 모드 - 프로세스당 최대 70/MB 제한
- 성능 모드 - 프로세스당 최대 100/MB 제한
<br><br>

### 캐시 및 쿠키 삭제 (로그인 정보 삭제)
- `C:\Users\UserName\AppData\Local\YouTubeMusicApp\Cache` 폴더 삭제
