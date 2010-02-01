using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MarketSimUtilities; // ProductTree

using MrktSimDb;

namespace BrandManager.Dialogues
{
	/// <summary>
	/// Summary description for EditScenario.
	/// </summary>
	public class ModifyData : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button OKbutton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox todoBox;
		private System.Windows.Forms.NumericUpDown percentChange;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label percentlabel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton varyValues;
		private System.Windows.Forms.RadioButton changeBy;
		private System.Windows.Forms.NumericUpDown percentMax;
		private System.Windows.Forms.NumericUpDown percentMin;
		private ProductTree prodTree;
		private System.Windows.Forms.DateTimePicker startDate;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.DateTimePicker endDate;
		private System.Windows.Forms.Label typeLab;
		private System.Windows.Forms.ComboBox typeBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ModifyData()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
	
			todoBox.SelectedIndex = 0;
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
			this.OKbutton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.todoBox = new System.Windows.Forms.ComboBox();
			this.percentChange = new System.Windows.Forms.NumericUpDown();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.prodTree = new MarketSimUtilities.ProductTree();
			this.panel1 = new System.Windows.Forms.Panel();
			this.typeBox = new System.Windows.Forms.ComboBox();
			this.typeLab = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.endDate = new System.Windows.Forms.DateTimePicker();
			this.label4 = new System.Windows.Forms.Label();
			this.startDate = new System.Windows.Forms.DateTimePicker();
			this.label3 = new System.Windows.Forms.Label();
			this.percentMax = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.percentMin = new System.Windows.Forms.NumericUpDown();
			this.varyValues = new System.Windows.Forms.RadioButton();
			this.changeBy = new System.Windows.Forms.RadioButton();
			this.percentlabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.percentChange)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.percentMax)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.percentMin)).BeginInit();
			this.SuspendLayout();
			// 
			// OKbutton
			// 
			this.OKbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.OKbutton.Location = new System.Drawing.Point(504, 164);
			this.OKbutton.Name = "OKbutton";
			this.OKbutton.Size = new System.Drawing.Size(40, 23);
			this.OKbutton.TabIndex = 0;
			this.OKbutton.Text = "OK";
			this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(424, 164);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(64, 24);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(288, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Modify";
			// 
			// todoBox
			// 
			this.todoBox.Items.AddRange(new object[] {
														 "Display",
														 "Distribution",
														 "Unpromoted Price",
														 "Promoted Price",
														 "Promoted Price Distribution",
														 "All Media"});
			this.todoBox.Location = new System.Drawing.Point(336, 16);
			this.todoBox.Name = "todoBox";
			this.todoBox.Size = new System.Drawing.Size(136, 21);
			this.todoBox.TabIndex = 3;
			// 
			// percentChange
			// 
			this.percentChange.DecimalPlaces = 2;
			this.percentChange.Location = new System.Drawing.Point(152, 104);
			this.percentChange.Minimum = new System.Decimal(new int[] {
																		  100,
																		  0,
																		  0,
																		  -2147483648});
			this.percentChange.Name = "percentChange";
			this.percentChange.Size = new System.Drawing.Size(64, 20);
			this.percentChange.TabIndex = 7;
			this.percentChange.Value = new System.Decimal(new int[] {
																		10,
																		0,
																		0,
																		0});
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.prodTree);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(560, 206);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "For Product";
			// 
			// prodTree
			// 
			this.prodTree.CheckBoxes = true;
			this.prodTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.prodTree.ImageIndex = -1;
			this.prodTree.Location = new System.Drawing.Point(3, 16);
			this.prodTree.Name = "prodTree";
			this.prodTree.SelectedImageIndex = -1;
			this.prodTree.Size = new System.Drawing.Size(554, 187);
			this.prodTree.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.typeBox);
			this.panel1.Controls.Add(this.typeLab);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.endDate);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.startDate);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.percentMax);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.percentMin);
			this.panel1.Controls.Add(this.varyValues);
			this.panel1.Controls.Add(this.changeBy);
			this.panel1.Controls.Add(this.percentlabel);
			this.panel1.Controls.Add(this.OKbutton);
			this.panel1.Controls.Add(this.cancelButton);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.todoBox);
			this.panel1.Controls.Add(this.percentChange);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 206);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(560, 200);
			this.panel1.TabIndex = 9;
			// 
			// typeBox
			// 
			this.typeBox.Location = new System.Drawing.Point(104, 16);
			this.typeBox.Name = "typeBox";
			this.typeBox.Size = new System.Drawing.Size(136, 21);
			this.typeBox.TabIndex = 20;
			this.typeBox.SelectedIndexChanged += new System.EventHandler(this.typeBox_SelectedIndexChanged);
			// 
			// typeLab
			// 
			this.typeLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.typeLab.Location = new System.Drawing.Point(56, 16);
			this.typeLab.Name = "typeLab";
			this.typeLab.Size = new System.Drawing.Size(40, 16);
			this.typeLab.TabIndex = 19;
			this.typeLab.Text = "Type";
			// 
			// label5
			// 
			this.label5.Enabled = false;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(272, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 23);
			this.label5.TabIndex = 18;
			this.label5.Text = "End Date:";
			this.label5.Visible = false;
			// 
			// endDate
			// 
			this.endDate.Enabled = false;
			this.endDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.endDate.Location = new System.Drawing.Point(360, 56);
			this.endDate.Name = "endDate";
			this.endDate.Size = new System.Drawing.Size(96, 20);
			this.endDate.TabIndex = 17;
			this.endDate.Visible = false;
			// 
			// label4
			// 
			this.label4.Enabled = false;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(64, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 23);
			this.label4.TabIndex = 16;
			this.label4.Text = "Start Date:";
			this.label4.Visible = false;
			// 
			// startDate
			// 
			this.startDate.Enabled = false;
			this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.startDate.Location = new System.Drawing.Point(152, 56);
			this.startDate.Name = "startDate";
			this.startDate.Size = new System.Drawing.Size(96, 20);
			this.startDate.TabIndex = 15;
			this.startDate.Visible = false;
			// 
			// label3
			// 
			this.label3.Enabled = false;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(352, 136);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 16);
			this.label3.TabIndex = 14;
			this.label3.Text = "Percent";
			this.label3.Visible = false;
			// 
			// percentMax
			// 
			this.percentMax.DecimalPlaces = 2;
			this.percentMax.Enabled = false;
			this.percentMax.Location = new System.Drawing.Point(272, 136);
			this.percentMax.Minimum = new System.Decimal(new int[] {
																	   100,
																	   0,
																	   0,
																	   -2147483648});
			this.percentMax.Name = "percentMax";
			this.percentMax.Size = new System.Drawing.Size(64, 20);
			this.percentMax.TabIndex = 13;
			this.percentMax.Value = new System.Decimal(new int[] {
																	 10,
																	 0,
																	 0,
																	 0});
			this.percentMax.Visible = false;
			// 
			// label2
			// 
			this.label2.Enabled = false;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(232, 136);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 16);
			this.label2.TabIndex = 12;
			this.label2.Text = "and";
			this.label2.Visible = false;
			// 
			// percentMin
			// 
			this.percentMin.DecimalPlaces = 2;
			this.percentMin.Enabled = false;
			this.percentMin.Location = new System.Drawing.Point(152, 136);
			this.percentMin.Minimum = new System.Decimal(new int[] {
																	   100,
																	   0,
																	   0,
																	   -2147483648});
			this.percentMin.Name = "percentMin";
			this.percentMin.Size = new System.Drawing.Size(64, 20);
			this.percentMin.TabIndex = 11;
			this.percentMin.Value = new System.Decimal(new int[] {
																	 10,
																	 0,
																	 0,
																	 -2147483648});
			this.percentMin.Visible = false;
			// 
			// varyValues
			// 
			this.varyValues.Enabled = false;
			this.varyValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.varyValues.Location = new System.Drawing.Point(32, 136);
			this.varyValues.Name = "varyValues";
			this.varyValues.TabIndex = 10;
			this.varyValues.Text = "Vary between";
			this.varyValues.Visible = false;
			this.varyValues.CheckedChanged += new System.EventHandler(this.varyValues_CheckedChanged);
			// 
			// changeBy
			// 
			this.changeBy.Checked = true;
			this.changeBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.changeBy.Location = new System.Drawing.Point(32, 104);
			this.changeBy.Name = "changeBy";
			this.changeBy.Size = new System.Drawing.Size(88, 16);
			this.changeBy.TabIndex = 9;
			this.changeBy.TabStop = true;
			this.changeBy.Text = "Change by";
			// 
			// percentlabel
			// 
			this.percentlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.percentlabel.Location = new System.Drawing.Point(224, 104);
			this.percentlabel.Name = "percentlabel";
			this.percentlabel.Size = new System.Drawing.Size(56, 16);
			this.percentlabel.TabIndex = 8;
			this.percentlabel.Text = "Percent";
			// 
			// ModifyData
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(560, 406);
			this.ControlBox = false;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.panel1);
			this.Name = "ModifyData";
			this.Text = "Editing Scenario";
			((System.ComponentModel.ISupportInitialize)(this.percentChange)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.percentMax)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.percentMin)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void varyValues_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.varyValues.Checked)
			{
				this.percentChange.Enabled = false;
				this.percentMin.Enabled = true;
				this.percentMax.Enabled = true;
			}
			else
			{
				
				this.percentChange.Enabled = true;
				this.percentMin.Enabled = false;
				this.percentMax.Enabled = false;
			}
		}

		private void OKbutton_Click(object sender, System.EventArgs e)
		{
			// create appropriate parameters on the selected products

			createParameters();

			this.DialogResult = DialogResult.OK;
		}

		public Database Db
		{
			set
			{
				prodTree.Db = value;
				db = value;
				DataRow[] rows = db.Data.market_plan_type.Select("","",DataViewRowState.CurrentRows);
				typeBox.Items.Clear();
				foreach(MrktSimDBSchema.market_plan_typeRow type in rows)
				{
					if(type.id != 0 && (type.id < 5 || type.id >7))
					{
						typeBox.Items.Add(type);
					}
				}
				typeBox.DisplayMember = "type";
				typeBox.ValueMember = "id";
				typeBox.SelectedIndex = 0;
			}
		}

		private MrktSimDb.MrktSimDBSchema.scenarioRow scenario = null;
		private Database db;
		private string[] parms;
		public MrktSimDb.MrktSimDBSchema.scenarioRow Scenario
		{
			set
			{
				scenario = value;

				this.startDate.Value = scenario.start_date;
				this.endDate.Value = scenario.end_date;

				this.Text = "Editing Scenario " + scenario.name;
			}
		}

		private void createParameters()
		{
			ArrayList products = prodTree.CheckedProducts;
			MrktSimDBSchema.market_plan_typeRow type = (MrktSimDBSchema.market_plan_typeRow)typeBox.SelectedItem;
			int type_id = type.id; 
			string type_name = type.type;
			string var_name = (string)todoBox.SelectedItem;

			foreach(MrktSimDBSchema.productRow product in products)
			{
				string query = "product_id = " + product.product_id + " AND type = " + type_id + " AND end_date > '" + scenario.start_date + "'";
				DataRow[] mrktPlans = db.Data.market_plan.Select(query,"",DataViewRowState.CurrentRows);
				string name = product.product_name + "_" + type_name + "_" + var_name;
				foreach(MrktSimDBSchema.market_planRow mrktPlan in mrktPlans)
				{
					if(scenarioHasPlan(mrktPlan))
					{
						createParameter(mrktPlan, name);
					}
				}
			}
		}

		private bool scenarioHasPlan(MrktSimDBSchema.market_planRow plan)
		{
			plan = getTopPlan(plan);
			string query = "market_plan_id = " + plan.id + " AND scenario_id = " + scenario.scenario_id;
			DataRow[] rows = db.Data.scenario_market_plan.Select(query,"",DataViewRowState.CurrentRows);
			if(rows.Length > 0)
			{
				return true;
			}
			return false;
		}

		private MrktSimDBSchema.market_planRow getTopPlan(MrktSimDBSchema.market_planRow plan)
		{
			string query = "child_id = " + plan.id;
			DataRow[] rows = db.Data.market_plan_tree.Select(query,"",DataViewRowState.CurrentRows);
			query = "id = " + rows[0]["parent_id"];
			rows = db.Data.market_plan.Select(query,"",DataViewRowState.CurrentRows);
			return (MrktSimDBSchema.market_planRow)rows[0];
		}

		private void createParameter(MrktSimDBSchema.market_planRow mrktPlan, string name)
		{
			int index = todoBox.SelectedIndex + 1;
			string parmName = "parm" + index;
			MrktSimDBSchema.model_parameterRow parm = db.ModelParameterExists(mrktPlan, parmName, "id");

				
			if(parm == null)
			{
				parm = db.CreateModelParameter(mrktPlan, parmName, "id");
				parm.name = name;
			}

			MrktSimDBSchema.scenario_parameterRow scenarioParam = db.CreateScenarioParameter(scenario.scenario_id, parm.id);
			scenarioParam.aValue = scenarioParam.aValue * (1 + (((double)this.percentChange.Value)*0.01));
		}

		private void typeBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			parms = null;

			switch((PlanType)((MrktSimDBSchema.market_plan_typeRow)((ComboBox)sender).SelectedItem).id)
			{
				case PlanType.Display:
					parms = new string[] {"Awareness Scale", "Persuasion Scale", "Distribution Scale"};
					break;

				case PlanType.Distribution:
					parms = new string[] {"Awareness Scale", "Persuasion Scale", "Distribution Scale"};
					break;

				case PlanType.Market_Utility:
					parms = new string[] {"Awareness Scale", "Persuasion Scale", "Utility Scale", "Distribution Scale"};
					break;

				case PlanType.Media:
					parms = new string[] {"Awareness Scale", "Persuasion Scale", "GRP Scale"};
					break;

				case PlanType.Coupons:
					parms = new string[] {"Awareness Scale", "Persuasion Scale", "Percent Population Scale", "Redemption Scale"};
					break;

				case PlanType.Price:
					parms = new string[] {"Price Scale", "Mark UP Scale", "Periodic Price Scale", "Percent Distribution Scale"};
					break;

				case PlanType.ProdEvent:
					parms = new string[] {"Modification Scale"};
					break;

				case PlanType.TaskEvent:
					parms = new string[] {"Modification Scale"};
					break;
			}

			todoBox.Items.Clear();
			todoBox.Items.AddRange(parms);
			todoBox.SelectedIndex = 0;
		}
	}
}
