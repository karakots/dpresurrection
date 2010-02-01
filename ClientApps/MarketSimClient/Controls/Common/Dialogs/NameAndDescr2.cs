using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Utilities;

namespace Common.Dialogs
{
    public partial class NameAndDescr2 : Form
    {
        private string specificItemName = null;
        private string helpTag = null;

        private string noNameMessage = "You must enter a name.";
        private string noNameMessageSpecif = "You must enter a name for the {0}.";
        private string illegalCharsMessage = "The name contains illegal characters.";
        private string illegalCharsMessageSpecif = "The {0} name contains illegal characters.";

        public NameAndDescr2( string title, Color color, string helpTag ) {
            InitializeComponent();
            this.titleLabel.Text = title;
            this.Color = color;
            this.helpTag = helpTag;
        }

        public Color Color {
            set {
                this.BackColor = value;
                this.panel1.BackColor = value;
            }
        }

        public string SpecificItemName {
            set {
                this.specificItemName = value;
            }
        }

        public string ObjName {
            set { this.nameTextBox.Text = value; }
            get { return this.nameTextBox.Text.Trim(); }
        }

        public string ObjDescription {
            set { this.descriptionTextBox.Text = value; }
            get { return this.descriptionTextBox.Text.Trim(); }
        }

        public void DisableNameField() {
            this.nameTextBox.Enabled = false;
            this.descriptionTextBox.SelectAll();
        }

        private void okButton_Click( object sender, EventArgs e ) {


            if( this.nameTextBox.Text.Trim().Length == 0 ){
                string msg = noNameMessage;
                if( this.specificItemName != null ) {
                    msg = String.Format( noNameMessageSpecif, this.specificItemName );
                }
                ConfirmDialog cdlg = new ConfirmDialog( msg, "Name Required" );
                cdlg.ShowDialog();
                return;
            }

            // validate the name
            char[] illegal = {',', '\'', '"', '.', ';'};
            int chkIndex = nameTextBox.Text.IndexOfAny( illegal );
            if( chkIndex >= 0 ) {
                string msg = illegalCharsMessage;
                if( this.specificItemName != null ) {
                    msg = String.Format( illegalCharsMessageSpecif, this.specificItemName );
                }
                ConfirmDialog cdlg = new ConfirmDialog( msg,  "Illegal characters are:", " . , \' \" ;", "Name Invalid" );
                cdlg.SetOkButtonOnlyStyle();
                cdlg.ShowDialog();
                return;
            }

		    this.DialogResult = DialogResult.OK;
        }

        private void helpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, this.helpTag );
        }

        private void NameAndDescr2_Load( object sender, EventArgs e ) {
            this.nameTextBox.Select();
        }
    }
}