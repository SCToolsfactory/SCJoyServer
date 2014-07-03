using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCJoyServer
{
  public class XChar
  {
    private const int cAT = ' ';
    private const int cXT = '~';

    static public String[] CIM = {"NUL", "SOH", "STX", "ETX", "EOT", "ENQ", "ACK", "BEL", "BS", "HT", "LF", "VT", "FF", "CR", "SO", "SI", 
                                "DLE", "DC1", "DC2", "DC3", "DC4", "NAK", "SYN", "ETB", "CAN", "EM", "SUB", "ESC", "FS", "GS", "RS", "US"};

    /// <summary>
    /// Returns a char - or a displayable image for Ctrl Chars
    /// - also adds a vbCr for CR
    /// </summary>
    /// <param name="c">The in char</param>
    /// <returns>A string to display as char representation</returns>
    /// <remarks></remarks>
    static public String XC( Char c )
    {
      String r = "";
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


    static public String XS( String s )
    {
      String r = "";
      foreach ( Char c in s ) r += XC( c );
      return r;
    }

  }
}
