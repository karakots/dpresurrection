using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Common.Utilities;
using Common.Dialogs;
using MrktSimDb;

using MarketSimUtilities;
namespace Common
{
	/// <summary>
	/// Summary description for ModelParameter.
	/// </summary>
	public class ModelParameter : MrktSimControl
	{
        private DataTable paramTable;
        // private Hashtable paramNames;
        private MrktSimGrid mrktSimGrid1;
        private ToolStrip toolStrip1;
        private ToolStripButton deleteBut;

        private static Hashtable marketPlanParamNames;

        public static Hashtable MarketPlanParamNames {
            get { return marketPlanParamNames; }
        }

        static ModelParameter() {
            marketPlanParamNames = new Hashtable();
            marketPlanParamNames.Add( ModelDb.PlanType.Coupons,
               new string[] { "Awareness Scale", "Persuasion Scale", "Percent Population Scale", "Redemption Scale", "Apply to ALL Coupon Plans" } );
            marketPlanParamNames.Add( ModelDb.PlanType.Distribution,
                new string[] { "Awareness Scale", "Persuasion Scale", "Distribution Scale", "Apply to ALL Distribution Plans" } );
            marketPlanParamNames.Add( ModelDb.PlanType.Display,
                new string[] { "Awareness Scale", "Persuasion Scale", "Distribution Scale", "Apply to ALL Display Plans" } );
            marketPlanParamNames.Add( ModelDb.PlanType.Market_Utility,
                new string[] { "Awareness Scale", "Persuasion Scale", "Utility Scale", "Distribution Scale", "Apply to ALL Market Utility Plans" } );
            marketPlanParamNames.Add( ModelDb.PlanType.Media,
                new string[] { "Awareness Scale", "Persuasion Scale", "GRP Scale", "Apply to ALL Media Plans" } );
            marketPlanParamNames.Add( ModelDb.PlanType.Price,
                new string[] { "Price Scale", "Markup Scale", "Periodic Price Scale", "Percent Distribution Scale", "Apply to ALL Price Plans" } );
            marketPlanParamNames.Add( ModelDb.PlanType.ProdEvent,
                new string[] { "Modification Scale", "Apply to ALL External Factor Plans" } );
            marketPlanParamNames.Add( ModelDb.PlanType.TaskEvent,
                new string[] { "Modification Scale", "Apply to ALL Task Factor Plans" } );
        }

		public override MrktSimDb.ModelDb Db
		{
			set
			{
				base.Db = value;

                UpdateDisplay();

                mrktSimGrid1.Table = paramTable;
				mrktSimGrid1.DescriptionWindow = false;

				// make sure parameters are up to date
				theDb.UpdateParameters();

				createTableStyle();
			}
		}

        public void UpdateDisplay() {
            DataRow[] modparms = theDb.Data.model_parameter.Select();
            mrktSimGrid1.Suspend = true;
            paramTable.Rows.Clear();
            foreach( DataRow modrow in modparms ) {
                DataRow newRow = paramTable.NewRow();
                newRow[ "name" ] = modrow[ "name" ];
                string tblName = (string)modrow[ "table_name" ];
                string colName = (string)modrow[ "col_name" ];
                int colID = (int)modrow[ "id" ];
                newRow[ "table_name" ] = tblName;
                newRow[ "col_name" ] = colName;
                newRow[ "id" ] = colID;
                if( tblName == "market_plan" ) {
                    // convert from the generic parmX format for market plans
                    string planFilter = (string)modrow[ "filter" ];
                    MrktSimDBSchema.market_planRow[] mprow = (MrktSimDBSchema.market_planRow[])theDb.Data.market_plan.Select( planFilter );
                    if( mprow != null && mprow.Length == 1 ) {
                        newRow[ "col_name" ] = LookupParmName( colName, (ModelDb.PlanType)mprow[ 0 ].type );
                    }

                }
                paramTable.Rows.Add( newRow );
            }
            paramTable.AcceptChanges();
            mrktSimGrid1.Suspend = false;
        }

		public override void Flush()
		{
			base.Flush ();
			mrktSimGrid1.Flush();

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
				mrktSimGrid1.Suspend = value;
			}
		}

