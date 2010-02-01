using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MrktSimDb;

using MarketSimUtilities;

namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for SimLog.
	/// </summary>
	public class SimLog : System.Windows.Forms.Form
	{
        SimLogDb theDb;
		public SimLogDb Db
		{
			set
			{
                theDb = value;
                simLogGrid.Table = theDb.Data.run_log;
                simLogGrid.DescriptionWindow = false;
                simLogGrid.AllowDelete = false;

                createTableStyle();
			}
		}

        //public int Run 
        //{
        //    set
        //    {
        //        runID = value;
        //        reset();
        //    }

        //    get
        //    {
        //        return runID;
        //    }
        //}

		private MrktSimGrid simLogGrid;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SimLog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		
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
			this.simLogGrid = new MrktSimGrid();
			this.SuspendLayout();
			// 
			// simLogGrid
			// 
			this.simLogGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simLogGrid.Location = new System.Drawing.Point(0, 0);
			this.simLogGrid.Name = "simLogGrid";
			this.simLogGrid.RowFilter = null;
			this.simLogGrid.RowID = null;
			this.simLogGrid.RowName = null;
			this.simLogGrid.Size = new System.Drawing.Size(376, 238);
			this.simLogGrid.Sort = "";
			this.simLogGrid.TabIndex = 0;
			// 
			// SimLog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(376, 238);
			this.Controls.Add(this.simLogGrid);
			this.Name = "SimLog";
			this.Text = "SimLog";
			this.ResumeLayout(false);

		}
		#endregion


		private void createTableStyle()
		{
			simLogGrid.Clear();

			simLogGrid.AddDateColumn("calendar_date", true);
			simLogGrid.AddComboBoxColumn("segment_id", theDb.Data.segment, "segment_name", "segment_id", true);
			simLogGrid.AddComboBoxColumn("product_id", theDb.Data.product, "product_name", "product_id", true);
			simLogGrid.AddComboBoxColumn("channel_id", theDb.Data.channel, "channel_name", "channel_id", true);
			simLogGrid.AddTextColumn("message", true);
			
			simLogGrid.Reset();
		}
	}
}
