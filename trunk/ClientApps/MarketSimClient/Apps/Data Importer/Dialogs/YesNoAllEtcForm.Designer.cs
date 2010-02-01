namespace DataImporter.Dialogs
{
    partial class YesNoAllEtcForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.yesButton = new System.Windows.Forms.Button();
            this.allButton = new System.Windows.Forms.Button();
            this.skipButton = new System.Windows.Forms.Button();
            this.renameButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 12, 9 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 118, 13 );
            this.label1.TabIndex = 0;
            this.label1.Text = "File Exixts Already: ";
            // 
            // nameLabel
            // 
            this.nameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameLabel.Location = new System.Drawing.Point( 12, 31 );
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size( 565, 31 );
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "<file name>";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 12, 71 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 86, 13 );
            this.label3.TabIndex = 2;
            this.label3.Text = "OK to overwrite?";
            // 
            // yesButton
            // 
            this.yesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.yesButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.yesButton.Location = new System.Drawing.Point( 15, 102 );
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size( 75, 24 );
            this.yesButton.TabIndex = 3;
            this.yesButton.Text = "Yes";
            this.yesButton.UseVisualStyleBackColor = true;
            this.yesButton.Click += new System.EventHandler( this.yesButton_Click );
            // 
            // allButton
            // 
            this.allButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.allButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.allButton.Location = new System.Drawing.Point( 107, 102 );
            this.allButton.Name = "allButton";
            this.allButton.Size = new System.Drawing.Size( 75, 24 );
            this.allButton.TabIndex = 4;
            this.allButton.Text = "Yes to All";
            this.allButton.UseVisualStyleBackColor = true;
            this.allButton.Click += new System.EventHandler( this.allButton_Click );
            // 
            // skipButton
            // 
            this.skipButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.skipButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.skipButton.Location = new System.Drawing.Point( 201, 102 );
            this.skipButton.Name = "skipButton";
            this.skipButton.Size = new System.Drawing.Size( 104, 24 );
            this.skipButton.TabIndex = 5;
            this.skipButton.Text = "No - Skip this File";
            this.skipButton.UseVisualStyleBackColor = true;
            this.skipButton.Click += new System.EventHandler( this.skipButton_Click );
            // 
            // renameButton
            // 
            this.renameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.renameButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.renameButton.Location = new System.Drawing.Point( 322, 102 );
            this.renameButton.Name = "renameButton";
            this.renameButton.Size = new System.Drawing.Size( 153, 24 );
            this.renameButton.TabIndex = 6;
            this.renameButton.Text = "No - Gemerate New Names";
            this.renameButton.UseVisualStyleBackColor = true;
            this.renameButton.Click += new System.EventHandler( this.renameButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 493, 102 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 24 );
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // YesNoAllEtcForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 589, 141 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.renameButton );
            this.Controls.Add( this.skipButton );
            this.Controls.Add( this.allButton );
            this.Controls.Add( this.yesButton );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.nameLabel );
            this.Controls.Add( this.label1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YesNoAllEtcForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Exists";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button allButton;
        private System.Windows.Forms.Button skipButton;
        private System.Windows.Forms.Button renameButton;
        private System.Windows.Forms.Button cancelButton;
    }
}