		public override void Refresh()
		{
			base.Refresh ();
			
			// make sure parameters are up to date
			theDb.UpdateParameters();
            UpdateDisplay();

			mrktSimGrid1.Refresh();
		}

		public override void Reset()
		{
			base.Refresh ();
			
			// make sure parameters are up to date
			theDb.UpdateParameters();

			mrktSimGrid1.Reset();
        }
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ModelParameter()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            paramTable = new DataTable( "model_parameter_display" );
            paramTable.Columns.Add( "name", typeof( string ) );
            paramTable.Columns.Add( "table_name", typeof( string ) );
            paramTable.Columns.Add( "col_name", typeof( string ) );
            paramTable.Columns.Add( "id", typeof( int ) );
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ModelParameter ) );
            this.mrktSimGrid1 = new MarketSimUtilities.MrktSimGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.deleteBut = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mrktSimGrid1
            // 
            this.mrktSimGrid1.DescribeRow = null;
            this.mrktSimGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mrktSimGrid1.EnabledGrid = true;
            this.mrktSimGrid1.Location = new System.Drawing.Point( 0, 25 );
            this.mrktSimGrid1.Name = "mrktSimGrid1";
            this.mrktSimGrid1.RowFilter = null;
            this.mrktSimGrid1.RowID = null;
            this.mrktSimGrid1.RowName = null;
            this.mrktSimGrid1.Size = new System.Drawing.Size( 560, 335 );
            this.mrktSimGrid1.Sort = "";
            this.mrktSimGrid1.TabIndex = 3;
            this.mrktSimGrid1.Table = null;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.deleteBut} );
            this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size( 560, 25 );
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // deleteBut
            // 
            this.deleteBut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deleteBut.Image = ((System.Drawing.Image)(resources.GetObject( "deleteBut.Image" )));
            this.deleteBut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteBut.Name = "deleteBut";
            this.deleteBut.Size = new System.Drawing.Size( 42, 22 );
            this.deleteBut.Text = "Delete";
            // 
            // ModelParameter
            // 
            this.Controls.Add( this.mrktSimGrid1 );
            this.Controls.Add( this.toolStrip1 );
            this.Name = "ModelParameter";
            this.Size = new System.Drawing.Size( 560, 360 );
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

		private void createTableStyle()
		{
			mrktSimGrid1.Clear();
			mrktSimGrid1.AddTextColumn("name");
			mrktSimGrid1.AddTextColumn("table_name", true);
			mrktSimGrid1.AddTextColumn("col_name", true);
            mrktSimGrid1.AllowDelete = false;
			mrktSimGrid1.Reset();
		}

        public static string LookupParmName( string origName, ModelDb.PlanType planType ) {

            string paramName = origName;

            if( origName.StartsWith( "parm" ) ){
                try {
                    int parmIndx = int.Parse( origName.Substring( 4 ) ) - 1;
                    if( marketPlanParamNames.ContainsKey( planType ) ) {
                        string[] paramNames = (string[])marketPlanParamNames[ planType ];
                        paramName = paramNames[ parmIndx ];
                    }
                }
                catch( Exception ){
                }
            }

            return paramName;
        }

        private void delButton_Click( object sender, EventArgs e ) {
            foreach( DataRow drow in mrktSimGrid1.GetSelected() ) {
                int delID = (int)drow[ "id" ];
                // delete the row with the given ID
                string filter = "id = " + delID.ToString();
                DataRow[] paramRow = theDb.Data.model_parameter.Select( filter );
                if( paramRow.Length == 1 ) {
                    ConfirmDialog cdlg = new ConfirmDialog( "Are you sure you want to delete the selected Parameter?", "Confirm Parameter Delete" );
                    cdlg.SetOkCancelButtonStyle();
                    cdlg.SelectWarningIcon();
                    DialogResult resp = cdlg.ShowDialog();
                    if( resp == DialogResult.OK ) {
                        paramRow[ 0 ].Delete();
                        drow.Delete();
                    }
                }
            }
        }

        private void button1_Click( object sender, EventArgs e ) {
            RefreshParameterDisplay();
        }

        private void RefreshParameterDisplay() {
            UpdateDisplay();
        }
	}
}
