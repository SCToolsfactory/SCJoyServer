using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

using System.Diagnostics;

namespace SCJoyServer
{
  /// <summary>
  /// Implements a vJoy Server
  /// Binds a Client and read msgs
  /// There is no reply for now
  /// </summary>
  class VJoyServer
  {
    private String                  m_RecvBuffer = "";  // there we hold whatever needs to be stored for processing
    private Byte[]                  m_byteBuffer = null;
    private TcpClient               m_clientSocket = null;
    private String                  m_clientIP = "";
    private int                     m_clientNumber = 0;
    private NetworkStream           m_networkStream;
    private bool                    m_continueProcess = false;

    /// <summary>
    /// cTor: Setup the client processing and wait for Process() to run 
    /// </summary>
    /// <param name="ClientSocket">An incomming client connection</param>
    public VJoyServer( TcpClient ClientSocket, int clientNumber )
    {
      this.m_clientSocket = ClientSocket;
      m_clientIP = ( ( IPEndPoint )m_clientSocket.Client.RemoteEndPoint ).Address.ToString( );
      m_clientNumber = clientNumber;
      this.m_clientSocket.ReceiveTimeout = 5000; // timeout to get the clients disconnected if the server wants to shut down

      m_networkStream = ClientSocket.GetStream( );
      m_byteBuffer = new Byte[ClientSocket.ReceiveBufferSize];
      m_continueProcess = true;

      VJoyServerStatus.Instance.SetClientsStatus( String.Format( "Client #{0} connected: {1}", m_clientNumber, m_clientIP ) );
      VJoyServerStatus.Instance.Debug( String.Format( "VJoyServer.ctor: Cl[{0}] connected: {1}\n", m_clientNumber, m_clientIP ) );
    }

    /// <summary>
    /// Close this client connection and let the thread die
    /// possibly only after a timeout
    /// </summary>
    public void Close( )
    {
      m_continueProcess = false;
      m_networkStream.Close( );
      m_clientSocket.Close( );

      VJoyServerStatus.Instance.SetClientsStatus( String.Format( "Client #{0} disconnected: {1}", m_clientNumber, m_clientIP ) );
      VJoyServerStatus.Instance.Debug( String.Format( "VJoyServer.Close: Cl[{0}] disconnected: {1}\n", m_clientNumber, m_clientIP ) );
    }

    /// <summary>
    /// Returns the state - True if the process is alive
    /// </summary>
    public bool Alive
    {
      get { return m_continueProcess; }
    }


    public void DebugMsg( String msg )
    {
      VJoyServerStatus.Instance.Debug( String.Format( "VJoyServer.thread: Cl[{0}]: {1}", m_clientNumber, msg ) );
    }

    #region Thread routine

    /// <summary>
    /// Task Routine: Processes the clients requests
    /// This just makes one receive round and returns to the clientdispatcher
    /// </summary>
    public void Process( )
    {
      try {
        // read from client and handle a telegram - it will care about invalid ones
        VJoyHandler.Instance.HandleMessage( RecvMessage( m_networkStream ) );
      }
      catch ( SocketException e ) {
        Close( ); DebugMsg( String.Format( "Conection is broken! {0}\n", e ) );
      }
      catch ( Exception e ) {
        Close( ); DebugMsg( String.Format( "Other exception! {0}\n", e ) );
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
      String recvBuffer = m_RecvBuffer; // load from last run

      // see if we have a complete message from a previous receive
      VJoyCommand.VJCommand vjCmd = VJoyCommand.TranslateMessage( ref recvBuffer );
      if ( vjCmd.IsValid ) {
        m_RecvBuffer = recvBuffer; // save for next run
        Debug.Print( "Previous Message complete - start processing now" );
        return vjCmd; // regular exit with a valid message
      }

      // if we did not found a complete message in the receiving buffer, try to get a new one
      try {
        ASCIIEncoding ascii = new ASCIIEncoding( );
        Debug.Print( "Read from stream .." );
        int  i = stream.Read( m_byteBuffer, 0, m_byteBuffer.Length ); // timeout raises the IOException
        if ( ( i == 0 ) && ( m_clientSocket.Available == 0 ) ) {
          // this is usually true when the client disapeared 
          // - there is no practical way to detect such a case other than trying to send something
          //   but there is no request/reply protocol implemented so far
          throw new SocketException( ); // handle outside
        }

        Debug.Print( "nRead: {0:D} - ", i );
        if ( i != 0 ) {
          recvBuffer += ascii.GetString( m_byteBuffer, 0, i );
          if ( recvBuffer.Length > 0 ) DebugMsg( String.Format( "Received data: {0}\n", XChar.XS( recvBuffer )) );
          // translate complete messages into commands (if there is one available)
          vjCmd = VJoyCommand.TranslateMessage( ref recvBuffer );
          if ( vjCmd.IsValid ) {
            DebugMsg( String.Format( "Valid Cmd of type {0}\n", vjCmd.CtrlType.ToString( ) ) );
            m_RecvBuffer = recvBuffer; // save for next run
            Debug.Print( "Message complete - start processing now\n" );
            return vjCmd; // regular exit with a valid message
          }
          else {
            DebugMsg( String.Format( "Invalid Cmd or garbage\n") );
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
