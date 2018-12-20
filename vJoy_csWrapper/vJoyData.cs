using System;
using System.Runtime.InteropServices;

/// <summary>
/// /// The C# structs and enums for the vJoyInterface DLL
/// 
/// copied from the original source: 
/// MIT License
/// Copyright( c) 2017 Shaul Eizikovich
/// 
/// https://github.com/shauleiz/vJoy/blob/master/apps/common/vJoyInterfaceCS/vJoyInterfaceWrap/Wrapper.cs
///  - detached struct defs into vJoyData.cs (this file)
///  - detached the API and Dll loader into a platform aware loader (this file)
///  
/// </summary>
namespace vJoyInterfaceWrap
{
  public enum HID_USAGES
  {
    HID_USAGE_X = 0x30,
    HID_USAGE_Y = 0x31,
    HID_USAGE_Z = 0x32,
    HID_USAGE_RX = 0x33,
    HID_USAGE_RY = 0x34,
    HID_USAGE_RZ = 0x35,
    HID_USAGE_SL0 = 0x36,
    HID_USAGE_SL1 = 0x37,
    HID_USAGE_WHL = 0x38,
    HID_USAGE_POV = 0x39,
  }

  public enum VjdStat  /* Declares an enumeration data type called BOOLEAN */
  {
    VJD_STAT_OWN,	// The  vJoy Device is owned by this application.
    VJD_STAT_FREE,	// The  vJoy Device is NOT owned by any application (including this one).
    VJD_STAT_BUSY,	// The  vJoy Device is owned by another application. It cannot be acquired by this application.
    VJD_STAT_MISS,	// The  vJoy Device is missing. It either does not exist or the driver is down.
    VJD_STAT_UNKN // Unknown
  };


  // FFB Declarations

  // HID Descriptor definitions - FFB Report IDs

  public enum FFBPType // FFB Packet Type
  {
    // Write
    PT_EFFREP = 0x01, // Usage Set Effect Report
    PT_ENVREP = 0x02, // Usage Set Envelope Report
    PT_CONDREP = 0x03,  // Usage Set Condition Report
    PT_PRIDREP = 0x04,  // Usage Set Periodic Report
    PT_CONSTREP = 0x05, // Usage Set Constant Force Report
    PT_RAMPREP = 0x06,  // Usage Set Ramp Force Report
    PT_CSTMREP = 0x07,  // Usage Custom Force Data Report
    PT_SMPLREP = 0x08,  // Usage Download Force Sample
    PT_EFOPREP = 0x0A,  // Usage Effect Operation Report
    PT_BLKFRREP = 0x0B, // Usage PID Block Free Report
    PT_CTRLREP = 0x0C,  // Usage PID Device Control
    PT_GAINREP = 0x0D,  // Usage Device Gain Report
    PT_SETCREP = 0x0E,  // Usage Set Custom Force Report

    // Feature
    PT_NEWEFREP = 0x01 + 0x10,  // Usage Create New Effect Report
    PT_BLKLDREP = 0x02 + 0x10,  // Usage Block Load Report
    PT_POOLREP = 0x03 + 0x10, // Usage PID Pool Report
  };

  public enum FFBEType // FFB Effect Type
  {

    // Effect Type
    ET_NONE = 0,    //    No Force
    ET_CONST = 1,    //    Constant Force
    ET_RAMP = 2,    //    Ramp
    ET_SQR = 3,    //    Square
    ET_SINE = 4,    //    Sine
    ET_TRNGL = 5,    //    Triangle
    ET_STUP = 6,    //    Sawtooth Up
    ET_STDN = 7,    //    Sawtooth Down
    ET_SPRNG = 8,    //    Spring
    ET_DMPR = 9,    //    Damper
    ET_INRT = 10,   //    Inertia
    ET_FRCTN = 11,   //    Friction
    ET_CSTM = 12,   //    Custom Force Data
  };

