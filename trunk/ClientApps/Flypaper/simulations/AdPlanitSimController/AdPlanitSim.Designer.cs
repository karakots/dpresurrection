namespace AdPlanitSimController
{
    partial class AdPlanitSim
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( AdPlanitSim ) );
            this.simState = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // simState
            // 
            this.simState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simState.Location = new System.Drawing.Point( 0, 0 );
            this.simState.Multiline = true;
            this.simState.Name = "simState";
            this.simState.ReadOnly = true;
            this.simState.Size = new System.Drawing.Size( 358, 151 );
            this.simState.TabIndex = 0;
            // 
            // AdPlanitSim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size( 358, 151 );
            this.Controls.Add( this.simState );
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.Name = "AdPlanitSim";
            this.Text = "Starting Simulation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.AdPlanitSim_FormClosing );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox simState;
    }
}