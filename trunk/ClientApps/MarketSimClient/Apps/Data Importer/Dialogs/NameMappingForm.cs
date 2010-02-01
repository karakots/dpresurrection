using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DataImporter;
using DataImporter.ImportSettings;

namespace DataImporter.Dialogs
{
    public partial class NameMappingForm : Form
    {
        private ArrayList nameList;
        private bool validateEntriesAsDouble = false;

        private DataTable displayTable;

        public ArrayList NameMappingList {
            get {
                return this.nameList;
            }
        }

        public bool ValidateEntriesAsDouble {
            set {
                validateEntriesAsDouble = value;
            }
        }

        public NameMappingForm() {
            InitializeComponent();
        }

        public NameMappingForm( ArrayList nameMappingList, string title ) : this() {
            this.nameList = nameMappingList;
            this.titleLabel.Text = title;

            dataGridView1.SuspendLayout(); 

            CreateDisplayTable();
            FillDisplayTable();

            this.dataGridView1.DataSource = displayTable;

            dataGridView1.ResumeLayout();
        }

        public void HideSourceCol() {
            dataGridView1.Columns[ 1 ].Visible = false;
        }

        private void okButton_Click( object sender, EventArgs e ) {
            CopyDisplayTableToNameList();
        }

        private void CreateDisplayTable() {
            displayTable = new DataTable( "namesDisplayTable" );
            displayTable.Columns.Add( "InputNameCol", typeof( string ) );
            displayTable.Columns.Add( "SourceCol", typeof( string ) );
            displayTable.Columns.Add( "OutputNameCol", typeof( string ) );
        }

        private void FillDisplayTable() {
            foreach( ProjectSettings.ImportItemInfo info in this.nameList ) {
                DataRow newRow = displayTable.NewRow();
                newRow[ "InputNameCol" ] = info.ImportName;
                newRow[ "SourceCol" ] = info.Source.ToString();
                newRow[ "OutputNameCol" ] = info.MarketSimName;
                displayTable.Rows.Add( newRow );
            }
        }

        private void CopyDisplayTableToNameList() {
            foreach( DataRow tblRow in displayTable.Rows ) {
                string rowName = (string)tblRow[ "InputNameCol" ];
                string rowSrc = (string)tblRow[ "SourceCol" ];
                string rowMSName = (string)tblRow[ "OutputNameCol" ];

                foreach( ProjectSettings.ImportItemInfo info in this.nameList ) {
                    if( info.ImportName == rowName && info.Source.ToString() == rowSrc ) {
                        info.MarketSimName = rowMSName;
                    }
                }
            }
       }

        private void autoSetButton_Click( object sender, EventArgs e ) {
            AutoNameSetForm nameSetForm = new AutoNameSetForm();

            DialogResult resp = nameSetForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            string pattern = nameSetForm.MatchSubstring;
            ProjectSettings.InfoSource source = nameSetForm.InputSource;
            string outName = nameSetForm.MarketSimName;

            // loop over the display table items and make the indicated changes
            dataGridView1.SuspendLayout();
            foreach( DataRow row in displayTable.Rows ) {
                string inName = (string)row[ "InputNameCol" ];
                string rowSrc = (string)row[ "SourceCol" ];

                if( source == ProjectSettings.InfoSource.Unknown || rowSrc == source.ToString() ) {
                    if( inName.IndexOf( pattern ) != -1 ) {
                        // found a matching item
                        row[ "OutputNameCol" ] = outName;
                    }
                }
            }
            dataGridView1.ResumeLayout();
        }
    }
}