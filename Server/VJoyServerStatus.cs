namespace SCJoyServer.Server
{
  /// <summary>
  /// Maintain the VJoy Server(s) Status - provide events to subscribe
  /// </summary>
  class VJoyServerStatus
  {
    private VJoyServerStatus( )
    {
    }

    public static VJoyServerStatus Instance { get; } = new VJoyServerStatus( );

    DebugForm DBF = null;

    public DebugForm ShowDebug( DebugForm dbf )
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
      public string Text { get; private set; } // readonly

    }

    // Declare the delegate (if using non-generic pattern).
    public delegate void SvrStatusEventHandler( object sender, SvrStatusEventArgs e );

    // Declare the event.
    public event SvrStatusEventHandler SvrStatusEvent;

    // Wrap the event in a protected virtual method
    // to enable derived classes to raise the event.
    protected virtual void RaiseSvrStatusEvent( string s )
    {
      // Raise the event by using the () operator.
      SvrStatusEvent?.Invoke( this, new SvrStatusEventArgs( s ) );
    }

    #endregion


    #region ClientsStatus Event

    public class ClientsStatusEventArgs
    {
      public ClientsStatusEventArgs( string s ) { Text = s; }
      public string Text { get; private set; } // readonly

    }

    // Declare the delegate (if using non-generic pattern).
    public delegate void ClientsStatusEventHandler( object sender, ClientsStatusEventArgs e );

    // Declare the event.
    public event ClientsStatusEventHandler ClientsStatusEvent;

    // Wrap the event in a protected virtual method
    // to enable derived classes to raise the event.
    protected virtual void RaiseClientsStatusEvent( string s )
    {
      // Raise the event by using the () operator.
      ClientsStatusEvent?.Invoke( this, new ClientsStatusEventArgs( s ) );
    }

    #endregion


    #region ClientsPing Event

    // Declare the delegate (if using non-generic pattern).
    public delegate void ClientsPingEventHandler( object sender );

    // Declare the event.
    public event ClientsPingEventHandler ClientsPingEvent;

    // Wrap the event in a protected virtual method
    // to enable derived classes to raise the event.
    protected virtual void RaiseClientsPingEvent( )
    {
      // Raise the event by using the () operator.
      ClientsPingEvent?.Invoke( this );
    }

    #endregion


    #region ClientsDebug Event

    public class ClientsDebugEventArgs
    {
      public ClientsDebugEventArgs( string s ) { Text = s; }
      public string Text { get; private set; } // readonly

    }

    // Declare the delegate (if using non-generic pattern).
    public delegate void ClientsDebugEventHandler( object sender, ClientsDebugEventArgs e );

    // Declare the event.
    public event ClientsDebugEventHandler ClientsDebugEvent;

    // Wrap the event in a protected virtual method
    // to enable derived classes to raise the event.
    protected virtual void RaiseClientsDebugEvent( string s )
    {
      // Raise the event by using the () operator.
      ClientsDebugEvent?.Invoke( this, new ClientsDebugEventArgs( s ) );
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

    public void SetClientsStatus( string msg )
    {
      RaiseClientsStatusEvent( msg );
    }

    public void SetClientsPing( )
    {
      RaiseClientsPingEvent( );
    }

    public void Debug( string msg )
    {
      RaiseClientsDebugEvent( msg );
    }


  }
}
