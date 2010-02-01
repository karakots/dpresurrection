using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using BrandManager.Dialogues;
using MrktSimDb;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for EditScenario.
	/// </summary>
	public class EditScenario : System.Windows.Forms.UserControl, Wizard
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button addDataModButton;
		private System.Windows.Forms.Button removeDataModButton;
		private System.Windows.Forms.ListBox dataModBox;
		private System.Windows.Forms.GroupBox scenarioGrpBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public EditScenario()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			allParams = new ArrayList();

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
			this.panel1 = new System.Windows.Forms.Panel();
			this.addDataModButton = new System.Windows.Forms.Button();
			this.removeDataModButton = new System.Windows.Forms.Button();
			this.scenarioGrpBox = new System.Windows.Forms.GroupBox();
			this.dataModBox = new System.Windows.Forms.ListBox();
			this.panel1.SuspendLayout();
			this.scenarioGrpBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.addDataModButton);
			this.panel1.Controls.Add(this.removeDataModButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 264);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(432, 48);
			this.panel1.TabIndex = 3;
			// 
			// addDataModButton
			// 
			this.addDataModButton.Location = new System.Drawing.Point(208, 8);
			this.addDataModButton.Name = "addDataModButton";
			this.addDataModButton.Size = new System.Drawing.Size(160, 23);
			this.addDataModButton.TabIndex = 4;
			this.addDataModButton.Text = "Add new data modification...";
			this.addDataModButton.Click += new System.EventHandler(this.addDataModButton_Click);
			// 
			// removeDataModButton
			// 
			this.removeDataModButton.Location = new System.Drawing.Point(32, 8);
			this.removeDataModButton.Name = "removeDataModButton";
			this.removeDataModButton.Size = new System.Drawing.Size(128, 24);
			this.removeDataModButton.TabIndex = 3;
			this.removeDataModButton.Text = "Remove modification";
			this.removeDataModButton.Click += new System.EventHandler(this.removeDataModButton_Click);
			// 
			// scenarioGrpBox
			// 
			this.scenarioGrpBox.Controls.Add(this.dataModBox);
			this.scenarioGrpBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scenarioGrpBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.scenarioGrpBox.Location = new System.Drawing.Point(0, 0);
			this.scenarioGrpBox.Name = "scenarioGrpBox";
			this.scenarioGrpBox.Size = new System.Drawing.Size(432, 264);
			this.scenarioGrpBox.TabIndex = 4;
			this.scenarioGrpBox.TabStop = false;
			this.scenarioGrpBox.Text = "Data Modifications";
			// 
			// dataModBox
			// 
			this.dataModBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataModBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dataModBox.ItemHeight = 15;
			this.dataModBox.Location = new System.Drawing.Point(3, 18);
			this.dataModBox.Name = "dataModBox";
			this.dataModBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.dataModBox.Size = new System.Drawing.Size(426, 229);
			this.dataModBox.TabIndex = 1;
			// 
			// EditScenario
			// 
			this.Controls.Add(this.scenarioGrpBox);
			this.Controls.Add(this.panel1);
			this.Name = "EditScenario";
			this.Size = new System.Drawing.Size(432, 312);
			this.panel1.ResumeLayout(false);
			this.scenarioGrpBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Wizard Members

		public bool Next()
		{
			// TODO:  Add AddCompetitor.Next implementation
			return true;
		}

		public bool Back()
		{
			MessageBox.Show("Please continue with this scenario before creating a new one","Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return false;
		}

		public void Start()
		{
			initializeDataModBox();
		}

		public void End()
		{
			db.Update();
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

		#region UI
		private void addDataModButton_Click(object sender, System.EventArgs e)
		{
			ModifyData dlg = new ModifyData();

			dlg.Db = Db;

			dlg.Scenario = CurrentScenario;


			dlg.ShowDialog();

			this.initializeDataModBox();
		}
		#endregion

		#region Properties
		private Database db;
		public Database Db
		{
			set
			{
				db = value;
			}

			get
			{
				return db;
			}
		}

		
		public MrktSimDb.MrktSimDBSchema.scenarioRow CurrentScenario
		{
			set
			{
				scenario = value;

				if (scenario != null)
				{
					scenarioGrpBox.Text = "Data Modifications for Scenario " + scenario.name;
				}

				initializeDataModBox();

			}

			get
			{
				return scenario;
			}
		}

		public string ParameterList
		{
			get
			{
				string list = "";
				foreach(Parameter param in dataModBox.Items)
				{
					list += param.descr + "\n";
				}

				return list;
			}
		}

		#endregion

		#region private data and methods
		private ArrayList allParams;
		private MrktSimDb.MrktSimDBSchema.scenarioRow scenario = null;

		private void initializeDataModBox()
		{
			this.dataModBox.Items.Clear();

			if (CurrentScenario == null)
				return;

			string scenarioQuery = "scenario_id = " + this.CurrentScenario.scenario_id;

			ArrayList paramsDone = new ArrayList();
			allParams.Clear();

			DataRow[] rows = Db.Data.scenario_parameter.Select(scenarioQuery, "", DataViewRowState.CurrentRows);
			foreach(MrktSimDBSchema.scenario_parameterRow scenarioParm in rows)
			{
				
				MrktSimDBSchema.model_parameterRow parm = scenarioParm.model_parameterRow;

				double scale = scenarioParm.aValue;
				string type = parm.table_name;
				string val = parm.col_name;

				if (type == "market_plan")
				{
					// determine if parm is actually usable by use
					MrktSimDBSchema.market_planRow plan = Db.Data.market_plan.FindByid(parm.row_id);

					MrktSimDBSchema.productRow product = plan.productRow;

					if ((plan.type == (byte) PlanType.ProdEvent) ||
						(plan.type == (byte) PlanType.TaskEvent))
					{
						val = "Demand Modification";
					}
					else
					{
						switch((PlanType) plan.type)
						{
							case PlanType.Price:
								type = "Price";

								if (val == "parm1")
									val = "Value";
								else if (val == "parm2")
									val = "Mark UP";
								else if (val == "parm3")
									val ="Periodic Value";
								else if (val == "parm4")
									val = "Distribution";
								break;

							case PlanType.Distribution:
								type = "Distribution";
								if (val == "parm1")
									val = "Awareness";
								else if (val == "parm2")
									val = "Pesuasion";
								else if (val == "parm3")
									val = "Percent";
								break;

							case PlanType.Display:
								type = "Display";
								if (val == "parm1")
									val = "Awareness";
								else if (val == "parm2")
									val = "Pesuasion";
								else if (val == "parm3")
									val = "Distribution";
								break;

							case PlanType.Media:
								type = "Media";
								if (val == "parm1")
									val = "Awareness";
								else if (val == "parm2")
									val = "Pesuasion";
								else if (val == "parm3")
									val = "GRP";
								break;

							case PlanType.Coupons:
								type = "Coupon";
								if (val == "parm1")
									val = "Awareness S";
								else if (val == "parm2")
									val = "Pesuasion";
								else if (val == "parm3")
									val = "Penetration";
								break;

							case PlanType.Market_Utility:
								type = "Market Utility";
								if (val == "parm1")
									val = "Awareness";
								else if (val == "parm2")
									val = "Pesuasion";
								else if (val == "parm3")
									val = "Utility";
								else if (val == "parm4")
									val = "Distribution";
								break;
						}

						string what = null;
						double percent = 0;

						if (scale > 1)
						{
							what = "Increasing ";
							percent = 100 * (scale - 1);
						}
						else
						{
							what = "Decreasing ";
							percent = 100 * (1 - scale);
						}
					
						what += product.product_name + " " + type + " "+ val +" by " + percent.ToString() + " percent";
						
						Parameter aParm = new Parameter(scenarioParm, what);

						allParams.Add(aParm);

						if(paramsDone.Contains(what))
						{
							continue;
						}
						else
						{
							paramsDone.Add(what);
						}

						this.dataModBox.Items.Add(aParm);

					}
				}
			}
			this.dataModBox.DisplayMember = "descr";
			this.dataModBox.Refresh();
		}

		private void removeDataModButton_Click(object sender, System.EventArgs e)
		{
			//TBD very very inefficient
			foreach(Parameter sel in this.dataModBox.SelectedItems)
			{
				foreach(Parameter parm in this.allParams)
				{
					if(parm.descr.CompareTo(sel.descr) == 0)
					{
						MrktSimDBSchema.scenario_parameterRow scenarioParm = parm.parm;
						MrktSimDBSchema.model_parameterRow modelParm = scenarioParm.model_parameterRow;
						int parm_id = scenarioParm.param_id;
						int scenario_id = scenarioParm.scenario_id;
						scenarioParm.Delete();
						string query = "param_id = " + parm_id + " AND scenario_id <> " + scenario_id;
						DataRow[] rows = db.Data.scenario_parameter.Select(query,"",DataViewRowState.CurrentRows);

						if(rows.Length == 0)
						{
							modelParm.Delete();
						}
					}
				}
			}

			db.Update();

			initializeDataModBox();

		}

		private class Parameter
		{
			private MrktSimDBSchema.scenario_parameterRow myRow;
			private string myDescr;
			public Parameter(MrktSimDBSchema.scenario_parameterRow parm, string descr)
			{
				myRow = parm;
				myDescr = descr;
			}

			public MrktSimDBSchema.scenario_parameterRow parm
			{
				get
				{
					return myRow;
				}
			}

			public string descr
			{
				get
				{
					return myDescr;
				}
			}
		}

		#endregion
	}
}
