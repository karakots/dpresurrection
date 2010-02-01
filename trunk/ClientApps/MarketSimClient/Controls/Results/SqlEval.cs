using System;
using System.Data;
using System.Collections;
using Common.Utilities;
using EquationParser;
using MrktSimDb;

using MarketSimUtilities;
using Utilities.Graphing;

namespace Results
{
	public enum DataType
	{
		SimResults,
		InputData,
        PanelData,
		externalData
	}

	public class Utility
	{
		static public double Convert2Double(Object obj)
		{
			if (obj.GetType() == typeof(double))
				return (double) obj;
		
			else if (obj.GetType() == typeof(int))
			{
				int ival = (int) obj;

				return (double) ival;
			}
			
			double val = double.Parse(obj.ToString());
			return val;	

		}

        static public DateTime Convert2DateTime( Object obj ) {
            if( obj.GetType() == typeof( DateTime ) )
                return (DateTime)obj;

            DateTime val;
            try {
                val = DateTime.Parse( obj.ToString() );
            }
            catch( Exception ) {
                val = DateTime.MinValue;
            }


            return val;
        }

	}

    [SerializableAttribute]
	abstract public class EvalData : Variable
	{
		// this is the mnemonic used to reference this value

        public bool hidden = false;

		public string units;

        public HowToSum timeAverage = HowToSum.Sum;

		// determines in which UI list it appears in
		public DataType type;


		public EvalData()
		{
			token = null;

			// which list this belongs in
			type = DataType.SimResults;

			//what units to display
			units = "#";
		}

        public string token;
		public override string Token
		{
			get
			{
                return token;
			}
		}

		public override string ToString()
		{
			return MrktSimControl.MrktSimColumnName("results_std", Token);
		}
	}

	// different kinds of eval data 


    [SerializableAttribute]
    /// <summary>
	/// DbTableEvalData  directly evaluates from a database table
	/// It makes no assumptions on structure
	/// </summary>
	public class DbTableEvalData : EvalData
	{
		// private string tableName = "";

		// column to use in query
		protected string colName = "";

		// tell us how we sum
		public string howToSum = "SUM";

        public bool useSegmentWeighting = false;
        public bool aggreagateOverProducts = false;
        public bool useOneChannel = true;

		// when we set the column we will
		// by default set the token to the same name
		public string Column
		{
			set
			{
				colName = value;
				token = value;
			}
		}

		public override bool IsSimple
		{
			get
			{
				return true;
			}
		}

		public override string Equation
		{
			get
			{
				return null;
			}
		}

        virtual public string Query( int run, int product_id, int segment_id, int channel_id, DateTime start, DateTime end ) { return null; }


		public DbTableEvalData() 
		{
		}
	}

    /// <summary>
    /// Used to pull stats out of the model
    /// </summary>
    public class NumConsumers : DbTableEvalData
    {
        public NumConsumers()
            : base() {
        }

        public override string Query( int run, int product_id, int segment_id, int channel_id, DateTime start, DateTime end ) {
            // how many consumers

            if( segment_id == Database.AllID ) {
                return "select '1/1/2000' as calendar_date, SUM(Model_info.pop_size * segment.segment_size/100.0) as X FROM " +
                "  segment, Model_info, sim_queue " +
                " WHERE model_info.model_id = sim_queue.model_id " +
                " AND segment.model_id = model_info.model_id " +
                " AND sim_queue.run_id = " + run;
            }

            return "select  '1/1/2000' as calendar_date, SUM(Model_info.pop_size * segment.segment_size/100.0) as X FROM " +
                 " segment, Model_info, sim_queue " +
                 " WHERE model_info.model_id = sim_queue.model_id " +
                 " AND segment.model_id = model_info.model_id " +
                 " AND sim_queue.run_id = " + run +
                  " AND segment_id = " + segment_id;
        }
    }

    [SerializableAttribute]
	/// <summary>
	/// Simple eval directly evaluates from ModelDb
	/// </summary>
	public class SimpleEvalData : DbTableEvalData
	{
		public SimpleEvalData() : base()
		{
		}


