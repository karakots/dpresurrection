namespace MrktSimClient.Controls.Dialogs
{
    partial class TransferScenarioPlanValuesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( TransferScenarioPlanValuesForm ) );
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.scenarioComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.startDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.endDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.columnsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.prodLevelComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 12, 9 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 555, 39 );
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString( "label1.Text" );
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 12, 68 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 86, 13 );
            this.label2.TabIndex = 1;
            this.label2.Text = "Source Scenario";
            // 
            // scenarioComboBox
            // 
            this.scenarioComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scenarioComboBox.FormattingEnabled = true;
            this.scenarioComboBox.Location = new System.Drawing.Point( 104, 65 );
            this.scenarioComboBox.Name = "scenarioComboBox";
            this.scenarioComboBox.Size = new System.Drawing.Size( 469, 21 );
            this.scenarioComboBox.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 12, 106 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 88, 13 );
            this.label3.TabIndex = 3;
            this.label3.Text = "Component Type";
            // 
            // typeComboBox
            // 
            this.typeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Items.AddRange( new object[] {
            "Display",
            "Distribution",
            "Media"} );
            this.typeComboBox.Location = new System.Drawing.Point( 106, 103 );
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size( 171, 21 );
            this.typeComboBox.TabIndex = 4;
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDateTimePicker.Location = new System.Drawing.Point( 338, 106 );
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new System.Drawing.Size( 94, 20 );
            this.startDateTimePicker.TabIndex = 12;
            this.startDateTimePicker.Value = new System.DateTime( 2006, 1, 1, 0, 0, 0, 0 );
            // 
            // endDateTimePicker
            // 
            this.endDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.endDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endDateTimePicker.Location = new System.Drawing.Point( 479, 106 );
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.Size = new System.Drawing.Size( 94, 20 );
            this.endDateTimePicker.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 447, 110 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 26, 13 );
            this.label4.TabIndex = 13;
            this.label4.Text = "End";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 303, 109 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 29, 13 );
            this.label5.TabIndex = 14;
            this.label5.Text = "Start";
            // 
            // columnsCheckedListBox
            // 
            this.columnsCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.columnsCheckedListBox.FormattingEnabled = true;
            this.columnsCheckedListBox.Location = new System.Drawing.Point( 15, 157 );
            this.columnsCheckedListBox.Name = "columnsCheckedListBox";
            this.columnsCheckedListBox.ScrollAlwaysVisible = true;
            this.columnsCheckedListBox.Size = new System.Drawing.Size( 262, 94 );
            this.columnsCheckedListBox.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 12, 141 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 127, 13 );
            this.label6.TabIndex = 16;
            this.label6.Text = "Data Columns to Transfer";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 433, 248 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 17;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 514, 248 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 18;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 303, 157 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 80, 13 );
            this.label7.TabIndex = 19;
            this.label7.Text = "Matching Level";
            // 
            // prodLevelComboBox
            // 
            this.prodLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.prodLevelComboBox.FormattingEnabled = true;
            this.prodLevelComboBox.Location = new System.Drawing.Point( 395, 154 );
            this.prodLevelComboBox.Name = "prodLevelComboBox";
            this.prodLevelComboBox.Size = new System.Drawing.Size( 178, 21 );
            this.prodLevelComboBox.TabIndex = 20;
            // 
            // TransferScenarioPlanValuesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 597, 278 );
            this.Controls.Add( this.prodLevelComboBox );
            this.Controls.Add( this.label7 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.columnsCheckedListBox );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.startDateTimePicker );
            this.Controls.Add( this.endDateTimePicker );
            this.Controls.Add( this.typeComboBox );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.scenarioComboBox );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransferScenarioPlanValuesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transfer Scenario Plan Values";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox scenarioComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.DateTimePicker startDateTimePicker;
        private System.Windows.Forms.DateTimePicker endDateTimePicker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox columnsCheckedListBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox prodLevelComboBox;
    }
}