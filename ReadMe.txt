SC vJoy Server V 2.7 - Build 30 BETA
(c) Cassini - 26-Mar-2020

Contains 6 files:

SCJoyServer.exe              The program (V2.7-B30)
x64\SCdxKeyboard.dll         Native dll for Keyboard (v2.00)      - MUST be in the same folder as the Exe file
x86\SCdxKeyboard.dll         Native dll for Keyboard (v2.00)      - MUST be in the same folder as the Exe file
x64\vJoyInterface.dll        Native dll for vJoy (v2.00)          - MUST be in the same folder as the Exe file
x86\vJoyInterface.dll        Native dll for vJoy (v2.00)          - MUST be in the same folder as the Exe file

ReadMe.txt                   This file

SC Virtual Joystick Server (.Net 4.7.2)

Put all files into one folder and hit SCJoyServer.exe to run it

For Updates and information visit:
https://github.com/SCToolsfactory/SCJoyServer

Scanned for viruses before packing... 
cassini@burri-web.org

Changelog:
V 2.7-B30
- update Process multiple commands from one UDP message
V 2.6-B29
- Removed Breaking change...
- add VKcodeEx as string element and reverted to VKcode as int code to maintain compatibility with V2.4 (see command ref)
V 2.5-B28
- BREAKING CHANGE:
  Key Command: VKcode field must be a string now - can contain a number OR a VK_KEYCODE string 
  - see SCdxKeycodes.cs for valid names
- add Key Modifier accepts multiple modifiers separated by & ("ls&la" for LeftShift+LeftAlt)
- update to .Net 4.7.2
- refact. Json parsing via .Net de-serialization
- fix - no delay if kbd not enabled
V 2.4-B27
- fix - Init Keyserver disabled
V 2.3-B26
- update for new SCRemoteServer Upload API
V 2.2-B25
- added multiple vJoy servers for any number of vJoysticks
V 2.1-B24
- added persistence for settings and robustnes when upload files are still locked
- added asynch. upload of files
V 2.0-B23
- added File monitor and uploader with http POST request
V 2.0-B22
- added L/RShift modifier
- added Short Tab command for convenience (very short down-up for button or key)
- added checkbox to disable keystroke sending at the lowest level (debug still possible)
V 2.0
- complete rework 
- The command is now Json syntax, supports also keystrokes sent to the active window.
V 1.0 initial 