       // query for results_std
        public override string Query( int run, int product_id, int segment_id, int channel_id, DateTime start, DateTime end ) {
            DateTime endplus = end;

            endplus = endplus.AddDays( 1 );

            string rVal = null;

            if( useSegmentWeighting && segment_id < 0 ) {
                rVal = "SELECT calendar_date, " + howToSum + "( CONVERT(float, " + colName + " ) * Counter.num_segments * segment.segment_size/100.0) as X " +
                     " FROM results_std WITH (NOLOCK), segment  WITH (NOLOCK), " +
                     " (Select COUNT(*) as num_segments from segment  WITH (NOLOCK) where segment.model_id in " +
                     " (SELECT model_id from sim_queue  WITH (NOLOCK) where run_id = " + run + ")) as Counter " +
                     " WHERE " +
                     " results_std.segment_id = segment.segment_id AND ";
            }
            else {
                rVal = "SELECT calendar_date, " + howToSum + "( CONVERT(float, " + colName + ")) as X " +
                   " FROM results_std WITH (NOLOCK) WHERE ";
            }

            rVal += " calendar_date >= " + "'" + start.ToShortDateString() + "'" +
                    " AND calendar_date < " + "'" + endplus.ToShortDateString() + "'";

            if( type == DataType.InputData ) {
                rVal += " AND results_std." + colName + " > 0 ";
            }

            if( !aggreagateOverProducts && product_id >= 0 ) {
                rVal += " AND product_id = " + product_id;
            }
            else { // get the leaves
                rVal += " AND product_id IN" +
                " (SELECT product_id FROM product WHERE brand_id = 1)";
            }

            if( segment_id >= 0 )
                rVal += " AND results_std.segment_id = " + segment_id;

            if( channel_id >= 0 )
                rVal += " AND channel_id = " + channel_id;
            // need to sum over all channels
            else if( type == DataType.PanelData && useOneChannel ) {
                // pick a channel
                rVal += " and channel_id = (SELECT top 1 channel_id FROM results_std WHERE run_id = " + run + ")";
            }
    
            rVal += " AND run_id = " + run;
          
            rVal += " GROUP BY calendar_date ";

            return rVal;
        }
	}

	/// <summary>
	/// Simple eval directly evaluates from ModelDb
	/// </summary>
	public class ExternalEvalData : DbTableEvalData
	{
		public ExternalEvalData() : base()
		{
			// default is SUM another option could be "AVG"
		
			Column = "value";
			type = DataType.externalData;
		}

        // query for real sales
        public override string Query( int run, int product_id, int segment_id, int channel_id, DateTime start, DateTime end ) {
            DateTime endplus = end;

            endplus = endplus.AddDays( 1 );

            string rVal = null;

            // if product is not a brand
            if( aggreagateOverProducts || product_id == Database.AllID ) {
                // sum over leaves
                rVal = " SELECT calendar_date, " + howToSum + "( CONVERT(float, " + colName + ")) as X " +
               " FROM external_data, product WITH (NOLOCK) WHERE external_data.product_id = product.product_id AND product.brand_id = 1 ";
            }
            else {

                 rVal = ";WITH Dec(dec_id) AS " +
                    " ( " +
                    " 	SELECT L.product_id  from product L WHERE L.product_id =  " + product_id +
                    " 	UNION ALL " +
                    " 	Select A.product_id  " +
                    " 	FROM product A, product_tree T, Dec D " +
                    " 	WHERE A.product_id =  T.child_id " +
                    " 	AND T.parent_id = D.dec_id " +
                    " ) ";

                rVal += " SELECT calendar_date, " + howToSum + "( CONVERT(float, " + colName + ")) as X " +
                   " FROM external_data, Dec WITH (NOLOCK) WHERE external_data.product_id = Dec.dec_id ";
            }


            rVal += " AND external_data.calendar_date >= " + "'" + start.ToShortDateString() + "'" +
              " AND external_data.calendar_date < " + "'" + endplus.ToShortDateString() + "'";

            if( channel_id != Database.AllID ) {
                rVal += " AND external_data.channel_id = " + channel_id;
            }

            rVal += " AND external_data.type = 1 ";
            rVal += " AND external_data.model_id = " + run;

            rVal += " GROUP BY calendar_date ";

            return rVal;
        }

     
	}

