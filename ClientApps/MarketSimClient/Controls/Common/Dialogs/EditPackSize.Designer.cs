namespace Common.Dialogs
{
    partial class EditPackSize
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
            this.components = new System.ComponentModel.Container();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numDistPoints = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.okBut = new System.Windows.Forms.Button();
            this.cancelBut = new System.Windows.Forms.Button();
            this.applyBut = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.totalBox = new System.Windows.Forms.TextBox();
            this.meanBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.devBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.normalizeBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
            this.infoBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numDistPoints)).BeginInit();
            this.SuspendLayout();
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nameBox.Location = new System.Drawing.Point( 176, 10 );
            this.nameBox.MaxLength = 32;
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size( 183, 20 );
            this.nameBox.TabIndex = 0;
            this.nameBox.Text = "default";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 69, 12 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 86, 13 );
            this.label1.TabIndex = 1;
            this.label1.Text = "Pack Size Name";
            // 
            // numDistPoints
            // 
            this.numDistPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numDistPoints.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numDistPoints.Location = new System.Drawing.Point( 176, 49 );
            this.numDistPoints.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numDistPoints.Name = "numDistPoints";
            this.numDistPoints.Size = new System.Drawing.Size( 49, 20 );
            this.numDistPoints.TabIndex = 2;
            this.numDistPoints.Value = new decimal( new int[] {
            100,
            0,
            0,
            0} );
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 12, 51 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 143, 13 );
            this.label2.TabIndex = 3;
            this.label2.Text = "Number of Distribution Points";
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.Location = new System.Drawing.Point( 323, 209 );
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size( 36, 23 );
            this.okBut.TabIndex = 4;
            this.okBut.Text = "OK";
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler( this.okBut_Click );
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.Location = new System.Drawing.Point( 187, 209 );
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size( 50, 23 );
            this.cancelBut.TabIndex = 5;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            this.cancelBut.Click += new System.EventHandler( this.cancelBut_Click );
            // 
            // applyBut
            // 
            this.applyBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.applyBut.Location = new System.Drawing.Point( 254, 209 );
            this.applyBut.Name = "applyBut";
            this.applyBut.Size = new System.Drawing.Size( 50, 23 );
            this.applyBut.TabIndex = 6;
            this.applyBut.Text = "Apply";
            this.applyBut.UseVisualStyleBackColor = true;
            this.applyBut.Click += new System.EventHandler( this.applyBut_Click );
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 69, 112 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 86, 13 );
            this.label3.TabIndex = 7;
            this.label3.Text = "Total Distribution";
            // 
            // totalBox
            // 
            this.totalBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.totalBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.totalBox.Location = new System.Drawing.Point( 177, 110 );
            this.totalBox.Name = "totalBox";
            this.totalBox.ReadOnly = true;
            this.totalBox.Size = new System.Drawing.Size( 48, 20 );
            this.totalBox.TabIndex = 8;
            this.totalBox.Text = "100";
            // 
            // meanBox
            // 
            this.meanBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.meanBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.meanBox.Location = new System.Drawing.Point( 177, 138 );
            this.meanBox.Name = "meanBox";
            this.meanBox.ReadOnly = true;
            this.meanBox.Size = new System.Drawing.Size( 48, 20 );
            this.meanBox.TabIndex = 10;
            this.meanBox.Text = "100";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 98, 140 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 57, 13 );
            this.label4.TabIndex = 9;
            this.label4.Text = "Mean Size";
            // 
            // devBox
            // 
            this.devBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.devBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.devBox.Location = new System.Drawing.Point( 177, 164 );
            this.devBox.Name = "devBox";
            this.devBox.ReadOnly = true;
            this.devBox.Size = new System.Drawing.Size( 48, 20 );
            this.devBox.TabIndex = 12;
            this.devBox.Text = "100";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 57, 166 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 98, 13 );
            this.label5.TabIndex = 11;
            this.label5.Text = "Standard Deviation";
            // 
            // normalizeBox
            // 
            this.normalizeBox.AutoSize = true;
            this.normalizeBox.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.normalizeBox.Location = new System.Drawing.Point( 244, 87 );
            this.normalizeBox.Name = "normalizeBox";
            this.normalizeBox.Size = new System.Drawing.Size( 81, 17 );
            this.normalizeBox.TabIndex = 13;
            this.normalizeBox.Text = "Normalize";
            this.toolTip1.SetToolTip( this.normalizeBox, "Probability will be normalized when Ok or Apply is clicked" );
            this.normalizeBox.UseVisualStyleBackColor = true;
            // 
            // infoBox
            // 
            this.infoBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoBox.Location = new System.Drawing.Point( 244, 110 );
            this.infoBox.Multiline = true;
            this.infoBox.Name = "infoBox";
            this.infoBox.ReadOnly = true;
            this.infoBox.Size = new System.Drawing.Size( 118, 74 );
            this.infoBox.TabIndex = 14;
            this.infoBox.Text = "Probabilities are divided\r\nby total to sum to 100%\r\n\r\nIf total = 0 a uniform\r\ndis" +
                "tribution is created";
            // 
            // EditPackSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 374, 244 );
            this.ControlBox = false;
            this.Controls.Add( this.infoBox );
            this.Controls.Add( this.normalizeBox );
            this.Controls.Add( this.devBox );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.meanBox );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.totalBox );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.applyBut );
            this.Controls.Add( this.cancelBut );
            this.Controls.Add( this.okBut );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.numDistPoints );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.nameBox );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditPackSize";
            this.Text = "Pack Size";
            ((System.ComponentModel.ISupportInitialize)(this.numDistPoints)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numDistPoints;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button okBut;
        private System.Windows.Forms.Button cancelBut;
        private System.Windows.Forms.Button applyBut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox totalBox;
        private System.Windows.Forms.TextBox meanBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox devBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox normalizeBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox infoBox;
    }
}