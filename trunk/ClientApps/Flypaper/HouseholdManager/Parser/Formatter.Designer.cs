namespace Parser
{
    partial class Formatter
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
            this.BrowseButton = new System.Windows.Forms.Button();
            this.FileName = new System.Windows.Forms.TextBox();
            this.GoButton = new System.Windows.Forms.Button();
            this.LeftUpDown = new System.Windows.Forms.NumericUpDown();
            this.TopUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.LeftUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(281, 9);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 0;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // FileName
            // 
            this.FileName.Location = new System.Drawing.Point(12, 12);
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(249, 20);
            this.FileName.TabIndex = 1;
            // 
            // GoButton
            // 
            this.GoButton.Location = new System.Drawing.Point(281, 93);
            this.GoButton.Name = "GoButton";
            this.GoButton.Size = new System.Drawing.Size(75, 23);
            this.GoButton.TabIndex = 2;
            this.GoButton.Text = "GO";
            this.GoButton.UseVisualStyleBackColor = true;
            this.GoButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // LeftUpDown
            // 
            this.LeftUpDown.Location = new System.Drawing.Point(12, 38);
            this.LeftUpDown.Name = "LeftUpDown";
            this.LeftUpDown.Size = new System.Drawing.Size(120, 20);
            this.LeftUpDown.TabIndex = 3;
            // 
            // TopUpDown
            // 
            this.TopUpDown.Location = new System.Drawing.Point(12, 64);
            this.TopUpDown.Name = "TopUpDown";
            this.TopUpDown.Size = new System.Drawing.Size(120, 20);
            this.TopUpDown.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(138, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Left Columns";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(138, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Top Rows";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 99);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(86, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Ignore Zeros";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Formatter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 128);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TopUpDown);
            this.Controls.Add(this.LeftUpDown);
            this.Controls.Add(this.GoButton);
            this.Controls.Add(this.FileName);
            this.Controls.Add(this.BrowseButton);
            this.Name = "Formatter";
            this.Text = "Formatter";
            ((System.ComponentModel.ISupportInitialize)(this.LeftUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.Button GoButton;
        private System.Windows.Forms.NumericUpDown LeftUpDown;
        private System.Windows.Forms.NumericUpDown TopUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