    [SerializableAttribute]
    /// <summary>
	/// This is an expression (of perhaps other expressions
	/// used eventually should lead to simple evaluator
	/// </summary>
	public class ExpressionEvalData : EvalData
	{
		public string equation;

		public override bool IsSimple
		{
			get
			{
				return false;
			}
		}

		public override string Equation
		{
			get
			{
				return equation;
			}
		}
	}

	/// <summary>
	/// Summary description for SqlEval.
	/// </summary>
	public abstract class SqlEval
	{
		static public System.Data.OleDb.OleDbConnection DefaultConnection;

		// evaluator factory
		static public SqlEval CreateEvaluator(EvalData evalData)
		{
			SqlEval evaluator = null;

	
			if (evalData.IsSimple)
			{
				DbTableEvalData simpleEval =  evalData as DbTableEvalData;

				if (simpleEval != null)
				{
					evaluator = new SimpleQueryEval(simpleEval);
				}
			}
			else
			{

				ExpressionEvalData expression = evalData as ExpressionEvalData;

				if (expression != null)
					evaluator = new ComplexExpressionEval(expression);
			}
		
			if (evaluator != null)
			{
				if (evaluator.Valid())
					evaluator.EvalData = evalData;
				else
					evaluator = null;
			}

			return evaluator;
		}

		protected System.Data.OleDb.OleDbCommand command = ProjectDb.newOleDbCommand();

        private int run_id;
		private int product_id;
      // private int product_type_id;
		private int segment_id;
		private int channel_id;

		private DateTime start_date;
		private DateTime end_date;

		private EvalData evalData;

		#region Properties
		public System.Data.OleDb.OleDbConnection Connection
		{
			set
			{
				DefaultConnection = value;
			}

			get
			{
				return DefaultConnection;
			}
		}

        public int Run
        {
            get
            {
                return run_id;
            }

            set
            {
                run_id = value;
            }
        }

		public int Product
		{
			set
			{
				product_id = value;
			}

			get
			{
				return product_id;
			}
		}

		public int Segment
		{
			set
			{
				segment_id = value;
			}

			get
			{
				return segment_id;
			}
		}

		public int Channel
		{
			set
			{
				channel_id = value;
			}

			get
			{
				return channel_id;
			}
		}

		public DateTime Start
		{
			set
			{
				start_date = value;
			}

			get
			{
				return start_date;
			}
		}

		public DateTime End
		{
			set
			{
				end_date = value;
			}

			get
			{
				return end_date;
			}
		}

		public string Units
		{
			get
			{
				return evalData.units;
			}

			set
			{
				evalData.units = value;
			}
		}

		public EvalData EvalData
		{
			get
			{
				return evalData;
			}

			set
			{
				evalData = value;
			}
		}


		#endregion

		public abstract DataCurve CreateDataCurve(int time);
		public abstract bool Valid();

		public static EvalData[] Variables;

