using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimInterface;

namespace SimFrame
{
    /// <summary>
    /// Simulations should implement this class
    /// </summary>
    public abstract class Simulation
    {
        public class SimError
        {
            static public string NoError = null;
            static public string NullData = "Null Data";
            static public string BadInputData = "Input error";

            public static implicit operator bool( SimError e )
            {
                return e.Message != null;
            }

            static public implicit operator SimError( string val )
            {
                return new SimError(val);
            }

            SimError()
            {
            }

            SimError( string val )
            {
                Message = val;
            }

            public string Message = null;
        }

        #region Abstract methods
        
        /// <summary>
        /// called before start of a new sim run
        /// </summary>
        /// <returns></returns>
        protected abstract SimError Reset();

        /// <summary>
        /// Called once for each sim step
        /// </summary>
        /// <returns></returns>
        protected abstract SimError Step();

        /// <summary>
        /// Perform any computation for output
        /// </summary>
        /// <returns></returns>
        protected abstract SimError Compute();

        public abstract byte[] Data
        {
            get;
        }

        #endregion

        #region protected data

        // defines the sim
        protected SimInput Input;

        // where to store results
        protected SimOutput Output = new SimOutput();

        #endregion

        #region public control

        
        
        /// <summary>
        /// Run Simulation for day
        /// </summary>
        /// <returns></returns>
        public SimError SimStep() {
            errorCode = Step();

            day++;

            return errorCode;
        }

        #endregion

        #region public properties

        public string Option
        {
            get
            {
                if( Input != null )
                {
                    return Input.option;
                }

                return null;
            }
        }

        public bool Error {
            get {
                return errorCode;
            }
        }

        public SimInput SimIn {
            set {

                // clear metrics
                Output = new SimOutput();

                // set day to start
                day = 0;

                // set input
                Input = value;

                if( Input != null ) {

                    if( Reset()) {
                        errorCode = SimError.BadInputData;

                    }
                }
                else {
                    errorCode = SimError.NullData;
                }
            }
        }

        public SimOutput SimOut {
            get {
                errorCode = Compute();

                return Output;
            }
        }

        public int Day {
            get {
                return (int) day;
            }
        }

        /// <summary>
        /// Progress is between 0 and 1
        /// </summary>
        public double Progress {
            get {

                double rval = 0;

                if( day > 0 && Input.EndDate > 0 ) {
                    return ((double) day) / (double) (Input.EndDate);
                }

                return Math.Min( rval, 1 );
            }
        }

        public bool Done {
            get {
                if( Input == null) {
                    return true;
                }

                return day > Input.EndDate;
            }
        }

        #endregion

        #region private data

        // the current day
        private uint day = 0;

        private SimError errorCode = SimError.NoError;

        #endregion

    }
}
