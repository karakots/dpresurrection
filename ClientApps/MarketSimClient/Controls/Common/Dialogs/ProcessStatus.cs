using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for ProcessStatus.
	/// </summary>
	public class ProcessStatus : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label processNameLabel;
		private System.Windows.Forms.Label CurrentItemLabel;
		private System.Windows.Forms.ProgressBar progressBar1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProcessStatus()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			progressBar1.Maximum = MAX;
			progressBar1.Minimum = 0;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ProcessStatus));
			this.processNameLabel = new System.Windows.Forms.Label();
			this.CurrentItemLabel = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// processNameLabel
			// 
			this.processNameLabel.Location = new System.Drawing.Point(16, 8);
			this.processNameLabel.Name = "processNameLabel";
			this.processNameLabel.Size = new System.Drawing.Size(88, 16);
			this.processNameLabel.TabIndex = 0;
			this.processNameLabel.Text = "Current Process";
			// 
			// CurrentItemLabel
			// 
			this.CurrentItemLabel.Location = new System.Drawing.Point(112, 8);
			this.CurrentItemLabel.Name = "CurrentItemLabel";
			this.CurrentItemLabel.Size = new System.Drawing.Size(240, 32);
			this.CurrentItemLabel.TabIndex = 1;
			this.CurrentItemLabel.Text = "012345678901234567890123456789";
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(16, 48);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(328, 23);
			this.progressBar1.TabIndex = 2;
			// 
			// ProcessStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(362, 80);
			this.ControlBox = false;
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.CurrentItemLabel);
			this.Controls.Add(this.processNameLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "ProcessStatus";
			this.Text = "Processing";
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		#endregion

		const int MAX = 100;

		#region Public Properties

		public string ProcessType
		{
			set
			{
				processNameLabel.Text = value;
			}

			get
			{
				return processNameLabel.Text;
			}
		}

		public string CurrentProcess
		{
			set
			{
				CurrentItemLabel.Text = value;
			}

			get
			{
				return CurrentItemLabel.Text;
			}
		}

		public bool  Progress(string procName, double amount)
		{
			if (amount >= 1)
				amount = 1;
			else if ( amount < 0)
				amount = 0;

			progressBar1.Value = (int) Math.Floor( MAX * amount);	
		
			CurrentItemLabel.Text = procName;

			this.Refresh();

			return true;
		}

		#endregion
	}
}