        private static void createInputData(ArrayList list)
        {
            DbTableEvalData variable;
            ExpressionEvalData expression;

            #region Pricing
            //
            // unpromoprice
            //

            variable = new SimpleEvalData();
            variable.Column = "unpromoprice";
            variable.type = DataType.InputData;
            variable.howToSum = "AVG";
            variable.timeAverage = HowToSum.Average;
            variable.units = "$";
            list.Add(variable);

            //relative unpromoprice
            expression = new ExpressionEvalData();
            expression.token = "rel_unpromoprice";
            expression.equation = "100 * unpromoprice/ave_unpromoprice";
            expression.type = DataType.InputData;
            expression.units = "%";
            list.Add(expression);

            //average unpromoprice
            variable = new SimpleEvalData();
            variable.Column = "unpromoprice";
            variable.token = "ave_unpromoprice";
            variable.howToSum = "AVG";
            variable.timeAverage = HowToSum.Average;
            variable.type = DataType.InputData;
            variable.aggreagateOverProducts = true;
            variable.units = "$";
            list.Add(variable);

            //
            //promoprice
            //
            variable = new SimpleEvalData();
            variable.Column = "promoprice";
            variable.type = DataType.InputData;
            variable.howToSum = "AVG";
            variable.units = "$";
            variable.timeAverage = HowToSum.Average;
            list.Add(variable);

            //relative promoprice
            expression = new ExpressionEvalData();
            expression.token = "rel_promoprice";
            expression.equation = "100 * promoprice/ave_promoprice";
            expression.type = DataType.InputData;
            expression.units = "%";
            list.Add(expression);

            //avg promoprice
            variable = new SimpleEvalData();
            variable.Column = "promoprice";
            variable.token = "ave_promoprice";
            variable.howToSum = "AVG";
            variable.type = DataType.InputData;
            variable.timeAverage = HowToSum.Average;
            variable.units = "$";
            variable.aggreagateOverProducts = true;
            list.Add(variable);

            // percent_sku_at_promo_price
            variable = new SimpleEvalData();
            variable.Column = "percent_sku_at_promo_price";
            variable.type = DataType.InputData;
            variable.howToSum = "AVG";
            variable.timeAverage = HowToSum.Average;
            variable.units = "%";
            list.Add(variable);

           
            //
            // display price
            //
            variable = new SimpleEvalData();
            variable.Column = "display_price";
            variable.howToSum = "AVG";
            variable.timeAverage = HowToSum.Average;
            variable.units = "$";
            variable.type = DataType.InputData;
            list.Add(variable);

            //relative display
            expression = new ExpressionEvalData();
            expression.token = "rel_display_price";
            expression.equation = "100 * display_price/ave_display_price";
            expression.type = DataType.InputData;
            expression.units = "%";
            list.Add(expression);

            //average display
            variable = new SimpleEvalData();
            variable.Column = "display_price";
            variable.token = "ave_display_price";
            variable.howToSum = "AVG";
            variable.timeAverage = HowToSum.Average;
            variable.type = DataType.InputData;
            variable.units = "$";
            variable.aggreagateOverProducts = true;
            list.Add(variable);

            // percent at display price
            variable = new SimpleEvalData();
            variable.Column = "percent_at_display_price";
            variable.howToSum = "AVG";
            variable.timeAverage = HowToSum.Average;
            variable.units = "%";
            variable.type = DataType.InputData;
            list.Add(variable);

            #endregion

            #region distribution
            // percent_preuse_distribution_sku
            variable = new SimpleEvalData();
            variable.Column = "percent_preuse_distribution_sku";
            variable.type = DataType.InputData;
            variable.howToSum = "AVG";
            variable.timeAverage = HowToSum.Average;
            variable.units = "%";
            list.Add(variable);

            //relative percent_preuse_distribution_sku
            expression = new ExpressionEvalData();
            expression.token = "rel_percent_preuse_distribution_sku";
            expression.equation = "100 * percent_preuse_distribution_sku/ave_percent_preuse_distribution_sku";
            expression.type = DataType.InputData;
            expression.units = "%";
            list.Add(expression);

            //average percent_preuse_distribution_sku
            variable = new SimpleEvalData();
            variable.Column = "percent_preuse_distribution_sku";
            variable.token = "ave_percent_preuse_distribution_sku";
            variable.howToSum = "AVG";
            variable.timeAverage = HowToSum.Average;
            variable.type = DataType.InputData;
            variable.units = "%";
            variable.aggreagateOverProducts = true;
            list.Add(variable);

            // percent_on_display_sku
            variable = new SimpleEvalData();
            variable.Column = "percent_on_display_sku";
            variable.type = DataType.InputData;
            variable.howToSum = "AVG";
            variable.timeAverage = HowToSum.Average;
            variable.units = "%";
            list.Add(variable);


            // rel percent_on_display_sku
            expression = new ExpressionEvalData();
            expression.token = "rel_percent_on_display_sku";
            expression.equation = "100 * percent_on_display_sku/ave_percent_on_display_sku";
            expression.type = DataType.InputData;
            expression.units = "%";
            list.Add(expression);


            // ave percent_on_display_sku
            variable = new SimpleEvalData();
            variable.Column = "percent_on_display_sku";
            variable.token = "ave_percent_on_display_sku";
            variable.type = DataType.InputData;
            variable.howToSum = "AVG";
            variable.timeAverage = HowToSum.Average;
            variable.units = "%";
            variable.aggreagateOverProducts = true;
            list.Add(variable);

            #endregion

            #region GRP
            // GRPs_SKU_tick
            variable = new SimpleEvalData();
            variable.Column = "GRPs_SKU_tick";
            variable.type = DataType.InputData;
            variable.howToSum = "AVG";
            variable.timeAverage = HowToSum.Average;
            variable.units = "GRP";
            list.Add(variable);


            // rel GRPs_SKU_tick
            expression = new ExpressionEvalData();
            expression.token = "rel_GRPs_SKU_tick";
            expression.equation = "100 * GRPs_SKU_tick/ave_GRPs_SKU_tick";
            expression.type = DataType.InputData;
            expression.units = "%";
            list.Add(expression);


            // ave GRPs_SKU_tick
            variable = new SimpleEvalData();
            variable.Column = "GRPs_SKU_tick";
            variable.token = "ave_GRPs_SKU_tick";
            variable.type = DataType.InputData;
            variable.howToSum = "AVG";
            variable.timeAverage = HowToSum.Average;
            variable.units = "GRP";
            variable.aggreagateOverProducts = true;
            list.Add(variable);

            #endregion


        }

