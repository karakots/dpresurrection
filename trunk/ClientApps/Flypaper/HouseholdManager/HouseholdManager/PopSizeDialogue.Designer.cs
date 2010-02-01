namespace HouseholdManager
{
    partial class PopSizeDialogue
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
            this.popSize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.OKBut = new System.Windows.Forms.Button();
            this.CancelBut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.popSize)).BeginInit();
            this.SuspendLayout();
            // 
            // popSize
            // 
            this.popSize.Location = new System.Drawing.Point( 113, 12 );
            this.popSize.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.popSize.Name = "popSize";
            this.popSize.Size = new System.Drawing.Size( 52, 20 );
            this.popSize.TabIndex = 0;
            this.popSize.Value = new decimal( new int[] {
            10,
            0,
            0,
            0} );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 12, 14 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 80, 13 );
            this.label1.TabIndex = 1;
            this.label1.Text = "Population Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label2.Location = new System.Drawing.Point( 187, 14 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 56, 13 );
            this.label2.TabIndex = 2;
            this.label2.Text = "x 10,000";
            // 
            // OKBut
            // 
            this.OKBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBut.Location = new System.Drawing.Point( 190, 55 );
            this.OKBut.Name = "OKBut";
            this.OKBut.Size = new System.Drawing.Size( 35, 23 );
            this.OKBut.TabIndex = 3;
            this.OKBut.Text = "OK";
            this.OKBut.UseVisualStyleBackColor = true;
            this.OKBut.Click += new System.EventHandler( this.OKBut_Click );
            // 
            // CancelBut
            // 
            this.CancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBut.Location = new System.Drawing.Point( 28, 55 );
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size( 53, 23 );
            this.CancelBut.TabIndex = 4;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            this.CancelBut.Click += new System.EventHandler( this.CancelBut_Click );
            // 
            // PopSizeDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 255, 82 );
            this.ControlBox = false;
            this.Controls.Add( this.CancelBut );
            this.Controls.Add( this.OKBut );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.popSize );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PopSizeDialogue";
            this.Text = "Population Size";
            ((System.ComponentModel.ISupportInitialize)(this.popSize)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown popSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button OKBut;
        private System.Windows.Forms.Button CancelBut;
    }
}