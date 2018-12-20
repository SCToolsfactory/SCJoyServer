using System;
using System.Runtime.InteropServices;

using static vJoyInterfaceWrap.vJoyInterface_DLL;
using static vJoyInterfaceWrap.vJoyData;

namespace vJoyInterfaceWrap
{
  /// <summary>
  /// The C# interface for the vJoyInterface DLL
  /// 
  /// copied from the original source 
  /// MIT License
  /// Copyright( c) 2017 Shaul Eizikovich
  /// 
  /// https://github.com/shauleiz/vJoy/blob/master/apps/common/vJoyInterfaceCS/vJoyInterfaceWrap/Wrapper.cs
  ///  - detached struct defs into vJoyData.cs
  ///  - detached the API and Dll loader into a platform aware loader
  ///  
  /// </summary>
  public class vJoy
  {
    public static bool isDllLoaded { get => UnsafeNativeMethods.NativeModuleHandle != IntPtr.Zero; }

    /***************************************************/
    /*********** Various declarations ******************/
    /***************************************************/
    private static RemovalCbFunc UserRemCB;
    private static WrapRemovalCbFunc wrf;
    private static GCHandle hRemUserData;


    private static FfbCbFunc UserFfbCB;
    private static WrapFfbCbFunc wf;
    private static GCHandle hFfbUserData;
         
    /***************************************************/
    /********** Export functions (C#) ******************/
    /***************************************************/

    /////	General driver data
    public short GetvJoyVersion() { return UnsafeNativeMethods._GetvJoyVersion( ); }
    public bool vJoyEnabled() { return UnsafeNativeMethods._vJoyEnabled( ); }
    public string GetvJoyProductString() { return Marshal.PtrToStringAuto( UnsafeNativeMethods._GetvJoyProductString( ) ); }
    public string GetvJoyManufacturerString() { return Marshal.PtrToStringAuto( UnsafeNativeMethods._GetvJoyManufacturerString( ) ); }
    public string GetvJoySerialNumberString() { return Marshal.PtrToStringAuto( UnsafeNativeMethods._GetvJoySerialNumberString( ) ); }
    public bool DriverMatch( ref UInt32 DllVer, ref UInt32 DrvVer ) { return UnsafeNativeMethods._DriverMatch( ref DllVer, ref DrvVer ); }

    /////	vJoy Device properties
    public int GetVJDButtonNumber( uint rID ) { return UnsafeNativeMethods._GetVJDButtonNumber( rID ); }
    public int GetVJDDiscPovNumber( uint rID ) { return UnsafeNativeMethods._GetVJDDiscPovNumber( rID ); }
    public int GetVJDContPovNumber( uint rID ) { return UnsafeNativeMethods._GetVJDContPovNumber( rID ); }
    public bool GetVJDAxisExist( UInt32 rID, HID_USAGES Axis )
    {
      UInt32 res = UnsafeNativeMethods._GetVJDAxisExist( rID, (uint)Axis );
      if ( res == 1 )
        return true;
      else
        return false;
    }
    public bool GetVJDAxisMax( UInt32 rID, HID_USAGES Axis, ref long Max ) { return UnsafeNativeMethods._GetVJDAxisMax( rID, (uint)Axis, ref Max ); }
    public bool GetVJDAxisMin( UInt32 rID, HID_USAGES Axis, ref long Min ) { return UnsafeNativeMethods._GetVJDAxisMin( rID, (uint)Axis, ref Min ); }
    public bool isVJDExists( UInt32 rID ) { return UnsafeNativeMethods._isVJDExists( rID ); }
    public int GetOwnerPid( UInt32 rID ) { return UnsafeNativeMethods._GetOwnerPid( rID ); }

    /////	Write access to vJoy Device - Basic
    public bool AcquireVJD( UInt32 rID ) { return UnsafeNativeMethods._AcquireVJD( rID ); }
    public void RelinquishVJD( uint rID ) { UnsafeNativeMethods._RelinquishVJD( rID ); }
    public bool UpdateVJD( UInt32 rID, ref JoystickState pData ) { return UnsafeNativeMethods._UpdateVJD( rID, ref pData ); }
    public VjdStat GetVJDStatus( UInt32 rID ) { return (VjdStat)UnsafeNativeMethods._GetVJDStatus( rID ); }

