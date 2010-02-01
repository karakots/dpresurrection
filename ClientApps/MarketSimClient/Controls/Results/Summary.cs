using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using MrktSimDb;
using MrktSimDb.Metrics;

using MarketSimUtilities;

namespace Results
{
	/// <summary>
	/// Summary description for Summary.
	/// </summary>
	public class Summary : System.Windows.Forms.Form
	{
		const int numMaxGrids = 8;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem tileItem;
		private System.Windows.Forms.MenuItem tileVItem;

		private System.Data.OleDb.OleDbDataAdapter tableAdapter;


		public void WriteToCVS(System.IO.StreamWriter writer)
		{
			foreach(object obj in this.MdiChildren)
			{
				GridView grid = (GridView) obj;

				grid.WriteToCVS(writer);
			}
		}

		public void GetTables(ArrayList list)
		{
			foreach(object obj in this.MdiChildren)
			{
				GridView grid = (GridView) obj;

				list.Add(grid.Table);
			}
		}
		// add a run to the simulation form
		public void AddMetricTable(int runID)
		{
			// fill tables

			foreach(object obj in this.MdiChildren)
			{
				GridView grid = (GridView) obj;

				grid.Suspend = true;

				switch(grid.Table.TableName)
				{
					case "SimSummary":
						tableAdapter.SelectCommand.CommandText =
                            "SELECT simulation.name as simulation, sim_queue.name as sim, num_units, num_dollars, results_summary_total.eq_units, volume" + 
							" FROM results_summary_total, simulation, sim_queue " +
							" WHERE results_summary_total.run_id = " + runID +
							" AND sim_queue.run_id = " + runID +
							" AND simulation.id = sim_queue.sim_id";

						tableAdapter.Fill(grid.Table);
						break;

					case "SimSummaryByProd":
						tableAdapter.SelectCommand.CommandText =
                            "SELECT simulation.name as simulation, sim_queue.name as sim, product.product_name, product_type.type_name, num_units, num_dollars, " +
                            "results_summary_by_product.eq_units, volume, unit_share, dollar_share " +
							" FROM results_summary_by_product, simulation, sim_queue, product, product_type " +
							" WHERE results_summary_by_product.run_id = " + runID +
							" AND sim_queue.run_id = " + runID +
							" AND simulation.id = sim_queue.sim_id" +
                            " AND product.product_type = product_type.id" +
							" AND product.product_id = results_summary_by_product.product_id";

						tableAdapter.Fill(grid.Table);
						break;

					case "SimSummaryByProdChan":
						tableAdapter.SelectCommand.CommandText =
                            "SELECT simulation.name as simulation, sim_queue.name as sim, product.product_name,  product_type.type_name, channel.channel_name, " +
                            " num_units, num_dollars, results_summary_by_product_channel.eq_units, volume, unit_share, dollar_share " +
                            " FROM results_summary_by_product_channel, simulation, sim_queue, product, channel, product_type  " +
							" WHERE results_summary_by_product_channel.run_id = " + runID +
							" AND sim_queue.run_id = " + runID +
                            " AND simulation.id = sim_queue.sim_id " +
                            " AND product.product_type = product_type.id" +
							" AND product.product_id = results_summary_by_product_channel.product_id " +
							" AND channel.channel_id = results_summary_by_product_channel.channel_id";

						tableAdapter.Fill(grid.Table);
						break;

                    case "SimSummaryByProdSeg":
                        tableAdapter.SelectCommand.CommandText =
                            "SELECT simulation.name as simulation, sim_queue.name as sim, product.product_name,  product_type.type_name, segment.segment_name, " +
                            " num_units, num_dollars, results_summary_by_product_segment.eq_units, volume, unit_share, dollar_share " +
                            " FROM results_summary_by_product_segment, simulation, sim_queue, product, segment, product_type  " +
                            " WHERE results_summary_by_product_segment.run_id = " + runID +
                            " AND sim_queue.run_id = " + runID +
                            " AND simulation.id = sim_queue.sim_id " +
                            " AND product.product_type = product_type.id" +
                            " AND product.product_id = results_summary_by_product_segment.product_id " +
                            " AND segment.segment_id = results_summary_by_product_segment.segment_id";

                        tableAdapter.Fill(grid.Table);
                        break;

                    case "SimSummaryByChanSeg":
                        tableAdapter.SelectCommand.CommandText =
                            "SELECT simulation.name as simulation, sim_queue.name as sim, channel.channel_name, segment.segment_name, " +
                            " num_units, num_dollars, results_summary_by_channel_segment.eq_units, volume, unit_share, dollar_share " +
                            " FROM results_summary_by_channel_segment, simulation, sim_queue, channel, segment  " +
                            " WHERE results_summary_by_channel_segment.run_id = " + runID +
                            " AND sim_queue.run_id = " + runID +
                            " AND simulation.id = sim_queue.sim_id " +
                            " AND channel.channel_id = results_summary_by_channel_segment.channel_id " +
                            " AND segment.segment_id = results_summary_by_channel_segment.segment_id";

                        tableAdapter.Fill(grid.Table);
                        break;

					case "SimSummaryByProdChanSeg":
						tableAdapter.SelectCommand.CommandText =
                            "SELECT simulation.name as simulation, sim_queue.name as sim, product.product_name,  product_type.type_name, channel.channel_name, segment.segment_name, " +
                            " num_units, num_dollars, results_summary_by_product_channel_segment.eq_units, volume, unit_share, dollar_share " +
                            " FROM results_summary_by_product_channel_segment, simulation, sim_queue, product, channel, segment, product_type  " +
							" WHERE results_summary_by_product_channel_segment.run_id = " + runID +
							" AND sim_queue.run_id = " + runID +
                            " AND simulation.id = sim_queue.sim_id " +
                            " AND product.product_type = product_type.id" +
							" AND product.product_id = results_summary_by_product_channel_segment.product_id " +
							" AND channel.channel_id = results_summary_by_product_channel_segment.channel_id" +
							" AND segment.segment_id = results_summary_by_product_channel_segment.segment_id";


						tableAdapter.Fill(grid.Table);
						break;

					case "CalSummary":
						tableAdapter.SelectCommand.CommandText = 
							"SELECT simulation.name as simulation, sim_queue.name as sim, mape" +
							" FROM results_calibration_total, simulation, sim_queue " +
							" WHERE results_calibration_total.run_id = " + runID +
							" AND sim_queue.run_id = " + runID +
							" AND simulation.id = sim_queue.sim_id";

						tableAdapter.Fill(grid.Table);
						break;

					case "CalSummaryByProd":
						tableAdapter.SelectCommand.CommandText =
                            "SELECT simulation.name as simulation, sim_queue.name as sim, product.product_name,  product_type.type_name, " +
							"  mape, sim_share, real_share, share_diff, percent_share_error " +
                            " FROM results_calibration_by_product, simulation, sim_queue, product, product_type  " +
							" WHERE results_calibration_by_product.run_id = " + runID +
                            " AND sim_queue.run_id = " + runID +
                            " AND product.product_type = product_type.id" +
							" AND simulation.id = sim_queue.sim_id" +
							" AND product.product_id = results_calibration_by_product.product_id";

						tableAdapter.Fill(grid.Table);
						break;

					case "CalSummaryByProdChan":
						tableAdapter.SelectCommand.CommandText =
                            "SELECT simulation.name as simulation, sim_queue.name as sim, product.product_name,  product_type.type_name, channel.channel_name, " +
							"  sim_share, real_share, share_diff, percent_share_error " +
                            " FROM results_calibration_by_product_channel, simulation, sim_queue, product, channel, product_type  " +
							" WHERE results_calibration_by_product_channel.run_id = " + runID +
							" AND sim_queue.run_id = " + runID +
                            " AND simulation.id = sim_queue.sim_id " +
                            " AND product.product_type = product_type.id" +
							" AND product.product_id = results_calibration_by_product_channel.product_id " +
							" AND channel.channel_id = results_calibration_by_product_channel.channel_id";

						tableAdapter.Fill(grid.Table);
						break;

					case "CalSummaryByChan":
						tableAdapter.SelectCommand.CommandText = 
							"SELECT simulation.name as simulation, sim_queue.name as sim, channel.channel_name, " +
							"  mape " +
							" FROM results_calibration_by_channel, simulation, sim_queue, channel " +
							" WHERE results_calibration_by_channel.run_id = " + runID +
							" AND sim_queue.run_id = " + runID +
							" AND simulation.id = sim_queue.sim_id " +
							" AND channel.channel_id = results_calibration_by_channel.channel_id";

						tableAdapter.Fill(grid.Table);
						break;

                    case "CalMAPEByProdChan":
                        tableAdapter.SelectCommand.CommandText =
                           "SELECT simulation.name as simulation, sim_queue.name as sim, product.product_name,  product_type.type_name, channel.channel_name, " +
                            "  mape " +
                            " FROM results_mape_by_product_channel, simulation, sim_queue, product, channel, product_type" +
                            " WHERE results_mape_by_product_channel.run_id = " + runID +
                            " AND sim_queue.run_id = " + runID +
                            " AND simulation.id = sim_queue.sim_id " +
                            " AND product.product_type = product_type.id" +
                            " AND product.product_id = results_mape_by_product_channel.product_id " +
                            " AND channel.channel_id = results_mape_by_product_channel.channel_id";

                        tableAdapter.Fill(grid.Table);
                        break;
				}

				grid.Suspend = false;
			}
		}

