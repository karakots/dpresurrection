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
	public class ModelOptionsDialog : System.Windows.Forms.Form
	{
        private string confirmMessage = "Warning: Disabling a feature will cause defaults to be used.\r\n" + 
            "This will change the results of the simulation and possibly invalidate the model.\r\n\r\nProceed?";
        private string confirmTitle = "Confirm Feature Disable";
        private string removeCheckPointMessage = "Are you sure you want to remove the check point set for {0}?";
        private string removeCheckPointTitle = "Confirm Check Point Removal";

        private System.Windows.Forms.CheckBox postUseAttributeBox;
		private System.Windows.Forms.CheckBox socNetworkBox;
		private System.Windows.Forms.CheckBox productDependencyBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.CheckBox budgetBox;
		private System.Windows.Forms.CheckBox checkPointBox;

		protected ModelDb theDb;

		public ModelDb Db
		{
			set { theDb = value; }
		}

		public ModelOptionsDialog(ModelDb Db)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			theDb = Db;

			//
			//this.taskBox.Checked = theDb.Model.task_based;
			this.socNetworkBox.Checked = theDb.Model.social_network;
			//this.profitLossBox.Checked = theDb.Model.profit_loss;
			//this.lineExtBox.Checked = theDb.Model.product_extensions;
			this.productDependencyBox.Checked = theDb.Model.product_dependency;
			//this.segmentGrowthBox.Checked = theDb.Model.segment_growth;
			this.budgetBox.Checked = theDb.Model.consumer_budget;
			this.postUseAttributeBox.Checked = theDb.Model.attribute_pre_and_post;
			if (!theDb.Model.checkpoint_valid)
			{
				this.checkPointBox.Enabled = false;
			}

            // do not use budget
            budgetBox.Visible = false;
            budgetBox.Enabled = false;

			if(ProjectDb.Nimo)
			{
				postUseAttributeBox.Visible = true;
				postUseAttributeBox.Enabled = true;
			}
			else
			{
				postUseAttributeBox.Visible = false;
				postUseAttributeBox.Enabled = false;
			}
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
            this.postUseAttributeBox = new System.Windows.Forms.CheckBox();
            this.socNetworkBox = new System.Windows.Forms.CheckBox();
            this.productDependencyBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.budgetBox = new System.Windows.Forms.CheckBox();
            this.checkPointBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // postUseAttributeBox
            // 
            this.postUseAttributeBox.Location = new System.Drawing.Point( 16, 16 );
            this.postUseAttributeBox.Name = "postUseAttributeBox";
            this.postUseAttributeBox.Size = new System.Drawing.Size( 232, 24 );
            this.postUseAttributeBox.TabIndex = 24;
            this.postUseAttributeBox.Text = "Enable post-use attibute values";
            // 
            // socNetworkBox
            // 
            this.socNetworkBox.Location = new System.Drawing.Point( 16, 40 );
            this.socNetworkBox.Name = "socNetworkBox";
            this.socNetworkBox.Size = new System.Drawing.Size( 232, 24 );
            this.socNetworkBox.TabIndex = 23;
            this.socNetworkBox.Text = "Enable social networks";
            // 
            // productDependencyBox
            // 
            this.productDependencyBox.Location = new System.Drawing.Point( 16, 64 );
            this.productDependencyBox.Name = "productDependencyBox";
            this.productDependencyBox.Size = new System.Drawing.Size( 232, 24 );
            this.productDependencyBox.TabIndex = 16;
            this.productDependencyBox.Text = "Consider product dependencies";
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.okButton.Location = new System.Drawing.Point( 27, 136 );
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
            this.cancelButton.Location = new System.Drawing.Point( 112, 136 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 27;
            this.cancelButton.Text = "Cancel";
            // 
            // budgetBox
            // 
            this.budgetBox.Location = new System.Drawing.Point( 16, 16 );
            this.budgetBox.Name = "budgetBox";
            this.budgetBox.Size = new System.Drawing.Size( 232, 24 );
            this.budgetBox.TabIndex = 18;
            this.budgetBox.Text = "Enable consumer budgets";
            // 
            // checkPointBox
            // 
            this.checkPointBox.Location = new System.Drawing.Point( 16, 96 );
            this.checkPointBox.Name = "checkPointBox";
            this.checkPointBox.Size = new System.Drawing.Size( 152, 24 );
            this.checkPointBox.TabIndex = 28;
            this.checkPointBox.Text = "Remove checkpoint";
            // 
            // ModelOptionsDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 210, 168 );
            this.Controls.Add( this.checkPointBox );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.postUseAttributeBox );
            this.Controls.Add( this.socNetworkBox );
            this.Controls.Add( this.budgetBox );
            this.Controls.Add( this.productDependencyBox );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModelOptionsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Model Options";
            this.ResumeLayout( false );

		}
		#endregion

		
		private void applyChanges()
		{
			// check if are turning anything off

			if (//(theDb.Model.task_based && !this.taskBox.Checked) ||
				(theDb.Model.social_network && !this.socNetworkBox.Checked) ||
				//(theDb.Model.profit_loss && !this.profitLossBox.Checked) ||
				//(theDb.Model.product_extensions && !this.lineExtBox.Checked) ||
				(theDb.Model.product_dependency && !this.productDependencyBox.Checked) ||
				//(theDb.Model.segment_growth && !this.segmentGrowthBox.Checked) ||
				(theDb.Model.consumer_budget && !this.budgetBox.Checked) ||
				(theDb.Model.attribute_pre_and_post && !this.postUseAttributeBox.Checked))
			{
                Common.Dialogs.ConfirmDialog cdlg = new Common.Dialogs.ConfirmDialog( confirmMessage, confirmTitle );
                cdlg.SelectWarningIcon();
                cdlg.SetOkCancelButtonStyle();
                cdlg.Height += 50;
                DialogResult rslt = cdlg.ShowDialog();

                if( rslt != DialogResult.OK ) {
                    return;
                }
			}
			if (theDb.Model.checkpoint_valid && this.checkPointBox.Checked)
			{
                string msg = String.Format( removeCheckPointMessage, theDb.Model.checkpoint_date.ToShortDateString() );
                Common.Dialogs.ConfirmDialog cdlg = new Common.Dialogs.ConfirmDialog( msg, removeCheckPointTitle );
                cdlg.SelectWarningIcon();
                DialogResult rslt = cdlg.ShowDialog();

				if (rslt == DialogResult.Yes)
				{
					theDb.Model.checkpoint_valid = false;
				}
			}

			// for each check bod we need to set the appropriate Model attribute
			//theDb.Model.task_based = this.taskBox.Checked;
			theDb.Model.social_network = this.socNetworkBox.Checked;
			//theDb.Model.profit_loss = this.profitLossBox.Checked;
			//theDb.Model.product_extensions = this.lineExtBox.Checked;
			theDb.Model.product_dependency = this.productDependencyBox.Checked;
			//theDb.Model.segment_growth = this.segmentGrowthBox.Checked;
			theDb.Model.consumer_budget = this.budgetBox.Checked;
			theDb.Model.attribute_pre_and_post = this.postUseAttributeBox.Checked;
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			applyChanges();

			this.DialogResult = DialogResult.OK;
		}

	}
}
