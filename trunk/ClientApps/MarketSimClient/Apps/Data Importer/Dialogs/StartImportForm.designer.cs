namespace DataImporter.Dialogs
{
    partial class StartImportForm
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
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tolTextBox = new System.Windows.Forms.TextBox();
            this.channelsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.typesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.scaleNumTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.scaleDenTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 12, 9 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 120, 14 );
            this.label1.TabIndex = 0;
            this.label1.Text = "Ready to Import Data";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 27, 39 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 117, 14 );
            this.label2.TabIndex = 1;
            this.label2.Text = "Import data on or after:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point( 144, 36 );
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size( 87, 20 );
            this.dateTimePicker1.TabIndex = 2;
            this.dateTimePicker1.Value = new System.DateTime( 2000, 1, 1, 0, 0, 0, 0 );
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 331, 267 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 412, 267 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 27, 71 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 124, 14 );
            this.label4.TabIndex = 6;
            this.label4.Text = "Compression Tolerance:";
            // 
            // tolTextBox
            // 
            this.tolTextBox.Location = new System.Drawing.Point( 151, 68 );
            this.tolTextBox.Name = "tolTextBox";
            this.tolTextBox.Size = new System.Drawing.Size( 53, 20 );
            this.tolTextBox.TabIndex = 7;
            this.tolTextBox.Text = "0.01";
            this.tolTextBox.TextChanged += new System.EventHandler( this.tolTextBox_TextChanged );
            // 
            // channelsCheckedListBox
            // 
            this.channelsCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.channelsCheckedListBox.CheckOnClick = true;
            this.channelsCheckedListBox.FormattingEnabled = true;
            this.channelsCheckedListBox.Location = new System.Drawing.Point( 15, 113 );
            this.channelsCheckedListBox.Name = "channelsCheckedListBox";
            this.channelsCheckedListBox.Size = new System.Drawing.Size( 290, 139 );
            this.channelsCheckedListBox.TabIndex = 8;
            // 
            // typesCheckedListBox
            // 
            this.typesCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.typesCheckedListBox.CheckOnClick = true;
            this.typesCheckedListBox.FormattingEnabled = true;
            this.typesCheckedListBox.Items.AddRange( new object[] {
            "Coupons",
            "Display",
            "Distribution",
            "Media",
            "PriceRegular",
            "PricePromo",
            "PromoPricePct",
            "RealSales"} );
            this.typesCheckedListBox.Location = new System.Drawing.Point( 322, 113 );
            this.typesCheckedListBox.Name = "typesCheckedListBox";
            this.typesCheckedListBox.Size = new System.Drawing.Size( 150, 139 );
            this.typesCheckedListBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 18, 96 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 52, 14 );
            this.label3.TabIndex = 10;
            this.label3.Text = "Channels";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 328, 96 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 62, 14 );
            this.label5.TabIndex = 11;
            this.label5.Text = "Data Types";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 236, 39 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 44, 14 );
            this.label6.TabIndex = 12;
            this.label6.Text = "through";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point( 283, 36 );
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size( 87, 20 );
            this.dateTimePicker2.TabIndex = 13;
            this.dateTimePicker2.Value = new System.DateTime( 2010, 1, 1, 0, 0, 0, 0 );
            // 
            // scaleNumTextBox
            // 
            this.scaleNumTextBox.Location = new System.Drawing.Point( 308, 69 );
            this.scaleNumTextBox.Name = "scaleNumTextBox";
            this.scaleNumTextBox.Size = new System.Drawing.Size( 41, 20 );
            this.scaleNumTextBox.TabIndex = 15;
            this.scaleNumTextBox.Text = "1";
            this.scaleNumTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 225, 72 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 85, 14 );
            this.label7.TabIndex = 14;
            this.label7.Text = "Scale Prices by:";
            // 
            // scaleDenTextBox
            // 
            this.scaleDenTextBox.Location = new System.Drawing.Point( 362, 69 );
            this.scaleDenTextBox.Name = "scaleDenTextBox";
            this.scaleDenTextBox.Size = new System.Drawing.Size( 44, 20 );
            this.scaleDenTextBox.TabIndex = 16;
            this.scaleDenTextBox.Text = "1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point( 351, 72 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 10, 14 );
            this.label8.TabIndex = 17;
            this.label8.Text = "/";
            // 
            // StartImportForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 14F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 497, 298 );
            this.Controls.Add( this.label8 );
            this.Controls.Add( this.scaleDenTextBox );
            this.Controls.Add( this.scaleNumTextBox );
            this.Controls.Add( this.label7 );
            this.Controls.Add( this.dateTimePicker2 );
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.typesCheckedListBox );
            this.Controls.Add( this.channelsCheckedListBox );
            this.Controls.Add( this.tolTextBox );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.dateTimePicker1 );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.Name = "StartImportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Start Import";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tolTextBox;
        private System.Windows.Forms.CheckedListBox channelsCheckedListBox;
        private System.Windows.Forms.CheckedListBox typesCheckedListBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.TextBox scaleNumTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox scaleDenTextBox;
        private System.Windows.Forms.Label label8;
    }
}