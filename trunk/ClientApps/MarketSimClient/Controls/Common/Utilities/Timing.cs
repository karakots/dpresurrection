using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Common.Utilities
{
	/// <summary>
	/// Summary description for Timing.
	/// </summary>
	public class Timing : System.Windows.Forms.UserControl
	{
		public bool Interval
		{
			get
			{
				return timingButton.Checked;
			}
		}

		public int NumRows
		{
			get
			{
				return (int) numericUpDown.Value;
			}
		}

		public enum TimingType
		{
			None = 0,
			Daily,
			Weekly,
			Monthly,
			Yearly
		}

		public TimingType Type
		{
			get
			{
				return (TimingType) timingBox.SelectedIndex;
			}
		}

			public TimeSpan TimeInterval
		{
			get
			{
				switch (timingBox.SelectedIndex)
				{
					case 1:
						return new TimeSpan(1,0,0,0,0);
						
					case 2:
						return new TimeSpan(7,0,0,0,0);
						
					case 3:
						return new TimeSpan(31,0,0,0,0);
						
					case 4:
						return new TimeSpan(366,0,0,0,0);
						
				}

				return new TimeSpan(0,0,0,0,0);
			}
		}

		private System.Windows.Forms.ComboBox timingBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numericUpDown;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton numButton;
		private System.Windows.Forms.RadioButton timingButton;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Timing()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			timingButton.Checked = true;
			timingBox.SelectedIndex = 0;

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
			this.timingBox = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.numericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.numButton = new System.Windows.Forms.RadioButton();
			this.timingButton = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// timingBox
			// 
			this.timingBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.timingBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.timingBox.Items.AddRange(new object[] {
														   "None",
														   "Daily",
														   "Weekly",
														   "Monthly",
														   "Yearly"});
			this.timingBox.Location = new System.Drawing.Point(82, 8);
			this.timingBox.Name = "timingBox";
			this.timingBox.Size = new System.Drawing.Size(96, 21);
			this.timingBox.TabIndex = 18;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(28, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 16);
			this.label4.TabIndex = 19;
			this.label4.Text = "Timing";
			// 
			// numericUpDown
			// 
			this.numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDown.Location = new System.Drawing.Point(80, 40);
			this.numericUpDown.Maximum = new System.Decimal(new int[] {
																		  1000,
																		  0,
																		  0,
																		  0});
			this.numericUpDown.Minimum = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.numericUpDown.Name = "numericUpDown";
			this.numericUpDown.Size = new System.Drawing.Size(56, 20);
			this.numericUpDown.TabIndex = 23;
			this.numericUpDown.Value = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(28, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 22;
			this.label1.Text = "Number";
			// 
			// numButton
			// 
			this.numButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.numButton.Location = new System.Drawing.Point(8, 40);
			this.numButton.Name = "numButton";
			this.numButton.Size = new System.Drawing.Size(16, 16);
			this.numButton.TabIndex = 21;
			// 
			// timingButton
			// 
			this.timingButton.Location = new System.Drawing.Point(8, 8);
			this.timingButton.Name = "timingButton";
			this.timingButton.Size = new System.Drawing.Size(16, 16);
			this.timingButton.TabIndex = 20;
			this.timingButton.CheckedChanged += new System.EventHandler(this.timingButton_CheckedChanged);
			// 
			// Timing
			// 
			this.Controls.Add(this.numericUpDown);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numButton);
			this.Controls.Add(this.timingButton);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.timingBox);
			this.Name = "Timing";
			this.Size = new System.Drawing.Size(176, 64);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void timingButton_CheckedChanged(object sender, System.EventArgs e)
		{
			if (timingButton.Checked)
			{
				timingBox.Enabled = true;
				numericUpDown.Enabled = false;
			}
			else if (numButton.Checked)
			{
				timingBox.Enabled = false;
				numericUpDown.Enabled = true;
			}
		
		}
	}
}
