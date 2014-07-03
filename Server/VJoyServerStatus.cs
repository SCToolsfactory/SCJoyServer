using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCJoyServer
{
  /// <summary>
  /// Maintain the VJoy Server(s) Status - provide events to subscribe
  /// </summary>
  class VJoyServerStatus
  {

    // Singleton
    private static readonly VJoyServerStatus instance = new VJoyServerStatus( );
    private VJoyServerStatus( )
    {
    }

    public static VJoyServerStatus Instance
    {
      get
      {
        return instance;
      }
    }

    DebugForm DBF = null; 

    public void ShowDebug( )
    {
      if ( DBF == null ) DBF = new DebugForm();
      DBF.Show( );
    }

    public void HideDebug( )
    {
      if ( DBF == null ) return;
      DBF.Hide( );
    }



    // ****

    #region SvrStatus Event

    public class SvrStatusEventArgs
    {
      public SvrStatusEventArgs( string s ) { Text = s; }
      public String Text { get; private set; } // readonly

    }

    // Declare the delegate (if using non-generic pattern).
    public delegate void SvrStatusEventHandler( object sender, SvrStatusEventArgs e );

    // Declare the event.
    public event SvrStatusEventHandler SvrStatusEvent;

    // Wrap the event in a protected virtual method
    // to enable derived classes to raise the event.
    protected virtual void RaiseSvrStatusEvent( String s )
    {
      // Raise the event by using the () operator.
      if ( SvrStatusEvent != null )
        SvrStatusEvent( this, new SvrStatusEventArgs( s ) );
    }

    #endregion


    #region ClientsStatus Event

    public class ClientsStatusEventArgs
    {
      public ClientsStatusEventArgs( string s ) { Text = s; }
      public String Text { get; private set; } // readonly

    }

    // Declare the delegate (if using non-generic pattern).
    public delegate void ClientsStatusEventHandler( object sender, ClientsStatusEventArgs e );

    // Declare the event.
    public event ClientsStatusEventHandler ClientsStatusEvent;

    // Wrap the event in a protected virtual method
    // to enable derived classes to raise the event.
    protected virtual void RaiseClientsStatusEvent( String s )
    {
      // Raise the event by using the () operator.
      if ( ClientsStatusEvent != null )
        ClientsStatusEvent( this, new ClientsStatusEventArgs( s ) );
    }

    #endregion


    #region ClientsDebug Event

    public class ClientsDebugEventArgs
    {
      public ClientsDebugEventArgs( string s ) { Text = s; }
      public String Text { get; private set; } // readonly

    }

    // Declare the delegate (if using non-generic pattern).
    public delegate void ClientsDebugEventHandler( object sender, ClientsDebugEventArgs e );

    // Declare the event.
    public event ClientsDebugEventHandler ClientsDebugEvent;

    // Wrap the event in a protected virtual method
    // to enable derived classes to raise the event.
    protected virtual void RaiseClientsDebugEvent( String s )
    {
      // Raise the event by using the () operator.
      if ( ClientsDebugEvent != null )
        ClientsDebugEvent( this, new ClientsDebugEventArgs( s ) );
    }

    #endregion


    public enum SvrStatus
    {
      Idle = 0,
      Running,
      Shutdown,
      Error,
    }
    private SvrStatus m_svrStatus = SvrStatus.Idle;

    public void SetSvrStatus( SvrStatus status )
    {
      m_svrStatus = status;
      RaiseSvrStatusEvent( m_svrStatus.ToString( ) );
    }

    public void SetClientsStatus( String msg )
    {
      RaiseClientsStatusEvent( msg );
    }

    public void Debug( String msg )
    {
      RaiseClientsDebugEvent( msg );
    }


  }
}