        private static void createSimSalesData(ArrayList list)
        {
            DbTableEvalData variable;
            ExpressionEvalData expression;

            variable = new NumConsumers();
            variable.type = DataType.SimResults;
            variable.hidden = true;
            variable.timeAverage = HowToSum.Constant;
            variable.token = "num_consumers";
            list.Add( variable );

            //
            // Unit Sales
            //

            // share
            expression = new ExpressionEvalData();
            expression.token = "sku_share";
            expression.equation = "100 * num_sku_bought/total_sku_bought";
            expression.units = "%";
            list.Add(expression);

            // dollar share
            expression = new ExpressionEvalData();
            expression.token = "dollar_share";
            expression.equation = "100 * sku_dollar_purchased_tick/total_sku_dollar_purchased_tick";
            expression.units = "%";
            list.Add(expression);

            // num_sku_bought
            variable = new SimpleEvalData();
            variable.Column = "num_sku_bought";
            
            variable.units = "#";
            list.Add(variable);


            //
            // Dollar Sales
            //

            // sku_dollar_purchased_tick
            variable = new SimpleEvalData();
            variable.Column = "sku_dollar_purchased_tick";
            
            variable.units = "$";
            list.Add(variable);

            // percent_aware_sku_cum
            variable = new SimpleEvalData();
            variable.Column = "percent_aware_sku_cum";
            variable.howToSum = "AVG";
            variable.units = "%";
            variable.useSegmentWeighting = true;
            variable.timeAverage = HowToSum.Average;
            list.Add(variable);

            // persuasion_sku
            variable = new SimpleEvalData();
            variable.Column = "persuasion_sku";
            variable.howToSum = "AVG";
            variable.units = "persuasion";
            variable.useSegmentWeighting = true;
            variable.timeAverage = HowToSum.Average;
            list.Add(variable);



            // total sku bought
            variable = new SimpleEvalData();
            variable.Column = "num_sku_bought";
            variable.token = "total_sku_bought";
            variable.units = "#";
            variable.aggreagateOverProducts = true;
            variable.hidden = true;
            list.Add(variable);

            // total dollars
            variable = new SimpleEvalData();
            variable.Column = "sku_dollar_purchased_tick";
            variable.token = "total_sku_dollar_purchased_tick";
            variable.units = "$";
            variable.aggreagateOverProducts = true;
            variable.hidden = true;
            list.Add(variable);


            // eq units
            variable = new SimpleEvalData();
            variable.Column = "eq_units";
            
            variable.units = "#";
            list.Add(variable);

            // volume
            variable = new SimpleEvalData();
            variable.Column = "volume";
            
            variable.units = "#";
            list.Add(variable);


            // number units sold at non-promo price
            variable = new SimpleEvalData();
            variable.Column = "num_units_unpromo";
            
            variable.units = "#";
            list.Add(variable);

            // number units sold at unpromo price
            expression = new ExpressionEvalData();
            expression.token = "percent_units_unpromo";
            expression.equation = "100 * num_units_unpromo/num_sku_bought";
            expression.units = "%";
            list.Add(expression);

            // number units sold at promo price
            variable = new SimpleEvalData();
            variable.Column = "num_units_promo";
            
            variable.units = "#";
            list.Add(variable);


            // percent units sold at promo price
            expression = new ExpressionEvalData();
            expression.token = "percent_units_promo";
            expression.equation = "100 * num_units_promo/num_sku_bought";
            expression.units = "%";
            list.Add(expression);

            // number units sold at display price
            variable = new SimpleEvalData();
            variable.Column = "num_units_display";
            
            variable.units = "#";
            list.Add(variable);

            // percent units sold at display price
            expression = new ExpressionEvalData();
            expression.token = "percent_units_display";
            expression.equation = "100 * num_units_display/num_sku_bought";
            expression.units = "%";
            list.Add(expression);

            // num_coupon_redemptions
            variable = new SimpleEvalData();
            variable.Column = "num_coupon_redemptions";
            
            variable.units = "#";
            list.Add(variable);

            // num_units_bought_on_coupon
            variable = new SimpleEvalData();
            variable.Column = "num_units_bought_on_coupon";
            
            variable.units = "#";
            list.Add(variable);
        }

