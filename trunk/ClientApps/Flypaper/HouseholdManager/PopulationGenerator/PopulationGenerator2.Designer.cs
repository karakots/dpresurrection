namespace PopulationGenerator
{
    partial class PopulationGenerator2
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
            this.components = new System.ComponentModel.Container();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.generate_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.scale_up_down = new System.Windows.Forms.NumericUpDown();
            this.pop_timer = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.SaveDirectoryText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.scale_up_down)).BeginInit();
            this.SuspendLayout();
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar.Location = new System.Drawing.Point(12, 126);
            this.ProgressBar.Maximum = 26000;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(360, 23);
            this.ProgressBar.TabIndex = 16;
            // 
            // generate_button
            // 
            this.generate_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.generate_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generate_button.Location = new System.Drawing.Point(12, 62);
            this.generate_button.Name = "generate_button";
            this.generate_button.Size = new System.Drawing.Size(361, 58);
            this.generate_button.TabIndex = 15;
            this.generate_button.Text = "Generate!";
            this.generate_button.UseVisualStyleBackColor = true;
            this.generate_button.Click += new System.EventHandler(this.generate_button_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(182, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Population Scale";
            // 
            // scale_up_down
            // 
            this.scale_up_down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scale_up_down.DecimalPlaces = 4;
            this.scale_up_down.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.scale_up_down.Location = new System.Drawing.Point(12, 36);
            this.scale_up_down.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.scale_up_down.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.scale_up_down.Name = "scale_up_down";
            this.scale_up_down.Size = new System.Drawing.Size(164, 20);
            this.scale_up_down.TabIndex = 13;
            this.scale_up_down.Value = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            // 
            // pop_timer
            // 
            this.pop_timer.Interval = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Save Directory:";
            // 
            // SaveDirectoryText
            // 
            this.SaveDirectoryText.Location = new System.Drawing.Point(98, 10);
            this.SaveDirectoryText.Name = "SaveDirectoryText";
            this.SaveDirectoryText.ReadOnly = true;
            this.SaveDirectoryText.Size = new System.Drawing.Size(274, 20);
            this.SaveDirectoryText.TabIndex = 18;
            // 
            // PopulationGenerator2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 158);
            this.Controls.Add(this.SaveDirectoryText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.generate_button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scale_up_down);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopulationGenerator2";
            this.Text = "PopulationGenerator2";
            ((System.ComponentModel.ISupportInitialize)(this.scale_up_down)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Button generate_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown scale_up_down;
        private System.Windows.Forms.Timer pop_timer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SaveDirectoryText;
    }
}