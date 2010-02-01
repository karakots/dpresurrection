using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ModelView
{
	/// <summary>
	/// Summary description for ProjectControl.
	/// </summary>
	public class ProjectControl : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem modelMenuItem;
		private System.Windows.Forms.MenuItem newModelItem;
		private System.Windows.Forms.MenuItem renameModelItem;
		private System.Windows.Forms.MenuItem copyModelItem;
		private System.Windows.Forms.MenuItem deleteModelItem;
		private System.Windows.Forms.MenuItem exportMenu;
		private System.Windows.Forms.MenuItem importMenu;
		private System.Windows.Forms.MenuItem copyItems;
		private System.Windows.Forms.MenuItem copyMarketPlans;
		private System.Windows.Forms.MenuItem simulationMenuItem;
		private System.Windows.Forms.MenuItem runBatch;
		private System.Windows.Forms.MenuItem openResults;
		private System.Windows.Forms.MenuItem fileMenu;
		private System.Windows.Forms.MenuItem newProjectItem;
		private System.Windows.Forms.MenuItem openProjectItem;
		private System.Windows.Forms.MenuItem closeProjectItem;
		private System.Windows.Forms.MenuItem connectItem;
		private System.Windows.Forms.MenuItem exitItem;
		private System.Windows.Forms.MenuItem versionItem;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProjectControl()
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
			// 
			// openProjectItem
			// 
			this.openProjectItem.Index = 1;
			this.openProjectItem.Text = "Open Project..";
			// 
			// closeProjectItem
			// 
			this.closeProjectItem.Index = 2;
			this.closeProjectItem.Text = "Close Project";
			// 
			// connectItem
			// 
			this.connectItem.Index = 3;
			this.connectItem.Text = "Connect...";
			// 
			// exitItem
			// 
			this.exitItem.Index = 4;
			this.exitItem.Text = "Exit";
			// 
			// versionItem
			// 
			this.versionItem.Index = 5;
			this.versionItem.Text = "Version...";
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
			// 
			// newModelItem
			// 
			this.newModelItem.Index = 0;
			this.newModelItem.Text = "New Model";
			// 
			// renameModelItem
			// 
			this.renameModelItem.Index = 1;
			this.renameModelItem.Text = "Rename Model";
			// 
			// copyModelItem
			// 
			this.copyModelItem.Index = 2;
			this.copyModelItem.Text = "Copy Model";
			// 
			// deleteModelItem
			// 
			this.deleteModelItem.Index = 3;
			this.deleteModelItem.Text = "Delete Model";
			// 
			// exportMenu
			// 
			this.exportMenu.Index = 4;
			this.exportMenu.Text = "Export...";
			// 
			// importMenu
			// 
			this.importMenu.Index = 5;
			this.importMenu.Text = "Import...";
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
			// 
			// openResults
			// 
			this.openResults.Index = 1;
			this.openResults.Text = "Results";
			// 
			// ProjectControl
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(592, 245);
			this.Menu = this.mainMenu;
			this.Name = "ProjectControl";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "ProjectControl";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

		}
		#endregion
	}
}
