using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SCJoyServer.Server;
using SCJoyServer.Uploader;

namespace SCJoyServer
{
  public partial class FrmStatus : Form
  {

    #region Main Form

    private int m_msgReceived = 0;
    private int m_msgSent = 0;
    private long m_lastHash = 0;

    // asynch ping support
    private delegate void ExecPing();
    private ExecPing myDelExecPing;


    public FrmStatus()
    {
      InitializeComponent( );

      myDelExecPing = new ExecPing( ExecPingMethod );
      fswUploader.EnableRaisingEvents = false;
    }

    private void Form1_Load( object sender, EventArgs e )
    {
      // add handlers
      VJoyServerStatus.Instance.SvrStatusEvent += new VJoyServerStatus.SvrStatusEventHandler( Instance_SvrStatusEvent );
      VJoyServerStatus.Instance.ClientsPingEvent += Instance_ClientsPingEvent;

      // setup the GUI
      txLocIP.Text = ServerManager.GetLocalIP( );
      pnlState.BackgroundImage = IL.Images["off"];

      txRemIP.Text = ServerManager.GetLocalIP( );
      pnlUpState.BackgroundImage = IL.Images["off"];

      // vJoy DLL
      cbxJoystick.Items.Clear( );
      lblVJoy.Text = "not available";
      if ( vJoyInterfaceWrap.vJoy.isDllLoaded ) {
        var tvJoy = new vJoyInterfaceWrap.vJoy( );
        for ( uint i = 1; i <= 16; i++ ) {
          if ( tvJoy.isVJDExists( i ) ) {
            cbxJoystick.Items.Add( $"Joystick #{i}" );
          }
        }
        if ( cbxJoystick.Items.Count > 0 ) {
          cbxJoystick.SelectedIndex = 0;
        }
        lblVJoy.Text = $"loaded   - {cbxJoystick.Items.Count:#} device(s)";
        tvJoy = null;
      }
      // Kbd DLL
      if ( DxKbd.SCdxKeyboard.isDllLoaded ) {
        cbxKBon.Checked = true;
        cbxKBon.Enabled = true;
        lblSCdx.Text = "loaded";
      }
      else {
        lblSCdx.Text = "not available";
      }
      lblSignal.Text = "0";
      lblUpSignal.Text = "0";
    }

    private void ExecPingMethod()
    {
      lblSignal.Text = ( ++m_msgReceived ).ToString( );
    }

    private void Instance_ClientsPingEvent( object sender )
    {
      this.Invoke( myDelExecPing );
    }


    private void Form1_FormClosing( object sender, FormClosingEventArgs e )
    {
      if ( UPLOADER != null ) UPLOADER.Dispose( );
      if ( SVR.UdpRunning || SVR.TcpRunning ) SVR.StopServer( ); // Shuts and wait for completion
    }

    #endregion



    private ServerManager SVR = new ServerManager( );
    private int SVR_PORT = 34123;

    private WebUploader UPLOADER = null;
    private int REM_PORT = 8080;

    #region Event Handling

    // receives in thread server status events
    void Instance_SvrStatusEvent( object sender, VJoyServerStatus.SvrStatusEventArgs e )
    {
      lblStatusTxt.Text = e.Text;
    }


    // toggles Start/Stop
    private void btStartStop_Click( object sender, EventArgs e )
    {
      btStartStop.Enabled = false;
      if ( SVR.UdpRunning || SVR.TcpRunning ) {
        SVR.StopServer( );
        btStartStop.Text = "Start Server";
        pnlState.BackgroundImage = IL.Images["off"];
      }
      else {
        // start server
        if ( !ServerManager.CheckIP( txLocIP.Text ) ) {
          txLocIP.Text = ServerManager.GetLocalIP( );
        }

        if ( !int.TryParse( txPort.Text, out int port ) ) {
          txPort.Text = SVR_PORT.ToString( );
          port = SVR_PORT;
        }
        lbxClients.Items.Clear( );
        int jsIndex = 0;
        if ( cbxJoystick.Items.Count > 0 ) {
          string[] js = ( cbxJoystick.SelectedItem as string ).Split( new char[] { '#' } );
          if ( js.Length > 1 ) {
            jsIndex = int.Parse( js[1] );
          }
        }

        if ( cbxUdp.Checked ) {
          SVR.StartUdpServer( txLocIP.Text, port, jsIndex, "" );
        }
        if ( cbxTcp.Checked ) {
          SVR.StartTcpServer( txLocIP.Text, port, jsIndex, "" );
        }

        if ( SVR.UdpRunning || SVR.TcpRunning ) {
          btStartStop.Text = "Stop Server";
          m_msgReceived = 0;
          lblSignal.Text = ( ++m_msgReceived ).ToString( );
          pnlState.BackgroundImage = IL.Images["on"];
        }
        else {
          pnlState.BackgroundImage = IL.Images["error"];
        }
      }

      btStartStop.Enabled = true;
    }

    private void btDebugWin_Click( object sender, EventArgs e )
    {
      VJoyServerStatus.Instance.ShowDebug( );
    }

    private void btUpStartStop_Click( object sender, EventArgs e )
    {
      btUpStartStop.Enabled = false;
      if ( UPLOADER != null ) {
        fswUploader.EnableRaisingEvents = false;
        UPLOADER.Dispose( );
        UPLOADER = null;
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
          UPLOADER = new WebUploader( $"{txRemIP.Text}:{port:#0}" );
        }
        if ( UPLOADER != null ) {
          btUpStartStop.Text = "Stop Client";
          pnlUpState.BackgroundImage = IL.Images["on"];
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
      }
    }

    private void fswUploader_Changed( object sender, System.IO.FileSystemEventArgs e )
    {
      if ( UPLOADER == null ) return;


      if ( e.ChangeType== WatcherChangeTypes.Changed || e.ChangeType== WatcherChangeTypes.Created ) {
        var hash = File.GetLastWriteTime( e.FullPath ).Ticks;
        if ( hash == m_lastHash ) return; // we did this one already - sometimes the filewatcher gets more than one event..
        m_lastHash = hash;

        if ( UPLOADER.Upload( e.FullPath ) ) {
          m_msgSent++;
          lblUpSignal.Text = $"{m_msgSent:#0}";
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
      if ( DxKbd.SCdxKeyboard.isDllLoaded )
        DxKbd.SCdxKeyboard.Enabled = cbxKBon.Checked;
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


  }
}
