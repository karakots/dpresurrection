using System;
using System.Collections;
using System.Data;

using MrktSimDb;
using MrktSimDb.Metrics;
using MathUtility;

using System.Collections.Generic;

namespace SimControlMethods
{
	/// <summary>
	/// Summary description for Callibrate.
	/// </summary>
	public class Calibrate
	{
        // evaluators
        public delegate int SimpleEvaluator(out bool cancel);
        public delegate double[] VectorEvaluator(Value metricType, out bool cancel);
        public delegate double MetricEvaluator(Value metric, out bool cancel);
        public delegate int CalibrationMetricEvaluator(Value metric, out bool cancel, out double value);
        
      

		protected Value metric = null;

		// control variables
        protected double stepSize;
        protected int maxIters;
        protected double tolerance;

        protected bool clearAllResults = false;
        public bool ClearAll
        {
            get
            {
                return clearAllResults;
            }
        }

        private bool apply = false;
        public bool ApplyParameters
        {
            get
            {
                return apply;
            }
        }

        public Calibrate(CalibrationControl control)
		{
			stepSize = control.StepSize;
			tolerance = control.Tolerance;
			maxIters = control.MaxIters;

			metric = control.Metric;

            clearAllResults = control.ClearAll;
            apply = control.ApplyParameters;
		}

        public virtual bool Run() { return false; }

        /// <summary>
        /// Determine if calibration is going south by monitoring if a selected metric
        /// </summary>
        /// <param name="history"></param>
        /// <returns> false if calibration should halt</returns>
        public bool HistoryCheck( List<double> history ) {

            bool rval = true;

            const int numToCheck = 6;
          
            double scale = history.Count - numToCheck;

            if( scale <= float.Epsilon ) {
                return true;
            }

            double ave = 0;
            double numToCheckAverage = 0;

            for( int ii = 0; ii < history.Count - numToCheck; ++ii ) {
                ave += history[ ii ]/scale;
            }

            for( int ii = history.Count - numToCheck; ii < history.Count; ++ii ) {
                numToCheckAverage += history[ ii ] / numToCheck;
            }

            if( numToCheckAverage > ave ) {
               
                // this looks bad
                rval = false;

                // maybe the last one is going in the right direction ?
                if( history[ history.Count - 1 ] < history[ history.Count - 2 ] ) {
                    // hope springs eternal
                    rval = true;
                }  
            }

            return rval;
        }
	}

    public class AutoCalibration : Calibrate
    {

        private CallibrationDb theDb = null;
        public CallibrationDb Db {
            set {
                theDb = value;
            }

            get {
                return theDb;
            }
        }

        public event CalibrationMetricEvaluator Evaluate;
        CalibrationControl calControl = null;

        public AutoCalibration( CalibrationControl control ) : base( control ) {

            calControl = control;
        }

        protected List<double> optParms = new List<double>();

        public override bool Run() {
            bool cancel = false;
            bool done = false;

            int numIters = 0;

            List<double> history = new List<double>();

            // perform baseline simulation
            double curVal = 0; ;
            int currentRunId = Evaluate( metric, out cancel, out curVal );

            double currentMin = curVal;

            updateParameters( false );

            while( !done ) {
                done = true;

                foreach( CalibrationControl.CalibrationSubType calType in Enum.GetValues( typeof( CalibrationControl.CalibrationSubType ) ) ) {

                    if( !calControl.Calibrate( calType ) ) {
                        continue;
                    }

                    if( solve( calType, currentRunId ) ) {
                        // for the moment we are not done
                        done = false;
                        
                        // update database
                        theDb.Update();


                        // we changed the simulation rerun a baseline
                        currentRunId = Evaluate( metric, out cancel, out curVal );

                         if( cancel ) {
                            return false;
                        }

                        history.Add( curVal );

                        if( curVal < currentMin ) {
                            updateParameters( false );

                            currentMin = curVal;
                        }

                        // we are small enough
                        if( currentMin < tolerance ) {
                            done = true;
                            break;
                        }


                        // check history
                        if( !HistoryCheck( history ) ) {
                            done = true;
                            break;
                        }

                        // check if number of iterations is at max
                        ++numIters;

                        if( numIters > maxIters ) {
                            done = true;
                            break;
                        }
                    }
                }
            }

            updateParameters( true );
            theDb.Update();

            if( ApplyParameters ) {
                foreach( MrktSimDBSchema.scenario_parameterRow parmRow in Db.Data.scenario_parameter ) {
                    Db.ApplySimulationlParameter( parmRow );
                }
            }


            return true;
        }

