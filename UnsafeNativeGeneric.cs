using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

/// <summary>
/// Project independent namespace for this utility class
/// </summary>
namespace UnsafeNativeSupport
{
  /// <summary>
  /// Generic part to implement a DLL API in c#.
  /// The calling interface for the dll is library specific and refers to this class.
  /// Derived from sqLite code (removed all conditionals and other specifics)
  /// Also separated the generic Code to UnsafeNativeSupport.cs (this file)
  /// no copyright was included or attached to this file - taken from Internet 20160401
  /// </summary>
  internal static class UnsafeNativeGeneric
  {
    #region Private Methods and Data
    /// <summary>
    /// This lock is used to protect the static field.
    /// </summary>
    private static readonly object m_staticSyncRoot = new object( );

    /// <summary>
    /// The name of the environment variable containing the processor
    /// architecture of the current process.
    /// </summary>
    private static readonly string PROCESSOR_ARCHITECTURE = "PROCESSOR_ARCHITECTURE";

    /// <summary>
    /// The file extension used for dynamic link libraries.
    /// </summary>
    private static readonly string m_dllFileExtension = ".dll";

    /////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This dictionary stores the mappings between processor architecture
    /// names and platform names.  These mappings are now used for two
    /// purposes.  First, they are used to determine if the assembly code
    /// base should be used instead of the location, based upon whether one
    /// or more of the named sub-directories exist within the assembly code
    /// base.  Second, they are used to assist in loading the appropriate
    /// SCdxKeyboard interop assembly into the current process.
    /// </summary>
    private static Dictionary<string, string> ProcessorArchitecturePlatforms;

    /////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Queries and returns the directory for the assembly currently being
    /// executed.
    /// </summary>
    /// <returns>
    /// The directory for the assembly currently being executed -OR- null if
    /// it cannot be determined.
    /// </returns>
    private static string GetAssemblyDirectory()
    {
      try {
        var assembly = Assembly.GetExecutingAssembly( );
        if ( assembly == null ) return null;
        string fileName = null;
        if ( !CheckAssemblyCodeBase( assembly, ref fileName ) ) fileName = assembly.Location;
        if ( string.IsNullOrEmpty( fileName ) ) return null;
        string directory = Path.GetDirectoryName( fileName );
        if ( string.IsNullOrEmpty( directory ) ) return null;
        return directory;
      }
      catch ( Exception ) {
      }
      return null;
    }


    private static Int32 CheckForArchitecturesAndPlatforms( string directory, ref List<string> matches )
    {
      Int32 result = 0;
      if ( matches == null ) matches = new List<string>( );
      lock ( m_staticSyncRoot ) {
        if ( !string.IsNullOrEmpty( directory ) &&
            ( ProcessorArchitecturePlatforms != null ) ) {
          foreach ( KeyValuePair<string, string> pair
                    in ProcessorArchitecturePlatforms ) {
            if ( Directory.Exists( MaybeCombinePath( directory, pair.Key ) ) ) {
              matches.Add( pair.Key );
              result++;
            }
            string value = pair.Value;
            if ( value == null )
              continue;

            if ( Directory.Exists( MaybeCombinePath( directory, value ) ) ) {
              matches.Add( value );
              result++;
            }
          }
        }
      }
      return result;
    }


    private static bool CheckAssemblyCodeBase( Assembly assembly, ref string fileName )
    {
      try {
        if ( assembly == null ) return false;

        string codeBase = assembly.CodeBase;
        if ( string.IsNullOrEmpty( codeBase ) ) return false;

        var uri = new Uri( codeBase );
        string localFileName = uri.LocalPath;
        if ( !File.Exists( localFileName ) ) return false;

        string directory = Path.GetDirectoryName( localFileName ); /* throw */
        List<string> matches = null;
        if ( CheckForArchitecturesAndPlatforms( directory, ref matches ) > 0 ) {
          fileName = localFileName;
          return true;
        }
        return false;
      }
      catch ( Exception ) {
      }
      return false;
    }

    #endregion

