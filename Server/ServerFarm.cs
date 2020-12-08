using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJoyServer.Server
{
  /// <summary>
  /// Manages all servers provided
  /// </summary>
  class ServerFarm
  {
    private Dictionary<int, ServerManager> m_serverFarm = new Dictionary<int, ServerManager>( );


    public bool UdpRunning {
      get {
        bool ret = false;
        foreach ( var kv in m_serverFarm ) {
          ret |= kv.Value.UdpRunning;
        }
        return ret;
      }
    }

    public bool TcpRunning { 
      get {
        bool ret = false;
        foreach (var kv in m_serverFarm ) {
          ret |= kv.Value.TcpRunning;
        }
        return ret;
      }
    }


    /// <summary>
    /// Starts an UDP Service
    /// </summary>
    /// <param name="sIpAddress">The local IP to bind to</param>
    /// <param name="port">The local port to bind to</param>
    /// <param name="jsIndex">Joystick Index or -1 when no joystick should be connected</param>
    /// <param name="primaryPort">True if the port used is the primary one</param>
    public void StartUdpServer( string sIpAddress, int port, int jsIndex, bool primaryPort )
    {
      ServerManager sm = null;
      if ( m_serverFarm.ContainsKey( port ) ) {
        sm = m_serverFarm[port];
      }
      else {
        sm = new ServerManager( );
        m_serverFarm.Add( port, sm );
      }
      // start it
      sm.StartUdpServer( sIpAddress, port, jsIndex, primaryPort );
    }


    /// <summary>
    /// Starts a TCP Service
    /// </summary>
    /// <param name="sIpAddress">The local IP to bind to</param>
    /// <param name="port">The local port to bind to</param>
    /// <param name="jsIndex">Joystick Index or -1 when no joystick should be connected</param>
    /// <param name="primaryPort">True if the port used is the primary one</param>
    public void StartTcpServer( string sIpAddress, int port, int jsIndex, bool primaryPort )
    {
      ServerManager sm = null;
      if ( m_serverFarm.ContainsKey( port ) ) {
        sm = m_serverFarm[port];
      }
      else {
        sm = new ServerManager( );
        m_serverFarm.Add( port, sm );
      }
      // start it
      sm.StartTcpServer( sIpAddress, port, jsIndex, primaryPort );
    }


    /// <summary>
    /// Stops the Service and its threads
    /// </summary>
    public void StopServer( int port )
    {
      if ( m_serverFarm.ContainsKey( port ) ) {
        m_serverFarm[port].StopServer( );
      }
    }

    /// <summary>
    /// Stops all Services and its threads
    /// </summary>
    public void StopAllServers( )
    {
      VJoyServerStatus.Instance.Debug( $"Stopping all Servers\n" );

      foreach ( var kv in m_serverFarm ) {
        kv.Value.StopServer( );
      }
    }


  }
}
