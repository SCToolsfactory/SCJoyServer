using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SCJoyServer_Client
{
  class UdpMessenger
  {
    private string m_host = "";
    private IPAddress m_ipAddress = null;
    private int m_port = 0;
    private UdpClient m_udpClient = null;
    private IPEndPoint m_endpoint = null;

    public UdpMessenger( string remoteHost, int remotePort )
    {
      m_host = remoteHost;
      m_port = remotePort;

      if ( IPAddress.TryParse( remoteHost, out IPAddress lAddr ) ) {
        m_ipAddress = lAddr;
        m_endpoint = new IPEndPoint( m_ipAddress, m_port );
        m_udpClient = new UdpClient( );
      }
    }

    public bool SendMsg( string msg )
    {
      if ( m_endpoint == null ) return false;
      if ( m_udpClient == null ) return false;

      var ascii = new ASCIIEncoding( );

      byte[] buffer = ascii.GetBytes( msg );
      int bytes = buffer.Length;

      int howMuch = m_udpClient.Send( buffer, bytes, m_endpoint );
      if ( howMuch != bytes ) {
        // failed .. expect to send it in one call, no buffer manager here..
        return false;
      }

      return true;
    }

  }
}
