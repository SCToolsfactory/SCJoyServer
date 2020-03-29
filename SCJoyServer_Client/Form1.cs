using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCJoyServer_Client
{
  public partial class Form1 : Form
  {

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
    }

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


    VJoyMsg m_joyMsg = new VJoyMsg( );
    UdpMessenger m_udp = null;
    TcpMessenger m_tcp = null;


    public Form1()
    {
      InitializeComponent( );
    }

    private void Form1_Load( object sender, EventArgs e )
    {
      // setup the GUI
      txLocIP.Text = GetLocalIP( );

    }

    private bool m_started = false;

    private void btStartStop_Click( object sender, EventArgs e )
    {
      btStartStop.Enabled = false;
      if ( m_started ) {
        // STOP
        if ( m_tcp != null ) m_tcp.Disconnect( );
        m_tcp = null;
        m_udp = null;

        btStartStop.Text = "Start Client";
        m_started = false;
      }
      else {
        // START
        lbxClients.Items.Clear( );
        if ( rbJS1.Checked ) {
          // TCP
          m_tcp = new TcpMessenger( txLocIP.Text, int.Parse( txPort.Text ) );
          if ( !m_tcp.Connect( ) ) {
            AddEntry( "*** TCP Failed to connect ***" );
            AddEntry( m_tcp.Error );
            m_tcp = null;
          }
          else {
            lbxClients.Items.Add( "*** TCP connected ***" );
            m_started = true;
          }
        }
        else {
          // UDP
          m_udp = new UdpMessenger( txLocIP.Text, int.Parse( txPort.Text ) );
          AddEntry( "*** UDP started ***" );
          m_started = true;
        }

        if ( m_started ) {
          btStartStop.Text = "Stop Client";
        }
      }

      btStartStop.Enabled = true;
    }


    private void btSendNext_Click( object sender, EventArgs e )
    {
      string msg = m_joyMsg.NextMsg( );
      AddEntry( msg );
      if ( m_udp!=null ) {
        if ( !m_udp.SendMsg( msg ) ) {
          AddEntry( "*** Failed ***" );
        }
      }
      else if ( m_tcp != null ) {
        if ( !m_tcp.SendMsg( msg ) ) {
          AddEntry( "*** Failed ***" );
        }
      }
      else {
        AddEntry( "*** Failed - no client active ***" );
      }
    }

    private void AddEntry(string entry )
    {
      if ( lbxClients.Items.Count > 10 ) {
        lbxClients.Items.RemoveAt( 0 );
      }
      lbxClients.Items.Add( entry );
    }


  }
}