    /// <summary>
    /// cTor: get some basic system information here
    /// </summary>
    static UnsafeNativeGeneric()
    {
      // TODO: Make sure this list is updated if the supported processor architecture names and/or platform names
      //       changes.
      if ( ProcessorArchitecturePlatforms == null ) {
        //
        // Create the map of processor architecture names to platform names using a case-insensitive string comparer.
        ProcessorArchitecturePlatforms = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase ) {

              // NOTE: Setup the list of platform names associated with
              //       the supported processor architectures.
              { "x86", "Win32" },
              { "AMD64", "x64" }
            };
      }
    }

    #region Internal Methods
    /// <summary>
    /// Determines if the current process is running on one of the Windows
    /// [sub-]platforms.
    /// </summary>
    /// <returns>
    /// Non-zero when running on Windows; otherwise, zero.
    /// </returns>
    internal static bool IsWindows()
    {
      PlatformID platformId = Environment.OSVersion.Platform;
      if ( ( platformId == PlatformID.Win32Windows ) || ( platformId == PlatformID.Win32NT ) ) {
        return true;
      }
      return false;
    }

    /// <summary>
    /// This is a wrapper around the
    /// <see cref="string.Format(IFormatProvider, string, object[])" /> method.
    /// On Mono, it has to call the method overload without the
    /// <see cref="IFormatProvider" /> parameter, due to a bug in Mono.
    /// </summary>
    /// <param name="provider">
    /// This is used for culture-specific formatting.
    /// </param>
    /// <param name="format">
    /// The format string.
    /// </param>
    /// <param name="args">
    /// An array the objects to format.
    /// </param>
    /// <returns>
    /// The resulting string.
    /// </returns>
    internal static string stringFormat( IFormatProvider provider, string format, params object[] args )
    {
      return string.Format( provider, format, args );
    }

    /// <summary>
    /// Searches for the native SCdxKeyboard library in the directory containing
    /// the assembly currently being executed as well as the base directory
    /// for the current application domain.
    /// </summary>
    /// <param name="baseDirectory">
    /// Upon success, this parameter will be modified to refer to the base
    /// directory containing the native SCdxKeyboard library.
    /// </param>
    /// <param name="processorArchitecture">
    /// Upon success, this parameter will be modified to refer to the name
    /// of the immediate directory (i.e. the offset from the base directory)
    /// containing the native SCdxKeyboard library.
    /// </param>
    /// <returns>
    /// Non-zero (success) if the native SCdxKeyboard library was found; otherwise,
    /// zero (failure).
    /// </returns>
    internal static bool SearchForDirectory(
        string nativeLibraryFileNameOnly, /* in */
        ref string baseDirectory,        /* out */
        ref string processorArchitecture /* out */
        )
    {
      string fileNameOnly = nativeLibraryFileNameOnly;
      if ( fileNameOnly == null ) return false;

      // NOTE: Build the list of base directories and processor/platform
      //       names.  These lists will be used to help locate the native
      //       core library (or interop assembly) to pre-load into this process.
      string[] directories = { GetAssemblyDirectory( ), AppDomain.CurrentDomain.BaseDirectory, };
      string[] subDirectories = { GetProcessorArchitecture( ), GetPlatformName( null ) };
      foreach ( string directory in directories ) {
        if ( directory == null ) continue;
        foreach ( string subDirectory in subDirectories ) {
          if ( subDirectory == null ) continue;
          string fileName = FixUpDllFileName( MaybeCombinePath(
                        MaybeCombinePath( directory, subDirectory ),
                        fileNameOnly ) );
          // NOTE: If the DLL file exists, return success.
          //       Prior to returning, set the base directory and processor architecture to reflect the location
          //       where it was found.
          if ( File.Exists( fileName ) ) {
            baseDirectory = directory;
            processorArchitecture = subDirectory;
            return true; /* FOUND */
          }
        }
      }
      return false; /* NOT FOUND */
    }

