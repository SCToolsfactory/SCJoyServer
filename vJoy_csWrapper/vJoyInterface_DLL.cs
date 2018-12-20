using System;
using System.Security;
using System.Runtime.InteropServices;

// supporting generic helper class
using static UnsafeNativeSupport.UnsafeNativeGeneric;

// some structs used for parameter passing
using static vJoyInterfaceWrap.vJoyData;


/// <summary>
/// Calling interface for vJoyInterface.dll  libraries.
/// 
/// The C# API and dll loader for the vJoyInterface DLL
/// 
/// copied from the original source 
/// MIT License
/// Copyright( c) 2017 Shaul Eizikovich
/// 
/// https://github.com/shauleiz/vJoy/blob/master/apps/common/vJoyInterfaceCS/vJoyInterfaceWrap/Wrapper.cs
///  - detached struct defs into vJoyData.cs
///  - detached the API and Dll loader into a platform aware loader (this file)
///  
/// Loader code derived from sqLite code (removed all conditionals and other specifics)
/// Also separated the generic Code to UnsafeNativeSupport.cs (which is used above..)
/// no copyright was included or attached to this file - taken from Internet 20160401
/// </summary>
namespace vJoyInterfaceWrap
{
  /// <summary>
  /// This static class provides some methods for the 
  /// native library pre-loader and other classes.
  /// 
  /// NOTE:  you may name this class whatever you like but suggested is  DllName_DLL
  /// Edit the Dll name below and the API itself
  /// </summary>
  internal static class vJoyInterface_DLL
  {
    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
      #region Shared Native Library Pre-Loading Code

      // The native module file name for the native library or null.
      private static string _NativeModuleFileName = null;


      private static IntPtr _NativeModuleHandle = IntPtr.Zero;
      /// <summary>
      /// The native module handle for the native library or the value IntPtr.Zero.
      /// </summary>
      public static IntPtr NativeModuleHandle { get => _NativeModuleHandle; }

      /// <summary>
      /// cTor: For now, this method simply calls the Initialize method.
      /// </summary>
      static UnsafeNativeMethods()
      {
        Initialize( );
      }

      /// <summary>
      /// Determines the base file name (without any directory information)
      /// for the native library to be pre-loaded by this class.
      /// </summary>
      /// <returns>
      /// The base file name for the native library to be pre-loaded by
      /// this class -OR- null if its value cannot be determined.
      /// </returns>
      public static string GetNativeLibraryFileNameOnly()
      {
        return Module_DLL_Name;
      }

      // This lock is used to protect the static _NativeModuleFileName,
      // _NativeModuleHandle fields
      private static readonly object m_staticSyncRoot = new object( );

      /// <summary>
      /// Attempts to initialize this class by pre-loading the native 
      /// library for the processor architecture of the current process.
      /// </summary>
      private static void Initialize()
      {
        lock ( m_staticSyncRoot ) {

          if ( _NativeModuleHandle == IntPtr.Zero ) {
            string baseDirectory = null;
            string processorArchitecture = null;

            SearchForDirectory( GetNativeLibraryFileNameOnly( ), ref baseDirectory, ref processorArchitecture );

            // NOTE: Attempt to pre-load the SCdxKeyboard core library (or
            //       interop assembly) and store both the file name
            //       and native module handle for later usage.
            PreLoad_Dll( GetNativeLibraryFileNameOnly( ), baseDirectory, processorArchitecture,
                          ref _NativeModuleFileName, ref _NativeModuleHandle );
          }
        }// lock
      }

      #endregion // Shared Native Library Pre-Loading Code

      /////////////////////////////////////////////////////////////////////////

      #region vJoyInterface API calls 

      internal const string Module_DLL_Name = "vJoyInterface.dll";

      /***************************************************/
      /***** Import from file vJoyInterface.dll (C) ******/
      /***************************************************/

      /////	General driver data
      [DllImport( Module_DLL_Name, EntryPoint = "GetvJoyVersion" )]
      internal static extern short _GetvJoyVersion();

