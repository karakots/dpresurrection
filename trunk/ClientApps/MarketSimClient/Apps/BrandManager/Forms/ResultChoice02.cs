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
	public class ResultChoice02 : System.Windows.Forms.UserControl, Wizard
	{
		private System.Windows.Forms.RadioButton NoParameters;
		private System.Windows.Forms.RadioButton YesParameters;
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ResultChoice02()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			NoParameters.Checked = true;

		}

		public bool Yes
		{
			get
			{
				if(YesParameters.Checked)
				{
					return true;
				}
				return false;
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
			this.NoParameters = new System.Windows.Forms.RadioButton();
			this.YesParameters = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// NoParameters
			// 
			this.NoParameters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.NoParameters.Location = new System.Drawing.Point(152, 152);
			this.NoParameters.Name = "NoParameters";
			this.NoParameters.Size = new System.Drawing.Size(240, 24);
			this.NoParameters.TabIndex = 0;
			this.NoParameters.Text = "No";
			// 
			// YesParameters
			// 
			this.YesParameters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.YesParameters.Location = new System.Drawing.Point(152, 120);
			this.YesParameters.Name = "YesParameters";
			this.YesParameters.Size = new System.Drawing.Size(256, 24);
			this.YesParameters.TabIndex = 1;
			this.YesParameters.Text = "View time series plots";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(504, 64);
			this.label1.TabIndex = 2;
			this.label1.Text = "Would you like to write your results to file?";
			// 
			// ResultChoice02
			// 
			this.Controls.Add(this.label1);
			this.Controls.Add(this.YesParameters);
			this.Controls.Add(this.NoParameters);
			this.Name = "ResultChoice02";
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
			this.NoParameters.Checked = true;
			this.YesParameters.Checked = false;
		}

		public void End()
		{
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

	}
}
