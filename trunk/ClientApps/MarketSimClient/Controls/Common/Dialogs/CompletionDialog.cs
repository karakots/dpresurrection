using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Common.Dialogs
{
    /// <summary>
    /// MessageDialog is a simple 1-message dialog.  Help is not available.
    /// </summary>
    public partial class CompletionDialog : Form
    {
         public string Message {
            set {
                this.messageLabel.Text = value;
            }
        }

        public CompletionDialog( string message ) : this( message, true ) {
        }

        public CompletionDialog( string message, bool showCheckIcon ) {
            InitializeComponent();

            this.messageLabel.Text = message;
            this.checkMarkPictureBox.Visible = showCheckIcon;
        }
    }
}