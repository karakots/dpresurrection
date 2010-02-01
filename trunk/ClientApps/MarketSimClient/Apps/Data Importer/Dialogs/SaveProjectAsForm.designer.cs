namespace DataImporter.Dialogs
{
    partial class SaveProjectAsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( SaveProjectAsForm ) );
            this.label1 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.normalizePriceCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.importButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fileTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 23, 25 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 106, 13 );
            this.label1.TabIndex = 3;
            this.label1.Text = "Import Project Name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point( 135, 21 );
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size( 380, 20 );
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.Text = "MarketSim Import Project1";
            // 
            // normalizePriceCheckBox
            // 
            this.normalizePriceCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.normalizePriceCheckBox.AutoSize = true;
            this.normalizePriceCheckBox.Location = new System.Drawing.Point( 26, 172 );
            this.normalizePriceCheckBox.Name = "normalizePriceCheckBox";
            this.normalizePriceCheckBox.Size = new System.Drawing.Size( 159, 17 );
            this.normalizePriceCheckBox.TabIndex = 13;
            this.normalizePriceCheckBox.Text = "Normalize Price Distributions";
            this.normalizePriceCheckBox.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 521, 167 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 25 );
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.importButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.importButton.Location = new System.Drawing.Point( 440, 167 );
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size( 75, 25 );
            this.importButton.TabIndex = 11;
            this.importButton.Text = "OK";
            this.importButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point( 23, 79 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 573, 53 );
            this.label2.TabIndex = 14;
            this.label2.Text = resources.GetString( "label2.Text" );
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 23, 51 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 94, 13 );
            this.label3.TabIndex = 16;
            this.label3.Text = "Import Project File:";
            // 
            // fileTextBox
            // 
            this.fileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTextBox.Location = new System.Drawing.Point( 135, 47 );
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.Size = new System.Drawing.Size( 380, 20 );
            this.fileTextBox.TabIndex = 15;
            this.fileTextBox.Text = "MarketSim Import Project1";
            // 
            // SaveProjectAsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 609, 201 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.fileTextBox );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.normalizePriceCheckBox );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.importButton );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.nameTextBox );
            this.Name = "SaveProjectAsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Save Project As";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.CheckBox normalizePriceCheckBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fileTextBox;
    }
}