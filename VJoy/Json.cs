using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJoyServer.VJoy
{
  /// <summary>
  /// Very limited and targeted Json Handler for
  /// VJoy server only...
  /// </summary>
  public class Json
  {
    // json:    { content }

    /// <summary>
    /// Extracts a top level '{ anything }' element from a Json string
    /// </summary>
    /// <param name="jsInput">The input string</param>
    /// <param name="fragment">out the extracted fragment</param>
    /// <param name="jsRemaining">out the input - the extracted part</param>
    /// <returns></returns>
    public static bool ExtractFragment( string jsInput, out string fragment, out string jsRemaining )
    {
      fragment = ""; jsRemaining = jsInput;
      // do some houskeeping first
      if ( string.IsNullOrWhiteSpace( jsInput ) ) return false; // no usable content

      jsInput = jsInput.Replace( "\n", "" ).Replace( "\r", "" ).TrimStart( ); // cleanup any CR, LFs and whitespaces

      int endPos = 0;
      int bOpen = 0; bool triggered = false;
      if ( jsInput.IndexOf( "{" ) != 0 ) return false; // seems not having a starting { item..

      for ( int i = 0; i < jsInput.Length; i++ ) {
        if ( jsInput[i] == '{' ) {
          bOpen++; triggered = true;
        }
        else if ( triggered && jsInput[i] == '}' ) {
          bOpen--;
        }
        if ( triggered && bOpen == 0 ) {
          endPos = i;
          triggered = false;
          break; // no further reading needed
        }
      }
      if ( endPos > 0 ) {
        // extract
        fragment = jsInput.Substring( 0, endPos + 1 );
        jsRemaining = jsInput.Substring( endPos + 1 );
        return true;
      }
      return false;
    }

  }
}
