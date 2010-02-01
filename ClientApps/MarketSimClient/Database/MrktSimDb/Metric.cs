using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Collections.Generic;

namespace MrktSimDb.Metrics
{
	/// <summary>
	/// Metric knows the structure of the database that needs to be read
    /// Metrics perform the calculation of the metric values
	/// </summary>
	public abstract class Metric
	{	
		public delegate string Description(string tokenWord);
		public static event Description TokenConvert;


		
		#region Fields

		private string token;
        protected string viewName;
		protected string table;

		#endregion

		#region public Properties and methods
	
		public string Token
		{
			get
			{
				return token;
			}

			set
			{
				token = value;
			}
		}

		public string Descr
		{
			get
			{
				return this.ToString();
			}
		}
	
		public override string ToString()
		{
			if (TokenConvert != null)
				return TokenConvert("Metric." + Token);

			return Token;
		}

           
        public virtual bool CreateMetric(MetricUtil util ) {

            bool rval = true;

            util.OleDbCommand.CommandText = "INSERT INTO " + table;
            util.OleDbCommand.CommandText += " SELECT * FROM " + viewName + " WHERE run_id = " + util.Run;

            util.OleDbCommand.Connection.Open();

            try {
                util.OleDbCommand.ExecuteNonQuery();
            }
            catch( System.Data.OleDb.OleDbException ) {
                rval = false;
            }

            util.OleDbCommand.Connection.Close();

            return rval;
        }

        public void ClearMetric( MetricUtil util ) {
            util.OleDbCommand.CommandText = "DELETE from " + table + " WHERE run_id = " + util.Run;

            util.OleDbCommand.Connection.Open();

            try {
                util.OleDbCommand.ExecuteNonQuery();
            }
            catch( System.Data.OleDb.OleDbException ) {
            }

            util.OleDbCommand.Connection.Close();
        }

		#endregion
    }

    public class SimSummary : Metric
    {
        public SimSummary() {
            Token = "SimSummary";
            table = "results_summary_total";
            viewName = "summary_total";
        }
    }

    public class SimSummaryByProd : Metric
    {
        public SimSummaryByProd() {
            Token = "SimSummaryByProd";
            table = "results_summary_by_product";
            viewName = "summary_by_product";
        }
    }

    public class SimSummaryByProdChan : Metric
    {
        public SimSummaryByProdChan() {
            Token = "SimSummaryByProdChan";
            table = "results_summary_by_product_channel";
            viewName = "summary_by_product_channel";
         }
    }

    public class SimSummaryByProdChanSeg : Metric
    {
         public SimSummaryByProdChanSeg() {
            Token = "SimSummaryByProdChanSeg";
            table = "results_summary_by_product_channel_segment";
            viewName = "summary_by_product_channel_segment";
         }
    }

    public class SimSummaryByProdSeg : Metric
    {
         public SimSummaryByProdSeg() {
            Token = "SimSummaryByProdSeg";
            table = "results_summary_by_product_segment";
            viewName = "summary_by_product_segment";
         }
    }

    public class SimSummaryByChanSeg : Metric
    {
         public SimSummaryByChanSeg() {
            Token = "SimSummaryByChanSeg";
            table = "results_summary_by_channel_segment";
            viewName = "summary_by_channel_segment";
         }
    }

    public class CalSummaryByProd : Metric
    {
         public CalSummaryByProd() {
            Token = "CalSummaryByProd";
            table = "results_calibration_by_product";
            viewName = "res_compare_by_product";
         }

        public override bool CreateMetric( MetricUtil util) {

            bool rval = true;

            // compute mape using utility
            SortedDictionary<int, MetricUtil.ProdCalibration> prodMape = util.ComputeProductMape();

            foreach( int product in prodMape.Keys ) {
                MetricUtil.ProdCalibration cal = prodMape[ product ];

                // need to store in database
                util.OleDbCommand.CommandText = "INSERT  results_calibration_by_product (run_id, product_id, mape, sim_share, real_share, share_diff, percent_share_error) ";
                util.OleDbCommand.CommandText += " VALUES (" + util.Run + ", ";
                util.OleDbCommand.CommandText += product + ", ";
                util.OleDbCommand.CommandText += cal.MAPE + ", ";
                util.OleDbCommand.CommandText += cal.simShare + ", ";
                util.OleDbCommand.CommandText += cal.realShare + ", ";
                util.OleDbCommand.CommandText += cal.shareDiff + ", ";
                util.OleDbCommand.CommandText += cal.percentError + " )";

                util.OleDbCommand.Connection.Open();

                try {
                    util.OleDbCommand.ExecuteNonQuery();
                }
                catch( System.Data.OleDb.OleDbException) {
                    rval = false;
                }

                util.OleDbCommand.Connection.Close();

                if( !rval ) {
                    break;
                }
            }


            return rval;
        }
    }

    public class CalSummaryByProdChan : Metric
    {
         public CalSummaryByProdChan() {
            Token = "CalSummaryByProdChan";
            table = "results_calibration_by_product_channel";
            viewName = "res_compare_by_product_channel";
         }
    }
   
    public class CalSummary : Metric
    {
         public CalSummary() {
            Token = "CalSummary";
            table = "results_calibration_total";
            viewName = "res_compare_total";
         }

        public override bool CreateMetric( MetricUtil util ) {

            bool rval = true;

            // compute mape using utility
            util.ComputeProductMape();

            // need to store in database
            util.OleDbCommand.CommandText = "INSERT  results_calibration_total (run_id, mape)";
            util.OleDbCommand.CommandText += " VALUES (" + util.Run + ", " + util.TotalMape + ")";

            util.OleDbCommand.Connection.Open();

            try {
                util.OleDbCommand.ExecuteNonQuery();
            }
            catch( System.Data.OleDb.OleDbException ) {
                rval = false;
            }

            util.OleDbCommand.Connection.Close();

            return rval;
        }
    }

    public class CalSummaryByChan : Metric
    {
         public CalSummaryByChan() {
            Token = "CalSummaryByChan";
            table = "results_calibration_by_channel";
            viewName = "res_compare_by_channel";
         }
    }

    public class CalMAPEByProdChan : Metric
    {
         public CalMAPEByProdChan() {
            Token = "CalMAPEByProdChan";
            table = "results_mape_by_product_channel";
            viewName = "mape_by_product_channel";
         }
    }
}



	