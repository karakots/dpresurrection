namespace Common.Dialogs
{
    partial class ChangePlanDates
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
            this.topLevelInfoLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.shiftDays = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.productView = new System.Data.DataView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shiftDays)).BeginInit();
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
            this.titleLabel.Text = "Shft Plan in Time";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add( this.topLevelInfoLabel );
            this.panel2.Controls.Add( this.label2 );
            this.panel2.Controls.Add( this.shiftDays );
            this.panel2.Controls.Add( this.label1 );
            this.panel2.Location = new System.Drawing.Point( 7, 24 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 485, 99 );
            this.panel2.TabIndex = 10;
            // 
            // topLevelInfoLabel
            // 
            this.topLevelInfoLabel.AutoSize = true;
            this.topLevelInfoLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.topLevelInfoLabel.Location = new System.Drawing.Point( 20, 62 );
            this.topLevelInfoLabel.Name = "topLevelInfoLabel";
            this.topLevelInfoLabel.Size = new System.Drawing.Size( 443, 14 );
            this.topLevelInfoLabel.TabIndex = 54;
            this.topLevelInfoLabel.Text = "All Components in the selected Market Plan will be COPIED and shifted along with " +
                "the Plan. ";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label2.Location = new System.Drawing.Point( 266, 40 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 129, 23 );
            this.label2.TabIndex = 53;
            this.label2.Text = "days ";
            // 
            // shiftDays
            // 
            this.shiftDays.Location = new System.Drawing.Point( 163, 38 );
            this.shiftDays.Maximum = new decimal( new int[] {
            10000,
            0,
            0,
            0} );
            this.shiftDays.Minimum = new decimal( new int[] {
            10000,
            0,
            0,
            -2147483648} );
            this.shiftDays.Name = "shiftDays";
            this.shiftDays.Size = new System.Drawing.Size( 98, 21 );
            this.shiftDays.TabIndex = 52;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 20, 40 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 141, 23 );
            this.label1.TabIndex = 51;
            this.label1.Text = "Shift Plan dates by ";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cancelButton.BackColor = System.Drawing.SystemColors.Control;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.cancelButton.Location = new System.Drawing.Point( 257, 129 );
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
            this.okButton.Location = new System.Drawing.Point( 164, 129 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // ChangePlanDates
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(200)))) );
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 498, 156 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.panel2 );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.panel1 );
            this.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ChangePlanDates";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shift Plan";
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout( false );
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shiftDays)).EndInit();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown shiftDays;
        private System.Windows.Forms.Label label1;
        private System.Data.DataView productView;
        private System.Windows.Forms.Label topLevelInfoLabel;
    }
}