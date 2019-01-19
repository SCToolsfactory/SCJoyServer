using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace SCJoyServer.Uploader
{
  /// <summary>
  /// Uploads a file to the designated web server
  /// expecting a PHP script to receive the POST request with the file
  /// </summary>
  class WebUploader : IDisposable
  {
    // Note: this is const and expected to be setup in the WebServer...
    private WebClient m_client = null;
    private string m_host_port = "";
    private string m_reply = "";

    private ClientConnectionPool m_connectionPool;
    private Thread m_clientTask = null;
    private bool m_continueProcess = false;

    public bool WebClientRunning { get; private set; } = false;



    /// <summary>
    /// cTor: submit the webserver as host:port (localhost:8080  or 192.168.1.10:9000 ..)
    /// </summary>
    /// <param name="host_port">Host and port to use in http request</param>
    public WebUploader( )
    {
      m_client = new WebClient( );
      m_connectionPool = new ClientConnectionPool( );
    }

    /// <summary>
    /// Contains the reply from the upload request
    /// </summary>
    public string Reply { get => m_reply; }

    public void Dispose()
    {
      if ( m_continueProcess ) {
        this.StopService( );
      }

      ( (IDisposable)m_client ).Dispose( );
    }



    public void StartService( string host_port )
    {
      m_host_port = host_port;
      m_clientTask = new Thread( new ThreadStart( this.Process ) );
      m_continueProcess = true;
      m_clientTask.Start( );
      if ( m_clientTask.IsAlive ) {
        WebClientRunning = true;
        WebUploaderStatus.Instance.SetSvrStatus( WebUploaderStatus.WCliStatus.Running );
      }
    }

    public void StopService()
    {
      this.Stop( );
    }

    /// <summary>
    /// Shut the ClientService
    /// </summary>
    public void Stop()
    {
      WebClientRunning = false; 
      WebUploaderStatus.Instance.SetSvrStatus( WebUploaderStatus.WCliStatus.Shutdown );
      m_continueProcess = false; // m_clientTask should die now

      // Close all pending client connections in the pool
      WebUploaderStatus.Instance.Debug( $"Shutting down - queue len: {m_connectionPool.Count}\n" );
      while ( m_connectionPool.Count > 0 ) {
        UploadService client = m_connectionPool.Dequeue( );
      }

      WebUploaderStatus.Instance.Debug( $"Shutting down - going idle\n" );
      WebUploaderStatus.Instance.SetSvrStatus( WebUploaderStatus.WCliStatus.Idle );
    }

    /// <summary>
    /// Upload a file to the webserver
    /// Note: this is a async call and will execute through a queue
    /// </summary>
    /// <param name="filename">A file to upload</param>
    /// <returns>True if successfully dispatched, else false</returns>
    public bool Upload( string filename )
    {
      m_reply = "Service already shutdown or not running ??!!";
      if ( !m_continueProcess ) return false;
      // some sanity..
      m_reply = "File does not exist";
      if ( !File.Exists( filename ) ) return false;
      m_reply = "Client not created or vanished ??!!";
      if ( m_client == null ) return false;
      m_reply = "Client is busy - try later";
      if ( m_client.IsBusy ) return false;
      m_reply = "";

      // create a service client that will do the upload
      var client = new UploadService( m_client, filename, m_host_port );
      m_connectionPool.Enqueue( client );
      return true;
    }

    /// <summary>
    /// The client thread environment
    /// Get a client connection from the pool and handles it
    /// </summary>
    private void Process()
    {
      WebUploaderStatus.Instance.Debug( $"WebClient Service started\n" );

      while ( m_continueProcess ) {
        // get a Job and do the upload
        UploadService client = null;
        lock ( m_connectionPool.SyncRoot ) {
          if ( m_connectionPool.Count > 0 ) client = m_connectionPool.Dequeue( );
        }
        if ( client != null ) {
          var ret = client.ProcessData( ); // Provoke client
          if ( !ret ) {
            m_reply = client.Reply;
            WebUploaderStatus.Instance.Debug( $"Upload failed: {m_reply}\n" );
          }
          client = null; // destroy
        }

        Thread.Sleep( 100 ); // dispatches clients every 100ms
      }
    }

  }


  #region Class UploadService

  /// <summary>
  /// Creates a number of services that will handle the clients waiting in the queue
  /// </summary>
  class UploadService
  {
    private const string m_uploadString = @"/data/fupload.php"; // this completes the URI path and php

    private WebClient m_client = null;
    private string m_filename = "";
    private string m_host_port = "";

    private string m_reply = "";
    /// <summary>
    /// Contains the reply from the upload request
    /// </summary>
    public string Reply { get => m_reply; }

    public UploadService( WebClient webClient, string filename, string hostport )
    {
      m_client = webClient;
      m_filename = filename;
      m_host_port = hostport;
    }

    public bool ProcessData()
    {
      // we shall retry up to 5 times if the file is still in use or locked
      int retry = 5;
      do {
        try {
          m_client.UseDefaultCredentials = true;
          //m_client.Credentials = CredentialCache.DefaultCredentials;
          byte[] responseArray = m_client.UploadFile( @"http://" + m_host_port + m_uploadString, "POST", m_filename );
          // gather the reply from php
          m_reply = $"{Encoding.ASCII.GetString( responseArray )}";
          if ( m_reply.ToLowerInvariant( ).Contains( "error" ) ) {
            // as per definiton in fupload.php - Starts with "Error:..." or PHP itself replies with error
            // don't send a filename that contains 'error' though...
            return false;
          }
          WebUploaderStatus.Instance.SetPing( );
          return true;
        }
        catch ( WebException e ) {
          // ERROR_SHARING_VIOLATION - 0x80070020 (-2147024864) - file in use
          // ERROR_LOCK_VIOLATION    - 0x80070021 (-2147024863) - file locked
          if ( ( e.InnerException.HResult == -2147024864 )
            || ( e.InnerException.HResult == -2147024863 ) ) {
            // catch file in use error once here and retry
            // sometimes the filewatcher is just too fast...
            retry--; // again..
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
      } while ( retry > 0 );
      return false;
    }
  }
  #endregion

  #region Class ClientConnectionPool

  class ClientConnectionPool
  {
    // Creates a synchronized wrapper around the Queue.
    private Queue SyncdQ = Queue.Synchronized( new Queue( ) );

    public void Enqueue( UploadService client )
    {
      SyncdQ.Enqueue( client );
    }

    public UploadService Dequeue()
    {
      return (UploadService)( SyncdQ.Dequeue( ) );
    }

    public int Count
    {
      get { return SyncdQ.Count; }
    }

    public object SyncRoot
    {
      get { return SyncdQ.SyncRoot; }
    }

  } // class ClientConnectionPool

  #endregion

}
