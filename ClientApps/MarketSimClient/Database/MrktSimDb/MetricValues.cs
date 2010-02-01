using System;
using System.Collections.Generic;
using System.Text;

namespace MrktSimDb.Metrics
{
    public abstract class Value
	{
		#region Fields

        private int channelID = Database.AllID;
        private int prodID = Database.AllID;
        private int runID = -Database.AllID;

		#endregion

        public bool ConstantOverProducts = false;

		public Value()
		{
		}

		public virtual int Product
		{
			get
			{
				return prodID;
			}

			set
			{
				prodID = value;
			}
		}

        public  int Channel
        {
            get
            {
                return channelID;
            }

            set
            {
                channelID = value;
            }
        }

		public int Run
		{
			get
			{
				return runID;
			}

			set
			{
				runID = value;
			}
		}


		virtual public string Descr
		{
			get
			{
				return this.ToString();
			}
		}

		

        public abstract double Evaluate( MetricUtil util );
	}

    public abstract class QueryEvaluate : Value
    {
        public abstract string EvaluateQuery();

        public override double Evaluate( MetricUtil util ) {
            util.OleDbCommand.CommandText = EvaluateQuery();

            util.OleDbCommand.Connection.Open();

            object obj = null;

            try {
                obj = util.OleDbCommand.ExecuteScalar();
            }
            catch( Exception ) { }

            util.OleDbCommand.Connection.Close();

            if( obj != null && obj.GetType() != typeof( System.DBNull ) ) {
                return Convert.ToDouble( obj );
            }

            return 0.0;
        }
    }

    public class AvgPrice : QueryEvaluate
    {

        public AvgPrice() {
        }

        public override string Descr {
            get {
                return "Average Price";
            }
        }
        public override string EvaluateQuery() {
            return "SELECT AVG(results_std.unpromoprice)/AVG(simulation.access_time) " +
                " FROM results_std, simulation, sim_queue " +
                " WHERE	sim_queue.run_id = " + this.Run +
                " AND sim_queue.run_id = results_std.run_id " +
                " AND simulation.id = sim_queue.sim_id " +
                " AND product_id = " + this.Product;
        }
    }

    public class AvgDistribution : QueryEvaluate
    {

        public AvgDistribution() {
        }

        public override string Descr {
            get {
                return "Average Distribution";
            }
        }
        public override string EvaluateQuery() {
            return "SELECT AVG(results_std.percent_preuse_distribution_sku)/AVG(simulation.access_time) " +
                " FROM results_std, simulation, sim_queue " +
                " WHERE	sim_queue.run_id = " + this.Run +
                " AND sim_queue.run_id = results_std.run_id " +
                " AND simulation.id = sim_queue.sim_id " +
                " AND product_id = " + this.Product;
        }
    }

    public class AvgAwareness : QueryEvaluate
    {

        public AvgAwareness() {
        }

        public override string Descr {
            get {
                return "Average Awareness";
            }
        }
        public override string EvaluateQuery() {
            return "SELECT AVG(results_std.percent_aware_sku_cum)/AVG(simulation.access_time) " +
                " FROM results_std, simulation, sim_queue " +
                " WHERE	sim_queue.run_id = " + this.Run +
                " AND sim_queue.run_id = results_std.run_id " +
                " AND simulation.id = sim_queue.sim_id " +
                " AND product_id = " + this.Product;
        }
    }

    public class AvgACVDisplay : QueryEvaluate
    {

        public AvgACVDisplay() {
        }

        public override string Descr {
            get {
                return "Average %ACV Display";
            }
        }
        public override string EvaluateQuery() {
            return "SELECT AVG(results_std.percent_on_display_sku)/AVG(simulation.access_time) " +
                " FROM results_std, simulation, sim_queue " +
                " WHERE	sim_queue.run_id = " + this.Run +
                " AND sim_queue.run_id = results_std.run_id " +
                " AND simulation.id = sim_queue.sim_id " +
                " AND product_id = " + this.Product;
        }
    }


    public class BasePrice : QueryEvaluate
    {

        public BasePrice() {
        }

        public override string Descr {
            get {
                return "Base Price";
            }
        }
        public override string EvaluateQuery() {
            return "SELECT base_price FROM product WHERE product_id = " + this.Product;
        }
    }

    public class NIMOTau : QueryEvaluate
    {

        public NIMOTau()
		{
		}

		public override string Descr
		{
			get
			{
				return "Tau";
			}
		}
        public override string EvaluateQuery() {
            return "SELECT cust_tau FROM product_attribute, product WHERE " +
                " product_attribute_name = product.product_name" +
                " AND product_id = " + this.Product;

        }
    }

