using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SCJoyServer.VJoy
{

    // Command Enums


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
  
  
  /// <summary>
  /// VJoy Commands for processing
  /// </summary>
#if DEBUG  // murks to allow UT access (there are better means - but then...)
  public
#endif
    class VJoyCommand
  {
    #region Command Type Def

    public class VJCommand
    {
#if DEBUG  // murks to allow UT access (there are better means - but then...)
      public
#else
      internal
#endif
      const int VJ_MAXBUTTON = 60;  // the last allowed button number

#if DEBUG  // murks to allow UT access (there are better means - but then...)
      public
#else
      internal
#endif
      const int DEFAULT_DELAY = 150; // msec

#if DEBUG  // murks to allow UT access (there are better means - but then...)
      public
#else
      internal
#endif
      const int DEFAULT_SHORTDELAY = 5; // msec - short tap const

      /// <summary>
      /// The controller e.g. Button, Axis, etc.
      /// </summary>
      public VJ_ControllerType CtrlType { get; set; } = VJ_ControllerType.VJ_Unknown;

      /// <summary>
      /// The controller direction e.g. Axis X,Y,Z or POV direction or button up or down
      /// </summary>
      public VJ_ControllerDirection CtrlDirection { get; set; } = VJ_ControllerDirection.VJ_NotUsed;

      private List<VJ_Modifier> m_modifiers = new List<VJ_Modifier>( );
      /// <summary>
      /// The controller modifiers 
      /// </summary>
      public IList<VJ_Modifier> CtrlModifier { get => m_modifiers; }

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
      
      /// <summary>
      /// Returns true for a valid command
      /// </summary>
      public bool IsValid { get => this.CtrlType != VJ_ControllerType.VJ_Unknown; }

      /// <summary>
      /// true if it is not a Key message
      /// </summary>
      public bool IsVJoyMessage { get => this.CtrlType != VJ_ControllerType.DX_Key; }

      /// <summary>
      /// true if it is a Key message
      /// </summary>
      public bool IsVKeyMessage { get => this.CtrlType == VJ_ControllerType.DX_Key; }

    }; // class

    #endregion

    private static UnicodeEncoding m_uniEncoding = new UnicodeEncoding( );

    /// <summary>
    /// Parses the given message and returns the content as VJCommand
    /// Note: Use ExtractMessage to get at least only one message from the stream
    /// </summary>
    /// <param name="msg">The message to parse</param>
    /// <returns>The derived content as VJCommand</returns>
    static private VJCommand ParseMessage( string msg )
    {
      var ret = new VJCommand( );
      if ( string.IsNullOrEmpty( msg ) ) return ret; // empty string...

      using ( var memStream = new MemoryStream( ) ) {
        // Write the string to the stream.
        byte[] content = m_uniEncoding.GetBytes( msg );
        memStream.Write( content, 0, content.Length );
        memStream.Seek( 0, SeekOrigin.Begin );
        ret = VJoyCmdParser.FromJson( memStream ).VJCommand;
      }
      return ret;
    }

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
    /// Returns the next message decoded from the buffer as command
    /// removes the translated message from the buffer
    /// </summary>
    /// <param name="inBuffer">The stream buffer</param>
    /// <returns>A VJCommand</returns>
    static public VJCommand TranslateMessage( ref string inBuffer )
    {
      string msg = ExtractMessage( ref inBuffer );
      VJCommand retVal = ParseMessage( msg );
      return retVal;
    }



  }
}
