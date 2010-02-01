using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for AddCompetitor.
	/// </summary>
	public class AddCompetitor : System.Windows.Forms.UserControl, Wizard
	{
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddCompetitor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Papyrus", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(96, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(272, 200);
			this.label1.TabIndex = 0;
			this.label1.Text = "Under Construction";
			// 
			// AddCompetitor
			// 
			this.Controls.Add(this.label1);
			this.Name = "AddCompetitor";
			this.Size = new System.Drawing.Size(440, 248);
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