    //// Reset functions
    public bool ResetVJD( UInt32 rID ) { return UnsafeNativeMethods._ResetVJD( rID ); }
    public bool ResetAll() { return UnsafeNativeMethods._ResetAll( ); }
    public bool ResetButtons( UInt32 rID ) { return UnsafeNativeMethods._ResetButtons( rID ); }
    public bool ResetPovs( UInt32 rID ) { return UnsafeNativeMethods._ResetPovs( rID ); }

    ////// Write data
    public bool SetAxis( Int32 Value, UInt32 rID, HID_USAGES Axis ) { return UnsafeNativeMethods._SetAxis( Value, rID, Axis ); }
    public bool SetBtn( bool Value, UInt32 rID, uint nBtn ) { return UnsafeNativeMethods._SetBtn( Value, rID, (Byte)nBtn ); }
    public bool SetDiscPov( Int32 Value, UInt32 rID, uint nPov ) { return UnsafeNativeMethods._SetDiscPov( Value, rID, nPov ); }
    public bool SetContPov( Int32 Value, UInt32 rID, uint nPov ) { return UnsafeNativeMethods._SetContPov( Value, rID, nPov ); }

    // Register CB function that takes a C# object as userdata
    public void RegisterRemovalCB( RemovalCbFunc cb, object data )
    {
      // Free existing GCHandle (if exists)
      if ( hRemUserData.IsAllocated && hRemUserData.Target != null )
        hRemUserData.Free( );

      // Convert object to pointer
      hRemUserData = GCHandle.Alloc( data );

      // Apply the user-defined CB function          
      UserRemCB = new RemovalCbFunc( cb );
      wrf = new WrapRemovalCbFunc( WrapperRemCB );

      UnsafeNativeMethods._RegisterRemovalCB( wrf, (IntPtr)hRemUserData );
    }

    // Register CB function that takes a pointer as userdata
    public void RegisterRemovalCB( WrapRemovalCbFunc cb, IntPtr data )
    {
      wrf = new WrapRemovalCbFunc( cb );
      UnsafeNativeMethods._RegisterRemovalCB( wrf, data );
    }


    /////////////////////////////////////////////////////////////////////////////////////////////
    //// Force Feedback (FFB)


    public static void WrapperRemCB( bool complete, bool First, IntPtr userData )
    {

      object obj = null;

      if ( userData != IntPtr.Zero ) {
        // Convert userData from pointer to object
        GCHandle handle2 = (GCHandle)userData;
        obj = handle2.Target as object;
      }

      // Call user-defined CB function
      UserRemCB( complete, First, obj );
    }

    public static void WrapperFfbCB( IntPtr data, IntPtr userData )
    {

      object obj = null;

      if ( userData != IntPtr.Zero ) {
        // Convert userData from pointer to object
        GCHandle handle2 = (GCHandle)userData;
        obj = handle2.Target as object;
      }

      // Call user-defined CB function
      UserFfbCB( data, obj );
    }
          
    // Register CB function that takes a C# object as userdata
    public void FfbRegisterGenCB( FfbCbFunc cb, object data )
    {
      // Free existing GCHandle (if exists)
      if ( hFfbUserData.IsAllocated && hFfbUserData.Target != null )
        hFfbUserData.Free( );

      // Convert object to pointer
      hFfbUserData = GCHandle.Alloc( data );

      // Apply the user-defined CB function          
      UserFfbCB = new FfbCbFunc( cb );
      wf = new WrapFfbCbFunc( WrapperFfbCB );

      UnsafeNativeMethods._FfbRegisterGenCB( wf, (IntPtr)hFfbUserData );
    }

    // Register CB function that takes a pointer as userdata
    public void FfbRegisterGenCB( WrapFfbCbFunc cb, IntPtr data )
    {
      wf = new WrapFfbCbFunc( cb );
      UnsafeNativeMethods._FfbRegisterGenCB( wf, data );
    }

