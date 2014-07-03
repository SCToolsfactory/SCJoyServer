using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

using System.Diagnostics;

namespace SCJoyServer
{
  /// <summary>
  /// Implements the TCP Server Listener
  /// waiting for connections of VJoy Devices, spawns a Server
  /// </summary>
  class ClientDispatcher
  {

    private const int   NUM_OF_THREAD = 8; //handle so many parallel clients

    private TcpListener m_tcpListener = null;
    private Boolean     m_abortListener = false;

    private Thread      m_listenerTask  = null;
    private IPAddress   m_ipAddress = null;
    private int         m_port = 0;

    /// <summary>
    /// cTor: creates a listener task that dispatches processing threads
    /// </summary>
    /// <param name="address">The IP address to listen</param>
    /// <param name="port">The port number to listen</param>
    public ClientDispatcher( IPAddress address, int port )
    {
      m_ipAddress = address;
      m_port = port;

      m_listenerTask = new Thread( new ThreadStart( this.StartListening ) );
      m_listenerTask.Start( );
    }


    public Boolean IsAlive
    {
      get { return m_listenerTask.IsAlive; }
    }


    /// <summary>
    /// Sets the abort status for the listener and waits for the exit
    /// </summary>
    public void Abort( )
    {
      m_abortListener = true; // atomic - no sync needed
      if ( m_tcpListener != null ) m_tcpListener.Stop( );
      if ( m_listenerTask != null && m_listenerTask.IsAlive ) {
        m_listenerTask.Join( ); // waits for the threads to terminate and finally for the ListenerTask itself
      }
      m_listenerTask = null;
    }


    #region Listener Thread routine

    /// <summary>
    /// Task Routine to listen to the connection port
    /// Owns and dispatches the clients 
    /// </summary>
    public void StartListening( )
    {
      // Client Connections Pool
      ClientConnectionPool connectionPool = new ClientConnectionPool( );
      // Client Task to handle client requests
      ClientService clientService = new ClientService( connectionPool );
      clientService.StartThreads( ); // start the threads
      // the connection point
      m_tcpListener = new TcpListener( m_ipAddress, m_port );

      try {
        int ClientNbr = 0;
        m_tcpListener.Start( NUM_OF_THREAD ); // can accept so many clients - limits the backlog of connections
        VJoyServerStatus.Instance.Debug( String.Format( "StartListening: IP {0} at port {1:D}\n", m_ipAddress.ToString( ), m_port ) );

        // Start listening for connections.
        while ( !m_abortListener ) {
          Console.WriteLine( "Waiting for a connection..." );
          TcpClient client = m_tcpListener.AcceptTcpClient( );

          if ( client != null ) {
            ClientNbr++;  // next client
            VJoyServerStatus.Instance.SetClientsStatus( String.Format( "Client #{0} accepted: {1}", ClientNbr, ( ( IPEndPoint )client.Client.RemoteEndPoint ).Address.ToString( ) ) );
            VJoyServerStatus.Instance.Debug( String.Format( "StartListening: Cl[{0}] accepted: {1}\n", ClientNbr, ( ( IPEndPoint )client.Client.RemoteEndPoint ).Address.ToString( ) ) );

            // An incoming connection needs to be processed.
            // create a client dedicated server instance and put it into the process queue of the ClientService
            connectionPool.Enqueue( new VJoyServer( client, ClientNbr ) );
          }
          else
            break;
        }

      }
      catch ( Exception e ) {
        VJoyServerStatus.Instance.Debug( String.Format( "\nStartListening: EXCEPTION:\n" ) );
        VJoyServerStatus.Instance.Debug( String.Format( "{0}\n", e.ToString( ) ));
      }

      // Stop client requests handling
      clientService.Stop( );
      VJoyServerStatus.Instance.Debug( String.Format( "StartListening: Done...\n") );
    }

    #endregion


    #region Class ClientService

    /// <summary>
    /// Creates a number of services that will handle the clients waiting in the queue
    /// </summary>
    class ClientService
    {
      private ClientConnectionPool ConnectionPool;
      private bool ContinueProcess = false;
      private Thread [] ThreadTask  = new Thread[NUM_OF_THREAD]; // the client processing threads

      public ClientService( ClientConnectionPool ConnectionPool )
      {
        this.ConnectionPool = ConnectionPool;
      }

      /// <summary>
      /// Start the Dispatcher threads
      /// </summary>
      public void StartThreads( )
      {
        ContinueProcess = true;
        // Start threads to handle Client Task
        for ( int i = 0; i < ThreadTask.Length; i++ ) {
          ThreadTask[i] = new Thread( new ThreadStart( this.Process ) );
          ThreadTask[i].Start( ); 
        }
      }

      /// <summary>
      /// The client thread environment
      /// Get a client connection from the pool and handles it
      /// </summary>
      private void Process( )
      {
        while ( ContinueProcess ) {

          VJoyServer client  = null;
          lock ( ConnectionPool.SyncRoot ) {
            if ( ConnectionPool.Count > 0 ) client = ConnectionPool.Dequeue( );
          }
          if ( client != null ) {
            client.Process( ); // Provoke client
            // if client still connected, schedule for later processing 
            if ( client.Alive ) ConnectionPool.Enqueue( client );
          }

          Thread.Sleep( 100 ); // dispatches clients every 100ms
        }
      }

      /// <summary>
      /// Shut the ClientService, its processes and the client connections pending
      /// </summary>
      public void Stop( )
      {
        if ( ! ContinueProcess ) return; // alreday called

        ContinueProcess = false;

        // Shut server threads - clients go back in the pool
        for ( int i = 0; i < ThreadTask.Length; i++ ) {
          if ( ThreadTask[i] != null && ThreadTask[i].IsAlive ) {
            ThreadTask[i].Join( ); // waits for the threads to terminate
          }
        }
        VJoyServerStatus.Instance.Debug( String.Format( "CDispatcher.Stop: Threads are down!\n") );

        // Close all pending client connections in the pool
        while ( ConnectionPool.Count > 0 ) {
          VJoyServer client = ConnectionPool.Dequeue( );
          client.Close( );
        }
        VJoyServerStatus.Instance.Debug( String.Format( "CDispatcher.Stop: Client connection are closed!\n" ) );

      }

    } // class ClientService

    #endregion


    #region Class ClientConnectionPool

    class ClientConnectionPool
    {
      // Creates a synchronized wrapper around the Queue.
      private  Queue SyncdQ = Queue.Synchronized( new Queue( ) );

      public void Enqueue( VJoyServer client )
      {
        SyncdQ.Enqueue( client );
      }

      public VJoyServer Dequeue( )
      {
        return ( VJoyServer )( SyncdQ.Dequeue( ) );
      }

      public int Count
      {
        get { return SyncdQ.Count; }
      }

      public object SyncRoot
      {
        get { return SyncdQ.SyncRoot; }
      }

    } // class ClientConnectionPool

    #endregion


  }// ClientDispatcher
}
