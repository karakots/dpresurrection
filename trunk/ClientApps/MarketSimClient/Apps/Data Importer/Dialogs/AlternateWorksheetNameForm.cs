using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DataImporter;
using DataImporter.ImportSettings;

namespace DataImporter.Dialogs
{
    public partial class AlternateWorksheetNameForm : Form
    {
        public string SelectedWorksheetName {
            get {
                if( namesComboBox.SelectedIndex < 1 ) {
                    return null;
                }
                else {
                    return (string)namesComboBox.SelectedItem;
                }
            }
        }

        public AlternateWorksheetNameForm() {
            InitializeComponent();
        }

        public AlternateWorksheetNameForm( WorksheetSettings worksheetSettings, string[] worksheetNamesInFile ) : this() {
            this.fileNameLabel.Text = worksheetSettings.InputFile.Substring( worksheetSettings.InputFile.LastIndexOf( "\\" ) + 1 );
            this.worksheetNameLabel.Text = worksheetSettings.SheetName;

            this.namesComboBox.Items.Add( "<select one>" );
            Array.Sort( worksheetNamesInFile );
            for( int i = 0; i < worksheetNamesInFile.Length; i++ ) {
                this.namesComboBox.Items.Add( worksheetNamesInFile[ i ] );
            }
            this.namesComboBox.SelectedIndex = 0;
        }
    }
}