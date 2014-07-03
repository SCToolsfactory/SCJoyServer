using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCJoyServer
{
  public partial class Form1 : Form
  {

    #region Main Form

    public Form1( )
    {
      InitializeComponent( );

    }

    private void Form1_Load( object sender, EventArgs e )
    {
      // add handlers
      VJoyServerStatus.Instance.SvrStatusEvent += new VJoyServerStatus.SvrStatusEventHandler( Instance_SvrStatusEvent );
      VJoyServerStatus.Instance.ClientsStatusEvent += new VJoyServerStatus.ClientsStatusEventHandler( Instance_ClientsStatusEvent );

      // setup the GUI
      txLocIP.Text = ServerManager.GetLocalIP( );
      timer1.Start( ); // display out of thread messages
    }

    private void Form1_FormClosing( object sender, FormClosingEventArgs e )
    {
      if ( SVR.Running ) SVR.StopServer( ); // Shuts and wait for completion
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
      if ( SVR.Running ) {
        SVR.StopServer( );
        btStartStop.Text = "Start Server";
      }
      else {
        // start server
        if ( !ServerManager.CheckIP( txLocIP.Text ) ) {
          txLocIP.Text = ServerManager.GetLocalIP( );
        }

        int port;
        if ( !int.TryParse( txPort.Text, out port ) ) {
          txPort.Text = PORT.ToString( );
          port = PORT;
        }
        lbxClients.Items.Clear( );
        int jsIndex = ( rbJS1.Checked ) ? 0 : 1;
        SVR.StartServer( txLocIP.Text, port, jsIndex, "" );
        if ( SVR.Running ) btStartStop.Text = "Stop Server";
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
        lbxClients.Items.Add( m_asyncMsgQ.Dequeue( ) );
      }
    }

    #endregion


    #region Out of thread Event Handling

    // note: incomming events cannot manipulate the GUI !!!
    private Queue<String> m_asyncMsgQ = new Queue<String>( );

    // receives out of thread client processing status events
    void Instance_ClientsStatusEvent( object sender, VJoyServerStatus.ClientsStatusEventArgs e )
    {
      m_asyncMsgQ.Enqueue( e.Text );
    }

    #endregion




  }
}
