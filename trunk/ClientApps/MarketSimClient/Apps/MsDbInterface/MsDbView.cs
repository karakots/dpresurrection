using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

using MrktSimDb;

namespace MsDbInterface
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MsDbView : System.Windows.Forms.Form
	{

		private Database data = null;
		private ModelInfoDb modelDb = null;

		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.ListBox TableList;
		private System.Windows.Forms.DataGrid tableGrid;
		private System.Data.DataView tableView;
		private System.Data.DataView modelView;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.CheckedListBox ModelList;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.ContextMenu modelContextMenu;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MsDbView()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			modelDb = new ModelInfoDb();

			data = new Database();

			System.Data.OleDb.OleDbConnection mrktsimConnection = new System.Data.OleDb.OleDbConnection();
			
			mrktsimConnection.ConnectionString = "File Name=mrktsim.udl;";
			data.Connection = mrktsimConnection;
			modelDb.Connection = mrktsimConnection;
			modelDb.ProjectID = 1;

			modelDb.Refresh();

			ModelList.DataSource = modelDb.ModelData;
			ModelList.DisplayMember = "Model_info.model_name";

			DataRowView view = (DataRowView) ModelList.SelectedItem;
			ModelInfo.Model_infoRow row = (ModelInfo.Model_infoRow) view.Row;

			data.CurrentModel = row.model_id;

			data.Refresh();

			tableView.Table = data.Data.Tables[0];

			// fill out tableList
			foreach(DataTable table in data.Data.Tables)
			{
				TableList.Items.Add(table.TableName);
			}

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MsDbView));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.modelView = new System.Data.DataView();
			this.TableList = new System.Windows.Forms.ListBox();
			this.tableGrid = new System.Windows.Forms.DataGrid();
			this.tableView = new System.Data.DataView();
			this.ModelList = new System.Windows.Forms.CheckedListBox();
			this.modelContextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.modelView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tableGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tableView)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem5,
																					  this.menuItem2,
																					  this.menuItem3});
			this.menuItem1.Text = "Simulation";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 0;
			this.menuItem5.Text = "Results...";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "Refresh";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click_1);
			// 
			// TableList
			// 
			this.TableList.Dock = System.Windows.Forms.DockStyle.Left;
			this.TableList.Location = new System.Drawing.Point(91, 0);
			this.TableList.Name = "TableList";
			this.TableList.Size = new System.Drawing.Size(88, 303);
			this.TableList.TabIndex = 2;
			this.TableList.SelectedIndexChanged += new System.EventHandler(this.tableList_itemChanged);
			// 
			// tableGrid
			// 
			this.tableGrid.DataMember = "";
			this.tableGrid.DataSource = this.tableView;
			this.tableGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.tableGrid.Location = new System.Drawing.Point(187, 0);
			this.tableGrid.Name = "tableGrid";
			this.tableGrid.Size = new System.Drawing.Size(549, 305);
			this.tableGrid.TabIndex = 4;
			// 
			// ModelList
			// 
			this.ModelList.CheckOnClick = true;
			this.ModelList.ContextMenu = this.modelContextMenu;
			this.ModelList.Dock = System.Windows.Forms.DockStyle.Left;
			this.ModelList.Location = new System.Drawing.Point(0, 0);
			this.ModelList.Name = "ModelList";
			this.ModelList.Size = new System.Drawing.Size(88, 304);
			this.ModelList.TabIndex = 5;
			this.ModelList.SelectedIndexChanged += new System.EventHandler(this.modelList_itemChanged);
			// 
			// modelContextMenu
			// 
			this.modelContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							 this.menuItem6});
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 0;
			this.menuItem6.Text = "Run Nimo on checked items...";
			this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(88, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 305);
			this.splitter1.TabIndex = 6;
			this.splitter1.TabStop = false;
			// 
			// splitter2
			// 
			this.splitter2.Location = new System.Drawing.Point(179, 0);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(8, 305);
			this.splitter2.TabIndex = 7;
			this.splitter2.TabStop = false;
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.Text = "Save";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click_1);
			// 
			// MsDbView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(736, 305);
			this.Controls.Add(this.tableGrid);
			this.Controls.Add(this.splitter2);
			this.Controls.Add(this.TableList);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.ModelList);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "MsDbView";
			this.Text = "MarketSim Database Viewer";
			((System.ComponentModel.ISupportInitialize)(this.modelView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tableGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tableView)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MsDbView());
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			// set selected model to be run
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			data.Data.WriteXml("C:\\data\\MsTestXML.xms");
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			data.Data.Clear();

			data.Data.ReadXml("C:\\data\\MsTestXML.xms");
		}

		private void tableList_itemChanged(object sender, System.EventArgs e)
		{
			// set current table in view
			int index = TableList.SelectedIndices[0];
			tableView.Table = data.Data.Tables[index];
		}

		private void modelList_itemChanged(object sender, System.EventArgs e)
		{
			DataRowView view = (DataRowView) ModelList.SelectedItem;

			ModelInfo.Model_infoRow row = (ModelInfo.Model_infoRow) view.Row;

			data.CurrentModel = row.model_id;

			data.Refresh();
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
		}

		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			
		}

		private void menuItem2_Click_1(object sender, System.EventArgs e)
		{
			data.Refresh();
		}

		private void menuItem3_Click_1(object sender, System.EventArgs e)
		{
			data.Update();
		}
	}
}
