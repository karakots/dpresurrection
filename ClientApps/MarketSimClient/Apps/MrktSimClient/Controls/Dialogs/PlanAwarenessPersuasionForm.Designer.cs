namespace MrktSimClient.Controls.Dialogs
{
    partial class PlanAwarenessPersuasionForm
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.planNameLabel = new System.Windows.Forms.Label();
            this.awLabel = new System.Windows.Forms.Label();
            this.perLabel = new System.Windows.Forms.Label();
            this.awarenessTextBox = new System.Windows.Forms.TextBox();
            this.persuasionTextBox = new System.Windows.Forms.TextBox();
            this.awAvgLabel = new System.Windows.Forms.Label();
            this.perAvgLabel = new System.Windows.Forms.Label();
            this.setSpecificCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 291, 97 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 372, 97 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 12, 9 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 100, 13 );
            this.label1.TabIndex = 2;
            this.label1.Text = "Component to Alter:";
            // 
            // planNameLabel
            // 
            this.planNameLabel.AutoSize = true;
            this.planNameLabel.Location = new System.Drawing.Point( 118, 9 );
            this.planNameLabel.Name = "planNameLabel";
            this.planNameLabel.Size = new System.Drawing.Size( 95, 13 );
            this.planNameLabel.TabIndex = 3;
            this.planNameLabel.Text = "<selected plan{s}>";
            // 
            // awLabel
            // 
            this.awLabel.Location = new System.Drawing.Point( 12, 40 );
            this.awLabel.Name = "awLabel";
            this.awLabel.Size = new System.Drawing.Size( 156, 23 );
            this.awLabel.TabIndex = 4;
            this.awLabel.Text = "Awareness:";
            this.awLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // perLabel
            // 
            this.perLabel.Location = new System.Drawing.Point( 12, 66 );
            this.perLabel.Name = "perLabel";
            this.perLabel.Size = new System.Drawing.Size( 156, 23 );
            this.perLabel.TabIndex = 5;
            this.perLabel.Text = "Persuasion:";
            this.perLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // awarenessTextBox
            // 
            this.awarenessTextBox.Location = new System.Drawing.Point( 174, 37 );
            this.awarenessTextBox.Name = "awarenessTextBox";
            this.awarenessTextBox.Size = new System.Drawing.Size( 69, 20 );
            this.awarenessTextBox.TabIndex = 6;
            // 
            // persuasionTextBox
            // 
            this.persuasionTextBox.Location = new System.Drawing.Point( 174, 63 );
            this.persuasionTextBox.Name = "persuasionTextBox";
            this.persuasionTextBox.Size = new System.Drawing.Size( 69, 20 );
            this.persuasionTextBox.TabIndex = 7;
            // 
            // awAvgLabel
            // 
            this.awAvgLabel.AutoSize = true;
            this.awAvgLabel.Location = new System.Drawing.Point( 249, 40 );
            this.awAvgLabel.Name = "awAvgLabel";
            this.awAvgLabel.Size = new System.Drawing.Size( 126, 13 );
            this.awAvgLabel.TabIndex = 8;
            this.awAvgLabel.Text = "Awareness Average = 99";
            this.awAvgLabel.Visible = false;
            // 
            // perAvgLabel
            // 
            this.perAvgLabel.AutoSize = true;
            this.perAvgLabel.Location = new System.Drawing.Point( 249, 66 );
            this.perAvgLabel.Name = "perAvgLabel";
            this.perAvgLabel.Size = new System.Drawing.Size( 126, 13 );
            this.perAvgLabel.TabIndex = 9;
            this.perAvgLabel.Text = "Persuasion Average: ???";
            this.perAvgLabel.Visible = false;
            // 
            // setSpecificCheckBox
            // 
            this.setSpecificCheckBox.AutoSize = true;
            this.setSpecificCheckBox.Location = new System.Drawing.Point( 15, 99 );
            this.setSpecificCheckBox.Name = "setSpecificCheckBox";
            this.setSpecificCheckBox.Size = new System.Drawing.Size( 115, 17 );
            this.setSpecificCheckBox.TabIndex = 10;
            this.setSpecificCheckBox.Text = "Set specific values";
            this.setSpecificCheckBox.UseVisualStyleBackColor = true;
            this.setSpecificCheckBox.Visible = false;
            this.setSpecificCheckBox.CheckedChanged += new System.EventHandler( this.setSpecificCheckBox_CheckedChanged );
            // 
            // PlanAwarenessPersuasionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 455, 128 );
            this.Controls.Add( this.setSpecificCheckBox );
            this.Controls.Add( this.perAvgLabel );
            this.Controls.Add( this.awAvgLabel );
            this.Controls.Add( this.persuasionTextBox );
            this.Controls.Add( this.awarenessTextBox );
            this.Controls.Add( this.perLabel );
            this.Controls.Add( this.awLabel );
            this.Controls.Add( this.planNameLabel );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Name = "PlanAwarenessPersuasionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Awareness and Persuasion";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label planNameLabel;
        private System.Windows.Forms.Label awLabel;
        private System.Windows.Forms.Label perLabel;
        private System.Windows.Forms.TextBox awarenessTextBox;
        private System.Windows.Forms.TextBox persuasionTextBox;
        private System.Windows.Forms.Label awAvgLabel;
        private System.Windows.Forms.Label perAvgLabel;
        private System.Windows.Forms.CheckBox setSpecificCheckBox;
    }
}