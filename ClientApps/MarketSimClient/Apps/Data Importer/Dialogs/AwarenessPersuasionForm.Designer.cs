namespace DataImporter.Dialogs
{
    partial class AwarenessPersuasionForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.displayAwareness = new System.Windows.Forms.TextBox();
            this.displayPersuasion = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.distPersuasion = new System.Windows.Forms.TextBox();
            this.distAwareness = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.mediaPersuasion = new System.Windows.Forms.TextBox();
            this.mediaAwareness = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.couponsPersuasion = new System.Windows.Forms.TextBox();
            this.couponsAwareness = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 77, 34 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 59, 13 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Awareness";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 166, 34 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 59, 13 );
            this.label2.TabIndex = 1;
            this.label2.Text = "Persuasion";
            // 
            // displayAwareness
            // 
            this.displayAwareness.Location = new System.Drawing.Point( 75, 57 );
            this.displayAwareness.Name = "displayAwareness";
            this.displayAwareness.Size = new System.Drawing.Size( 67, 20 );
            this.displayAwareness.TabIndex = 2;
            this.displayAwareness.TextChanged += new System.EventHandler( this.textboxTextChanged );
            // 
            // displayPersuasion
            // 
            this.displayPersuasion.Location = new System.Drawing.Point( 161, 57 );
            this.displayPersuasion.Name = "displayPersuasion";
            this.displayPersuasion.Size = new System.Drawing.Size( 69, 20 );
            this.displayPersuasion.TabIndex = 3;
            this.displayPersuasion.TextChanged += new System.EventHandler( this.textboxTextChanged );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 219, 186 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 19;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 138, 186 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 18;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 12, 60 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 41, 13 );
            this.label3.TabIndex = 20;
            this.label3.Text = "Display";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 12, 86 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 59, 13 );
            this.label4.TabIndex = 23;
            this.label4.Text = "Distribution";
            // 
            // distPersuasion
            // 
            this.distPersuasion.Location = new System.Drawing.Point( 161, 83 );
            this.distPersuasion.Name = "distPersuasion";
            this.distPersuasion.Size = new System.Drawing.Size( 69, 20 );
            this.distPersuasion.TabIndex = 22;
            this.distPersuasion.TextChanged += new System.EventHandler( this.textboxTextChanged );
            // 
            // distAwareness
            // 
            this.distAwareness.Location = new System.Drawing.Point( 75, 83 );
            this.distAwareness.Name = "distAwareness";
            this.distAwareness.Size = new System.Drawing.Size( 67, 20 );
            this.distAwareness.TabIndex = 21;
            this.distAwareness.TextChanged += new System.EventHandler( this.textboxTextChanged );
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 12, 119 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 36, 13 );
            this.label6.TabIndex = 29;
            this.label6.Text = "Media";
            // 
            // mediaPersuasion
            // 
            this.mediaPersuasion.Location = new System.Drawing.Point( 161, 116 );
            this.mediaPersuasion.Name = "mediaPersuasion";
            this.mediaPersuasion.Size = new System.Drawing.Size( 69, 20 );
            this.mediaPersuasion.TabIndex = 28;
            this.mediaPersuasion.TextChanged += new System.EventHandler( this.textboxTextChanged );
            // 
            // mediaAwareness
            // 
            this.mediaAwareness.Location = new System.Drawing.Point( 75, 116 );
            this.mediaAwareness.Name = "mediaAwareness";
            this.mediaAwareness.Size = new System.Drawing.Size( 67, 20 );
            this.mediaAwareness.TabIndex = 27;
            this.mediaAwareness.TextChanged += new System.EventHandler( this.textboxTextChanged );
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 12, 145 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 49, 13 );
            this.label7.TabIndex = 32;
            this.label7.Text = "Coupons";
            // 
            // couponsPersuasion
            // 
            this.couponsPersuasion.Location = new System.Drawing.Point( 161, 142 );
            this.couponsPersuasion.Name = "couponsPersuasion";
            this.couponsPersuasion.Size = new System.Drawing.Size( 69, 20 );
            this.couponsPersuasion.TabIndex = 31;
            this.couponsPersuasion.TextChanged += new System.EventHandler( this.textboxTextChanged );
            // 
            // couponsAwareness
            // 
            this.couponsAwareness.Location = new System.Drawing.Point( 75, 142 );
            this.couponsAwareness.Name = "couponsAwareness";
            this.couponsAwareness.Size = new System.Drawing.Size( 67, 20 );
            this.couponsAwareness.TabIndex = 30;
            this.couponsAwareness.TextChanged += new System.EventHandler( this.textboxTextChanged );
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point( 12, 11 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 196, 13 );
            this.label8.TabIndex = 33;
            this.label8.Text = "Set the values to be used in output files:";
            // 
            // AwarenessPersuasionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 299, 214 );
            this.Controls.Add( this.label8 );
            this.Controls.Add( this.label7 );
            this.Controls.Add( this.couponsPersuasion );
            this.Controls.Add( this.couponsAwareness );
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.mediaPersuasion );
            this.Controls.Add( this.mediaAwareness );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.distPersuasion );
            this.Controls.Add( this.distAwareness );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.displayPersuasion );
            this.Controls.Add( this.displayAwareness );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AwarenessPersuasionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Awareness and Persuasion";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox displayAwareness;
        private System.Windows.Forms.TextBox displayPersuasion;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox distPersuasion;
        private System.Windows.Forms.TextBox distAwareness;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox mediaPersuasion;
        private System.Windows.Forms.TextBox mediaAwareness;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox couponsPersuasion;
        private System.Windows.Forms.TextBox couponsAwareness;
        private System.Windows.Forms.Label label8;
    }
}