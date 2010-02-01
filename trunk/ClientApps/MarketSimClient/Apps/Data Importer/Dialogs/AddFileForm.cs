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
    public partial class AddFileForm : Form
    {
        private ProjectSettings.ProjectSection projectSection;

        public ProjectSettings.ProjectSection ProjectSection {
            get { return projectSection; }
        }

        public AddFileForm( string folderPathRelToRoot ) {
            InitializeComponent();
            projectSection = null;
            this.nameLabel.Text = folderPathRelToRoot;
        }

        public string SpecifiedBrand {
            get {
                if( brandFileSpecRadioButton.Checked ) {
                    return brandTextBox.Text;
                }
                else {
                    return null;
                }
            }
        }

        public string SpecifiedChannel {
            get {
                if( chanFileSpecRadioButton.Checked ) {
                    return chanTextBox.Text;
                }
                else {
                    return null;
                }
            }
        }

        public ProjectSettings.InfoSource BrandInfoSource {
            get {
                if( brandFileNameRadioButton.Checked == true ) {
                    return ProjectSettings.InfoSource.FileName;
                }
                else if( brandFileDataRadioButton.Checked == true ) {
                    return ProjectSettings.InfoSource.FileContents;
                }
                else if( brandFileSpecRadioButton.Checked == true ) {
                    return ProjectSettings.InfoSource.UserSpecified;
                }
                else if( brandFileWsRadioButton.Checked == true ) {
                    return ProjectSettings.InfoSource.WorksheetName;
                }
                else {
                    // should never get here
                    return ProjectSettings.InfoSource.Unknown;
                }
            }
        }

        public ProjectSettings.InfoSource ChannelInfoSource {
            get {
                if( chanFileNameRadioButton.Checked == true ) {
                    return ProjectSettings.InfoSource.FileName;
                }
                else if( chanFileDataRadioButton.Checked == true ) {
                    return ProjectSettings.InfoSource.FileContents;
                }
                else if( chanFileSpecRadioButton.Checked == true ) {
                    return ProjectSettings.InfoSource.UserSpecified;
                }
                else if( chanFileWsRdioButton.Checked == true ) {
                    return ProjectSettings.InfoSource.WorksheetName;
                }
                else {
                    // should never get here
                    return ProjectSettings.InfoSource.Unknown;
                }
            }
        }

        private void importButtonClick( object sender, EventArgs e ) {
            projectSection = new ProjectSettings.ProjectSection( this.nameLabel.Text, false, this.BrandInfoSource, this.ChannelInfoSource );
            projectSection.SpecificBrand = this.SpecifiedBrand;
            projectSection.SpecificChannel = this.SpecifiedChannel;
        }

        private void chanFileSpecRadioButton_CheckedChanged( object sender, EventArgs e ) {
            chanTextBox.Enabled = chanFileSpecRadioButton.Checked;
        }
    }
}