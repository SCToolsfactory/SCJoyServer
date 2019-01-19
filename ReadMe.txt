SC vJoy Server V 2.2 - Build 25 BETA
(c) Cassini - 19-Jan-2019

Contains 6 files:

SCJoyServer.exe              The program (V2.2-B25)
x64\SCdxKeyboard.dll         Native dll for Keyboard (v2.00)      - MUST be in the same folder as the Exe file
x86\SCdxKeyboard.dll         Native dll for Keyboard (v2.00)      - MUST be in the same folder as the Exe file
x64\vJoyInterface.dll        Native dll for vJoy (v2.00)          - MUST be in the same folder as the Exe file
x86\vJoyInterface.dll        Native dll for vJoy (v2.00)          - MUST be in the same folder as the Exe file

ReadMe.txt                   This file

SC Virtual Joystick Server (.Net 4.6.2)

Put all files into one folder and hit SCJoyServer.exe to run it

For Updates and information visit:
https://github.com/SCToolsfactory/SCJoyServer

Scanned for viruses before packing... 
cassini@burri-web.org

Changelog:
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
