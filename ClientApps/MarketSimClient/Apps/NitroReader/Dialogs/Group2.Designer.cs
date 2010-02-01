namespace NitroReader.Dialogs
{
    partial class Group2
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
            this.okButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.newButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupsComboBox = new System.Windows.Forms.ComboBox();
            this.correlationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 135, 134 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point( 231, 134 );
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size( 75, 23 );
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // newButton
            // 
            this.newButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newButton.Location = new System.Drawing.Point( 349, 78 );
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size( 66, 23 );
            this.newButton.TabIndex = 2;
            this.newButton.Text = "New...";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler( this.newButton_Click );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 12, 19 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 132, 13 );
            this.label1.TabIndex = 3;
            this.label1.Text = "Add Variants to Group";
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point( 26, 47 );
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size( 174, 13 );
            this.infoLabel.TabIndex = 4;
            this.infoLabel.Text = "There are ?? items selected to add.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 28, 82 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 89, 13 );
            this.label3.TabIndex = 5;
            this.label3.Text = "Group to Add To:";
            // 
            // groupsComboBox
            // 
            this.groupsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupsComboBox.FormattingEnabled = true;
            this.groupsComboBox.Items.AddRange( new object[] {
            "Group 1",
            "Group 2"} );
            this.groupsComboBox.Location = new System.Drawing.Point( 122, 79 );
            this.groupsComboBox.Name = "groupsComboBox";
            this.groupsComboBox.Size = new System.Drawing.Size( 221, 21 );
            this.groupsComboBox.TabIndex = 6;
            this.groupsComboBox.SelectedIndexChanged += new System.EventHandler( this.groupsComboBox_SelectedIndexChanged );
            // 
            // correlationLabel
            // 
            this.correlationLabel.AutoSize = true;
            this.correlationLabel.Location = new System.Drawing.Point( 28, 107 );
            this.correlationLabel.Name = "correlationLabel";
            this.correlationLabel.Size = new System.Drawing.Size( 33, 13 );
            this.correlationLabel.TabIndex = 7;
            this.correlationLabel.Text = "%corr";
            // 
            // Group2
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size( 441, 169 );
            this.Controls.Add( this.correlationLabel );
            this.Controls.Add( this.groupsComboBox );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.infoLabel );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.newButton );
            this.Controls.Add( this.button2 );
            this.Controls.Add( this.okButton );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Group2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add to Group";
            this.Load += new System.EventHandler( this.Group2_Load );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button newButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox groupsComboBox;
        private System.Windows.Forms.Label correlationLabel;
    }
}