namespace DataImporter.Dialogs
{
    partial class FileCombinerForm
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
            this.sourceLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.src2TextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.browseOutButton = new System.Windows.Forms.Button();
            this.outTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ruleComboBox = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 12, 9 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 339, 13 );
            this.label1.TabIndex = 0;
            this.label1.Text = "This tool will create a new set of files by combining two multi-file groups";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 12, 35 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 85, 13 );
            this.label2.TabIndex = 1;
            this.label2.Text = "Source Folder 1:";
            // 
            // sourceLabel
            // 
            this.sourceLabel.AutoSize = true;
            this.sourceLabel.Location = new System.Drawing.Point( 112, 34 );
            this.sourceLabel.Name = "sourceLabel";
            this.sourceLabel.Size = new System.Drawing.Size( 51, 13 );
            this.sourceLabel.TabIndex = 2;
            this.sourceLabel.Text = "<source>";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 12, 63 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 85, 13 );
            this.label4.TabIndex = 3;
            this.label4.Text = "Source Folder 2:";
            // 
            // src2TextBox
            // 
            this.src2TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.src2TextBox.Location = new System.Drawing.Point( 115, 57 );
            this.src2TextBox.Name = "src2TextBox";
            this.src2TextBox.Size = new System.Drawing.Size( 430, 20 );
            this.src2TextBox.TabIndex = 4;
            // 
            // browseButton
            // 
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseButton.Location = new System.Drawing.Point( 551, 55 );
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size( 75, 23 );
            this.browseButton.TabIndex = 5;
            this.browseButton.Text = "Browse...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler( this.browseButton_Click );
            // 
            // browseOutButton
            // 
            this.browseOutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseOutButton.Location = new System.Drawing.Point( 551, 90 );
            this.browseOutButton.Name = "browseOutButton";
            this.browseOutButton.Size = new System.Drawing.Size( 75, 23 );
            this.browseOutButton.TabIndex = 8;
            this.browseOutButton.Text = "Browse...";
            this.browseOutButton.UseVisualStyleBackColor = true;
            this.browseOutButton.Click += new System.EventHandler( this.browseOutButton_Click );
            // 
            // outTextBox
            // 
            this.outTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outTextBox.Location = new System.Drawing.Point( 115, 92 );
            this.outTextBox.Name = "outTextBox";
            this.outTextBox.Size = new System.Drawing.Size( 430, 20 );
            this.outTextBox.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 12, 98 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 74, 13 );
            this.label5.TabIndex = 6;
            this.label5.Text = "Output Folder:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 12, 135 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 89, 13 );
            this.label6.TabIndex = 9;
            this.label6.Text = "Combining Rules:";
            // 
            // ruleComboBox
            // 
            this.ruleComboBox.FormattingEnabled = true;
            this.ruleComboBox.Items.AddRange( new object[] {
            "Dentsu Price from $ and Volume Sales"} );
            this.ruleComboBox.Location = new System.Drawing.Point( 115, 127 );
            this.ruleComboBox.Name = "ruleComboBox";
            this.ruleComboBox.Size = new System.Drawing.Size( 331, 21 );
            this.ruleComboBox.TabIndex = 10;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 473, 155 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 11;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 554, 155 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // FileCombinerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 638, 186 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.ruleComboBox );
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.browseOutButton );
            this.Controls.Add( this.outTextBox );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.browseButton );
            this.Controls.Add( this.src2TextBox );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.sourceLabel );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Name = "FileCombinerForm";
            this.Text = "Combine Input Files";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label sourceLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox src2TextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Button browseOutButton;
        private System.Windows.Forms.TextBox outTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ruleComboBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}