  public enum FFB_CTRL
  {
    CTRL_ENACT = 1,	// Enable all device actuators.
    CTRL_DISACT = 2,	// Disable all the device actuators.
    CTRL_STOPALL = 3,	// Stop All Effects­ Issues a stop on every running effect.
    CTRL_DEVRST = 4,	// Device Reset– Clears any device paused condition, enables all actuators and clears all effects from memory.
    CTRL_DEVPAUSE = 5,	// Device Pause– The all effects on the device are paused at the current time step.
    CTRL_DEVCONT = 6, // Device Continue– The all effects that running when the device was paused are restarted from their last time step.
  };

  public enum FFBOP
  {
    EFF_START = 1, // EFFECT START
    EFF_SOLO = 2, // EFFECT SOLO START
    EFF_STOP = 3, // EFFECT STOP
  };


  public class vJoyData
  {
    [StructLayout( LayoutKind.Sequential )]
    public struct JoystickState
    {
      public byte bDevice;
      public Int32 Throttle;
      public Int32 Rudder;
      public Int32 Aileron;
      public Int32 AxisX;
      public Int32 AxisY;
      public Int32 AxisZ;
      public Int32 AxisXRot;
      public Int32 AxisYRot;
      public Int32 AxisZRot;
      public Int32 Slider;
      public Int32 Dial;
      public Int32 Wheel;
      public Int32 AxisVX;
      public Int32 AxisVY;
      public Int32 AxisVZ;
      public Int32 AxisVBRX;
      public Int32 AxisVBRY;
      public Int32 AxisVBRZ;
      public UInt32 Buttons;
      public UInt32 bHats;  // Lower 4 bits: HAT switch or 16-bit of continuous HAT switch
      public UInt32 bHatsEx1; // Lower 4 bits: HAT switch or 16-bit of continuous HAT switch
      public UInt32 bHatsEx2; // Lower 4 bits: HAT switch or 16-bit of continuous HAT switch
      public UInt32 bHatsEx3; // Lower 4 bits: HAT switch or 16-bit of continuous HAT switch
      public UInt32 ButtonsEx1;
      public UInt32 ButtonsEx2;
      public UInt32 ButtonsEx3;
    };

    [StructLayout( LayoutKind.Sequential )]
    private struct FFB_DATA
    {
      private UInt32 size;
      private UInt32 cmd;
      private IntPtr data;
    }

    [StructLayout( LayoutKind.Explicit )]
    public struct FFB_EFF_CONSTANT
    {
      [FieldOffset( 0 )]
      public Byte EffectBlockIndex;
      [FieldOffset( 4 )]
      public Int16 Magnitude;
    }

    [System.Obsolete( "use FFB_EFF_REPORT" )]
    [StructLayout( LayoutKind.Explicit )]
    public struct FFB_EFF_CONST
    {
      [FieldOffset( 0 )]
      public Byte EffectBlockIndex;
      [FieldOffset( 4 )]
      public FFBEType EffectType;
      [FieldOffset( 8 )]
      public UInt16 Duration;// Value in milliseconds. 0xFFFF means infinite
      [FieldOffset( 10 )]
      public UInt16 TrigerRpt;
      [FieldOffset( 12 )]
      public UInt16 SamplePrd;
      [FieldOffset( 14 )]
      public Byte Gain;
      [FieldOffset( 15 )]
      public Byte TrigerBtn;
      [FieldOffset( 16 )]
      public bool Polar; // How to interpret force direction Polar (0-360°) or Cartesian (X,Y)
      [FieldOffset( 20 )]
      public Byte Direction; // Polar direction: (0x00-0xFF correspond to 0-360°)
      [FieldOffset( 20 )]
      public Byte DirX; // X direction: Positive values are To the right of the center (X); Negative are Two's complement
      [FieldOffset( 21 )]
      public Byte DirY; // Y direction: Positive values are below the center (Y); Negative are Two's complement
    }

