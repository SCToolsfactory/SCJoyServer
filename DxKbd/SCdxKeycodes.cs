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
* - 21-Mar-2020 BM- make codes an Enum and provide a conversion tool for strings
*                   add VK_NP_xy symbols to match SC control names better
* - dd-mmm-yyyy V. Name: Description
*
*****************************************************************************/

namespace SCJoyServer.DxKbd
{
  public enum SCdxKeycode
  {

    /*
    * Virtual Keys, Standard Set
    */
    VK_LBUTTON = 0x01,
    VK_RBUTTON = 0x02,
    VK_CANCEL = 0x03,
    VK_MBUTTON = 0x04,    /* NOT contiguous with L & RBUTTON */

    VK_XBUTTON1 = 0x05,    /* NOT contiguous with L & RBUTTON */
    VK_XBUTTON2 = 0x06,    /* NOT contiguous with L & RBUTTON */

    // 0x07 : reserved

    VK_BACK = 0x08,
    VK_TAB = 0x09,

    // 0x0A - 0x0B : reserved

    VK_CLEAR = 0x0C,
    VK_RETURN = 0x0D,

    // 0x0E - 0x0F : unassigned

    VK_SHIFT = 0x10,
    VK_CONTROL = 0x11,
    VK_MENU = 0x12,
    VK_PAUSE = 0x13,
    VK_CAPITAL = 0x14,

    // 0x16 : unassigned
    // 0x1A : unassigned

    VK_ESCAPE = 0x1B,

    VK_CONVERT = 0x1C,
    VK_NONCONVERT = 0x1D,
    VK_ACCEPT = 0x1E,
    VK_MODECHANGE = 0x1F,

    VK_SPACE = 0x20,
    VK_PRIOR = 0x21,
    VK_NEXT = 0x22,
    VK_END = 0x23,
    VK_HOME = 0x24,
    VK_LEFT = 0x25,  // Arrows
    VK_UP = 0x26,  // Arrows
    VK_RIGHT = 0x27,  // Arrows
    VK_DOWN = 0x28,  // Arrows
    VK_SELECT = 0x29,
    VK_PRINT = 0x2A,
    VK_EXECUTE = 0x2B,
    VK_SNAPSHOT = 0x2C,
    VK_INSERT = 0x2D,
    VK_DELETE = 0x2E,
    VK_HELP = 0x2F,

    /*
    * VK_0 - VK_9 are the same as ASCII '0' - '9' (0x30 - 0x39)
    * 0x3A - 0x40 : unassigned
    * VK_A - VK_Z are the same as ASCII 'A' - 'Z' (0x41 - 0x5A)
    */
    VK_0 = 0x30,
    VK_1 = 0x31,
    VK_2 = 0x32,
    VK_3 = 0x33,
    VK_4 = 0x34,
    VK_5 = 0x35,
    VK_6 = 0x36,
    VK_7 = 0x37,
    VK_8 = 0x38,
    VK_9 = 0x39,

    VK_A = 0x41,
    VK_B = 0x42,
    VK_C = 0x43,
    VK_D = 0x44,
    VK_E = 0x45,
    VK_F = 0x46,
    VK_G = 0x47,
    VK_H = 0x48,
    VK_I = 0x49,
    VK_J = 0x4A,
    VK_K = 0x4B,
    VK_L = 0x4C,
    VK_M = 0x4D,
    VK_N = 0x4E,
    VK_O = 0x4F,
    VK_P = 0x50,
    VK_Q = 0x51,
    VK_R = 0x52,
    VK_S = 0x53,
    VK_T = 0x54,
    VK_U = 0x55,
    VK_V = 0x56,
    VK_W = 0x57,
    VK_X = 0x58,
    VK_Y = 0x59,
    VK_Z = 0x5A,

    VK_LWIN = 0x5B,  // Left Win Key
    VK_RWIN = 0x5C,  // RIght Win Key
    VK_APPS = 0x5D,

    // 0x5E : reserved

    VK_SLEEP = 0x5F,

    VK_NUMPAD0 = 0x60,
    VK_NP_0 = VK_NUMPAD0,
    VK_NUMPAD1 = 0x61,
    VK_NP_1 = VK_NUMPAD1,
    VK_NUMPAD2 = 0x62,
    VK_NP_2 = VK_NUMPAD2,
    VK_NUMPAD3 = 0x63,
    VK_NP_3 = VK_NUMPAD3,
    VK_NUMPAD4 = 0x64,
    VK_NP_4 = VK_NUMPAD4,
    VK_NUMPAD5 = 0x65,
    VK_NP_5 = VK_NUMPAD5,
    VK_NUMPAD6 = 0x66,
    VK_NP_6 = VK_NUMPAD6,
    VK_NUMPAD7 = 0x67,
    VK_NP_7 = VK_NUMPAD7,
    VK_NUMPAD8 = 0x68,
    VK_NP_8 = VK_NUMPAD8,
    VK_NUMPAD9 = 0x69,
    VK_NP_9 = VK_NUMPAD9,
    VK_MULTIPLY = 0x6A,
    VK_ADD = 0x6B,
    VK_SEPARATOR = 0x6C,
    VK_SUBTRACT = 0x6D,
    VK_DECIMAL = 0x6E,
    VK_DIVIDE = 0x6F,
    VK_F1 = 0x70,
    VK_F2 = 0x71,
    VK_F3 = 0x72,
    VK_F4 = 0x73,
    VK_F5 = 0x74,
    VK_F6 = 0x75,
    VK_F7 = 0x76,
    VK_F8 = 0x77,
    VK_F9 = 0x78,
    VK_F10 = 0x79,
    VK_F11 = 0x7A,
    VK_F12 = 0x7B,
    VK_F13 = 0x7C,
    VK_F14 = 0x7D,
    VK_F15 = 0x7E,
    /*
    not defined in DirectInput - hence left out here
        VK_F16            0x7F
        VK_F17            0x80
        VK_F18            0x81
        VK_F19            0x82
        VK_F20            0x83
        VK_F21            0x84
        VK_F22            0x85
        VK_F23            0x86
        VK_F24            0x87
    */

