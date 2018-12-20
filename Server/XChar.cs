using System;

namespace SCJoyServer.Server
{
  public class XChar
  {
    private const int cAT = ' ';
    private const int cXT = '~';

    static public string[] CIM = {"NUL", "SOH", "STX", "ETX", "EOT", "ENQ", "ACK", "BEL", "BS", "HT", "LF", "VT", "FF", "CR", "SO", "SI", 
                                "DLE", "DC1", "DC2", "DC3", "DC4", "NAK", "SYN", "ETB", "CAN", "EM", "SUB", "ESC", "FS", "GS", "RS", "US"};

    /// <summary>
    /// Returns a char - or a displayable image for Ctrl Chars
    /// - also adds a vbCr for CR
    /// </summary>
    /// <param name="c">The in char</param>
    /// <returns>A string to display as char representation</returns>
    /// <remarks></remarks>
    static public string XC( char c )
    {
      string r = "";
      int a = Convert.ToInt32( c );

      if ( a < cAT ) {
        r = "<" + CIM[a] + ">";
        //if ( a == 0x0d ) r += "\r";
      }
      else if ( a > cXT ) {
        r = "<" + a.ToString( "000" ) + ">";
      }
      else {
        r = Convert.ToString( c );
      }

      return r;
    }


    static public string XS( string s )
    {
      string r = "";
      foreach ( char c in s ) r += XC( c );
      return r;
    }

  }
}
