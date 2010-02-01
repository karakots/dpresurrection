using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataImporter.Dialogs
{
    public partial class YesNoAllEtcForm : Form
    {
        private bool yesClicked = false;
        private bool yesAllClicked = false;
        private bool noClicked = false;
        private bool renameClicked = false;

        public bool Yes {
            get { return yesClicked; }
        }

        public bool YesToAll {
            get { return yesAllClicked; }
        }

        public bool No {
            get { return noClicked; }
        }

        public bool Rename {
            get { return renameClicked; }
        }

        public YesNoAllEtcForm( string pathThatExists ) {
            InitializeComponent();

            this.nameLabel.Text = pathThatExists;
        }

        private void yesButton_Click( object sender, EventArgs e ) {
            yesClicked = true;
        }

        private void allButton_Click( object sender, EventArgs e ) {
            yesAllClicked = true;
        }

        private void skipButton_Click( object sender, EventArgs e ) {
            noClicked = true;
        }

        private void renameButton_Click( object sender, EventArgs e ) {
            renameClicked = true;
        }
    }
}