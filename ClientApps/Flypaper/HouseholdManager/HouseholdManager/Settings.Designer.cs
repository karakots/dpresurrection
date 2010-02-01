namespace HouseholdManager
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OKButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.rawdata_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.web_path = new System.Windows.Forms.TextBox();
            this.RawBrowse = new System.Windows.Forms.Button();
            this.MediaBrowse = new System.Windows.Forms.Button();
            this.AgentBrowse = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.hhinput_path = new System.Windows.Forms.TextBox();
            this.OutputBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.output_path = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Enabled = false;
            this.OKButton.Location = new System.Drawing.Point(356, 193);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(275, 193);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // rawdata_path
            // 
            this.rawdata_path.Location = new System.Drawing.Point(15, 25);
            this.rawdata_path.Name = "rawdata_path";
            this.rawdata_path.ReadOnly = true;
            this.rawdata_path.Size = new System.Drawing.Size(326, 20);
            this.rawdata_path.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Raw Data Location";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Web Media Directory";
            // 
            // web_path
            // 
            this.web_path.Location = new System.Drawing.Point(15, 64);
            this.web_path.Name = "web_path";
            this.web_path.ReadOnly = true;
            this.web_path.Size = new System.Drawing.Size(326, 20);
            this.web_path.TabIndex = 4;
            // 
            // RawBrowse
            // 
            this.RawBrowse.Location = new System.Drawing.Point(347, 25);
            this.RawBrowse.Name = "RawBrowse";
            this.RawBrowse.Size = new System.Drawing.Size(75, 23);
            this.RawBrowse.TabIndex = 6;
            this.RawBrowse.Text = "Browse...";
            this.RawBrowse.UseVisualStyleBackColor = true;
            this.RawBrowse.Click += new System.EventHandler(this.HHBrowse_Click);
            // 
            // MediaBrowse
            // 
            this.MediaBrowse.Location = new System.Drawing.Point(347, 64);
            this.MediaBrowse.Name = "MediaBrowse";
            this.MediaBrowse.Size = new System.Drawing.Size(75, 23);
            this.MediaBrowse.TabIndex = 7;
            this.MediaBrowse.Text = "Browse...";
            this.MediaBrowse.UseVisualStyleBackColor = true;
            this.MediaBrowse.Click += new System.EventHandler(this.MediaBrowse_Click);
            // 
            // AgentBrowse
            // 
            this.AgentBrowse.Location = new System.Drawing.Point(347, 103);
            this.AgentBrowse.Name = "AgentBrowse";
            this.AgentBrowse.Size = new System.Drawing.Size(75, 23);
            this.AgentBrowse.TabIndex = 12;
            this.AgentBrowse.Text = "Browse...";
            this.AgentBrowse.UseVisualStyleBackColor = true;
            this.AgentBrowse.Click += new System.EventHandler(this.AgentBrowse_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(175, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Household Manager Input Directory";
            // 
            // hhinput_path
            // 
            this.hhinput_path.Location = new System.Drawing.Point(15, 103);
            this.hhinput_path.Name = "hhinput_path";
            this.hhinput_path.ReadOnly = true;
            this.hhinput_path.Size = new System.Drawing.Size(326, 20);
            this.hhinput_path.TabIndex = 8;
            // 
            // OutputBrowse
            // 
            this.OutputBrowse.Location = new System.Drawing.Point(347, 142);
            this.OutputBrowse.Name = "OutputBrowse";
            this.OutputBrowse.Size = new System.Drawing.Size(75, 23);
            this.OutputBrowse.TabIndex = 15;
            this.OutputBrowse.Text = "Browse...";
            this.OutputBrowse.UseVisualStyleBackColor = true;
            this.OutputBrowse.Click += new System.EventHandler(this.OutputBrowse_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Household Manager Output Directory";
            // 
            // output_path
            // 
            this.output_path.Location = new System.Drawing.Point(15, 142);
            this.output_path.Name = "output_path";
            this.output_path.ReadOnly = true;
            this.output_path.Size = new System.Drawing.Size(326, 20);
            this.output_path.TabIndex = 13;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 228);
            this.ControlBox = false;
            this.Controls.Add(this.OutputBrowse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.output_path);
            this.Controls.Add(this.AgentBrowse);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.hhinput_path);
            this.Controls.Add(this.MediaBrowse);
            this.Controls.Add(this.RawBrowse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.web_path);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rawdata_path);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox rawdata_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox web_path;
        private System.Windows.Forms.Button RawBrowse;
        private System.Windows.Forms.Button MediaBrowse;
        private System.Windows.Forms.Button AgentBrowse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox hhinput_path;
        private System.Windows.Forms.Button OutputBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox output_path;
    }
}