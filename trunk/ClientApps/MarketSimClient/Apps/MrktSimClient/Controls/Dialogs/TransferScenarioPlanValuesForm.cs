using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;

namespace MrktSimClient.Controls.Dialogs
{
    public partial class TransferScenarioPlanValuesForm : Form
    {
        private MrktSimDBSchema.scenarioRow[] scenarios;
        private MrktSimDBSchema.product_typeRow[] productTypes;
        private ModelDb theDb;

        public TransferScenarioPlanValuesForm( ModelDb db ) {
            InitializeComponent();
            theDb = db;

            typeComboBox.SelectedItem = "Distribution";

            scenarios = (MrktSimDBSchema.scenarioRow[])theDb.Data.scenario.Select();
            string[] scenarioName = new string[ scenarios.Length ];
            for( int i = 0; i < scenarios.Length; i++ ) {
                scenarioName[ i ] = scenarios[ i ].name;
            }
            Array.Sort( scenarioName, scenarios );
            for( int i = 0; i < scenarios.Length; i++ ) {
                scenarioComboBox.Items.Add( scenarioName[ i ] );
            }
            scenarioComboBox.SelectedIndex = 0;

            startDateTimePicker.Value = new DateTime( 2000, 1, 1 );
            endDateTimePicker.Value = new DateTime( 2030, 1, 1 );

            columnsCheckedListBox.Items.Add( "Awareness" );
            columnsCheckedListBox.Items.Add( "Persuasion" );
            columnsCheckedListBox.SetItemChecked( 0, true );
            columnsCheckedListBox.SetItemChecked( 1, true );

            productTypes = (MrktSimDBSchema.product_typeRow[])theDb.Data.product_type.Select();
            for( int i = 0; i < productTypes.Length; i++ ) {
                prodLevelComboBox.Items.Add( productTypes[ i ].type_name );
            }
            prodLevelComboBox.SelectedIndex = productTypes.Length - 1;
        }

        public ModelDb.PlanType TransferPlanType {
            get {
                switch( (string)typeComboBox.SelectedItem ) {
                    case "Display":
                        return Database.PlanType.Display;
                    case "Distribution":
                        return Database.PlanType.Distribution;
                    case "Media":
                        return Database.PlanType.Media;
                    default:
                        return Database.PlanType.MarketPlan;
                }
            }
        }

        public int TransferProductType {
            get {
                MrktSimDBSchema.product_typeRow row = (MrktSimDBSchema.product_typeRow)productTypes[ prodLevelComboBox.SelectedIndex ];
                return row.id;
            }
        }

        public int SourceScenarioID {
            get {
                return scenarios[ scenarioComboBox.SelectedIndex ].scenario_id;
            }
        }

        public DateTime StartDate {
            get {
                return startDateTimePicker.Value;
            }
        }

        public DateTime EndDate {
            get {
                return endDateTimePicker.Value;
            }
        }

        public DataColumn[] TransferColumns {
            get {
                DataColumn[] cols = new DataColumn[ columnsCheckedListBox.CheckedItems.Count + 2 ];
                ArrayList colList = new ArrayList();
                if( columnsCheckedListBox.GetItemChecked( 0 ) ) {
                    DataColumn col = new DataColumn( "message_awareness_probability", typeof( double ) );
                    colList.Add( col );
                }
                if( columnsCheckedListBox.GetItemChecked( 1 ) ) {
                    DataColumn col = new DataColumn( "message_persuation_probability", typeof( double ) );
                    colList.Add( col );
                }

                // we always need start and end date
                DataColumn scol = new DataColumn( "start_date", typeof( DateTime ) );
                colList.Add( scol );
                DataColumn ecol = new DataColumn( "end_date", typeof( DateTime ) );
                colList.Add( ecol );

                colList.CopyTo( cols );
                return cols;
            }
        }

        //private void typeComboBox_SelectedIndexChanged( object sender, EventArgs e ) {
        //    string compType = (string)typeComboBox.SelectedItem;
        //    DataTable tbl = null;
        //    if( compType == "Distribution" ) {
        //        tbl = theDb.Data.distribution;
        //    }
        //    else if( compType == "Display" ) {
        //        tbl = theDb.Data.display;
        //    }
        //    else if( compType == "Media" ) {
        //        tbl = theDb.Data.mass_media;
        //    }

        //    columnsCheckedListBox.BeginUpdate();
        //    columnsCheckedListBox.Items.Clear();
        //    columnsCheckedListBox.Items.Add( "Awareness" );
        //    columnsCheckedListBox.Items.Add( "Persuasion" );
        //    columnsCheckedListBox.SetItemChecked( 0, true );
        //    columnsCheckedListBox.SetItemChecked( 1, true );
        //    ////ArrayList colNames = new ArrayList();
        //    ////for( int i = 0; i < tbl.Columns.Count; i++ ) {
        //    ////    DataColumn col = tbl.Columns[ i ];
        //    ////    string colName = col.ColumnName;
        //    ////    if( colName == "start_date" || colName == "end_date" || colName == "product_id" || colName == "channel_id" || colName == "model_id" ||
        //    ////         colName == "product_name" || colName == "channel_name" || colName == "record_id" || colName == "market_plan_id" ) {
        //    ////        continue;
        //    ////    }
        //    ////    columnsCheckedListBox.Items.Add( colName );
        //    ////}
        //    columnsCheckedListBox.EndUpdate();
        //}
    }
}