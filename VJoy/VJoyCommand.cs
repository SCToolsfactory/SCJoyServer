using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using vjMapper.VjOutput;

namespace SCJoyServer.VJoy
{

  /// <summary>
  /// VJoy Commands for processing
  /// </summary>
#if DEBUG  // murks to allow UT access (there are better means - but then...)
  public
#endif
    class VJoyCommand
  {

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

      return VJoyCmdParser.FromJson( msg ).VJCommand;
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
