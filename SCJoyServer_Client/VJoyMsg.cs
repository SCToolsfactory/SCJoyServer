using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJoyServer_Client
{
  /// <summary>
  /// Create valid Messages
  /// </summary>
  class VJoyMsg
  {

    private string AxisCmdFmt = "{{ \"A\": {{\"Direction\":\"{0}\", \"Value\":{1} }}}}";
    private string RAxisCmdFmt = "{{ \"R\": {{\"Direction\":\"{0}\", \"Value\":{1} }}}}";
    private string SLCmdFmt = "{{ \"S\": {{\"Index\":{0}, \"Value\":{1} }}}}";
    private string POVCmdFmt = "{{ \"P\": {{\"Index\":{0}, \"Direction\":\"{1}\" }}}}";
    private string BtCmdFmt = "{{ \"B\": {{\"Index\":{0}, \"Mode\":\"{1}\" }}}}";

    private string KeyCmdFmt = "{{ \"K\": {{\"VKcode\":\"{0}\", \"Mode\":\"{1}\" ,\"Modifier\":\"{2}\" }}}}";

    private string Press = "p";
    private string Release = "r";
    private string Tap = "t";
    private string DTap = "d";

    private string Up = "u";
    private string Down = "d";
    private string Left = "l";
    private string Right = "r";
    private string Center = "c";

    private List<string> m_msg = new List<string>( );
    private Random m_rnd = new Random( );

    public VJoyMsg()
    {
    m_rnd = new Random( );

    // Axis
    m_msg.Add( string.Format( AxisCmdFmt, "X", 0 ) );
      m_msg.Add( string.Format( AxisCmdFmt, "X", 1000 ) );
      m_msg.Add( string.Format( AxisCmdFmt, "X", 100 ) );
      m_msg.Add( string.Format( AxisCmdFmt, "X", 555 ) );

      m_msg.Add( string.Format( AxisCmdFmt, "Y", 0 ) );
      m_msg.Add( string.Format( AxisCmdFmt, "Y", 1000 ) );
      m_msg.Add( string.Format( AxisCmdFmt, "Y", 200 ) );
      m_msg.Add( string.Format( AxisCmdFmt, "Y", 755 ) );

      m_msg.Add( string.Format( AxisCmdFmt, "Z", 0 ) );
      m_msg.Add( string.Format( AxisCmdFmt, "Z", 1000 ) );
      m_msg.Add( string.Format( AxisCmdFmt, "Z", 800 ) );
      m_msg.Add( string.Format( AxisCmdFmt, "Z", 245 ) );

      // Rot Axis
      m_msg.Add( string.Format( RAxisCmdFmt, "X", 0 ) );
      m_msg.Add( string.Format( RAxisCmdFmt, "X", 1000 ) );
      m_msg.Add( string.Format( RAxisCmdFmt, "X", 100 ) );
      m_msg.Add( string.Format( RAxisCmdFmt, "X", 555 ) );

      m_msg.Add( string.Format( RAxisCmdFmt, "Y", 0 ) );
      m_msg.Add( string.Format( RAxisCmdFmt, "Y", 1000 ) );
      m_msg.Add( string.Format( RAxisCmdFmt, "Y", 200 ) );
      m_msg.Add( string.Format( RAxisCmdFmt, "Y", 755 ) );

      m_msg.Add( string.Format( RAxisCmdFmt, "Z", 0 ) );
      m_msg.Add( string.Format( RAxisCmdFmt, "Z", 1000 ) );
      m_msg.Add( string.Format( RAxisCmdFmt, "Z", 800 ) );
      m_msg.Add( string.Format( RAxisCmdFmt, "Z", 245 ) );

      // Slider
      m_msg.Add( string.Format( SLCmdFmt, 1, 0 ) );
      m_msg.Add( string.Format( SLCmdFmt, 1, 1000 ) );
      m_msg.Add( string.Format( SLCmdFmt, 1, 800 ) );
      m_msg.Add( string.Format( SLCmdFmt, 1, 245 ) );

      m_msg.Add( string.Format( SLCmdFmt, 2, 0 ) );
      m_msg.Add( string.Format( SLCmdFmt, 2, 1000 ) );
      m_msg.Add( string.Format( SLCmdFmt, 2, 800 ) );
      m_msg.Add( string.Format( SLCmdFmt, 2, 245 ) );

      // POV
      m_msg.Add( string.Format( POVCmdFmt, 1, Up ) );
      m_msg.Add( string.Format( POVCmdFmt, 1, Down ) );
      m_msg.Add( string.Format( POVCmdFmt, 1, Left ) );
      m_msg.Add( string.Format( POVCmdFmt, 1, Right ) );
      m_msg.Add( string.Format( POVCmdFmt, 1, Center ) );

      m_msg.Add( string.Format( POVCmdFmt, 2, Up ) );
      m_msg.Add( string.Format( POVCmdFmt, 2, Down ) );
      m_msg.Add( string.Format( POVCmdFmt, 2, Left ) );
      m_msg.Add( string.Format( POVCmdFmt, 2, Right ) );
      m_msg.Add( string.Format( POVCmdFmt, 2, Center ) );

      // Button
      m_msg.Add( string.Format( BtCmdFmt, 1, Press ) );
      m_msg.Add( string.Format( BtCmdFmt, 1, Release ) );
      m_msg.Add( string.Format( BtCmdFmt, 2, Tap ) );
      m_msg.Add( string.Format( BtCmdFmt, 3, DTap ) );

      m_msg.Add( string.Format( BtCmdFmt, 4, Press ) );
      m_msg.Add( string.Format( BtCmdFmt, 4, Release ) );
      m_msg.Add( string.Format( BtCmdFmt, 5, Tap ) );
      m_msg.Add( string.Format( BtCmdFmt, 6, DTap ) );

      m_msg.Add( string.Format( BtCmdFmt, 7, Press ) );
      m_msg.Add( string.Format( BtCmdFmt, 7, Release ) );
      m_msg.Add( string.Format( BtCmdFmt, 8, Tap ) );
      m_msg.Add( string.Format( BtCmdFmt, 9, DTap ) );

      m_msg.Add( string.Format( BtCmdFmt, 10, Press ) );
      m_msg.Add( string.Format( BtCmdFmt, 10, Release ) );
      m_msg.Add( string.Format( BtCmdFmt, 11, Tap ) );
      m_msg.Add( string.Format( BtCmdFmt, 12, DTap ) );

      // Keys
      m_msg.Add( string.Format( KeyCmdFmt, "VK_A", Tap, "" ) );
      m_msg.Add( string.Format( KeyCmdFmt, "VK_B", Tap, "n" ) );
      m_msg.Add( string.Format( KeyCmdFmt, "VK_C", Tap, "ls" ) );
      m_msg.Add( string.Format( KeyCmdFmt, "VK_D", Tap, "ls&rs" ) );
      m_msg.Add( string.Format( KeyCmdFmt, "VK_E", Tap, "la" ) );
      m_msg.Add( string.Format( KeyCmdFmt, "VK_F1", Tap, "n" ) );
      m_msg.Add( string.Format( KeyCmdFmt, "VK_NP_ENTER", Tap, "n" ) );
    }

    private string Amsg(string dir, int val )
    {
      return string.Format( AxisCmdFmt, dir, val );
    }


    public string NextMsg()
    {
      int choice = m_rnd.Next( m_msg.Count );
      return m_msg[choice];
    }

  }
}
