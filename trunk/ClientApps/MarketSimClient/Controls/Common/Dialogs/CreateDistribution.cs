using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for CreateDistribution.
	/// </summary>
	public class CreateDistribution : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.NumericUpDown attributeGBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown attributeFBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown persuasionBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown awarenessBox;
		private System.Windows.Forms.Label label2;
		private Common.Utilities.PlanData planData;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateDistribution()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.attributeGBox = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.attributeFBox = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.persuasionBox = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.awarenessBox = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.planData = new Common.Utilities.PlanData();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.attributeGBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.attributeFBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.attributeGBox);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.attributeFBox);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.persuasionBox);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.awarenessBox);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(16, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(240, 232);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Distribution Data";
			// 
			// attributeGBox
			// 
			this.attributeGBox.DecimalPlaces = 2;
			this.attributeGBox.Location = new System.Drawing.Point(144, 64);
			this.attributeGBox.Name = "attributeGBox";
			this.attributeGBox.Size = new System.Drawing.Size(72, 20);
			this.attributeGBox.TabIndex = 13;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(56, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 23);
			this.label1.TabIndex = 12;
			this.label1.Text = "% Considered";
			// 
			// attributeFBox
			// 
			this.attributeFBox.DecimalPlaces = 2;
			this.attributeFBox.Location = new System.Drawing.Point(144, 32);
			this.attributeFBox.Name = "attributeFBox";
			this.attributeFBox.Size = new System.Drawing.Size(72, 20);
			this.attributeFBox.TabIndex = 11;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(64, 32);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 23);
			this.label4.TabIndex = 10;
			this.label4.Text = "% Available";
			// 
			// persuasionBox
			// 
			this.persuasionBox.DecimalPlaces = 2;
			this.persuasionBox.Location = new System.Drawing.Point(144, 128);
			this.persuasionBox.Minimum = new System.Decimal(new int[] {
																		  100,
																		  0,
																		  0,
																		  -2147483648});
			this.persuasionBox.Name = "persuasionBox";
			this.persuasionBox.Size = new System.Drawing.Size(72, 20);
			this.persuasionBox.TabIndex = 9;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(72, 128);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 23);
			this.label3.TabIndex = 8;
			this.label3.Text = "Persuasion";
			// 
			// awarenessBox
			// 
			this.awarenessBox.DecimalPlaces = 2;
			this.awarenessBox.Increment = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   65536});
			this.awarenessBox.Location = new System.Drawing.Point(144, 96);
			this.awarenessBox.Maximum = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 0});
			this.awarenessBox.Name = "awarenessBox";
			this.awarenessBox.Size = new System.Drawing.Size(72, 20);
			this.awarenessBox.TabIndex = 7;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(48, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 23);
			this.label2.TabIndex = 6;
			this.label2.Text = "Awareness Prob.";
			// 
			// planData
			// 
			this.planData.Enabled = false;
			this.planData.Location = new System.Drawing.Point(280, 16);
			this.planData.Name = "planData";
			this.planData.Size = new System.Drawing.Size(320, 224);
			this.planData.Suspend = false;
			this.planData.TabIndex = 5;
			// 
			// CreateDistribution
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(608, 302);
			this.Controls.Add(this.planData);
			this.Controls.Add(this.groupBox1);
			this.Name = "CreateDistribution";
			this.Text = "CreateDistribution";
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.attributeGBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.attributeFBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.persuasionBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.awarenessBox)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