    /// <summary>
    /// Queries and returns the base directory of the current application
    /// domain.
    /// </summary>
    /// <returns>
    /// The base directory for the current application domain -OR- null if it
    /// cannot be determined.
    /// </returns>
    internal static string GetBaseDirectory()
    {
      // NOTE: Otherwise, fallback on using the base directory of the
      //       current application domain.
      return AppDomain.CurrentDomain.BaseDirectory;
    }

    /// <summary>
    /// Determines if the dynamic link library file name requires a suffix
    /// and adds it if necessary.
    /// </summary>
    /// <param name="fileName">
    /// The original dynamic link library file name to inspect.
    /// </param>
    /// <returns>
    /// The dynamic link library file name, possibly modified to include an
    /// extension.
    /// </returns>
    internal static string FixUpDllFileName( string fileName )
    {
      if ( !string.IsNullOrEmpty( fileName ) ) {
        if ( IsWindows( ) ) {
          if ( !fileName.EndsWith( m_dllFileExtension,
                  StringComparison.OrdinalIgnoreCase ) ) {
            return fileName + m_dllFileExtension;
          }
        }
      }
      return fileName;
    }

    /// <summary>
    /// Queries and returns the processor architecture of the current process.
    /// </summary>
    /// <returns>The processor architecture of the current process -OR- null if it cannot be determined.</returns>
    internal static string GetProcessorArchitecture()
    {
      // BUGBUG: Will this always be reliable?
      string processorArchitecture = GetSettingValue( PROCESSOR_ARCHITECTURE, null );
      // if not set - use a ptr size hint
      if ( string.IsNullOrEmpty( processorArchitecture ) ) {
        processorArchitecture = "x86"; // default if not set
        if ( IntPtr.Size == sizeof( Int64 ) ) processorArchitecture = "AMD64"; // update if ptr size is 64bit
      }

      // HACK: Check for an "impossible" situation.  If the pointer size
      //       is 32-bits, the processor architecture cannot be "AMD64".
      //       In that case, we are almost certainly hitting a bug in the
      //       operating system and/or Visual Studio that causes the
      //       PROCESSOR_ARCHITECTURE environment variable to contain the
      //       wrong value in some circumstances.  Please refer to ticket
      //       [9ac9862611] for further information.
      if ( ( IntPtr.Size == sizeof( Int32 ) ) && string.Equals( processorArchitecture, "AMD64", StringComparison.OrdinalIgnoreCase ) ) {
        // fallback if ptr does not match
        processorArchitecture = "x86";
      }
      else if ( ( IntPtr.Size == sizeof( Int64 ) ) && string.Equals( processorArchitecture, "x86", StringComparison.OrdinalIgnoreCase ) ) {
        // fallback if ptr does not match
        processorArchitecture = "AMD64";
      }
      // IA-64 does virtually not exist anymore ..
      return processorArchitecture;
    }

    /// <summary>
    /// Given the processor architecture, returns the name of the platform.
    /// </summary>
    /// <param name="processorArchitecture">The processor architecture to be translated to a platform name.</param>
    /// <returns>The platform name for the specified processor architecture -OR- nullif it cannot be determined.</returns>
    internal static string GetPlatformName( string processorArchitecture )
    {
      if ( processorArchitecture == null ) processorArchitecture = GetProcessorArchitecture( );
      if ( string.IsNullOrEmpty( processorArchitecture ) ) return null;
      lock ( m_staticSyncRoot ) {
        if ( ProcessorArchitecturePlatforms == null ) return null;

        string platformName;
        if ( ProcessorArchitecturePlatforms.TryGetValue( processorArchitecture, out platformName ) ) return platformName;
      }
      return null;
    }

    /// <summary>
    /// Combines two path strings.
    /// </summary>
    /// <param name="path1">The first path -OR- null.</param>
    /// <param name="path2">The second path -OR- null.</param>
    /// <returns>The combined path string -OR- null if both of the original path strings are null.</returns>
    internal static string MaybeCombinePath( string path1, string path2 )
    {
      if ( path1 != null ) {
        if ( path2 != null )
          return Path.Combine( path1, path2 );
        else
          return path1;
      }
      else {
        if ( path2 != null )
          return path2;
        else
          return null;
      }
    }