    [Obsolete( "you can remove the function from your code" )]
    public bool FfbStart( UInt32 rID ) { return UnsafeNativeMethods._FfbStart( rID ); }
    [Obsolete( "you can remove the function from your code" )]
    public bool FfbStop( UInt32 rID ) { return UnsafeNativeMethods._FfbStop( rID ); }
    public bool IsDeviceFfb( UInt32 rID ) { return UnsafeNativeMethods._IsDeviceFfb( rID ); }
    public bool IsDeviceFfbEffect( UInt32 rID, UInt32 Effect ) { return UnsafeNativeMethods._IsDeviceFfbEffect( rID, Effect ); }
    public UInt32 Ffb_h_DeviceID( IntPtr Packet, ref int DeviceID ) { return UnsafeNativeMethods._Ffb_h_DeviceID( Packet, ref DeviceID ); }
    public UInt32 Ffb_h_Type( IntPtr Packet, ref FFBPType Type ) { return UnsafeNativeMethods._Ffb_h_Type( Packet, ref Type ); }
    public UInt32 Ffb_h_Packet( IntPtr Packet, ref UInt32 Type, ref Int32 DataSize, ref Byte[] Data )
    {
      IntPtr buf = IntPtr.Zero;
      UInt32 res = UnsafeNativeMethods._Ffb_h_Packet( Packet, ref Type, ref DataSize, ref buf );
      if ( res != 0 )
        return res;

      DataSize -= 8;
      Data = new byte[DataSize];
      Marshal.Copy( buf, Data, 0, DataSize );
      return res;
    }
    public UInt32 Ffb_h_EBI( IntPtr Packet, ref Int32 Index ) { return UnsafeNativeMethods._Ffb_h_EBI( Packet, ref Index ); }
    [Obsolete( "use Ffb_h_Eff_Report instead" )]
    public UInt32 Ffb_h_Eff_Const( IntPtr Packet, ref FFB_EFF_CONST Effect ) { return UnsafeNativeMethods._Ffb_h_Eff_Const( Packet, ref Effect ); }
    public UInt32 Ffb_h_Eff_Report( IntPtr Packet, ref FFB_EFF_REPORT Effect ) { return UnsafeNativeMethods._Ffb_h_Eff_Report( Packet, ref Effect ); }
    public UInt32 Ffb_h_DevCtrl( IntPtr Packet, ref FFB_CTRL Control ) { return UnsafeNativeMethods._Ffb_h_DevCtrl( Packet, ref Control ); }
    public UInt32 Ffb_h_EffOp( IntPtr Packet, ref FFB_EFF_OP Operation ) { return UnsafeNativeMethods._Ffb_h_EffOp( Packet, ref Operation ); }
    public UInt32 Ffb_h_DevGain( IntPtr Packet, ref Byte Gain ) { return UnsafeNativeMethods._Ffb_h_DevGain( Packet, ref Gain ); }
    public UInt32 Ffb_h_Eff_Cond( IntPtr Packet, ref FFB_EFF_COND Condition ) { return UnsafeNativeMethods._Ffb_h_Eff_Cond( Packet, ref Condition ); }
    public UInt32 Ffb_h_Eff_Envlp( IntPtr Packet, ref FFB_EFF_ENVLP Envelope ) { return UnsafeNativeMethods._Ffb_h_Eff_Envlp( Packet, ref Envelope ); }
    public UInt32 Ffb_h_Eff_Period( IntPtr Packet, ref FFB_EFF_PERIOD Effect ) { return UnsafeNativeMethods._Ffb_h_Eff_Period( Packet, ref Effect ); }
    public UInt32 Ffb_h_EffNew( IntPtr Packet, ref FFBEType Effect ) { return UnsafeNativeMethods._Ffb_h_EffNew( Packet, ref Effect ); }
    public UInt32 Ffb_h_Eff_Ramp( IntPtr Packet, ref FFB_EFF_RAMP RampEffect ) { return UnsafeNativeMethods._Ffb_h_Eff_Ramp( Packet, ref RampEffect ); }
    public UInt32 Ffb_h_Eff_Constant( IntPtr Packet, ref FFB_EFF_CONSTANT ConstantEffect ) { return UnsafeNativeMethods._Ffb_h_Eff_Constant( Packet, ref ConstantEffect ); }



  }
}
