SCJoyServer V 2.7.0.30<br>
==========================<br>
<br>
SC Virtual Joystick Server  (.Net 4.7.2)<br>
<br>
The command is now Json syntax, supports also keystrokes sent to the active window.<br>
<br>
provides an UDP and TCP Server that actuates ONE vJoy virtual Joystick and supplies keystrokes to the active window<br>
Note: <br>
You have to install the vJoy (V 2.1) driver.<br>
The driver is not available here but use this link.<br>
http://vjoystick.sourceforge.net/site/index.php/download-a-install/download    <br>
<br>
Also note there is no security built in yet, i.e. if the server listens on the LAN port <br>
one may type remotely into your active window.. - be warned or use a firewalled intranet<br>
or for in machine use only use the loopback address (127.0.0.1)<br>
<br>
Open e.g. Putty and then connect to the server (default port is 34123 but you may change it)<br>
Send commands like <br>
<br>
  Joystick:<br>
    Axis:     { "A": {"Direction": "X|Y|Z", "Value": number} }<br>
                - number => 0..1000 (normalized)<br>
		Set the Axis to the value given (0->lower bound, 1000-> upper bound)<br>
		<br>
    RotAxis:  { "R": {"Direction": "X|Y|Z", "Value": number} }<br>
                - number => 0..1000 (normalized)<br>
		Set the Axis to the value given (0->lower bound, 1000-> upper bound)<br>
    <br>
    Slider:   { "S": {"Index": 1|2, "Value": number} }<br>
                - number => 0..1000 (normalized)<br>
		Set the Slider to the value given (0->lower bound, 1000-> upper bound)<br>
    <br>
    POV:      { "P": {"Index": 1|2|3|4, "Direction": "c | u | r | d | l" } }   <br>
                - Index n=> 1..MaxPOV (setup of vJoy, max = 60 CIG limit)<br>
                - Direction either of the chars (center (released), up, right, donw, left)<br>
		Set the POV to the direction given (will stay there until another command changes this to e.g. center)<br>
		<br>
    Button:   { "B": {"Index": n, "Mode": "p|r|t|s|d", "Delay":100 } } <br>
                - Button Index n => 1..VJ_MAXBUTTON (setup of vJoy)<br>
                - Mode optional - either of the chars (see below)<br>
	    Trigger the button with Index with the mode given (NOTE: a "p"ress needs a "r"elease later - otherwise it remains pressed!!)<br>
		<br>
  Keyboard:<br>
    Key:      { "K": {"VKcodeEx": "keyName", "VKcode": n, "Mode": "p|r|t|s|d", "Modifier": "mod", "Delay": 100, "LED": "disp" } }  <br>
                - VKcodeEx "s" either a number n=> 1..255 or a WinUser VK_.. literal (see separate Reference file)<br>
                - VKcode n=> 1..255 WinUser VK_.. (see separate Reference file)<br>
                    if both are found the VKcodeEx item gets priority and the VKcode element is ignored<br>
                    if none is found the command is ignored<br>
                - Mode optional - either of the chars (see below)<br>
                - Modifier optional - a set of codes (see below)<br>
	    Trigger the key with VKcode with the mode given (NOTE: a "p"ress needs a "r"elease later - otherwise it remains pressed!!)<br>
		<br>
		<br>
     - Mode:     [mode]      (p)ress, (r)elease, (t)ap, (s)hort tap, (d)ouble tap           (default=tap - short tap is a tap with almost no delay)<br>
     - Modifier: [mod[&mod]] (n)one, (lc)trl, (rc)trl, (la)lt, (ra)lt, (ls)hift, (rs)hift   (default=none - concat modifiers with & char)<br>
     - Delay:    [delay]      nnnn  milliseconds, optional for Tap and Double Tap           (default=150)     <br>
<br>
<br>
Supports multiple UDP and TCP servers (one per vJoystick).<br>
 Port increments with each active Joystick selected.<br>
Supports UDP and TCP protocol. For TCP there are up to 8 simultaneous clients served.<br>
<br>
It supports also file upload from a monitored directory to a WebServer with http POST.<br>
The file is expected to be of type and extension .json<br>
The WebServer must have an upload route to /api/fileupload enabled and either e.g. a default PHP script or otherwise<br>
able to receive the upload POST request else you may need to change it in the WebUploader code.<br>
<br>
You may map the Virtual Joystick like any real one into SC by using e.g. SCJMapper-V2<br>
<br>
NOTE: THIS _ IS _ VERY _ EARLY _ WORK _ IN _ PROGRESS _ IT _ MAY _ JUST _ BREAK _ AT _ ANY _ TIME ;-)<br>
<br>
Usage can be:<br>
  A WebPage sending commands to control one vJoy device and sending Kbd Input <br>
  A RasPi with its own GUI sending commands to control one vJoy device and sending Kbd Input <br>
  Your idea ....<br>
<br>
In order to use the server one has to add the driver DLLs <br>
<br>
Just within the application Exe folder:<br>
x64\SCdxKeyboard.dll   (64bit)<br>
x64\vJoyInterface.dll  (from the vJoy218SDK-291116 - 64bit)<br>
x86\SCdxKeyboard.dll   (32bit)<br>
x86\vJoyInterface.dll  (from the vJoy218SDK-291116 - 32bit)<br>
<br>
For convenience find them in the SupportingBinaries as zip<br>
<br>
<br>