    /// <summary>
    /// Queries and returns the value of the specified setting, using the XML
    /// configuration file and/or the environment variables for the current
    /// process and/or the current system, when available.
    /// </summary>
    /// <param name="name">The name of the setting.</param>
    /// <param name="default">
    /// The value to be returned if the setting has not been set explicitly
    /// or cannot be determined.
    /// </param>
    /// <returns>
    /// The value of the setting -OR- the default value specified by
    /// <paramref name="default" /> if it has not been set explicitly or
    /// cannot be determined.  By default, all references to existing
    /// environment variables will be expanded to their corresponding values
    /// within the value to be returned unless either the "No_Expand" or
    /// "No_Expand_<paramref name="name" />" environment variable is set [to
    /// anything].
    /// </returns>
    internal static string GetSettingValue(
        string name,    /* in */
        string @default /* in */
        )
    {
      if ( name == null ) return @default;

      string value = null;
      bool expand = true;
      value = Environment.GetEnvironmentVariable( name );
      if ( expand && !string.IsNullOrEmpty( value ) )
        value = Environment.ExpandEnvironmentVariables( value );
      if ( value != null ) return value;
      return @default;
    }

    private static string ListTostring( IList<string> list )
    {
      if ( list == null ) return null;

      var result = new StringBuilder( );
      foreach ( string element in list ) {
        if ( element == null ) continue;
        if ( result.Length > 0 ) result.Append( ' ' );
        result.Append( element );
      }
      return result.ToString( );
    }


    /// <summary>
    /// Attempts to load the native SCdxKeyboard library based on the specified
    /// directory and processor architecture.
    /// </summary>
    /// <param name="baseDirectory">
    /// The base directory to use, null for default (the base directory of
    /// the current application domain).  This directory should contain the
    /// processor architecture specific sub-directories.
    /// </param>
    /// <param name="processorArchitecture">
    /// The requested processor architecture, null for default (the
    /// processor architecture of the current process).  This caller should
    /// almost always specify null for this parameter.
    /// </param>
    /// <param name="nativeModuleFileName">
    /// The candidate native module file name to load will be stored here,
    /// if necessary.
    /// </param>
    /// <param name="nativeModuleHandle">
    /// The native module handle as returned by LoadLibrary will be stored
    /// here, if necessary.  This value will be IntPtr.Zero if the call to
    /// LoadLibrary fails.
    /// </param>
    /// <returns>
    /// Non-zero if the native module was loaded successfully; otherwise,
    /// zero.
    /// </returns>
    internal static bool PreLoad_Dll(
        string nativeLibraryFileNameOnly, /* in */
        string baseDirectory,            /* in */
        string processorArchitecture,    /* in */
        ref string nativeModuleFileName, /* out */
        ref IntPtr nativeModuleHandle    /* out */
        )
    {
      // NOTE: If the specified base directory is null, use the default
      //       (i.e. attempt to automatically detect it).
      if ( baseDirectory == null ) baseDirectory = GetBaseDirectory( );

      // NOTE: If we failed to query the base directory, stop now.
      if ( baseDirectory == null ) return false;

      // NOTE: Determine the base file name for the native SCdxKeyboard library.
      //       If this is not known by this class, we cannot continue.
      string fileNameOnly = nativeLibraryFileNameOnly;

      if ( fileNameOnly == null ) return false;
      // NOTE: If the native SCdxKeyboard library exists in the base directory itself, stop now.
      string fileName = FixUpDllFileName( MaybeCombinePath( baseDirectory, fileNameOnly ) );

      if ( File.Exists( fileName ) ) return false;
      // NOTE: If the specified processor architecture is null, use the default.
      if ( processorArchitecture == null ) processorArchitecture = GetProcessorArchitecture( );
      // NOTE: If we failed to query the processor architecture, stop now.
      if ( processorArchitecture == null ) return false;
      // NOTE: Build the full path and file name for the native SCdxKeyboard library using the processor architecture name.
      fileName = FixUpDllFileName( MaybeCombinePath( MaybeCombinePath( baseDirectory, processorArchitecture ), fileNameOnly ) );
      // NOTE: If the file name based on the processor architecture name is not found, try using the associated platform name.
      if ( !File.Exists( fileName ) ) {
        // NOTE: Attempt to translate the processor architecture to a platform name.
        string platformName = GetPlatformName( processorArchitecture );
        // NOTE: If we failed to translate the platform name, stop now.
        if ( platformName == null ) return false;
        // NOTE: Build the full path and file name for the native SCdxKeyboard library using the platform name.
        fileName = FixUpDllFileName( MaybeCombinePath( MaybeCombinePath( baseDirectory, platformName ), fileNameOnly ) );
        // NOTE: If the file does not exist, skip trying to load it.
        if ( !File.Exists( fileName ) ) return false;
      }

      try {
        // NOTE: Attempt to load the native library.  This will either
        //       return a valid native module handle, return IntPtr.Zero,
        //       or throw an exception.  This must use the appropriate
        //       P/Invoke method for the current operating system.
        nativeModuleFileName = fileName;
        nativeModuleHandle = NativeLibraryHelper.LoadLibrary( fileName );

        return ( nativeModuleHandle != IntPtr.Zero );
      }
      catch ( Exception ) {
      }

      return false;
    }
    
