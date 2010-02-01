using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NitroReader.Dialogs
{
    public partial class GeneralSettings : Form
    {
        private GeneralSettingsValues generalSettings = null;

        public GeneralSettings( GeneralSettingsValues generalSettings ) {
            InitializeComponent();
            this.generalSettings = generalSettings;
            this.displayAwarenessTextBox.Text = String.Format( "{0:f2}", generalSettings.DisplayAwareness );
            this.displayPersuasionTextBox.Text = String.Format( "{0:f2}", generalSettings.DisplayPersuasion );
            this.distributionAwarenessTextBox.Text = String.Format( "{0:f2}", generalSettings.DistributionAwareness );
            this.distributionPersuasionTextBox.Text = String.Format( "{0:f2}", generalSettings.DistributionPersuasion );
            this.iconFileTextBox.Text = generalSettings.CustomerIconFile;
            this.versionLabel.Text = Application.ProductVersion.ToString();
        }

        private void selectButton_Click( object sender, EventArgs e ) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Customer Icon";
            ofd.Filter = "Icon files (*.ico)|*.ico|GIF images (*.gif)|*.gif|JPEG Images (*.jpg)|*.jpg";
            ofd.RestoreDirectory = true;
            DialogResult resp = ofd.ShowDialog( this );
            if( resp == DialogResult.OK ) {
                iconFileTextBox.Text = ofd.FileName;
            }
        }

        private void okButton_Click( object sender, EventArgs e ) {
            generalSettings.CustomerIconFile = this.iconFileTextBox.Text.Trim();
            try {
                generalSettings.DisplayAwareness = double.Parse( this.displayAwarenessTextBox.Text.Trim() );
            }
            catch( Exception ) {
            }
            try {
                generalSettings.DisplayPersuasion = double.Parse( this.displayPersuasionTextBox.Text.Trim() );
            }
            catch( Exception ) {
            }
            try {
                generalSettings.DistributionAwareness = double.Parse( this.distributionAwarenessTextBox.Text.Trim() );
            }
            catch( Exception ) {
            }
            try {
                generalSettings.DistributionPersuasion = double.Parse( this.distributionPersuasionTextBox.Text.Trim() );
            }
            catch( Exception ) {
            }
        }

        private void clearButton_Click( object sender, EventArgs e ) {
            iconFileTextBox.Text = "";
        }
    }
}