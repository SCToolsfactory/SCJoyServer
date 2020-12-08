using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dxKbdInterfaceWrap;
using SCJoyServer.Server;
using SCJoyServer.Uploader;

namespace SCJoyServer
{
  public partial class FrmStatus : Form
  {

    private const string AppName = "SCJoyServer";

    #region Main Form

    private int m_msgReceived = 0;
    private int m_msgSent = 0;
    private long m_lastHash = 0;

    private DebugForm DBF = null;

    // asynch ping support
    private delegate void ExecPing( );
    private ExecPing myDelExecPing;

    private delegate void ExecWCliPing( );
    private ExecWCliPing myDelExecWCliPing;

    private string IconStringRunning { get => $"{AppName}\nService active"; }
    private string IconStringStopped { get => $"{AppName}\nService stopped"; }
    private string IconStringIdle { get => $"{AppName}\nService idle"; }

    /// <summary>
    /// Checks if a rectangle is visible on any screen
    /// </summary>
    /// <param name="formRect"></param>
    /// <returns>True if visible</returns>
    private static bool IsOnScreen( Rectangle formRect )
    {
      Screen[] screens = Screen.AllScreens;
      foreach ( Screen screen in screens ) {
        if ( screen.WorkingArea.Contains( formRect ) ) {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// cTor
    /// </summary>
    public FrmStatus( )
    {
      InitializeComponent( );

      myDelExecPing = new ExecPing( ExecPingMethod );
      myDelExecWCliPing = new ExecWCliPing( ExecWCliPingMethod );
      fswUploader.EnableRaisingEvents = false;
    }

    /// <summary>
    /// Init stuff
    /// </summary>
    private void Form1_Load( object sender, EventArgs e )
    {
      // add handlers
      VJoyServerStatus.Instance.SvrStatusEvent += new VJoyServerStatus.SvrStatusEventHandler( Instance_SvrStatusEvent );
      VJoyServerStatus.Instance.ClientsPingEvent += Instance_ClientsPingEvent;
      WebUploaderStatus.Instance.WCliStatusEvent += new WebUploaderStatus.WCliStatusEventHandler( Instance_WCliStatusEvent );
      WebUploaderStatus.Instance.WCliPingEvent += Instance_WCliPingEvent;
      // connect to feedback from vjAction
      vjAction.vJoy.vJoyHandler.Instance.Ping += vJoy_Ping;
      vjAction.Kbd.KbdHandler.Instance.Ping += Kbd_Ping;


      AppSettings.Instance.Reload( );

      // Assign Size property - check if on screen, else use defaults
      if ( IsOnScreen( new Rectangle( AppSettings.Instance.FormLocation, this.Size ) ) ) {
        this.Location = AppSettings.Instance.FormLocation;
      }

      string version = Application.ProductVersion;  // get the version information
      // BETA VERSION; TODO -  comment out if not longer
      //lblTitle.Text += " - V " + version.Substring( 0, version.IndexOf( ".", version.IndexOf( "." ) + 1 ) ); // PRODUCTION
      lblVersion.Text = "Version: " + version + " beta"; // BETA

      ICON.Text = IconStringIdle;

      // setup the GUI
      // Server
      txLocIP.Text = ServerManager.GetLocalIP( );
      var s = AppSettings.Instance.ServerIP;
      if ( !string.IsNullOrEmpty( s ) ) {
        txLocIP.Text = s;
      }
      pnlState.BackgroundImage = IL.Images["off"];

      txPort.Text = "34123";
      s = AppSettings.Instance.ServerPort;
      if ( !string.IsNullOrEmpty( s ) ) {
        txPort.Text = s;
      }

      cbxUdp.Checked = AppSettings.Instance.UseUDP;
      cbxTcp.Checked = AppSettings.Instance.UseTCP;
      cbxReport.Checked = AppSettings.Instance.ReportClients;

      // Web Client
      txRemIP.Text = ServerManager.GetLocalIP( );
      s = AppSettings.Instance.WebServer;
      if ( !string.IsNullOrEmpty( s ) ) {
        txRemIP.Text = s;
      }
      pnlUpState.BackgroundImage = IL.Images["off"];

      txRemPort.Text = "8080";
      s = AppSettings.Instance.WebServerPort;
      if ( !string.IsNullOrEmpty( s ) ) {
        txRemPort.Text = s;
      }

      txUpDir.Text = Environment.CurrentDirectory;
      s = AppSettings.Instance.UploadDir;
      if ( !string.IsNullOrEmpty( s ) && Directory.Exists( s ) ) {
        txUpDir.Text = s;
      }
      fswUploader.Path = txUpDir.Text;

      // vJoy DLL
      cbxJoystick.Items.Clear( );
      lblVJoy.Text = "not available";
      if ( vjAction.vjActionHandler.IsvJoyDllLoaded ) {
        for ( int i = 1; i <= 16; i++ ) {
          if ( vjAction.vjActionHandler.IsvJoyDeviceExists( i ) ) {
            cbxJoystick.Items.Add( $"Joystick#{i}" );
          }
        }
        if ( cbxJoystick.Items.Count > 0 ) {
          cbxJoystick.SelectedIndex = 0;
          // select the one in AppSettings
          string[] js = AppSettings.Instance.JoystickUsed.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // a list
          for ( int i = 0; i < js.Length; i++ ) {
            var idx = cbxJoystick.Items.IndexOf( js[i] );
            if ( idx >= 0 ) {
              cbxJoystick.SetItemChecked( idx, true );
            }
          }
        }
        lblVJoy.Text = $"loaded   - {cbxJoystick.Items.Count:#} device(s)";
      }
      // Kbd DLL
      if ( vjAction.vjActionHandler.IsKbdDllLoaded ) {
        lblSCdx.Text = "loaded";
        cbxKBon.Enabled = true;
        cbxKBon.Checked = AppSettings.Instance.UseKeyboard;
      }
      else {
        lblSCdx.Text = "not available";
        cbxKBon.Checked = false;
        cbxKBon.Enabled = false;
      }
      // counters
      lblSignal.Text = "0";
      lblUpSignal.Text = "0";
    }

    // asynch counter update TCP/UDP Server connection
    private void Instance_ClientsPingEvent( object sender )
    {
      this.Invoke( myDelExecPing );
    }
    private void Kbd_Ping( object sender, vjAction.vjActionEventArgs e )
    {
      this.Invoke( myDelExecPing );
    }

    private void vJoy_Ping( object sender, vjAction.vjActionEventArgs e )
    {
      this.Invoke( myDelExecPing );
    }


    private void ExecPingMethod( )
    {
      lblSignal.Text = ( ++m_msgReceived ).ToString( );
    }


    // asynch counter update WebClient
    private void Instance_WCliPingEvent( object sender )
    {
      this.Invoke( myDelExecWCliPing );
    }


    private void ExecWCliPingMethod( )
    {
      lblUpSignal.Text = ( ++m_msgSent ).ToString( );
    }



    private void Form1_FormClosing( object sender, FormClosingEventArgs e )
    {
      ICON.Text = IconStringStopped;

      // don't record minimized, maximized forms
      if ( this.WindowState == FormWindowState.Normal ) {
        AppSettings.Instance.FormLocation = this.Location;
      }
      AppSettings.Instance.UseKeyboard = cbxKBon.Checked;
      string s = "";
      foreach ( var cl in cbxJoystick.CheckedItems ) {
        s += cl.ToString( ) + " ";
      }
      AppSettings.Instance.JoystickUsed = s;
      AppSettings.Instance.ServerIP = txLocIP.Text;
      AppSettings.Instance.ServerPort = txPort.Text;
      AppSettings.Instance.UseUDP = cbxUdp.Checked;
      AppSettings.Instance.UseTCP = cbxTcp.Checked;
      AppSettings.Instance.ReportClients = cbxReport.Checked;

      AppSettings.Instance.WebServer = txRemIP.Text;
      AppSettings.Instance.WebServerPort = txRemPort.Text;
      AppSettings.Instance.UploadDir = txUpDir.Text;
      AppSettings.Instance.Save( );

      if ( UPLOADER != null && UPLOADER.WebClientRunning ) UPLOADER.Dispose( ); // shuts and disposes
      SVR.StopAllServers( ); // Shuts and wait for completion
    }

    #endregion



    private ServerFarm SVR = new ServerFarm( );
    private int SVR_PORT = 34123;

    private WebUploader UPLOADER = new WebUploader( );
    private int REM_PORT = 8080;


    #region Event Handling

    // receives in thread server status events
    void Instance_SvrStatusEvent( object sender, VJoyServerStatus.SvrStatusEventArgs e )
    {
      lblStatusTxt.Text = e.Text;
    }

    // receives in thread server status events
    void Instance_WCliStatusEvent( object sender, WebUploaderStatus.WCliStatusEventArgs e )
    {
      lblWCliStatusTxt.Text = e.Text;
    }


    // toggles Start/Stop
    private void btStartStop_Click( object sender, EventArgs e )
    {
      btStartStop.Enabled = false;
      if ( SVR.UdpRunning || SVR.TcpRunning ) {
        ICON.Text = IconStringStopped;
        SVR.StopAllServers( );
        btStartStop.Text = "Start Server";
        pnlState.BackgroundImage = IL.Images["off"];
        ICON.Text = IconStringIdle;
      }
      else {
        // start server(s)
        if ( !ServerManager.CheckIP( txLocIP.Text ) ) {
          txLocIP.Text = ServerManager.GetLocalIP( );
        }

        if ( !int.TryParse( txPort.Text, out int port ) ) {
          txPort.Text = SVR_PORT.ToString( );
          port = SVR_PORT;
        }
        lbxClients.Items.Clear( );
        int jsIndex = 0;
        int sport = port; // first one
        if ( cbxJoystick.CheckedItems.Count > 0 ) {
          bool primary=true; // first round
          // checked joysticks
          foreach ( var jsx in cbxJoystick.CheckedItems ) {
            string[] js = ( jsx as string ).Split( new char[] { '#' } );
            if ( js.Length > 1 ) {
              jsIndex = int.Parse( js[1] );

              if ( !primary )
                sport = port + ( jsIndex - 1 ); // NON primary ports use Port = BasePort + JsIndex-1 i.e. Js3 goes to Base+2 (if not primary)

              if ( cbxUdp.Checked ) {
                SVR.StartUdpServer( txLocIP.Text, sport, jsIndex, primary );
              }
              if ( cbxTcp.Checked ) {
                SVR.StartTcpServer( txLocIP.Text, sport, jsIndex, primary );
              }

              primary = false;
            }
          }
        }
        else {
          //Kbd Only 
          if ( cbxUdp.Checked ) {
            SVR.StartUdpServer( txLocIP.Text, port, -1, true );
          }
          if ( cbxTcp.Checked ) {
            SVR.StartTcpServer( txLocIP.Text, port, -1, true );
          }
        }


        if ( SVR.UdpRunning || SVR.TcpRunning ) {
          btStartStop.Text = "Stop Server";
          m_msgReceived = 0;
          lblSignal.Text = m_msgReceived.ToString( );
          pnlState.BackgroundImage = IL.Images["on"];

          AppSettings.Instance.ServerIP = txLocIP.Text;
          AppSettings.Instance.ServerPort = txPort.Text;
          AppSettings.Instance.UseUDP = cbxUdp.Checked;
          AppSettings.Instance.UseTCP = cbxTcp.Checked;
          AppSettings.Instance.ReportClients = cbxReport.Checked;
          AppSettings.Instance.Save( );

          ICON.Text = IconStringRunning;
        }
        else {
          pnlState.BackgroundImage = IL.Images["error"];
          ICON.Text = IconStringIdle;
        }
      }

      btStartStop.Enabled = true;
    }

    private void btDebugWin_Click( object sender, EventArgs e )
    {
      DBF = VJoyServerStatus.Instance.ShowDebug( DBF );
      DBF = WebUploaderStatus.Instance.ShowDebug( DBF );
    }

    private void btUpStartStop_Click( object sender, EventArgs e )
    {
      btUpStartStop.Enabled = false;
      if ( UPLOADER.WebClientRunning ) {
        fswUploader.EnableRaisingEvents = false;
        UPLOADER.StopService( );
        btUpStartStop.Text = "Start Client";
        pnlUpState.BackgroundImage = IL.Images["off"];
      }
      else {
        // start uploader
        if ( !ServerManager.CheckIP( txRemIP.Text ) ) {
          // input is not valid so we just try local
          txRemIP.Text = ServerManager.GetLocalIP( );
        }
        if ( !int.TryParse( txRemPort.Text, out int port ) ) {
          txRemPort.Text = REM_PORT.ToString( );
          port = REM_PORT;
        }

        if ( Directory.Exists( txUpDir.Text ) ) {
          UPLOADER.StartService( $"{txRemIP.Text}:{port:#0}" );
        }
        if ( UPLOADER.WebClientRunning ) {
          btUpStartStop.Text = "Stop Client";
          m_msgSent = 0;
          lblUpSignal.Text = m_msgSent.ToString( );
          pnlUpState.BackgroundImage = IL.Images["on"];

          AppSettings.Instance.WebServer = txRemIP.Text;
          AppSettings.Instance.WebServerPort = txRemPort.Text;
          AppSettings.Instance.Save( );

          fswUploader.EnableRaisingEvents = true;
        }
        else {
          pnlUpState.BackgroundImage = IL.Images["error"];
        }
      }

      btUpStartStop.Enabled = true;
    }

    private void btUpDir_Click( object sender, EventArgs e )
    {
      SFD.Title = "Upload Folder";
      SFD.FileName = "Dummy-DirSelector.json";
      if ( SFD.ShowDialog( ) == DialogResult.OK ) {
        txUpDir.Text = Path.GetDirectoryName( SFD.FileName );
        fswUploader.Path = txUpDir.Text;
        fswUploader.Filter = "*.json";
        fswUploader.NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.Size;

        AppSettings.Instance.UploadDir = txUpDir.Text;
        AppSettings.Instance.Save( );
      }
    }

    private void fswUploader_Changed( object sender, System.IO.FileSystemEventArgs e )
    {
      if ( UPLOADER == null ) return;


      if ( e.ChangeType == WatcherChangeTypes.Changed || e.ChangeType == WatcherChangeTypes.Created ) {
        var hash = File.GetLastWriteTime( e.FullPath ).Ticks;
        if ( hash == m_lastHash ) return; // we did this one already - sometimes the filewatcher gets more than one event..
        m_lastHash = hash;

        if ( UPLOADER.Upload( e.FullPath ) ) {
          ; // nothing but DEBUG stop
        }
        else {
          // have an error 
          VJoyServerStatus.Instance.Debug( $"{UPLOADER.Reply}\n" );
        }
      }
    }


    // check the out of thread event queue and display the ones arrives
    private void timer1_Tick( object sender, EventArgs e )
    {
      while ( m_asyncMsgQ.Count > 0 ) {
        if ( lbxClients.Items.Count > 10 ) {
          lbxClients.Items.RemoveAt( 0 );
        }
        lbxClients.Items.Add( m_asyncMsgQ.Dequeue( ) );
      }
    }

    private void cbxKBon_CheckedChanged( object sender, EventArgs e )
    {
      if ( SCdxKeyboard.isDllLoaded )
        SCdxKeyboard.Enabled = cbxKBon.Checked;
    }

    #endregion


    #region Out of thread Event Handling

    // note: incomming events cannot manipulate the GUI !!!
    private Queue<string> m_asyncMsgQ = new Queue<string>( );

    // receives out of thread client processing status events
    void Instance_ClientsStatusEvent( object sender, VJoyServerStatus.ClientsStatusEventArgs e )
    {
      m_asyncMsgQ.Enqueue( e.Text );
    }

    #endregion

    private void cbxReport_CheckedChanged( object sender, EventArgs e )
    {
      if ( cbxReport.Checked ) {
        VJoyServerStatus.Instance.ClientsStatusEvent += new VJoyServerStatus.ClientsStatusEventHandler( Instance_ClientsStatusEvent );
        timer1.Start( ); // display out of thread messages
      }
      else {
        VJoyServerStatus.Instance.ClientsStatusEvent -= Instance_ClientsStatusEvent;
        timer1.Stop( );
      }
    }

    private void ICON_DoubleClick( object sender, EventArgs e )
    {
      // Show the form when the user double clicks on the notify icon.
      if ( this.WindowState == FormWindowState.Minimized )
        this.WindowState = FormWindowState.Normal;

      // Activate the form.
      this.Activate( );
    }

    private void btMyIP_Click( object sender, EventArgs e )
    {
      txLocIP.Text = ServerManager.GetLocalIP( );
    }

  }
}
