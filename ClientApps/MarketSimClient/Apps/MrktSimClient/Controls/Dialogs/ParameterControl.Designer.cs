namespace MrktSimClient.Controls.Dialogs
{
    partial class ParameterControl
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.modelParmGrid = new MarketSimUtilities.MrktSimGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.applyParmsButton = new System.Windows.Forms.Button();
            this.resetParamValuesButton = new System.Windows.Forms.Button();
            this.removeParmButton = new System.Windows.Forms.Button();
            this.addparmButton = new System.Windows.Forms.Button();
            this.scenarioParmGrid = new MarketSimUtilities.MrktSimGrid();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.modelParmGrid );
            this.splitContainer1.Panel1.Controls.Add( this.panel1 );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.scenarioParmGrid );
            this.splitContainer1.Size = new System.Drawing.Size( 729, 476 );
            this.splitContainer1.SplitterDistance = 253;
            this.splitContainer1.TabIndex = 0;
            // 
            // modelParmGrid
            // 
            this.modelParmGrid.DescribeRow = null;
            this.modelParmGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelParmGrid.EnabledGrid = true;
            this.modelParmGrid.Location = new System.Drawing.Point( 0, 0 );
            this.modelParmGrid.Name = "modelParmGrid";
            this.modelParmGrid.RowFilter = null;
            this.modelParmGrid.RowID = null;
            this.modelParmGrid.RowName = null;
            this.modelParmGrid.Size = new System.Drawing.Size( 729, 159 );
            this.modelParmGrid.Sort = "";
            this.modelParmGrid.TabIndex = 1;
            this.modelParmGrid.Table = null;
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.applyParmsButton );
            this.panel1.Controls.Add( this.resetParamValuesButton );
            this.panel1.Controls.Add( this.removeParmButton );
            this.panel1.Controls.Add( this.addparmButton );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point( 0, 159 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 729, 94 );
            this.panel1.TabIndex = 0;
            // 
            // applyParmsButton
            // 
            this.applyParmsButton.Location = new System.Drawing.Point( 317, 55 );
            this.applyParmsButton.Name = "applyParmsButton";
            this.applyParmsButton.Size = new System.Drawing.Size( 254, 23 );
            this.applyParmsButton.TabIndex = 3;
            this.applyParmsButton.Text = "Apply Selected Parameter Values to Model";
            this.applyParmsButton.UseVisualStyleBackColor = true;
            this.applyParmsButton.Click += new System.EventHandler( this.applyParmsButton_Click );
            // 
            // resetParamValuesButton
            // 
            this.resetParamValuesButton.Location = new System.Drawing.Point( 317, 14 );
            this.resetParamValuesButton.Name = "resetParamValuesButton";
            this.resetParamValuesButton.Size = new System.Drawing.Size( 254, 23 );
            this.resetParamValuesButton.TabIndex = 2;
            this.resetParamValuesButton.Text = "Reset Parameter Values";
            this.resetParamValuesButton.UseVisualStyleBackColor = true;
            this.resetParamValuesButton.Click += new System.EventHandler( this.resetParamValuesButton_Click );
            // 
            // removeParmButton
            // 
            this.removeParmButton.Location = new System.Drawing.Point( 15, 55 );
            this.removeParmButton.Name = "removeParmButton";
            this.removeParmButton.Size = new System.Drawing.Size( 245, 23 );
            this.removeParmButton.TabIndex = 1;
            this.removeParmButton.Text = "Remove Parameter from Simulation";
            this.removeParmButton.UseVisualStyleBackColor = true;
            this.removeParmButton.Click += new System.EventHandler( this.removeParmButton_Click );
            // 
            // addparmButton
            // 
            this.addparmButton.Location = new System.Drawing.Point( 15, 14 );
            this.addparmButton.Name = "addparmButton";
            this.addparmButton.Size = new System.Drawing.Size( 245, 23 );
            this.addparmButton.TabIndex = 0;
            this.addparmButton.Text = "Add Parameter to Simulation";
            this.addparmButton.UseVisualStyleBackColor = true;
            this.addparmButton.Click += new System.EventHandler( this.addparmButton_Click );
            // 
            // scenarioParmGrid
            // 
            this.scenarioParmGrid.DescribeRow = null;
            this.scenarioParmGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenarioParmGrid.EnabledGrid = true;
            this.scenarioParmGrid.Location = new System.Drawing.Point( 0, 0 );
            this.scenarioParmGrid.Name = "scenarioParmGrid";
            this.scenarioParmGrid.RowFilter = null;
            this.scenarioParmGrid.RowID = null;
            this.scenarioParmGrid.RowName = null;
            this.scenarioParmGrid.Size = new System.Drawing.Size( 729, 219 );
            this.scenarioParmGrid.Sort = "";
            this.scenarioParmGrid.TabIndex = 0;
            this.scenarioParmGrid.Table = null;
            this.scenarioParmGrid.Validated += new System.EventHandler( this.scenarioParmGrid_Validated );
            // 
            // ParameterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainer1 );
            this.Name = "ParameterControl";
            this.Size = new System.Drawing.Size( 729, 476 );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.panel1.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button resetParamValuesButton;
        private System.Windows.Forms.Button removeParmButton;
        private System.Windows.Forms.Button addparmButton;
        private MarketSimUtilities.MrktSimGrid modelParmGrid;
        private MarketSimUtilities.MrktSimGrid scenarioParmGrid;
        private System.Windows.Forms.Button applyParmsButton;
    }
}
