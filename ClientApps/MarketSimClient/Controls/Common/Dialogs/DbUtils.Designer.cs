namespace Common.Dialogs
{
    partial class DbUtils
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.unLockModelsButton = new System.Windows.Forms.Button();
            this.lockedModelsLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.shrinkButton = new System.Windows.Forms.Button();
            this.resultsSizeLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.sizeLabel = new System.Windows.Forms.Label();
            this.truncateResultsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.productView = new System.Data.DataView();
            this.stopSimBut = new System.Windows.Forms.Button();
            this.stopSimLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.titleLabel.Text = "Database Utilities";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add( this.groupBox1 );
            this.panel2.Controls.Add( this.groupBox2 );
            this.panel2.Location = new System.Drawing.Point( 7, 24 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 485, 244 );
            this.panel2.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add( this.stopSimLabel );
            this.groupBox2.Controls.Add( this.stopSimBut );
            this.groupBox2.Controls.Add( this.unLockModelsButton );
            this.groupBox2.Controls.Add( this.lockedModelsLabel );
            this.groupBox2.Controls.Add( this.versionLabel );
            this.groupBox2.Controls.Add( this.label5 );
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point( 0, 0 );
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size( 485, 130 );
            this.groupBox2.TabIndex = 59;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Configuration";
            // 
            // unLockModelsButton
            // 
            this.unLockModelsButton.BackColor = System.Drawing.Color.White;
            this.unLockModelsButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.unLockModelsButton.Location = new System.Drawing.Point( 262, 57 );
            this.unLockModelsButton.Name = "unLockModelsButton";
            this.unLockModelsButton.Size = new System.Drawing.Size( 174, 21 );
            this.unLockModelsButton.TabIndex = 58;
            this.unLockModelsButton.Text = "Unlock Models or Simulations";
            this.unLockModelsButton.UseVisualStyleBackColor = false;
            this.unLockModelsButton.Click += new System.EventHandler( this.unLockModelsButton_Click );
            // 
            // lockedModelsLabel
            // 
            this.lockedModelsLabel.AutoSize = true;
            this.lockedModelsLabel.Location = new System.Drawing.Point( 31, 57 );
            this.lockedModelsLabel.Name = "lockedModelsLabel";
            this.lockedModelsLabel.Size = new System.Drawing.Size( 189, 15 );
            this.lockedModelsLabel.TabIndex = 54;
            this.lockedModelsLabel.Text = "Models or simulations are locked";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.versionLabel.Location = new System.Drawing.Point( 191, 25 );
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size( 44, 15 );
            this.versionLabel.TabIndex = 53;
            this.versionLabel.Text = "8.8.8.8";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label5.Location = new System.Drawing.Point( 24, 25 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 162, 15 );
            this.label5.TabIndex = 51;
            this.label5.Text = "MarketSim Schema Version:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.shrinkButton );
            this.groupBox1.Controls.Add( this.resultsSizeLabel );
            this.groupBox1.Controls.Add( this.label3 );
            this.groupBox1.Controls.Add( this.sizeLabel );
            this.groupBox1.Controls.Add( this.truncateResultsButton );
            this.groupBox1.Controls.Add( this.label1 );
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point( 0, 130 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 485, 114 );
            this.groupBox1.TabIndex = 58;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Storage Utilization";
            // 
            // shrinkButton
            // 
            this.shrinkButton.BackColor = System.Drawing.Color.White;
            this.shrinkButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.shrinkButton.Location = new System.Drawing.Point( 318, 24 );
            this.shrinkButton.Name = "shrinkButton";
            this.shrinkButton.Size = new System.Drawing.Size( 118, 21 );
            this.shrinkButton.TabIndex = 57;
            this.shrinkButton.Text = "Shrink Database";
            this.shrinkButton.UseVisualStyleBackColor = false;
            this.shrinkButton.Click += new System.EventHandler( this.shrinkButton_Click );
            // 
            // resultsSizeLabel
            // 
            this.resultsSizeLabel.AutoSize = true;
            this.resultsSizeLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.resultsSizeLabel.Location = new System.Drawing.Point( 153, 53 );
            this.resultsSizeLabel.Name = "resultsSizeLabel";
            this.resultsSizeLabel.Size = new System.Drawing.Size( 62, 15 );
            this.resultsSizeLabel.TabIndex = 56;
            this.resultsSizeLabel.Text = "1,234,567";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label3.Location = new System.Drawing.Point( 24, 53 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 111, 15 );
            this.label3.TabIndex = 55;
            this.label3.Text = "Results Data Size: ";
            // 
            // sizeLabel
            // 
            this.sizeLabel.AutoSize = true;
            this.sizeLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.sizeLabel.Location = new System.Drawing.Point( 153, 27 );
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size( 62, 15 );
            this.sizeLabel.TabIndex = 54;
            this.sizeLabel.Text = "1,234,567";
            // 
            // truncateResultsButton
            // 
            this.truncateResultsButton.BackColor = System.Drawing.Color.White;
            this.truncateResultsButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.truncateResultsButton.Location = new System.Drawing.Point( 262, 51 );
            this.truncateResultsButton.Name = "truncateResultsButton";
            this.truncateResultsButton.Size = new System.Drawing.Size( 174, 21 );
            this.truncateResultsButton.TabIndex = 0;
            this.truncateResultsButton.Text = "Delete ALL Results Data";
            this.truncateResultsButton.UseVisualStyleBackColor = false;
            this.truncateResultsButton.Click += new System.EventHandler( this.deleteResultsButton_Click );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 24, 27 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 123, 15 );
            this.label1.TabIndex = 52;
            this.label1.Text = "Total Database Size: ";
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.okButton.BackColor = System.Drawing.SystemColors.Control;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.okButton.Location = new System.Drawing.Point( 213, 274 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = false;
            // 
            // stopSimBut
            // 
            this.stopSimBut.BackColor = System.Drawing.Color.White;
            this.stopSimBut.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.stopSimBut.Location = new System.Drawing.Point( 262, 91 );
            this.stopSimBut.Name = "stopSimBut";
            this.stopSimBut.Size = new System.Drawing.Size( 174, 21 );
            this.stopSimBut.TabIndex = 59;
            this.stopSimBut.Text = "Reset Simulations";
            this.stopSimBut.UseVisualStyleBackColor = false;
            this.stopSimBut.Click += new System.EventHandler( this.stopSimBut_Click );
            // 
            // stopSimLabel
            // 
            this.stopSimLabel.AutoSize = true;
            this.stopSimLabel.Location = new System.Drawing.Point( 31, 91 );
            this.stopSimLabel.Name = "stopSimLabel";
            this.stopSimLabel.Size = new System.Drawing.Size( 198, 15 );
            this.stopSimLabel.TabIndex = 60;
            this.stopSimLabel.Text = "Simulations are running or queued";
            // 
            // DbUtils
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(200)))) );
            this.ClientSize = new System.Drawing.Size( 498, 301 );
            this.Controls.Add( this.panel2 );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.panel1 );
            this.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DbUtils";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database Utilities";
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout( false );
            this.groupBox2.ResumeLayout( false );
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.Button okButton;
        private System.Data.DataView productView;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label sizeLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button truncateResultsButton;
        private System.Windows.Forms.Label resultsSizeLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button shrinkButton;
        private System.Windows.Forms.Label lockedModelsLabel;
        private System.Windows.Forms.Button unLockModelsButton;
        private System.Windows.Forms.Button stopSimBut;
        private System.Windows.Forms.Label stopSimLabel;
    }
}