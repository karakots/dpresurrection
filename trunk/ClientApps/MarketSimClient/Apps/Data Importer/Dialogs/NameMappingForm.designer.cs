namespace DataImporter.Dialogs
{
    partial class NameMappingForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.autoSetButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.InputNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SourceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutputNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add( this.autoSetButton );
            this.panel1.Controls.Add( this.titleLabel );
            this.panel1.Controls.Add( this.label1 );
            this.panel1.Controls.Add( this.dataGridView1 );
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 791, 385 );
            this.panel1.TabIndex = 1;
            // 
            // autoSetButton
            // 
            this.autoSetButton.Location = new System.Drawing.Point( 493, 26 );
            this.autoSetButton.Name = "autoSetButton";
            this.autoSetButton.Size = new System.Drawing.Size( 124, 23 );
            this.autoSetButton.TabIndex = 4;
            this.autoSetButton.Text = "Auto-Set Names...";
            this.autoSetButton.UseVisualStyleBackColor = true;
            this.autoSetButton.Click += new System.EventHandler( this.autoSetButton_Click );
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.titleLabel.Location = new System.Drawing.Point( 12, 11 );
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size( 161, 16 );
            this.titleLabel.TabIndex = 3;
            this.titleLabel.Text = "Set MarketSim Brands";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 12, 31 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 374, 13 );
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter the MarketSim names that you want to use for the given input identifiers.";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.InputNameCol,
            this.SourceCol,
            this.OutputNameCol} );
            this.dataGridView1.Location = new System.Drawing.Point( 12, 55 );
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size( 767, 314 );
            this.dataGridView1.TabIndex = 1;
            // 
            // InputNameCol
            // 
            this.InputNameCol.DataPropertyName = "InputNameCol";
            this.InputNameCol.HeaderText = "Identifier from Input Data";
            this.InputNameCol.Name = "InputNameCol";
            this.InputNameCol.Width = 300;
            // 
            // SourceCol
            // 
            this.SourceCol.DataPropertyName = "SourceCol";
            this.SourceCol.HeaderText = "Identifier Source";
            this.SourceCol.Name = "SourceCol";
            this.SourceCol.Width = 130;
            // 
            // OutputNameCol
            // 
            this.OutputNameCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OutputNameCol.DataPropertyName = "OutputNameCol";
            this.OutputNameCol.HeaderText = "MarketSim Name";
            this.OutputNameCol.Name = "OutputNameCol";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 627, 393 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 708, 392 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // NameMappingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 791, 422 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.panel1 );
            this.Name = "NameMappingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set MarketSim Names";
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputNameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SourceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutputNameCol;
        private System.Windows.Forms.Button autoSetButton;

    }
}