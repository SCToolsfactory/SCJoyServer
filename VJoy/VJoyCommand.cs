using System;
using System.Collections.Generic;
using System.Linq;

namespace SCJoyServer.VJoy
{

  /// <summary>
  /// Handles VJoy Messages
  /// </summary>
#if DEBUG  // murks to allow UT access (there are better means - but then...)
  public
#endif
    class VJoyCommand
  {
    static private int VJ_MAXBUTTON = 60;  // the last allowed button number
    static private int DEFAULT_DELAY = 100; // msec
    static private int DEFAULT_SHORTDELAY = 5; // msec - short tap const

    #region Command Type Def

    // Message Handling 
    public enum VJ_ControllerType
    {
      VJ_Unknown = -1,
      VJ_Button = 0,
      VJ_Axis,
      VJ_RotAxis,
      VJ_Slider,
      VJ_Hat,

      DX_Key, // key input 
    }

    // Message Handling 
    public enum VJ_Modifier
    {
      VJ_None = 0,
      VJ_LCtrl,
      VJ_RCtrl,
      VJ_LAlt,
      VJ_RAlt,
      VJ_LShift,
      VJ_RShift,
    }

    public enum VJ_ControllerDirection
    {
      VJ_NotUsed = 0,

      VJ_X,
      VJ_Y,
      VJ_Z,

      VJ_Center,    // POV
      VJ_Left,      // POV
      VJ_Right,     // POV
      VJ_Up,        // POV or button or key
      VJ_Down,      // POV or button or key
      VJ_Tap,       // button or key
      VJ_DoubleTap, // button or key
    }

    public class VJCommand
    {
      /// <summary>
      /// The controller e.g. Button, Axis, etc.
      /// </summary>
      public VJ_ControllerType CtrlType { get; set; } = VJ_ControllerType.VJ_Unknown;

      /// <summary>
      /// The controller direction e.g. Axis X,Y,Z or POV direction or button up or down
      /// </summary>
      public VJ_ControllerDirection CtrlDirection { get; set; } = VJ_ControllerDirection.VJ_NotUsed;

      /// <summary>
      /// The controller modifier 
      /// </summary>
      public VJ_Modifier CtrlModifier { get; set; } = VJ_Modifier.VJ_None;

      /// <summary>
      /// The index of the controller 1..n
      /// for K it is the Key Integer VK_Key
      /// </summary>
      public int CtrlIndex { get; set; } = 0;

      /// <summary>
      /// The value of the controller 1..1000
      /// for B and K  Taps it is the delay in mseconds 
      /// </summary>
      public int CtrlValue { get; set; } = 0;

      public VJCommand()
      {
        // set defaults
        CtrlType = VJ_ControllerType.VJ_Unknown; // if there is no known content
        CtrlDirection = VJ_ControllerDirection.VJ_NotUsed;
        CtrlModifier = VJ_Modifier.VJ_None;
        CtrlIndex = 0;
        CtrlValue = 0;
      }

      public bool IsValid { get => this.CtrlType != VJ_ControllerType.VJ_Unknown; }

      // true if it is not a Key message
      public bool IsVJoyMessage { get => this.CtrlType != VJ_ControllerType.DX_Key; }

    }; // class

    #endregion

    /// <summary>
    /// Detects a message and returns it, removes the returned one from the inBuffer
    /// </summary>
    /// <param name="inBuffer">The stream buffer</param>
    /// <returns>A message or an Empty string</returns>
    static private string ExtractMessage( ref string inBuffer )
    {
      // garbage cleaner..
      if ( inBuffer.IndexOf( '\0' ) >= 0 ) inBuffer = inBuffer.Substring( 0, inBuffer.IndexOf( '\0' ) ); // kill Nul chars in the string

      string retVal = "";

      if ( Json.ExtractFragment( inBuffer, out string msg, out string remain ) ) {
        // there is at least one complete message
        retVal = msg; // the current message
        inBuffer = remain; // return any remaining ones
      }
      return retVal;
    }


