using System;
using System.Collections.Generic;
using System.Windows.Forms;

using SCJoyServer.Server;
using SCJoyServer.Uploader;

namespace SCJoyServer
{
  public partial class DebugForm : Form
  {
    public DebugForm( )
    {
      InitializeComponent( );
    }


    private void DebugForm_Load( object sender, EventArgs e )
    {
      // add handlers
      VJoyServerStatus.Instance.ClientsDebugEvent += new VJoyServerStatus.ClientsDebugEventHandler( Instance_ClientsDebugEvent );
      WebUploaderStatus.Instance.WCliDebugEvent += new WebUploaderStatus.WCliDebugEventHandler( Instance_WCliDebugEvent );
      timer1.Enabled = true;
    }

    private void DebugForm_FormClosing( object sender, FormClosingEventArgs e )
    {
      timer1.Enabled = false;
      VJoyServerStatus.Instance.ClientsDebugEvent -= Instance_ClientsDebugEvent;
      WebUploaderStatus.Instance.WCliDebugEvent -= Instance_WCliDebugEvent;
    }

    private void btClose_Click( object sender, EventArgs e )
    {
      this.Hide( );
    }

    private void timer1_Tick( object sender, EventArgs e )
    {
      while ( m_asyncMsgQ.Count > 0 ) {
        rtb.Text += m_asyncMsgQ.Dequeue( );
      }
    }

    #region Out of thread Event Handling

    // note: incomming events cannot manipulate the GUI !!!
    private Queue<String> m_asyncMsgQ = new Queue<String>( );

    void Instance_ClientsDebugEvent( object sender, VJoyServerStatus.ClientsDebugEventArgs e )
    {
      m_asyncMsgQ.Enqueue( e.Text );
    }

    private void Instance_WCliDebugEvent( object sender, WebUploaderStatus.WCliDebugEventArgs e )
    {
      m_asyncMsgQ.Enqueue( e.Text );
    }

    #endregion


  }
}
