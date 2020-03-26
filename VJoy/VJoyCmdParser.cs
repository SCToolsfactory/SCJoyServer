using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


using static SCJoyServer.VJoy.VJoyCommand;
using SCJoyServer.DxKbd;

namespace SCJoyServer.VJoy
{
  /// <summary>
  ///  Defines the JSON syntax of the commands received from the client
  ///  provides methods to get valid VJ_Commands from
  /// </summary>
  class VJoyCmdParser
  {
    [DataContract]
    private class Command
    {
      /*
        COMMAND format:

          Joystick:
          Axis:     { "A": {"Direction": "X|Y|Z", "Value": number} }
                      - number => 0..1000 (normalized)

          RotAxis:  { "R": {"Direction": "X|Y|Z", "Value": number} }
                      - number => 0..1000 (normalized)

          Slider:   { "S": {"Index": 1|2, "Value": number} }
                      - number => 0..1000 (normalized)

          POV:      { "P": {"Index": 1|2|3|4, "Direction": "c | u | r | d | l" } }   
                      - Index n=> 1..MaxPOV (setup of vJoy, max = 60 CIG limit)
                      - Direction either of the chars (center (released), up, right, donw, left)

          Button:   { "B": {"Index": n, "Mode": "p|r|t|s|d", "Delay":100 } } 
                      - Button Index n => 1..VJ_MAXBUTTON (setup of vJoy)
                      - Mode optional - either of the chars (see below)

          Keyboard:
          Key:      { "K": {"VKcodeEx": "keyName", "VKcode": n, "Mode": "p|r|t|s|d", "Modifier": "mod", "Delay": 100 } }  
                      - VKcodeEx "s" either a number n=> 1..255 or a WinUser VK_.. literal (see separate Reference file)
                      - VKcode n=> 1..255 WinUser VK_.. (see separate Reference file)
                         if both are found the VKcodeEx item gets priority and the VKcode element is ignored
                         if none is found the command is ignored
                      - Mode optional - either of the chars (see below)
                      - Modifier optional - a set of codes (see below)

           - Mode:     [mode]      (p)ress, (r)elease, (t)ap, (s)hort tap, (d)ouble tap           (default=tap - short tap is a tap with almost no delay)
           - Modifier: [mod[&mod]] (n)one, (lc)trl, (rc)trl, (la)lt, (ra)lt, (ls)hift, (rs)hift   (default=none - concat modifiers with & char)
           - Delay:    [delay]      nnnn  milliseconds, optional for Tap and Double Tap           (default=150)     
      */

      [DataMember]
      public CommandAxis A { get; set; }
      // either of the one below
      [DataMember]
      public CommandRotAxis R { get; set; }
      [DataMember]
      public CommandSlider S { get; set; }
      [DataMember]
      public CommandPov P { get; set; }
      [DataMember]
      public CommandButton B { get; set; }
      [DataMember]
      public CommandKey K { get; set; }

      // non Json

      public VJCommand VJCommand
      {
        get {
          if ( A != null ) return A.Cmd;
          if ( R != null ) return R.Cmd;
          if ( S != null ) return S.Cmd;
          if ( P != null ) return P.Cmd;
          if ( B != null ) return B.Cmd;
          if ( K != null ) return K.Cmd;
          return null;
        }
      }
    }


    /// <summary>
    /// Command Template
    /// </summary>
    [DataContract]
    private abstract class CommandBase
    {
      // non Json
      virtual public VJCommand Cmd { get; }

      protected void HandleDelay( ref VJCommand cmd, int delay )
      {
        if ( delay > 0 )
          cmd.CtrlValue = delay;
        else
          cmd.CtrlValue = VJCommand.DEFAULT_DELAY;
      }

      protected void HandleMode( ref VJCommand cmd, string modeString )
      {
        if ( string.IsNullOrEmpty( modeString ) ) modeString = "t"; // Tap is default if nothing is given
        switch ( modeString.ToLowerInvariant( ) ) {
          case "p": // pressed
            cmd.CtrlDirection = VJ_ControllerDirection.VJ_Down;
            break;
          case "r": // released
            cmd.CtrlDirection = VJ_ControllerDirection.VJ_Up;
            break;
          case "t": // tap
            cmd.CtrlDirection = VJ_ControllerDirection.VJ_Tap;
            break;
          case "s": // short tap
            cmd.CtrlDirection = VJ_ControllerDirection.VJ_Tap;
            cmd.CtrlValue = VJCommand.DEFAULT_SHORTDELAY; // const for short tap
            break;
          case "d": // double tap
            cmd.CtrlDirection = VJ_ControllerDirection.VJ_DoubleTap;
            break;
          default: // just return the default message (unknown ctrl)
            cmd.CtrlType = VJ_ControllerType.VJ_Unknown;
            break;
        }

      }


    }

