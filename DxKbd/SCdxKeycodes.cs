/**
* @file           SXdxKeyCodes.cs
*****************************************************************************
* Consts
*
*  Provides keyboard scan codes for Input simulation
*   Note: this is just a copy of the WinUser.h section for convenience only
*
* Copyright (C) 2018 Martin Burri  (bm98@burri-web.org)
*
*
*<hr>
*
* @b Project      SXdxInput<br>
*
* @author         M. Burri
* @date           15-Dec-2018
*
*****************************************************************************
*<hr>
* @b Updates
* - dd-mmm-yyyy V. Name: Description
*
*****************************************************************************/

namespace SCJoyServer.DxKbd
{
  public class SCdxKeycodes
  {
    /*
    * Virtual Keys, Standard Set
    */
    public const int VK_LBUTTON = 0x01;
    public const int VK_RBUTTON = 0x02;
    public const int VK_CANCEL = 0x03;
    public const int VK_MBUTTON = 0x04;    /* NOT contiguous with L & RBUTTON */

    public const int VK_XBUTTON1 = 0x05;    /* NOT contiguous with L & RBUTTON */
    public const int VK_XBUTTON2 = 0x06;    /* NOT contiguous with L & RBUTTON */

    /*
    * 0x07 : reserved
    */

    public const int VK_BACK = 0x08;
    public const int VK_TAB = 0x09;

    /*
    * 0x0A - 0x0B : reserved
    */

    public const int VK_CLEAR = 0x0C;
    public const int VK_RETURN = 0x0D;

    /*
    * 0x0E - 0x0F : unassigned
    */

    public const int VK_SHIFT = 0x10;
    public const int VK_CONTROL = 0x11;
    public const int VK_MENU = 0x12;
    public const int VK_PAUSE = 0x13;
    public const int VK_CAPITAL = 0x14;

    /*
    * 0x16 : unassigned
    */

    /*
    * 0x1A : unassigned
    */

    public const int VK_ESCAPE = 0x1B;

    public const int VK_CONVERT = 0x1C;
    public const int VK_NONCONVERT = 0x1D;
    public const int VK_ACCEPT = 0x1E;
    public const int VK_MODECHANGE = 0x1F;

    public const int VK_SPACE = 0x20;
    public const int VK_PRIOR = 0x21;
    public const int VK_NEXT = 0x22;
    public const int VK_END = 0x23;
    public const int VK_HOME = 0x24;
    public const int VK_LEFT = 0x25;  // Arrows
    public const int VK_UP = 0x26;  // Arrows
    public const int VK_RIGHT = 0x27;  // Arrows
    public const int VK_DOWN = 0x28;  // Arrows
    public const int VK_SELECT = 0x29;
    public const int VK_PRINT = 0x2A;
    public const int VK_EXECUTE = 0x2B;
    public const int VK_SNAPSHOT = 0x2C;
    public const int VK_INSERT = 0x2D;
    public const int VK_DELETE = 0x2E;
    public const int VK_HELP = 0x2F;

    /*
    * VK_0 - VK_9 are the same as ASCII '0' - '9' (0x30 - 0x39)
    * 0x3A - 0x40 : unassigned
    * VK_A - VK_Z are the same as ASCII 'A' - 'Z' (0x41 - 0x5A)
    */

    public const int VK_LWIN = 0x5B;  // Left Win Key
    public const int VK_RWIN = 0x5C;  // RIght Win Key
    public const int VK_APPS = 0x5D;

    /*
    * 0x5E : reserved
    */

    public const int VK_SLEEP = 0x5F;

    public const int VK_NUMPAD0 = 0x60;
    public const int VK_NUMPAD1 = 0x61;
    public const int VK_NUMPAD2 = 0x62;
    public const int VK_NUMPAD3 = 0x63;
    public const int VK_NUMPAD4 = 0x64;
    public const int VK_NUMPAD5 = 0x65;
    public const int VK_NUMPAD6 = 0x66;
    public const int VK_NUMPAD7 = 0x67;
    public const int VK_NUMPAD8 = 0x68;
    public const int VK_NUMPAD9 = 0x69;
    public const int VK_MULTIPLY = 0x6A;
    public const int VK_ADD = 0x6B;
    public const int VK_SEPARATOR = 0x6C;
    public const int VK_SUBTRACT = 0x6D;
    public const int VK_DECIMAL = 0x6E;
    public const int VK_DIVIDE = 0x6F;
    public const int VK_F1 = 0x70;
    public const int VK_F2 = 0x71;
    public const int VK_F3 = 0x72;
    public const int VK_F4 = 0x73;
    public const int VK_F5 = 0x74;
    public const int VK_F6 = 0x75;
    public const int VK_F7 = 0x76;
    public const int VK_F8 = 0x77;
    public const int VK_F9 = 0x78;
    public const int VK_F10 = 0x79;
    public const int VK_F11 = 0x7A;
    public const int VK_F12 = 0x7B;
    public const int VK_F13 = 0x7C;
    public const int VK_F14 = 0x7D;
    public const int VK_F15 = 0x7E;
    /*
    not defined in DirectInput - hence left out here
        public const int VK_F16            0x7F
        public const int VK_F17            0x80
        public const int VK_F18            0x81
        public const int VK_F19            0x82
        public const int VK_F20            0x83
        public const int VK_F21            0x84
        public const int VK_F22            0x85
        public const int VK_F23            0x86
        public const int VK_F24            0x87
    */

