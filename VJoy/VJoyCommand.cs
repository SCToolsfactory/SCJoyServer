using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCJoyServer
{

  /// <summary>
  /// Handles VJoy Messages
  /// </summary>
  class VJoyCommand
  {

    static private Char VJ_EOM = '!';       // End of Message
    static private int  VJ_MAXBUTTON = 32;  // the last allowed button number

    /// <summary>
    /// Detects a message and returns it, removes the returned one from the inBuffer
    /// </summary>
    /// <param name="inBuffer">The stream buffer</param>
    /// <returns>A message or an Empty String</returns>
    static private String ExtractMessage( ref String inBuffer )
    {
      // garbage cleaner..
      if ( inBuffer.IndexOf( '\0' ) >= 0 ) inBuffer = inBuffer.Substring( 0, inBuffer.IndexOf( '\0' ) ); // kill Nul chars in the string
      inBuffer = inBuffer.Replace( "\r", null );
      inBuffer = inBuffer.Replace( "\n", null );
      inBuffer = inBuffer.Replace( "\t", null );

      String retVal = inBuffer;

      String [] elem = inBuffer.Split( new Char[] { VJ_EOM }, StringSplitOptions.RemoveEmptyEntries );
      if ( elem.Length > 0 ) {
        // there is at least one complete message
        retVal = elem[0]; // the current message
        inBuffer = String.Join( VJ_EOM.ToString( ), elem, 1, elem.Length - 1 ); // return any remaining ones
      }
      return retVal;
    }


    /// <summary>
    /// Parses the given message and returns the content as VJMessage
    /// Note: Use ExtractMessage to get at least only one message from the stream
    /// </summary>
    /// <param name="msg">The message to parse</param>
    /// <returns>The derived content as VJMessage</returns>
    static private VJCommand ParseMessage( String msg )
    {
      VJCommand retVal = new VJCommand( );
      if ( msg.Length < 3 ) return retVal; // trivial - msg is empty or not long enough to be valid

      // message format is like:
      // Axis:     A[X|Y|Z]value          ; XYZ: direction; value 0..1000
      // RotAxis:  R[X|Y|Z]value          ; XYZ: direction; value 0..1000
      // Slider:   S[1|2]value            ; n: Slider index 1..2; value 0..1000
      // POV:      P[1|2|3|4][c|r|l|u|d]  ; n: POV index 1..4; crlud: direction (center (released), right, left, up, down); 
      // Button:   B[p|r][n]              ; pr: pressed or released n: Button index 1..VJ_MAXBUTTON; 

      // we can rely on at least 3 chars in the string
      String ctrl = msg.Substring( 0, 1 );
      int value = 0; 
      switch ( ctrl ) {
        case "A": // one of the linear axis
          // Axis:     A[X|Y|Z]value          ; XYZ: direction; value 0..1000
          if ( int.TryParse( msg.Substring( 2 ), out value ) ) {
            // we have a value
            retVal.CtrlValue = value;
            retVal.CtrlType = VJ_ControllerType.VJ_Axis;
            String dir = msg.Substring( 1, 1 );
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
          break;

        case "R": // one of the rotational axis
          // RotAxis:  R[X|Y|Z]value          ; XYZ: direction; value 0..1000
          if ( int.TryParse( msg.Substring( 2 ), out value ) ) {
            // we have a value
            retVal.CtrlValue = value;
            retVal.CtrlType = VJ_ControllerType.VJ_RotAxis;
            String dir = msg.Substring( 1, 1 );
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
          break;

        case "S": // one of the sliders
          // Slider:   S[1|2]value            ; n: Slider index 1..2; value 0..1000
          if ( int.TryParse( msg.Substring( 2 ), out value ) ) {
            // we have a value
            retVal.CtrlValue = value;
            retVal.CtrlType = VJ_ControllerType.VJ_Slider;
            retVal.CtrlDirection = VJ_ControllerDirection.VJ_NotUsed;
            String index = msg.Substring( 1, 1 );
            switch ( index ) {
              case "1":
                retVal.CtrlIndex = 1;
                break;
              case "2":
                retVal.CtrlIndex = 2;
                break;
              default: // just return the default message (unknown ctrl)
                retVal.CtrlType = VJ_ControllerType.VJ_Unknown;
                break;
            }
          }
          break;

        case "P": // one of the POVs
          // POV:      P[1|2|3|4][c|r|l|u|d]  ; n: POV index 1..4; crlud: direction (center (released), right, left, up, down); 
          {
            retVal.CtrlType = VJ_ControllerType.VJ_Hat;
            String index = msg.Substring( 1, 1 );
            switch ( index ) {
              case "1":
                retVal.CtrlIndex = 1;
                break;
              case "2":
                retVal.CtrlIndex = 2;
                break;
              case "3":
                retVal.CtrlIndex = 3;
                break;
              case "4":
                retVal.CtrlIndex = 4;
                break;
              default: // just return the default message (unknown ctrl)
                retVal.CtrlType = VJ_ControllerType.VJ_Unknown;
                break;
            }
            String dir = msg.Substring( 2, 1 );
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
          }
          break;

        case "B": // one of the buttons
          // Button:   B[p|r][n]              ; pr: pressed or released n: Button index 1..32; 
          if ( int.TryParse( msg.Substring( 2 ), out value ) ) {
            // we have a value i.e. the index
            if ( ( value < 1 ) || ( value > VJ_MAXBUTTON ) ) return retVal; // ERROR - bail out on invalid button number

            retVal.CtrlIndex = value;
            retVal.CtrlType = VJ_ControllerType.VJ_Button;
            String index = msg.Substring( 1, 1 );
            switch ( index ) {
              case "p": // pressed
                retVal.CtrlDirection = VJ_ControllerDirection.VJ_Down;
                break;
              case "r": // released
                retVal.CtrlDirection = VJ_ControllerDirection.VJ_Up;
                break;
              default: // just return the default message (unknown ctrl)
                retVal.CtrlType = VJ_ControllerType.VJ_Unknown;
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
    static public VJCommand TranslateMessage( ref String inBuffer )
    {
      String msg = ExtractMessage( ref inBuffer );
      VJCommand retVal = ParseMessage( msg );
      return retVal;
    }


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

    }

    public enum VJ_ControllerDirection
    {
      VJ_NotUsed = 0,

      VJ_X,
      VJ_Y,
      VJ_Z,

      VJ_Center,
      VJ_Up,
      VJ_Down,
      VJ_Left,
      VJ_Right,
    }

    public class VJCommand
    {
      /// <summary>
      /// The controller e.g. Button, Axis, etc.
      /// </summary>
      public VJ_ControllerType CtrlType { get; set; }
      /// <summary>
      /// The controller direction e.g. Axis X,Y,Z or POV direction or button up or down
      /// </summary>
      public VJ_ControllerDirection CtrlDirection { get; set; }
      /// <summary>
      /// The index of the controller 1..n
      /// </summary>
      public int CtrlIndex { get; set; }
      /// <summary>
      /// The value of the controller 1..1000
      /// </summary>
      public int CtrlValue { get; set; }

      public VJCommand( )
      {
        // set defaults
        CtrlType = VJ_ControllerType.VJ_Unknown; // if there is no known content
        CtrlDirection = VJ_ControllerDirection.VJ_NotUsed;
        CtrlIndex = 0;
        CtrlValue = 0;
      }

      public Boolean IsValid
      {
        get
        {
          return ( this.CtrlType != VJ_ControllerType.VJ_Unknown );
        }
      }

    }; // class

    #endregion

  }
}
