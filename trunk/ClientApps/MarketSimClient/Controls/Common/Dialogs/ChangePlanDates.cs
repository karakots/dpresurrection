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
    public partial class ChangePlanDates : Form
    {
        private string helpTag = "ChangePlanDates";
        private ArrayList currentPlans;
        private ModelDb theDb;

        /// <summary>
        /// Returns the shift amount set in the dialog.
        /// </summary>
        public int ShiftAmountDays {
            get {
                return (int)this.shiftDays.Value;
            }
        }

        /// <summary>
        /// Configures the display for the given plan type
        /// </summary>
        public void SetPlanType( ModelDb.PlanType planType ) {
            if( planType == ModelDb.PlanType.MarketPlan ) {
                int yofst = -12;
                this.label1.Location = new Point( this.label1.Location.X, this.label1.Location.Y + yofst );
                this.label2.Location = new Point( this.label2.Location.X, this.label2.Location.Y + yofst );
                this.shiftDays.Location = new Point( this.shiftDays.Location.X, this.shiftDays.Location.Y + yofst );
                this.topLevelInfoLabel.Visible = true;
            }
            else {
                this.topLevelInfoLabel.Visible = false;
            }
        }

        /// <summary>
        /// Creates a new dialog for making a modified copy of a market plan.
        /// </summary>
        public ChangePlanDates( ModelDb db, ArrayList plansOrComponents ) {
            InitializeComponent();
            this.currentPlans = plansOrComponents;
            this.theDb = db;
        }

        private void helpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, this.helpTag );
        }

        private void okButton_Click( object sender, EventArgs e ) {
            this.UseWaitCursor = true;
            int newPlanShiftDays = this.ShiftAmountDays;

            foreach( object currentPlanItem in currentPlans ) {

                MrktSimDBSchema.market_planRow currentPlan = null;
                if( currentPlanItem is int ) {
                    currentPlan = theDb.Data.market_plan.FindByid( (int)currentPlanItem );
                }
                else {
                    currentPlan = (MrktSimDBSchema.market_planRow)currentPlanItem;
                }

                currentPlan.start_date = currentPlan.start_date.AddDays( newPlanShiftDays );
                currentPlan.end_date = currentPlan.end_date.AddDays( newPlanShiftDays );

                theDb.UpdateMarketPlan( currentPlan, newPlanShiftDays );         // this will create a ccpy of the plan, all components, and all component data
            }
            this.UseWaitCursor = false;
        }
    }
}