namespace BusinessCategoryReader
{
    partial class Form1
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
            this.label3 = new System.Windows.Forms.Label();
            this.FileName = new System.Windows.Forms.TextBox();
            this.BrowseBut03 = new System.Windows.Forms.Button();
            this.GoBut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Subtype Info File";
            // 
            // FileName
            // 
            this.FileName.Location = new System.Drawing.Point(104, 6);
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.Size = new System.Drawing.Size(100, 20);
            this.FileName.TabIndex = 5;
            // 
            // BrowseBut03
            // 
            this.BrowseBut03.Location = new System.Drawing.Point(221, 4);
            this.BrowseBut03.Name = "BrowseBut03";
            this.BrowseBut03.Size = new System.Drawing.Size(75, 23);
            this.BrowseBut03.TabIndex = 8;
            this.BrowseBut03.Text = "Browse...";
            this.BrowseBut03.UseVisualStyleBackColor = true;
            this.BrowseBut03.Click += new System.EventHandler(this.BrowseBut03_Click);
            // 
            // GoBut
            // 
            this.GoBut.Location = new System.Drawing.Point(221, 33);
            this.GoBut.Name = "GoBut";
            this.GoBut.Size = new System.Drawing.Size(75, 23);
            this.GoBut.TabIndex = 9;
            this.GoBut.Text = "Go!";
            this.GoBut.UseVisualStyleBackColor = true;
            this.GoBut.Click += new System.EventHandler(this.GoBut_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 64);
            this.Controls.Add(this.GoBut);
            this.Controls.Add(this.BrowseBut03);
            this.Controls.Add(this.FileName);
            this.Controls.Add(this.label3);
            this.Name = "Form1";
            this.Text = "CategoryBuilder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.Button BrowseBut03;
        private System.Windows.Forms.Button GoBut;
    }
}

