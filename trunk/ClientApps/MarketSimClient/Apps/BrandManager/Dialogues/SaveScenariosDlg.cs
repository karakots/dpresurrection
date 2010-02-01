using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MrktSimDb;

namespace BrandManager.Dialogues
{
	/// <summary>
	/// Summary description for SaveScenariosDlg.
	/// </summary>
	public class SaveScenariosDlg : System.Windows.Forms.Form
	{
		private BrandManager.Forms.SaveScenario saveScenario;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button doneButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SaveScenariosDlg()
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
			this.saveScenario = new BrandManager.Forms.SaveScenario();
			this.panel1 = new System.Windows.Forms.Panel();
			this.doneButton = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// saveScenario
			// 
			this.saveScenario.Db = null;
			this.saveScenario.Dock = System.Windows.Forms.DockStyle.Fill;
			this.saveScenario.Location = new System.Drawing.Point(0, 0);
			this.saveScenario.Name = "saveScenario";
			this.saveScenario.Size = new System.Drawing.Size(486, 268);
			this.saveScenario.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.doneButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 268);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(486, 40);
			this.panel1.TabIndex = 1;
			// 
			// doneButton
			// 
			this.doneButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.doneButton.Location = new System.Drawing.Point(400, 8);
			this.doneButton.Name = "doneButton";
			this.doneButton.TabIndex = 0;
			this.doneButton.Text = "Done";
			// 
			// SaveScenariosDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(486, 308);
			this.Controls.Add(this.saveScenario);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SaveScenariosDlg";
			this.Text = "Select Scenarios to be Saved";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.SaveScenariosDlg_Closing);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void SaveScenariosDlg_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			saveScenario.End();
		
		}

		public Database Db
		{
			set
			{
				saveScenario.Db = value;
			}
		}
	}
}
