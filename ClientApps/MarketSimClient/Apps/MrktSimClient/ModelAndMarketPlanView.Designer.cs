namespace MrktSimClient
{
    partial class ModelAndMarketPlanView
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
            this.SuspendLayout();
            // 
            // navigatePane
            // 
            this.navigatePane.LineColor = System.Drawing.Color.Black;
            this.navigatePane.Size = new System.Drawing.Size( 168, 402 );
            // 
            // ModelAndMarketPlanView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 792, 402 );
            this.Name = "ModelAndMarketPlanView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ModelAndMarketView";
            this.SizeChanged += new System.EventHandler( this.ModelAndMarketPlanView_SizeChanged );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.ModelAndMarketPlanView_FormClosing );
            this.LocationChanged += new System.EventHandler( this.ModelAndMarketPlanView_LocationChanged );
            this.Load += new System.EventHandler( this.ModelAndMarketPlanView_Load );
            this.ResumeLayout( false );

        }

        #endregion

    }
}