    /// <summary>
    /// Axis Command
    /// </summary>
    [DataContract]
    private class CommandAxis : CommandBase
    {
      /*
          Axis:     { "A": {"Direction": "X|Y|Z", "Value": number} }
                      - number => 0..1000 (normalized)
      */
      [DataMember( IsRequired = true )]
      public string Direction { get; set; }
      [DataMember( IsRequired = true )]
      public short Value { get; set; }

      // non Json
      public override VJCommand Cmd
      {
        get {
          var retVal = new VJCommand( );

          if ( Value < 0 || Value > 1000 ) {
            return retVal; // ERROR - bail out on invalid number
          }

          retVal.CtrlType = VJ_ControllerType.VJ_Axis;
          retVal.CtrlValue = Value;
          switch ( Direction.ToUpperInvariant( ) ) {
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
          return retVal;
        }
      }

    }

    /// <summary>
    /// RotAxis Command
    /// </summary>
    [DataContract]
    private class CommandRotAxis : CommandBase
    {
      /*
          RotAxis:  { "R": {"Direction": "X|Y|Z", "Value": number} }
                      - number => 0..1000 (normalized)
      */
      [DataMember( IsRequired = true )]
      public string Direction { get; set; }
      [DataMember( IsRequired = true )]
      public short Value { get; set; }

      // non Json
      public override VJCommand Cmd
      {
        get {
          var retVal = new VJCommand( );

          if ( Value < 0 || Value > 1000 ) {
            return retVal; // ERROR - bail out on invalid number
          }

          retVal.CtrlType = VJ_ControllerType.VJ_RotAxis;
          retVal.CtrlValue = Value;
          switch ( Direction.ToUpperInvariant( ) ) {
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
          return retVal;
        }
      }
    }

    /// <summary>
    /// Slider Command
    /// </summary>
    [DataContract]
    private class CommandSlider : CommandBase
    {
      /*
          Slider:   { "S": {"Index": 1|2, "Value": number} }
                      - number => 0..1000 (normalized)
      */
      [DataMember( IsRequired = true )]
      public short Index { get; set; }
      [DataMember( IsRequired = true )]
      public short Value { get; set; }

      // non Json
      public override VJCommand Cmd
      {
        get {
          var retVal = new VJCommand( );

          if ( Value < 0 || Value > 1000 ) {
            return retVal; // ERROR - bail out on invalid number
          }

          retVal.CtrlValue = Value;
          retVal.CtrlType = VJ_ControllerType.VJ_Slider;
          retVal.CtrlDirection = VJ_ControllerDirection.VJ_NotUsed;
          if ( Index >= 1 && Index <= 2 ) {
            retVal.CtrlIndex = Index;
          }
          else {
            retVal.CtrlType = VJ_ControllerType.VJ_Unknown;
          }
          return retVal;
        }
      }
    }

    /// <summary>
    /// POV Command
    /// </summary>
    [DataContract]
    private class CommandPov : CommandBase
    {
      /*
          POV:      { "P": {"Index": 1|2|3|4, "Direction": "c | u | r | d | l", "LED": "disp" } }   
                      - Index n=> 1..MaxPOV (setup of vJoy, max = 60 CIG limit)
                      - Direction either of the chars (center (released), up, right, donw, left)
      */
      [DataMember( IsRequired = true )]
      public short Index { get; set; }
      [DataMember( IsRequired = true )]
      public string Direction { get; set; }
      [DataMember]
      public string LED { get; set; }

      // non Json
      public override VJCommand Cmd
      {
        get {
          var retVal = new VJCommand( );

          if ( Index < 1 || Index > 4 ) {
            return retVal; // ERROR - bail out on invalid number
          }

          retVal.CtrlType = VJ_ControllerType.VJ_Hat;
          retVal.CtrlIndex = Index;
          switch ( Direction.ToLowerInvariant( ) ) {
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

          return retVal;
        }
      }
    }

    /// <summary>
    /// Button Command
    /// </summary>
    [DataContract]
    private class CommandButton : CommandBase
    {
      /*
          Button:   { "B": {"Index": n, "Mode": "p|r|t|s|d", "Delay":100, "LED": "disp" } } 
                      - Button Index n => 1..VJ_MAXBUTTON (setup of vJoy)
                      - Mode optional - either of the chars (see below)
      */
      [DataMember( IsRequired = true )]
      public short Index { get; set; }
      [DataMember]
      public string Mode { get; set; }
      [DataMember]
      public short Delay { get; set; }
      [DataMember]
      public string LED { get; set; }

