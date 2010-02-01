namespace HouseholdManager
{
    partial class PopulationGenerator
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
            this.total_file = new System.Windows.Forms.TextBox();
            this.total_browse = new System.Windows.Forms.Button();
            this.size_browse = new System.Windows.Forms.Button();
            this.size_file = new System.Windows.Forms.TextBox();
            this.income_browse = new System.Windows.Forms.Button();
            this.income_file = new System.Windows.Forms.TextBox();
            this.age_browse = new System.Windows.Forms.Button();
            this.age_file = new System.Windows.Forms.TextBox();
            this.auto_browse = new System.Windows.Forms.Button();
            this.scale_up_down = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.generate_button = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.GenerateTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.scale_up_down)).BeginInit();
            this.SuspendLayout();
            // 
            // total_file
            // 
            this.total_file.Location = new System.Drawing.Point(12, 41);
            this.total_file.Name = "total_file";
            this.total_file.ReadOnly = true;
            this.total_file.Size = new System.Drawing.Size(280, 20);
            this.total_file.TabIndex = 0;
            this.total_file.Text = "Total Households File";
            this.total_file.TextChanged += new System.EventHandler(this.total_file_TextChanged);
            // 
            // total_browse
            // 
            this.total_browse.Location = new System.Drawing.Point(298, 39);
            this.total_browse.Name = "total_browse";
            this.total_browse.Size = new System.Drawing.Size(75, 23);
            this.total_browse.TabIndex = 1;
            this.total_browse.Text = "Browse...";
            this.total_browse.UseVisualStyleBackColor = true;
            this.total_browse.Click += new System.EventHandler(this.total_browse_Click);
            // 
            // size_browse
            // 
            this.size_browse.Location = new System.Drawing.Point(298, 65);
            this.size_browse.Name = "size_browse";
            this.size_browse.Size = new System.Drawing.Size(75, 23);
            this.size_browse.TabIndex = 3;
            this.size_browse.Text = "Browse...";
            this.size_browse.UseVisualStyleBackColor = true;
            this.size_browse.Click += new System.EventHandler(this.size_browse_Click);
            // 
            // size_file
            // 
            this.size_file.Location = new System.Drawing.Point(12, 67);
            this.size_file.Name = "size_file";
            this.size_file.ReadOnly = true;
            this.size_file.Size = new System.Drawing.Size(280, 20);
            this.size_file.TabIndex = 2;
            this.size_file.Text = "Houshold Size and Type by Race";
            this.size_file.TextChanged += new System.EventHandler(this.size_file_TextChanged);
            // 
            // income_browse
            // 
            this.income_browse.Location = new System.Drawing.Point(298, 91);
            this.income_browse.Name = "income_browse";
            this.income_browse.Size = new System.Drawing.Size(75, 23);
            this.income_browse.TabIndex = 5;
            this.income_browse.Text = "Browse...";
            this.income_browse.UseVisualStyleBackColor = true;
            this.income_browse.Click += new System.EventHandler(this.income_browse_Click);
            // 
            // income_file
            // 
            this.income_file.Location = new System.Drawing.Point(12, 93);
            this.income_file.Name = "income_file";
            this.income_file.ReadOnly = true;
            this.income_file.Size = new System.Drawing.Size(280, 20);
            this.income_file.TabIndex = 4;
            this.income_file.Text = "Household Income by Race";
            this.income_file.TextChanged += new System.EventHandler(this.income_file_TextChanged);
            // 
            // age_browse
            // 
            this.age_browse.Location = new System.Drawing.Point(298, 117);
            this.age_browse.Name = "age_browse";
            this.age_browse.Size = new System.Drawing.Size(75, 23);
            this.age_browse.TabIndex = 7;
            this.age_browse.Text = "Browse...";
            this.age_browse.UseVisualStyleBackColor = true;
            this.age_browse.Click += new System.EventHandler(this.age_browse_Click);
            // 
            // age_file
            // 
            this.age_file.Location = new System.Drawing.Point(12, 119);
            this.age_file.Name = "age_file";
            this.age_file.ReadOnly = true;
            this.age_file.Size = new System.Drawing.Size(280, 20);
            this.age_file.TabIndex = 6;
            this.age_file.Text = "Sex and Age by Race";
            this.age_file.TextChanged += new System.EventHandler(this.age_file_TextChanged);
            // 
            // auto_browse
            // 
            this.auto_browse.Location = new System.Drawing.Point(12, 12);
            this.auto_browse.Name = "auto_browse";
            this.auto_browse.Size = new System.Drawing.Size(152, 23);
            this.auto_browse.TabIndex = 8;
            this.auto_browse.Text = "Choose By Directory...";
            this.auto_browse.UseVisualStyleBackColor = true;
            this.auto_browse.Click += new System.EventHandler(this.auto_browse_Click);
            // 
            // scale_up_down
            // 
            this.scale_up_down.DecimalPlaces = 4;
            this.scale_up_down.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.scale_up_down.Location = new System.Drawing.Point(12, 145);
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
            this.scale_up_down.TabIndex = 9;
            this.scale_up_down.Value = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(182, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Population Scale";
            // 
            // generate_button
            // 
            this.generate_button.Font = new System.Drawing.Font("PMingLiU", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generate_button.Location = new System.Drawing.Point(12, 171);
            this.generate_button.Name = "generate_button";
            this.generate_button.Size = new System.Drawing.Size(361, 58);
            this.generate_button.TabIndex = 11;
            this.generate_button.Text = "Generate!";
            this.generate_button.UseVisualStyleBackColor = true;
            this.generate_button.Click += new System.EventHandler(this.generate_button_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 236);
            this.progressBar1.Maximum = 26000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(360, 23);
            this.progressBar1.TabIndex = 12;
            // 
            // GenerateTimer
            // 
            this.GenerateTimer.Interval = 1;
            this.GenerateTimer.Tick += new System.EventHandler(this.GenerateTimer_Tick);
            // 
            // PopulationGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 273);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.generate_button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scale_up_down);
            this.Controls.Add(this.auto_browse);
            this.Controls.Add(this.age_browse);
            this.Controls.Add(this.age_file);
            this.Controls.Add(this.income_browse);
            this.Controls.Add(this.income_file);
            this.Controls.Add(this.size_browse);
            this.Controls.Add(this.size_file);
            this.Controls.Add(this.total_browse);
            this.Controls.Add(this.total_file);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopulationGenerator";
            this.Text = "PopulationGenerator";
            ((System.ComponentModel.ISupportInitialize)(this.scale_up_down)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox total_file;
        private System.Windows.Forms.Button total_browse;
        private System.Windows.Forms.Button size_browse;
        private System.Windows.Forms.TextBox size_file;
        private System.Windows.Forms.Button income_browse;
        private System.Windows.Forms.TextBox income_file;
        private System.Windows.Forms.Button age_browse;
        private System.Windows.Forms.TextBox age_file;
        private System.Windows.Forms.Button auto_browse;
        private System.Windows.Forms.NumericUpDown scale_up_down;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button generate_button;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer GenerateTimer;
    }
}