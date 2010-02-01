using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;

using System.Data;
using System.Diagnostics;
using System.IO;
using MrktSimDb;
using MrktSimDb.Metrics;
using ModelView;
using Common;
using System.Configuration;
using System.Collections.Specialized;
using Common.Utilities;
using Common.Dialogs;
using ModelView.MsTree;
using MarketSimUtilities;
using MarketSimUtilities.MsTree;
using Results;

namespace NimoClient
{
	/// <summary>
	/// Summary description for Form.
	/// </summary>
	public class MrktSim : System.Windows.Forms.Form
	{
		MSConnect msConnect = null;
		private ModelInfoDb modelDb = null;
		private MsTopProjectNode topProjectNode;

		/// <summary>
		/// Required designer variable.
		/// </summary>

		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem simulationMenuItem;
		private System.Windows.Forms.TreeView projectView;
		private System.Windows.Forms.MenuItem runBatch;
		private System.Windows.Forms.MenuItem openResults;
		private System.Windows.Forms.MenuItem fileMenu;
		private System.Windows.Forms.MenuItem newProjectItem;
		private System.Windows.Forms.MenuItem openProjectItem;
		private System.Windows.Forms.MenuItem closeProjectItem;
		private System.Windows.Forms.Panel startUpPanel;
		private System.Windows.Forms.MenuItem newModelItem;
		private System.Windows.Forms.MenuItem copyModelItem;
		private System.Windows.Forms.MenuItem deleteModelItem;
		private System.Windows.Forms.MenuItem modelMenuItem;
		private System.Windows.Forms.MenuItem exitItem;
		private ModelView.ModelControl modelControl;
		private System.Windows.Forms.MenuItem renameModelItem;
		private System.Windows.Forms.MenuItem connectItem;
		private System.Windows.Forms.MenuItem exportMenu;
		private System.Windows.Forms.MenuItem importMenu;
		private System.Windows.Forms.MenuItem versionItem;
		private System.Windows.Forms.MenuItem copyItems;
		private System.Windows.Forms.MenuItem copyMarketPlans;

		public MrktSim()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			// This is a NIMO app
			ModelInfoDb.Nimo = true;

			// read in application settings
			MrktSimControl.AppSettings = ConfigurationSettings.AppSettings;

			modelDb = new ModelInfoDb();

			modelControl.Visible = false;

			modelControl.ModelInfo = modelDb;

			topProjectNode = new MsTopProjectNode();

			topProjectNode.Db = modelDb;

			projectView.Nodes.Add(topProjectNode);
			msConnect = new MSConnect(Application.StartupPath);
			
			modelDb.Connection = msConnect.Connection;
			if (!msConnect.TestConnection())
			{
				// disable opening project
				openProjectItem.Enabled = false;
				newProjectItem.Enabled = false;
			}

			closeProjectItem.Enabled = false;
			runBatch.Enabled = false;

			projectOpen(false);

			this.Closing +=new CancelEventHandler(MrktSim_Closing);

			// initialize metric proper names
			Metric.TokenConvert +=new MrktSimDb.Metrics.Metric.Description(MrktSimControl.MrktSimMessage);
		}


		/// <summary>

		/// Clean up any resources being used.

		/// </summary>

		protected override void Dispose( bool disposing )

