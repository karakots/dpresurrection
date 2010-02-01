using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NitroReader.Dialogs
{
    public partial class Saved2 : Form
    {
        public Saved2( string file ) {
            InitializeComponent();

            this.label1.Text = "NITRO Reader settings saved for " + file;
        }

        private void label1_Click( object sender, EventArgs e ) {

        }
    }
}