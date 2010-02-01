using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataImporter.Dialogs
{
    public partial class StartImportForm : Form
    {
        public StartImportForm() {
            InitializeComponent();
        }

        public DateTime StartDate {
            get {
                return dateTimePicker1.Value;
            }
        }

        public DateTime EndDate {
            get {
                return dateTimePicker2.Value;
            }
        }

        public double Tolerance {
            get {
                return double.Parse( tolTextBox.Text );
            }
        }
         
        public double Scaling {
            get {
                return double.Parse( scaleNumTextBox.Text ) / double.Parse( scaleDenTextBox.Text );
            }
        }

        public string[] Channels {
            get {
                string[] chs = new string[ channelsCheckedListBox.CheckedItems.Count ];
                for( int i = 0; i < channelsCheckedListBox.CheckedItems.Count; i++ ) {
                    chs[ i ] = (string)channelsCheckedListBox.CheckedItems[ i ];
                }
                return chs;
            }
        }

        public string[] DataTypes {
            get {
                string[] types = new string[ typesCheckedListBox.CheckedItems.Count ];
                for( int i = 0; i < typesCheckedListBox.CheckedItems.Count; i++ ) {
                    types[ i ] = (string)typesCheckedListBox.CheckedItems[ i ];
                }
                return types;
            }
        }

        public StartImportForm( DateTime start, DateTime end, double tol, double overallScaling, ArrayList channels, bool validateOnly ) {
            InitializeComponent();

            dateTimePicker1.Value = start;
            dateTimePicker2.Value = end;
            tolTextBox.Text = tol.ToString();

            if( overallScaling > 1 || overallScaling == 0 ) {
                scaleNumTextBox.Text = overallScaling.ToString();
            }
            else if( overallScaling < 1 ) {
                double denVal = 1 / overallScaling;
                scaleDenTextBox.Text = denVal.ToString();
            }

            if( validateOnly ) {
                this.Text = "Validate Files for Import";
                label1.Text = "Ready to Validate Data";
            }

            for( int i = 0; i < typesCheckedListBox.Items.Count; i++ ){
                typesCheckedListBox.SetItemChecked( i, true );
            }

            for( int i = 0; i < channels.Count; i++ ){
                channelsCheckedListBox.Items.Add( (string)channels[ i ] );
            }
            for( int i = 0; i < channelsCheckedListBox.Items.Count; i++ ) {
                channelsCheckedListBox.SetItemChecked( i, true );
            }
        }

        private void tolTextBox_TextChanged( object sender, EventArgs e ) {
            try {
                double dum = double.Parse( tolTextBox.Text );
                okButton.Enabled = true;
            }
            catch( Exception ) {
                okButton.Enabled = false;
            }
        }
    }
}