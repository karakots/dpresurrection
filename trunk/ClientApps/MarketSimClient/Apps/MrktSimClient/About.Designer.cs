namespace MrktSimClient
{
    partial class About
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.appNameLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point( 233, 142 );
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size( 75, 23 );
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 84, 112 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 382, 15 );
            this.label1.TabIndex = 1;
            this.label1.Text = "Powered by DecisionPower Advanced Agent Based Modeling Engine";
            // 
            // appNameLabel
            // 
            this.appNameLabel.AutoSize = true;
            this.appNameLabel.Font = new System.Drawing.Font( "Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.appNameLabel.Location = new System.Drawing.Point( 84, 17 );
            this.appNameLabel.Name = "appNameLabel";
            this.appNameLabel.Size = new System.Drawing.Size( 86, 19 );
            this.appNameLabel.TabIndex = 2;
            this.appNameLabel.Text = "MarketSim";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font( "Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label3.Location = new System.Drawing.Point( 163, 14 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 23, 11 );
            this.label3.TabIndex = 3;
            this.label3.Text = "TM";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font( "Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label4.ForeColor = System.Drawing.Color.FromArgb( ((int)(((byte)(0)))), ((int)(((byte)(105)))), ((int)(((byte)(170)))) );
            this.label4.Location = new System.Drawing.Point( 397, 18 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 126, 17 );
            this.label4.TabIndex = 4;
            this.label4.Text = "Revealing the Future!";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 84, 49 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 330, 15 );
            this.label5.TabIndex = 5;
            this.label5.Text = "© Copyright 2008 DecisionPower Inc., Campbell, California";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 84, 80 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 103, 15 );
            this.label6.TabIndex = 6;
            this.label6.Text = "Program Version:";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point( 194, 80 );
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size( 44, 15 );
            this.versionLabel.TabIndex = 7;
            this.versionLabel.Text = "9.9.9.9";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point( 185, 20 );
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size( 140, 15 );
            this.label8.TabIndex = 8;
            this.label8.Text = "Market Modeling System";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MrktSimClient.Properties.Resources.MarketSimLogoIcon48x48;
            this.pictureBox1.Location = new System.Drawing.Point( 10, 12 );
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size( 48, 48 );
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // About
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size( 537, 173 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.appNameLabel );
            this.Controls.Add( this.pictureBox1 );
            this.Controls.Add( this.label8 );
            this.Controls.Add( this.versionLabel );
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.button1 );
            this.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About MarketSim";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label appNameLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}