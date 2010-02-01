using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;
using Common.Dialogs;

using MarketSimUtilities;
namespace Common.Utilities
{
	/// <summary>
	/// Summary description for MrktPlanComponent.
	/// </summary>
	public class MrktPlanComponent : MrktSimControl
	{
		private System.Windows.Forms.Button createPlanbutton;
		private System.Windows.Forms.ListBox mrktPlanBox;
		private System.Windows.Forms.GroupBox planBox;
		private System.Data.DataView planView;
		private System.Windows.Forms.Label productLabel;
		private System.Windows.Forms.Button editButton;
		private System.Windows.Forms.Button copyButton;
		private System.Windows.Forms.Label prodText;
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.Button mrktPlanParm;
		private System.Windows.Forms.CheckBox showAllCheckBox;
		private ProductTree productTree;
		private System.Windows.Forms.ContextMenu ProductTreeMenu;
		private System.Windows.Forms.MenuItem SelectByType;
		private System.Windows.Forms.MenuItem[] Product_Types;
		private System.Windows.Forms.Splitter splitter1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MrktPlanComponent()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			productTree.SelectedItemsChanged +=new ProductTree.SelectedItems(productTree_SelectedItemsChanged);
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
			this.createPlanbutton = new System.Windows.Forms.Button();
			this.mrktPlanBox = new System.Windows.Forms.ListBox();
			this.planBox = new System.Windows.Forms.GroupBox();
			this.showAllCheckBox = new System.Windows.Forms.CheckBox();
			this.mrktPlanParm = new System.Windows.Forms.Button();
			this.deleteButton = new System.Windows.Forms.Button();
			this.copyButton = new System.Windows.Forms.Button();
			this.editButton = new System.Windows.Forms.Button();
			this.prodText = new System.Windows.Forms.Label();
			this.productLabel = new System.Windows.Forms.Label();
			this.planView = new System.Data.DataView();
			this.productTree = new MarketSimUtilities.ProductTree();
			this.ProductTreeMenu = new System.Windows.Forms.ContextMenu();
			this.SelectByType = new System.Windows.Forms.MenuItem();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.planBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.planView)).BeginInit();
			this.SuspendLayout();
			// 
			// createPlanbutton
			// 
			this.createPlanbutton.Location = new System.Drawing.Point(16, 56);
			this.createPlanbutton.Name = "createPlanbutton";
			this.createPlanbutton.Size = new System.Drawing.Size(45, 23);
			this.createPlanbutton.TabIndex = 16;
			this.createPlanbutton.Text = "New";
			this.createPlanbutton.Click += new System.EventHandler(this.createPlanbutton_Click);
			// 
			// mrktPlanBox
			// 
			this.mrktPlanBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mrktPlanBox.Location = new System.Drawing.Point(64, 56);
			this.mrktPlanBox.Name = "mrktPlanBox";
			this.mrktPlanBox.Size = new System.Drawing.Size(136, 160);
			this.mrktPlanBox.TabIndex = 15;
			this.mrktPlanBox.SelectedIndexChanged += new System.EventHandler(this.planBox_SelectedIndexChanged);
			// 
			// planBox
			// 
			this.planBox.Controls.Add(this.showAllCheckBox);
			this.planBox.Controls.Add(this.mrktPlanParm);
			this.planBox.Controls.Add(this.deleteButton);
			this.planBox.Controls.Add(this.copyButton);
			this.planBox.Controls.Add(this.editButton);
			this.planBox.Controls.Add(this.createPlanbutton);
			this.planBox.Controls.Add(this.prodText);
			this.planBox.Controls.Add(this.productLabel);
			this.planBox.Controls.Add(this.mrktPlanBox);
			this.planBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.planBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.planBox.Location = new System.Drawing.Point(280, 0);
			this.planBox.Name = "planBox";
			this.planBox.Size = new System.Drawing.Size(208, 224);
			this.planBox.TabIndex = 20;
			this.planBox.TabStop = false;
			this.planBox.Text = "Select --------- plan";
			// 
			// showAllCheckBox
			// 
			this.showAllCheckBox.Location = new System.Drawing.Point(72, 40);
			this.showAllCheckBox.Name = "showAllCheckBox";
			this.showAllCheckBox.Size = new System.Drawing.Size(152, 16);
			this.showAllCheckBox.TabIndex = 36;
			this.showAllCheckBox.Text = "Show All";
			this.showAllCheckBox.CheckedChanged += new System.EventHandler(this.showAllCheckBox_CheckedChanged);
			// 
			// mrktPlanParm
			// 
			this.mrktPlanParm.Location = new System.Drawing.Point(16, 128);
			this.mrktPlanParm.Name = "mrktPlanParm";
			this.mrktPlanParm.Size = new System.Drawing.Size(45, 23);
			this.mrktPlanParm.TabIndex = 35;
			this.mrktPlanParm.Text = "Parms";
			this.mrktPlanParm.Click += new System.EventHandler(this.mrktPlanParm_Click);
			// 
			// deleteButton
			// 
			this.deleteButton.Location = new System.Drawing.Point(16, 192);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(45, 23);
			this.deleteButton.TabIndex = 34;
			this.deleteButton.Text = "Delete";
			this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
			// 
			// copyButton
			// 
			this.copyButton.Location = new System.Drawing.Point(16, 104);
			this.copyButton.Name = "copyButton";
			this.copyButton.Size = new System.Drawing.Size(45, 23);
			this.copyButton.TabIndex = 24;
			this.copyButton.Text = "Copy";
			this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
			// 
			// editButton
			// 
			this.editButton.Location = new System.Drawing.Point(16, 80);
			this.editButton.Name = "editButton";
			this.editButton.Size = new System.Drawing.Size(45, 23);
			this.editButton.TabIndex = 23;
			this.editButton.Text = "Edit";
			this.editButton.Click += new System.EventHandler(this.editButton_Click);
			// 
			// prodText
			// 
			this.prodText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.prodText.Location = new System.Drawing.Point(64, 16);
			this.prodText.Name = "prodText";
			this.prodText.Size = new System.Drawing.Size(240, 16);
			this.prodText.TabIndex = 31;
			this.prodText.Text = "product";
			this.prodText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// productLabel
			// 
			this.productLabel.Location = new System.Drawing.Point(8, 16);
			this.productLabel.Name = "productLabel";
			this.productLabel.Size = new System.Drawing.Size(48, 16);
			this.productLabel.TabIndex = 22;
			this.productLabel.Text = "Product:";
			this.productLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// productTree
			// 
			this.productTree.CheckBoxes = true;
			this.productTree.ContextMenu = this.ProductTreeMenu;
			this.productTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.productTree.ImageIndex = -1;
			this.productTree.Location = new System.Drawing.Point(0, 0);
			this.productTree.Name = "productTree";
			this.productTree.SelectedImageIndex = -1;
			this.productTree.Size = new System.Drawing.Size(280, 224);
			this.productTree.TabIndex = 21;
			// 
			// ProductTreeMenu
			// 
			this.ProductTreeMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							this.SelectByType});
			// 
			// SelectByType
			// 
			this.SelectByType.Index = 0;
			this.SelectByType.Text = "Select By Type";
			// 
			// splitter1
			// 
			this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter1.Location = new System.Drawing.Point(280, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 224);
			this.splitter1.TabIndex = 22;
			this.splitter1.TabStop = false;
			// 
			// MrktPlanComponent
			// 
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.planBox);
			this.Controls.Add(this.productTree);
			this.Name = "MrktPlanComponent";
			this.Size = new System.Drawing.Size(488, 224);
			this.planBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.planView)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

	
		#region public properties
		public override bool Suspend
		{
			get
			{
				return base.Suspend;
			}
			set
			{
				base.Suspend = value;
			}
		}

		public MrktSimDBSchema.market_planRow SelectedPlan
		{
			get
			{
				if (mrktPlanBox.SelectedItem == null)
					return null;

				return (MrktSimDBSchema.market_planRow) ((DataRowView) mrktPlanBox.SelectedItem).Row;
			}

			set
			{
				if( value == null)
					return;

				mrktPlanBox.SelectedValue = value.id;
			}
		}

		public delegate void CurrentMarketPlan(ArrayList prodList, MrktSimDBSchema.market_planRow plan);

		public event CurrentMarketPlan MarketPlanChanged;

		ModelDb.PlanType planType = ModelDb.PlanType.MarketPlan;

		public ModelDb.PlanType Type
		{
			get
			{
				return planType;
			}

			set
			{
				planType = value;

				mrktPlanParm.Visible = true;

				switch (planType)
				{
					case ModelDb.PlanType.MarketPlan:
						planBox.Text = "Select Marketing Plan";
						mrktPlanParm.Visible = false;
						break;
					case ModelDb.PlanType.Price:
						planBox.Text = "Select Price Plan";
						break;

					case ModelDb.PlanType.Distribution:
						planBox.Text = "Select Distribution Plan";
						break;

					case ModelDb.PlanType.Display:
						planBox.Text = "Select Display Plan";
						break;

					case ModelDb.PlanType.Media:
						planBox.Text = "Select Media Plan";
						break;

					case ModelDb.PlanType.Coupons:
						planBox.Text = "Select Coupon Plan";
						break;

					case ModelDb.PlanType.ProdEvent:
						planBox.Text = "Select External Factor";
						break;
				}

				string query = "type = " + ((int) planType);
				planView.RowFilter = query;
			}
		}

		public override ModelDb Db
		{
			set
			{
				base.Db = value;

				planView.Table = theDb.Data.market_plan;
				string query = "type = " + ((int) planType);
				planView.RowFilter = query;
			
				mrktPlanBox.DataSource = planView;
				mrktPlanBox.DisplayMember = "name";
				mrktPlanBox.ValueMember = "id";

				this.productTree.Db = theDb;

				this.SelectByType.MenuItems.Clear();

				DataRow[] Types = theDb.Data.product_type.Select("","",DataViewRowState.CurrentRows);

				Product_Types = new System.Windows.Forms.MenuItem[Types.Length];
			
				int i = 0;
				foreach(MrktSimDBSchema.product_typeRow row in Types)
				{
					System.Windows.Forms.MenuItem menuItem = new MenuItem(row.type_name);
					menuItem.Click += new System.EventHandler(this.SelectByType_Click);
					Product_Types[i] = menuItem;
					i++;
				}

				this.SelectByType.MenuItems.AddRange(Product_Types);

				mrktPlanBox.Update();

				this.showAllCheckBox.Checked = true;
			}
		}

		public bool ShowAllCheckBox
		{
			set
			{
				this.showAllCheckBox.Visible = value;

				// if not visible make sure not checked
				if (!value)
					showAllCheckBox.Checked = false;
			}
		}

		#endregion

		private void enablePlanControls(bool on)
		{
			editButton.Enabled = on;
			copyButton.Enabled = on;
			deleteButton.Enabled = on;
			mrktPlanParm.Enabled = on;
		}


		private void planBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (Suspend)
				return;

			if (this.showAllCheckBox.Checked)
			{
				enablePlanControls(false);

				ArrayList list = this.productTree.CheckedProducts;

				prodText.Text = "Showing All Products";
	
				if (MarketPlanChanged != null)
					MarketPlanChanged(list, null);
			}
			else if (mrktPlanBox.SelectedItem == null)
			{
				prodText.Text = null;

				// no selected plan disable everything
				enablePlanControls(false);

				// get current product if there is one
				
				if (MarketPlanChanged != null)
					MarketPlanChanged(null, null);
			}
			else
			{
				enablePlanControls(true);

				MrktSimDBSchema.market_planRow plan = (MrktSimDBSchema.market_planRow)
					((DataRowView) mrktPlanBox.SelectedItem).Row;

				prodText.Text = (theDb.Data.product.FindByproduct_id(plan.product_id)).product_name;

				if (MarketPlanChanged != null)
					MarketPlanChanged(null, plan);
			}
		}

		private void productTree_SelectedItemsChanged(ArrayList prodList)
		{
			if (Suspend)
				return;

			string query = "type = " + ((int) planType);

			bool firstTime = true;
			foreach(DataRow row in prodList)
			{
				if (firstTime)
				{
					query += " AND (product_id = " + row["product_id"].ToString();
					firstTime = false;
				}
				else
				{

					query +=  " OR product_id = " + row["product_id"].ToString();
				}
			}

			if (prodList.Count > 0)
			{
				query += " )";
			}

			planView.RowFilter = query;
			planBox_SelectedIndexChanged(this, new System.EventArgs());
		}

		private void createPlanbutton_Click(object sender, System.EventArgs e)
		{
			CreateComponentPlan dlg = new CreateComponentPlan(theDb);
			dlg.Type = planType;

			if(planType == ModelDb.PlanType.Media || planType == ModelDb.PlanType.MarketPlan)
			{
				dlg.leafOnly = false;
			}

			// dlg.ProductID = this.productPicker.ProductID;

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{

				SelectedPlan = dlg.CurrentPlan;

				this.showAllCheckBox.Checked = false;

			}
		}

		private void editButton_Click(object sender, System.EventArgs e)
		{
			MrktSimDBSchema.market_planRow plan = SelectedPlan;

			if (plan == null)
				return;

			CreateComponentPlan dlg = new CreateComponentPlan(theDb);
			dlg.Type = planType;
			dlg.CurrentPlan = plan;

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				if (MarketPlanChanged != null)
					MarketPlanChanged(null, plan);
			}
		}

		/// <summary>
		/// Create a deep copy of the plan data
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void copyButton_Click(object sender, System.EventArgs e)
		{
			MrktSimDBSchema.market_planRow plan = SelectedPlan;

			if (plan != null)
			{
				theDb.CopyPlan(plan);
			}

		}

		private void deleteButton_Click(object sender, System.EventArgs e)
		{
			MrktSimDBSchema.market_planRow plan = SelectedPlan;

			if (plan != null)
				plan.Delete();
		}

		private void mrktPlanParm_Click(object sender, System.EventArgs e)
		{
			MrktSimDBSchema.market_planRow plan = SelectedPlan;

			if (plan == null)
				return;

			MarketPlanParameter dlg = new MarketPlanParameter(plan);

			dlg.Db = theDb;

			dlg.ShowDialog();
				
		}

		private void showAllCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			if (showAllCheckBox.Checked)
			{
				// disable selection of plan box
				mrktPlanBox.Enabled = false;
			}
			else
			{
				// enable selection of plan box
				mrktPlanBox.Enabled = true;
			}

			planBox_SelectedIndexChanged(sender, e);
		}

		private void SelectByType_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.MenuItem item = (System.Windows.Forms.MenuItem) sender;
			productTree.SelectByType(item.Text);
		}
		public bool ShowAll
		{
			set
			{
				showAllCheckBox.Checked = value;
			}
			get
			{
				return 	showAllCheckBox.Checked;
			}
		}
	}
}
