using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
using Common.Utilities;
using MarketSimUtilities.MsTree;

using MarketSimUtilities;
namespace Common
{
	/// <summary>
	/// Summary description for ProductMatrix.
	/// </summary>
	public class ProductMatrixControl : MrktSimControl
	{

		public override void Reset()
		{
			base.Reset ();

			this.createTableStyle();
		}

		public override void Refresh()
		{
			base.Refresh();

			incompatGrid.Refresh();
			prereqGrid.Refresh();
		}

		public override void Flush()
		{
			incompatGrid.Flush();
			prereqGrid.Flush();
		}

		public override bool Suspend
		{
			get
			{
				return base.Suspend;
			}
			set
			{
				base.Suspend = value;
				incompatGrid.Suspend = value;
				prereqGrid.Suspend = value;
			}
		}

		private string currentRelation;
		private const string incompat = "Incompatible";
		private const string prereq = "Prerequisite";

		private const string wantString = "";
		private const string dontWantString = "";
	
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.ContextMenu prereqContextMenu;
		private System.Windows.Forms.ContextMenu incompatMenu;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.GroupBox prodMatrixBox;
		private System.Windows.Forms.TabPage incompatiblePage;
		private System.Windows.Forms.TabPage prereqPage;
		private MrktSimGrid incompatGrid;
		private MrktSimGrid prereqGrid;
		private System.Windows.Forms.Label wantLabel;
		private System.Windows.Forms.TabControl prodDependTab;
		private System.Windows.Forms.Button createRelButton;
		private Common.Utilities.ProductPicker wantProductPicker;
		private Common.Utilities.ProductPicker haveProductPicker;
		private System.Windows.Forms.Label haveLabel;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ProductMatrixControl(ModelDb db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			currentRelation = prereq;

			wantProductPicker.Db = theDb;
			haveProductPicker.Db = theDb;

			incompatGrid.Table = theDb.Data.product_matrix;
			incompatGrid.RowFilter = "value = '" + incompat + "'";
			incompatGrid.DescriptionWindow = false;
			
			prereqGrid.Table = theDb.Data.product_matrix;
			prereqGrid.RowFilter = "value = '" + prereq + "'";
			prereqGrid.DescriptionWindow = false;

			this.haveLabel.Text = "I need this product...";
			this.wantLabel.Text = "To buy this product...";

			createTableStyle();
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
			this.prereqContextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.incompatMenu = new System.Windows.Forms.ContextMenu();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.prodMatrixBox = new System.Windows.Forms.GroupBox();
			this.createRelButton = new System.Windows.Forms.Button();
			this.wantLabel = new System.Windows.Forms.Label();
			this.haveLabel = new System.Windows.Forms.Label();
			this.wantProductPicker = new Common.Utilities.ProductPicker();
			this.haveProductPicker = new Common.Utilities.ProductPicker();
			this.prodDependTab = new System.Windows.Forms.TabControl();
			this.prereqPage = new System.Windows.Forms.TabPage();
			this.prereqGrid = new MrktSimGrid();
			this.incompatiblePage = new System.Windows.Forms.TabPage();
			this.incompatGrid = new MrktSimGrid();
			this.prodMatrixBox.SuspendLayout();
			this.prodDependTab.SuspendLayout();
			this.prereqPage.SuspendLayout();
			this.incompatiblePage.SuspendLayout();
			this.SuspendLayout();
			// 
			// prereqContextMenu
			// 
			this.prereqContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "Delete relation";
			// 
			// incompatMenu
			// 
			this.incompatMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem2});
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "remove relation";
			// 
			// prodMatrixBox
			// 
			this.prodMatrixBox.Controls.Add(this.createRelButton);
			this.prodMatrixBox.Controls.Add(this.wantLabel);
			this.prodMatrixBox.Controls.Add(this.haveLabel);
			this.prodMatrixBox.Controls.Add(this.wantProductPicker);
			this.prodMatrixBox.Controls.Add(this.haveProductPicker);
			this.prodMatrixBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.prodMatrixBox.Location = new System.Drawing.Point(0, 0);
			this.prodMatrixBox.Name = "prodMatrixBox";
			this.prodMatrixBox.Size = new System.Drawing.Size(704, 128);
			this.prodMatrixBox.TabIndex = 4;
			this.prodMatrixBox.TabStop = false;
			// 
			// createRelButton
			// 
			this.createRelButton.Location = new System.Drawing.Point(552, 40);
			this.createRelButton.Name = "createRelButton";
			this.createRelButton.Size = new System.Drawing.Size(96, 23);
			this.createRelButton.TabIndex = 4;
			this.createRelButton.Text = "Create Relation";
			this.createRelButton.Click += new System.EventHandler(this.createRelButton_Click);
			// 
			// wantLabel
			// 
			this.wantLabel.Location = new System.Drawing.Point(368, 16);
			this.wantLabel.Name = "wantLabel";
			this.wantLabel.Size = new System.Drawing.Size(144, 16);
			this.wantLabel.TabIndex = 3;
			this.wantLabel.Text = "To buy this product....";
			// 
			// haveLabel
			// 
			this.haveLabel.Location = new System.Drawing.Point(112, 16);
			this.haveLabel.Name = "haveLabel";
			this.haveLabel.Size = new System.Drawing.Size(144, 16);
			this.haveLabel.TabIndex = 2;
			this.haveLabel.Text = "I need this product...";
			// 
			// wantProductPicker
			// 
			this.wantProductPicker.Location = new System.Drawing.Point(296, 40);
			this.wantProductPicker.Name = "wantProductPicker";
			this.wantProductPicker.ProductID = -1;
			this.wantProductPicker.Size = new System.Drawing.Size(216, 56);
			this.wantProductPicker.TabIndex = 1;
			// 
			// haveProductPicker
			// 
			
