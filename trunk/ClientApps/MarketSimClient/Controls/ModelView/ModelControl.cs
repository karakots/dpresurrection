using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;
using ModelView;
using Common.Utilities;
using Common.Dialogs;
using ModelView.Dialogs;

using MarketSimUtilities;

namespace ModelView
{
	/// <summary>
	/// Summary description for ModelControl.
	/// </summary>
	public class ModelControl : System.Windows.Forms.UserControl
	{
		private ProjectDb iModelDb;

		public ProjectDb Db
		{
			set
			{
				iModelDb = value;

				scenarioView.Table = iModelDb.Data.simulation;
				scenarioBox.DisplayMember = "name";
			}
		}

		private object modelNode = null;
		public object ModelNode
		{
			set
			{
				modelNode = value;
			}
		}

        public object Model
		{
			get
			{
				if (modelNode == null)
					return null;

				return modelNode;
			}
		}

        private System.Windows.Forms.ListBox scenarioBox;
		private System.Windows.Forms.TextBox descBox;
		private System.Windows.Forms.TextBox nameBox;
        private System.Data.DataView scenarioView;
        private System.Windows.Forms.TextBox scenarioDescrBox;
		private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label checkPointLabel;
        private ContextMenuStrip simulationMenu;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private IContainer components;

		public ModelControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
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
            this.scenarioBox = new System.Windows.Forms.ListBox();
            this.simulationMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scenarioView = new System.Data.DataView();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.descBox = new System.Windows.Forms.TextBox();
            this.scenarioDescrBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkPointLabel = new System.Windows.Forms.Label();
            this.simulationMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioView)).BeginInit();
            this.SuspendLayout();
            // 
            // scenarioBox
            // 
            this.scenarioBox.ContextMenuStrip = this.simulationMenu;
            this.scenarioBox.DataSource = this.scenarioView;
            this.scenarioBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.scenarioBox.Location = new System.Drawing.Point(272, 0);
            this.scenarioBox.Name = "scenarioBox";
            this.scenarioBox.Size = new System.Drawing.Size(240, 316);
            this.scenarioBox.TabIndex = 0;
            this.scenarioBox.SelectedIndexChanged += new System.EventHandler(this.scenarioBox_SelectedIndexChanged);
            // 
            // simulationMenu
            // 
            this.simulationMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.editToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.simulationMenu.Name = "contextMenuStrip1";
            this.simulationMenu.Size = new System.Drawing.Size(129, 92);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.newToolStripMenuItem.Text = "New...";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newScenarioButton_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.editToolStripMenuItem.Text = "Edit...";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editScenarioButton_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.copyToolStripMenuItem.Text = "Copy...";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyScenarioButton_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.deleteToolStripMenuItem.Text = "Delete...";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // nameBox
            // 
            this.nameBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.nameBox.Location = new System.Drawing.Point(11, 6);
            this.nameBox.Name = "nameBox";
            this.nameBox.ReadOnly = true;
            this.nameBox.Size = new System.Drawing.Size(240, 20);
            this.nameBox.TabIndex = 20;
            this.nameBox.WordWrap = false;
            // 
            // descBox
            // 
            this.descBox.Location = new System.Drawing.Point(11, 32);
            this.descBox.Multiline = true;
            this.descBox.Name = "descBox";
            this.descBox.ReadOnly = true;
            this.descBox.Size = new System.Drawing.Size(240, 56);
            this.descBox.TabIndex = 23;
            // 
            // scenarioDescrBox
            // 
            this.scenarioDescrBox.Location = new System.Drawing.Point(11, 276);
            this.scenarioDescrBox.Multiline = true;
            this.scenarioDescrBox.Name = "scenarioDescrBox";
            this.scenarioDescrBox.ReadOnly = true;
            this.scenarioDescrBox.Size = new System.Drawing.Size(248, 40);
            this.scenarioDescrBox.TabIndex = 29;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 136);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 16);
            this.label7.TabIndex = 38;
            this.label7.Text = "Check Point: ";
            // 
            // checkPointLabel
            // 
            this.checkPointLabel.Location = new System.Drawing.Point(96, 136);
            this.checkPointLabel.Name = "checkPointLabel";
            this.checkPointLabel.Size = new System.Drawing.Size(72, 16);
            this.checkPointLabel.TabIndex = 39;
            this.checkPointLabel.Text = "11/11/2005";
            // 
            // ModelControl
            // 
            this.Controls.Add(this.checkPointLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.scenarioDescrBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.descBox);
            this.Controls.Add(this.scenarioBox);
            this.Name = "ModelControl";
            this.Size = new System.Drawing.Size(512, 328);
            this.simulationMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scenarioView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public void UpdateModelInfo()
		{
			if (Model == null)
				return;

            //nameBox.Text = Model.model_name;
            //descBox.Text = Model.descr;

            //if (Model.checkpoint_valid)
            //{
            //    this.checkPointLabel.Text = Model.checkpoint_date.ToShortDateString();
            //}
            //else
            //{							//  "11/11/2005"
            //    this.checkPointLabel.Text = "Not Set";
            //}

			// count products
            //numProductsLabel.Text = iModelDb.NumObjects(Model.model_id, "product", "brand_id = 1").ToString();
            //numSegmentsLabel.Text = iModelDb.NumObjects(Model.model_id, "segment").ToString();
            //numChannelsLabel.Text = iModelDb.NumObjects(Model.model_id, "channel").ToString();
            //numAttributesLabel.Text = iModelDb.NumObjects(Model.model_id, "product_attribute").ToString();

			// scenarioView.RowFilter = "model_id = " + Model.model_id;

			if (scenarioView.Count == 0)
			{
				this.newToolStripMenuItem.Enabled = false;
				this.copyToolStripMenuItem.Enabled = false;
				this.deleteToolStripMenuItem.Enabled = false;
			}
			else
			{
				this.editToolStripMenuItem.Enabled = true;
				this.editToolStripMenuItem.Enabled = true;
				this.deleteToolStripMenuItem.Enabled = true;

				if (scenarioBox.SelectedItem != null)
				{
                    MrktSimDBSchema.simulationRow simulation = (MrktSimDBSchema.simulationRow)((DataRowView)scenarioBox.SelectedItem).Row;
                    scenarioDescrBox.Text = simulation.descr;
				}
			}
		}

		private void newScenarioButton_Click(object sender, System.EventArgs e)
		{
            //NameAndDescr dlg = new NameAndDescr();
            //dlg.Type = "Scenario";

            //DialogResult rslt = dlg.ShowDialog();

            //MrktSimDBSchema.scenarioRow scenario = null;// new MrktSimDBSchema.scenarioRow();

            //if (rslt == DialogResult.OK)
            //{
            //    MrktSimDBSchema.simulationRow currentSimulation = iModelDb.CreateStandardSimulation(scenario, dlg.ObjName);

            //    CurrentSimulation.descr = dlg.ObjDescription;

            //    iModelDb.UpdateModelData();

            //    dlg_ScenarioDbChanged();
            //}
		}

		private void editScenarioButton_Click(object sender, System.EventArgs e)
		{
			if (scenarioBox.SelectedItem == null)
				return;

            MrktSimDBSchema.simulationRow simulation = (MrktSimDBSchema.simulationRow)((DataRowView)scenarioBox.SelectedItem).Row;

            SimulationDb db = new SimulationDb();

			db.Connection = iModelDb.Connection;

            db.ProjectID = iModelDb.ProjectID;

			db.Refresh();

			CreateScenario dlg = new CreateScenario();

			dlg.Db = db;
            dlg.Text = "Editing Simulation " + simulation.name;
            dlg.CurrentSimulation = simulation;


            //if (db.ReadOnly)
            //    dlg.Text += " (Read Only)";
            //else 
            if (dlg.CurrentSimulation.sim_num >= 0)
                dlg.Text += " (Scenario has been run, editing is restricted)";


			// modelNode.AddModelForm(dlg);

			dlg.ShowDialog();

            // process the simulation as needed
		}

		private void dlg_ScenarioDbChanged()
		{
			this.iModelDb.Refresh();

			if (scenarioView.Count == 0)
			{
				this.editToolStripMenuItem.Enabled = false;
				this.copyToolStripMenuItem.Enabled = false;
				this.deleteToolStripMenuItem.Enabled = false;
			}
			else
			{
                this.editToolStripMenuItem.Enabled = true;
                this.copyToolStripMenuItem.Enabled = true;
                this.deleteToolStripMenuItem.Enabled = true;

				if (scenarioBox.SelectedItem != null)
				{
					MrktSimDBSchema.scenarioRow scenario = (MrktSimDBSchema.scenarioRow) ((DataRowView) scenarioBox.SelectedItem).Row;
					scenarioDescrBox.Text = scenario.descr;
				}
			}
		}

        /// <summary>
        /// Brings up a form for editing the model and market plans.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void openModelButton_Click(object sender, System.EventArgs e)
		{
           // // new empty database
           // ModelDb db = new ModelDb();
           // db.Connection = iModelDb.Connection;
           // db.ModelID = Model.model_id;
			
           // db.Open();
			
           // if (db.ReadOnly)
           // {
           //     MessageBox.Show(MrktSimControl.MrktSimMessage("Model.Locked"));
           // }

           // db.Refresh();

           //ModelAndMarketPlanView form = new ModelAndMarketPlanView();

           // form.Db = db;

           // form.Text = Model.model_name + " - Model and Market Plans";

           // if( db.ReadOnly )
           //     form.Text += " (Read Only)";

           // modelNode.AddModelForm( form );

           // form.Show();
		}

		private void deleteButton_Click(object sender, System.EventArgs e)
		{
			if (scenarioBox.SelectedItem == null)
				return;


            MrktSimDBSchema.simulationRow simulation = (MrktSimDBSchema.simulationRow)((DataRowView)scenarioBox.SelectedItem).Row;

            if (simulation.Getsim_queueRows().Length > 0)
			{
				DialogResult rslt = MessageBox.Show(this, "This scenario has results associated with it that will be deleted", "Delete Scenario?", MessageBoxButtons.OKCancel);

				if (rslt == DialogResult.Cancel)
					return;
			}

            iModelDb.DeleteSimulation(simulation.id);

			dlg_ScenarioDbChanged();
		}

		private void copyScenarioButton_Click(object sender, System.EventArgs e)
		{
			if (scenarioBox.SelectedItem == null)
				return;

            MrktSimDBSchema.simulationRow simulation = (MrktSimDBSchema.simulationRow)((DataRowView)scenarioBox.SelectedItem).Row;
            
            iModelDb.CopySimulation(simulation);

			dlg_ScenarioDbChanged();
		
		}

		private void externalDataButton_Click(object sender, System.EventArgs e)
		{
//            // new empty database
//            ModelDb db = new ModelDb();
//            db.Connection = iModelDb.Connection;
//            db.ModelID = Model.model_id;
			
//            db.Open();
			
//            if (db.ReadOnly)
//            {
//                MessageBox.Show(MrktSimControl.MrktSimMessage("Model.Locked"));
//            }

//            db.Refresh();
			
//            ExternalData extDataView = new ExternalData();

//            extDataView.Db = db;
//            extDataView.Text = "External Data for " + Model.model_name;

////			if (db.ReadOnly)
////				modelView.Text += " (Read Only)";


//            extDataView.ShowDialog();

//            db.Close();
		}

		private void calibrationButton_Click(object sender, System.EventArgs e)
		{
			
		}

		private void scenarioBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (scenarioBox.SelectedItem == null)
				return;

            MrktSimDBSchema.simulationRow simulation = (MrktSimDBSchema.simulationRow)((DataRowView)scenarioBox.SelectedItem).Row;

            scenarioDescrBox.Text = simulation.descr;
		}
	}
}
