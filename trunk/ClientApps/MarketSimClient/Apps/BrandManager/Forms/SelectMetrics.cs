using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
using MrktSimDb.Metrics;


namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for SelectMetrics.
	/// </summary>
	public class SelectMetrics : System.Windows.Forms.UserControl, Wizard
	{
		private System.Windows.Forms.CheckedListBox metricList;
		private System.Windows.Forms.GroupBox groupBox1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SelectMetrics()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

//			foreach(Metric metric in Metric.ForcastTypes)
//			{
//				metricList.Items.Add(metric);
//			}
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.metricList = new System.Windows.Forms.CheckedListBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// metricList
			// 
			this.metricList.CheckOnClick = true;
			this.metricList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.metricList.Location = new System.Drawing.Point(3, 16);
			this.metricList.Name = "metricList";
			this.metricList.Size = new System.Drawing.Size(434, 304);
			this.metricList.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.metricList);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(440, 336);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Available Metrics";
			// 
			// SelectMetrics
			// 
			this.Controls.Add(this.groupBox1);
			this.Name = "SelectMetrics";
			this.Size = new System.Drawing.Size(440, 336);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		Database db = null;

		public Database Db
		{
			set
			{
				db = value;
			}

			get
			{
				return db;
			}
		}

		#region Wizard Members

		public bool Next()
		{
			return true;
		}

		public bool Back()
		{
			// TODO:  Add AddCompetitor.Back implementation
			return true;
		}

		public void Start()
		{
		}

		public void End()
		{
			string query = "user_name = '" + Setup.Settings.User + "'";
			DataRow[] rows = db.Data.scenario.Select(query, "", DataViewRowState.CurrentRows);
			foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in rows)
			{
				for(int ii = 0; ii < metricList.Items.Count; ++ii)
				{
					if (metricList.GetItemChecked(ii))
					{
						Db.CreateScenarioMetric(scenario, ((Metric) metricList.Items[ii]).Token);
					}
					else
					{
						Db.DeleteScenarioMetric(scenario, ((Metric) metricList.Items[ii]).Token);
					}
				}
			}
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

		#region UI
		#endregion

		/*public bool GotoNext()
		{
			if (this.saveResultsInFile.Checked &&
				(fileName.Text == null || fileName.Text.Length == 0))
			{
				return false;
			}

			if (itemsChecked)
				return true;

			return false;
		}*/

		/*bool itemsChecked = false;
		private void metricList_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			if (e.CurrentValue == CheckState.Unchecked || metricList.CheckedItems.Count > 1)
			{
				itemsChecked = true;
			}
			else if (e.CurrentValue == CheckState.Checked && metricList.CheckedItems.Count == 1)
			{
				itemsChecked = false;
			}

		}*/
	}
}
