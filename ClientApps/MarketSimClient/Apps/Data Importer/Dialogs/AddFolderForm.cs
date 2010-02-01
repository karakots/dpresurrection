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
    public partial class AddFolderForm : Form
    {
        private ProjectSettings.ProjectSection projectSection;
        private string inRootStr = "<Project Input Folder>";

        public ProjectSettings.ProjectSection ProjectSection {
            get { return projectSection; }
        }

        public AddFolderForm( string folderPathRelToRoot ) {
            InitializeComponent();
            projectSection = null;
            this.nameLabel.Text = folderPathRelToRoot;
            if( folderPathRelToRoot == "" ) {
                this.nameLabel.Text = inRootStr;
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
                else if( brandWsRdioButton.Checked == true ) {
                    return ProjectSettings.InfoSource.WorksheetName;
                }
                else if( brandFolderRadioButton.Checked == true ) {
                    return ProjectSettings.InfoSource.DirectoryName;
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
                else if( chanFoldeeRadioButton.Checked == true ) {
                    return ProjectSettings.InfoSource.DirectoryName;
                }
                else if( chanWsRadioButton.Checked == true ) {
                    return ProjectSettings.InfoSource.WorksheetName;
                }
                else {
                    // should never get here
                    return ProjectSettings.InfoSource.Unknown;
                }
            }
        }

        private void importButtonClick( object sender, EventArgs e ) {
            string name = this.nameLabel.Text;
            if( name == inRootStr ) {
                name = "";
            }
            projectSection = new ProjectSettings.ProjectSection( name, true, this.BrandInfoSource, this.ChannelInfoSource );
            projectSection.SpecificBrand = brandTextBox.Text.Trim();
            projectSection.SpecificChannel = chanTextBox.Text.Trim();
        }

        private void chanFileSpecRadioButton_CheckedChanged( object sender, EventArgs e ) {
            chanTextBox.Enabled = chanFileSpecRadioButton.Checked;
        }
    }
}