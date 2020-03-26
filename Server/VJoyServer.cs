using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

using SCJoyServer.VJoy;

namespace SCJoyServer.Server
{
  /// <summary>
  /// Implements a vJoy Server
  /// Binds a Client and read msgs
  /// There is no reply for now
  /// </summary>
  class VJoyServer
  {
    private string m_RecvBuffer = "";  // there we hold whatever needs to be stored for processing
    private byte[] m_byteBuffer = null;
    private TcpClient m_clientSocket = null;
    private string m_clientIP = "";
    private int m_clientNumber = 0;
    private NetworkStream m_networkStream;

    /// <summary>
    /// cTor: Setup the client processing UDP
    /// </summary>
    public VJoyServer( byte[] data, int clientNumber )
    {
      this.m_clientSocket = null;
      m_clientIP = "";
      m_clientNumber = clientNumber;

      m_networkStream = null;
      m_byteBuffer = new byte[data.Length];
      data.CopyTo( m_byteBuffer, 0 );
      Alive = true;
      VJoyServerStatus.Instance.Debug( $"VJoyServer.ctor UDP: Client[{m_clientNumber}] connected: {m_clientIP}\n" );
    }

    /// <summary>
    /// cTor: Setup the client processing and wait for Process() to run  TCP
    /// </summary>
    /// <param name="ClientSocket">An incomming client connection</param>
    public VJoyServer( TcpClient ClientSocket, int clientNumber )
    {
      this.m_clientSocket = ClientSocket;
      m_clientIP = ( (IPEndPoint)m_clientSocket.Client.RemoteEndPoint ).Address.ToString( );
      m_clientNumber = clientNumber;
      this.m_clientSocket.ReceiveTimeout = 5000; // timeout to get the clients disconnected if the server wants to shut down

      m_networkStream = ClientSocket.GetStream( );
      m_byteBuffer = new byte[ClientSocket.ReceiveBufferSize];
      Alive = true;

      VJoyServerStatus.Instance.SetClientsStatus( $"TCP Client #{m_clientNumber} connected: {m_clientIP}" );
      VJoyServerStatus.Instance.Debug( $"VJoyServer.ctor TCP: Client[{m_clientNumber}] connected: {m_clientIP}\n" );
    }

    /// <summary>
    /// Close this client connection and let the thread die
    /// possibly only after a timeout
    /// </summary>
    public void Close( bool withMsg = true )
    {
      Alive = false;
      if ( m_networkStream != null )
        m_networkStream.Close( );
      if ( m_clientSocket != null )
        m_clientSocket.Close( );

      if ( withMsg ) {
        VJoyServerStatus.Instance.SetClientsStatus( $"Client #{m_clientNumber} disconnected: {m_clientIP}" );
      }
      VJoyServerStatus.Instance.Debug( $"VJoyServer.Close: Client[{m_clientNumber}] disconnected: {m_clientIP}\n" );
    }

    /// <summary>
    /// Returns the state - True if the process is alive
    /// </summary>
    public bool Alive { get; private set; } = false;


    public void DebugMsg( string msg )
    {
      VJoyServerStatus.Instance.Debug( $"VJoyServer.thread: Cl[{m_clientNumber}]: {msg}" );
    }

    #region Thread routine

    /// <summary>
    /// Task Routine: Processes the clients requests UDP
    /// This just processes the data at hand and returns to the clientdispatcher
    /// </summary>
    public void ProcessData() 
    {
      try {
        // read from client and handle telegrams - it will care about invalid ones
        var vjCmd = ProcessMessage( );
        while ( vjCmd.IsValid ) {
          VJoyHandler.Instance.HandleMessage( vjCmd );
          vjCmd = ProcessMessage( ); // get next (if there are any..
        }
      }
      catch ( Exception e ) {
        DebugMsg( $"Other exception! {e}\n" );
      }
      Close( false ); // just dies after processing
    }  // ProcessData()


