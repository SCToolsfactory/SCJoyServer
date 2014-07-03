using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;

namespace SCJoyServer
{

  /// <summary>
  /// The primary TCP server
  /// Accepts Clients to call in and dispatches them to a dedicated ServerInstance
  /// </summary>
  class ServerManager
  {

    #region Static IP Utilities

    
    static public String GetLocalIP( )
    {
      IPHostEntry host;
      String localIP = "";

      host = Dns.GetHostEntry( Dns.GetHostName( ) );
      foreach ( IPAddress ip in host.AddressList ) {
        if ( ip.AddressFamily.ToString( ) == "InterNetwork" ) {
          // just catch the first for now - have to see how to do it better...
          localIP = ip.ToString( );
          break;
        }
      }
      return localIP;
    }//GetLocalIP


    /// <summary>
    /// Checks for a valid IP and one that the server owns i.e. can be used to receive connections
    /// </summary>
    /// <param name="ipAddr">THe IP address string</param>
    /// <returns>True if it can be used</returns>
    static public Boolean CheckIP( String ipAddr )
    {
      if ( !String.IsNullOrEmpty( ipAddr ) ) {
        IPAddress lAddr;
        if ( IPAddress.TryParse( ipAddr, out lAddr ) ) {
          // seems to be a valid IP
          if ( IPAddress.Equals(lAddr, IPAddress.Loopback) ) return true;
          // check if we own such an IP
          IPHostEntry host = Dns.GetHostEntry( Dns.GetHostName( ) );
          foreach ( IPAddress ip in host.AddressList ) {
            if ( IPAddress.Equals(lAddr,ip) ) return true;
          }
        }
      }
      return false; // nope
    }


    #endregion



    
    #region ServerManager

    private IPAddress               m_localAddr  = IPAddress.Loopback;
    private ClientDispatcher        m_vjDispatcher = null;    // instance of the dispatcher

    private Boolean                 m_running = false;
    public Boolean Running { get { return m_running; } }

    /// <summary>
    /// Starts the Service
    /// </summary>
    /// <param name="sIpAddress">The local IP to bind to</param>
    /// <param name="port">The local port to bind to</param>
    /// <param name="setupFilePath">A setupfile (not yet used)</param>
    public void StartServer( String sIpAddress, int port, int jsIndex, String setupFilePath )
    {
      if ( m_vjDispatcher != null ) if ( m_vjDispatcher.IsAlive ) return; // already running

      // Start the client interface
      IPAddress lAddr = m_localAddr;
      // see if we have a valid IP, else use loopback
      if ( !String.IsNullOrEmpty( sIpAddress ) ) {
        if ( !IPAddress.TryParse( sIpAddress, out lAddr ) ) {
          lAddr = m_localAddr; // use loopback
        }
      }

      // First try to connect the Joystick interface
      if ( ( jsIndex < 0 ) || ( jsIndex > 1 ) ) throw new IndexOutOfRangeException( );
      if ( !VJoyHandler.Instance.Connect( jsIndex ) ) {
        VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Error );
        Debug.Print( "\nERROR - cannot start the Joystick Handler ..." );
        return; // ERROR - cannot connect
      }

      // load and run the dispatcher
      m_vjDispatcher = new ClientDispatcher( lAddr, port );
      if ( m_vjDispatcher.IsAlive ) {
        m_running = true;
        VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Running ); // maintain status information 
      }
      else {
        VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Error ); // maintain status information 
      }
    }


    /// <summary>
    /// Stops the Service and its threads
    /// </summary>
    public void StopServer( )
    {
      m_running = false;
      VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Shutdown ); // maintain status information 

      // take down dispatcher
      if ( m_vjDispatcher != null ) {
        m_vjDispatcher.Abort( );  // will only return after Joining the threads
        m_vjDispatcher = null;
      }
      // should return once the dispatcher and it's threads are down.

      VJoyHandler.Instance.Disconnect( ); // shut the Joystick handler

      VJoyServerStatus.Instance.SetSvrStatus( VJoyServerStatus.SvrStatus.Idle ); // maintain status information 
    }

    #endregion

  }
}