        private void updateParameters( bool applyToModel ) {
            if( !applyToModel ) {
                optParms.Clear();
            }

            int index = 0;
            foreach( MrktSimDBSchema.scenario_parameterRow simParm in theDb.Data.scenario_parameter.Select() ) {
                if( applyToModel ) {

                    if( index >= optParms.Count ) {

                    }
                    else {
                        simParm.aValue = optParms[ index ];
                    }
                }
                else {
                    // store away for future reference
                    this.optParms.Add( simParm.aValue );
                }

                index++;
            }
        }

        private bool solve(CalibrationControl.CalibrationSubType calType, int run_id ) {

            switch( calType ) {
                case CalibrationControl.CalibrationSubType.PriceSensitivity:
                    return SolvePrice( run_id );

                case CalibrationControl.CalibrationSubType.Attribute:
                    return SolveAttribute( run_id );

                case CalibrationControl.CalibrationSubType.Media:
                    return SolveMedia( run_id );
            }

            return false;
        }

        private List<int> attrsAllowed = null;
        public bool SolveAttribute(int run_id ) {

            List<List<double>> totalStep = new List<List<double>>();
            List<List<List<double>>> sim_share = null;
            List<List<double>> real_share = null;
            List<List<double>> weights = null;
            List<List<List<double>>> attributes = new List<List<List<double>>>();


            theDb.GetLeafShareData( run_id, out sim_share, out real_share, out weights );




            if( attrsAllowed == null ) {

                attrsAllowed = new List<int>();

                foreach( MrktSimDBSchema.product_attributeRow attr in
                    theDb.Data.product_attribute.Select(
                    "",
                    "product_attribute_id",
                    DataViewRowState.CurrentRows ) ) {

                    bool allow = false;

                    foreach( MrktSimDBSchema.segmentRow seg in
                        theDb.Data.segment.Select(
                        "segment_id <> " + Database.AllID,
                        "segment_id",
                        DataViewRowState.CurrentRows ) ) {

                        foreach( MrktSimDBSchema.consumer_preferenceRow dbPref in
                                theDb.Data.consumer_preference.Select(
                                "segment_id = " + seg.segment_id + " AND product_attribute_id = " + attr.product_attribute_id,
                                "",
                                DataViewRowState.CurrentRows ) ) {

                            MrktSimDBSchema.scenario_parameterRow simParm = theDb.GetParm( theDb.Id,
                                "consumer_preference", "pre_preference_value", "record_id", dbPref.record_id );

                            if( simParm != null ) {
                                allow = true;
                                break;
                            }
                        }

                        if( allow ) {
                            break;
                        }
                    }

                    if( allow ) {
                        attrsAllowed.Add( attr.product_attribute_id );
                    }

                }
            }

            theDb.ProdAttrVals( run_id, attrsAllowed, out attributes );

            // we iterate over each segment seperately
          
            int segDex = 0;

            foreach( MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select( "segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows ) ) {

                totalStep.Add( Solver.Solve( sim_share[ segDex ], real_share, weights[ segDex ], attributes ) );
                segDex++;
            }

            // loop over segments

            segDex = 0;

            double step = 0;
            foreach( List<double> steps in totalStep ) {
                foreach( double val in steps ) {
                    step += val * val;
                }
            }

            if( Math.Sqrt( step ) < tolerance ) {
                return false;
            }

            foreach( MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select( "segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows ) ) {
                foreach( MrktSimDBSchema.consumer_preferenceRow dbPref in
                        theDb.Data.consumer_preference.Select( "segment_id = " + seg.segment_id, "product_attribute_id", DataViewRowState.CurrentRows ) ) {


                   int index = attrsAllowed.IndexOf( dbPref.product_attributeRow.product_attribute_id );

                   if( index < 0 ) {
                       continue;
                   }

                    double aVal = totalStep[ segDex ][ index ];
                    theDb.UpdateParm("consumer_preference", "pre_preference_value", "record_id", dbPref.record_id, aVal );

                    // also update post use preference
                    theDb.UpdateParm("consumer_preference", "post_preference_value", "record_id", dbPref.record_id, aVal );
                }
                segDex++;
            }

            return true;
        }

        public bool SolvePrice( int run_id ) {
            //bool rval = false;

            List<List<List<double>>> sim_share = null; //new List<List<List<double>>>();
            List<List<double>> real_share = null;
            List<List<double>> weights = null;
            List<List<List<double>>> prices = null;

            List<List<double>> totalStep = new List<List<double>>();

            theDb.GetLeafShareData( run_id, out sim_share, out real_share, out weights );
            theDb.RelPriceVals( run_id, out prices );

            // we iterate over each segment seperately
            int segDex = 0;
            foreach (MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows))
            {

                totalStep.Add(Solver.Solve(sim_share[segDex], real_share, weights[segDex], prices));
                segDex++;
            }

            double step = 0;
            foreach( List<double> steps in totalStep ) {
                foreach( double val in steps ) {
                    step += val * val;
                }
            }

            if( Math.Sqrt( step ) < tolerance ) {
                return false;
            }

            // loop over segments
            segDex = 0;
            foreach (MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows))
            {
                double aVal = totalStep[segDex][0];
                theDb.UpdateParm( "segment", "price_disutility", "segment_id", seg.segment_id,  aVal );

                segDex++;
            }
    
            return true;
        }

