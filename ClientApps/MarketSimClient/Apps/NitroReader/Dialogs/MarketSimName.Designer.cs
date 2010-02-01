namespace NitroReader.Dialogs
{
    partial class MarketSimName
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
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButtom = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nitroNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point( 104, 35 );
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size( 354, 20 );
            this.nameTextBox.TabIndex = 0;
            this.nameTextBox.TextChanged += new System.EventHandler( this.nameTextBox_TextChanged );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 8, 11 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 75, 13 );
            this.label1.TabIndex = 1;
            this.label1.Text = "NITRO Name:";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 153, 68 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // cancelButtom
            // 
            this.cancelButtom.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButtom.Location = new System.Drawing.Point( 252, 68 );
            this.cancelButtom.Name = "cancelButtom";
            this.cancelButtom.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButtom.TabIndex = 3;
            this.cancelButtom.Text = "Cancel";
            this.cancelButtom.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 8, 38 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 91, 13 );
            this.label2.TabIndex = 4;
            this.label2.Text = "MarketSim Name:";
            // 
            // nitroNameLabel
            // 
            this.nitroNameLabel.AutoSize = true;
            this.nitroNameLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.nitroNameLabel.Location = new System.Drawing.Point( 101, 11 );
            this.nitroNameLabel.Name = "nitroNameLabel";
            this.nitroNameLabel.Size = new System.Drawing.Size( 82, 13 );
            this.nitroNameLabel.TabIndex = 5;
            this.nitroNameLabel.Text = "NITRO Name";
            // 
            // MarketSimName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 483, 100 );
            this.Controls.Add( this.nitroNameLabel );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.cancelButtom );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.nameTextBox );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MarketSimName";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Product Name for MarketSim";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButtom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label nitroNameLabel;
    }
}