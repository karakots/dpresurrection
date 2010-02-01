namespace SimControlForm
{
    partial class SimControl
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
            this.createSimBut = new System.Windows.Forms.Button();
            this.runSimBut = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.resultsBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // createSimBut
            // 
            this.createSimBut.Location = new System.Drawing.Point( 12, 12 );
            this.createSimBut.Name = "createSimBut";
            this.createSimBut.Size = new System.Drawing.Size( 134, 31 );
            this.createSimBut.TabIndex = 0;
            this.createSimBut.Text = "Create Simulation";
            this.createSimBut.UseVisualStyleBackColor = true;
            this.createSimBut.Click += new System.EventHandler( this.createSimBut_Click );
            // 
            // runSimBut
            // 
            this.runSimBut.Location = new System.Drawing.Point( 12, 49 );
            this.runSimBut.Name = "runSimBut";
            this.runSimBut.Size = new System.Drawing.Size( 134, 31 );
            this.runSimBut.TabIndex = 1;
            this.runSimBut.Text = "Run Simulation";
            this.runSimBut.UseVisualStyleBackColor = true;
            this.runSimBut.Click += new System.EventHandler( this.runSimBut_Click );
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point( 0, 215 );
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size( 345, 22 );
            this.progressBar.TabIndex = 2;
            // 
            // resultsBox
            // 
            this.resultsBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.resultsBox.Location = new System.Drawing.Point( 0, 86 );
            this.resultsBox.Multiline = true;
            this.resultsBox.Name = "resultsBox";
            this.resultsBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.resultsBox.Size = new System.Drawing.Size( 345, 129 );
            this.resultsBox.TabIndex = 3;
            // 
            // SimControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 345, 237 );
            this.Controls.Add( this.resultsBox );
            this.Controls.Add( this.progressBar );
            this.Controls.Add( this.runSimBut );
            this.Controls.Add( this.createSimBut );
            this.Name = "SimControl";
            this.Text = "Simulation Testbed";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button createSimBut;
        private System.Windows.Forms.Button runSimBut;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox resultsBox;
    }
}

