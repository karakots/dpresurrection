using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NitroReader.Dialogs
{
    /// <summary>
    /// InfoForm allows the display of general inforrmational text.
    /// </summary>
    public partial class InfoForm : Form
    {
        public string Info {
            set { this.infoTextBox.Text = value; }
        }

        public InfoForm( string name, string title, string info) {
            InitializeComponent();

            this.Text = name;
            this.titleLabel.Text = title;
            this.infoTextBox.Text = info;
        }

        private void InfoForm_Load( object sender, EventArgs e ) {
            this.infoTextBox.Select( 0, 0 );
        }
    }
}