        private List<int> plans = null;
        public bool SolveMedia( int run_id ) {

            const double eps = 0.00001;

            // bool rval = false;

            List<List<double>> sim_share = null;
            List<List<double>> real_share = null;
            List<double> weights = null;
       
            List<DateTime> dates = null;
            SortedDictionary<int, List<double>> prodPersuasion = null;
            List<List<double>> totalVals = null;

            SortedDictionary<int, SortedDictionary<int, List<double>>> mediaPersuasion = null;
            List<List<List<double>>> attributes = null;

            // find plans that we have parameters for
            if( plans == null ) {
                plans = new List<int>();

                foreach( MrktSimDBSchema.scenario_parameterRow simParm in theDb.Data.scenario_parameter.Select() ) {

                    // check if this is a media parameter
                    MrktSimDBSchema.model_parameterRow modelParm = simParm.model_parameterRow;

                    if( modelParm.table_name == "market_plan" && modelParm.col_name == "parm2" ) {

                        Database.PlanType planType;

                        theDb.GetMarketPlanInfo(theDb.Simulation.scenario_id, modelParm.row_id, out  planType );

                        if( planType == Database.PlanType.Media ) {
                            plans.Add( modelParm.row_id );
                        }
                    }
                }
            }

            theDb.MediaEGRP( run_id, plans, out mediaPersuasion, out prodPersuasion, out dates );

            List<int> leafs = null;
            theDb.GetMediaData( run_id, mediaPersuasion, prodPersuasion,  out sim_share, out real_share, out weights, out attributes, out totalVals, out leafs );
           

            Solver.Saturation fcn = theDb.InitSaturationFcn();

            List<double> totalStep = null;
            if( fcn != null ) {
                totalStep = Solver.SolveWithSaturation( fcn, sim_share, real_share, weights, totalVals, attributes );
            }
            else {
                totalStep = Solver.Solve( sim_share, real_share, weights, attributes );
            }

            double step = 0;
            foreach( double val in totalStep ) {
                step += val * val;
            }

            if( Math.Sqrt( step ) < tolerance ) {
                return false;
            }
  
            int mediaIndex = 0;
            foreach( int prodId in mediaPersuasion.Keys )
            {
                MrktSimDBSchema.productRow prod = theDb.Data.product.FindByproduct_id( prodId );
                SortedDictionary<int, List<double>> prodPlans = mediaPersuasion[prodId];

                foreach( int mediaID in prodPlans.Keys )
                {
                    double aVal = totalStep[mediaIndex];

                     MrktSimDBSchema.scenario_parameterRow simParm = theDb.GetParm( theDb.Id, "market_plan", "parm2", "id", mediaID );

                     if( simParm != null )
                     {
                         if( simParm.aValue > 0 )
                         {
                             if( aVal < 0 && aVal < eps - simParm.aValue )
                            {
                                totalStep[mediaIndex] = eps - simParm.aValue;
                            }
                         }

                     }

                    mediaIndex++;
                }
            }

            mediaIndex = 0;
            foreach( int prodId in mediaPersuasion.Keys ) {
                MrktSimDBSchema.productRow prod = theDb.Data.product.FindByproduct_id( prodId );
                SortedDictionary<int, List<double>> prodPlans = mediaPersuasion[ prodId ];

                foreach( int mediaID in prodPlans.Keys ) {
                    double aVal = totalStep[mediaIndex];

                    theDb.UpdateParm("market_plan", "parm2", "id", mediaID, aVal );

                    mediaIndex++;
                }
            }

            return true;
        }

    }

