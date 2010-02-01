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
    public partial class ExtendScenarioForm : Form
    {
        private MrktSimDBSchema.product_typeRow[] productTypes;
        private ModelDb theDb;

        public ExtendScenarioForm( ModelDb db ) {
            InitializeComponent();
            theDb = db;

            typeComboBox.SelectedItem = "Price";
            endDateTimePicker.Value = DateTime.Now;

            productTypes = (MrktSimDBSchema.product_typeRow[])theDb.Data.product_type.Select();
            for( int i = 0; i < productTypes.Length; i++ ) {
                prodLevelComboBox.Items.Add( productTypes[ i ].type_name );
            }
            prodLevelComboBox.SelectedIndex = productTypes.Length - 1;
        }

        public ModelDb.PlanType PlanTypeToExtend {
            get {
                switch( (string)typeComboBox.SelectedItem ) {
                    case "Display":
                        return Database.PlanType.Display;
                    case "Distribution":
                        return Database.PlanType.Distribution;
                    case "Price":
                        return Database.PlanType.Price;
                    case "Media":
                        return Database.PlanType.Media;
                    case "Real Sales":
                        return Database.PlanType.ProdEvent;
                }
                return Database.PlanType.MarketPlan;
            }
        }

        public int ExtensionProductType {
            get {
                MrktSimDBSchema.product_typeRow row = (MrktSimDBSchema.product_typeRow)productTypes[ prodLevelComboBox.SelectedIndex ];
                return row.id;
            }
        }

        public DateTime ExtensionStartDate {
            get {
                return startDateTimePicker.Value;
            }
        }

        public DateTime ExtensionEndDate {
            get {
                return endDateTimePicker.Value;
            }
        }

        public int ExtensionPatternMonths {
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