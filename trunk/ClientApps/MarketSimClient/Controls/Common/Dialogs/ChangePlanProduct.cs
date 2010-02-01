using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;
using Utilities;

namespace Common.Dialogs
{
    public partial class ChangePlanProduct : Form
    {
        private string helpTag = "ChangePlanProduct";
        private ArrayList currentPlans;
        private ModelDb theDb;

        /// <summary>
        /// Returns the ID of the selected product in the dialog.
        /// </summary>
        public int ProductID {
            get {
                if( brandComboBox.SelectedItem == null )
                    return Database.AllID;

                MrktSimDBSchema.productRow prodRow = (MrktSimDBSchema.productRow)
                    ((DataRowView)brandComboBox.SelectedItem).Row;

                return prodRow.product_id;
            }
        }


        /// <summary>
        /// Configures the display for the given plan type
        /// </summary>
        public void SetPlanType( ModelDb.PlanType planType ) {
            if( planType == ModelDb.PlanType.MarketPlan ) {
                int yofst = -5;
                this.label5.Location = new Point( this.label5.Location.X, this.label5.Location.Y + yofst );
                this.brandComboBox.Location = new Point( this.brandComboBox.Location.X, this.brandComboBox.Location.Y + yofst );
                this.topLevelInfoLabel.Visible = true;
            }
            else {
                this.topLevelInfoLabel.Visible = false;
            }
        }
        
        /// <summary>
        /// Creates a new dialog for making a modified copy of a market plan.
        /// </summary>
        public ChangePlanProduct( ModelDb db, ArrayList plansOrComponents ) {
            InitializeComponent();
            this.currentPlans = plansOrComponents;
            this.theDb = db;

            // setup product picker
            productView.Table = theDb.Data.product;
            brandComboBox.DisplayMember = "product_name";
            brandComboBox.ValueMember = "product_id";
            brandComboBox.DataSource = productView;

            if( currentPlans.Count > 0 ) {
                MrktSimDBSchema.market_planRow prow = (MrktSimDBSchema.market_planRow)currentPlans[ 0 ];
                brandComboBox.SelectedValue = prow.product_id;
            }
        }

        private void helpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, this.helpTag );
        }

        private void okButton_Click( object sender, EventArgs e ) {
            this.Cursor = Cursors.WaitCursor;

            foreach( object currentPlanItem in currentPlans ) {

                MrktSimDBSchema.market_planRow currentPlan = null;
                if( currentPlanItem is int ) {
                    currentPlan = theDb.Data.market_plan.FindByid( (int)currentPlanItem );
                }
                else {
                    currentPlan = (MrktSimDBSchema.market_planRow)currentPlanItem;
                }

                currentPlan.product_id = this.ProductID;

                theDb.UpdateMarketPlan( currentPlan, 0 );         // this will create a ccpy of the plan, all components, and all component data
            }
            this.Cursor = Cursors.Default;
        }
    }
}