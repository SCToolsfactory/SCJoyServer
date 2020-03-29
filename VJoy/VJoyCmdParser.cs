using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using vjMapper.VjOutput;
using vjMapper;
using vjMapper.JInput;

namespace SCJoyServer.VJoy
{
  /// <summary>
  ///  Defines the JSON syntax of the commands received from the client
  ///  provides methods to get valid VJ_Commands from
  /// </summary>
  public class VJoyCmdParser
  {

    #region Static Class VJoyCmdParser

    /// <summary>
    /// Reads from the open stream and returns a VJoyCmdParser entry
    /// </summary>
    /// <param name="jStream">An open stream at position</param>
    /// <returns>A VJoyCmdParser obj</returns>
    public static VJoyCmdParser FromJson( Stream jStream )
    {
      var ret = new VJoyCmdParser( );
      ret.VJoyCmdParserInit( vjMapping.FromJsonStream<Command>( jStream ) );
      return ret;
    }

    /// <summary>
    /// Reads from the string and returns a VJoyCmdParser entry
    /// </summary>
    /// <param name="jString">An Json string</param>
    /// <returns>A VJoyCmdParser obj</returns>
    public static VJoyCmdParser FromJson( string jString )
    {
      var ret = new VJoyCmdParser( );
      ret.VJoyCmdParserInit( vjMapping.FromJsonString<Command>( jString ) );
      return ret;
    }

    #region Class VJoyCmdParser

    public bool Valid { get; private set; } = false;

    private VJCommand m_command = new VJCommand( );
    /// <summary>
    /// All commands as dictonary (string key = input name)
    /// </summary>
    public VJCommand VJCommand { get => m_command; }


    /// <summary>
    /// Decompose the Command file
    /// </summary>
    /// <param name="cmd"></param>
    private void VJoyCmdParserInit( Command cmd )
    {
      if ( cmd == null ) return; // No Command - bail out Valid=> false;
      m_command = cmd.VJCommand( new MacroDefList() ); // decomposes the JSON object into our VJ_Command
      Valid = m_command.IsValid;
    }

    #endregion

    #endregion

  }
}
