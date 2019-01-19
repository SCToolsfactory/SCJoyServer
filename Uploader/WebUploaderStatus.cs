using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJoyServer.Uploader
{
  class WebUploaderStatus
  {
    private WebUploaderStatus() { }

    public static WebUploaderStatus Instance { get; } = new WebUploaderStatus( );

    DebugForm DBF = null;

    public DebugForm ShowDebug(DebugForm dbf)
    {
      if ( dbf == null ) {
        DBF = new DebugForm( );
      }
      else {
        DBF = dbf;
      }
      DBF.Show( );

      return DBF;
    }

    public void HideDebug()
    {
      if ( DBF == null ) return;
      DBF.Hide( );
    }



    // ****

    #region SvrStatus Event

    public class WCliStatusEventArgs
    {
      public WCliStatusEventArgs( string s ) { Text = s; }
      public string Text { get; private set; } // readonly

    }

    // Declare the delegate (if using non-generic pattern).
    public delegate void WCliStatusEventHandler( object sender, WCliStatusEventArgs e );

    // Declare the event.
    public event WCliStatusEventHandler WCliStatusEvent;

    // Wrap the event in a protected virtual method
    // to enable derived classes to raise the event.
    protected virtual void RaiseWCliStatusEvent( string s )
    {
      // Raise the event by using the () operator.
      WCliStatusEvent?.Invoke( this, new WCliStatusEventArgs( s ) );
    }

    #endregion

    #region ClientsPing Event

    // Declare the delegate (if using non-generic pattern).
    public delegate void WCliPingEventHandler( object sender );

    // Declare the event.
    public event WCliPingEventHandler WCliPingEvent;

    // Wrap the event in a protected virtual method
    // to enable derived classes to raise the event.
    protected virtual void RaiseWCliPingEvent()
    {
      // Raise the event by using the () operator.
      WCliPingEvent?.Invoke( this );
    }

    #endregion


    #region ClientsDebug Event

    public class WCliDebugEventArgs
    {
      public WCliDebugEventArgs( string s ) { Text = s; }
      public string Text { get; private set; } // readonly

    }

    // Declare the delegate (if using non-generic pattern).
    public delegate void WCliDebugEventHandler( object sender, WCliDebugEventArgs e );

    // Declare the event.
    public event WCliDebugEventHandler WCliDebugEvent;

    // Wrap the event in a protected virtual method
    // to enable derived classes to raise the event.
    protected virtual void RaiseWCliDebugEvent( string s )
    {
      // Raise the event by using the () operator.
      WCliDebugEvent?.Invoke( this, new WCliDebugEventArgs( s ) );
    }

    #endregion


    public enum WCliStatus
    {
      Idle = 0,
      Running,
      Shutdown,
      Error,
    }
    private WCliStatus m_svrStatus = WCliStatus.Idle;

    public void SetSvrStatus( WCliStatus status )
    {
      m_svrStatus = status;
      RaiseWCliStatusEvent( m_svrStatus.ToString( ) );
    }

    public void SetPing()
    {
      RaiseWCliPingEvent( );
    }


    public void Debug( string msg )
    {
      RaiseWCliDebugEvent( msg );
    }


  }
}
