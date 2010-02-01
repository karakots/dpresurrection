namespace DataImporter.Dialogs
{
    partial class AddFileForm
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
            this.importButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.brandTextBox = new System.Windows.Forms.TextBox();
            this.brandFileSpecRadioButton = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chanFileWsRdioButton = new System.Windows.Forms.RadioButton();
            this.chanFileSpecRadioButton = new System.Windows.Forms.RadioButton();
            this.chanTextBox = new System.Windows.Forms.TextBox();
            this.chanFileDataRadioButton = new System.Windows.Forms.RadioButton();
            this.chanFileNameRadioButton = new System.Windows.Forms.RadioButton();
            this.brandFileNameRadioButton = new System.Windows.Forms.RadioButton();
            this.brandFileDataRadioButton = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.brandFileWsRadioButton = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 12, 19 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 48, 13 );
            this.label1.TabIndex = 1;
            this.label1.Text = "Add File:";
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.importButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.importButton.Location = new System.Drawing.Point( 493, 232 );
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size( 75, 23 );
            this.importButton.TabIndex = 8;
            this.importButton.Text = "Import...";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler( this.importButtonClick );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 574, 232 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // brandTextBox
            // 
            this.brandTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.brandTextBox.Enabled = false;
            this.brandTextBox.Location = new System.Drawing.Point( 221, 106 );
            this.brandTextBox.Name = "brandTextBox";
            this.brandTextBox.Size = new System.Drawing.Size( 412, 20 );
            this.brandTextBox.TabIndex = 17;
            // 
            // brandFileSpecRadioButton
            // 
            this.brandFileSpecRadioButton.AutoSize = true;
            this.brandFileSpecRadioButton.Location = new System.Drawing.Point( 101, 107 );
            this.brandFileSpecRadioButton.Name = "brandFileSpecRadioButton";
            this.brandFileSpecRadioButton.Size = new System.Drawing.Size( 103, 17 );
            this.brandFileSpecRadioButton.TabIndex = 16;
            this.brandFileSpecRadioButton.Text = "Specified Brand:";
            this.brandFileSpecRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add( this.chanFileWsRdioButton );
            this.panel1.Controls.Add( this.chanFileSpecRadioButton );
            this.panel1.Controls.Add( this.chanTextBox );
            this.panel1.Controls.Add( this.chanFileDataRadioButton );
            this.panel1.Controls.Add( this.chanFileNameRadioButton );
            this.panel1.Location = new System.Drawing.Point( 93, 136 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 552, 87 );
            this.panel1.TabIndex = 15;
            // 
            // chanFileWsRdioButton
            // 
            this.chanFileWsRdioButton.AutoSize = true;
            this.chanFileWsRdioButton.Location = new System.Drawing.Point( 8, 22 );
            this.chanFileWsRdioButton.Name = "chanFileWsRdioButton";
            this.chanFileWsRdioButton.Size = new System.Drawing.Size( 120, 17 );
            this.chanFileWsRdioButton.TabIndex = 21;
            this.chanFileWsRdioButton.Text = "Name of Worksheet";
            this.chanFileWsRdioButton.UseVisualStyleBackColor = true;
            // 
            // chanFileSpecRadioButton
            // 
            this.chanFileSpecRadioButton.AutoSize = true;
            this.chanFileSpecRadioButton.Checked = true;
            this.chanFileSpecRadioButton.Location = new System.Drawing.Point( 8, 63 );
            this.chanFileSpecRadioButton.Name = "chanFileSpecRadioButton";
            this.chanFileSpecRadioButton.Size = new System.Drawing.Size( 114, 17 );
            this.chanFileSpecRadioButton.TabIndex = 2;
            this.chanFileSpecRadioButton.TabStop = true;
            this.chanFileSpecRadioButton.Text = "Specified Channel:";
            this.chanFileSpecRadioButton.UseVisualStyleBackColor = true;
            this.chanFileSpecRadioButton.CheckedChanged += new System.EventHandler( this.chanFileSpecRadioButton_CheckedChanged );
            // 
            // chanTextBox
            // 
            this.chanTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chanTextBox.Enabled = false;
            this.chanTextBox.Location = new System.Drawing.Point( 128, 61 );
            this.chanTextBox.Name = "chanTextBox";
            this.chanTextBox.Size = new System.Drawing.Size( 408, 20 );
            this.chanTextBox.TabIndex = 6;
            this.chanTextBox.Text = "All";
            // 
            // chanFileDataRadioButton
            // 
            this.chanFileDataRadioButton.AutoSize = true;
            this.chanFileDataRadioButton.Location = new System.Drawing.Point( 8, 42 );
            this.chanFileDataRadioButton.Name = "chanFileDataRadioButton";
            this.chanFileDataRadioButton.Size = new System.Drawing.Size( 79, 17 );
            this.chanFileDataRadioButton.TabIndex = 1;
            this.chanFileDataRadioButton.Text = "In File Data";
            this.chanFileDataRadioButton.UseVisualStyleBackColor = true;
            // 
            // chanFileNameRadioButton
            // 
            this.chanFileNameRadioButton.AutoSize = true;
            this.chanFileNameRadioButton.Location = new System.Drawing.Point( 8, 3 );
            this.chanFileNameRadioButton.Name = "chanFileNameRadioButton";
            this.chanFileNameRadioButton.Size = new System.Drawing.Size( 85, 17 );
            this.chanFileNameRadioButton.TabIndex = 0;
            this.chanFileNameRadioButton.Text = "Name of FIle";
            this.chanFileNameRadioButton.UseVisualStyleBackColor = true;
            // 
            // brandFileNameRadioButton
            // 
            this.brandFileNameRadioButton.AutoSize = true;
            this.brandFileNameRadioButton.Location = new System.Drawing.Point( 101, 47 );
            this.brandFileNameRadioButton.Name = "brandFileNameRadioButton";
            this.brandFileNameRadioButton.Size = new System.Drawing.Size( 84, 17 );
            this.brandFileNameRadioButton.TabIndex = 14;
            this.brandFileNameRadioButton.Text = "Name of File";
            this.brandFileNameRadioButton.UseVisualStyleBackColor = true;
            // 
            // brandFileDataRadioButton
            // 
            this.brandFileDataRadioButton.AutoSize = true;
            this.brandFileDataRadioButton.Location = new System.Drawing.Point( 101, 87 );
            this.brandFileDataRadioButton.Name = "brandFileDataRadioButton";
            this.brandFileDataRadioButton.Size = new System.Drawing.Size( 79, 17 );
            this.brandFileDataRadioButton.TabIndex = 13;
            this.brandFileDataRadioButton.Text = "In File Data";
            this.brandFileDataRadioButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 12, 47 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 75, 13 );
            this.label5.TabIndex = 12;
            this.label5.Text = "Brand Source:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 12, 139 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 86, 13 );
            this.label4.TabIndex = 11;
            this.label4.Text = "Channel Source:";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.nameLabel.Location = new System.Drawing.Point( 66, 19 );
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size( 94, 13 );
            this.nameLabel.TabIndex = 19;
            this.nameLabel.Text = "File Name Here";
            // 
            // brandFileWsRadioButton
            // 
            this.brandFileWsRadioButton.AutoSize = true;
            this.brandFileWsRadioButton.Checked = true;
            this.brandFileWsRadioButton.Location = new System.Drawing.Point( 101, 66 );
            this.brandFileWsRadioButton.Name = "brandFileWsRadioButton";
            this.brandFileWsRadioButton.Size = new System.Drawing.Size( 120, 17 );
            this.brandFileWsRadioButton.TabIndex = 20;
            this.brandFileWsRadioButton.TabStop = true;
            this.brandFileWsRadioButton.Text = "Name of Worksheet";
            this.brandFileWsRadioButton.UseVisualStyleBackColor = true;
            // 
            // AddFileForm
            // 
            this.AcceptButton = this.importButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 657, 262 );
            this.Controls.Add( this.brandFileWsRadioButton );
            this.Controls.Add( this.nameLabel );
            this.Controls.Add( this.brandTextBox );
            this.Controls.Add( this.brandFileSpecRadioButton );
            this.Controls.Add( this.panel1 );
            this.Controls.Add( this.brandFileNameRadioButton );
            this.Controls.Add( this.brandFileDataRadioButton );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.importButton );
            this.Controls.Add( this.label1 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddFileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Individual File to Projact";
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox brandTextBox;
        private System.Windows.Forms.RadioButton brandFileSpecRadioButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton chanFileSpecRadioButton;
        private System.Windows.Forms.TextBox chanTextBox;
        private System.Windows.Forms.RadioButton chanFileDataRadioButton;
        private System.Windows.Forms.RadioButton chanFileNameRadioButton;
        private System.Windows.Forms.RadioButton brandFileNameRadioButton;
        private System.Windows.Forms.RadioButton brandFileDataRadioButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.RadioButton chanFileWsRdioButton;
        private System.Windows.Forms.RadioButton brandFileWsRadioButton;
    }
}