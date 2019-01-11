using System;
using System.IO;
using System.Net;
using System.Text;

namespace SCJoyServer.Uploader
{
  /// <summary>
  /// Uploads a file to the designated web server
  /// expecting a PHP script to receive the POST request with the file
  /// </summary>
  class WebUploader : IDisposable
  {
    // Note: this is const and expected to be setup in the WebServer...
    private const string m_uploadString = @"/data/fupload.php"; // this completes the URI path and php
    private WebClient m_client = null;
    private string m_host_port = "";
    private string m_reply = "";

    /// <summary>
    /// cTor: submit the webserver as host:port (localhost:8080  or 192.168.1.10:9000 ..)
    /// </summary>
    /// <param name="host_port">Host and port to use in http request</param>
    public WebUploader(string host_port)
    {
      m_host_port = host_port;
      m_client = new WebClient( );
    }

    /// <summary>
    /// Contains the reply from the upload request
    /// </summary>
    public string Reply { get => m_reply; }

    public void Dispose()
    {
      ( (IDisposable)m_client ).Dispose( );
    }

    /// <summary>
    /// Upload a file to the webserver
    /// Note: this is a synch call expected to be fast
    ///       don't upload MBs here with this facility
    /// </summary>
    /// <param name="filename">A file to upload</param>
    /// <returns>True if successful, else false</returns>
    public bool Upload(string filename )
    {
      // some sanity..
      m_reply = "File does not exist";
      if ( !File.Exists( filename ) ) return false;
      m_reply = "Client not created or vanished ??!!";
      if ( m_client == null ) return false;
      m_reply = "Client is busy - try later";
      if ( m_client.IsBusy ) return false;
      m_reply = "";

      // we shall retry once if the file is still in use and locked
      bool retry = false;
      do {
        try {
          m_client.UseDefaultCredentials = true;
          //m_client.Credentials = CredentialCache.DefaultCredentials;
          byte[] responseArray = m_client.UploadFile( @"http://" + m_host_port + m_uploadString, "POST", filename );
          // gather the reply from php
          m_reply = $"{Encoding.ASCII.GetString( responseArray )}";
          if ( m_reply.ToLowerInvariant( ).Contains( "error" ) ) {
            // as per definiton in fupload.php - Starts with Error:... or PHP itself replies with error
            // don't send a filename that contains 'error' though...
            return false;
          }

          return true;
        }
        catch ( WebException e ) {
          if ( e.InnerException.HResult == -2147024864 ) {
            // catch file in use error once here and retry
            // sometimes the filewatcher is just too fast...
            if ( ! retry ) retry = true; // once only
          }
          else {
            m_reply = e.Message;
            return false;
          }
        }
        catch ( Exception e ) {
          m_reply = e.Message;
          return false;
        }
      } while ( retry );
      return false; 
    }

  }
}
