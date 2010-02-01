namespace SimulationControlPanel
{
    partial class SimulationControlPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( SimulationControlPanel ) );
            this.simGrid = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.createSim = new System.Windows.Forms.ToolStripMenuItem();
            this.updateTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.simGrid)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateTime)).BeginInit();
            this.SuspendLayout();
            // 
            // simGrid
            // 
            this.simGrid.AllowUserToAddRows = false;
            this.simGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.simGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simGrid.Location = new System.Drawing.Point( 0, 24 );
            this.simGrid.Name = "simGrid";
            this.simGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.simGrid.Size = new System.Drawing.Size( 790, 324 );
            this.simGrid.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.createSim} );
            this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size( 790, 24 );
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // createSim
            // 
            this.createSim.Name = "createSim";
            this.createSim.Size = new System.Drawing.Size( 125, 20 );
            this.createSim.Text = "Create Test Simulation";
            this.createSim.Click += new System.EventHandler( this.createSim_Click );
            // 
            // updateTime
            // 
            this.updateTime.Location = new System.Drawing.Point( 314, 0 );
            this.updateTime.Maximum = new decimal( new int[] {
            9,
            0,
            0,
            0} );
            this.updateTime.Name = "updateTime";
            this.updateTime.Size = new System.Drawing.Size( 36, 20 );
            this.updateTime.TabIndex = 2;
            this.updateTime.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.updateTime.ValueChanged += new System.EventHandler( this.updateTime_ValueChanged );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 199, 2 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 109, 13 );
            this.label1.TabIndex = 3;
            this.label1.Text = "refresh rate (seconds)";
            // 
            // SimulationControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 790, 348 );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.updateTime );
            this.Controls.Add( this.simGrid );
            this.Controls.Add( this.menuStrip1 );
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SimulationControlPanel";
            this.Text = "SimulationControlPanel";
            ((System.ComponentModel.ISupportInitialize)(this.simGrid)).EndInit();
            this.menuStrip1.ResumeLayout( false );
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateTime)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView simGrid;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createSim;
        private System.Windows.Forms.NumericUpDown updateTime;
        private System.Windows.Forms.Label label1;
    }
}