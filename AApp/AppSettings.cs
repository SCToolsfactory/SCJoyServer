using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Drawing;

namespace SCJoyServer
{
  sealed class AppSettings : ApplicationSettingsBase
  {

    // Singleton
    private static readonly Lazy<AppSettings> m_lazy = new Lazy<AppSettings>( () => new AppSettings( ) );
    public static AppSettings Instance { get => m_lazy.Value; }

    private AppSettings()
    {
      if ( this.FirstRun ) {
        // migrate the settings to the new version if the app runs the first time
        try {
          this.Upgrade( );
        }
        catch { }
        this.FirstRun = false;
        this.Save( );
      }
    }

    #region Setting Properties

    // manages Upgrade
    [UserScopedSetting( )]
    [DefaultSettingValue( "True" )]
    public bool FirstRun
    {
      get { return (bool)this["FirstRun"]; }
      set { this["FirstRun"] = value; }
    }


    // Control bound settings
    [UserScopedSetting( )]
    [DefaultSettingValue( "10, 10" )]
    public Point FormLocation
    {
      get { return (Point)this["FormLocation"]; }
      set { this["FormLocation"] = value; }
    }

    // User Config Settings

    // Devices
    [UserScopedSetting( )]
    [DefaultSettingValue( "False" )]
    public bool UseKeyboard
    {
      get { return (bool)this["UseKeyboard"]; }
      set { this["UseKeyboard"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "" )]
    public string JoystickUsed
    {
      get { return (string)this["JoystickUsed"]; }
      set { this["JoystickUsed"] = value; }
    }

    // Server
    [UserScopedSetting( )]
    [DefaultSettingValue( "" )]
    public string ServerIP
    {
      get { return (string)this["ServerIP"]; }
      set { this["ServerIP"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "" )]
    public string ServerPort
    {
      get { return (string)this["ServerPort"]; }
      set { this["ServerPort"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "True" )]
    public bool UseUDP
    {
      get { return (bool)this["UseUDP"]; }
      set { this["UseUDP"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "False" )]
    public bool UseTCP
    {
      get { return (bool)this["UseTCP"]; }
      set { this["UseTCP"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "False" )]
    public bool ReportClients
    {
      get { return (bool)this["ReportClients"]; }
      set { this["ReportClients"] = value; }
    }


    // WebClient
    [UserScopedSetting( )]
    [DefaultSettingValue( "" )]
    public string WebServer
    {
      get { return (string)this["WebServer"]; }
      set { this["WebServer"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "" )]
    public string WebServerPort
    {
      get { return (string)this["WebServerPort"]; }
      set { this["WebServerPort"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "" )]
    public string UploadDir
    {
      get { return (string)this["UploadDir"]; }
      set { this["UploadDir"] = value; }
    }

    #endregion


  }
}