    //public class AttributeCalibration : Calibrate
    //{

    //    private CallibrationDb theDb = null;
    //    public CallibrationDb Db
    //    {
    //        set
    //        {
    //            theDb = value;
    //        }

    //        get
    //        {
    //            return theDb;
    //        }
    //    }

    //    public event CalibrationMetricEvaluator Evaluate;

    //    public AttributeCalibration(CalibrationControl control) : base(control) { }

    //    protected ArrayList optParms = new ArrayList();

    //    public override bool Run()
    //    {
    //        bool cancel = false;
    //        bool done = false;

    //        int numIters = 0;

    //        double currentMin = 1000000000;

    //        List<double> history = new List<double>();
         
    //        while (!done)
    //        {
    //            done = true;

    //            double curVal;
    //            int currentRunId = Evaluate(metric, out cancel, out curVal);

    //            if( cancel )
    //                return false;


    //            history.Add( curVal );

    //            if (curVal < currentMin)
    //            {
    //                updateParameters(false);

    //                currentMin = curVal;
    //            }

    //            // check history
    //            if( !HistoryCheck( history ) ) {
    //                break;
    //            }

    //            // we are small enough
    //            if( currentMin < tolerance ) {
    //                break;
    //            }

    //            if (solve(currentRunId))
    //            {
    //                done = false;
    //            }

    //            // update database
    //            theDb.Update();

    //            ++numIters;

    //            if (numIters > maxIters)
    //                done = true;
    //        }

    //        updateParameters(true);
    //        theDb.Update();

    //        if (ApplyParameters)
    //        {
    //            foreach (MrktSimDBSchema.scenario_parameterRow parmRow in Db.Data.scenario_parameter)
    //            {
    //                Db.ApplySimulationlParameter(parmRow);
    //            }
    //        }
          

    //        return true;
    //    }

    //    virtual protected void  updateParameters(bool applyToModel)
    //    {
    //        if (!applyToModel)
    //        {
    //            optParms.Clear();
    //        }

    //        int index = 0;
    //        foreach (MrktSimDBSchema.consumer_preferenceRow dbPref in theDb.Data.consumer_preference.Select())
    //        {
    //            MrktSimDBSchema.model_parameterRow preParm = theDb.ModelParameterExists(dbPref, "pre_preference_value", "record_id");

    //            if (preParm != null)
    //            {
    //                // now check if simulation parameter  exists
    //                string query = "param_id = " + preParm.id;

    //                DataRow[] parmRows = theDb.Data.scenario_parameter.Select(query);

    //                foreach (DataRow row in parmRows)
    //                {
    //                    MrktSimDBSchema.scenario_parameterRow parmRow = (MrktSimDBSchema.scenario_parameterRow)row;

    //                    if (applyToModel)
    //                    {
    //                        parmRow.aValue = (double)optParms[index];
    //                    }
    //                    else
    //                    {
    //                        // store away for future reference
    //                        this.optParms.Add(parmRow.aValue);
    //                    }

    //                    index++;
    //                }
    //            }

    //            MrktSimDBSchema.model_parameterRow postParm = theDb.ModelParameterExists(dbPref, "post_preference_value", "record_id");

    //            if (postParm != null)
    //            {
    //                // now check if simulation parameter  exists
    //                string query = "param_id = " + postParm.id;

    //                DataRow[] parmRows = theDb.Data.scenario_parameter.Select(query);

    //                foreach (DataRow row in parmRows)
    //                {
    //                    MrktSimDBSchema.scenario_parameterRow parmRow = (MrktSimDBSchema.scenario_parameterRow)row;