    public class RunNum : QueryEvaluate
    {
        public override int Product
        {
            get
            {
                return Database.AllID;
            }
            set
            {
                base.Product = Database.AllID;
            }
        }

        public RunNum()
        {
            this.ConstantOverProducts = true;
        }

        public override string Descr
        {
            get
            {
                return "Run #";
            }
        }

        public override string EvaluateQuery()
        {
            return "SELECT COUNT(*) FROM sim_queue WHERE run_id <= " + Run
                + " AND sim_queue.sim_id IN (SELECT sim_id from sim_queue WHERE run_id = " + Run + ")"; 
        }
    }

    public class MAPE : QueryEvaluate
    {
        public MAPE() {
        }

        public override string Descr
        {
            get {
                if( this.Product == Database.AllID )
                    return "Total MAPE";

                return "MAPE";
            }
        }

        public override string EvaluateQuery()
        {
            if (Product == Database.AllID)
            return "SELECT mape FROM results_calibration_total WHERE run_id = " + Run;

        return "SELECT mape FROM results_calibration_by_product WHERE run_id = " + Run +
            " AND product_id = " + Product;
        }
    }


    public class AttrRMS : QueryEvaluate
    {

        // cannot compute Attribute RMS for individual products
        public override int Product
        {
            get
            {
                return Database.AllID;
            }
            set
            {
                base.Product = Database.AllID;
            }
        }

        public AttrRMS() {
            this.ConstantOverProducts = true;
        }

        public override string Descr
        {
            get
            {
                return "Attribute RMS";
            }
        }

        public override string EvaluateQuery()
        {
            return "SELECT SQRT(SUM(diff*diff)) FROM " +
                " (SELECT  attr.product_attribute_id,  results_calibration_by_product_channel.channel_id, " +
                " SUM(attr.pre_attribute_value * (results_calibration_by_product_channel.sim_share - results_calibration_by_product_channel.real_share)/100.0) as diff  " +
                " FROM product_attribute_value as attr, results_calibration_by_product_channel  " +
                " WHERE results_calibration_by_product_channel.run_id = " + Run +
                " AND results_calibration_by_product_channel.product_id = attr.product_id  " +
                " GROUP BY attr.product_attribute_id, results_calibration_by_product_channel.channel_id " +
                " ) as AttrRMS ";
        }
    }

    public class ShareDiff : QueryEvaluate
	{
		
		public ShareDiff()
		{
		}

		public override string Descr
		{
			get
			{
				return "Share Difference";
			}
		}

		public override string EvaluateQuery()
		{
			return "SELECT share_diff FROM results_calibration_by_product WHERE run_id = " + Run +
				" AND product_id = " + Product;
		}
	}

    public class PercentError : QueryEvaluate
	{
		
		public PercentError()
		{
		}

		public override string Descr
		{
			get
			{
				return "Percent Error";
			}
		}

		public override string EvaluateQuery()
		{
			return "SELECT percent_share_error FROM results_calibration_by_product WHERE run_id = " + Run +
				" AND product_id = " + Product;
		}
	}

    public class SqrError : QueryEvaluate
	{
		public SqrError()
		{
		}

		public override string Descr
		{
			get
			{
				return "RMS Error";
			}
		}


		public override string EvaluateQuery()
		{
			if (Product == -1)
			{
                return "SELECT SQRT (SUM(share_diff*share_diff)) FROM results_calibration_by_product WHERE run_id = " + Run;
			}

            return "SELECT SQRT ( SUM(share_diff*share_diff)) FROM results_calibration_by_product WHERE run_id = " + Run +
				" AND product_id = " + Product;
			
		}
	}

    public class DollarShare : QueryEvaluate
	{
		public DollarShare()
		{
		}

		public override string Descr
		{
			get
			{
				return "Dollar Share";
			}
		}


		public override string EvaluateQuery()
		{
			if (Product == -1)
			{
                return "SELECT SUM(dollar_share) FROM results_summary_by_product, product " + 
                    " WHERE product.product_id = results_summary_by_product.product_id AND " +
                    " product.brand_id = 1 AND run_id = " + Run;
			}

			return "SELECT dollar_share FROM results_summary_by_product WHERE run_id = " + Run +
				" AND product_id = " + Product;
			
		}
	}

    public class UnitShare : QueryEvaluate
	{
		public UnitShare()
		{
		}

		public override string Descr
		{
			get
			{
				return "Unit Share";
			}
		}


		public override string EvaluateQuery()
		{
			if (Product == -1)
			{
                return "SELECT SUM(unit_share) FROM results_summary_by_product WHERE product.product_id = results_summary_by_product.product_id AND " +
                   " product.brand_id = 1 AND run_id = " + Run;
			}

			return "SELECT SUM(unit_share) FROM results_summary_by_product WHERE run_id = " + Run +
				" AND product_id = " + Product;
			
		}
	}

