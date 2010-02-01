namespace NitroReader.Dialogs
{
    partial class GeneralSettings
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
            this.versionLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.selectButton = new System.Windows.Forms.Button();
            this.iconFileTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.clearButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.distributionPersuasionTextBox = new System.Windows.Forms.TextBox();
            this.distributionAwarenessTextBox = new System.Windows.Forms.TextBox();
            this.displayPersuasionTextBox = new System.Windows.Forms.TextBox();
            this.displayAwarenessTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 17, 20 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 267, 13 );
            this.label1.TabIndex = 0;
            this.label1.Text = "NITRO Data Reader Program for MarketSim and NIMO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 32, 72 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 84, 13 );
            this.label2.TabIndex = 1;
            this.label2.Text = "Program Version";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point( 121, 72 );
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size( 40, 13 );
            this.versionLabel.TabIndex = 2;
            this.versionLabel.Text = "1.1.1.1";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 209, 297 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 299, 297 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 19, 45 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 289, 13 );
            this.label3.TabIndex = 11;
            this.label3.Text = "(c) 2007 DecisionPower Inc.          www.decisionpower.com";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.selectButton );
            this.groupBox1.Controls.Add( this.iconFileTextBox );
            this.groupBox1.Controls.Add( this.label6 );
            this.groupBox1.Controls.Add( this.clearButton );
            this.groupBox1.Location = new System.Drawing.Point( 12, 219 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 553, 67 );
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Customer Icon";
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point( 376, 25 );
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size( 75, 23 );
            this.selectButton.TabIndex = 15;
            this.selectButton.Text = "Browse...";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler( this.selectButton_Click );
            // 
            // iconFileTextBox
            // 
            this.iconFileTextBox.Location = new System.Drawing.Point( 48, 27 );
            this.iconFileTextBox.Name = "iconFileTextBox";
            this.iconFileTextBox.Size = new System.Drawing.Size( 322, 20 );
            this.iconFileTextBox.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 19, 30 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 23, 13 );
            this.label6.TabIndex = 13;
            this.label6.Text = "File";
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point( 457, 24 );
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size( 75, 23 );
            this.clearButton.TabIndex = 0;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler( this.clearButton_Click );
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add( this.label8 );
            this.groupBox2.Controls.Add( this.label7 );
            this.groupBox2.Controls.Add( this.distributionPersuasionTextBox );
            this.groupBox2.Controls.Add( this.distributionAwarenessTextBox );
            this.groupBox2.Controls.Add( this.displayPersuasionTextBox );
            this.groupBox2.Controls.Add( this.displayAwarenessTextBox );
            this.groupBox2.Controls.Add( this.label5 );
            this.groupBox2.Controls.Add( this.label4 );
            this.groupBox2.Location = new System.Drawing.Point( 12, 108 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size( 553, 100 );
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Simulation Parameters";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point( 160, 23 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 59, 13 );
            this.label8.TabIndex = 17;
            this.label8.Text = "Distribution";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point( 95, 23 );
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size( 41, 13 );
            this.label7.TabIndex = 16;
            this.label7.Text = "Display";
            // 
            // distributionPersuasionTextBox
            // 
            this.distributionPersuasionTextBox.Location = new System.Drawing.Point( 164, 68 );
            this.distributionPersuasionTextBox.Name = "distributionPersuasionTextBox";
            this.distributionPersuasionTextBox.Size = new System.Drawing.Size( 51, 20 );
            this.distributionPersuasionTextBox.TabIndex = 15;
            this.distributionPersuasionTextBox.Text = "0.20";
            // 
            // distributionAwarenessTextBox
            // 
            this.distributionAwarenessTextBox.Location = new System.Drawing.Point( 164, 42 );
            this.distributionAwarenessTextBox.Name = "distributionAwarenessTextBox";
            this.distributionAwarenessTextBox.Size = new System.Drawing.Size( 51, 20 );
            this.distributionAwarenessTextBox.TabIndex = 14;
            this.distributionAwarenessTextBox.Text = "0.03";
            // 
            // displayPersuasionTextBox
            // 
            this.displayPersuasionTextBox.Location = new System.Drawing.Point( 94, 68 );
            this.displayPersuasionTextBox.Name = "displayPersuasionTextBox";
            this.displayPersuasionTextBox.Size = new System.Drawing.Size( 51, 20 );
            this.displayPersuasionTextBox.TabIndex = 13;
            this.displayPersuasionTextBox.Text = "0";
            // 
            // displayAwarenessTextBox
            // 
            this.displayAwarenessTextBox.Location = new System.Drawing.Point( 94, 42 );
            this.displayAwarenessTextBox.Name = "displayAwarenessTextBox";
            this.displayAwarenessTextBox.Size = new System.Drawing.Size( 51, 20 );
            this.displayAwarenessTextBox.TabIndex = 12;
            this.displayAwarenessTextBox.Text = "0.25";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 19, 72 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 62, 13 );
            this.label5.TabIndex = 11;
            this.label5.Text = "Persuasion:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 19, 46 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 59, 13 );
            this.label4.TabIndex = 10;
            this.label4.Text = "Awareness";
            // 
            // GeneralSettings
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 586, 332 );
            this.Controls.Add( this.groupBox2 );
            this.Controls.Add( this.groupBox1 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.versionLabel );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GeneralSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Program Settings";
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout( false );
            this.groupBox2.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.TextBox iconFileTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox displayPersuasionTextBox;
        private System.Windows.Forms.TextBox displayAwarenessTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox distributionPersuasionTextBox;
        private System.Windows.Forms.TextBox distributionAwarenessTextBox;
    }
}