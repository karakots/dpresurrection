using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NitroReader.Dialogs
{
    public partial class ConfirmForm : Form
    {
        public ConfirmForm( string message, string title) {
            InitializeComponent();

            this.infoLabel.Text = message;
            this.Text = title;
        }

        public Color TextColor {
            set {
                this.infoLabel.ForeColor = value;
            }
        }

        public void HideCancel() {
            this.cancelButton.Visible = false;
            this.okButton.Location = new Point( this.okButton.Location.X + 40, this.okButton.Location.Y );
        }
    }
}