		public System.Data.OleDb.OleDbConnection Connection
		{
			set

			{
				tableAdapter.SelectCommand.Connection = value;
			}
		}
		/// <summary>
		/// Required designer variable.
		/// </summary>

		private System.ComponentModel.Container components = null;

		public Summary(Metric[] metrics)
		{
			//
			// Required for Windows Form Designer support
			//

			InitializeComponent();	

			tableAdapter = new System.Data.OleDb.OleDbDataAdapter();

			tableAdapter.SelectCommand = new System.Data.OleDb.OleDbCommand();

			// count number off grids

			// configure gridViews
			configureGrids(metrics);
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}

			base.Dispose( disposing );
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>

		private void InitializeComponent()

		{
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.tileItem = new System.Windows.Forms.MenuItem();
			this.tileVItem = new System.Windows.Forms.MenuItem();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MdiList = true;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.tileItem,
																					  this.tileVItem});
			this.menuItem1.Text = "Window";
			// 
			// tileItem
			// 
			this.tileItem.Index = 0;
			this.tileItem.Text = "Tile Horizontal";
			this.tileItem.Click += new System.EventHandler(this.tileItem_Click);
			// 
			// tileVItem
			// 
			this.tileVItem.Index = 1;
			this.tileVItem.Text = "Tile Vertical";
			this.tileVItem.Click += new System.EventHandler(this.tileVItem_Click);
			// 
			// Summary
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(880, 374);
			this.IsMdiContainer = true;
			this.Menu = this.mainMenu1;
			this.Name = "Summary";
			this.Text = "Summary";

		}

		#endregion

		private void configureGrids(Metric[] metrics)
		{
			GridView grid = null;
			DataTable tbl = null;

			foreach(Metric metric in metrics)
			{
				switch(metric.Token)
				{
					case "SimSummary":
						grid = new GridView();
						grid.MdiParent = this;

						grid.CreateTableStyle +=new MarketSimUtilities.GridView.TableStyle(createSimSummaryTableStyle);

                        tbl = new DataTable(metric.Token);

						tbl.Columns.Add("simulation", typeof(string));
						tbl.Columns.Add("sim", typeof(string));
						tbl.Columns.Add("num_units", typeof(double));
						tbl.Columns.Add("eq_units", typeof(double));	
						tbl.Columns.Add("num_dollars", typeof(double));	
						tbl.Columns.Add("volume", typeof(double));	

						grid.Table = tbl;

						grid.Text = metric.ToString();

						// set tablestyle for sim summary

						grid.Show();
						break;
			
					case "SimSummaryByProd":
						grid = new GridView();
						grid.MdiParent = this;

						grid.CreateTableStyle +=new MarketSimUtilities.GridView.TableStyle(createSimSummaryByProdTableStyle);

                        tbl = new DataTable(metric.Token);

						tbl.Columns.Add("simulation", typeof(string));
						tbl.Columns.Add("sim", typeof(string));
						tbl.Columns.Add("product_name", typeof(string));
                        tbl.Columns.Add("type_name", typeof(string));
						tbl.Columns.Add("num_units", typeof(double));
                        tbl.Columns.Add( "eq_units", typeof( double ) );
						tbl.Columns.Add("num_dollars", typeof(double));
						tbl.Columns.Add("dollar_share", typeof(double));		
                        tbl.Columns.Add( "unit_share", typeof( double ) );
                        tbl.Columns.Add( "volume", typeof( double ) );

						grid.Table = tbl;

						grid.Text = "Simulation Summary by Product";

						// set tablestyle for sim summary

						grid.Show();
						break;

					case "SimSummaryByProdChan":

						grid = new GridView();
						grid.MdiParent = this;

						grid.CreateTableStyle +=new MarketSimUtilities.GridView.TableStyle(createSimSummaryByProdChanTableStyle);

                        tbl = new DataTable(metric.Token);

						tbl.Columns.Add("simulation", typeof(string));
						tbl.Columns.Add("sim", typeof(string));
                        tbl.Columns.Add("product_name", typeof(string));
                        tbl.Columns.Add("type_name", typeof(string));
						tbl.Columns.Add("channel_name", typeof(string));
						tbl.Columns.Add("num_units", typeof(double));
                        tbl.Columns.Add( "eq_units", typeof( double ) );
						tbl.Columns.Add("num_dollars", typeof(double));
                        tbl.Columns.Add( "unit_share", typeof( double ) );
						tbl.Columns.Add("dollar_share", typeof(double));		
                        tbl.Columns.Add( "volume", typeof( double ) );

						grid.Table = tbl;

						grid.Text = "Simulation Summary by Product and Channel";

						// set tablestyle for sim summary

						grid.Show();
						break;

                    case "SimSummaryByProdSeg":

                        grid = new GridView();
                        grid.MdiParent = this;

                        grid.CreateTableStyle += new MarketSimUtilities.GridView.TableStyle(createSimSummaryByProdSegTableStyle);

                        tbl = new DataTable(metric.Token);

                        tbl.Columns.Add("simulation", typeof(string));
                        tbl.Columns.Add("sim", typeof(string));
                        tbl.Columns.Add("product_name", typeof(string));
                        tbl.Columns.Add("type_name", typeof(string));
                        tbl.Columns.Add("segment_name", typeof(string));
                        tbl.Columns.Add("num_units", typeof(double));
                        tbl.Columns.Add( "eq_units", typeof( double ) );
                        tbl.Columns.Add("num_dollars", typeof(double));
                        tbl.Columns.Add( "unit_share", typeof( double ) );
                        tbl.Columns.Add("dollar_share", typeof(double));
                        tbl.Columns.Add( "volume", typeof( double ) );

                        grid.Table = tbl;

                        grid.Text = "Simulation Summary by Product and Segment";

                        // set tablestyle for sim summary

                        grid.Show();
                        break;

                    case "SimSummaryByChanSeg":

                        grid = new GridView();
                        grid.MdiParent = this;

                        grid.CreateTableStyle += new MarketSimUtilities.GridView.TableStyle(createSimSummaryByChanSegTableStyle);

                        tbl = new DataTable(metric.Token);

                        tbl.Columns.Add("simulation", typeof(string));
                        tbl.Columns.Add("sim", typeof(string));
                        tbl.Columns.Add("channel_name", typeof(string));
                        tbl.Columns.Add("segment_name", typeof(string));
                        tbl.Columns.Add("num_units", typeof(double));
                        tbl.Columns.Add( "eq_units", typeof( double ) );
                        tbl.Columns.Add( "num_dollars", typeof( double ) );
                        tbl.Columns.Add("unit_share", typeof(double));
                        tbl.Columns.Add("dollar_share", typeof(double));
                        tbl.Columns.Add( "volume", typeof( double ) );

                        grid.Table = tbl;

                        grid.Text = "Simulation Summary by Channel and Segment";

                        // set tablestyle for sim summary

                        grid.Show();
                        break;

					case "SimSummaryByProdChanSeg":

						grid = new GridView();
						grid.MdiParent = this;

						grid.CreateTableStyle +=new MarketSimUtilities.GridView.TableStyle(createSimSummaryByProdChanSegTableStyle);

                        tbl = new DataTable(metric.Token);

						tbl.Columns.Add("simulation", typeof(string));
						tbl.Columns.Add("sim", typeof(string));
                        tbl.Columns.Add("product_name", typeof(string));
                        tbl.Columns.Add("type_name", typeof(string));
						tbl.Columns.Add("channel_name", typeof(string));
						tbl.Columns.Add("segment_name", typeof(string));
						tbl.Columns.Add("num_units", typeof(double));
                        tbl.Columns.Add( "eq_units", typeof( double ) );
                        tbl.Columns.Add( "num_dollars", typeof( double ) );	
						tbl.Columns.Add("unit_share", typeof(double));
						tbl.Columns.Add("dollar_share", typeof(double));
                        tbl.Columns.Add( "volume", typeof( double ) );

						grid.Table = tbl;

						grid.Text = "Simulation Summary by Product, Channel, and Segment";

						// set tablestyle for sim summary

						grid.Show();
						break;

					case "CalSummary":

						grid = new GridView();
						grid.MdiParent = this;

						grid.CreateTableStyle +=new MarketSimUtilities.GridView.TableStyle(createCalSummaryTableStyle);

                        tbl = new DataTable(metric.Token);

						tbl.Columns.Add("simulation", typeof(string));
						tbl.Columns.Add("sim", typeof(string));
						tbl.Columns.Add("mape", typeof(double));

						grid.Table = tbl;

						grid.Text = "MAPE total";

						// set tablestyle for sim summary

						grid.Show();
						break;

					case "CalSummaryByProd":

						grid = new GridView();
						grid.MdiParent = this;

						grid.CreateTableStyle +=new MarketSimUtilities.GridView.TableStyle(createCalSummaryByProdTableStyle);

                        tbl = new DataTable(metric.Token);

						tbl.Columns.Add("simulation", typeof(string));
						tbl.Columns.Add("sim", typeof(string));
                        tbl.Columns.Add("product_name", typeof(string));
                        tbl.Columns.Add("type_name", typeof(string));
						tbl.Columns.Add("mape", typeof(double));
						tbl.Columns.Add("sim_share", typeof(double));	
						tbl.Columns.Add("real_share", typeof(double));
						tbl.Columns.Add("percent_share_error", typeof(double));		

						grid.Table = tbl;

						grid.Text = "Calibration by Product";

						// set tablestyle for sim summary

						grid.Show();
						break;

					case "CalSummaryByProdChan":

						grid = new GridView();
						grid.MdiParent = this;

						grid.CreateTableStyle +=new MarketSimUtilities.GridView.TableStyle(createCalSummaryByProdChanTableStyle);

                        tbl = new DataTable(metric.Token);

						tbl.Columns.Add("simulation", typeof(string));
						tbl.Columns.Add("sim", typeof(string));
                        tbl.Columns.Add("product_name", typeof(string));
                        tbl.Columns.Add("type_name", typeof(string));
						tbl.Columns.Add("channel_name", typeof(string));
						tbl.Columns.Add("mape", typeof(double));
						tbl.Columns.Add("sim_share", typeof(double));	
						tbl.Columns.Add("real_share", typeof(double));
						tbl.Columns.Add("percent_share_error", typeof(double));

						grid.Table = tbl;

						grid.Text = "Calibration by Product and Channel";

						// set tablestyle for sim summary

						grid.Show();
						break;

					case "CalSummaryByChan":

						grid = new GridView();
						grid.MdiParent = this;

						grid.CreateTableStyle +=new MarketSimUtilities.GridView.TableStyle(createCalSummaryByChanTableStyle);

                        tbl = new DataTable(metric.Token);

						tbl.Columns.Add("simulation", typeof(string));
						tbl.Columns.Add("sim", typeof(string));
						tbl.Columns.Add("channel_name", typeof(string));
						tbl.Columns.Add("mape", typeof(double));

						grid.Table = tbl;

						grid.Text = "MAPE by Channel";

						// set tablestyle for sim summary

						grid.Show();
						break;

                    case "CalMAPEByProdChan":

                        grid = new GridView();
                        grid.MdiParent = this;

                        grid.CreateTableStyle += new MarketSimUtilities.GridView.TableStyle(createCalMAPEByProdChanTableStyle);

                        tbl = new DataTable(metric.Token);

                        tbl.Columns.Add("simulation", typeof(string));
                        tbl.Columns.Add("sim", typeof(string));
                        tbl.Columns.Add("product_name", typeof(string));
                        tbl.Columns.Add("type_name", typeof(string));
                        tbl.Columns.Add("channel_name", typeof(string));
                        tbl.Columns.Add("mape", typeof(double));

                        grid.Table = tbl;

                        grid.Text = "MAPE by Product and Channel";

                        // set tablestyle for sim summary

                        grid.Show();
                        break;
				}
			}
        }

        #region table styles
        private void createSimSummaryTableStyle(MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddTextColumn("simulation", "Simulation", true);
            grid.AddTextColumn("sim", "Run", true);
			grid.AddNumericColumn("num_units", "Units", true);
            grid.AddNumericColumn( "num_dollars", "Dollars", true );
            grid.AddNumericColumn( "eq_units", "Eq Units", true );
            grid.AddNumericColumn( "volume", "Volume", true );

			grid.Reset();
		}

		private void createSimSummaryByProdTableStyle(MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddTextColumn("simulation", "Simulation", true);
			grid.AddTextColumn("sim", "Run", true);
			grid.AddTextColumn("product_name", "Product", true);
            grid.AddTextColumn("type_name", "Type", true);
			grid.AddNumericColumn("num_units", "Units", true);
			grid.AddNumericColumn("unit_share", "Unit Share", true);
			grid.AddNumericColumn("num_dollars", "Dollars", true);
            grid.AddNumericColumn( "eq_units", "Eq Units", true );
            grid.AddNumericColumn( "volume", "Volume", true );
            grid.AddNumericColumn( "dollar_share", "Dollar Share", true );

			grid.Reset();
		}

		private void createSimSummaryByProdChanTableStyle(MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddTextColumn("simulation", "Simulation", true);
            grid.AddTextColumn("sim", "Run", true);
            grid.AddTextColumn("product_name", "Product", true);
            grid.AddTextColumn("type_name", "Type", true);
			grid.AddTextColumn("channel_name", "Channel", true);
			grid.AddNumericColumn("num_units", "Units", true);
			grid.AddNumericColumn("unit_share", "Unit Share", true);
			grid.AddNumericColumn("num_dollars", "Dollars", true);
            grid.AddNumericColumn( "eq_units", "Eq Units", true );
            grid.AddNumericColumn( "volume", "Volume", true );
            grid.AddNumericColumn( "dollar_share", "Dollar Share", true );

			grid.Reset();
		}


        private void createSimSummaryByProdSegTableStyle(MrktSimGrid grid)
        {
            grid.Clear();

            grid.AddTextColumn("simulation", "Simulation", true);
            grid.AddTextColumn("sim", "Run", true);
            grid.AddTextColumn("product_name", "Product", true);
            grid.AddTextColumn("type_name", "Type", true);
            grid.AddTextColumn("segment_name", "Segment", true);
            grid.AddNumericColumn("num_units", "Units", true);
            grid.AddNumericColumn("unit_share", "Unit Share", true);
            grid.AddNumericColumn("num_dollars", "Dollars", true);
            grid.AddNumericColumn( "eq_units", "Eq Units", true );
            grid.AddNumericColumn( "volume", "Volume", true );
            grid.AddNumericColumn("dollar_share", "Dollar Share", true);

            grid.Reset();
        }

        private void createSimSummaryByChanSegTableStyle(MrktSimGrid grid)
        {
            grid.Clear();

            grid.AddTextColumn("simulation", "Simulation", true);
            grid.AddTextColumn("sim", "Run", true);
            grid.AddTextColumn("channel_name", "Channel", true);
            grid.AddTextColumn("segment_name", "Segment", true);
            grid.AddNumericColumn("num_units", "Units", true);
            grid.AddNumericColumn("unit_share", "Unit Share", true);
            grid.AddNumericColumn("num_dollars", "Dollars", true);
            grid.AddNumericColumn( "eq_units", "Eq Units", true );
            grid.AddNumericColumn( "volume", "Volume", true );
            grid.AddNumericColumn( "dollar_share", "Dollar Share", true );

            grid.Reset();
        }

		private void createSimSummaryByProdChanSegTableStyle(MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddTextColumn("simulation", "Simulation", true);
            grid.AddTextColumn("sim", "Run", true);
            grid.AddTextColumn("product_name", "Product", true);
            grid.AddTextColumn("type_name", "Type", true);
			grid.AddTextColumn("channel_name", "Channel", true);
			grid.AddTextColumn("segment_name", "Segment", true);
			grid.AddNumericColumn("num_units", "Units", true);
			grid.AddNumericColumn("unit_share", "Unit Share", true);
			grid.AddNumericColumn("num_dollars", "Dollars", true);
            grid.AddNumericColumn( "eq_units", "Eq Units", true );
            grid.AddNumericColumn( "volume", "Volume", true );
            grid.AddNumericColumn( "dollar_share", "Dollar Share", true );

			grid.Reset();
		}

		// calibration
		private void createCalSummaryTableStyle(MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddTextColumn("simulation", "Simulation", true);
            grid.AddTextColumn("sim", "Run", true);
			grid.AddNumericColumn("mape", "MAPE", true);

			grid.Reset();
		}

		private void createCalSummaryByProdTableStyle(MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddTextColumn("simulation", "Simulation", true);
            grid.AddTextColumn("sim", "Run", true);
			grid.AddTextColumn("product_name", "Product", true);
            grid.AddTextColumn("type_name", "Type", true);
			grid.AddNumericColumn("sim_share", "Sim Share", true);
			grid.AddNumericColumn("real_share", "Real Share", true);
			grid.AddNumericColumn("share_diff", "Excess Share", true);
			grid.AddNumericColumn("percent_share_error", "Percent Error", true);
			grid.AddNumericColumn("mape", "MAPE", true);

			grid.Reset();
		}

		private void createCalSummaryByProdChanTableStyle(MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddTextColumn("simulation", "Simulation", true);
            grid.AddTextColumn("sim", "Run", true);
            grid.AddTextColumn("product_name", "Product", true);
            grid.AddTextColumn("type_name", "Type", true);
			grid.AddTextColumn("channel_name", "Channel", true);
			grid.AddNumericColumn("sim_share", "Sim Share", true);
			grid.AddNumericColumn("real_share", "Real Share", true);
			grid.AddNumericColumn("share_diff", "Excess Share", true);
			grid.AddNumericColumn("percent_share_error", "Percent Error", true);

			grid.Reset();
		}

        private void createCalMAPEByProdChanTableStyle(MrktSimGrid grid)
        {
            grid.Clear();

            grid.AddTextColumn("simulation", "Simulation", true);
            grid.AddTextColumn("sim", "Run", true);
            grid.AddTextColumn("product_name", "Product", true);
            grid.AddTextColumn("type_name", "Type", true);
            grid.AddTextColumn("channel_name", "Channel", true);
            grid.AddNumericColumn("mape", "MAPE", true);

            grid.Reset();
        }

		private void createCalSummaryByChanTableStyle(MrktSimGrid grid)
		{
			grid.Clear();

			grid.AddTextColumn("simulation", "Simulation", true);
            grid.AddTextColumn("sim", "Run", true);
			grid.AddTextColumn("channel_name", "Channel", true);
			grid.AddNumericColumn("mape", "MAPE", true);

			grid.Reset();
        }
        #endregion


        private void tileItem_Click(object sender, System.EventArgs e)
		{
			this.LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void tileVItem_Click(object sender, System.EventArgs e)
		{
		this.LayoutMdi(MdiLayout.TileVertical);
		}
	}
}

