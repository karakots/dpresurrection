namespace MrktSimClient.Controls.Dialogs
{
    partial class ExtendScenarioForm
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
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.endDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.patternLengthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.finalValuesRadioButton = new System.Windows.Forms.RadioButton();
            this.repeatPatternRadioButton = new System.Windows.Forms.RadioButton();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.startDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.prodLevelComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.patternLengthNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 31, 25 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 133, 13 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Plan Type to be Extended:";
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Items.AddRange( new object[] {
            "Display",
            "Distribution",
            "Media",
            "Price",
            "Real Sales"} );
            this.typeComboBox.Location = new System.Drawing.Point( 170, 22 );
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size( 134, 21 );
            this.typeComboBox.TabIndex = 1;
            // 
            // endDateTimePicker
            // 
            this.endDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endDateTimePicker.Location = new System.Drawing.Point( 188, 89 );
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.Size = new System.Drawing.Size( 94, 20 );
            this.endDateTimePicker.TabIndex = 2;
            // 
            // patternLengthNumericUpDown
            // 
            this.patternLengthNumericUpDown.Location = new System.Drawing.Point( 293, 162 );
            this.patternLengthNumericUpDown.Maximum = new decimal( new int[] {
            1000,
            0,
            0,
            0} );
            this.patternLengthNumericUpDown.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.patternLengthNumericUpDown.Name = "patternLengthNumericUpDown";
            this.patternLengthNumericUpDown.Size = new System.Drawing.Size( 43, 20 );
            this.patternLengthNumericUpDown.TabIndex = 3;
            this.patternLengthNumericUpDown.Value = new decimal( new int[] {
            12,
            0,
            0,
            0} );
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 31, 93 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 146, 13 );
            this.label2.TabIndex = 4;
            this.label2.Text = "End date for Extenstion Plans";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 342, 164 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 41, 13 );
            this.label3.TabIndex = 5;
            this.label3.Text = "months";
            // 
            // finalValuesRadioButton
            // 
            this.finalValuesRadioButton.AutoSize = true;
            this.finalValuesRadioButton.Location = new System.Drawing.Point( 34, 127 );
            this.finalValuesRadioButton.Name = "finalValuesRadioButton";
            this.finalValuesRadioButton.Size = new System.Drawing.Size( 228, 17 );
            this.finalValuesRadioButton.TabIndex = 6;
            this.finalValuesRadioButton.Text = "Extend (constant values)  from Final Values";
            this.finalValuesRadioButton.UseVisualStyleBackColor = true;
            // 
            // repeatPatternRadioButton
            // 
            this.repeatPatternRadioButton.AutoSize = true;
            this.repeatPatternRadioButton.Checked = true;
            this.repeatPatternRadioButton.Location = new System.Drawing.Point( 34, 162 );
            this.repeatPatternRadioButton.Name = "repeatPatternRadioButton";
            this.repeatPatternRadioButton.Size = new System.Drawing.Size( 259, 17 );
            this.repeatPatternRadioButton.TabIndex = 7;
            this.repeatPatternRadioButton.TabStop = true;
            this.repeatPatternRadioButton.Text = "Replicate Historic Data Pattern.    Pattern Length:";
            this.repeatPatternRadioButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 292, 254 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 373, 254 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 31, 61 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 149, 13 );
            this.label4.TabIndex = 11;
            this.label4.Text = "Start date for Extenstion Plans";
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDateTimePicker.Location = new System.Drawing.Point( 188, 57 );
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new System.Drawing.Size( 94, 20 );
            this.startDateTimePicker.TabIndex = 10;
            this.startDateTimePicker.Value = new System.DateTime( 2006, 1, 1, 0, 0, 0, 0 );
            // 
            // prodLevelComboBox
            // 
            this.prodLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.prodLevelComboBox.FormattingEnabled = true;
            this.prodLevelComboBox.Location = new System.Drawing.Point( 123, 203 );
            this.prodLevelComboBox.Name = "prodLevelComboBox";
            this.prodLevelComboBox.Size = new System.Drawing.Size( 178, 21 );
            this.prodLevelComboBox.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 31, 206 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 80, 13 );
            this.label7.TabIndex = 23;
            this.label7.Text = "Matching Level";
            // 
            // ExtendScenarioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 456, 285 );
            this.Controls.Add( this.patternLengthNumericUpDown );
            this.Controls.Add( this.prodLevelComboBox );
            this.Controls.Add( this.label7 );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.startDateTimePicker );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.repeatPatternRadioButton );
            this.Controls.Add( this.finalValuesRadioButton );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.endDateTimePicker );
            this.Controls.Add( this.typeComboBox );
            this.Controls.Add( this.label1 );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtendScenarioForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Extend Scenario Plans";
            ((System.ComponentModel.ISupportInitialize)(this.patternLengthNumericUpDown)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.DateTimePicker endDateTimePicker;
        private System.Windows.Forms.NumericUpDown patternLengthNumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton finalValuesRadioButton;
        private System.Windows.Forms.RadioButton repeatPatternRadioButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker startDateTimePicker;
        private System.Windows.Forms.ComboBox prodLevelComboBox;
        private System.Windows.Forms.Label label7;
    }
}