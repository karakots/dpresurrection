namespace DataImporter.Dialogs
{
    partial class OutputFolderEditForm
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
            this.browseOutButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.importButton = new System.Windows.Forms.Button();
            this.outputDirTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // browseOutButton
            // 
            this.browseOutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseOutButton.Location = new System.Drawing.Point( 643, 13 );
            this.browseOutButton.Name = "browseOutButton";
            this.browseOutButton.Size = new System.Drawing.Size( 75, 25 );
            this.browseOutButton.TabIndex = 22;
            this.browseOutButton.Text = "Browse...";
            this.browseOutButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 22, 19 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 71, 13 );
            this.label5.TabIndex = 21;
            this.label5.Text = "Output Folder";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 643, 79 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 25 );
            this.cancelButton.TabIndex = 20;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.importButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.importButton.Location = new System.Drawing.Point( 562, 79 );
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size( 75, 25 );
            this.importButton.TabIndex = 19;
            this.importButton.Text = "OK";
            this.importButton.UseVisualStyleBackColor = true;
            // 
            // outputDirTextBox
            // 
            this.outputDirTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outputDirTextBox.Location = new System.Drawing.Point( 99, 16 );
            this.outputDirTextBox.Name = "outputDirTextBox";
            this.outputDirTextBox.Size = new System.Drawing.Size( 538, 20 );
            this.outputDirTextBox.TabIndex = 23;
            // 
            // OutputFolderEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 730, 116 );
            this.Controls.Add( this.outputDirTextBox );
            this.Controls.Add( this.browseOutButton );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.importButton );
            this.Name = "OutputFolderEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Output Folder";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button browseOutButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.TextBox outputDirTextBox;
    }
}