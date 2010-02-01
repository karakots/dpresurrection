using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataImporter.Dialogs
{
    public partial class OutputFolderEditForm : Form
    {
        public string OutputFolder {
            get { return outputDirTextBox.Text.Trim(); }
        }

        public OutputFolderEditForm( string curOutputFolder ) {
            InitializeComponent();

            outputDirTextBox.Text = curOutputFolder;
        }
    }
}