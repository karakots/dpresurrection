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
    public partial class AutoNameSetForm : Form
    {
        public AutoNameSetForm() {
            InitializeComponent();

            sourceComboBox.SelectedIndex = 0;
        }

        public string MatchSubstring {
            get { return matchTextBox.Text; }
        }

        public string MarketSimName {
            get { return outputTextBox.Text.Trim(); }
        }

        public ProjectSettings.InfoSource InputSource {
            get {
                switch( (string)sourceComboBox.SelectedItem ) {
                    case "Directory Name":
                        return ProjectSettings.InfoSource.DirectoryName;
                    case "File Name":
                        return ProjectSettings.InfoSource.FileName;
                    case "Worksheet Name":
                        return ProjectSettings.InfoSource.WorksheetName;
                    case "File Contents":
                        return ProjectSettings.InfoSource.FileContents;
                    case "User Specified":
                        return ProjectSettings.InfoSource.UserSpecified;
                }
                return ProjectSettings.InfoSource.Unknown;
            }
        }
    }
}