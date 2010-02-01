using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Common.Utilities;
using Common.Dialogs;
using MrktSimDb;

namespace Common
{
	/// <summary>
	/// Summary description for Scenario.
	/// </summary>
	public class Scenario : MrktSimControl
	{
		public override MrktSimDb.Database Db
		{
			set
			{
				base.Db = value;

				scenarioBox.DataSource = theDb.Data.scenario;
				scenarioBox.DisplayMember = "name";
				scenarioBox.ValueMember = "scenario_id";

				scenarioGrid.Table = theDb.Data.scenario;
				scenarioGrid.DescriptionWindow = false;

				createTableStyle();

			}
		}

		public MrktSimDBSchema.scenarioRow SelectedScenario
		{
			get
			{
				if (scenarioBox.SelectedItem == null)
					return null;

				return (MrktSimDBSchema.scenarioRow) ((DataRowView) scenarioBox.SelectedItem).Row;
			}
		}

		private System.Windows.Forms.GroupBox groupBox1;
		private Common.Utilities.MrktSimGrid scenarioGrid;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListBox scenarioBox;
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.Button copyButton;
		private System.Windows.Forms.Button editButton;
		private System.Windows.Forms.Button createbutton;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Scenario()
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.scenarioGrid = new Common.Utilities.MrktSimGrid();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.scenarioBox = new System.Windows.Forms.ListBox();
			this.deleteButton = new System.Windows.Forms.Button();
			this.copyButton = new System.Windows.Forms.Button();
			this.editButton = new System.Windows.Forms.Button();
			this.createbutton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(464, 152);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			// 
			// scenarioGrid
			// 
			this.scenarioGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scenarioGrid.Location = new System.Drawing.Point(0, 152);
			this.scenarioGrid.Name = "scenarioGrid";
			this.scenarioGrid.RowFilter = null;
			this.scenarioGrid.Size = new System.Drawing.Size(464, 160);
			this.scenarioGrid.TabIndex = 7;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.deleteButton);
			this.groupBox2.Controls.Add(this.copyButton);
			this.groupBox2.Controls.Add(this.editButton);
			this.groupBox2.Controls.Add(this.createbutton);
			this.groupBox2.Controls.Add(this.scenarioBox);
			this.groupBox2.Location = new System.Drawing.Point(8, 16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(272, 128);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Select Scenario";
			// 
			// scenarioBox
			// 
			this.scenarioBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.scenarioBox.Location = new System.Drawing.Point(3, 16);
			this.scenarioBox.Name = "scenarioBox";
			this.scenarioBox.Size = new System.Drawing.Size(213, 108);
			this.scenarioBox.TabIndex = 0;
			// 
			// deleteButton
			// 
			this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.deleteButton.Location = new System.Drawing.Point(224, 88);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(45, 23);
			this.deleteButton.TabIndex = 38;
			this.deleteButton.Text = "Delete";
			// 
			// copyButton
			// 
			this.copyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.copyButton.Location = new System.Drawing.Point(224, 64);
			this.copyButton.Name = "copyButton";
			this.copyButton.Size = new System.Drawing.Size(45, 23);
			this.copyButton.TabIndex = 37;
			this.copyButton.Text = "Copy";
			// 
			// editButton
			// 
			this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.editButton.Location = new System.Drawing.Point(224, 40);
			this.editButton.Name = "editButton";
			this.editButton.Size = new System.Drawing.Size(45, 23);
			this.editButton.TabIndex = 36;
			this.editButton.Text = "Edit";
			this.editButton.Click += new System.EventHandler(this.editButton_Click);
			// 
			// createbutton
			// 
			this.createbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.createbutton.Location = new System.Drawing.Point(224, 16);
			this.createbutton.Name = "createbutton";
			this.createbutton.Size = new System.Drawing.Size(45, 23);
			this.createbutton.TabIndex = 35;
			this.createbutton.Text = "New";
			this.createbutton.Click += new System.EventHandler(this.createbutton_Click);
			// 
			// Scenario
			// 
			this.Controls.Add(this.scenarioGrid);
			this.Controls.Add(this.groupBox1);
			this.Name = "Scenario";
			this.Size = new System.Drawing.Size(464, 312);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void createTableStyle()
		{
			scenarioGrid.Clear();
			scenarioGrid.AddTextColumn("name");
			scenarioGrid.AddDateColumn("start_date");
			scenarioGrid.AddDateColumn("end_date");
			scenarioGrid.Reset();
		}


		private void createbutton_Click(object sender, System.EventArgs e)
		{
			//CreateScenario dlg = new CreateScenario(theDb);

			// dlg.ShowDialog(this);
		}

		private void editButton_Click(object sender, System.EventArgs e)
		{
			//CreateScenario dlg = new CreateScenario(theDb);

			//dlg.CurrentScenario = this.SelectedScenario;

			//dlg.ShowDialog(this);		
		}
	}
}