        private static void createSimPanelData(ArrayList list)
        {
            DbTableEvalData variable;
            ExpressionEvalData expression;

            

            // num_sku_triers
            variable = new SimpleEvalData();
            variable.Column = "num_sku_triers";
            variable.units = "number of triers";
            variable.type = DataType.PanelData;
            variable.timeAverage = HowToSum.Max;
            list.Add(variable);

            // Trial Rate
            expression = new ExpressionEvalData();
            expression.token = "trial_rate";
            expression.equation = "100*num_sku_triers/num_consumers";
            expression.units = "trial rate";
            expression.type = DataType.PanelData;
            list.Add( expression );

            // num_sku_repeaters
            variable = new SimpleEvalData();
            variable.Column = "num_sku_repeaters";
            variable.units = "number of repeaters";
            variable.type = DataType.PanelData;
            variable.timeAverage = HowToSum.Max;
            list.Add(variable);

            // num_sku_repeaters_trips_cum
            variable = new SimpleEvalData();
            variable.Column = "num_sku_repeater_trips_cum";
            variable.units = "number of repeaters per trip";
            variable.type = DataType.PanelData;
            variable.timeAverage = HowToSum.Max;
            list.Add(variable);

            // Repeaters_over_Triers
            expression = new ExpressionEvalData();
            expression.token = "repeaters_per_trier";
            expression.equation = "num_sku_repeaters/num_sku_triers";
            expression.units = "repeaters rate";
            expression.type = DataType.PanelData;
            list.Add(expression);

            // num_sku_repeater_trips_cum_over_repeaters
            expression = new ExpressionEvalData();
            expression.token = "repeats_per_repeater";
            expression.equation = "num_sku_repeater_trips_cum/num_sku_repeaters";
            expression.type = DataType.PanelData;
            list.Add(expression);

            //  purchase frequency
            expression = new ExpressionEvalData();
            expression.token = "purchase_frequency";
            expression.equation = "(num_sku_triers + num_sku_repeater_trips_cum)/num_sku_triers";
            expression.type = DataType.PanelData;
            expression.units = "Freq.";
            list.Add(expression);

            // buyng rate
            expression = new ExpressionEvalData();
            expression.token = "buying_rate";
            expression.equation = "sku_dollar_purchased_tick/num_sku_triers";
            expression.units = "buy rate";
            expression.type = DataType.PanelData;
            list.Add(expression);

            // number of shopping trips
            variable = new SimpleEvalData();
            variable.Column = "num_trips";
            variable.units = "#";
            variable.type = DataType.PanelData;
            variable.useOneChannel = false;
            list.Add(variable);

            // Transaction size
            expression = new ExpressionEvalData();
            expression.token = "transaction_size_units";
            expression.equation = "num_sku_bought/num_trips";
            expression.units = "#";
            expression.type = DataType.PanelData;
            list.Add(expression);

            // avg_sku_transaction_dollars
            expression = new ExpressionEvalData();
            expression.token = "transaction_size_dollars";
            expression.equation = "sku_dollar_purchased_tick/num_trips";
            expression.units = "$";
            expression.type = DataType.PanelData;
            list.Add(expression);



            // num_adds_sku
            variable = new SimpleEvalData();
            variable.Column = "num_adds_sku";
            variable.units = "#";
            variable.type = DataType.PanelData;
            list.Add(variable);

            // num_drop_sku
            variable = new SimpleEvalData();
            variable.Column = "num_drop_sku";
            variable.units = "#";
            variable.type = DataType.PanelData;
            list.Add(variable);
        }