    public class RealUnitShare : QueryEvaluate
    {
        public RealUnitShare() {
        }

        public override string Descr {
            get {
                return "Real Unit Share";
            }
        }


        public override string EvaluateQuery() {
            if( Product == -1 ) {
                return "SELECT SUM(unit_share) FROM results_calibration_by_product, product WHERE product.product_id = results_calibration_by_product.product_id AND " +
                    " product.brand_id = 1 AND run_id = " + Run;
            }

            return "SELECT SUM(unit_share) FROM results_calibration_by_product WHERE run_id = " + Run +
                " AND product_id = " + Product;

        }
    }

    public class UnitsSold : QueryEvaluate
	{
		public UnitsSold()
		{
		}

		public override string Descr
		{
			get
			{
				return "Units Sold";
			}
		}


		public override string EvaluateQuery()
		{
			if (Product == -1 && Channel == -1)
			{
				return "SELECT SUM(num_units) FROM results_summary_total WHERE run_id = " + Run;
			}
            else if (Channel == -1)
            {

                return "SELECT SUM(num_units) FROM results_summary_by_product WHERE run_id = " + Run +
                    " AND product_id = " + Product;
            }
            else
            {
                return "SELECT SUM(num_units) FROM results_summary_by_channel_segment WHERE run_id = " + Run +
                 " AND channel_id = " + Channel;
            }
			
		}
	}

    public class RealUnitsSold : QueryEvaluate
    {
        public RealUnitsSold()
        {
        }

        public override string Descr
        {
            get
            {
                return "Real Units Sold";
            }
        }


        public override string EvaluateQuery()
        {
            if (Product == -1 && Channel == -1)
            {
                return "SELECT SUM(num_units) FROM external_data_alligned_sales_by_product_channel WHERE run_id = " + Run;
            }
            else if (Channel == -1)
            {

                return "SELECT SUM(num_units) FROM external_data_alligned_sales_by_product_channel WHERE run_id = " + Run +
                    " AND product_id = " + Product;
            }
            else
            {
                return "SELECT SUM(num_units) FROM external_data_alligned_sales_by_product_channel WHERE run_id = " + Run +
                 " AND channel_id = " + Channel;
            }

        }
    }

    public class Dollars : QueryEvaluate
    {
        public Dollars() {
        }

        public override string Descr {
            get {
                return "Dollars";
            }
        }


        public override string EvaluateQuery() {
            if( Product == -1 ) {
                return "SELECT num_dollars FROM results_summary_total WHERE run_id = " + Run;
            }

            return "SELECT num_dollars FROM results_summary_by_product WHERE run_id = " + Run +
                " AND product_id = " + Product;

        }
    }

    public abstract class Calculator : Value
    {
        protected abstract double CalculateVal( MetricUtil util );

        public override double Evaluate( MetricUtil util ) {

            util.Run = this.Run;


            return CalculateVal( util );
        }
    }

    public class SalesRate : Calculator
    {
        public SalesRate() { }

        public override string Descr {
            get {
                return "Sales Rate";
            }
        }

        protected override double CalculateVal( MetricUtil util ) {

            SortedDictionary<int, MetricUtil.PricePointValue> rval = util.PricePoint;

            if( rval.ContainsKey( Product ) ) {

                return rval[ Product ].Val;
            }

            return 0.0;
        }
    }

    public class RealSalesRate : Calculator
    {
        public RealSalesRate() { }

        public override string Descr {
            get {
                return "Real Sales Rate";
            }
        }

        protected override double CalculateVal( MetricUtil util ) {
            SortedDictionary<int, MetricUtil.PricePointValue> rval = util.RealPricePoint;

            if( rval.ContainsKey( Product ) ) {

                return rval[ Product ].Val;
            }

            return 0.0;
        }
    }

    public class SalesRateR2 : Calculator
    {
        public SalesRateR2() { }

        public override string Descr {
            get {
                return "Sales Rate R^2";
            }
        }

        protected override double CalculateVal( MetricUtil util ) {
            SortedDictionary<int, MetricUtil.PricePointValue> rval = util.PricePoint;

            if( rval.ContainsKey( Product ) ) {

                return rval[ Product ].R2;
            }

            return 0.0;
        }
    }

    public class RealSalesRateR2 : Calculator
    {
        public RealSalesRateR2() { }

        public override string Descr {
            get {
                return "Real Sales Rate R^2";
            }
        }

        protected override double CalculateVal( MetricUtil util ) {
            SortedDictionary<int, MetricUtil.PricePointValue> rval = util.RealPricePoint;

            if( rval.ContainsKey( Product ) ) {

                return rval[ Product ].R2;
            }

            return 0.0;
        }
    }

}
