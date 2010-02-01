using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;

namespace MrktSimClient.Controls.Dialogs
{
    public partial class TransferPlanValuesForm : Form
    {
        private MrktSimDBSchema.product_typeRow[] productTypes;
        private ModelDb theDb;

        public TransferPlanValuesForm( ModelDb db ) {
            InitializeComponent();
            theDb = db;

            typeComboBox.SelectedItem = "Distribution";
            endDateTimePicker.Value = DateTime.Now;

            productTypes = (MrktSimDBSchema.product_typeRow[])theDb.Data.product_type.Select();
            for( int i = 0; i < productTypes.Length; i++ ) {
                prodLevelComboBox.Items.Add( productTypes[ i ].type_name );
            }
            prodLevelComboBox.SelectedIndex = productTypes.Length - 1;
        }

        public ModelDb.PlanType PlanType {
            get {
                switch( (string)typeComboBox.SelectedItem ) {
                    case "Display":
                        return Database.PlanType.Display;
                    case "Distribution":
                        return Database.PlanType.Distribution;
                    case "Price":
                        return Database.PlanType.Price;
                    case "Real Sales":
                        return Database.PlanType.ProdEvent;
                }
                return Database.PlanType.MarketPlan;
            }
        }

        public int TransferProductType {
            get {
                MrktSimDBSchema.product_typeRow row = (MrktSimDBSchema.product_typeRow)productTypes[ prodLevelComboBox.SelectedIndex ];
                return row.id;
            }
        }

        public DateTime TransferStartDate {
            get {
                return startDateTimePicker.Value;
            }
        }

        public DateTime TransferEndDate {
            get {
                return endDateTimePicker.Value;
            }
        }

        public int TransferPatternMonths {
            get {
                if( finalValuesRadioButton.Checked == true ) {
                    return -1;
                }
                else {
                    return (int)patternLengthNumericUpDown.Value;
                }
            }
        }
    }
}