			this.haveProductPicker.Location = new System.Drawing.Point(32, 40);
			this.haveProductPicker.Name = "haveProductPicker";
			this.haveProductPicker.ProductID = -1;
			this.haveProductPicker.Size = new System.Drawing.Size(216, 56);
			this.haveProductPicker.TabIndex = 0;
			// 
			// prodDependTab
			// 
			this.prodDependTab.Controls.Add(this.prereqPage);
			this.prodDependTab.Controls.Add(this.incompatiblePage);
			this.prodDependTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.prodDependTab.Location = new System.Drawing.Point(0, 128);
			this.prodDependTab.Name = "prodDependTab";
			this.prodDependTab.SelectedIndex = 0;
			this.prodDependTab.Size = new System.Drawing.Size(704, 176);
			this.prodDependTab.TabIndex = 5;
			this.prodDependTab.SelectedIndexChanged += new System.EventHandler(this.prodDependTab_SelectedIndexChanged);
			// 
			// prereqPage
			// 
			this.prereqPage.Controls.Add(this.prereqGrid);
			this.prereqPage.Location = new System.Drawing.Point(4, 22);
			this.prereqPage.Name = "prereqPage";
			this.prereqPage.Size = new System.Drawing.Size(696, 150);
			this.prereqPage.TabIndex = 1;
			this.prereqPage.Text = "Prequisite Products";
			// 
			// prereqGrid
			// 
			this.prereqGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.prereqGrid.Location = new System.Drawing.Point(0, 0);
			this.prereqGrid.Name = "prereqGrid";
			this.prereqGrid.RowFilter = "";
			this.prereqGrid.RowID = null;
			this.prereqGrid.RowName = null;
			this.prereqGrid.Size = new System.Drawing.Size(696, 150);
			this.prereqGrid.Sort = "";
			this.prereqGrid.TabIndex = 0;
			// 
			// incompatiblePage
			// 
			this.incompatiblePage.Controls.Add(this.incompatGrid);
			this.incompatiblePage.Location = new System.Drawing.Point(4, 22);
			this.incompatiblePage.Name = "incompatiblePage";
			this.incompatiblePage.Size = new System.Drawing.Size(696, 150);
			this.incompatiblePage.TabIndex = 0;
			this.incompatiblePage.Text = "Incompatible Products";
			// 
			// incompatGrid
			// 
			this.incompatGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.incompatGrid.Location = new System.Drawing.Point(0, 0);
			this.incompatGrid.Name = "incompatGrid";
			this.incompatGrid.RowFilter = "";
			this.incompatGrid.RowID = null;
			this.incompatGrid.RowName = null;
			this.incompatGrid.Size = new System.Drawing.Size(696, 150);
			this.incompatGrid.Sort = "";
			this.incompatGrid.TabIndex = 0;
			// 
			// ProductMatrixControl
			// 
			this.Controls.Add(this.prodDependTab);
			this.Controls.Add(this.prodMatrixBox);
			this.Name = "ProductMatrixControl";
			this.Size = new System.Drawing.Size(704, 304);
			this.prodMatrixBox.ResumeLayout(false);
			this.prodDependTab.ResumeLayout(false);
			this.prereqPage.ResumeLayout(false);
			this.incompatiblePage.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		// TODO externalize these strings
		private void createTableStyle()
		{
			incompatGrid.Clear();
			prereqGrid.Clear();

			// prereq..
			// have product
			prereqGrid.AddTextColumn("have_product_name", "I need this product...", true);
			
			// want product
			prereqGrid.AddTextColumn("want_product_name", "To buy this product...", true);


			// incompat..
			// have product
			incompatGrid.AddTextColumn("have_product_name", "If I have this product...", true);
			
			// want product
			incompatGrid.AddTextColumn("want_product_name", "I do not want this product", true);

			incompatGrid.Reset();
			prereqGrid.Reset();
		}

		private void prodDependTab_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (prodDependTab.SelectedTab == this.incompatiblePage)
			{
				this.haveLabel.Text = "If I have this product...";
				this.wantLabel.Text = "I do not want this product...";
				currentRelation = incompat;
			}
			else if (prodDependTab.SelectedTab == this.prereqPage)
			{
				this.haveLabel.Text = "I need this product...";
				this.wantLabel.Text = "To buy this product...";
				currentRelation = prereq;
			}
		}

		private string  relationExists(int haveID, int wantID)
		{
			foreach(MrktSimDBSchema.product_matrixRow row in theDb.Data.product_matrix)
			{
				if (row.have_product_id == haveID &&
					row.want_product_id == wantID)
				{
					return row.value;
				}
			}

			return null;
		}

		private void createRelButton_Click(object sender, System.EventArgs e)
		{
			int wantProductID = this.wantProductPicker.ProductID;
			int haveProductID = this.haveProductPicker.ProductID;

			if (wantProductID == ModelDb.AllID ||
				haveProductID == ModelDb.AllID)
			{
				MessageBox.Show("Please select one product");
				return;
			}

			if (wantProductID == haveProductID)
			{
				MessageBox.Show("A product cannot be in a relation with itself");
				return;
			}

			string existingRel = relationExists(haveProductID, wantProductID);

			if (existingRel != null)
			{
				if (existingRel == prereq)
					MessageBox.Show("Products have an prerequisite relation, please delete relation first");	
				else
					MessageBox.Show("Products are already incompatible, please delete relation first");

				return;
			}

			theDb.CreateProductMatrix(haveProductID, wantProductID, currentRelation);
		}
	}
}