        private static void createRealSalesData(ArrayList list)
        {
            ExpressionEvalData expression;

            //
            // real data
            // user is allowed to add new types
            // these are types we are aware off and want to build expressions
            // out of

            ExternalEvalData extVariable;

            // real sales
            extVariable = new ExternalEvalData();
            extVariable.Column = "value";
            extVariable.token = "real_sales";
            list.Add(extVariable);

            // total sales
            extVariable = new ExternalEvalData();
            extVariable.Column = "value";
            extVariable.token = "real_total_sales";
            extVariable.aggreagateOverProducts = true;
            extVariable.hidden = true;
            list.Add(extVariable);

            // share
            expression = new ExpressionEvalData();
            expression.token = "real_share";
            expression.equation = "100 * real_sales/real_total_sales";
            expression.units = "%";
            expression.type = DataType.externalData;
            list.Add(expression);

            // removed this until we actually support it
            // SSN 6/12/2007
            // real sales
            //extVariable = new ExternalEvalData();
            //extVariable.Column ="value";
            //extVariable.token = "real_awareness";
            //expression.selectByBrand = true;
            //extVariable.Id = 2;
            //list.Add(extVariable);

        }

        static public EvalData GetEvalDataFromToken( string token ) {

            foreach( EvalData eval in Variables ) {

                if( eval.token == token ) {
                    return eval;
                }

            }

            return null;
        }

        /// <summary>
        /// Returns all variables of the given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EvalData[] GetEvalData(DataType type)
        {
            ArrayList list = new ArrayList();
            foreach (EvalData eval in Variables)
            {
                if (eval.type == type && !eval.hidden)
                {
                    list.Add(eval);
                }
            }

            EvalData[] rval = new EvalData[list.Count];

            for (int index = 0; index < list.Count; index++)
                rval[index] = (EvalData) list[index];

            return rval;
        }

		static SqlEval()
		{
			ArrayList list = new ArrayList(100);

            createInputData(list);

            createSimSalesData(list);

            createSimPanelData(list);

            createRealSalesData(list);

			Variables = new EvalData[list.Count];

			for(int index = 0; index < list.Count; index++)
				Variables[index] = (EvalData) list[index];
		}
	}

