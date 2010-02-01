using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataImporter.Dialogs
{
    public partial class DescriptionForm : Form
    {
        public string Description {
            get {
                return this.descriptionTextBox.Text;
            }
        }

        public string ProjectName {
            get {
                return this.nameTextBox.Text.Trim() ;
            }
        }

        public DescriptionForm( string name, string description ) {
            InitializeComponent();

            this.nameTextBox.Text = name;
            this.descriptionTextBox.Text = description;
        }
    }
}