# SCJoyServer V 2.10.0.33

Provides an UDP and TCP Server that actuates vJoy virtual Joysticks and supplies keystrokes to the active window  

### General note to builders
The Project files expect referenced Libraries which have no NuGet package reference in a Solution directory  ´ExtLibraries´  
Those external libraries can built from the project SC_Toolbox:  
https://github.com/SCToolsfactory/SC_Toolbox

-----

# SC Virtual Joystick Server  (.Net 4.7.2)

The command is Json syntax, supports also keystrokes sent to the **active** window.  
--> The keystrokes sent via server are typed in the window that has the keyboard input focus.  


Note:  
You have to install the vJoy (V 2.1) driver.  
The driver is not available here but use this link.  
http://vjoystick.sourceforge.net/site/index.php/download-a-install/download     

Also note there is no security built in, i.e. if the server listens on the LAN port one may type remotely into your active window.. - 
take care or use a firewalled intranet or for in machine use only use the loopback address (127.0.0.1)

Open e.g. Putty and then connect to the server (default port is 34123 but you may change it)  

Send commands like 

  Joystick:  

    Axis:     { "A": {"Direction": "X|Y|Z", "Value": number, "JNo": j} }  
                - number => 0..1000 (normalized)  
		Set the Axis to the value given (0->lower bound, 1000-> upper bound)  

    RotAxis:  { "R": {"Direction": "X|Y|Z", "Value": number, "JNo": j} }  
                - number => 0..1000 (normalized)  
		Set the Axis to the value given (0->lower bound, 1000-> upper bound)  

    Slider:   { "S": {"Index": 1|2, "Value": number, "JNo": j} }  
                - number => 0..1000 (normalized)  
		Set the Slider to the value given (0->lower bound, 1000-> upper bound)  

    POV:      { "P": {"Index": 1|2|3|4, "Direction": "c | u | r | d | l", "JNo": j} }    
                - Index n=> 1..MaxPOV (setup of vJoy, max = 60 CIG limit)  
                - Direction either of the chars (center (released), up, right, donw, left)  
		Set the POV to the direction given (will stay there until another command changes this to e.g. center)  

    Button:   { "B": {"Index": n, "Mode": "p|r|t|s|d", "Delay":100, "JNo": j} }   
                - Button Index n => 1..VJ_MAXBUTTON (setup of vJoy)  
                - Mode optional - either of the chars (see below)  
	    Trigger the button with Index with the mode given (NOTE: a "p"ress needs a "r"elease later - otherwise it remains pressed!!)  

  Keyboard:  

    Key:      { "K": {"VKcodeEx": "keyName", "VKcode": n, "Mode": "p|r|t|s|d", "Modifier": "mod", "Delay": 100, "LED": "disp" } }  
                - VKcodeEx "s" either a number n=> 1..255 or a WinUser VK_.. literal (see separate Reference file)  
                - VKcode n=> 1..255 WinUser VK_.. (see separate Reference file)  
                    if both are found the VKcodeEx item gets priority and the VKcode element is ignored  
                    if none is found the command is ignored  
                - Mode optional - either of the chars (see below)  
                - Modifier optional - a set of codes (see below)  
	    Trigger the key with VKcode with the mode given (NOTE: a "p"ress needs a "r"elease later - otherwise it remains pressed!!)  

     - Mode:     [mode]      (p)ress, (r)elease, (t)ap, (s)hort tap, (d)ouble tap           (default=tap - short tap is a tap with almost no delay)  
     - Modifier: [mod[&mod]] (n)one, (lc)trl, (rc)trl, (la)lt, (ra)lt, (ls)hift, (rs)hift   (default=none - concat modifiers with & char)  
     - Delay:    [delay]      nnnn  milliseconds, optional for Tap and Double Tap           (default=150)       
     - JNo:      [joyNumber] >0 Joystick number                                             (default=1)


Supports UDP and TCP protocol. For TCP there are up to 8 simultaneous clients served.  
The primary port accepts commands directed to any vJoy (JNo) device enabled.

#### Legacy support when no JNo is provided: 

**Please try to include the JNo as soon as possible then send everything to the BasePort.**    
If the JNo is not provided it defaults to JNo=1 if sent to the BasePort!  
Supports multiple UDP and TCP servers (one per vJoystick for #devices >1).  
The Network port is set to BasePort + Joystick#-1 with each active Joystick selected.  
To send commands without embedded JNo to a specific server port use BasePort+(JNo-1) as target port.
I.e. with BasePort = 34123 to send to Joystick 3 use 34123+(3-1) = 34125 (don't forget to have Js3 checked)

### File upload service

The program supports also file upload from a monitored directory to a WebServer with http POST.  
The file is expected to be of type and extension ``.json``  
The WebServer must have an upload route to /api/fileupload enabled and either e.g. a default PHP script or otherwise able to receive the upload POST request else you may need to change it in the WebUploader code.  
  
You may map the Virtual Joystick like any real one into SC by using e.g. SCJMapper-V2  

# Usage 

* A WebPage sending commands to control one vJoy device and sending Kbd Input 
* A RasPi with its own GUI sending commands to control one vJoy device and sending Kbd Input 
* A MFD display utility  
* A Switch panel utility that does not provide native Joystick/Keyboard input  
* Your idea ....

Note: if a vJoy device is captured from another application the server startup may fail with an error
as it cannot connect the vJoy device. In such case uncheck the device used by the other application and try again.
Add one more vJoy device and use this one or uncheck all and use only the keyboard service.


In order to use the server one has to add the library DLLs 

Just within the application Exe folder:  
* vjMapper.dll                 Command Mapping Library
* vjAction.dll                 Command Execution Library
* dxKbdInterfaceWrap.dll       application keyboard typing support
*   x64\SCdxKeyboard.dll       (64bit version)
*   x86\SCdxKeyboard.dll       (32bit version)
* vJoy_csWrapper.dll           vJoy Access Library
*   x64\vJoyInterface.dll      (from vJoy218SDK-291116 - 64bit)
*   x86\vJoyInterface.dll      (from vJoy218SDK-291116 - 32bit)

