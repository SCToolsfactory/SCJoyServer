using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace SCJoyServer
{
  /// <summary>
  /// Interfaces with the vJoy DLLs
  /// Copied a bit from the SDK
  /// modded to be a singleton and giving access to either JS1 or JS2
  /// </summary>
  sealed class VJoyDLL
  {
    // Singleton to load one DLL only for all clients calling
    private static readonly VJoyDLL instance = new VJoyDLL( );
    private VJoyDLL( )
    {
      m_mode64 = Is64BitMode( ); // see which interface to use...
      m_joyUsed = new Boolean[] { false, false };
      m_timer = new TimerThread(this); // fudge around the Demo Mode (we are not commercial...)
    }

    public static VJoyDLL Instance
    {
      get
      {
        return instance;
      }
    }


    #region SDK Structures

    /// <summary>
    /// Data Structures for the DLL interface (from the SDK)
    /// </summary>
    private bool m_mode64 = false;

    public enum POVType
    {
      Up = 0,
      Right = 1,
      Down = 2,
      Left = 3,
      Nil = 4
    };

    [StructLayout( LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi )]
    public struct JoystickState
    {
      public byte ReportId;
      public short XAxis;
      public short YAxis;
      public short ZAxis;
      public short XRotation;
      public short YRotation;
      public short ZRotation;
      public short Slider;
      public short Dial;
      public ushort POV;
      public uint Buttons;
    };

    #endregion

    #region DLL declares

    #region 32bit

    [DllImport( "VJoy32.dll", EntryPoint = "VJoy_Initialize", CallingConvention = CallingConvention.Cdecl )]
    [return: MarshalAs( UnmanagedType.Bool )]
    private static extern bool VJoy_Initialize32( StringBuilder name, StringBuilder serial );

    [DllImport( "VJoy32.dll", EntryPoint = "VJoy_Shutdown", CallingConvention = CallingConvention.Cdecl )]
    private static extern void VJoy_Shutdown32( );

    [DllImport( "VJoy32.dll", EntryPoint = "VJoy_UpdateJoyState", CallingConvention = CallingConvention.Cdecl )]
    [return: MarshalAs( UnmanagedType.Bool )]
    private static extern bool VJoy_UpdateJoyState32( int id, ref JoystickState joyState );

    #endregion

    #region 64bit

    [DllImport( "VJoy64.dll", EntryPoint = "VJoy_Initialize", CallingConvention = CallingConvention.Cdecl )]
    [return: MarshalAs( UnmanagedType.Bool )]
    private static extern bool VJoy_Initialize64( StringBuilder name, StringBuilder serial );

    [DllImport( "VJoy64.dll", EntryPoint = "VJoy_Shutdown", CallingConvention = CallingConvention.Cdecl )]
    private static extern void VJoy_Shutdown64( );

    [DllImport( "VJoy64.dll", EntryPoint = "VJoy_UpdateJoyState", CallingConvention = CallingConvention.Cdecl )]
    [return: MarshalAs( UnmanagedType.Bool )]
    private static extern bool VJoy_UpdateJoyState64( int id, ref JoystickState joyState );

    private bool Is64BitMode( )
    {
      return Marshal.SizeOf( typeof( IntPtr ) ) == 8;
    }

    #endregion

    #endregion

    #region TimerThread Class

    /// <summary>
    /// Implements a timer that calls every 4 minutes the Init of the DLL - avoiding the Demo Popup
    /// Well - from Google it seems that headsoft does not answer if asked for a non commercial license
    /// so this is a fair to use workaround as the homepage states a free license for non commercial use
    /// </summary>
    class TimerThread
    {
      private Thread        m_timerTask = null;

      readonly VJoyDLL      m_vjdll;
      private Boolean       m_continue = false;

      public TimerThread( VJoyDLL vjdll )
      {
        m_vjdll = vjdll;
      }

      public void Start( )
      {

        // Start threads to handle Client Task
        m_timerTask = new Thread( new ThreadStart( this.Process ) );
        m_continue = true;
        m_timerTask.Start( );
      }

      private void Process( )
      {
        // to remain responsive we Sleep only 5 sec and loop until the 4 Mins are reached
        int DELAY = 5; // sec
        int LOOPS = ( 4 * 60 ) / DELAY; // 4 Minutes
        int nLoops = 0;
        TimeSpan interval = new TimeSpan( 0, 0, DELAY ); 
        do {
          Thread.Sleep( interval );
          nLoops++;
          if ( nLoops >= LOOPS ) {
            if ( !m_continue ) break; // bail out if asked for

            m_vjdll.Initialize( ); // calling init - see if it breaks something or we have to reload values and the like...
            nLoops = 0;
          }
        } while ( m_continue ) ;
      }

      public void Stop( )
      {
        m_continue = false;
        if ( m_timerTask != null && m_timerTask.IsAlive ) m_timerTask.Join( ); // wait until dead
      }

      public Boolean IsAlive
      {
        get
        {
          if ( m_timerTask == null ) return false; 
          else  return m_timerTask.IsAlive;
        }
      }
    }// TimerThread

    #endregion


    private JoystickState[] m_joyState = new JoystickState[2];
    private Boolean[] m_joyUsed;
    private TimerThread m_timer;

    /// <summary>
    /// Disconnect from the JS system
    /// </summary>
    public void Shutdown( )
    {
      m_timer.Stop( ); // kill the timer as well

      // then shut the DLL connection
      if ( m_mode64 )
        VJoy_Shutdown64( );
      else
        VJoy_Shutdown32( );
    }


    /// <summary>
    /// Initializes Joystick n
    /// </summary>
    /// <returns>True if successfull</returns>
    private bool Initialize( )
    {
      if ( ! m_timer.IsAlive ) m_timer.Start();

      StringBuilder Name = new StringBuilder( "" );
      StringBuilder Serial = new StringBuilder( "" );

      if ( m_mode64 )
        return VJoy_Initialize64( Name, Serial );
      else
        return VJoy_Initialize32( Name, Serial );
    }

    /// <summary>
    /// Resets the Joystick index 0/1 to default values
    /// </summary>
    /// <param name="n">Joystick index 0/1</param>
    private void Reset( int n )
    {
      if ( ( n >= 0 ) && ( n <= 1 ) ) {
        ( m_joyState[n] ).ReportId = 0;
        m_joyState[n].XAxis = 0;
        m_joyState[n].YAxis = 0;
        m_joyState[n].ZAxis = 0;
        m_joyState[n].XRotation = 0;
        m_joyState[n].YRotation = 0;
        m_joyState[n].ZRotation = 0;
        m_joyState[n].Slider = 0;
        m_joyState[n].Dial = 0;
        m_joyState[n].POV = ( ( int )POVType.Nil << 12 ) | ( ( int )POVType.Nil << 8 ) | ( ( int )POVType.Nil << 4 ) | ( int )POVType.Nil;
        m_joyState[n].Buttons = 0;
      }
      else throw new IndexOutOfRangeException( );
    }


    /// <summary>
    /// Get an update from the Joystick index 0/1 
    /// </summary>
    /// <param name="n">Joystick index 0/1</param>
    /// <returns>True if successfull</returns>
    private bool Update( int n )
    {
      if ( ( n >= 0 ) && ( n <= 1 ) ) {
        if ( m_mode64 )
          return VJoy_UpdateJoyState64( n, ref m_joyState[n] );
        else
          return VJoy_UpdateJoyState32( n, ref m_joyState[n] );
      }
      else throw new IndexOutOfRangeException( );
    }


    /// <summary>
    /// Creates a Joystick index 0/1 and returns it
    /// </summary>
    /// <param name="n"></param>
    /// <returns>A new Joystick or null if not available</returns>
    public vJoystick Joystick( int n )
    {
      vJoystick retVal = null;

      // valid ?
      if ( ( n >= 0 ) && ( n <= 1 ) ) {
        if ( !m_joyUsed[n] ) {
          // now ...
          if ( Initialize( ) ) {
            Reset( n );
            Update( n );
            retVal = new vJoystick( n, this );
            m_joyUsed[n] = true;
          }
        }
        return retVal;
      }
      else throw new IndexOutOfRangeException( );
    }

    public void DropJoystick( ref vJoystick js )
    {
      // valid ?
      if ( js != null) {
        m_joyUsed[js.Index] = false;
        js = null;
      }
    }


    #region vJoystick Class

    /// <summary>
    /// Provides a joystick
    /// </summary>
    public class vJoystick
    {
      // convert to and from -32768 .. 32767 base on external range 0...1000
      static private double m_range = ( Math.Pow( 2, 16 ) - 1.0 ) / 1000.0;
      static short ToJS( int value )
      {
        // make sure we reach the borders..
        if ( value == 0 ) return short.MinValue;
        if ( value == 1000 ) return short.MaxValue;

        int ret = (int)( value * m_range + short.MinValue );
        // make sure we are in range
        if ( ret < short.MinValue ) return short.MinValue;
        if ( ret > short.MaxValue ) return short.MaxValue;
        // just return the result
        return ( short )ret;
      }

      static int FromJS( short value )
      {
        // make sure we reach the borders..
        if ( value == short.MinValue ) return 0;
        if ( value == short.MaxValue ) return 1000;

        int ret = ( int )(( value - short.MinValue ) / m_range);
        // make sure we are in range
        if ( ret < 0 ) return 0;
        if ( ret > 1000 ) return 1000;
        // just return the result
        return ret;
      }

      private int m_myIndex = -1;
      private VJoyDLL m_vjdll;

      public int Index { get { return m_myIndex; } }

      public vJoystick( int n, VJoyDLL vjdll )
      {
        m_myIndex = n;
        m_vjdll = vjdll;
      }

      public void Update()
      {
        m_vjdll.Update( m_myIndex );
      }

      public int XAxis
      {
        get
        {
          return FromJS( m_vjdll.m_joyState[m_myIndex].XAxis );
        }
        set { m_vjdll.m_joyState[m_myIndex].XAxis = ToJS( value ); }
      }

      public int YAxis
      {
        get
        {
          return FromJS( m_vjdll.m_joyState[m_myIndex].YAxis );
        }
        set { m_vjdll.m_joyState[m_myIndex].YAxis = ToJS( value ); }
      }

      public int ZAxis
      {
        get
        {
          return FromJS( m_vjdll.m_joyState[m_myIndex].ZAxis );
        }
        set { m_vjdll.m_joyState[m_myIndex].ZAxis = ToJS( value ); }
      }


      public int XRotAxis
      {
        get
        {
          return FromJS( m_vjdll.m_joyState[m_myIndex].XRotation );
        }
        set { m_vjdll.m_joyState[m_myIndex].XRotation = ToJS( value ); }
      }

      public int YRotAxis
      {
        get
        {
          return FromJS( m_vjdll.m_joyState[m_myIndex].YRotation );
        }
        set { m_vjdll.m_joyState[m_myIndex].YRotation = ToJS( value ); }
      }

      public int ZRotAxis
      {
        get
        {
          return FromJS( m_vjdll.m_joyState[m_myIndex].ZRotation );
        }
        set { m_vjdll.m_joyState[m_myIndex].ZRotation = ToJS( value ); }
      }


      public int Slider1
      {
        get
        {
          return FromJS( m_vjdll.m_joyState[m_myIndex].Slider );
        }
        set { m_vjdll.m_joyState[m_myIndex].Slider = ToJS( value ); }
      }

      public int Slider2
      {
        get
        {
          return FromJS( m_vjdll.m_joyState[m_myIndex].Dial );
        }
        set { m_vjdll.m_joyState[m_myIndex].Dial = ToJS( value ); }
      }


      public void SetPOV( int pov, POVType value )
      {
        m_vjdll.m_joyState[m_myIndex].POV &= ( ushort )~( ( int )0xf << ( ( 3 - pov ) * 4 ) );
        m_vjdll.m_joyState[m_myIndex].POV |= ( ushort )( ( int )value << ( ( 3 - pov ) * 4 ) );
      }

      public POVType GetPOV( int pov )
      {
        return ( POVType )( ( m_vjdll.m_joyState[m_myIndex].POV >> ( ( 3 - pov ) * 4 ) ) & 0xf );
      }

      public void SetButton( int button, bool value )
      {
        if ( value )
          m_vjdll.m_joyState[m_myIndex].Buttons |= ( uint )( 1 << button );
        else
          m_vjdll.m_joyState[m_myIndex].Buttons &= ( uint )~( 1 << button );
      }

      public bool GetButton( int button )
      {
        return ( ( m_vjdll.m_joyState[m_myIndex].Buttons & ( 1 << button ) ) == 1 );
      }
    }

    #endregion


  }
}
