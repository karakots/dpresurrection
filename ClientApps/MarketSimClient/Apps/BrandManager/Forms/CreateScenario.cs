using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

// using Common.Utilities;
using MrktSimDb;
using BrandManager.Utilities;
using MarketSimUtilities;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for CreateScenario.
	/// </summary>
	public class CreateScenario : System.Windows.Forms.UserControl, Wizard
	{
		private MrktSimGrid mrktSimGrid;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label5;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateScenario()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			mrktSimGrid.DescribeRow = "descr";
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
			this.mrktSimGrid = new MarketSimUtilities.MrktSimGrid();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mrktSimGrid
			// 
			this.mrktSimGrid.DescribeRow = null;
			this.mrktSimGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mrktSimGrid.EnabledGrid = true;
			this.mrktSimGrid.Location = new System.Drawing.Point(0, 56);
			this.mrktSimGrid.Name = "mrktSimGrid";
			this.mrktSimGrid.RowFilter = null;
			this.mrktSimGrid.RowID = null;
			this.mrktSimGrid.RowName = null;
			this.mrktSimGrid.Size = new System.Drawing.Size(472, 256);
			this.mrktSimGrid.Sort = "";
			this.mrktSimGrid.TabIndex = 13;
			this.mrktSimGrid.Table = null;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label5);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(472, 56);
			this.panel1.TabIndex = 14;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(24, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(432, 23);
			this.label5.TabIndex = 17;
			this.label5.Text = "Select Scenario  for Baseline";
			// 
			// CreateScenario
			// 
			this.Controls.Add(this.mrktSimGrid);
			this.Controls.Add(this.panel1);
			this.Name = "CreateScenario";
			this.Size = new System.Drawing.Size(472, 312);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Wizard Members

		public bool Next()
		{
			// TODO:  Add AddCompetitor.Next implementation
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
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

		#region Public emthods
		public Database Db
		{
			set
			{
				db = value;
				this.mrktSimGrid.Table = value.Data.scenario;

				mrktSimGrid.RowFilter = "sim_num >= 0 AND type = 0";

				createTableStyle();
			}

			get
			{
				return db;
			}
		}

		public MrktSimDb.MrktSimDBSchema.scenarioRow SelectedScenario
		{
			get
			{
				// get current scenario from grid
				DataRow row = this.mrktSimGrid.CurrentRow;

				if (row == null)
					return null;

				// construct name from doc name
				return (MrktSimDb.MrktSimDBSchema.scenarioRow) row;
			}
		}

		#endregion

		#region Member Data
		Database db = null;
		#endregion

		#region UI methods
		
		#endregion

		#region private methods

		private void createTableStyle()
		{
			this.mrktSimGrid.Clear();

			this.mrktSimGrid.AddTextColumn("name", "Scenario", true);
			this.mrktSimGrid.AddTextColumn("user_name", "Owner", true);

			this.mrktSimGrid.Reset();
		}
		

		private void createButton_Click(object sender, System.EventArgs e)
		{
			// get current scenario from grid
			DataRow row = this.mrktSimGrid.CurrentRow;

			if (row == null)
				return;

			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			openFileDlg.DefaultExt = ".xls";
			openFileDlg.Filter = "Excel File (*.xls)|*.xls";

			// openFileDlg.ReadOnlyChecked = false;
			DialogResult frslt = openFileDlg.ShowDialog();

			if (frslt == DialogResult.OK)
			{
				string fileName = openFileDlg.FileName;

				ScenarioUtilities.CopyScenario(Db, (MrktSimDb.MrktSimDBSchema.scenarioRow) row, fileName);
		
			}

		}


		#endregion
	}
}
