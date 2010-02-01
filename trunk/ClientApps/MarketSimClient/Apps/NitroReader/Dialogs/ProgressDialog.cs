using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NitroReader.Dialogs
{
    public partial class ProgressDialog : Form
    {
        public string Info0 {
            set {
                this.label1.Text = value;
            }
        }

        public string Info {
            set {
                this.label2.Text = value;
            }
        }

        public int ProgressPercent {
            set {
                this.progressBar1.Value = value;
            }
        }

        public ProgressDialog() {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e ) {
            this.Visible = false;
        }
    }
}