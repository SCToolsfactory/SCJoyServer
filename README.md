SCJoyServer V 2.0.0.20<br>
==========================<br>
<br>
SC Virtual Joystick Server  (.Net 4.6.2)<br>
<br>
Note: this is a complete rework..<br>
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
      // Json message format is like:<br>
      // Axis:     { "A": {"Direction":"X|Y|Z", "Value":number}}        ; value 0..1000<br>
      // RotAxis:  { "R": {"Direction":"X|Y|Z", "Value":number}}        ; value 0..1000<br>
      // Slider:   { "S": {"Index":1|2, "Value":number}}                ; value 0..1000<br>
      // POV:      { "P": {"Index":1|2|3|4, "Direction":"c|r|l|u|d"}}   ; direction (center (released), right, left, up, down)<br>
	  <br>
      // Button:   { "B": {"Index":n, "Mode":"p|r|t|d", "Delay":100}}   ; Button index 1..VJ_MAXBUTTON<br>
      // Key:      { "K": {"VKcode":n, "Mode":"p|r|t|d", "Modifier":"mod", "Delay":100}}  ; VKcode 0..255 WinUser VK_..<br>
	  <br>
      // - Mode:    (p)ress, (r)elease, (t)ap, (d)ouble tap<br>
      // - Modifier:  (n)one, (lc)trl, (rc)trl, (la)lt, (ra)lt   (optional - default=none - only one modifier is supported)<br>
	  //   note: when using KeyDown / Up make sure to use the same modifier for both (else the modKey remains pressed...)<br>
      // - Delay:   nnnn  milliseconds (optional for Tap and Double Tap - default=100)<br>
<br>
<br>
Supports UDP and TCP protocol. For TCP there are up to 8 simultaneous clients served.<br>
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