      [DllImport( Module_DLL_Name, EntryPoint = "vJoyEnabled" )]
      internal static extern bool _vJoyEnabled();

      [DllImport( Module_DLL_Name, EntryPoint = "GetvJoyProductString" )]
      internal static extern IntPtr _GetvJoyProductString();

      [DllImport( Module_DLL_Name, EntryPoint = "GetvJoyManufacturerString" )]
      internal static extern IntPtr _GetvJoyManufacturerString();

      [DllImport( Module_DLL_Name, EntryPoint = "GetvJoySerialNumberString" )]
      internal static extern IntPtr _GetvJoySerialNumberString();

      [DllImport( Module_DLL_Name, EntryPoint = "DriverMatch" )]
      internal static extern bool _DriverMatch( ref UInt32 DllVer, ref UInt32 DrvVer );

      /////	vJoy Device properties
      [DllImport( Module_DLL_Name, EntryPoint = "GetVJDButtonNumber" )]
      internal static extern int _GetVJDButtonNumber( UInt32 rID );

      [DllImport( Module_DLL_Name, EntryPoint = "GetVJDDiscPovNumber" )]
      internal static extern int _GetVJDDiscPovNumber( UInt32 rID );

      [DllImport( Module_DLL_Name, EntryPoint = "GetVJDContPovNumber" )]
      internal static extern int _GetVJDContPovNumber( UInt32 rID );

      [DllImport( Module_DLL_Name, EntryPoint = "GetVJDAxisExist" )]
      internal static extern UInt32 _GetVJDAxisExist( UInt32 rID, UInt32 Axis );

      [DllImport( Module_DLL_Name, EntryPoint = "GetVJDAxisMax" )]
      internal static extern bool _GetVJDAxisMax( UInt32 rID, UInt32 Axis, ref long Max );

      [DllImport( Module_DLL_Name, EntryPoint = "GetVJDAxisMin" )]
      internal static extern bool _GetVJDAxisMin( UInt32 rID, UInt32 Axis, ref long Min );

      [DllImport( Module_DLL_Name, EntryPoint = "isVJDExists" )]
      internal static extern bool _isVJDExists( UInt32 rID );

      [DllImport( Module_DLL_Name, EntryPoint = "GetOwnerPid" )]
      internal static extern int _GetOwnerPid( UInt32 rID );

      /////	Write access to vJoy Device - Basic
      [DllImport( Module_DLL_Name, EntryPoint = "AcquireVJD" )]
      internal static extern bool _AcquireVJD( UInt32 rID );

      [DllImport( Module_DLL_Name, EntryPoint = "RelinquishVJD" )]
      internal static extern void _RelinquishVJD( UInt32 rID );

      [DllImport( Module_DLL_Name, EntryPoint = "UpdateVJD" )]
      internal static extern bool _UpdateVJD( UInt32 rID, ref JoystickState pData );

      [DllImport( Module_DLL_Name, EntryPoint = "GetVJDStatus" )]
      internal static extern int _GetVJDStatus( UInt32 rID );


      //// Reset functions
      [DllImport( Module_DLL_Name, EntryPoint = "ResetVJD" )]
      internal static extern bool _ResetVJD( UInt32 rID );

      [DllImport( Module_DLL_Name, EntryPoint = "ResetAll" )]
      internal static extern bool _ResetAll();

      [DllImport( Module_DLL_Name, EntryPoint = "ResetButtons" )]
      internal static extern bool _ResetButtons( UInt32 rID );

      [DllImport( Module_DLL_Name, EntryPoint = "ResetPovs" )]
      internal static extern bool _ResetPovs( UInt32 rID );

      ////// Write data
      [DllImport( Module_DLL_Name, EntryPoint = "SetAxis" )]
      internal static extern bool _SetAxis( Int32 Value, UInt32 rID, HID_USAGES Axis );

      [DllImport( Module_DLL_Name, EntryPoint = "SetBtn" )]
      internal static extern bool _SetBtn( bool Value, UInt32 rID, Byte nBtn );

