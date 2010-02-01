namespace DataImporter.Dialogs
{
    partial class NewProject
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
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.importButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.normalizePriceCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.inputDirTextBox = new System.Windows.Forms.TextBox();
            this.outputDirTextBox = new System.Windows.Forms.TextBox();
            this.browseInButton = new System.Windows.Forms.Button();
            this.browseOutButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point( 124, 16 );
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size( 499, 20 );
            this.nameTextBox.TabIndex = 0;
            this.nameTextBox.Text = "MarketSim Import Project1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 12, 20 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 105, 14 );
            this.label1.TabIndex = 1;
            this.label1.Text = "Import Project Name:";
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.importButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.importButton.Location = new System.Drawing.Point( 563, 162 );
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size( 75, 25 );
            this.importButton.TabIndex = 8;
            this.importButton.Text = "OK";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler( this.importButtonClick );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 644, 162 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 25 );
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // normalizePriceCheckBox
            // 
            this.normalizePriceCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.normalizePriceCheckBox.AutoSize = true;
            this.normalizePriceCheckBox.Location = new System.Drawing.Point( 17, 166 );
            this.normalizePriceCheckBox.Name = "normalizePriceCheckBox";
            this.normalizePriceCheckBox.Size = new System.Drawing.Size( 162, 18 );
            this.normalizePriceCheckBox.TabIndex = 10;
            this.normalizePriceCheckBox.Text = "Normalize Price Distributions";
            this.normalizePriceCheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 14, 62 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 105, 14 );
            this.label3.TabIndex = 12;
            this.label3.Text = "Folder for Input Data";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 14, 117 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 72, 14 );
            this.label5.TabIndex = 14;
            this.label5.Text = "Output Folder";
            // 
            // inputDirTextBox
            // 
            this.inputDirTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.inputDirTextBox.Location = new System.Drawing.Point( 124, 59 );
            this.inputDirTextBox.Name = "inputDirTextBox";
            this.inputDirTextBox.Size = new System.Drawing.Size( 499, 20 );
            this.inputDirTextBox.TabIndex = 15;
            // 
            // outputDirTextBox
            // 
            this.outputDirTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outputDirTextBox.Location = new System.Drawing.Point( 124, 114 );
            this.outputDirTextBox.Name = "outputDirTextBox";
            this.outputDirTextBox.Size = new System.Drawing.Size( 499, 20 );
            this.outputDirTextBox.TabIndex = 16;
            // 
            // browseInButton
            // 
            this.browseInButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseInButton.Location = new System.Drawing.Point( 640, 57 );
            this.browseInButton.Name = "browseInButton";
            this.browseInButton.Size = new System.Drawing.Size( 75, 25 );
            this.browseInButton.TabIndex = 17;
            this.browseInButton.Text = "Browse...";
            this.browseInButton.UseVisualStyleBackColor = true;
            this.browseInButton.Click += new System.EventHandler( this.browseInButton_Click );
            // 
            // browseOutButton
            // 
            this.browseOutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseOutButton.Location = new System.Drawing.Point( 640, 112 );
            this.browseOutButton.Name = "browseOutButton";
            this.browseOutButton.Size = new System.Drawing.Size( 75, 25 );
            this.browseOutButton.TabIndex = 18;
            this.browseOutButton.Text = "Browse...";
            this.browseOutButton.UseVisualStyleBackColor = true;
            this.browseOutButton.Click += new System.EventHandler( this.browseOutButton_Click );
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label2.Location = new System.Drawing.Point( 24, 82 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 366, 14 );
            this.label2.TabIndex = 19;
            this.label2.Text = "All input data files for the project must be in this folder or subfolders of it.";
            // 
            // NewProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 14F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 727, 195 );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.browseOutButton );
            this.Controls.Add( this.browseInButton );
            this.Controls.Add( this.outputDirTextBox );
            this.Controls.Add( this.inputDirTextBox );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.normalizePriceCheckBox );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.importButton );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.nameTextBox );
            this.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New Project";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox normalizePriceCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox inputDirTextBox;
        private System.Windows.Forms.TextBox outputDirTextBox;
        private System.Windows.Forms.Button browseInButton;
        private System.Windows.Forms.Button browseOutButton;
        private System.Windows.Forms.Label label2;
    }
}