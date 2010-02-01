namespace MrktSimClient.Controls.Dialogs
{
    partial class VariableControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.createVariableButton = new System.Windows.Forms.Button();
            this.variableGrid = new MarketSimUtilities.MrktSimGrid();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.createVariableButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(491, 37);
            this.panel1.TabIndex = 0;
            // 
            // createVariableButton
            // 
            this.createVariableButton.Location = new System.Drawing.Point(145, 7);
            this.createVariableButton.Name = "createVariableButton";
            this.createVariableButton.Size = new System.Drawing.Size(142, 23);
            this.createVariableButton.TabIndex = 3;
            this.createVariableButton.Text = "Create New Variable...";
            this.createVariableButton.UseVisualStyleBackColor = true;
            this.createVariableButton.Click += new System.EventHandler(this.createVariableButton_Click);
            // 
            // variableGrid
            // 
            this.variableGrid.DescribeRow = null;
            this.variableGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.variableGrid.EnabledGrid = true;
            this.variableGrid.Location = new System.Drawing.Point(0, 37);
            this.variableGrid.Name = "variableGrid";
            this.variableGrid.RowFilter = null;
            this.variableGrid.RowID = null;
            this.variableGrid.RowName = null;
            this.variableGrid.Size = new System.Drawing.Size(491, 284);
            this.variableGrid.Sort = "";
            this.variableGrid.TabIndex = 1;
            this.variableGrid.Table = null;
            // 
            // VariableControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.variableGrid);
            this.Controls.Add(this.panel1);
            this.Name = "VariableControl";
            this.Size = new System.Drawing.Size(491, 321);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button createVariableButton;
        private MarketSimUtilities.MrktSimGrid variableGrid;
    }
}
