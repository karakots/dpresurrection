namespace Utilities
{
    partial class FadeTextPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.fadePanel = new System.Windows.Forms.Panel();
            this.label = new System.Windows.Forms.Label();
            this.fadePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // fadePanel
            // 
            this.fadePanel.AutoScroll = true;
            this.fadePanel.AutoSize = true;
            this.fadePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fadePanel.Controls.Add( this.label );
            this.fadePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fadePanel.Location = new System.Drawing.Point( 0, 0 );
            this.fadePanel.Name = "fadePanel";
            this.fadePanel.Padding = new System.Windows.Forms.Padding( 4 );
            this.fadePanel.Size = new System.Drawing.Size( 197, 61 );
            this.fadePanel.TabIndex = 0;
            this.fadePanel.Resize += new System.EventHandler( this.fadePanel_Resize );
            this.fadePanel.Paint += new System.Windows.Forms.PaintEventHandler( this.fadePanel_Paint );
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.BackColor = System.Drawing.Color.Transparent;
            this.label.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label.Location = new System.Drawing.Point( 4, 4 );
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size( 29, 14 );
            this.label.TabIndex = 0;
            this.label.Text = "label";
            // 
            // FadeTextPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.fadePanel );
            this.Name = "FadeTextPanel";
            this.Size = new System.Drawing.Size( 197, 61 );
            this.fadePanel.ResumeLayout( false );
            this.fadePanel.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel fadePanel;
        private System.Windows.Forms.Label label;
    }
}
