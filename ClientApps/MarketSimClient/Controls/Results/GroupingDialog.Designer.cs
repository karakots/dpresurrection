namespace Results
{
    partial class GroupingDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.GroupNameBox = new System.Windows.Forms.TextBox();
            this.CanButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Group Name:";
            // 
            // GroupNameBox
            // 
            this.GroupNameBox.Location = new System.Drawing.Point(88, 6);
            this.GroupNameBox.Name = "GroupNameBox";
            this.GroupNameBox.Size = new System.Drawing.Size(215, 20);
            this.GroupNameBox.TabIndex = 1;
            // 
            // CanButton
            // 
            this.CanButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CanButton.Location = new System.Drawing.Point(147, 36);
            this.CanButton.Name = "CanButton";
            this.CanButton.Size = new System.Drawing.Size(75, 23);
            this.CanButton.TabIndex = 2;
            this.CanButton.Text = "Cancel";
            this.CanButton.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(228, 36);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 3;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // GroupingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 71);
            this.ControlBox = false;
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CanButton);
            this.Controls.Add(this.GroupNameBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GroupingDialog";
            this.Text = "Create Group";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox GroupNameBox;
        private System.Windows.Forms.Button CanButton;
        private System.Windows.Forms.Button OKButton;
    }
}