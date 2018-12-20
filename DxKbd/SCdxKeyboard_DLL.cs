using System;
using System.Security;
using System.Runtime.InteropServices;

// supporting generic helper class
using static UnsafeNativeSupport.UnsafeNativeGeneric;

/// <summary>
/// Calling interface for SCdxKeyboard.dll  libraries.
/// Derived from sqLite code (removed all conditionals and other specifics)
/// Also separated the generic Code to UnsafeNativeSupport.cs (which is included above..)
/// no copyright was included or attached to this file - taken from Internet 20160401
/// </summary>
namespace SCJoyServer.DxKbd
{
  /// <summary>
  /// This static class provides some methods for the 
  /// native library pre-loader and other classes.
  /// 
  /// NOTE:  you may name this class whatever you like but suggested is  DllName_DLL
  /// Edit the Dll name below and the API itself
  /// </summary>
  internal static class SCdxKeyboard_DLL
  {

    /// <summary>
    /// This class declares P/Invoke methods to call native APIs.
    /// </summary>
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
      internal static string GetNativeLibraryFileNameOnly()
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

      #region API calls 

      /// <summary>
      /// The native DLL name
      /// --> TO BE SET when using this for other modules
      /// </summary>
      private const string Module_DLL_Name = "SCdxKeyboard.dll";

      // ( VOID ) KeyDown( int vKey )
      [DllImport( Module_DLL_Name, EntryPoint = "KeyDown_C" )]
      internal static extern void KeyDown( Int32 vKey );

      // ( VOID ) KeyUp( int vKey )
      [DllImport( Module_DLL_Name, EntryPoint = "KeyUp_C" )]
      internal static extern void KeyUp( Int32 vKey );

      // ( VOID ) KeyTap( int vKey )
      [DllImport( Module_DLL_Name, EntryPoint = "KeyTap_C" )]
      internal static extern void KeyTap( Int32 vKey );

      // ( VOID ) KeyStroke( int vKey, unsigned short msec )
      [DllImport( Module_DLL_Name, EntryPoint = "KeyStroke_C" )]
      internal static extern void KeyStroke( Int32 vKey, UInt32 msec );

      // ( VOID ) Sleep_ms( unsigned short msec )
      [DllImport( Module_DLL_Name, EntryPoint = "Sleep_ms_C" )]
      internal static extern void Sleep_ms( UInt32 msec );

      #endregion // API calls

    }

  }
}
