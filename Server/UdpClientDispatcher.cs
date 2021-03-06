﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SCJoyServer.Server
{
  class UdpClientDispatcher
  {

    private const int NUM_OF_THREAD = 8; //handle so many parallel clients

    private UdpClient m_udpClient = null;
    private bool m_abortListener = false;

    private Thread m_clientTask = null;
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
    public UdpClientDispatcher( IPAddress address, int port, int jsIndex, bool primaryPort )
    {
      m_ipAddress = address;
      m_port = port;
      m_jsIndex = jsIndex;
      m_primaryPort = primaryPort;

      m_clientTask = new Thread( new ThreadStart( this.StartReceiving ) );
      m_clientTask.Start( );
    }


    public bool IsAlive
    {
      get { return m_clientTask.IsAlive; }
    }


    /// <summary>
    /// Sets the abort status for the listener and waits for the exit
    /// </summary>
    public void Abort()
    {
      m_abortListener = true; // atomic - no sync needed
      if ( m_udpClient != null ) m_udpClient.Close( );
      if ( m_clientTask != null && m_clientTask.IsAlive ) {
        m_clientTask.Join( ); // waits for the threads to terminate and finally for the ListenerTask itself
      }
      m_clientTask = null;
    }


    #region Listener Thread routine

    /// <summary>
    /// Task Routine to listen to the connection port
    /// Owns and dispatches the clients 
    /// </summary>
    public void StartReceiving()
    {
      // Client Connections Pool
      var connectionPool = new ClientConnectionPool( );
      // Client Task to handle client requests
      var clientService = new ClientService( connectionPool );
      clientService.StartThreads( ); // start the threads
      // the connection point
      m_udpClient = new UdpClient( new IPEndPoint( m_ipAddress, m_port ) );

      try {
        int ClientNbr = 0;
        // Start listening for connections.
        while ( !m_abortListener ) {
          Console.WriteLine( "Waiting for a connection..." );
          var remoteEP = new IPEndPoint( IPAddress.Any, 0 );
          var data = m_udpClient.Receive( ref remoteEP ); // blocks until receiving data

          if ( data != null ) {
            ClientNbr++;  // next client
            string clientEP = remoteEP.Address.ToString( );
            VJoyServerStatus.Instance.SetClientsStatus( $"UDP Client #{ClientNbr} data accepted: {clientEP}" );
            VJoyServerStatus.Instance.Debug( $"UDP: Client[{ClientNbr}] data accepted: {clientEP}\n" );

            // incoming data needs to be processed.
            // create a dedicated server instance and put it into the process queue of the ClientService
            connectionPool.Enqueue( new VJoyServer( data, ClientNbr, m_jsIndex, m_primaryPort ) );
          }
          else
            break;
        }//while

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
            client.ProcessData( ); // Provoke client
            // if client still connected, schedule for later processing (should not...)
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
