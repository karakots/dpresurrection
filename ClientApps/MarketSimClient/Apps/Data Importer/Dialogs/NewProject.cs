using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DataImporter.ImportSettings;

namespace DataImporter.Dialogs
{
    public partial class NewProject : Form
    {
        private ProjectSettings projectSettings;

        public ProjectSettings ProjectSettings {
            get { return projectSettings; }
        }

        public NewProject() {
            InitializeComponent();
            projectSettings = null;
        }

        private void importButtonClick( object sender, EventArgs e ) {
            ArrayList fileNames = new ArrayList();

            if( this.nameTextBox.Text.Trim() == "" ) {
                MessageBox.Show( "You must provide a name for the project.", "Name Needed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                this.DialogResult = DialogResult.None;
                return;
            }

            if( this.inputDirTextBox.Text.Trim() == "" ) {
                MessageBox.Show( "You must specify the folder where input data will be found.", "Input Folder Needed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                this.DialogResult = DialogResult.None;
                return;
            }

            if( Directory.Exists( this.inputDirTextBox.Text.Trim() ) == false ){
                MessageBox.Show( "Error: The specified input folder does not exist.", "Input Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error );
                this.DialogResult = DialogResult.None;
                return;
            }

            string testProj = this.inputDirTextBox.Text.Trim() + "\\" + this.nameTextBox.Text.Trim() + DataImporter.OutputFileExt;
            if( File.Exists( testProj ) == true ) {
                string msg = String.Format( "Error: The import project file exists already:\r\n\r\n{0}\r\n\r\nChoose a different name for the project.", testProj );
                MessageBox.Show( msg, "Project Exists Already", MessageBoxButtons.OK, MessageBoxIcon.Error );
                this.DialogResult = DialogResult.None;
                return;
            }

            if( this.outputDirTextBox.Text.Trim() == "" ) {
                MessageBox.Show( "You must specify a folder for the output files (MarketSim-importable market plan files).", "Output Folder Needed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                this.DialogResult = DialogResult.None;
                return;
            }

            if( Directory.Exists( this.outputDirTextBox.Text.Trim() ) == false ) {
                string msg = String.Format( "The specified output folder does not exist:\r\n\r\n{0}\r\n\r\nDo you want to create it now?", this.outputDirTextBox.Text.Trim() );
                DialogResult resp = MessageBox.Show( msg, "Output Folder Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Error );
                if( resp == DialogResult.No ) {
                    this.DialogResult = DialogResult.None;
                    return;
                }
                // the answer was yes - create the dir
                Directory.CreateDirectory( this.outputDirTextBox.Text.Trim() );
            }

            this.DialogResult = DialogResult.OK;
            projectSettings = new ProjectSettings( this.nameTextBox.Text.Trim(), this.normalizePriceCheckBox.Checked, 
                inputDirTextBox.Text, outputDirTextBox.Text );

            projectSettings.SetEdited();
        }

        private void browseInButton_Click( object sender, EventArgs e ) {
            SetFromFolderBrowser( inputDirTextBox, "Choose the folder containing all input data for this project" );

            if( outputDirTextBox.Text == "" ) {
                outputDirTextBox.Text = inputDirTextBox.Text + "\\MarketSim Data";
                outputDirTextBox.SelectAll();
            }
        }

        private void browseOutButton_Click( object sender, EventArgs e ) {
            SetFromFolderBrowser( outputDirTextBox, "Choose the folder where the importable MarketSim will be saved" );
        }

        private void SetFromFolderBrowser( TextBox textBox, string pickerCaption ) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = pickerCaption;
            DialogResult resp = fbd.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            textBox.Text = fbd.SelectedPath;
            textBox.SelectAll();      // show the end of the string, not the beginning (if it is too long)
        }
    }
}