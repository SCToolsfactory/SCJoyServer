using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJoyServer.VJoy
{
  // json:    { content }
  // content:  object | array 
  // value:    array | object | string | number | bool | null
  // array:    [ value <,value> ]
  // object:   { "key": value <, "key": value <, ..>> }


  public class JsonRecord : SortedList<string, JsonContent>
  {

  }

  public class JsonContent : SortedList<string, object>
  {

  }

  // [  value <,value> ]
  public class JsonArray : List<object>
  {
    public JsonArray() { }

    public JsonArray( object[] ary )
    {
      foreach ( var element in ary ) {
        this.Add( element );
      }
    }

    public string AsString()
    {
      string ret = "[";
      foreach ( var element in this ) {
        if ( element == null )
          ret += $"null,";
        else if ( element.GetType( ) == typeof( bool ) )
          ret += $"{element.ToString( ).ToLowerInvariant( )},";
        else if ( element.GetType( ) == typeof( string ) )
          ret += $"\"{(string)element}\",";
        else if ( element.GetType( ).IsValueType )
          ret += $"{element.ToString( )},";
        else if ( element.GetType( ) == typeof( JsonArray ) )
          ret += $"{( (JsonArray)element ).AsString( )},";
        else if ( element.GetType( ) == typeof( JsonObject ) )
          ret += $"{( (JsonObject)element ).AsString( )},";
      }
      ret = ret.Remove( ret.Length ); // cut last comma
      ret += " ]";
      return ret;
    }

  }

  // { Key : Value <, Key : Value> }
  public class JsonObject : Dictionary<string, object>
  {
    public JsonObject() { }

    public JsonObject( Dictionary<string, object> obj )
    {
      foreach ( var kv in obj ) {
        this.Add( kv.Key, kv.Value );
      }
    }

    public JsonObject( string key, object value )
    {
      this.Add( key, value );
    }


    public string AsString()
    {
      string ret = "{";
      foreach ( var kv in this ) {
        ret += $" \"{kv.Key}\" : ";
        var element = kv.Value;

        if ( element == null )
          ret += $"null,";
        else if ( element.GetType( ) == typeof( bool ) )
          ret += $"{element.ToString( ).ToLowerInvariant( )},";
        else if ( element.GetType( ) == typeof( string ) )
          ret += $"\"{(string)element}\",";
        else if ( element.GetType( ).IsValueType )
          ret += $"{element.ToString( )},";
        else if ( element.GetType( ) == typeof( JsonArray ) )
          ret += $"{( (JsonArray)element ).AsString( )},";
        else if ( element.GetType( ) == typeof( JsonObject ) )
          ret += $"{( (JsonObject)element ).AsString( )},";
      }
      ret = ret.Remove( ret.Length ); // cut last comma
      ret += " }";
      return ret;
    }
  }

  /// <summary>
  /// Very limited and targeted Json Handler for
  /// VJoy server only...
  /// </summary>
  public class Json
  {
    public Json() { }

    /// <summary>
    /// Decomp the basic message {key:{ object }}
    /// </summary>
    /// <param name="jString"></param>
    /// <returns></returns>
    public static JsonObject Parse( string jString )
    {
      var ret = DecomposeObject( jString );
      return ret;
    }

    /// <summary>
    /// Split a comma separated line - take care of comas in strings
    /// Note: no whitespace treatment whatsoever
    /// </summary>
    /// <param name="csvLine"></param>
    /// <returns>A list of separated items</returns>
    internal static IList<string> Split( string csvLine, char splitchar = ',' )
    {
      var elements = new List<string>( );
      bool ins = false;
      string cap = "";
      for ( int i = 0; i < csvLine.Length; i++ ) {
        if ( csvLine[i] == '"' ) {
          if ( ins ) {
            // end of string
            ins = false;
          }
          else {
            // start of string
            ins = true;
          }
          cap += csvLine[i]; // collect
        }
        else if ( csvLine[i] == splitchar ) {
          if ( !ins ) {
            // end capture
            elements.Add( cap );
            cap = "";
          }
          else {
            cap += csvLine[i]; // collect
          }
        }
        else {
          // neither , nor "
          cap += csvLine[i]; // collect
        }
      }
      if ( !string.IsNullOrEmpty( cap ) ) {
        elements.Add( cap );
      }
      return elements;
    }

    /// <summary>
    /// Decompose a Json string object - expecting {  "key": {value} }
    /// where value can be string, bool, int, double only
    /// </summary>
    /// <param name="js"></param>
    /// <returns>A Json Object or null</returns>
    public static JsonObject DecomposeObject( string js )
    {
      // straight forward and not really nice - should do it..
      var record = new JsonObject( );
      // do some houskeeping first
      if ( string.IsNullOrWhiteSpace( js ) ) return null; // no usable content
      js = js.Replace( "\n", "" ).Replace( "\r", "" ).Trim( ); // cleanup any CR, LFs and whitespaces
      if ( js.EndsWith( "," ) )
        js = js.Substring( 0, js.Length - 1 ).TrimEnd( ); // cut end comma and clean

      if ( !js.StartsWith( "{" ) ) return null; // no content at all ??
      if ( !js.EndsWith( "}" ) ) return null; // misalinged content not { something }
      js = js.Substring( 1, js.Length - 2 ).Trim( ); // cut {} and trim

      // divide key and content
      int colPos = js.IndexOf( ':' );
      if ( colPos < 3 ) return null; // no key element at all

      string key = RemoveApo( js.Substring( 0, colPos ) );
      js = js.Substring( colPos + 1 ).TrimStart( ); // cut the key part and clean
      // starting brace must be at the beginning now
      if ( !js.StartsWith( "{" ) ) return null; // no content at all ?? key only is disregarded here
      if ( !js.EndsWith( "}" ) ) return null; // misalinged content not { something }
      js = js.Substring( 1, js.Length - 2 ).Trim( );
      // we should be left with 'content, content' here
      IList<string> contList = Split( js, ',' );
      foreach ( var item in contList ) {
        IList<string> itemPairs = Split( item, ':' );
        if ( !record.ContainsKey( key ) ) {
          record.Add( key, new JsonObject( ) );
        }
        // the value can be a string or a number
        if ( itemPairs[1].Trim( ) == "null" ) {
          ( record[key] as JsonObject ).Add( RemoveApo( itemPairs[0] ), null );
        }
        else if ( bool.TryParse( itemPairs[1], out bool bRes ) ) {
          ( record[key] as JsonObject ).Add( RemoveApo( itemPairs[0] ), bRes.ToString().ToLowerInvariant() );
        }
        else if ( int.TryParse( itemPairs[1], out int iRes ) ) {
          ( record[key] as JsonObject ).Add( RemoveApo( itemPairs[0] ), iRes );
        }
        else if ( double.TryParse( itemPairs[1], out double dRes ) ) {
          ( record[key] as JsonObject ).Add( RemoveApo( itemPairs[0] ), dRes );
        }
        else {
          ( record[key] as JsonObject ).Add( RemoveApo( itemPairs[0] ), RemoveApo( itemPairs[1] ) );
        }
      }
      return record;
    }


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


    /// <summary>
    /// Get a Json fragment from the input string   [key:]{ content } 
    /// </summary>
    /// <param name="jsInput"></param>
    /// <returns>The fragment</returns>
    public static string ExtractFragment( string jsInput )
    {
      // do some houskeeping first
      if ( string.IsNullOrWhiteSpace( jsInput ) ) return ""; // no usable content

      int endPos = 0;
      int bOpen = 0; bool triggered = false;
      if ( !jsInput.Contains( "{" ) ) return ""; // seems not having an { item..
      if ( !jsInput.Contains( "}" ) ) return ""; // seems not having an } item..

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
        var fragment = jsInput.Substring( 0, endPos + 1 );
        return fragment;
      }
      return "";
    }

    /// <summary>
    /// Removes the Apostroph encapsulation and returns a plain string from it
    /// </summary>
    /// <param name="apoString">A string enclosed in Apostrophes</param>
    /// <returns>The plain string</returns>
    public static string RemoveApo( string apoString )
    {
      return apoString.Trim( ).Replace( "\"", "" );
    }


  }
}