      // non Json
      public override VJCommand Cmd
      {
        get {
          var retVal = new VJCommand( );

          retVal.CtrlIndex = Index;
          if ( ( retVal.CtrlIndex < 1 ) || ( retVal.CtrlIndex > VJCommand.VJ_MAXBUTTON ) ) {
            return retVal; // ERROR - bail out on invalid number
          }

          retVal.CtrlType = VJ_ControllerType.VJ_Button;

          HandleDelay( ref retVal, Delay );
          HandleMode( ref retVal, Mode );

          return retVal;
        }
      }
    }

    /// <summary>
    /// Key Command
    /// </summary>
    [DataContract]
    private class CommandKey : CommandBase
    {
      /*
          Button:   { "B": {"Index": n, "Mode": "p|r|t|s|d", "Delay":100, "LED": "disp" } } 
                      - Button Index n => 1..VJ_MAXBUTTON (setup of vJoy)
                      - Mode optional - either of the chars (see below)
      */
      [DataMember]
      public string VKcodeEx { get; set; }
      [DataMember]
      public int VKcode { get; set; } = 0;
      [DataMember]
      public string Mode { get; set; }
      [DataMember]
      public string Modifier { get; set; }
      [DataMember]
      public short Delay { get; set; }
      [DataMember]
      public string LED { get; set; }

      // non Json
      public override VJCommand Cmd
      {
        get {
          var retVal = new VJCommand( );

          // either a number or a keyname
          if ( !string.IsNullOrEmpty(VKcodeEx)) {
            if ( int.TryParse( VKcodeEx, out int code ) ) {
              VKcode = code; // VKcodeEx has priority
            }
            else {
              VKcode = SCdxKeycodes.KeyCodeFromKeyName( VKcodeEx );
            }
          }
          // merged VKCodeEx into VKcode if it was supplied
          retVal.CtrlIndex = VKcode;

          if ( ( retVal.CtrlIndex < 1 ) || ( retVal.CtrlIndex > 0xff ) ) {
            return retVal; // ERROR - bail out on invalid number
          }

          retVal.CtrlType = VJ_ControllerType.DX_Key;
          if ( !string.IsNullOrEmpty( Modifier ) ) {
            // treat multiples
            string[] e = Modifier.ToLowerInvariant( ).Split( new char[] { '&' } );
            for ( int i = 0; i < e.Length; i++ ) {
              switch ( e[i] ) {
                case "lc": // leftCtrl
                  retVal.CtrlModifier.Add( VJ_Modifier.VJ_LCtrl );
                  break;
                case "rc": // rightCtrl
                  retVal.CtrlModifier.Add( VJ_Modifier.VJ_RCtrl );
                  break;
                case "la": // leftAlt
                  retVal.CtrlModifier.Add( VJ_Modifier.VJ_LAlt );
                  break;
                case "ra": // rightAlt
                  retVal.CtrlModifier.Add( VJ_Modifier.VJ_RAlt );
                  break;
                case "ls": // leftShift
                  retVal.CtrlModifier.Add( VJ_Modifier.VJ_LShift );
                  break;
                case "rs": // rightShift
                  retVal.CtrlModifier.Add( VJ_Modifier.VJ_RShift );
                  break;
                default: // none                
                  break;
              }
            }
          }

          HandleDelay( ref retVal, Delay );
          HandleMode( ref retVal, Mode );

          return retVal;
        }
      }
    }//CommandKey

    #region Static Class VJoyCmdParser

    /// <summary>
    /// Reads from the open stream one ConfigFile entry
    /// </summary>
    /// <param name="jStream">An open stream at position</param>
    /// <returns>A ConfigFile obj or null for errors</returns>
    private static Command FromJson_low( Stream jStream )
    {
      try {
        var jsonSerializer = new DataContractJsonSerializer( typeof( Command ) );
        object objResponse = jsonSerializer.ReadObject( jStream );
        if ( objResponse is Command ) {
          var jsonResults = objResponse as Command;
          return jsonResults;
        }
        return null; // ERROR - not a Command Type object ??
      }
      catch ( Exception e ) {
        return null; // ERROR - de-serialization failed
      }
    }

    /// <summary>
    /// Reads from the open stream and returns a SwitchPanelConfig entry
    /// </summary>
    /// <param name="jStream">An open stream at position</param>
    /// <returns>A SwitchPanelConfig obj</returns>
    public static VJoyCmdParser FromJson( Stream jStream )
    {
      var ret = new VJoyCmdParser( );
      ret.VJoyCmdParserInit( FromJson_low( jStream ) );
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
      m_command = cmd.VJCommand; // decomposes the JSON object into our VJ_Command
      Valid = m_command.IsValid;
    }

    #endregion

    #endregion

  }
}
