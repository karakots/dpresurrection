using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Common.Utilities
{
	/// <summary>
	/// Summary description for ScenarioPicker.
	/// </summary>
	public class ScenarioPicker : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.Button copyButton;
		private System.Windows.Forms.Button editButton;
		private System.Windows.Forms.Button createbutton;
		private System.Windows.Forms.ListBox scenarioBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ScenarioPicker()
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
			this.deleteButton = new System.Windows.Forms.Button();
			this.copyButton = new System.Windows.Forms.Button();
			this.editButton = new System.Windows.Forms.Button();
			this.createbutton = new System.Windows.Forms.Button();
			this.scenarioBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// deleteButton
			// 
			this.deleteButton.Location = new System.Drawing.Point(232, 72);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(45, 23);
			this.deleteButton.TabIndex = 43;
			this.deleteButton.Text = "Delete";
			// 
			// copyButton
			// 
			this.copyButton.Location = new System.Drawing.Point(232, 48);
			this.copyButton.Name = "copyButton";
			this.copyButton.Size = new System.Drawing.Size(45, 23);
			this.copyButton.TabIndex = 42;
			this.copyButton.Text = "Copy";
			// 
			// editButton
			// 
			this.editButton.Location = new System.Drawing.Point(232, 24);
			this.editButton.Name = "editButton";
			this.editButton.Size = new System.Drawing.Size(45, 23);
			this.editButton.TabIndex = 41;
			this.editButton.Text = "Edit";
			// 
			// createbutton
			// 
			this.createbutton.Location = new System.Drawing.Point(232, 0);
			this.createbutton.Name = "createbutton";
			this.createbutton.Size = new System.Drawing.Size(45, 23);
			this.createbutton.TabIndex = 40;
			this.createbutton.Text = "New";
			// 
			// scenarioBox
			// 
			this.scenarioBox.Location = new System.Drawing.Point(0, 0);
			this.scenarioBox.Name = "scenarioBox";
			this.scenarioBox.Size = new System.Drawing.Size(224, 95);
			this.scenarioBox.TabIndex = 39;
			// 
			// ScenarioPicker
			// 
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.copyButton);
			this.Controls.Add(this.editButton);
			this.Controls.Add(this.createbutton);
			this.Controls.Add(this.scenarioBox);
			this.Name = "ScenarioPicker";
			this.Size = new System.Drawing.Size(280, 96);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