    /// <summary>
    /// Try to get a valid message
    /// </summary>
    /// <remarks>
    /// Received data is stored in the instance buffer and therefore
    /// the first thing is to see if there is already a complete message available
    /// from a former read - if not it will attempt to read from the stream
    /// The dispatcher call us every 100ms or so - this pace is high enough to process
    /// already received messages before reading more data from the client
    /// Read is with timeout - we can then shutdown if needed
    /// </remarks>
    /// <param name="stream">The network stream</param>
    /// <returns>A message, either valid or invalid</returns>
    private VJoyCommand.VJCommand ProcessMessage()
    {
      string recvBuffer = m_RecvBuffer; // load from last run

      // see if we have a complete message from a previous receive
      VJoyCommand.VJCommand vjCmd = VJoyCommand.TranslateMessage( ref recvBuffer );
      if ( vjCmd.IsValid ) {
        DebugMsg( $"Valid Cmd of type {vjCmd.CtrlType.ToString( )}\n" );
        m_RecvBuffer = recvBuffer; // save for next run
        Debug.Print( "Previous Message complete - start processing now" );
        return vjCmd; // regular exit with a valid message
      }

      // if we did not found a complete message in the receiving buffer, try to get a new one
      try {
        var ascii = new ASCIIEncoding( );
        Debug.Print( "Read from stream .." );
        int i = m_byteBuffer.Length;
        Debug.Print( "nRead: {0:D} - ", i );
        if ( i != 0 ) {
          recvBuffer += ascii.GetString( m_byteBuffer, 0, i );
          Array.Resize( ref m_byteBuffer, 0 ); // content is used
          if ( recvBuffer.Length > 0 ) DebugMsg( $"Received data: {XChar.XS( recvBuffer )}\n" );
          // translate complete messages into commands (if there is one available)
          vjCmd = VJoyCommand.TranslateMessage( ref recvBuffer );
          if ( vjCmd.IsValid ) {
            DebugMsg( $"Valid Cmd of type {vjCmd.CtrlType.ToString( )}\n" );
            m_RecvBuffer = recvBuffer; // save for next run
            Debug.Print( "Message complete - start processing now\n" );
            return vjCmd; // regular exit with a valid message
          }
          else {
            DebugMsg( $"Invalid Cmd or garbage\n" );
          }
        }
      }
      catch ( IOException ) {
        // client did not answer in time - this is OK
      }

      return new VJoyCommand.VJCommand( ); // the client did not complete the message - try next time

    }//ProcessMessage


    /// <summary>
    /// Task Routine: Processes the clients requests
    /// This just makes one receive round and returns to the clientdispatcher
    /// </summary>
    public void ProcessNetworkStream()
    {
      try {
        // read from client and handle a telegram - it will care about invalid ones
        VJoyHandler.Instance.HandleMessage( RecvMessage( m_networkStream ) );
      }
      catch ( SocketException e ) {
        Close( ); DebugMsg( $"Conection is broken! {e}\n" );
      }
      catch ( Exception e ) {
        Close( ); DebugMsg( $"Other exception! {e}\n" );
      }
    }  // Process()


    /// <summary>
    /// Try to get a valid message
    /// </summary>
    /// <remarks>
    /// Received data is stored in the instance buffer and therefore
    /// the first thing is to see if there is already a complete message available
    /// from a former read - if not it will attempt to read from the stream
    /// The dispatcher call us every 100ms or so - this pace is high enough to process
    /// already received messages before reading more data from the client
    /// Read is with timeout - we can then shutdown if needed
    /// </remarks>
    /// <param name="stream">The network stream</param>
    /// <returns>A message, either valid or invalid</returns>
    private VJoyCommand.VJCommand RecvMessage( NetworkStream stream )
    {
      string recvBuffer = m_RecvBuffer; // load from last run

      // see if we have a complete message from a previous receive
      VJoyCommand.VJCommand vjCmd = VJoyCommand.TranslateMessage( ref recvBuffer );
      if ( vjCmd.IsValid ) {
        m_RecvBuffer = recvBuffer; // save for next run
        Debug.Print( "Previous Message complete - start processing now" );
        return vjCmd; // regular exit with a valid message
      }

      // if we did not found a complete message in the receiving buffer, try to get a new one
      try {
        var ascii = new ASCIIEncoding( );
        Debug.Print( "Read from stream .." );
        int i = stream.Read( m_byteBuffer, 0, m_byteBuffer.Length ); // timeout raises the IOException
        if ( ( i == 0 ) && ( m_clientSocket.Available == 0 ) ) {
          // this is usually true when the client disapeared 
          // - there is no practical way to detect such a case other than trying to send something
          //   but there is no request/reply protocol implemented so far
          throw new SocketException( ); // handle outside
        }

        Debug.Print( "nRead: {0:D} - ", i );
        if ( i != 0 ) {
          recvBuffer += ascii.GetString( m_byteBuffer, 0, i );
          if ( recvBuffer.Length > 0 ) DebugMsg( $"Received data: {XChar.XS( recvBuffer )}\n" );
          // translate complete messages into commands (if there is one available)
          vjCmd = VJoyCommand.TranslateMessage( ref recvBuffer );
          if ( vjCmd.IsValid ) {
            DebugMsg( $"Valid Cmd of type {vjCmd.CtrlType.ToString( )}\n" );
            m_RecvBuffer = recvBuffer; // save for next run
            Debug.Print( "Message complete - start processing now\n" );
            return vjCmd; // regular exit with a valid message
          }
          else {
            DebugMsg( $"Invalid Cmd or garbage\n" );
          }
        }
      }
      catch ( IOException ) {
        // client did not answer in time - this is OK
      }

      return new VJoyCommand.VJCommand( ); // the client did not complete the message - try next time

    }//RecvMessage

    #endregion

  }
}
