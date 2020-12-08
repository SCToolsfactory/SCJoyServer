using System;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace SCJoyServer.Server
{
  /// <summary>
  /// Implements the TCP Server Listener
  /// waiting for connections of VJoy Devices, spawns a Server
  /// </summary>
  class TcpClientDispatcher
  {

    private const int NUM_OF_THREAD = 8; //handle so many parallel clients

    private TcpListener m_tcpListener = null;
    private bool m_abortListener = false;

    private Thread m_listenerTask = null;
    private IPAddress m_ipAddress = null;
    private int m_port = 0;
    private int m_jsIndex = 0;
    private bool m_primaryPort = false;

    /// <summary>
    /// cTor: creates a listener task that dispatches processing threads
    /// </summary>
    /// <param name="address">The IP address to listen</param>
    /// <param name="port">The port number to listen</param>
    /// <param name="jsIndex">Joystick Index or -1 when no joystick should be connected</param>
    /// <param name="primaryPort">True if the port used is the primary one</param>
    public TcpClientDispatcher( IPAddress address, int port, int jsIndex, bool primaryPort )
    {
      m_ipAddress = address;
      m_port = port;
      m_jsIndex = jsIndex;
      m_primaryPort = primaryPort;

      m_listenerTask = new Thread( new ThreadStart( this.StartListening ) );
      m_listenerTask.Start( );
    }


    public bool IsAlive
    {
      get { return m_listenerTask.IsAlive; }
    }


    /// <summary>
    /// Sets the abort status for the listener and waits for the exit
    /// </summary>
    public void Abort()
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
    public void StartListening()
    {
      // Client Connections Pool
      var connectionPool = new ClientConnectionPool( );
      // Client Task to handle client requests
      var clientService = new ClientService( connectionPool );
      clientService.StartThreads( ); // start the threads
      // the connection point
      m_tcpListener = new TcpListener( m_ipAddress, m_port );

      try {
        int ClientNbr = 0;
        m_tcpListener.Start( NUM_OF_THREAD ); // can accept so many clients - limits the backlog of connections
        VJoyServerStatus.Instance.Debug( $"StartListening: IP {m_ipAddress.ToString( )} at port {m_port:D}\n" );

        // Start listening for connections.
        while ( !m_abortListener ) {
          Console.WriteLine( "Waiting for a connection..." );
          TcpClient client = null;
          if ( m_tcpListener.Pending( ) ) {
            client = m_tcpListener.AcceptTcpClient( );
          }

          if ( client != null ) {
            ClientNbr++;  // next client
            string clientEP = ( (IPEndPoint)client.Client.RemoteEndPoint ).Address.ToString( );
            VJoyServerStatus.Instance.SetClientsStatus( $"Client #{ClientNbr} accepted: {clientEP}" );
            VJoyServerStatus.Instance.Debug( $"StartListening: Cl[{ClientNbr}] accepted: {clientEP}\n" );

            // An incoming connection needs to be processed.
            // create a client dedicated server instance and put it into the process queue of the ClientService
            connectionPool.Enqueue( new VJoyServer( client, ClientNbr, m_jsIndex, m_primaryPort ) );
          }
          else {
            Thread.Sleep( 100 );
          }
        }// loop

      }
      catch ( Exception e ) {
        VJoyServerStatus.Instance.Debug( $"\nStartListening: EXCEPTION:\n" );
        VJoyServerStatus.Instance.Debug( $"{e.ToString( )}\n" );
      }

      // Stop client requests handling
      clientService.Stop( );
      VJoyServerStatus.Instance.Debug( $"StartListening: Done...\n" );
    }

    #endregion


    #region Class ClientService

    /// <summary>
    /// Creates a number of services that will handle the clients waiting in the queue
    /// </summary>
    class ClientService
    {
      private ClientConnectionPool m_connectionPool;
      private bool m_continueProcess = false;
      private Thread[] m_threadTask = new Thread[NUM_OF_THREAD]; // the client processing threads

      public ClientService( ClientConnectionPool ConnectionPool )
      {
        this.m_connectionPool = ConnectionPool;
      }

      /// <summary>
      /// Start the Dispatcher threads
      /// </summary>
      public void StartThreads()
      {
        m_continueProcess = true;
        // Start threads to handle Client Task
        for ( int i = 0; i < m_threadTask.Length; i++ ) {
          m_threadTask[i] = new Thread( new ThreadStart( this.Process ) );
          m_threadTask[i].Start( );
        }
      }

      /// <summary>
      /// The client thread environment
      /// Get a client connection from the pool and handles it
      /// </summary>
      private void Process()
      {
        while ( m_continueProcess ) {

          VJoyServer client = null;
          lock ( m_connectionPool.SyncRoot ) {
            if ( m_connectionPool.Count > 0 ) client = m_connectionPool.Dequeue( );
          }
          if ( client != null ) {
            client.ProcessNetworkStream( ); // Provoke client
            // if client still connected, schedule for later processing 
            if ( client.Alive ) m_connectionPool.Enqueue( client );
          }

          Thread.Sleep( 100 ); // dispatches clients every 100ms
        }
      }

      /// <summary>
      /// Shut the ClientService, its processes and the client connections pending
      /// </summary>
      public void Stop()
      {
        if ( !m_continueProcess ) return; // alreday called

        m_continueProcess = false;

        // Shut server threads - clients go back in the pool
        for ( int i = 0; i < m_threadTask.Length; i++ ) {
          if ( m_threadTask[i] != null && m_threadTask[i].IsAlive ) {
            m_threadTask[i].Join( ); // waits for the threads to terminate
          }
        }
        VJoyServerStatus.Instance.Debug( $"CDispatcher.Stop: Threads are down!\n" );

        // Close all pending client connections in the pool
        while ( m_connectionPool.Count > 0 ) {
          VJoyServer client = m_connectionPool.Dequeue( );
          client.Close( );
        }
        VJoyServerStatus.Instance.Debug( $"CDispatcher.Stop: Client connection are closed!\n" );

      }

    } // class ClientService

    #endregion


    #region Class ClientConnectionPool

    class ClientConnectionPool
    {
      // Creates a synchronized wrapper around the Queue.
      private Queue SyncdQ = Queue.Synchronized( new Queue( ) );

      public void Enqueue( VJoyServer client )
      {
        SyncdQ.Enqueue( client );
      }

      public VJoyServer Dequeue()
      {
        return (VJoyServer)( SyncdQ.Dequeue( ) );
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
