SCJoyServer
===========

SC Virtual Joystick Server  (.Net 4)<br>

provides a TCP/IP Server that actuates the VJoy (Headsoft) virtual Joysticks<br>
Note: 
You have to install the VJoy (V 1.2) driver e.g. by installing the NoTrackIR suite.
The driver is not available here.

Open e.g. Putty and then connect to the server (default port is 34123 but you may change it)
Send commands like 
<br>
      // Axis:     A[X|Y|Z]value          ; XYZ: direction; value 0..1000
      // RotAxis:  R[X|Y|Z]value          ; XYZ: direction; value 0..1000
      // Slider:   S[1|2]value            ; n: Slider index 1..2; value 0..1000
      // POV:      P[1|2|3|4][c|r|l|u|d]  ; n: POV index 1..4; crlud: direction (center (released), right, left, up, down); 
      // Button:   B[p|r][n]              ; pr: pressed or released n: Button index 1..VJ_MAXBUTTON; 
<br>
There are up to 8 simultaneous clients served.<br>
  <br>
You may map the Virtual Joystick like any real one into SC by using e.g. SCJMapper-V2<br>
<br>
NOTE: THIS _ IS _ VERY _ EARLY _ WORK _ IN _ PROGRESS _ IT _ MAY _ JUST _ BREAK _ AT _ ANY _ TIME ;-)<br>
<br>
<br>


