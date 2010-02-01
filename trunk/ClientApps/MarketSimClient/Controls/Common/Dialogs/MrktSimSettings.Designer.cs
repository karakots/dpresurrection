namespace Common.Dialogs
{
    partial class MrktSimSettings
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.showCalibrationControlsCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.productView = new System.Data.DataView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(200)))) );
            this.panel1.Controls.Add( this.helpButton );
            this.panel1.Controls.Add( this.titleLabel );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 498, 26 );
            this.panel1.TabIndex = 9;
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.BackColor = System.Drawing.SystemColors.Control;
            this.helpButton.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.helpButton.Location = new System.Drawing.Point( 469, 2 );
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size( 24, 21 );
            this.helpButton.TabIndex = 4;
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
            this.titleLabel.Size = new System.Drawing.Size( 118, 16 );
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Program Settings";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add( this.groupBox1 );
            this.panel2.Location = new System.Drawing.Point( 7, 24 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 485, 112 );
            this.panel2.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.showCalibrationControlsCheckBox );
            this.groupBox1.Location = new System.Drawing.Point( 10, 14 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 464, 77 );
            this.groupBox1.TabIndex = 58;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Calibration Controls";
            // 
            // showCalibrationControlsCheckBox
            // 
            this.showCalibrationControlsCheckBox.AutoSize = true;
            this.showCalibrationControlsCheckBox.Location = new System.Drawing.Point( 36, 35 );
            this.showCalibrationControlsCheckBox.Name = "showCalibrationControlsCheckBox";
            this.showCalibrationControlsCheckBox.Size = new System.Drawing.Size( 170, 19 );
            this.showCalibrationControlsCheckBox.TabIndex = 56;
            this.showCalibrationControlsCheckBox.Text = "Show Calibration Controls";
            this.showCalibrationControlsCheckBox.UseVisualStyleBackColor = true;
            this.showCalibrationControlsCheckBox.CheckedChanged += new System.EventHandler( this.showCalibrationControlsCheckBox_CheckedChanged );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cancelButton.BackColor = System.Drawing.SystemColors.Control;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.cancelButton.Location = new System.Drawing.Point( 257, 142 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.okButton.BackColor = System.Drawing.SystemColors.Control;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.okButton.Location = new System.Drawing.Point( 164, 142 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = false;
            // 
            // MrktSimSettings
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(200)))) );
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 498, 169 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.panel2 );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.panel1 );
            this.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MrktSimSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout( false );
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productView)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Data.DataView productView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox showCalibrationControlsCheckBox;
    }
}