    /// <summary>
    /// Parses the given message and returns the content as VJMessage
    /// Note: Use ExtractMessage to get at least only one message from the stream
    /// </summary>
    /// <param name="msg">The message to parse</param>
    /// <returns>The derived content as VJMessage</returns>
    static private VJCommand ParseMessage( string msg )
    {
      var retVal = new VJCommand( );

      // Json message format is like:
      // Axis:     { "A": {"Direction":"X|Y|Z", "Value":number}}        ; value 0..1000
      // RotAxis:  { "R": {"Direction":"X|Y|Z", "Value":number}}        ; value 0..1000
      // Slider:   { "S": {"Index":1|2, "Value":number}}                ; value 0..1000
      // POV:      { "P": {"Index":1|2|3|4, "Direction":"c|r|l|u|d"}}   ; direction (center (released), right, left, up, down)

      // Button:   { "B": {"Index":n, "Mode":"p|r|t|s|d", "Delay":100}}   ; Button index 1..VJ_MAXBUTTON
      // Key:      { "K": {"VKcode":n, "Mode":"p|r|t|s|d", "Modifier":"mod", "Delay":100}}  ; VKcode 0..255 WinUser VK_..

      // - Mode:      (p)ress, (r)elease, (t)ap, (s)hort tap, (d)ouble tap  (short tap is a tap with almost no delay)
      // - Modifier:  (n)one, (lc)trl, (rc)trl, (la)lt, (ra)lt, (ls)hift, (rs)hift   (optional - default=none - only one modifier is supported)
      // - Delay:     nnnn  milliseconds (optional for Tap and Double Tap - default=100)


      // we can rely on at least 3 chars in the string
      JsonObject jo = Json.Parse( msg );
      JsonObject jox = null;

      if ( jo == null ) return retVal;
      if ( jo.Count < 1 ) return retVal;

      string dir = "";
      int valueNum = 0;
      switch ( jo.ElementAt( 0 ).Key ) {
        case "A": // one of the linear axis
          // Axis:     { "A": {"Direction":"X|Y|Z", "Value":number}}        ; value 0..1000
          jox = jo["A"] as JsonObject;
          if ( jox.ContainsKey( "Direction" ) ) {
            dir = (string)jox["Direction"];
          }
          if ( jox.ContainsKey( "Value" ) ) {
            if ( jox["Value"].GetType( ) == typeof( int ) ) {
              // we have a value
              retVal.CtrlValue = (int)jox["Value"];
              retVal.CtrlType = VJ_ControllerType.VJ_Axis;
              switch ( dir ) {
                case "X":
                  retVal.CtrlDirection = VJ_ControllerDirection.VJ_X;
                  break;
                case "Y":
                  retVal.CtrlDirection = VJ_ControllerDirection.VJ_Y;
                  break;
                case "Z":
                  retVal.CtrlDirection = VJ_ControllerDirection.VJ_Z;
                  break;
                default: // just return the default message (unknown ctrl)
                  retVal.CtrlType = VJ_ControllerType.VJ_Unknown;
                  break;
              }
            }
          }
          break;

        case "R": // one of the rotational axis
          // RotAxis:  { "R": {"Direction":"X|Y|Z", "Value":number}}        ; value 0..1000
          jox = jo["R"] as JsonObject;
          if ( jox.ContainsKey( "Direction" ) ) {
            dir = (string)jox["Direction"];
          }
          if ( jox.ContainsKey( "Value" ) ) {
            if ( jox["Value"].GetType( ) == typeof( int ) ) {
              // we have a value
              retVal.CtrlValue = (int)jox["Value"];
              retVal.CtrlType = VJ_ControllerType.VJ_RotAxis;
              switch ( dir ) {
                case "X":
                  retVal.CtrlDirection = VJ_ControllerDirection.VJ_X;
                  break;
                case "Y":
                  retVal.CtrlDirection = VJ_ControllerDirection.VJ_Y;
                  break;
                case "Z":
                  retVal.CtrlDirection = VJ_ControllerDirection.VJ_Z;
                  break;
                default: // just return the default message (unknown ctrl)
                  retVal.CtrlType = VJ_ControllerType.VJ_Unknown;
                  break;
              }
            }
          }
          break;

        case "S": // one of the sliders
          // Slider:   { "S": {"Index":1|2, "Value":number}}                ; value 0..1000
          jox = jo["S"] as JsonObject;
          if ( jox.ContainsKey( "Index" ) ) {
            if ( jox["Index"].GetType( ) == typeof( int ) ) {
              valueNum = (int)jox["Index"];
            }
          }
          if ( jox.ContainsKey( "Value" ) ) {
            if ( jox["Value"].GetType( ) == typeof( int ) ) {
              // we have a value
              retVal.CtrlValue = (int)jox["Value"];
              retVal.CtrlType = VJ_ControllerType.VJ_Slider;
              retVal.CtrlDirection = VJ_ControllerDirection.VJ_NotUsed;
              if ( valueNum >= 1 && valueNum <= 2 ) {
                retVal.CtrlIndex = valueNum;
              }
              else {
                retVal.CtrlType = VJ_ControllerType.VJ_Unknown;
              }
            }
          }
          break;

        case "P": // one of the POVs
          // POV:      { "P": {"Index":1|2|3|4,"Direction":"c|r|l|u|d"}}    ; direction (center (released), right, left, up, down)
          jox = jo["P"] as JsonObject;
          if ( jox.ContainsKey( "Index" ) ) {
            if ( jox["Index"].GetType( ) == typeof( int ) ) {
              valueNum = (int)jox["Index"];
            }
          }
          if ( jox.ContainsKey( "Direction" ) ) {
            dir = (string)jox["Direction"];
          }
          retVal.CtrlType = VJ_ControllerType.VJ_Hat;
          if ( valueNum >= 1 && valueNum <= 4 ) {
            retVal.CtrlIndex = valueNum;
          }
          else {
            retVal.CtrlType = VJ_ControllerType.VJ_Unknown;
          }
          switch ( dir ) {
            case "c":
              retVal.CtrlDirection = VJ_ControllerDirection.VJ_Center;
              break;
            case "r":
              retVal.CtrlDirection = VJ_ControllerDirection.VJ_Right;
              break;
            case "l":
              retVal.CtrlDirection = VJ_ControllerDirection.VJ_Left;
              break;
            case "u":
              retVal.CtrlDirection = VJ_ControllerDirection.VJ_Up;
              break;
            case "d":
              retVal.CtrlDirection = VJ_ControllerDirection.VJ_Down;
              break;
            default: // just return the default message (unknown ctrl)
              retVal.CtrlType = VJ_ControllerType.VJ_Unknown;
              break;
          }
          break;

        case "B": // one of the buttons
          // Button:   { "B": {"Index":n, "Mode":"p|r|t|s|d", "Delay":100}}   ; Button index 1..VJ_MAXBUTTON
          jox = jo["B"] as JsonObject;
          if ( jox.ContainsKey( "Mode" ) ) {
            dir = (string)jox["Mode"];
          }
          if ( jox.ContainsKey( "Delay" ) && ( jox["Delay"].GetType( ) == typeof( int ) ) ) {
            retVal.CtrlValue = (int)jox["Delay"];
          }
          else {
            retVal.CtrlValue = DEFAULT_DELAY;
          }
          if ( jox.ContainsKey( "Index" ) ) {
            if ( jox["Index"].GetType( ) == typeof( int ) ) {
              // we have a value
              retVal.CtrlIndex = (int)jox["Index"];
              // we have a value i.e. the index
              if ( ( retVal.CtrlIndex < 1 ) || ( retVal.CtrlIndex > VJ_MAXBUTTON ) ) return retVal; // ERROR - bail out on invalid button number
              retVal.CtrlType = VJ_ControllerType.VJ_Button;
              switch ( dir ) {
                case "p": // pressed
                  retVal.CtrlDirection = VJ_ControllerDirection.VJ_Down;
                  break;
                case "r": // released
                  retVal.CtrlDirection = VJ_ControllerDirection.VJ_Up;
                  break;
                case "t": // tap
                  retVal.CtrlDirection = VJ_ControllerDirection.VJ_Tap;
                  break;
                case "s": // short tap
                  retVal.CtrlDirection = VJ_ControllerDirection.VJ_Tap;
                  retVal.CtrlValue = DEFAULT_SHORTDELAY; // const for short tap
                  break;
                case "d": // double tap
                  retVal.CtrlDirection = VJ_ControllerDirection.VJ_DoubleTap;
                  break;
                default: // just return the default message (unknown ctrl)
                  retVal.CtrlType = VJ_ControllerType.VJ_Unknown;
                  break;
              }
            }
          }
          break;

        case "K": // Key press or release
                  // Key:      { "K": {"VKcode":n, "Mode":"p|r|t|d", "Modifier":"mod", "Delay":100}}  ; VKcode 0..255 WinUser VK_..
          jox = jo["K"] as JsonObject;
          if ( jox.ContainsKey( "Mode" ) ) {
            dir = (string)jox["Mode"];
          }
          string mod = "";
          if ( jox.ContainsKey( "Modifier" ) ) {
            mod = (string)jox["Modifier"];
          }
          if ( jox.ContainsKey( "Delay" ) && ( jox["Delay"].GetType( ) == typeof( int ) ) ) {
            retVal.CtrlValue = (int)jox["Delay"];
          }
          else {
            retVal.CtrlValue = DEFAULT_DELAY;
          }
          if ( jox.ContainsKey( "VKcode" ) && ( jox["VKcode"].GetType( ) == typeof( int ) ) ) {
            retVal.CtrlIndex = (int)jox["VKcode"];
            if ( ( retVal.CtrlIndex < 1 ) || ( retVal.CtrlIndex > 0xff ) ) return retVal; // ERROR - bail out on invalid key int
            retVal.CtrlType = VJ_ControllerType.DX_Key;
            switch ( dir ) {
              case "p": // pressed
                retVal.CtrlDirection = VJ_ControllerDirection.VJ_Down;
                break;
              case "r": // released
                retVal.CtrlDirection = VJ_ControllerDirection.VJ_Up;
                break;
              case "t": // tap
                retVal.CtrlDirection = VJ_ControllerDirection.VJ_Tap;
                break;
              case "s": // short tap
                retVal.CtrlDirection = VJ_ControllerDirection.VJ_Tap;
                retVal.CtrlValue = DEFAULT_SHORTDELAY; // const for short tap
                break;
              case "d": // double tap
                retVal.CtrlDirection = VJ_ControllerDirection.VJ_DoubleTap;
                break;
              default: // just return the default message (unknown ctrl)
                retVal.CtrlType = VJ_ControllerType.VJ_Unknown;
                break;
            }
            switch ( mod ) {
              case "lc": // leftCtrl
                retVal.CtrlModifier = VJ_Modifier.VJ_LCtrl;
                break;
              case "rc": // rightCtrl
                retVal.CtrlModifier = VJ_Modifier.VJ_RCtrl;
                break;
              case "la": // leftAlt
                retVal.CtrlModifier = VJ_Modifier.VJ_LAlt;
                break;
              case "ra": // rightAlt
                retVal.CtrlModifier = VJ_Modifier.VJ_RAlt;
                break;
              case "ls": // leftShift
                retVal.CtrlModifier = VJ_Modifier.VJ_LShift;
                break;
              case "rs": // rightShift
                retVal.CtrlModifier = VJ_Modifier.VJ_RShift;
                break;
              default: // none
                retVal.CtrlModifier = VJ_Modifier.VJ_None;
                break;
            }
          }
          break;

        default: // just return the default message (unknown ctrl)
          break;
      }

      return retVal;
    }


    /// <summary>
    /// Returns the next message decoded from the buffer as command
    /// removes the translated message from the buffer
    /// </summary>
    /// <param name="inBuffer">The stream buffer</param>
    /// <returns>A VJMessage</returns>
    static public VJCommand TranslateMessage( ref string inBuffer )
    {
      string msg = ExtractMessage( ref inBuffer );
      VJCommand retVal = ParseMessage( msg );
      return retVal;
    }



  }
}
