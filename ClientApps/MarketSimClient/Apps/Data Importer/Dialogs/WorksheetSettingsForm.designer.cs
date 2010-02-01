namespace DataImporter.Dialogs
{
    partial class WorksheetSettingsForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.scanAllDatesCheckBox = new System.Windows.Forms.CheckBox();
            this.chanHeadingLabel = new System.Windows.Forms.Label();
            this.chanLabel3 = new System.Windows.Forms.Label();
            this.chanColTextBox = new System.Windows.Forms.TextBox();
            this.chanRowTextBox = new System.Windows.Forms.TextBox();
            this.chanLabel2 = new System.Windows.Forms.Label();
            this.chanLabel1 = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.timeStepLabel = new System.Windows.Forms.Label();
            this.startEndComboBox = new System.Windows.Forms.ComboBox();
            this.hvComboBox = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.sheetNameLabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.skuHeadingLabel = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.skuColTextBox = new System.Windows.Forms.TextBox();
            this.skuRowTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.timeStepLabelLabel = new System.Windows.Forms.Label();
            this.dateHeadingLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dhColTextBox = new System.Windows.Forms.TextBox();
            this.dhRowTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.setStepTypeComboBox = new System.Windows.Forms.ComboBox();
            this.setStepCountTextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 476, 241 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 557, 241 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add( this.setStepCountTextBox );
            this.panel1.Controls.Add( this.setStepTypeComboBox );
            this.panel1.Controls.Add( this.scanAllDatesCheckBox );
            this.panel1.Controls.Add( this.chanHeadingLabel );
            this.panel1.Controls.Add( this.chanLabel3 );
            this.panel1.Controls.Add( this.chanColTextBox );
            this.panel1.Controls.Add( this.chanRowTextBox );
            this.panel1.Controls.Add( this.chanLabel2 );
            this.panel1.Controls.Add( this.chanLabel1 );
            this.panel1.Controls.Add( this.errorLabel );
            this.panel1.Controls.Add( this.timeStepLabel );
            this.panel1.Controls.Add( this.startEndComboBox );
            this.panel1.Controls.Add( this.hvComboBox );
            this.panel1.Controls.Add( this.label16 );
            this.panel1.Controls.Add( this.label15 );
            this.panel1.Controls.Add( this.sheetNameLabel );
            this.panel1.Controls.Add( this.label11 );
            this.panel1.Controls.Add( this.skuHeadingLabel );
            this.panel1.Controls.Add( this.label12 );
            this.panel1.Controls.Add( this.skuColTextBox );
            this.panel1.Controls.Add( this.skuRowTextBox );
            this.panel1.Controls.Add( this.label13 );
            this.panel1.Controls.Add( this.label10 );
            this.panel1.Controls.Add( this.timeStepLabelLabel );
            this.panel1.Controls.Add( this.dateHeadingLabel );
            this.panel1.Controls.Add( this.label8 );
            this.panel1.Controls.Add( this.dhColTextBox );
            this.panel1.Controls.Add( this.dhRowTextBox );
            this.panel1.Controls.Add( this.label7 );
            this.panel1.Controls.Add( this.label6 );
            this.panel1.Controls.Add( this.label5 );
            this.panel1.Controls.Add( this.label3 );
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 644, 233 );
            this.panel1.TabIndex = 11;
            // 
            // scanAllDatesCheckBox
            // 
            this.scanAllDatesCheckBox.AutoSize = true;
            this.scanAllDatesCheckBox.Location = new System.Drawing.Point( 403, 95 );
            this.scanAllDatesCheckBox.Name = "scanAllDatesCheckBox";
            this.scanAllDatesCheckBox.Size = new System.Drawing.Size( 141, 18 );
            this.scanAllDatesCheckBox.TabIndex = 12;
            this.scanAllDatesCheckBox.Text = "Scan ALL date headers";
            this.scanAllDatesCheckBox.UseVisualStyleBackColor = true;
            this.scanAllDatesCheckBox.CheckedChanged += new System.EventHandler( this.scanAllDatesCheckBox_CheckedChanged );
            // 
            // chanHeadingLabel
            // 
            this.chanHeadingLabel.AutoSize = true;
            this.chanHeadingLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.chanHeadingLabel.Location = new System.Drawing.Point( 328, 203 );
            this.chanHeadingLabel.Name = "chanHeadingLabel";
            this.chanHeadingLabel.Size = new System.Drawing.Size( 110, 14 );
            this.chanHeadingLabel.TabIndex = 56;
            this.chanHeadingLabel.Text = "WEEK ENDING 9/9/99";
            // 
            // chanLabel3
            // 
            this.chanLabel3.AutoSize = true;
            this.chanLabel3.Location = new System.Drawing.Point( 276, 203 );
            this.chanLabel3.Name = "chanLabel3";
            this.chanLabel3.Size = new System.Drawing.Size( 56, 14 );
            this.chanLabel3.TabIndex = 55;
            this.chanLabel3.Text = "Cell value:";
            // 
            // chanColTextBox
            // 
            this.chanColTextBox.Location = new System.Drawing.Point( 242, 199 );
            this.chanColTextBox.Name = "chanColTextBox";
            this.chanColTextBox.Size = new System.Drawing.Size( 20, 20 );
            this.chanColTextBox.TabIndex = 54;
            this.chanColTextBox.Text = "19";
            this.chanColTextBox.TextChanged += new System.EventHandler( this.chanRowColTextBox_TextChanged );
            // 
            // chanRowTextBox
            // 
            this.chanRowTextBox.Location = new System.Drawing.Point( 172, 199 );
            this.chanRowTextBox.Name = "chanRowTextBox";
            this.chanRowTextBox.Size = new System.Drawing.Size( 20, 20 );
            this.chanRowTextBox.TabIndex = 53;
            this.chanRowTextBox.Text = "19";
            this.chanRowTextBox.TextChanged += new System.EventHandler( this.chanRowColTextBox_TextChanged );
            // 
            // chanLabel2
            // 
            this.chanLabel2.AutoSize = true;
            this.chanLabel2.Location = new System.Drawing.Point( 191, 203 );
            this.chanLabel2.Name = "chanLabel2";
            this.chanLabel2.Size = new System.Drawing.Size( 47, 14 );
            this.chanLabel2.TabIndex = 52;
            this.chanLabel2.Text = ", column";
            // 
            // chanLabel1
            // 
            this.chanLabel1.AutoSize = true;
            this.chanLabel1.Location = new System.Drawing.Point( 22, 203 );
            this.chanLabel1.Name = "chanLabel1";
            this.chanLabel1.Size = new System.Drawing.Size( 146, 14 );
            this.chanLabel1.TabIndex = 51;
            this.chanLabel1.Text = "First Channel Heading in row";
            // 
            // errorLabel
            // 
            this.errorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.errorLabel.AutoSize = true;
            this.errorLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point( 483, 10 );
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size( 121, 15 );
            this.errorLabel.TabIndex = 50;
            this.errorLabel.Text = "Error: Format Invalid";
            this.errorLabel.Visible = false;
            // 
            // timeStepLabel
            // 
            this.timeStepLabel.AutoSize = true;
            this.timeStepLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.timeStepLabel.Location = new System.Drawing.Point( 283, 98 );
            this.timeStepLabel.Name = "timeStepLabel";
            this.timeStepLabel.Size = new System.Drawing.Size( 14, 14 );
            this.timeStepLabel.TabIndex = 49;
            this.timeStepLabel.Text = "?";
            // 
            // startEndComboBox
            // 
            this.startEndComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.startEndComboBox.FormattingEnabled = true;
            this.startEndComboBox.Items.AddRange( new object[] {
            "start",
            "end"} );
            this.startEndComboBox.Location = new System.Drawing.Point( 142, 125 );
            this.startEndComboBox.Name = "startEndComboBox";
            this.startEndComboBox.Size = new System.Drawing.Size( 56, 22 );
            this.startEndComboBox.TabIndex = 48;
            // 
            // hvComboBox
            // 
            this.hvComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hvComboBox.FormattingEnabled = true;
            this.hvComboBox.Items.AddRange( new object[] {
            "Horizontal",
            "Vertical"} );
            this.hvComboBox.Location = new System.Drawing.Point( 117, 93 );
            this.hvComboBox.Name = "hvComboBox";
            this.hvComboBox.Size = new System.Drawing.Size( 88, 22 );
            this.hvComboBox.TabIndex = 47;
            this.hvComboBox.SelectedIndexChanged += new System.EventHandler( this.dateRowColTextBox_TextChanged );
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point( 204, 130 );
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size( 99, 14 );
            this.label16.TabIndex = 46;
            this.label16.Text = "on specified dates.";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point( 23, 130 );
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size( 116, 14 );
            this.label15.TabIndex = 45;
            this.label15.Text = "Measurement intervals";
            // 
            // sheetNameLabel
            // 
            this.sheetNameLabel.AutoSize = true;
            this.sheetNameLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.sheetNameLabel.Location = new System.Drawing.Point( 97, 39 );
            this.sheetNameLabel.Name = "sheetNameLabel";
            this.sheetNameLabel.Size = new System.Drawing.Size( 116, 14 );
            this.sheetNameLabel.TabIndex = 41;
            this.sheetNameLabel.Text = "%ACV DISTRIBUTION";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point( 22, 39 );
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size( 62, 14 );
            this.label11.TabIndex = 40;
            this.label11.Text = "Worksheet:";
            // 
            // skuHeadingLabel
            // 
            this.skuHeadingLabel.AutoSize = true;
            this.skuHeadingLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.skuHeadingLabel.Location = new System.Drawing.Point( 349, 173 );
            this.skuHeadingLabel.Name = "skuHeadingLabel";
            this.skuHeadingLabel.Size = new System.Drawing.Size( 110, 14 );
            this.skuHeadingLabel.TabIndex = 38;
            this.skuHeadingLabel.Text = "WEEK ENDING 9/9/99";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point( 297, 173 );
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size( 56, 14 );
            this.label12.TabIndex = 37;
            this.label12.Text = "Cell value:";
            // 
            // skuColTextBox
            // 
            this.skuColTextBox.Location = new System.Drawing.Point( 263, 169 );
            this.skuColTextBox.Name = "skuColTextBox";
            this.skuColTextBox.Size = new System.Drawing.Size( 20, 20 );
            this.skuColTextBox.TabIndex = 36;
            this.skuColTextBox.Text = "19";
            this.skuColTextBox.TextChanged += new System.EventHandler( this.skuRowColTextBox_TextChanged );
            // 
            // skuRowTextBox
            // 
            this.skuRowTextBox.Location = new System.Drawing.Point( 193, 169 );
            this.skuRowTextBox.Name = "skuRowTextBox";
            this.skuRowTextBox.Size = new System.Drawing.Size( 20, 20 );
            this.skuRowTextBox.TabIndex = 35;
            this.skuRowTextBox.Text = "19";
            this.skuRowTextBox.TextChanged += new System.EventHandler( this.skuRowColTextBox_TextChanged );
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point( 212, 173 );
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size( 47, 14 );
            this.label13.TabIndex = 34;
            this.label13.Text = ", column";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point( 22, 173 );
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size( 170, 14 );
            this.label10.TabIndex = 33;
            this.label10.Text = "First SKU / Group  Heading in row";
            // 
            // timeStepLabelLabel
            // 
            this.timeStepLabelLabel.AutoSize = true;
            this.timeStepLabelLabel.Location = new System.Drawing.Point( 228, 98 );
            this.timeStepLabelLabel.Name = "timeStepLabelLabel";
            this.timeStepLabelLabel.Size = new System.Drawing.Size( 56, 14 );
            this.timeStepLabelLabel.TabIndex = 28;
            this.timeStepLabelLabel.Text = "Time step:";
            // 
            // dateHeadingLabel
            // 
            this.dateHeadingLabel.AutoSize = true;
            this.dateHeadingLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.dateHeadingLabel.Location = new System.Drawing.Point( 310, 68 );
            this.dateHeadingLabel.Name = "dateHeadingLabel";
            this.dateHeadingLabel.Size = new System.Drawing.Size( 110, 14 );
            this.dateHeadingLabel.TabIndex = 27;
            this.dateHeadingLabel.Text = "WEEK ENDING 9/9/99";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point( 257, 68 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 56, 14 );
            this.label8.TabIndex = 26;
            this.label8.Text = "Cell value:";
            // 
            // dhColTextBox
            // 
            this.dhColTextBox.Location = new System.Drawing.Point( 222, 64 );
            this.dhColTextBox.Name = "dhColTextBox";
            this.dhColTextBox.Size = new System.Drawing.Size( 20, 20 );
            this.dhColTextBox.TabIndex = 25;
            this.dhColTextBox.Text = "19";
            this.dhColTextBox.TextChanged += new System.EventHandler( this.dateRowColTextBox_TextChanged );
            // 
            // dhRowTextBox
            // 
            this.dhRowTextBox.Location = new System.Drawing.Point( 152, 64 );
            this.dhRowTextBox.Name = "dhRowTextBox";
            this.dhRowTextBox.Size = new System.Drawing.Size( 20, 20 );
            this.dhRowTextBox.TabIndex = 24;
            this.dhRowTextBox.Text = "19";
            this.dhRowTextBox.TextChanged += new System.EventHandler( this.dateRowColTextBox_TextChanged );
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 172, 68 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 47, 14 );
            this.label7.TabIndex = 23;
            this.label7.Text = ", column";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 22, 68 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 129, 14 );
            this.label6.TabIndex = 22;
            this.label6.Text = "First Date Heading in row";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 22, 98 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 93, 14 );
            this.label5.TabIndex = 19;
            this.label5.Text = "Time Progression:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label3.Location = new System.Drawing.Point( 12, 9 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 164, 15 );
            this.label3.TabIndex = 13;
            this.label3.Text = "Set Worksheet Data Format";
            // 
            // setStepTypeComboBox
            // 
            this.setStepTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.setStepTypeComboBox.FormattingEnabled = true;
            this.setStepTypeComboBox.Items.AddRange( new object[] {
            "days",
            "month(s)"} );
            this.setStepTypeComboBox.Location = new System.Drawing.Point( 324, 93 );
            this.setStepTypeComboBox.Name = "setStepTypeComboBox";
            this.setStepTypeComboBox.Size = new System.Drawing.Size( 73, 22 );
            this.setStepTypeComboBox.TabIndex = 57;
            this.setStepTypeComboBox.Visible = false;
            this.setStepTypeComboBox.SelectedIndexChanged += new System.EventHandler( this.setStepCountTextBox_TextChanged );
            // 
            // setStepCountTextBox
            // 
            this.setStepCountTextBox.Location = new System.Drawing.Point( 296, 95 );
            this.setStepCountTextBox.Name = "setStepCountTextBox";
            this.setStepCountTextBox.Size = new System.Drawing.Size( 20, 20 );
            this.setStepCountTextBox.TabIndex = 58;
            this.setStepCountTextBox.Text = "19";
            this.setStepCountTextBox.Visible = false;
            this.setStepCountTextBox.TextChanged += new System.EventHandler( this.setStepCountTextBox_TextChanged );
            // 
            // WorksheetSettingsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 14F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 643, 270 );
            this.Controls.Add( this.panel1 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WorksheetSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scan Worksheet Format";
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label dateHeadingLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox dhColTextBox;
        private System.Windows.Forms.TextBox dhRowTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label timeStepLabelLabel;
        private System.Windows.Forms.Label skuHeadingLabel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox skuColTextBox;
        private System.Windows.Forms.TextBox skuRowTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label sheetNameLabel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox hvComboBox;
        private System.Windows.Forms.ComboBox startEndComboBox;
        private System.Windows.Forms.Label timeStepLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label chanHeadingLabel;
        private System.Windows.Forms.Label chanLabel3;
        private System.Windows.Forms.TextBox chanColTextBox;
        private System.Windows.Forms.TextBox chanRowTextBox;
        private System.Windows.Forms.Label chanLabel2;
        private System.Windows.Forms.Label chanLabel1;
        private System.Windows.Forms.CheckBox scanAllDatesCheckBox;
        private System.Windows.Forms.TextBox setStepCountTextBox;
        private System.Windows.Forms.ComboBox setStepTypeComboBox;
    }
}