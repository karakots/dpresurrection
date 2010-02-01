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
    public partial class ConfirmDialog : Form
    {
        private string helpTag = null;

        private int twoButtonOkX = -1;

        /// <summary>
        /// Creates a new dialog suitable for OK-ing a simple message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        public ConfirmDialog( string message, string title )
            : this( message, title, null ) {
        }

        /// <summary>
        /// Creates a new dialog suitable for OK-ing a simple message.  Help can be provided.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="helpTag"></param>
        public ConfirmDialog( string message, string title, string helpTag )
            : this( message, null, null, title, helpTag ) {
            this.helpTag = helpTag;
        }

        /// <summary>
        /// Creates a new dialog allowing the user to answer Yes or No to a question.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        public ConfirmDialog( string mainMessage, string categoryName, string itemName, string title )
            : this( mainMessage, categoryName, itemName, title, null ) {
        }

        /// <summary>
        /// Creates a new dialog allowing the user to answer Yes or No to a question.
        /// </summary>
        /// <param name="mainMessage"></param>
        /// <param name="categoryName"></param>
        /// <param name="itemName"></param>
        /// <param name="title"></param>
        /// <param name="helpTag"></param>
        public ConfirmDialog( string mainMessage, string categoryName, string itemName, string title, string helpTag ) {

            InitializeComponent();
            twoButtonOkX = this.yesButton.Location.X;
            this.mainLabel.Text = mainMessage;
            this.Text = title;

            if( categoryName != null ) {
                this.typeLabel.Text = categoryName;
                this.nameLabel.Text = itemName;
            }
            else {
                // we are in the OK-only style
                this.typeLabel.Text = "";
                this.nameLabel.Text = "";
                this.Height = this.Height - 40;
                SetOkButtonOnlyStyle();
            }

            this.helpTag = helpTag;
            if( this.helpTag != null ) {
                this.helpButton.Visible = true;
            }
       }

        public void SetOkCancelButtonStyle() {
            this.yesButton.Location = new Point( twoButtonOkX, this.yesButton.Location.Y );
            this.yesButton.Text = "OK";
            this.yesButton.DialogResult = DialogResult.OK;
            this.noButton.Text = "Cancel";
            this.noButton.DialogResult = DialogResult.Cancel;
            this.noButton.Visible = true;
        }

        public void SetOkButtonOnlyStyle() {
            this.yesButton.Text = "OK";
            this.yesButton.DialogResult = DialogResult.OK;
            this.yesButton.Location = new Point( twoButtonOkX + 50, yesButton.Location.Y );
            this.noButton.Visible = false;
        }

        public void SelectQuestionIcon() {
            this.warningPictureBox.Visible = false;
            this.errorPictureBox.Visible = false;
            this.infoPictureBox.Visible = false;
            this.questionPictureBox.Visible = true;
        }

        public void SelectInfoIcon() {
            this.warningPictureBox.Visible = false;
            this.errorPictureBox.Visible = false;
            this.questionPictureBox.Visible = false;
            this.infoPictureBox.Visible = true;
        }

        public void SelectWarningIcon() {
            this.errorPictureBox.Visible = false;
            this.questionPictureBox.Visible = false;
            this.infoPictureBox.Visible = false;
            this.warningPictureBox.Visible = true;
        }

        public void SelectErrorIcon() {
            this.warningPictureBox.Visible = false;
            this.infoPictureBox.Visible = false;
            this.questionPictureBox.Visible = false;
            this.errorPictureBox.Visible = true;
        }

        private void helpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, this.helpTag );
        }
    }
}