    public const int VK_NUMLOCK = 0x90;
    public const int VK_SCROLL = 0x91;  // SCROLL LOCK

    /*
    * 0x97 - 0x9F : unassigned
    */

    /*
    * VK_L* & VK_R* - left and right Alt, Ctrl and Shift virtual keys.
    * Used only as parameters to GetAsyncKeyState() and GetKeyState().
    * No other API or message will distinguish left and right keys in this way.
    */
    public const int VK_LSHIFT = 0xA0;
    public const int VK_RSHIFT = 0xA1;
    public const int VK_LCONTROL = 0xA2;
    public const int VK_RCONTROL = 0xA3;
    public const int VK_LMENU = 0xA4;
    public const int VK_RMENU = 0xA5;

    /*
    * 0xB8 - 0xB9 : reserved
    */

    public const int VK_OEM_1 = 0xBA;   // ';:' for US
    public const int VK_OEM_PLUS = 0xBB;   // '+' any country
    public const int VK_OEM_COMMA = 0xBC;   // ',' any country
    public const int VK_OEM_MINUS = 0xBD;   // '-' any country
    public const int VK_OEM_PERIOD = 0xBE;   // '.' any country
    public const int VK_OEM_2 = 0xBF;   // '/?' for US
    public const int VK_OEM_3 = 0xC0;   // '`~' for US

    /*
    * 0xC1 - 0xC2 : reserved
    */


    public const int VK_OEM_4 = 0xDB;  //  '[{' for US
    public const int VK_OEM_5 = 0xDC;  //  '\|' for US
    public const int VK_OEM_6 = 0xDD;  //  ']}' for US
    public const int VK_OEM_7 = 0xDE;  //  ''"' for US
    public const int VK_OEM_8 = 0xDF;

    /*
    * 0xE0 : reserved
    */
    public const int VK_PROCESSKEY = 0xE5;

    /*
    * 0xE8 : unassigned
    */

    /*
    * 0xFF : reserved
    */

    // Added to converge more with DirectInput naming and additions

    // US ISO Kbd 1st row after Key 0
    public const int VK_BACKSPACE = VK_BACK;  // added
    public const int VK_EQUALS = VK_OEM_6; // added
    public const int VK_MINUS = VK_OEM_4; // added

    // US ISO Kbd 2nd row after Key P
    public const int VK_LBRACKET = VK_OEM_1; // added
    public const int VK_RBRACKET = VK_OEM_3; // added

    // US ISO Kbd 3rd row after Key L
    public const int VK_SEMICOLON = VK_OEM_7; // added
    public const int VK_APOSTROPHE = VK_OEM_5; // added
    public const int VK_BACKSLASH = VK_OEM_8; // added

    // US ISO Kbd 4th row after Key M
    public const int VK_SLASH = VK_OEM_MINUS; // added
    public const int VK_PERIOD = VK_OEM_PERIOD; // added
    public const int VK_COMMA = VK_OEM_COMMA; // added


    // NumPad aside from numbers
    public const int VK_NUMPADSLASH = VK_DIVIDE;  // added 
    public const int VK_NUMPADSTAR = VK_MULTIPLY; // added 
    public const int VK_NUMPADMINUS = VK_SUBTRACT;  // added 
    public const int VK_NUMPADPLUS = VK_ADD; // added 
    public const int VK_NUMPADENTER = VK_RETURN + 1; // added - needs special treatment in the DLL...
    public const int VK_NUMPADPERIOD = VK_DECIMAL;  // added 

    public const int VK_ALT = VK_MENU;   //  added generic ALT key == MENU
    public const int VK_CAPSLOCK = VK_CAPITAL;  // added
    public const int VK_PGUP = VK_PRIOR;  //  added
    public const int VK_PGDN = VK_NEXT;  //  added
    public const int VK_LEFTARROW = VK_LEFT;  // Arrows
    public const int VK_UPARROW = VK_UP;  // Arrows
    public const int VK_RIGHTARROW = VK_RIGHT;  // Arrows
    public const int VK_DOWNARROW = VK_DOWN;  // Arrows
    public const int VK_PRINTSCREEN = VK_SNAPSHOT; // added
    public const int VK_LALT = VK_LMENU; // added
    public const int VK_RALT = VK_RMENU; // added

    // NUMLOCK -> PAUSE

  }
}
