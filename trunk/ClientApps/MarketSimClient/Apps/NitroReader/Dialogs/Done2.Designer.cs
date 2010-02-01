namespace NitroReader.Dialogs
{
    partial class Done2
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
            this.label6 = new System.Windows.Forms.Label();
            this.planNameLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.planPathLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.variantCountLabel = new System.Windows.Forms.Label();
            this.warningsButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.showReportCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 43, 81 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 76, 13 );
            this.label6.TabIndex = 14;
            this.label6.Text = "Main Plan File:";
            // 
            // planNameLabel
            // 
            this.planNameLabel.AutoSize = true;
            this.planNameLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.planNameLabel.Location = new System.Drawing.Point( 153, 54 );
            this.planNameLabel.Name = "planNameLabel";
            this.planNameLabel.Size = new System.Drawing.Size( 115, 13 );
            this.planNameLabel.TabIndex = 13;
            this.planNameLabel.Text = " Market Plan Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 43, 131 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 104, 13 );
            this.label4.TabIndex = 12;
            this.label4.Text = "Variants Processed: ";
            // 
            // planPathLabel
            // 
            this.planPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.planPathLabel.Location = new System.Drawing.Point( 119, 81 );
            this.planPathLabel.Name = "planPathLabel";
            this.planPathLabel.Size = new System.Drawing.Size( 353, 45 );
            this.planPathLabel.TabIndex = 11;
            this.planPathLabel.Text = " C:\\Progam Files\\DececisionPower\\MrktSimData\\plan-file.mpl";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 43, 54 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 110, 13 );
            this.label2.TabIndex = 10;
            this.label2.Text = "Market Plan Created: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point( 43, 28 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 285, 13 );
            this.label1.TabIndex = 9;
            this.label1.Text = "NITRO Data Processing Completed Successfully!";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point( 201, 167 );
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size( 105, 23 );
            this.button1.TabIndex = 8;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // variantCountLabel
            // 
            this.variantCountLabel.AutoSize = true;
            this.variantCountLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.variantCountLabel.Location = new System.Drawing.Point( 153, 131 );
            this.variantCountLabel.Name = "variantCountLabel";
            this.variantCountLabel.Size = new System.Drawing.Size( 14, 13 );
            this.variantCountLabel.TabIndex = 15;
            this.variantCountLabel.Text = "9";
            // 
            // warningsButton
            // 
            this.warningsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.warningsButton.BackColor = System.Drawing.Color.Yellow;
            this.warningsButton.Font = new System.Drawing.Font( "Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.warningsButton.Location = new System.Drawing.Point( 43, 169 );
            this.warningsButton.Name = "warningsButton";
            this.warningsButton.Size = new System.Drawing.Size( 104, 19 );
            this.warningsButton.TabIndex = 16;
            this.warningsButton.Text = "Show Warnings";
            this.warningsButton.UseVisualStyleBackColor = false;
            this.warningsButton.Visible = false;
            this.warningsButton.Click += new System.EventHandler( this.warningsButton_Click );
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 224, 131 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 91, 13 );
            this.label3.TabIndex = 17;
            this.label3.Text = "Processing Time: ";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point( 312, 131 );
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size( 28, 13 );
            this.timeLabel.TabIndex = 18;
            this.timeLabel.Text = "2:30";
            // 
            // showReportCheckBox
            // 
            this.showReportCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showReportCheckBox.AutoSize = true;
            this.showReportCheckBox.Location = new System.Drawing.Point( 322, 171 );
            this.showReportCheckBox.Name = "showReportCheckBox";
            this.showReportCheckBox.Size = new System.Drawing.Size( 107, 17 );
            this.showReportCheckBox.TabIndex = 19;
            this.showReportCheckBox.Text = "Show Report File";
            this.showReportCheckBox.UseVisualStyleBackColor = true;
            this.showReportCheckBox.CheckedChanged += new System.EventHandler( this.showReportCheckBox_CheckedChanged );
            // 
            // Done2
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 507, 202 );
            this.Controls.Add( this.showReportCheckBox );
            this.Controls.Add( this.timeLabel );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.warningsButton );
            this.Controls.Add( this.variantCountLabel );
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.planNameLabel );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.planPathLabel );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.button1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Done2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NITRO Import Complete";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label planNameLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label planPathLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label variantCountLabel;
        private System.Windows.Forms.Button warningsButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.CheckBox showReportCheckBox;
    }
}