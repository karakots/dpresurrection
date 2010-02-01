using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MrktSimDb;
using ModelView;

namespace ModelView.Dialogs
{
	/// <summary>
	/// Summary description for ModelOptionsDialog.
	/// </summary>
    public class MarketPlanOptionsDialog : System.Windows.Forms.Form
	{
        private string confirmMessage = "Warning: Disabling a feature will cause defaults to be used.\r\n" +
            "This will change the results of the simulation and possibly invalidate the model.\r\n\r\nProceed?";

        private string confirmTitle = "Confirm Feature Disable";

		private System.Windows.Forms.CheckBox displayBox;
		private System.Windows.Forms.CheckBox distributionBox;
        private System.Windows.Forms.CheckBox promoPriceBox;
		private System.Windows.Forms.CheckBox MarketUtilityBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		protected ModelDb theDb;

		public ModelDb Db
		{
			set { theDb = value;	}
		}

        public MarketPlanOptionsDialog( ModelDb Db ) {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            theDb = Db;

            this.promoPriceBox.Checked = theDb.Model.promoted_price;
            this.distributionBox.Checked = theDb.Model.distribution;
            this.displayBox.Checked = theDb.Model.display;
            this.MarketUtilityBox.Checked = theDb.Model.market_utility;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>

		/// Required method for Designer support - do not modify

		/// the contents of this method with the code editor.

		/// </summary>

		private void InitializeComponent()

		{
            this.displayBox = new System.Windows.Forms.CheckBox();
            this.distributionBox = new System.Windows.Forms.CheckBox();
            this.promoPriceBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.MarketUtilityBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // displayBox
            // 
            this.displayBox.Location = new System.Drawing.Point( 16, 40 );
            this.displayBox.Name = "displayBox";
            this.displayBox.Size = new System.Drawing.Size( 232, 24 );
            this.displayBox.TabIndex = 22;
            this.displayBox.Text = "Enable display";
            // 
            // distributionBox
            // 
            this.distributionBox.Location = new System.Drawing.Point( 16, 16 );
            this.distributionBox.Name = "distributionBox";
            this.distributionBox.Size = new System.Drawing.Size( 232, 24 );
            this.distributionBox.TabIndex = 21;
            this.distributionBox.Text = "Use distribution";
            // 
            // promoPriceBox
            // 
            this.promoPriceBox.Location = new System.Drawing.Point( 161, 16 );
            this.promoPriceBox.Name = "promoPriceBox";
            this.promoPriceBox.Size = new System.Drawing.Size( 232, 24 );
            this.promoPriceBox.TabIndex = 20;
            this.promoPriceBox.Text = "Enable promoted prices";
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.okButton.Location = new System.Drawing.Point( 80, 114 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 72, 24 );
            this.okButton.TabIndex = 26;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 168, 114 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 27;
            this.cancelButton.Text = "Cancel";
            // 
            // MarketUtilityBox
            // 
            this.MarketUtilityBox.Location = new System.Drawing.Point( 16, 64 );
            this.MarketUtilityBox.Name = "MarketUtilityBox";
            this.MarketUtilityBox.Size = new System.Drawing.Size( 232, 24 );
            this.MarketUtilityBox.TabIndex = 28;
            this.MarketUtilityBox.Text = "Use market utility";
            // 
            // MarketPlanOptionsDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 322, 148 );
            this.Controls.Add( this.promoPriceBox );
            this.Controls.Add( this.MarketUtilityBox );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.displayBox );
            this.Controls.Add( this.distributionBox );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MarketPlanOptionsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Market Plan Options";
            this.ResumeLayout( false );

		}

		#endregion	

        private void applyChanges() {
            // check if are turning anything off

            if( (theDb.Model.promoted_price && !this.promoPriceBox.Checked) ||
                (theDb.Model.distribution && !this.distributionBox.Checked) ||
                (theDb.Model.market_utility && !this.MarketUtilityBox.Checked) ||
                (theDb.Model.display && !this.displayBox.Checked) ) {
                Common.Dialogs.ConfirmDialog cdlg = new Common.Dialogs.ConfirmDialog( confirmMessage, confirmTitle );
                cdlg.SelectWarningIcon();
                cdlg.SetOkCancelButtonStyle();
                cdlg.Height += 50;
                DialogResult rslt = cdlg.ShowDialog();

                if( rslt == DialogResult.Cancel ) {
                    return;
                }
            }

            // for each check bod we need to set the appropriate Model attribute
            theDb.Model.promoted_price = this.promoPriceBox.Checked;
            theDb.Model.distribution = this.distributionBox.Checked;
            theDb.Model.display = this.displayBox.Checked;
            theDb.Model.market_utility = this.MarketUtilityBox.Checked;
        }

        private void okButton_Click( object sender, System.EventArgs e ) {
            applyChanges();
            this.DialogResult = DialogResult.OK;
        }
	}
}