    #endregion

    /////////////////////////////////////////////////////////////////////////////

    #region Native Library Helper Class
    /// <summary>
    /// This static class provides a thin wrapper around the native library
    /// loading features of the underlying platform.
    /// </summary>
    internal static class NativeLibraryHelper
    {
      /// <summary>
      /// This delegate is used to wrap the concept of loading a native
      /// library, based on a file name, and returning the loaded module handle.
      /// </summary>
      /// <param name="fileName">The file name of the native library to load.</param>
      /// <returns>The native module handle upon success -OR- IntPtr.Zero on failure.</returns>
      private delegate IntPtr LoadLibraryCallback( string fileName );

      /////////////////////////////////////////////////////////////////////////

      /// <summary>
      /// Attempts to load the specified native library file using the Win32 API.
      /// </summary>
      /// <param name="fileName">The file name of the native library to load.</param>
      /// <returns>The native module handle upon success -OR- IntPtr.Zero on failure.</returns>
      private static IntPtr LoadLibraryWin32( string fileName )
      {
        return UnsafeNativeMethodsWin32.LoadLibrary( fileName );
      }


      /// <summary>
      /// Attempts to load the specified native library file.
      /// </summary>
      /// <param name="fileName">The file name of the native library to load.</param>
      /// <returns>The native module handle upon success -OR- IntPtr.Zero on failure.</returns>
      public static IntPtr LoadLibrary( string fileName )
      {
        LoadLibraryCallback callback = LoadLibraryWin32;
        return callback( fileName );
      }
    }
    #endregion

    /////////////////////////////////////////////////////////////////////////////

    #region Unmanaged Interop Methods Static Class (Win32)
    /// <summary>
    /// This class declares P/Invoke methods to call native Win32 APIs.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethodsWin32
    {

      /////////////////////////////////////////////////////////////////////////
      /// <summary>
      /// This is the P/Invoke method that wraps the native Win32 LoadLibrary
      /// function.  See the MSDN documentation for full details on what it
      /// does.
      /// </summary>
      /// <param name="fileName">
      /// The name of the executable library.
      /// </param>
      /// <returns>
      /// The native module handle upon success -OR- IntPtr.Zero on failure.
      /// </returns>
      [DllImport( "kernel32",
        CallingConvention = CallingConvention.Winapi,
        CharSet = CharSet.Ansi,
        BestFitMapping = false,
        ThrowOnUnmappableChar = false,
        SetLastError = true )]
      internal static extern IntPtr LoadLibrary( string fileName );

    }
    #endregion


  }
}
