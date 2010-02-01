using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;

using ModelView;



using MrktSimDb;



namespace ModelView.Dialogs

{

	/// <summary>

	/// Summary description for CalibrationView.

	/// </summary>

	public class CalibrationView : System.Windows.Forms.Form, ModelViewInterface

	{

		public bool HasChanges()

		{

			return theDb.HasChanges();

		}



		public void SaveModel()

		{		

			//			// save data

			//			scenarioMarketPlanGrid.Suspend = true;

			//			scenarioFactorsGrid.Suspend = true;



			theDb.Update();

		}





		ModelDb theDb = null;

		private System.Windows.Forms.Button button1;



		public ModelDb Db

		{

			set

			{

				theDb = value;



//				mrktSimGrid1.Table = theDb.Data.scenario_parameter;

//				mrktSimGrid1.AllowDelete = false;

//				mrktSimGrid1.DescriptionWindow = false;

//

//				createTableStyle();

			}

		}

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;



		public CalibrationView()

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

			this.button1 = new System.Windows.Forms.Button();

			this.SuspendLayout();

			// 

			// button1

			// 

			this.button1.Location = new System.Drawing.Point(352, 256);

			this.button1.Name = "button1";

			this.button1.TabIndex = 0;

			this.button1.Text = "Accept";

			// 

			// CalibrationView

			// 

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.ClientSize = new System.Drawing.Size(560, 294);

			this.Controls.Add(this.button1);

			this.Name = "CalibrationView";

			this.Text = "Scenario Parameters for  ";

			this.ResumeLayout(false);



		}

		#endregion



//		private void createTableStyle()

//		{

//			mrktSimGrid1.Clear();

//

//			mrktSimGrid1.AddComboBoxColumn("scenario_id", theDb.Data.scenario, "name", "scenario_id", true);

//			mrktSimGrid1.AddComboBoxColumn("param_id", theDb.Data.model_parameter, "name", "id", true);

//			mrktSimGrid1.AddNumericColumn("aValue");

//

//			mrktSimGrid1.Reset();

//		}

	}

}

