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
    public partial class MediaNameMappingForm : Form
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

        public MediaNameMappingForm() {
            InitializeComponent();
        }

        public MediaNameMappingForm( ArrayList nameMappingList, string title )
            : this() {
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
            displayTable.Columns.Add( "OutputCampaignCol", typeof( string ) );
        }

        private void FillDisplayTable() {
            foreach( ProjectSettings.ImportItemInfo info in this.nameList ) {
                DataRow newRow = displayTable.NewRow();
                newRow[ "InputNameCol" ] = info.ImportName;
                newRow[ "SourceCol" ] = info.Source.ToString();
                newRow[ "OutputNameCol" ] = info.MarketSimName;
                newRow[ "OutputCampaignCol" ] = info.MarketSimCampaign;
                displayTable.Rows.Add( newRow );
            }
        }

        private void CopyDisplayTableToNameList() {
            foreach( DataRow tblRow in displayTable.Rows ) {
                string rowName = (string)tblRow[ "InputNameCol" ];
                //string rowSrc = (string)tblRow[ "SourceCol" ];
                string rowMSName = (string)tblRow[ "OutputNameCol" ];
                string rowMSCampaign = (string)tblRow[ "OutputCampaignCol" ];

                foreach( ProjectSettings.ImportItemInfo info in this.nameList ) {
                    if( info.ImportName == rowName ) {
                        info.MarketSimName = rowMSName;
                        info.MarketSimCampaign = rowMSCampaign;
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

        private void setCampaignsButton_Click( object sender, EventArgs e ) {
            AutoCampaignSetForm cSetForm = new AutoCampaignSetForm();

            DialogResult resp = cSetForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            // no params for now

            // The (only so far) rule is that the item name is the first word, and that the campaign comes from the part after ".xls - ".
            // This was the rule needed for the Unilever phase 2 master-brand media data conversion.

            // loop over the display table items and make the indicated changes
            dataGridView1.SuspendLayout();

            foreach( DataRow row in displayTable.Rows ) {

                string inName = (string)row[ "InputNameCol" ];
                string outName = inName;
                if( inName.IndexOf( " " ) != -1 ) {
                    outName = inName.Substring( 0, inName.IndexOf( " " ) );
                }

                string endDelim = ".xls - ";
                string outCamp = inName;
                if( inName.IndexOf( endDelim ) != -1 ) {
                    outCamp = inName.Substring( inName.IndexOf( endDelim ) + endDelim.Length );
                }
                row[ "OutputNameCol" ] = outName;
                row[ "OutputCampaignCol" ] = outCamp;
            }
            dataGridView1.ResumeLayout();
        }
    }
}