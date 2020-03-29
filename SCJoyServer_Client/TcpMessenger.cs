using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SCJoyServer_Client
{
  class TcpMessenger
  {
    private string m_host = "";
    private IPAddress m_ipAddress = null;
    private int m_port = 0;
    private TcpClient m_tcpClient = null;
    private IPEndPoint m_endpoint = null;
    private NetworkStream m_nStream = null;

    public TcpMessenger( string remoteHost, int remotePort )
    {
      m_host = remoteHost;
      m_port = remotePort;

      if ( IPAddress.TryParse( remoteHost, out IPAddress lAddr ) ) {
        m_ipAddress = lAddr;
        m_endpoint = new IPEndPoint( m_ipAddress, m_port );
        m_tcpClient = new TcpClient( );
      }
    }

    public string Error = "";

    public bool Connect()
    {
      if ( m_endpoint == null ) return false;
      if ( m_tcpClient == null ) return false;

      try {
        m_tcpClient.Connect( m_endpoint );
        if ( !m_tcpClient.Connected ) {
          Error = "Cannot connect";
          Disconnect( );
          return false;
        }
        m_nStream = m_tcpClient.GetStream( );
        if ( !m_nStream.CanWrite ) {
          Disconnect( );
          return false;
        }
        m_nStream.WriteTimeout = 5000; // 5 sec only..
      }
      catch ( SocketException se ) {
        Error = se.Message;
        Disconnect( );
        return false;
      }
      catch ( Exception e ) {
        Error = e.Message;
        Disconnect( );
        return false;
      }

      return m_tcpClient.Connected && m_nStream.CanWrite;
    }

    public void Disconnect()
    {
      if ( m_nStream != null ) {
        m_nStream.Close( );
        m_nStream = null;
      }
      if ( m_tcpClient != null ) {
        m_tcpClient.Close( );
        m_tcpClient = null;
      }
    }

    public bool SendMsg( string msg )
    {
      if ( m_endpoint == null ) return false;
      if ( m_tcpClient == null ) return false;
      if ( !m_tcpClient.Connected ) return false;
      if ( m_nStream == null ) return false;
      if ( !m_nStream.CanWrite ) return false;

      var ascii = new ASCIIEncoding( );

      byte[] buffer = ascii.GetBytes( msg );
      int bytes = buffer.Length;

      try {
        // despite checks above this may fail..
        m_nStream.Write( buffer, 0, bytes );
      }
      catch ( SocketException se ) {
        return false;
      }
      catch {
        return false;
      }

      return true;
    }

  }
}
