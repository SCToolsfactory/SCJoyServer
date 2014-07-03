using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCJoyServer
{
  /// <summary>
  /// Singleton that interacts with the VJoy DLL
  /// Accepts messages to be processed
  /// </summary>
  sealed class VJoyHandler
  {
    private static readonly VJoyHandler instance = new VJoyHandler( );
    private VJoyHandler( ) { }

    public static VJoyHandler Instance
    {
      get
      {
        return instance;
      }
    }


    private VJoyDLL.vJoystick m_joystick;

    private Boolean m_connected = false;
    public Boolean Connected { get { return m_connected; } }

    public Boolean Connect( int n )
    {
      if ( Connected ) return true; // already connected

      try {
        m_joystick = VJoyDLL.Instance.Joystick( n );
        m_connected = ( m_joystick != null );
      }
      catch ( IndexOutOfRangeException ) {
        // wrong index 
      }

      return Connected;
    }

    /// <summary>
    /// Last one close the door...
    /// 
    /// Disconnect the Joystick system
    /// </summary>
    public void Disconnect( )
    {
      if ( Connected ) {
        VJoyDLL.Instance.DropJoystick( ref m_joystick );
      }
      m_connected = false;
      VJoyDLL.Instance.Shutdown( );
    }


    /// <summary>
    /// Dispatch the command message 
    /// </summary>
    /// <param name="message">A VJoy Message</param>
    public Boolean HandleMessage( VJoyCommand.VJCommand message )
    {
      if ( !Connected ) return false; // ERROR - bail out for unde messages
      if ( !message.IsValid ) return false; // ERROR - bail out for unde messages

      Boolean retVal = false;

      // mutual exclusive access to the device
      lock ( m_joystick ) {

        try {
          switch ( message.CtrlType ) {
            case VJoyCommand.VJ_ControllerType.VJ_Axis:
              switch ( message.CtrlDirection ) {
                case VJoyCommand.VJ_ControllerDirection.VJ_X:
                  m_joystick.XAxis = message.CtrlValue;
                  break;
                case VJoyCommand.VJ_ControllerDirection.VJ_Y:
                  m_joystick.YAxis = message.CtrlValue;
                  break;
                case VJoyCommand.VJ_ControllerDirection.VJ_Z:
                  m_joystick.ZAxis = message.CtrlValue;
                  break;
                default:
                  break;
              }
              break;

            case VJoyCommand.VJ_ControllerType.VJ_RotAxis:
              switch ( message.CtrlDirection ) {
                case VJoyCommand.VJ_ControllerDirection.VJ_X:
                  m_joystick.XRotAxis = message.CtrlValue;
                  break;
                case VJoyCommand.VJ_ControllerDirection.VJ_Y:
                  m_joystick.YRotAxis = message.CtrlValue;
                  break;
                case VJoyCommand.VJ_ControllerDirection.VJ_Z:
                  m_joystick.ZRotAxis = message.CtrlValue;
                  break;
                default:
                  break;
              }
              break;

            case VJoyCommand.VJ_ControllerType.VJ_Slider:
              switch ( message.CtrlIndex ) {
                case 1:
                  m_joystick.Slider1 = message.CtrlValue;
                  break;
                case 2:
                  m_joystick.Slider2 = message.CtrlValue;
                  break;
                default:
                  break;
              }
              break;

            case VJoyCommand.VJ_ControllerType.VJ_Hat:
              switch ( message.CtrlDirection ) {
                case VJoyCommand.VJ_ControllerDirection.VJ_Center:
                  m_joystick.SetPOV( message.CtrlIndex - 1, VJoyDLL.POVType.Nil );
                  break;
                case VJoyCommand.VJ_ControllerDirection.VJ_Left:
                  m_joystick.SetPOV( message.CtrlIndex - 1, VJoyDLL.POVType.Left );
                  break;
                case VJoyCommand.VJ_ControllerDirection.VJ_Right:
                  m_joystick.SetPOV( message.CtrlIndex - 1, VJoyDLL.POVType.Right );
                  break;
                case VJoyCommand.VJ_ControllerDirection.VJ_Up:
                  m_joystick.SetPOV( message.CtrlIndex - 1, VJoyDLL.POVType.Up );
                  break;
                case VJoyCommand.VJ_ControllerDirection.VJ_Down:
                  m_joystick.SetPOV( message.CtrlIndex - 1, VJoyDLL.POVType.Down );
                  break;
                default:
                  break;
              }
              break;

            case VJoyCommand.VJ_ControllerType.VJ_Button:
              switch ( message.CtrlDirection ) {
                case VJoyCommand.VJ_ControllerDirection.VJ_Down:
                  m_joystick.SetButton( message.CtrlIndex - 1, true );
                  break;
                case VJoyCommand.VJ_ControllerDirection.VJ_Up:
                  m_joystick.SetButton( message.CtrlIndex - 1, false );
                  break;
                default:
                  break;
              }
              break;
            default:
              break;
          }//switch message type

          m_joystick.Update( );

          retVal = true; // finally we made it
        }
        catch { // anything
          m_connected = false; // probably something went wrong...
        }

      }//endlock

      return retVal;
    }



  }
}
