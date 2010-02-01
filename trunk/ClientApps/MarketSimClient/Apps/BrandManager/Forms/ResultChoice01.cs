using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for EditChoice01.
	/// </summary>
	public class ResultChoice01 : System.Windows.Forms.UserControl, Wizard
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton writeToDisk;
		private System.Windows.Forms.RadioButton timeSeries;
		private System.Windows.Forms.ToolTip ViewPlotToolTip;
		private System.ComponentModel.IContainer components;

		public ResultChoice01()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			timeSeries.Checked = true;
		}

		public bool TimeSeries
		{
			get
			{
				return this.timeSeries.Checked;
			}
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
			this.components = new System.ComponentModel.Container();
			this.writeToDisk = new System.Windows.Forms.RadioButton();
			this.timeSeries = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.ViewPlotToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// writeToDisk
			// 
			this.writeToDisk.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.writeToDisk.Location = new System.Drawing.Point(152, 152);
			this.writeToDisk.Name = "writeToDisk";
			this.writeToDisk.Size = new System.Drawing.Size(240, 24);
			this.writeToDisk.TabIndex = 0;
			this.writeToDisk.Text = "Write To File";
			this.ViewPlotToolTip.SetToolTip(this.writeToDisk, "Write the results of all completed and running scenarios to a CSV file");
			// 
			// timeSeries
			// 
			this.timeSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.timeSeries.Location = new System.Drawing.Point(152, 120);
			this.timeSeries.Name = "timeSeries";
			this.timeSeries.Size = new System.Drawing.Size(256, 24);
			this.timeSeries.TabIndex = 1;
			this.timeSeries.Text = "View Plots";
			this.ViewPlotToolTip.SetToolTip(this.timeSeries, "View the results of all completed and running scenarios as time series plots.");
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(504, 32);
			this.label1.TabIndex = 2;
			this.label1.Text = "Please select the type of results to view?";
			// 
			// ResultChoice01
			// 
			this.Controls.Add(this.label1);
			this.Controls.Add(this.timeSeries);
			this.Controls.Add(this.writeToDisk);
			this.Name = "ResultChoice01";
			this.Size = new System.Drawing.Size(552, 440);
			this.ResumeLayout(false);

		}
		#endregion

		#region Wizard Members

		public bool Next()
		{
			// TODO:  Add EditChoice03.Next implementation
			return true;
		}

		public bool Back()
		{
			// TODO:  Add EditChoice03.Back implementation
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

	}
}
