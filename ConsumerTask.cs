using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Collections;

using System.Diagnostics;

namespace SCJoyServer
{
    /// <summary>
    /// Implements an Instrument Simulation 
    /// </summary>
    class ConsumerTask
    {

        private Boolean     m_abortConsumer = false;
        private Thread      m_ConsumerTask  = null;
        private c8kSim      m_c8kSim = null;


        public ConsumerTask( )
        {

            m_ConsumerTask = new Thread( new ThreadStart( this.StartConsuming ) );
            m_c8kSim = new c8kSim( );
        }

        public void Run()
        {
            m_ConsumerTask.Start( );
        }


        // return the simulation obj
        public c8kSim cobas8000
        {
            get { return m_c8kSim; }
        }


        /// <summary>
        /// Sets the abort status for the listener
        /// </summary>
        public void Abort( )
        {
            m_abortConsumer = true; // atomic - no sync needed
        }


        private void StartConsuming( )
        {
            Int32 cycInterval_10 = 18;  // 10th of seconds
            Int32 cycTime = cycInterval_10 * 100;  // msec
            Int32 simTime_10 = 0;

            Debug.Print( "Consuming now ..." );
            while ( !m_abortConsumer ) {
                // Instrument runs ... - consumes reagent and triggers error msgs once in a while
                Thread.Sleep( cycTime );
                simTime_10 += cycInterval_10;
                m_c8kSim.CycleSim( simTime_10 );
            }
        }


    }
}
