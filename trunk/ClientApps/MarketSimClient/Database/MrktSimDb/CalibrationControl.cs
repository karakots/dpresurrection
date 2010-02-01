using System;

using MrktSimDb.Metrics;

namespace MrktSimDb
{
	/// <summary>
	/// Summary description for Calibration.
	/// </summary>
    public class CalibrationControl
    {
        public enum CalibrationType
        {
            Standard,
            AutoMatic,
            Parametric,
            Optimization
        };

        /// <summary>
        /// Order determines which is done first
        /// </summary>
        public enum CalibrationSubType
        {
            PriceSensitivity = 0,
            Attribute,
            Media
        };


        const char comma = ',';



        /// <summary>
        /// Create a calibration object from a string
        /// </summary>
        /// <param name="control_string"></param>
        public CalibrationControl( string control_string ) {
            Control = control_string;
        }

        /// <summary>
        /// Create a new calibration object
        /// </summary>
        public CalibrationControl() {
        }

        #region fields

        private CalibrationType type = CalibrationType.Standard;
        private Value metric = null;

        private double stepSize = 0.01;
        private double tolerance = 0.001;
        private int maxIters = 10;
        private bool clearResults = false;
        private bool applyParms = false;
        private bool[] autoCalType = new bool[ Enum.GetNames( typeof( CalibrationSubType ) ).Length ];

        #endregion

        public CalibrationType Type {
            get {
                return type;
            }

            set {
                type = value;
            }
        }

        public Value Metric {
            get {

                return metric;
            }

            set {
                metric = value;
            }
        }

        public double StepSize {
            get {
                return stepSize;
            }

            set {
                stepSize = value;
            }
        }

        public double Tolerance {
            get {
                return tolerance;
            }

            set {
                tolerance = value;
            }
        }

        public int MaxIters {
            get {
                return maxIters;
            }

            set {
                maxIters = value;
            }
        }

        public bool ClearAll {
            get {
                return clearResults;
            }

            set {
                clearResults = value;
            }
        }

        public bool ApplyParameters {
            get {
                return applyParms;
            }

            set {
                applyParms = value;
            }
        }

        public bool Calibrate( CalibrationSubType type ) {
            return autoCalType[ (int)type ];
        }

        public void SetCalibration( CalibrationSubType type, bool inVal ) {
            autoCalType[ (int)type ] = inVal;
        }

        public string Control {
            get {
                string rval = StepSize.ToString();
                rval += "," + Tolerance.ToString();
                rval += "," + MaxIters.ToString();

                if( Metric != null ) {
                    rval += "," + this.Metric.ToString();
                }
                else {
                    rval += ", ";
                }

                rval += "," + this.ClearAll.ToString();
                rval += "," + ((int)Type).ToString();
                rval += "," + this.ApplyParameters.ToString();

                foreach( CalibrationSubType calType in Enum.GetValues( typeof( CalibrationSubType ) ) ) {
                    rval += "," + Calibrate( calType ).ToString();
                }

                return rval;
            }

            set {
                // default
                Type = CalibrationType.Standard;
                Metric = null;
                for( int ii = 0; ii < autoCalType.Length; ++ii ) {
                    autoCalType[ ii ] = false;
                }

                if( value == null || value == "" ) {
                    return;
                }

                string[] values = value.Split( comma );
                int parmIndex = 0;

                // get StepSize
                try {
                    StepSize = double.Parse( values[ parmIndex ] );
                }
                catch( Exception ) {
                    return;
                }
                ++parmIndex;

                // get Tolerance
                try {
                    Tolerance = double.Parse( values[ parmIndex ] );
                }
                catch( Exception ) {
                    return;
                }
                ++parmIndex;

                // get max iters
                try {
                    MaxIters = int.Parse( values[ parmIndex ] );
                }
                catch( Exception ) {
                    return;
                }
                ++parmIndex;

                // get type
                if( parmIndex == values.Length ) {
                    return;
                }

                for( int index = 0; index < MetricMan.MetricValues.Length; ++index ) {
                    if( values[ parmIndex ] == MetricMan.MetricValues[ index ].ToString() ) {
                        Metric = MetricMan.MetricValues[ index ];
                    }
                }
                ++parmIndex;

                if( parmIndex == values.Length ) {
                    return;
                }

                // get clear flag
                try {
                    ClearAll = bool.Parse( values[ parmIndex ] );
                }
                catch( Exception ) {
                    return;
                }
                ++parmIndex;

                if( parmIndex == values.Length ) {
                    return;
                }


                // get the calibration type
                try {
                    Type = (CalibrationType)int.Parse( values[ parmIndex ] );
                }
                catch( Exception ) {
                    return;
                }
                ++parmIndex;

                if( parmIndex == values.Length ) {
                    return;
                }

                // get apply flag
                try {
                    ApplyParameters = bool.Parse( values[ parmIndex ] );
                }
                catch( Exception ) {
                    return;
                }
                ++parmIndex;

                if( parmIndex == values.Length ) {
                    return;
                }

                for( int ii = 0; ii < autoCalType.Length; ++ii ) {
                    try {
                        autoCalType[ ii ] = bool.Parse( values[ parmIndex ] );
                    }
                    catch( Exception ) {
                        return;
                    }

                    ++parmIndex;

                    if( parmIndex == values.Length ) {
                        return;
                    }
                }
            }
        }
    }
}