		{

			if( disposing )

			{

				if (components != null) 

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MrktSim));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.fileMenu = new System.Windows.Forms.MenuItem();
			this.newProjectItem = new System.Windows.Forms.MenuItem();
			this.openProjectItem = new System.Windows.Forms.MenuItem();
			this.closeProjectItem = new System.Windows.Forms.MenuItem();
			this.connectItem = new System.Windows.Forms.MenuItem();
			this.exitItem = new System.Windows.Forms.MenuItem();
			this.versionItem = new System.Windows.Forms.MenuItem();
			this.modelMenuItem = new System.Windows.Forms.MenuItem();
			this.newModelItem = new System.Windows.Forms.MenuItem();
			this.renameModelItem = new System.Windows.Forms.MenuItem();
			this.copyModelItem = new System.Windows.Forms.MenuItem();
			this.deleteModelItem = new System.Windows.Forms.MenuItem();
			this.exportMenu = new System.Windows.Forms.MenuItem();
			this.importMenu = new System.Windows.Forms.MenuItem();
			this.copyItems = new System.Windows.Forms.MenuItem();
			this.copyMarketPlans = new System.Windows.Forms.MenuItem();
			this.simulationMenuItem = new System.Windows.Forms.MenuItem();
			this.runBatch = new System.Windows.Forms.MenuItem();
			this.openResults = new System.Windows.Forms.MenuItem();
			this.projectView = new System.Windows.Forms.TreeView();
			this.startUpPanel = new System.Windows.Forms.Panel();
			this.modelControl = new ModelView.ModelControl();
			this.startUpPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.fileMenu,
																					 this.modelMenuItem,
																					 this.simulationMenuItem});
			// 
			// fileMenu
			// 
			this.fileMenu.Index = 0;
			this.fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.newProjectItem,
																					 this.openProjectItem,
																					 this.closeProjectItem,
																					 this.connectItem,
																					 this.exitItem,
																					 this.versionItem});
			this.fileMenu.Text = "File";
			// 
			// newProjectItem
			// 
			this.newProjectItem.Index = 0;
			this.newProjectItem.Text = "New Project...";
			this.newProjectItem.Click += new System.EventHandler(this.newProjectItem_Click);
			// 
			// openProjectItem
			// 
			this.openProjectItem.Index = 1;
			this.openProjectItem.Text = "Open Project..";
			this.openProjectItem.Click += new System.EventHandler(this.openProjectItem_Click);
			// 
			// closeProjectItem
			// 
			this.closeProjectItem.Index = 2;
			this.closeProjectItem.Text = "Close Project";
			this.closeProjectItem.Click += new System.EventHandler(this.closeProjectItem_Click);
			// 
			// connectItem
			// 
			this.connectItem.Index = 3;
			this.connectItem.Text = "Connect...";
			this.connectItem.Click += new System.EventHandler(this.connectItem_Click);
			// 
			// exitItem
			// 
			this.exitItem.Index = 4;
			this.exitItem.Text = "Exit";
			this.exitItem.Click += new System.EventHandler(this.exitItem_Click);
			// 
			// versionItem
			// 
			this.versionItem.Index = 5;
			this.versionItem.Text = "Version...";
			this.versionItem.Click += new System.EventHandler(this.versionItem_Click);
			// 
			// modelMenuItem
			// 
			this.modelMenuItem.Index = 1;
			this.modelMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.newModelItem,
																						  this.renameModelItem,
																						  this.copyModelItem,
																						  this.deleteModelItem,
																						  this.exportMenu,
																						  this.importMenu,
																						  this.copyItems});
			this.modelMenuItem.Text = "Model";
			this.modelMenuItem.Select += new System.EventHandler(this.modelMenuItem_Click);
			// 
			// newModelItem
			// 
			this.newModelItem.Index = 0;
			this.newModelItem.Text = "New Model";
			this.newModelItem.Click += new System.EventHandler(this.newModel_Click);
			// 
			// renameModelItem
			// 
			this.renameModelItem.Index = 1;
			this.renameModelItem.Text = "Rename Model";
			this.renameModelItem.Click += new System.EventHandler(this.renameModelItem_Click);
			// 
			// copyModelItem
			// 
			this.copyModelItem.Index = 2;
			this.copyModelItem.Text = "Copy Model";
			this.copyModelItem.Click += new System.EventHandler(this.copyModelItem_Click);
			// 
			// deleteModelItem
			// 
			this.deleteModelItem.Index = 3;
			this.deleteModelItem.Text = "Delete Model";
			this.deleteModelItem.Click += new System.EventHandler(this.deleteModel_Click);
			// 
			// exportMenu
			// 
			this.exportMenu.Index = 4;
			this.exportMenu.Text = "Export...";
			this.exportMenu.Click += new System.EventHandler(this.exportMenu_Click);
			// 
			// importMenu
			// 
			this.importMenu.Index = 5;
			this.importMenu.Text = "Import...";
			this.importMenu.Click += new System.EventHandler(this.importMenu_Click);
			// 
			// copyItems
			// 
			this.copyItems.Index = 6;
			this.copyItems.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.copyMarketPlans});
			this.copyItems.Text = "Copy";
			// 
			// copyMarketPlans
			// 
			this.copyMarketPlans.Index = 0;
			this.copyMarketPlans.Text = "Market Plans";
			this.copyMarketPlans.Click += new System.EventHandler(this.copyMarketPlans_Click);
			// 
			// simulationMenuItem
			// 
			this.simulationMenuItem.Index = 2;
			this.simulationMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							   this.runBatch,
																							   this.openResults});
			this.simulationMenuItem.Text = "Simulation";
			// 
			// runBatch
			// 
			this.runBatch.Index = 0;
			this.runBatch.Text = "Run Simulation...";
			this.runBatch.Click += new System.EventHandler(this.runBatch_Click);
			// 
			// openResults
			// 
			this.openResults.Index = 1;
			this.openResults.Text = "Results";
			this.openResults.Click += new System.EventHandler(this.openResults_Click);
			// 
			// projectView
			// 
			this.projectView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.projectView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectView.ImageIndex = -1;
			this.projectView.Location = new System.Drawing.Point(0, 0);
			this.projectView.Name = "projectView";
			this.projectView.SelectedImageIndex = -1;
			this.projectView.Size = new System.Drawing.Size(187, 396);
			this.projectView.TabIndex = 7;
			this.projectView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.projectView_MouseDown);
			this.projectView.Click += new System.EventHandler(this.projectView_Click);
			// 
			// startUpPanel
			// 
			this.startUpPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("startUpPanel.BackgroundImage")));
			this.startUpPanel.Controls.Add(this.modelControl);
			this.startUpPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.startUpPanel.Location = new System.Drawing.Point(187, 0);
			this.startUpPanel.Name = "startUpPanel";
			this.startUpPanel.Size = new System.Drawing.Size(525, 396);
			this.startUpPanel.TabIndex = 9;
			// 
			// modelControl
			// 
			this.modelControl.BackColor = System.Drawing.SystemColors.ControlLight;
			this.modelControl.Dock = System.Windows.Forms.DockStyle.Right;
			this.modelControl.Location = new System.Drawing.Point(5, 0);
			this.modelControl.Name = "modelControl";
			this.modelControl.Size = new System.Drawing.Size(520, 396);
			this.modelControl.TabIndex = 0;
			// 
			// MrktSim
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(712, 396);
			this.Controls.Add(this.projectView);
			this.Controls.Add(this.startUpPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(1200, 600);
			this.Menu = this.mainMenu;
			this.MinimumSize = new System.Drawing.Size(640, 430);
			this.Name = "MrktSim";
			this.Text = "Nimo";
			this.startUpPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion


		/// <summary>

		/// The main entry point for the application.

		/// </summary>

		[STAThread]

		static void Main() 
		{
			Application.Run(new MrktSim());
		}

		private void readDb()
		{
			modelDb.Refresh();
			topProjectNode.Initialize();
		}

		private void refreshDb()
		{
			modelDb.Refresh();
			topProjectNode.Refresh();
			projectView.SelectedNode = topProjectNode;
			modelSelected(false);
		}

		// utility to check if selected node is a model node

		private MsProjectModelNode getSelectedModelNode()
		{
			if (projectView.SelectedNode == null)
				return null;

			// get selected node
			MrktSimTreeNode node = (MrktSimTreeNode) projectView.SelectedNode;

			if( node.NodeType != MsNodeType.projectModelNodeType)
				return null;

			return (MsProjectModelNode) node;
		}

		// GUI event
		// create model view form on selected model
		// start up batch processing form
		private void runBatch_Click(object sender, System.EventArgs e)
		{
			// first check for proper installation
			string batchApp = Application.StartupPath + @"\BatchRun.exe";
			FileInfo fi = new FileInfo(batchApp);
			if (fi.Exists)
			{

				Process myProcess = new Process();

				myProcess.StartInfo.FileName = batchApp;

				myProcess.StartInfo.WorkingDirectory = Application.StartupPath;

				myProcess.StartInfo.Arguments = modelDb.ProjectID.ToString();

				myProcess.Start();
			}
			else
			{
				MessageBox.Show("Missing Batch Processing Application. Please check installation");
			}
			//
			//			ModelInfoDb tempDb = new ModelInfoDb();
			//		
			//			tempDb.ProjectID = 
			//
			//			tempDb.Connection = msConnect.Connection;
			//
			//			tempDb.Refresh();
			//
			//			using(BatchProcessForm batchForm = new BatchProcessForm(tempDb))
			//			{
			//				batchForm.ShowDialog();
			//			}
			//				
			//			this.readDb();
		}

		// run results spreadsheet
		private void openResults_Click(object sender, System.EventArgs e)
		{
			//			Process myProcess = new Process();
			//			myProcess.StartInfo.FileName = "Excel";
			//			myProcess.StartInfo.Arguments = ".\\Results.xls";
			//
			//			myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
			//			myProcess.Start();

			MsProjectModelNode node = getSelectedModelNode();

			if (node == null)
				return;			

			// new empty database
			Database db = new Database();
			db.Connection = modelDb.Connection;
			db.CurrentModel = node.model_id;
			db.ReadModelForResults();
			ResultsForm dlg = new ResultsForm();

			dlg.Db = db;
			modelDb.RefreshSimQueue();
			dlg.Project = modelDb;

			dlg.Show();
		}

		// create a new model
		private void newModel_Click(object sender, System.EventArgs e)
		{
			NameAndDescr dlg = new NameAndDescr();

			dlg.Type = "Model";

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				MrktSimDb.ModelInfo.Model_infoRow model = modelDb.CreateModel(dlg.ObjName, dlg.ObjDescription);
				model.app_code = ModelInfoDb.NimoAppCode;

				// writes data to disk
				modelDb.UpdateModelData();

				topProjectNode.Refresh();

				// create brand and product types
				modelDb.CreateProductType(model.model_id, "Brand");
				modelDb.CreateProductType(model.model_id, "Product");
			}		
		}


		// delete selected model

		private void deleteModel_Click(object sender, System.EventArgs e)
		{
			MrktSimTreeNode node = (MrktSimTreeNode) projectView.SelectedNode;

			DialogResult rslt = MessageBox.Show("This will delete the model from the database, this action cannot be undone",
				"Delete Model", MessageBoxButtons.OKCancel);

			if (rslt == DialogResult.OK)
				node.DeleteItem();

			// write to disk
			modelDb.UpdateModelData();

			// refresh
			refreshDb();
		}

		// set up context menu

		private void projectView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			MrktSimTreeNode node = (MrktSimTreeNode) projectView.GetNodeAt(e.X, e.Y);

			if (node == null)
				return;

			if (node.NodeType == MsNodeType.projectModelNodeType)
			{
				MsProjectModelNode modelNode = (MsProjectModelNode) node;

				modelSelected(true);

				if (modelNode.HasForms)
					allowModelEdit(false);
			}
			else
			{
				modelSelected(false);
			}

			projectView.SelectedNode = node;
		}

		private void projectView_Click(object sender, System.EventArgs e)
		{
			// set the node
			MsProjectModelNode node = getSelectedModelNode();
			if (node == null)
				return;

			modelControl.ModelNode = node;
		}

		private void newProjectItem_Click(object sender, System.EventArgs e)
		{
			NameAndDescr dlg = new NameAndDescr();

			dlg.Type = "Project";

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				ModelInfo.projectRow proj = modelDb.CreateProject(dlg.ObjName, dlg.ObjDescription);

				modelDb.UpdateModelData();

				modelDb.ProjectID = proj.id;				

				readDb();

				projectOpen(true);
			}
		}

		private void openProjectItem_Click(object sender, System.EventArgs e)
		{
			modelDb.ReadProjects();

			// check if we can close current project
			if (modelDb.ProjectID != ModelInfoDb.NoProjectID)
			{
				if (!checkDataQueryUser())
					return;

				topProjectNode.Close();

				modelDb.ProjectID = ModelInfoDb.NoProjectID;

				readDb();

				projectOpen(false);
			}

			OpenProject dlg = new OpenProject();

			dlg.Db = modelDb;

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				modelDb.ProjectID = dlg.ProjectID;

				checkForLockedModels();
				
				readDb();

				projectOpen(true);
			}
		}

		private bool checkDataQueryUser()
		{
			if (topProjectNode.HasChanges())
			{
				string msg = "Do you wish to save project";
				string caption = "Closing project";

				DialogResult res = MessageBox.Show(this, msg, caption, MessageBoxButtons.OKCancel);

				if( res == DialogResult.Cancel)
				{
					return false;
				}

				topProjectNode.Save();
			}

			return true;
		}

		private void closeProjectItem_Click(object sender, System.EventArgs e)
		{
			if (!checkDataQueryUser())
				return;

			topProjectNode.Close();

			modelDb.ProjectID = ModelInfoDb.NoProjectID;

			readDb();

			projectOpen(false);
		}

		private void databaseChanged(object sender, System.EventArgs e)
		{
			refreshDb();
		}


		private void allowModelEdit(bool allow)
		{
			renameModelItem.Enabled = allow;
			copyModelItem.Enabled = allow;
			deleteModelItem.Enabled = allow;
			exportMenu.Enabled = allow;
			copyItems.Enabled = allow;
		}

		private void modelSelected(bool aModelSelected)
		{
			openResults.Enabled = aModelSelected;
			allowModelEdit(aModelSelected);
			modelControl.Visible = aModelSelected;		
		}

		private void projectOpen(bool open)
		{
			runBatch.Enabled = open;
			projectView.Enabled = open;
			newModelItem.Enabled = open;
			closeProjectItem.Enabled = open;
			connectItem.Enabled = !open;
			importMenu.Enabled = open;

			// note we only set model selected to false
			// we do note set it to true until an actual selection takes place
			if (!open)
			{
				modelSelected(false);
			}
		}

		private void copyModelItem_Click(object sender, System.EventArgs e)
		{
			MsProjectModelNode node = getSelectedModelNode();

			if (node == null)
				return;

			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

			// new empty database
			Database db = new Database();
			db.Connection = modelDb.Connection;
			db.CurrentModel = node.model_id;

			db.Refresh();						

			// we make a copy
			// this makes all the records "new"
			// cannot find how to force this in an existing dataset

			Database dbCopy = db.Copy();

			// get a unique name
			string modelName = db.Model.model_name;

			string filter = "project_id = " + modelDb.ProjectID;

			dbCopy.Model.model_name = Database.CreateUniqueName(modelDb.ModelData.Model_info, "model_name", modelName, filter);

			dbCopy.Update();

			refreshDb();

			this.Cursor = System.Windows.Forms.Cursors.Default;
		}


		private void renameModelItem_Click(object sender, System.EventArgs e)
		{
			MsProjectModelNode node = getSelectedModelNode();

			if (node == null)
				return;

			NameAndDescr dlg = new NameAndDescr();

			dlg.Type = "Model";

			dlg.ObjDescription = node.Model.descr;

			dlg.ObjName = node.Model.model_name;

			dlg.Editing = true;

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				// rename
				node.ModelName = dlg.ObjName;
				node.Model.descr = dlg.ObjDescription;

				// write to disk
				modelDb.UpdateModelData();

				this.modelControl.UpdateModelInfo();
			}
		}

		private void connectItem_Click(object sender, System.EventArgs e)
		{
			string error = null;
			if (!msConnect.Connect(out error))
			{
				MessageBox.Show(error);

				// disable opening project
				openProjectItem.Enabled = false;
				newProjectItem.Enabled = false;
			}
			else
			{
				// enable opening project
				openProjectItem.Enabled = true;
				newProjectItem.Enabled = true;
				// check for engine connect file
				string msEngineConnectFile = Application.StartupPath + @"\..\MarketSimEngine\dbconnect";
				FileInfo fi = new FileInfo(msEngineConnectFile);
				if (fi.Exists)
				{
					// ask user if she wants to reset connection for sim engine
					DialogResult res = MessageBox.Show(this, "Do you wish to reset the Simulation Engine?", "Reset Engine", MessageBoxButtons.YesNo);

					if (res == DialogResult.Yes)
					{
						try
						{
							fi.Delete();
						}
						catch(Exception oops)
						{
							MessageBox.Show("Error removing sim engine connection file: System error = " + oops.Message);
						}

						//					// tell engine to reset
						//					Process resetProcess = new Process();
						//					resetProcess.StartInfo.FileName = Application.StartupPath;
						//					resetProcess.StartInfo.FileName += @"\" + "ResetServer.bat";
					}
				}
			}
			//
			//			mrktsimConnection.ConnectionString = "";
			//			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();
			//			openFileDlg.DefaultExt = ".udl";
			//			openFileDlg.Filter = "Connection Files|*.udl";
			//
			//			DialogResult rslt = openFileDlg.ShowDialog();
			//
			//			if (rslt == DialogResult.OK)
			//			{
			//				connectFile = openFileDlg.FileName;
			//
			//				if (!testConnection())
			//				{
			//					MessageBox.Show("Connection Failed");
			//
			//					// disable opening project\
			//					openProjectItem.Enabled = false;
			//					newProjectItem.Enabled = false;
			//				}
			//				else
			//				{
			//					// enable opening project
			//					openProjectItem.Enabled = true;
			//					newProjectItem.Enabled = true;
			//				}
			//			}
		}

		private void importMenu_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			openFileDlg.Filter = "Market Sim Model (*.msdb)|*.msdb";

			openFileDlg.CheckFileExists = false;

			openFileDlg.ReadOnlyChecked = false;

			DialogResult rslt = openFileDlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

				// TODO wrap up in a class method
				Database db = new Database();
				try	
				{
					db.Data.ReadXml(openFileDlg.FileName);
				}
				catch
				{
					MessageBox.Show("Error: File is out of date or damaged","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
					this.Cursor = System.Windows.Forms.Cursors.Default;
					return;
				}
				db.CurrentModel = (int) db.Data.Model_info.Rows[0]["model_id"];
				db.Connection = modelDb.Connection;

				string modelName = db.Model.model_name;
				
				// we make a copy
				// this makes all the records "new"
				// cannot find how to force this in an existing dataset

				Database dbCopy = db.Copy();

				// get a unique name
				string filter = "project_id = " + modelDb.ProjectID;

				dbCopy.Model.model_name = Database.CreateUniqueName(modelDb.ModelData.Model_info, "model_name", modelName, filter);

				// move copy to this project
				dbCopy.Model.project_id = modelDb.ProjectID;

				dbCopy.Update();
				refreshDb();

				this.Cursor = System.Windows.Forms.Cursors.Default;
			}
		}

		private void exportMenu_Click(object sender, System.EventArgs e)
		{
			MsProjectModelNode node = getSelectedModelNode();

			if (node == null)
				return;

			//System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();
			System.Windows.Forms.SaveFileDialog saveFileDlg = new SaveFileDialog();

			saveFileDlg.DefaultExt = ".msdb";

			saveFileDlg.Filter = "Market Sim Model (*.msdb)|*.msdb";

			saveFileDlg.CheckFileExists = false;
			//saveFileDlg.ReadOnlyChecked = false;

			DialogResult rslt = saveFileDlg.ShowDialog();

			if (rslt != DialogResult.OK)
				return;

			DialogResult rsltQuery = MessageBox.Show(this, "Do you wish to include results in the export?","Save results", MessageBoxButtons.YesNoCancel);

			string tempFile = Application.StartupPath + "\\temp.msdb";

			if (rsltQuery == DialogResult.Yes)
			{
				// new empty database
				Database db = new Database();
				db.Connection = modelDb.Connection;
				db.CurrentModel = node.model_id;
				db.RefreshModelPlusResults();
				db.Data.WriteXml(tempFile);
			}
			if (rsltQuery == DialogResult.No)
			{
				// new empty database
				Database db = new Database();
				db.Connection = modelDb.Connection;
				db.CurrentModel = node.model_id;
				db.Refresh();
				db.Data.WriteXml(tempFile);
			}

			FileCleaner cleaner = new FileCleaner();

			cleaner.clean(tempFile,saveFileDlg.FileName);
		}

		private void modelMenuItem_Click(object sender, System.EventArgs e)
		{
			if (projectView.SelectedNode == null)
			{
				modelSelected(false);
				return;
			}

			if (((MrktSimTreeNode) projectView.SelectedNode).NodeType == MsNodeType.projectModelNodeType)
			{
				MsProjectModelNode modelNode = (MsProjectModelNode) projectView.SelectedNode;

				modelSelected(true);

				if (modelNode.HasForms)
					allowModelEdit(false);
			}
			else
			{
				modelSelected(false);
			}
		}

		private void exitItem_Click(object sender, System.EventArgs e)
		{		
			this.Close();
		}
		private void MrktSim_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!checkDataQueryUser())
			{
				e.Cancel = true;
			}

			topProjectNode.Close();
		}
		// I am throwing this in as a check for the non-interprise version
		// will remove if we move central storage
		private void checkForLockedModels()
		{
			if (modelDb.LockedModels())
			{
				DialogResult rslt = MessageBox.Show("There are locked models or scenarios in this project, Do you wish to unlock them?", "Unlock models and scenarios", MessageBoxButtons.YesNo);
				if (rslt == DialogResult.Yes)
					modelDb.UnLockModels();
			}
		}

		private void versionItem_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(Application.ProductName + " Version = " + Application.ProductVersion);
		}

		private void copyMarketPlans_Click(object sender, System.EventArgs e)
		{
			MsProjectModelNode node = getSelectedModelNode();

			if (node == null)
				return;

			// new empty database
			Database toDb = new Database();
			toDb.Connection = modelDb.Connection;
			toDb.CurrentModel = node.model_id;
			toDb.Open();
			if (toDb.ReadOnly)
			{	
				MessageBox.Show(MrktSimControl.MrktSimMessage("Model.Locked"));
				toDb.Close();
				
				return;
			}
			
			// now pick a model
			SelectMarketPlan pickModel = new SelectMarketPlan(modelDb.ModelData.Model_info);

			pickModel.Text = "Select Model to Copy Market Plan From";
			pickModel.AllowMultiSelect = false;

			pickModel.Grid.Clear();
			pickModel.Grid.AddTextColumn("model_name");
			pickModel.Grid.Reset();
			pickModel.Grid.RowFilter = "model_id <> " + node.model_id;

			DialogResult rslt = pickModel.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				ArrayList list = pickModel.SelectedItems;

				if (list.Count > 0)
				{
					DataRow row = (DataRow) list[0];

					int fromModelID = (int) row["model_id"];

					Database fromDb = new Database();
					fromDb.Connection = modelDb.Connection;
					fromDb.CurrentModel = fromModelID;

					fromDb.Refresh();

					// now pick marketPlans
					SelectMarketPlan pickMarketPlan = new SelectMarketPlan(fromDb.Data.market_plan);

					pickMarketPlan.Text = "Select Market Plan";

					pickMarketPlan.Grid.Clear();

					// pickMarketPlan.Grid.AddComboBoxColumn("type", fromDb.Data.market_plan_type, "type", "id", true);
					pickMarketPlan.Grid.AddComboBoxColumn("product_id", fromDb.Data.product, "product_name", "product_id", true);
					pickMarketPlan.Grid.AddTextColumn("name");
					
					pickMarketPlan.Grid.Reset();

					pickMarketPlan.Grid.RowFilter = "type = 0";

					DialogResult rslt2 = pickMarketPlan.ShowDialog();

					if (rslt2 == DialogResult.OK)
					{
						// update the database we need to copy to
						toDb.Refresh();

						MrktSimDb.MrktSimDBSchema.market_planRow[] plans = new MrktSimDb.MrktSimDBSchema.market_planRow[pickMarketPlan.SelectedItems.Count];

						for(int index = 0; index < plans.Length; ++index)
						{
							plans[index] = (MrktSimDb.MrktSimDBSchema.market_planRow) pickMarketPlan.SelectedItems[index];
						}

						// add market plans
						if (modelDb.CopyMarketPlans(fromDb, toDb, plans))
							toDb.Update();
					}

					fromDb.Close();
				}
			}

			toDb.Close();
		}
	}
}