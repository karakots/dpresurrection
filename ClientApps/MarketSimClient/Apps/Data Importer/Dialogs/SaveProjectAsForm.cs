using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataImporter.Dialogs
{
    public partial class SaveProjectAsForm : Form
    {
        public string ProjectName {
            get { return this.nameTextBox.Text.Trim(); }
        }

        public string ProjectFile {
            get { return this.fileTextBox.Text.Trim(); }
        }

        public bool ProjectPricesNormalized {
            get { return this.normalizePriceCheckBox.Checked; }
        }

        public SaveProjectAsForm( string curProjectName, string curProjectFile, bool curNormalizePrices ) {
            InitializeComponent();

            this.nameTextBox.Text = curProjectName;
            this.fileTextBox.Text = curProjectFile;
            this.normalizePriceCheckBox.Checked = curNormalizePrices;
        }
    }
}