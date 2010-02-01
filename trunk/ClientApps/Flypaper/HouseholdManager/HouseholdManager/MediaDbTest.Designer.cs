namespace HouseholdManager
{
    partial class MediaDbTest
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.subtypeBox = new System.Windows.Forms.ComboBox();
            this.mediaBox = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.vclGrid = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.regSize = new System.Windows.Forms.TextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.regionView = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.vclGrid)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point( 22, 21 );
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size( 75, 23 );
            this.button1.TabIndex = 1;
            this.button1.Text = "read web data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler( this.button1_Click );
            // 
            // subtypeBox
            // 
            this.subtypeBox.FormattingEnabled = true;
            this.subtypeBox.Location = new System.Drawing.Point( 23, 77 );
            this.subtypeBox.Name = "subtypeBox";
            this.subtypeBox.Size = new System.Drawing.Size( 209, 21 );
            this.subtypeBox.TabIndex = 6;
            // 
            // mediaBox
            // 
            this.mediaBox.FormattingEnabled = true;
            this.mediaBox.Location = new System.Drawing.Point( 22, 50 );
            this.mediaBox.Name = "mediaBox";
            this.mediaBox.Size = new System.Drawing.Size( 210, 21 );
            this.mediaBox.TabIndex = 5;
            this.mediaBox.SelectedIndexChanged += new System.EventHandler( this.mediaBox_SelectedIndexChanged );
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point( 23, 114 );
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size( 136, 23 );
            this.button2.TabIndex = 4;
            this.button2.Text = "select vehicles";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler( this.button2_Click );
            // 
            // vclGrid
            // 
            this.vclGrid.AllowUserToAddRows = false;
            this.vclGrid.AllowUserToDeleteRows = false;
            this.vclGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.vclGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vclGrid.Location = new System.Drawing.Point( 0, 0 );
            this.vclGrid.Name = "vclGrid";
            this.vclGrid.ReadOnly = true;
            this.vclGrid.Size = new System.Drawing.Size( 330, 414 );
            this.vclGrid.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.regSize );
            this.splitContainer1.Panel1.Controls.Add( this.mediaBox );
            this.splitContainer1.Panel1.Controls.Add( this.subtypeBox );
            this.splitContainer1.Panel1.Controls.Add( this.button1 );
            this.splitContainer1.Panel1.Controls.Add( this.button2 );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.splitContainer2 );
            this.splitContainer1.Size = new System.Drawing.Size( 755, 414 );
            this.splitContainer1.SplitterDistance = 251;
            this.splitContainer1.TabIndex = 8;
            // 
            // regSize
            // 
            this.regSize.Location = new System.Drawing.Point( 23, 173 );
            this.regSize.Name = "regSize";
            this.regSize.ReadOnly = true;
            this.regSize.Size = new System.Drawing.Size( 151, 20 );
            this.regSize.TabIndex = 7;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add( this.regionView );
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add( this.vclGrid );
            this.splitContainer2.Size = new System.Drawing.Size( 500, 414 );
            this.splitContainer2.SplitterDistance = 166;
            this.splitContainer2.TabIndex = 8;
            // 
            // regionView
            // 
            this.regionView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.regionView.Location = new System.Drawing.Point( 0, 0 );
            this.regionView.Name = "regionView";
            this.regionView.Size = new System.Drawing.Size( 166, 414 );
            this.regionView.TabIndex = 0;
            // 
            // MediaDbTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.splitContainer1 );
            this.Name = "MediaDbTest";
            this.Size = new System.Drawing.Size( 755, 414 );
            ((System.ComponentModel.ISupportInitialize)(this.vclGrid)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.splitContainer2.Panel1.ResumeLayout( false );
            this.splitContainer2.Panel2.ResumeLayout( false );
            this.splitContainer2.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox subtypeBox;
        private System.Windows.Forms.ComboBox mediaBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView vclGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox regSize;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView regionView;
    }
}
