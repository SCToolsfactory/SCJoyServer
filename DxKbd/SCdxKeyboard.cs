/**
* @file           SCdxKeyboard.cs
*****************************************************************************
* Consts
*
*  Interfaces the named DLL sending keycodes to the activa application
*
* Copyright (C) 2018 Martin Burri  (bm98@burri-web.org)
*
*
*<hr>
*
* @b Project      SXdxInput<br>
*
* @author         M. Burri
* @date           16-Dec-2018
*
*****************************************************************************
*<hr>
* @b Updates
* - 15-Aug-2019 BM:Enabled defaults to false to not catch early keystrokes
* - 21-Nar-2020 BM:Remove Enabled switch from Sleep ...
* - dd-mmm-yyyy V. Name: Description
*
*****************************************************************************/

using System;
using static SCJoyServer.DxKbd.SCdxKeyboard_DLL;

namespace SCJoyServer.DxKbd
{
  public class SCdxKeyboard
  {
    public static bool Enabled { get; set; } = false; // default false - not true...

    public static bool isDllLoaded { get => UnsafeNativeMethods.NativeModuleHandle != IntPtr.Zero; }

    public static void KeyDown( int vKey )
    {
      if ( Enabled )
        UnsafeNativeMethods.KeyDown( vKey );
    }

    public static void KeyUp( int vKey )
    {
      if ( Enabled )
        UnsafeNativeMethods.KeyUp( vKey );
    }

    public static void KeyTap( int vKey )
    {
      if ( Enabled )
        UnsafeNativeMethods.KeyTap( vKey );
    }

    public static void KeyStroke( int vKey, uint msec )
    {
      if ( Enabled )
        UnsafeNativeMethods.KeyStroke( vKey, msec );
    }

    public static void Sleep_ms( uint msec )
    {
        UnsafeNativeMethods.Sleep_ms( msec );
    }

  }
}
