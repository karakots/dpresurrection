namespace MrktSimClient.Controls
{
    partial class BannerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( BannerControl ) );
            this.startUpPanel1 = new System.Windows.Forms.Panel();
            this.AppLabel = new System.Windows.Forms.Label();
            this.copyright = new System.Windows.Forms.Label();
            this.startUpPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startUpPanel1
            // 
            this.startUpPanel1.BackColor = System.Drawing.Color.White;
            this.startUpPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject( "startUpPanel1.BackgroundImage" )));
            this.startUpPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.startUpPanel1.Controls.Add( this.AppLabel );
            this.startUpPanel1.Controls.Add( this.copyright );
            this.startUpPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startUpPanel1.Location = new System.Drawing.Point( 0, 0 );
            this.startUpPanel1.Name = "startUpPanel1";
            this.startUpPanel1.Size = new System.Drawing.Size( 562, 405 );
            this.startUpPanel1.TabIndex = 10;
            // 
            // AppLabel
            // 
            this.AppLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AppLabel.AutoSize = true;
            this.AppLabel.Font = new System.Drawing.Font( "Franklin Gothic Medium", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.AppLabel.ForeColor = System.Drawing.Color.Blue;
            this.AppLabel.Location = new System.Drawing.Point( 116, 363 );
            this.AppLabel.Name = "AppLabel";
            this.AppLabel.Size = new System.Drawing.Size( 214, 26 );
            this.AppLabel.TabIndex = 1;
            this.AppLabel.Text = "<App Name> Edition";
            // 
            // copyright
            // 
            this.copyright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.copyright.AutoSize = true;
            this.copyright.BackColor = System.Drawing.Color.White;
            this.copyright.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.copyright.Location = new System.Drawing.Point( 352, 372 );
            this.copyright.Name = "copyright";
            this.copyright.Size = new System.Drawing.Size( 120, 14 );
            this.copyright.TabIndex = 0;
            this.copyright.Text = "Copyright 2007 to 2008";
            // 
            // BannerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.startUpPanel1 );
            this.Name = "BannerControl";
            this.Size = new System.Drawing.Size( 562, 405 );
            this.startUpPanel1.ResumeLayout( false );
            this.startUpPanel1.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Panel startUpPanel1;
        private System.Windows.Forms.Label copyright;
        private System.Windows.Forms.Label AppLabel;
    }
}
