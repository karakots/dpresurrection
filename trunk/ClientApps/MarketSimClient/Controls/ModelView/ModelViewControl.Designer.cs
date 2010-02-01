namespace ModelView
{
    partial class ModelViewControl
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
            this.navigatePane = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // navigatePane
            // 
            this.navigatePane.Dock = System.Windows.Forms.DockStyle.Left;
            this.navigatePane.HideSelection = false;
            this.navigatePane.HotTracking = true;
            this.navigatePane.Location = new System.Drawing.Point( 0, 0 );
            this.navigatePane.Name = "navigatePane";
            this.navigatePane.Scrollable = false;
            this.navigatePane.ShowLines = false;
            this.navigatePane.Size = new System.Drawing.Size( 168, 480 );
            this.navigatePane.TabIndex = 1;
            this.navigatePane.MouseDown += new System.Windows.Forms.MouseEventHandler( this.navigatePane_MouseDown );
            // 
            // ModelViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.navigatePane );
            this.Name = "ModelViewControl";
            this.Size = new System.Drawing.Size( 800, 480 );
            this.Load += new System.EventHandler( this.ModelViewForm_Load );
            this.ResumeLayout( false );

        }

        #endregion

        protected System.Windows.Forms.TreeView navigatePane;
    }
}
