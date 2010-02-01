namespace Calibrator
{
    partial class Calibrator
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.sim_run = new System.Windows.Forms.Timer(this.components);
            this.write_timer = new System.Windows.Forms.Timer(this.components);
            this.spawner = new System.Windows.Forms.Timer(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.write_to_file = new System.Windows.Forms.Button();
            this.use_results_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(433, 268);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Write to DB";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(30, 24);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Run Sim";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // sim_run
            // 
            this.sim_run.Interval = 1001;
            this.sim_run.Tick += new System.EventHandler(this.sim_run_Tick);
            // 
            // write_timer
            // 
            this.write_timer.Interval = 2001;
            this.write_timer.Tick += new System.EventHandler(this.write_timer_Tick);
            // 
            // spawner
            // 
            this.spawner.Interval = 1000;
            this.spawner.Tick += new System.EventHandler(this.spawner_Tick);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(30, 74);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Write Results";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // write_to_file
            // 
            this.write_to_file.Location = new System.Drawing.Point(433, 239);
            this.write_to_file.Name = "write_to_file";
            this.write_to_file.Size = new System.Drawing.Size(75, 23);
            this.write_to_file.TabIndex = 3;
            this.write_to_file.Text = "Write to File";
            this.write_to_file.UseVisualStyleBackColor = true;
            this.write_to_file.Click += new System.EventHandler(this.write_to_file_Click);
            // 
            // use_results_button
            // 
            this.use_results_button.Location = new System.Drawing.Point(30, 127);
            this.use_results_button.Name = "use_results_button";
            this.use_results_button.Size = new System.Drawing.Size(75, 23);
            this.use_results_button.TabIndex = 4;
            this.use_results_button.Text = "Use Results";
            this.use_results_button.UseVisualStyleBackColor = true;
            this.use_results_button.Click += new System.EventHandler(this.use_results_button_Click);
            // 
            // Calibrator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 303);
            this.Controls.Add(this.use_results_button);
            this.Controls.Add(this.write_to_file);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Calibrator";
            this.Text = "Calibrator";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer sim_run;
        private System.Windows.Forms.Timer write_timer;
        private System.Windows.Forms.Timer spawner;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button write_to_file;
        private System.Windows.Forms.Button use_results_button;
    }
}

