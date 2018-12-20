using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SCJoyServer.Server;

namespace SCJoyServer
{
  public partial class FrmStatus : Form
  {

    #region Main Form

    private int m_msgReceived = 0;

    // asynch ping support
    private delegate void ExecPing();
    private ExecPing myDelExecPing;


    public FrmStatus( )
    {
      InitializeComponent( );

      myDelExecPing = new ExecPing( ExecPingMethod );
    }

    private void Form1_Load( object sender, EventArgs e )
    {
      // add handlers
      VJoyServerStatus.Instance.SvrStatusEvent += new VJoyServerStatus.SvrStatusEventHandler( Instance_SvrStatusEvent );
      VJoyServerStatus.Instance.ClientsPingEvent += Instance_ClientsPingEvent;

      // setup the GUI
      txLocIP.Text = ServerManager.GetLocalIP( );
      pnlState.BackgroundImage = IL.Images["off"];

      cbxJoystick.Items.Clear( );
      lblVJoy.Text = "not available";
      if ( vJoyInterfaceWrap.vJoy.isDllLoaded ) {
        var tvJoy = new vJoyInterfaceWrap.vJoy( );
        for (uint i = 1; i<=16; i++ ) {
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
      lblSCdx.Text = ( DxKbd.SCdxKeyboard.isDllLoaded ) ? "loaded" : "not available";
      lblSignal.Text = "0";
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
      if ( SVR.UdpRunning || SVR.TcpRunning ) SVR.StopServer( ); // Shuts and wait for completion
    }

    #endregion



    private ServerManager SVR = new ServerManager( );
    private int       PORT = 34123;
    

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
          txPort.Text = PORT.ToString( );
          port = PORT;
        }
        lbxClients.Items.Clear( );
        int jsIndex = 0;
        string[] js = ( cbxJoystick.SelectedItem as string ).Split( new char[] { '#' } );
        if (js.Length>1) {
          jsIndex = int.Parse(js[1]);
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
        VJoyServerStatus.Instance.ClientsStatusEvent -=  Instance_ClientsStatusEvent;
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
