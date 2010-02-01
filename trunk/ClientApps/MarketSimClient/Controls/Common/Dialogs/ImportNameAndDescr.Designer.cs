namespace Common.Dialogs
{
    partial class ImportNameAndDescr
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.helpButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.fileTextBox = new System.Windows.Forms.TextBox();
            this.fileLabel = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(200)))), ((int)(((byte)(250)))), ((int)(((byte)(100)))) );
            this.panel1.Controls.Add( this.helpButton );
            this.panel1.Controls.Add( this.titleLabel );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 542, 26 );
            this.panel1.TabIndex = 8;
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.BackColor = System.Drawing.SystemColors.Control;
            this.helpButton.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.helpButton.Location = new System.Drawing.Point( 514, 2 );
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size( 24, 21 );
            this.helpButton.TabIndex = 13;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler( this.helpButton_Click );
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font( "Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.titleLabel.Location = new System.Drawing.Point( 5, 5 );
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size( 235, 16 );
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Import Distribution Plan Component";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add( this.label2 );
            this.panel2.Controls.Add( this.browseButton );
            this.panel2.Controls.Add( this.fileTextBox );
            this.panel2.Controls.Add( this.fileLabel );
            this.panel2.Controls.Add( this.infoLabel );
            this.panel2.Controls.Add( this.descriptionTextBox );
            this.panel2.Controls.Add( this.nameTextBox );
            this.panel2.Controls.Add( this.label4 );
            this.panel2.Controls.Add( this.nameLabel );
            this.panel2.Location = new System.Drawing.Point( 5, 26 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 532, 172 );
            this.panel2.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Location = new System.Drawing.Point( 5, 102 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 52, 14 );
            this.label2.TabIndex = 14;
            this.label2.Text = "(optional)";
            // 
            // browseButton
            // 
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.browseButton.Location = new System.Drawing.Point( 434, 12 );
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size( 83, 23 );
            this.browseButton.TabIndex = 13;
            this.browseButton.Text = "Browse...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler( this.browseButton_Click );
            // 
            // fileTextBox
            // 
            this.fileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTextBox.Location = new System.Drawing.Point( 93, 14 );
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.Size = new System.Drawing.Size( 338, 21 );
            this.fileTextBox.TabIndex = 12;
            this.fileTextBox.TextChanged += new System.EventHandler( this.fileTextBox_TextChanged );
            // 
            // fileLabel
            // 
            this.fileLabel.AutoSize = true;
            this.fileLabel.Location = new System.Drawing.Point( 5, 16 );
            this.fileLabel.Name = "fileLabel";
            this.fileLabel.Size = new System.Drawing.Size( 84, 15 );
            this.fileLabel.TabIndex = 11;
            this.fileLabel.Text = "Source File(s)";
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.infoLabel.Location = new System.Drawing.Point( 5, 149 );
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size( 516, 17 );
            this.infoLabel.TabIndex = 10;
            this.infoLabel.Text = "If there is any additional info, show it here...";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point( 93, 86 );
            this.descriptionTextBox.MaxLength = 200;
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.descriptionTextBox.Size = new System.Drawing.Size( 424, 56 );
            this.descriptionTextBox.TabIndex = 9;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point( 93, 51 );
            this.nameTextBox.MaxLength = 100;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size( 424, 21 );
            this.nameTextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 5, 89 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 70, 15 );
            this.label4.TabIndex = 7;
            this.label4.Text = "Description";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point( 5, 53 );
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size( 69, 15 );
            this.nameLabel.TabIndex = 6;
            this.nameLabel.Text = "Plan Name";
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.okButton.BackColor = System.Drawing.SystemColors.Control;
            this.okButton.Location = new System.Drawing.Point( 193, 203 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cancelButton.BackColor = System.Drawing.SystemColors.Control;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 283, 203 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // ImportNameAndDescr
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(200)))), ((int)(((byte)(250)))), ((int)(((byte)(100)))) );
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 542, 231 );
            this.Controls.Add( this.panel2 );
            this.Controls.Add( this.panel1 );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.cancelButton );
            this.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size( 443, 231 );
            this.Name = "ImportNameAndDescr";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout( false );
            this.panel2.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox fileTextBox;
        private System.Windows.Forms.Label fileLabel;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button helpButton;
    }
}