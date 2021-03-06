﻿using System;
using System.Net;
using System.Diagnostics;

using System.Net.NetworkInformation;

namespace SCJoyServer.Server
{

  /// <summary>
  /// The primary TCP server
  /// Accepts Clients to call in and dispatches them to a dedicated ServerInstance
  /// </summary>
  class ServerManager
  {

    #region Static IP Utilities

    /// <summary>
    /// Try to get the best IP address for this machine...
    /// ignores virtual and loopback adapters 
    /// </summary>
    /// <returns></returns>
    static public string GetLocalIP()
    {
      string localIP = "";

      foreach ( NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces( ) ) {
        if ( nic.OperationalStatus == OperationalStatus.Up ) {
          // must be up..
          if ( nic.Description.ToLowerInvariant( ).Contains( "virtual" ) ) continue; // not with virtual interfaces
          if ( nic.NetworkInterfaceType == NetworkInterfaceType.Loopback ) continue;

          IPInterfaceProperties ipProps = nic.GetIPProperties( );
          foreach ( var ips in ipProps.UnicastAddresses ) {
            if ( ips.Address.AddressFamily.ToString( ) == "InterNetwork" ) {
              // that would be a IpV4 address..
              localIP = ips.Address.ToString( );
              return localIP;
            }
          }
        }
        // check if localAddr is in ipProps.UnicastAddresses
      }
      return localIP;

      /*
      IPHostEntry host = Dns.GetHostEntry( Dns.GetHostName( ) );
      foreach ( IPAddress ip in host.AddressList ) {
        if ( ip.AddressFamily.ToString( ) == "InterNetwork" ) {
          // just catch the first for now - have to see how to do it better...
          localIP = ip.ToString( );
          break;
        }
      }
      return localIP;
      */
    }//GetLocalIP


    /// <summary>
    /// Checks for a valid IP and one that the server owns i.e. can be used to receive connections
    /// </summary>
    /// <param name="ipAddr">THe IP address string</param>
    /// <returns>True if it can be used</returns>
    static public bool CheckIP( string ipAddr )
    {
      if ( !string.IsNullOrEmpty( ipAddr ) ) {
        if ( IPAddress.TryParse( ipAddr, out IPAddress lAddr ) ) {
          // seems to be a valid IP
          if ( Equals( lAddr, IPAddress.Loopback ) ) return true;
          // check if we own such an IP
          IPHostEntry host = Dns.GetHostEntry( Dns.GetHostName( ) );
          foreach ( IPAddress ip in host.AddressList ) {
            if ( Equals( lAddr, ip ) ) return true;
          }
        }
      }
      return false; // nope
    }


    #endregion




    #region ServerManager

    private IPAddress m_localAddr = IPAddress.Loopback;
    private TcpClientDispatcher m_vjDispatcherTcp = null;    // instance of the dispatcher
    private UdpClientDispatcher m_vjDispatcherUdp = null;    // instance of the dispatcher
    private int m_port = 0;

    public bool UdpRunning { get; private set; } = false;
    public bool TcpRunning { get; private set; } = false;

    /// <summary>
    /// Starts the UDP Service
    /// </summary>
    /// <param name="sIpAddress">The local IP to bind to</param>
    /// <param name="port">The local port to bind to</param>
    /// <param name="jsIndex">Joystick Index or -1 when no joystick should be connected</param>
    /// <param name="primaryPort">True if the port used is the primary one</param>
    public void StartUdpServer( string sIpAddress, int port, int jsIndex, bool primaryPort )
    {
      if ( m_vjDispatcherUdp != null ) if ( m_vjDispatcherUdp.IsAlive ) return; // already running

      VJoyServerStatus.Instance.Debug( $"Starting UDP at port {port} for Joystick#{jsIndex}\n" );

      // Start the client interface
      m_port = port;
      IPAddress lAddr = m_localAddr;
      // see if we have a valid IP, else use loopback
      if ( !string.IsNullOrEmpty( sIpAddress ) ) {
        if ( !IPAddress.TryParse( sIpAddress, out lAddr ) ) {
          lAddr = m_localAddr; // use loopback
        }
      }

      // First try to connect the Joystick interface
      if ( ( jsIndex < 1 ) || ( jsIndex > 16 ) ) {
        // valid case if the library is not loaded - no valid joystick index selected
        VJoyServerStatus.Instance.Debug( $"\nno valid Joystick index supplied - will not use vJoy\n" );
      }
      else {
        if ( !vjAction.vjActionHandler.ConnectJoystick( jsIndex ) ) {
          VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Error );
          VJoyServerStatus.Instance.Debug( $"\nERROR - cannot start the Joystick Handler ...\n" );
          return; // ERROR - cannot connect
        }
      }

