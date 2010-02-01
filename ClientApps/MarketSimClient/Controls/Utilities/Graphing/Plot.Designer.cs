namespace Utilities.Graphing
{
    partial class Plot
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
            this.plotControl = new Utilities.Graphing.PlotControl();
            this.SuspendLayout();
            // 
            // plotControl
            // 
            this.plotControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotControl.End = new System.DateTime( 1899, 12, 31, 4, 48, 0, 0 );
            this.plotControl.Location = new System.Drawing.Point( 0, 0 );
            this.plotControl.Max = 1.2;
            this.plotControl.MaxX = 1.2;
            this.plotControl.Min = 0;
            this.plotControl.MinimumSize = new System.Drawing.Size( 200, 100 );
            this.plotControl.MinX = 0;
            this.plotControl.Name = "plotControl";
            this.plotControl.PercentMax = 100;
            this.plotControl.PercentMin = 0;
            this.plotControl.ScatterPlot = false;
            this.plotControl.Size = new System.Drawing.Size( 328, 164 );
            this.plotControl.Start = new System.DateTime( 1899, 12, 30, 0, 0, 0, 0 );
            this.plotControl.TabIndex = 0;
            this.plotControl.TimeSeries = true;
            this.plotControl.Title = "Title";
            this.plotControl.XAxis = "Date";
            this.plotControl.YAxis = "Y-Axis";
            // 
            // Plot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 328, 164 );
            this.ControlBox = false;
            this.Controls.Add( this.plotControl );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size( 200, 100 );
            this.Name = "Plot";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Plot";
            this.ResumeLayout( false );

        }

        #endregion

        private PlotControl plotControl;
    }
}