	public class ComplexExpressionEval : SqlEval
	{
		public override DataCurve CreateDataCurve(int time)
		{
			if (expression == null)
				return null;

			ExpressionTree[] variables = expression.Variables;

			// perform queries
			foreach(ExpressionTree exp in variables)
			{
				QueryNode node = exp as QueryNode;

				if (node == null)
					continue;

				node.evaluator.Run = Run;
				node.evaluator.Product = Product;
				node.evaluator.Segment = Segment;
				node.evaluator.Channel = Channel;
				node.evaluator.Start = Start;
				node.evaluator.End = End;

				if (!node.ReadDataCrv(time))
					return null;
			}

			// get number of datum points
			// initialize variables

			TimeSpan span = End - Start;

			int numDays = (int) Math.Floor((double) (span.Days + 1)/time);

			if (numDays == 0)
				return null;

			DataCurve crv = new DataCurve(numDays);

			DateTime day = Start;

            if (numDays == 1)
            {
                day = End;
            }
            else
            {
                day = day.AddDays(time - 1);
            }

			while(day <= End)
			{
				double val = expression.Evaluate(day);

				crv.Add(day, val);

				day = day.AddDays(time);
			}

			return crv;
		}


		public override bool Valid()
		{
			if (expression == null)
				return false;

			return expression.Valid();
		}

		ExpressionTree expression;

		public ComplexExpressionEval(ExpressionEvalData data)
		{
			Parser parser = new Parser(data);
			parser.CreateExpression = new Parser.VarToExpression(createQueryNode);

			// complex expression can evaluate any variable
			parser.Variables = Variables;
			
			expression = parser.Parse();

			EvalData = data;
		}

		private ExpressionTree createQueryNode(Variable variable)
		{
			QueryNode node = new QueryNode(variable as EvalData);

			node.evaluator.Run = Run;
			node.evaluator.Product = Product;
			node.evaluator.Segment = Segment;
			node.evaluator.Channel = Channel;
			node.evaluator.Start = Start;
			node.evaluator.End = End;

			return node;
		}

	}

	public class SimpleQueryEval : SqlEval
	{
		public override bool Valid()
		{
			return true;
		}

		DbTableEvalData qEval;

		public SimpleQueryEval(DbTableEvalData data)
		{
			qEval = data;
		}

		public override DataCurve CreateDataCurve(int time)
		{
			string query;

            query = qEval.Query(Run, Product,  Segment, Channel, Start, End);
			
			if (query == null)
				return null;

			command.Connection = DefaultConnection;

            DataCurve crv = null;
            object[] crvData = new object[ 2 ];


            command.CommandText = query + " ORDER BY calendar_date";

            command.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = command.ExecuteReader( CommandBehavior.CloseConnection );

            crv = new DataCurve();

            while( dataReader.Read() ) {
                dataReader.GetValues( crvData );

                DateTime day = Utility.Convert2DateTime( crvData[ 0 ] );

                double val = Utility.Convert2Double( crvData[ 1 ] );

                crv.Add( day, val );
            }

            dataReader.Close();

            if( crv.X.Length == 0 ) {
                crv.Add( Start, 0.0 );
            }

            if( crv.X.Length < 2 ) {
                crv.Add( End, 0.0 );
            }

			TimeSpan span = new TimeSpan(time,0,0,0,0);

            crv = crv.Average(Start, End, span, qEval.timeAverage);

			return crv;
		}
	}


	//
	// this what a variable is 
	//

	public class QueryNode : ExpressionTree
	{
		public SqlEval evaluator;
		private DataCurve crv = null;

		public override bool Valid()
		{
			if ( evaluator == null)
				return false;

			return evaluator.Valid();
		}

		public QueryNode(EvalData data)
		{
			evaluator = SqlEval.CreateEvaluator(data);
		}

		public override int NumTerms()
		{
			return 1;
		}

		public bool ReadDataCrv(int time)
		{
			crv = evaluator.CreateDataCurve(time);

			if (crv == null)
				return false;

			return true;
		}

		public override double Evaluate()
		{
			return 0.0;
		}

		public override double Evaluate(Object obj)
		{
			DateTime day = (DateTime) obj;

			return crv.Eval(day);
		}
	}
}


