using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestScenarioManager
{
    public partial class NewNameForm : Form
    {
        public string ItemName {
            get {
                return this.nameTextBox.Text.Trim();
            }
        }
        public string ItemDescription {
            get {
                return this.descriptionTextBox.Text.Trim();
            }
        }

        public NewNameForm( string title, string name, string desc ) {
            InitializeComponent();

            this.typeLabel.Text = title;
            this.nameTextBox.Text = name;
            this.descriptionTextBox.Text = desc;
        }
    }
}