    //                    if (applyToModel)
    //                    {
    //                        parmRow.aValue = (double)optParms[index];
    //                    }
    //                    else
    //                    {
    //                        // store away for future reference
    //                        this.optParms.Add(parmRow.aValue);
    //                    }

    //                    index++;
    //                }
    //            }
    //        }
    //    }

    //    virtual protected bool solve(int run_id)
    //    {
    //        bool rval = false;

    //        //ArrayList attrIds = null;
    //        //ArrayList attrNames = null;
    //        double[][] diff = null;
    //        double[][][] sigma = null;


    //        // get stats from database
    //        // compute
    //        Newton solver = new Newton();
    //        double[] step = null;

    //        theDb.AttributeStats(run_id, out diff, out sigma);

    //        // change any diff values less then tolerance to zero

    //        for (int jj = 0; jj < diff.Length; ++jj)
    //        {
    //            for (int ii = 0; ii < diff[jj].Length; ++ii)
    //            {
    //                if (Math.Abs(diff[jj][ii]) < tolerance)
    //                {
    //                    diff[jj][ii] = 0.0;
    //                }
    //            }
    //        }

    //        // loop over segments
    //        int segDex = 0;
    //        foreach (MrktSimDBSchema.segmentRow seg in theDb.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows))
    //        {
    //            solver.Solve(diff[segDex], sigma[segDex], 0.0001, this.stepSize, out step);

    //            // loop over attributes
    //            int attrDex = 0;
    //            foreach (MrktSimDBSchema.product_attributeRow attr in theDb.Data.product_attribute.Select("", "product_attribute_id", DataViewRowState.CurrentRows))
    //           {
    //               if (Math.Abs(step[attrDex]) > 0)
    //               {
    //                   rval = true;

    //                   string prefQuery = "segment_id = " + seg.segment_id + " AND product_attribute_id = " + attr.product_attribute_id;

    //                   foreach (MrktSimDBSchema.consumer_preferenceRow dbPref in theDb.Data.consumer_preference.Select(prefQuery))
    //                   {
    //                       MrktSimDBSchema.model_parameterRow preParm = theDb.ModelParameterExists(dbPref, "pre_preference_value", "record_id");

    //                       if (preParm != null)
    //                       {

    //                           // now check if simulation parameter  exists
    //                           string query = "param_id = " + preParm.id;

    //                           DataRow[] parmRows = theDb.Data.scenario_parameter.Select(query);

    //                           foreach (DataRow row in parmRows)
    //                           {
    //                               MrktSimDBSchema.scenario_parameterRow parmRow = (MrktSimDBSchema.scenario_parameterRow)row;
    //                               parmRow.aValue += step[attrDex];
    //                           }
    //                       }

    //                       MrktSimDBSchema.model_parameterRow postParm = theDb.ModelParameterExists(dbPref, "post_preference_value", "record_id");

    //                       if (postParm != null)
    //                       {

    //                           // now check if simulation parameter  exists
    //                           string query = "param_id = " + postParm.id;

    //                           DataRow[] parmRows = theDb.Data.scenario_parameter.Select(query);

    //                           foreach (DataRow row in parmRows)
    //                           {
    //                               MrktSimDBSchema.scenario_parameterRow parmRow = (MrktSimDBSchema.scenario_parameterRow)row;
    //                               parmRow.aValue += step[attrDex];
    //                           }
    //                       }
    //                   }
    //               }

    //                attrDex++;
    //            }
    //            segDex++;
    //        }

    //        return rval;
    //    }
    //}

    //public class PriceSensitivityCalibration : AttributeCalibration
    //{
    //    public PriceSensitivityCalibration(CalibrationControl control) : base(control) { }

    //    protected override bool solve(int run_id)
    //    {
    //        bool rval = false;

    //        double[] diff = null;
    //        double[] sigma = null;

    //        Db.PriceSensitivity(run_id, out diff, out sigma);


    //        // loop over segments
    //        int segDex = 0;
    //        foreach (MrktSimDBSchema.segmentRow seg in Db.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows))
    //        {

    //            double step = 0.0;

    //            if ((diff[segDex] >= tolerance) &&  // diff is large enough
    //            Math.Abs(diff[segDex]) < Math.Abs(stepSize * sigma[segDex]))  // do not step to infinity and beyond
    //            {
    //                step = diff[segDex] / sigma[segDex];
    //            }