      // load and run the dispatcher
      m_vjDispatcherUdp = new UdpClientDispatcher( lAddr, port, jsIndex, primaryPort );
      if ( m_vjDispatcherUdp.IsAlive ) {
        UdpRunning = true;
        VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Running ); // maintain status information 
      }
      else {
        VJoyServerStatus.Instance.Debug( $"\nFailed to start the UDP dispatcher\n" );
        VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Error ); // maintain status information 
      }
    }


    /// <summary>
    /// Starts the TCP Service
    /// </summary>
    /// <param name="sIpAddress">The local IP to bind to</param>
    /// <param name="port">The local port to bind to</param>
    /// <param name="jsIndex">Joystick Index or -1 when no joystick should be connected</param>
    /// <param name="primaryPort">True if the port used is the primary one</param>
    public void StartTcpServer( string sIpAddress, int port, int jsIndex, bool primaryPort )
    {
      if ( m_vjDispatcherTcp != null ) if ( m_vjDispatcherTcp.IsAlive ) return; // already running

      VJoyServerStatus.Instance.Debug( $"Starting TCP at port {port} for Joystick#{jsIndex}\n" );

      // Start the client interface
      m_port = port;
      IPAddress lAddr = m_localAddr;
      // see if we have a valid IP, else use loopback
      if ( !string.IsNullOrEmpty( sIpAddress ) ) {
        if ( !IPAddress.TryParse( sIpAddress, out lAddr ) ) {
          lAddr = m_localAddr; // use loopback
        }
      }

      // First try to connect the Joystick interface
      if ( ( jsIndex < 1 ) || ( jsIndex > 16 ) ) {
        // valid case if the library is not loaded - no valid joystick index selected
        VJoyServerStatus.Instance.Debug( $"\nno valid Joystick index supplied - will not use vJoy\n" );
      }
      else {
        if ( !vjAction.vjActionHandler.ConnectJoystick( jsIndex ) ) {
          VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Error );
          VJoyServerStatus.Instance.Debug( $"\nERROR - cannot start the Joystick Handler ...\n" );
          return; // ERROR - cannot connect
        }
      }

      // load and run the dispatcher
      m_vjDispatcherTcp = new TcpClientDispatcher( lAddr, port, jsIndex, primaryPort );
      if ( m_vjDispatcherTcp.IsAlive ) {
        TcpRunning = true;
        VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Running ); // maintain status information 
      }
      else {
        VJoyServerStatus.Instance.Debug( $"\nFailed to start the TCP dispatcher\n" );
        VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Error ); // maintain status information 
      }
    }


    /// <summary>
    /// Stops the Service and its threads
    /// </summary>
    public void StopServer()
    {
      UdpRunning = false;
      TcpRunning = false;
      VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Shutdown ); // maintain status information 

      VJoyServerStatus.Instance.Debug( $"\nStopping Server at port {m_port}\n" );

      // take down dispatcher
      if ( m_vjDispatcherTcp != null ) {
        m_vjDispatcherTcp.Abort( );  // will only return after Joining the threads
        m_vjDispatcherTcp = null;
      }
      if ( m_vjDispatcherUdp != null ) {
        m_vjDispatcherUdp.Abort( );  // will only return after Joining the threads
        m_vjDispatcherUdp = null;
      }
      // should return once the dispatcher and it's threads are down.

      vjAction.vjActionHandler.DisconnectJoysticks( ); // shut the Joystick handler

      VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Idle ); // maintain status information 
    }

    #endregion

  }
}
