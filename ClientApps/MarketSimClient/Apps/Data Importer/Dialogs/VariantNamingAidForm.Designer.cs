namespace DataImporter.Dialogs
{
    partial class VariantNamingAidForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.autoMatchButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.inputListBox = new System.Windows.Forms.ListBox();
            this.showAllCheckBox = new System.Windows.Forms.CheckBox();
            this.loadNamesButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.marketSimNameTextBox = new System.Windows.Forms.TextBox();
            this.outputListBox = new System.Windows.Forms.ListBox();
            this.addButton = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 577, 383 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 658, 383 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))) );
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.addButton );
            this.splitContainer1.Panel1.Controls.Add( this.autoMatchButton );
            this.splitContainer1.Panel1.Controls.Add( this.label2 );
            this.splitContainer1.Panel1.Controls.Add( this.inputListBox );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.showAllCheckBox );
            this.splitContainer1.Panel2.Controls.Add( this.loadNamesButton );
            this.splitContainer1.Panel2.Controls.Add( this.label3 );
            this.splitContainer1.Panel2.Controls.Add( this.label1 );
            this.splitContainer1.Panel2.Controls.Add( this.marketSimNameTextBox );
            this.splitContainer1.Panel2.Controls.Add( this.outputListBox );
            this.splitContainer1.Size = new System.Drawing.Size( 733, 377 );
            this.splitContainer1.SplitterDistance = 374;
            this.splitContainer1.TabIndex = 9;
            // 
            // autoMatchButton
            // 
            this.autoMatchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.autoMatchButton.Location = new System.Drawing.Point( 175, 6 );
            this.autoMatchButton.Name = "autoMatchButton";
            this.autoMatchButton.Size = new System.Drawing.Size( 96, 23 );
            this.autoMatchButton.TabIndex = 16;
            this.autoMatchButton.Text = "Automatch";
            this.autoMatchButton.UseVisualStyleBackColor = true;
            this.autoMatchButton.Click += new System.EventHandler( this.autoMatchButton_Click_1 );
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 12, 9 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 127, 13 );
            this.label2.TabIndex = 6;
            this.label2.Text = "Imported Variant Identifier";
            // 
            // inputListBox
            // 
            this.inputListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.inputListBox.FormattingEnabled = true;
            this.inputListBox.Location = new System.Drawing.Point( 12, 35 );
            this.inputListBox.Name = "inputListBox";
            this.inputListBox.Size = new System.Drawing.Size( 359, 329 );
            this.inputListBox.TabIndex = 5;
            this.inputListBox.SelectedIndexChanged += new System.EventHandler( this.inputListBox_SelectedIndexChanged );
            // 
            // showAllCheckBox
            // 
            this.showAllCheckBox.AutoSize = true;
            this.showAllCheckBox.Location = new System.Drawing.Point( 182, 77 );
            this.showAllCheckBox.Name = "showAllCheckBox";
            this.showAllCheckBox.Size = new System.Drawing.Size( 67, 17 );
            this.showAllCheckBox.TabIndex = 14;
            this.showAllCheckBox.Text = "Show All";
            this.showAllCheckBox.UseVisualStyleBackColor = true;
            this.showAllCheckBox.CheckedChanged += new System.EventHandler( this.showAllCheckBox_CheckedChanged );
            // 
            // loadNamesButton
            // 
            this.loadNamesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadNamesButton.Location = new System.Drawing.Point( 61, 331 );
            this.loadNamesButton.Name = "loadNamesButton";
            this.loadNamesButton.Size = new System.Drawing.Size( 123, 23 );
            this.loadNamesButton.TabIndex = 13;
            this.loadNamesButton.Text = "Load Names List...";
            this.loadNamesButton.UseVisualStyleBackColor = true;
            this.loadNamesButton.Click += new System.EventHandler( this.loadNamesButton_Click );
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label3.Location = new System.Drawing.Point( 13, 38 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 106, 13 );
            this.label3.TabIndex = 12;
            this.label3.Text = "MarketSim Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 13, 79 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 139, 13 );
            this.label1.TabIndex = 11;
            this.label1.Text = "Available MarketSim Names";
            // 
            // marketSimNameTextBox
            // 
            this.marketSimNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.marketSimNameTextBox.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.marketSimNameTextBox.Location = new System.Drawing.Point( 125, 35 );
            this.marketSimNameTextBox.Name = "marketSimNameTextBox";
            this.marketSimNameTextBox.Size = new System.Drawing.Size( 221, 20 );
            this.marketSimNameTextBox.TabIndex = 10;
            this.marketSimNameTextBox.TextChanged += new System.EventHandler( this.marketSimNameTextBox_TextChanged );
            // 
            // outputListBox
            // 
            this.outputListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outputListBox.FormattingEnabled = true;
            this.outputListBox.Location = new System.Drawing.Point( 16, 100 );
            this.outputListBox.Name = "outputListBox";
            this.outputListBox.Size = new System.Drawing.Size( 330, 225 );
            this.outputListBox.TabIndex = 9;
            this.outputListBox.SelectedIndexChanged += new System.EventHandler( this.outputListBox_SelectedIndexChanged );
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point( 275, 6 );
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size( 81, 23 );
            this.addButton.TabIndex = 17;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler( this.addButton_Click );
            // 
            // VariantNamingAidForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 741, 412 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.splitContainer1 );
            this.Name = "VariantNamingAidForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Variant Naming Tool";
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox inputListBox;
        private System.Windows.Forms.Button loadNamesButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox marketSimNameTextBox;
        private System.Windows.Forms.ListBox outputListBox;
        private System.Windows.Forms.CheckBox showAllCheckBox;
        private System.Windows.Forms.Button autoMatchButton;
        private System.Windows.Forms.Button addButton;
    }
}