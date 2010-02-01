namespace Common.Dialogs
{
    partial class CompletionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( CompletionDialog ) );
            this.okButton = new System.Windows.Forms.Button();
            this.messageLabel = new System.Windows.Forms.Label();
            this.checkMarkPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.checkMarkPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 143, 61 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // messageLabel
            // 
            this.messageLabel.Location = new System.Drawing.Point( 54, 18 );
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size( 248, 32 );
            this.messageLabel.TabIndex = 1;
            this.messageLabel.Text = "messageLabel";
            this.messageLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // checkMarkPictureBox
            // 
            this.checkMarkPictureBox.Image = ((System.Drawing.Image)(resources.GetObject( "checkMarkPictureBox.Image" )));
            this.checkMarkPictureBox.Location = new System.Drawing.Point( 16, 16 );
            this.checkMarkPictureBox.Name = "checkMarkPictureBox";
            this.checkMarkPictureBox.Size = new System.Drawing.Size( 38, 38 );
            this.checkMarkPictureBox.TabIndex = 2;
            this.checkMarkPictureBox.TabStop = false;
            // 
            // CompletionDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size( 358, 96 );
            this.Controls.Add( this.checkMarkPictureBox );
            this.Controls.Add( this.messageLabel );
            this.Controls.Add( this.okButton );
            this.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CompletionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.checkMarkPictureBox)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.PictureBox checkMarkPictureBox;
    }
}