      [DllImport( Module_DLL_Name, EntryPoint = "SetDiscPov" )]
      internal static extern bool _SetDiscPov( Int32 Value, UInt32 rID, uint nPov );

      [DllImport( Module_DLL_Name, EntryPoint = "SetContPov" )]
      internal static extern bool _SetContPov( Int32 Value, UInt32 rID, uint nPov );

      [DllImport( Module_DLL_Name, EntryPoint = "RegisterRemovalCB", CallingConvention = CallingConvention.Cdecl )]
      internal extern static void _RegisterRemovalCB( WrapRemovalCbFunc cb, IntPtr data );

      // Force Feedback (FFB)

      [DllImport( Module_DLL_Name, EntryPoint = "FfbRegisterGenCB", CallingConvention = CallingConvention.Cdecl )]
      internal extern static void _FfbRegisterGenCB( WrapFfbCbFunc cb, IntPtr data );

      [DllImport( Module_DLL_Name, EntryPoint = "FfbStart" )]
      internal static extern bool _FfbStart( UInt32 rID );

      [DllImport( Module_DLL_Name, EntryPoint = "FfbStop" )]
      internal static extern bool _FfbStop( UInt32 rID );

      [DllImport( Module_DLL_Name, EntryPoint = "IsDeviceFfb" )]
      internal static extern bool _IsDeviceFfb( UInt32 rID );

      [DllImport( Module_DLL_Name, EntryPoint = "IsDeviceFfbEffect" )]
      internal static extern bool _IsDeviceFfbEffect( UInt32 rID, UInt32 Effect );

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_DeviceID" )]
      internal static extern UInt32 _Ffb_h_DeviceID( IntPtr Packet, ref int DeviceID );

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_Type" )]
      internal static extern UInt32 _Ffb_h_Type( IntPtr Packet, ref FFBPType Type );

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_Packet" )]
      internal static extern UInt32 _Ffb_h_Packet( IntPtr Packet, ref UInt32 Type, ref Int32 DataSize, ref IntPtr Data );


      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_EBI" )]
      internal static extern UInt32 _Ffb_h_EBI( IntPtr Packet, ref Int32 Index );

#pragma warning disable 618
      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_Eff_Const" )]
      internal static extern UInt32 _Ffb_h_Eff_Const( IntPtr Packet, ref FFB_EFF_CONST Effect );
#pragma warning restore 618

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_Eff_Report" )]
      internal static extern UInt32 _Ffb_h_Eff_Report( IntPtr Packet, ref FFB_EFF_REPORT Effect );

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_DevCtrl" )]
      internal static extern UInt32 _Ffb_h_DevCtrl( IntPtr Packet, ref FFB_CTRL Control );

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_EffOp" )]
      internal static extern UInt32 _Ffb_h_EffOp( IntPtr Packet, ref FFB_EFF_OP Operation );

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_DevGain" )]
      internal static extern UInt32 _Ffb_h_DevGain( IntPtr Packet, ref Byte Gain );

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_Eff_Cond" )]
      internal static extern UInt32 _Ffb_h_Eff_Cond( IntPtr Packet, ref FFB_EFF_COND Condition );

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_Eff_Envlp" )]
      internal static extern UInt32 _Ffb_h_Eff_Envlp( IntPtr Packet, ref FFB_EFF_ENVLP Envelope );

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_Eff_Period" )]
      internal static extern UInt32 _Ffb_h_Eff_Period( IntPtr Packet, ref FFB_EFF_PERIOD Effect );

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_EffNew" )]
      internal static extern UInt32 _Ffb_h_EffNew( IntPtr Packet, ref FFBEType Effect );

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_Eff_Ramp" )]
      internal static extern UInt32 _Ffb_h_Eff_Ramp( IntPtr Packet, ref FFB_EFF_RAMP RampEffect );

      [DllImport( Module_DLL_Name, EntryPoint = "Ffb_h_Eff_Constant" )]
      internal static extern UInt32 _Ffb_h_Eff_Constant( IntPtr Packet, ref FFB_EFF_CONSTANT ConstantEffect );

      #endregion // vJoyInterface API calls

    }

  }
}