    //            MrktSimDBSchema.model_parameterRow segParm = Db.ModelParameterExists(seg, "price_disutility", "segment_id");

    //            if (step > 0 && segParm != null)
    //            {

    //                // now check if simulation parameter  exists
    //                string query = "param_id = " + segParm.id;

    //                DataRow[] parmRows = Db.Data.scenario_parameter.Select(query);

    //                foreach (DataRow row in parmRows)
    //                {
    //                    MrktSimDBSchema.scenario_parameterRow parmRow = (MrktSimDBSchema.scenario_parameterRow)row;
    //                    parmRow.aValue += step;
    //                }

    //                rval = true;
    //            }


    //            segDex++;
    //        }

    //        return rval;

    //    }

    //    protected override void updateParameters(bool applyToModel)
    //    {
    //        if (!applyToModel)
    //        {
    //            optParms.Clear();
    //        }

    //        int segDex = 0;
    //        foreach (MrktSimDBSchema.segmentRow seg in Db.Data.segment.Select("segment_id <> " + Database.AllID, "segment_id", DataViewRowState.CurrentRows))
    //        {
    //            MrktSimDBSchema.model_parameterRow segParm = Db.ModelParameterExists(seg, "price_disutility", "segment_id");

    //            if (segParm != null)
    //            {
    //                // now check if simulation parameter  exists
    //                string query = "param_id = " + segParm.id;

    //                DataRow[] parmRows = Db.Data.scenario_parameter.Select(query);

    //                foreach (DataRow row in parmRows)
    //                {
    //                    MrktSimDBSchema.scenario_parameterRow parmRow = (MrktSimDBSchema.scenario_parameterRow)row;

    //                    if (applyToModel)
    //                    {
    //                        parmRow.aValue = (double)optParms[segDex];
    //                    }
    //                    else
    //                    {
    //                        // store away for future reference
    //                        this.optParms.Add(parmRow.aValue);
    //                    }

    //                    segDex++;
    //                }
    //            }
    //        }
    //    }
    //}
       
    
    public class GeneralCalibration : Calibrate
    {
        public event VectorEvaluator VectorEvaluate;

        // when we step the percent of interval we step by
        // this number should be less then 0.25 otherwise we may step over a minimum point

        double[] var = null;

        public double[] VariableValue
        {
            get
            {
                return var;
            }
        }

        public GeneralCalibration(CalibrationControl control, int numVars)
            : base(control)
		{
            if (numVars > 0)
            {
                var = new double[numVars];


                // start at one
                for (int ii = 0; ii < numVars; ++ii)
                    var[ii] = 1.0;
            }
		}

        /// <summary>
        /// perform calibration
        /// </summary>
        public override bool Run()
        {
            bool done = false;

            List<double> history = new List<double>();

            int numIters = 0;

            while (!done)
            {
                bool cancel = false;

                double[] excessShare = VectorEvaluate(metric, out cancel);

              

                if (cancel)
                    return false;

                // compute rms error

                double rmsError = 0;
                double maxError = 0.0;
                for( int ii = 0; ii < excessShare.Length; ++ii ) {
                    if (Math.Abs(excessShare[ ii ]) > maxError) 
                    {
                        maxError = Math.Abs(excessShare[ ii ]);
                    }

                    rmsError += excessShare[ ii ] * excessShare[ ii ];
                }

                 if (maxError < tolerance)
                {
                    done = true;
                    break;
                }

                history.Add( rmsError );

                if( !HistoryCheck( history ) ) {
                    break;
                }

                double scale = 0.1 * stepSize;
                if( Math.Abs( scale * maxError ) > 0.9 ) {
                    if( stepSize > 0 ) {
                        scale = 0.9 / maxError;
                    }
                    else {
                        scale = -0.9 / maxError;
                    }
                }


                done = true;
                for (int ii = 0; ii < excessShare.Length; ++ii)
                {
                    if (Math.Abs(excessShare[ii]) < tolerance)
                        continue;

                    done = false;

                    // Note: stepSize may be negative if share is negatively correlated with vals
                    var[ii] = var[ii] * (1 - scale * excessShare[ii]);
                }

                ++numIters;

                if (numIters > maxIters)
                    done = true;
            }

            return true;
        }
    }
}