    [StructLayout( LayoutKind.Explicit )]
    public struct FFB_EFF_REPORT
    {
      [FieldOffset( 0 )]
      public Byte EffectBlockIndex;
      [FieldOffset( 4 )]
      public FFBEType EffectType;
      [FieldOffset( 8 )]
      public UInt16 Duration;// Value in milliseconds. 0xFFFF means infinite
      [FieldOffset( 10 )]
      public UInt16 TrigerRpt;
      [FieldOffset( 12 )]
      public UInt16 SamplePrd;
      [FieldOffset( 14 )]
      public Byte Gain;
      [FieldOffset( 15 )]
      public Byte TrigerBtn;
      [FieldOffset( 16 )]
      public bool Polar; // How to interpret force direction Polar (0-360°) or Cartesian (X,Y)
      [FieldOffset( 20 )]
      public Byte Direction; // Polar direction: (0x00-0xFF correspond to 0-360°)
      [FieldOffset( 20 )]
      public Byte DirX; // X direction: Positive values are To the right of the center (X); Negative are Two's complement
      [FieldOffset( 21 )]
      public Byte DirY; // Y direction: Positive values are below the center (Y); Negative are Two's complement
    }

    [StructLayout( LayoutKind.Explicit )]
    public struct FFB_EFF_OP
    {
      [FieldOffset( 0 )]
      public Byte EffectBlockIndex;
      [FieldOffset( 4 )]
      public FFBOP EffectOp;
      [FieldOffset( 8 )]
      public Byte LoopCount;
    }

    [StructLayout( LayoutKind.Explicit )]
    public struct FFB_EFF_COND
    {
      [FieldOffset( 0 )]
      public Byte EffectBlockIndex;
      [FieldOffset( 4 )]
      public bool isY;
      [FieldOffset( 8 )]
      public Int16 CenterPointOffset; // CP Offset: Range 0x80­0x7F (­10000 ­ 10000)
      [FieldOffset( 12 )]
      public Int16 PosCoeff; // Positive Coefficient: Range 0x80­0x7F (­10000 ­ 10000)
      [FieldOffset( 16 )]
      public Int16 NegCoeff; // Negative Coefficient: Range 0x80­0x7F (­10000 ­ 10000)
      [FieldOffset( 20 )]
      public UInt32 PosSatur; // Positive Saturation: Range 0x00­0xFF (0 – 10000)
      [FieldOffset( 24 )]
      public UInt32 NegSatur; // Negative Saturation: Range 0x00­0xFF (0 – 10000)
      [FieldOffset( 28 )]
      public Int32 DeadBand; // Dead Band: : Range 0x00­0xFF (0 – 10000)
    }

    [StructLayout( LayoutKind.Explicit )]
    public struct FFB_EFF_ENVLP
    {
      [FieldOffset( 0 )]
      public Byte EffectBlockIndex;
      [FieldOffset( 4 )]
      public UInt16 AttackLevel;
      [FieldOffset( 8 )]
      public UInt16 FadeLevel;
      [FieldOffset( 12 )]
      public UInt32 AttackTime;
      [FieldOffset( 16 )]
      public UInt32 FadeTime;
    }

    [StructLayout( LayoutKind.Explicit )]
    public struct FFB_EFF_PERIOD
    {
      [FieldOffset( 0 )]
      public Byte EffectBlockIndex;
      [FieldOffset( 4 )]
      public UInt32 Magnitude;
      [FieldOffset( 8 )]
      public Int16 Offset;
      [FieldOffset( 12 )]
      public UInt32 Phase;
      [FieldOffset( 16 )]
      public UInt32 Period;
    }

    [StructLayout( LayoutKind.Explicit )]
    public struct FFB_EFF_RAMP
    {
      [FieldOffset( 0 )]
      public Byte EffectBlockIndex;
      [FieldOffset( 4 )]
      public Int16 Start;             // The Normalized magnitude at the start of the effect
      [FieldOffset( 8 )]
      public Int16 End;               // The Normalized magnitude at the end of the effect
    }


    // Force Feedback (FFB)
    public delegate void RemovalCbFunc( bool complete, bool First, object userData );
    public delegate void WrapRemovalCbFunc( bool complete, bool First, IntPtr userData );

    public delegate void FfbCbFunc( IntPtr data, object userData );
    public delegate void WrapFfbCbFunc( IntPtr data, IntPtr userData );

  }
}
