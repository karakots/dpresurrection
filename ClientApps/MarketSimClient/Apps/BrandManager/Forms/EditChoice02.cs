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
	public class EditChoice02 : System.Windows.Forms.UserControl, Wizard
	{
		private System.Windows.Forms.RadioButton NoParameters;
		private System.Windows.Forms.RadioButton YesParameters;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public EditChoice02()
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
			this.label2 = new System.Windows.Forms.Label();
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
			this.YesParameters.Text = "Yes";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(504, 32);
			this.label1.TabIndex = 2;
			this.label1.Text = "Would you like to see the advanced scenario options?";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(24, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(504, 24);
			this.label2.TabIndex = 3;
			this.label2.Text = "These options include how often results are written to the database";
			// 
			// EditChoice02
			// 
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.YesParameters);
			this.Controls.Add(this.NoParameters);
			this.Name = "EditChoice02";
			this.Size = new System.Drawing.Size(552, 440);
			this.ResumeLayout(false);

		}
		#endregion

		#region Wizard Members

		public bool Next()
		{
			// TODO:  Add EditChoice02.Next implementation
			return true;
		}

		public bool Back()
		{
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