    VK_NUMLOCK = 0x90,
    VK_SCROLL = 0x91,  // SCROLL LOCK

    // 0x97 - 0x9F : unassigned

    /*
    * VK_L* & VK_R* - left and right Alt, Ctrl and Shift virtual keys.
    * Used only as parameters to GetAsyncKeyState() and GetKeyState().
    * No other API or message will distinguish left and right keys in this way.
    */
    VK_LSHIFT = 0xA0,
    VK_RSHIFT = 0xA1,
    VK_LCONTROL = 0xA2,
    VK_RCONTROL = 0xA3,
    VK_LMENU = 0xA4,
    VK_RMENU = 0xA5,

    // 0xB8 - 0xB9 : reserved

    VK_OEM_1 = 0xBA,   // ',:' for US
    VK_OEM_PLUS = 0xBB,   // '+' any country
    VK_OEM_COMMA = 0xBC,   // ',' any country
    VK_OEM_MINUS = 0xBD,   // '-' any country
    VK_OEM_PERIOD = 0xBE,   // '.' any country
    VK_OEM_2 = 0xBF,   // '/?' for US
    VK_OEM_3 = 0xC0,   // '`~' for US

    // 0xC1 - 0xC2 : reserved

    VK_OEM_4 = 0xDB,  //  '[{' for US
    VK_OEM_5 = 0xDC,  //  '\|' for US
    VK_OEM_6 = 0xDD,  //  ']}' for US
    VK_OEM_7 = 0xDE,  //  ''"' for US
    VK_OEM_8 = 0xDF,

    // 0xE0 : reserved
    VK_PROCESSKEY = 0xE5,

    // 0xE8 : unassigned
    // 0xFF : reserved

    // Added to converge more with DirectInput naming and additions
    // US ISO Kbd 1st row after Key 0
    VK_BACKSPACE = VK_BACK,  // added
    VK_EQUALS = VK_OEM_6, // added
    VK_MINUS = VK_OEM_4, // added

    // US ISO Kbd 2nd row after Key P
    VK_LBRACKET = VK_OEM_1, // added
    VK_RBRACKET = VK_OEM_3, // added

    // US ISO Kbd 3rd row after Key L
    VK_SEMICOLON = VK_OEM_7, // added
    VK_APOSTROPHE = VK_OEM_5, // added
    VK_BACKSLASH = VK_OEM_8, // added

    // US ISO Kbd 4th row after Key M
    VK_SLASH = VK_OEM_MINUS, // added
    VK_PERIOD = VK_OEM_PERIOD, // added
    VK_COMMA = VK_OEM_COMMA, // added


    // NumPad aside from numbers
    VK_NP_DIVIDE = VK_DIVIDE,  // added 
    VK_NP_MULTIPLY = VK_MULTIPLY, // added 
    VK_NP_SUBTRACT = VK_SUBTRACT,  // added 
    VK_NP_ADD = VK_ADD, // added 
    VK_NP_ENTER = VK_RETURN + 1, // added - needs special treatment in the DLL...
    VK_NP_PERIOD = VK_DECIMAL,  // added 

    VK_ALT = VK_MENU,   //  added generic ALT key == MENU
    VK_SCROLLOCK = VK_SCROLL,// added
    VK_CAPSLOCK = VK_CAPITAL,  // added
    VK_PGUP = VK_PRIOR,  //  added
    VK_PGDN = VK_NEXT,  //  added
    VK_LEFTARROW = VK_LEFT,  // Arrows
    VK_UPARROW = VK_UP,  // Arrows
    VK_RIGHTARROW = VK_RIGHT,  // Arrows
    VK_DOWNARROW = VK_DOWN,  // Arrows
    VK_PRINTSCREEN = VK_SNAPSHOT, // added
    VK_LALT = VK_LMENU, // added
    VK_RALT = VK_RMENU, // added

    // NUMLOCK -> PAUSE

  }

  /// <summary>
  /// Helper to decode keynames
  /// </summary>
  public class SCdxKeycodes
  {
    /// <summary>
    /// Returnt the code for a name
    /// </summary>
    /// <param name="keyName">A valid keyname</param>
    /// <returns>The keycode or 0 if no match</returns>
    public static int KeyCodeFromKeyName( string keyName )
    {
      if ( System.Enum.TryParse( keyName.ToUpperInvariant( ), out SCdxKeycode keycode ) ) {
        return (int)keycode;
      }
      return 0;
    }

  }

}
