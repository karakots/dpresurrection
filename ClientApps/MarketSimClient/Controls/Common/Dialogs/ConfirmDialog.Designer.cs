namespace Common.Dialogs
{
    partial class ConfirmDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ConfirmDialog ) );
            this.yesButton = new System.Windows.Forms.Button();
            this.noButton = new System.Windows.Forms.Button();
            this.mainLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.typeLabel = new System.Windows.Forms.Label();
            this.warningPictureBox = new System.Windows.Forms.PictureBox();
            this.errorPictureBox = new System.Windows.Forms.PictureBox();
            this.infoPictureBox = new System.Windows.Forms.PictureBox();
            this.questionPictureBox = new System.Windows.Forms.PictureBox();
            this.helpButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.warningPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.questionPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // yesButton
            // 
            this.yesButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.yesButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.yesButton.Location = new System.Drawing.Point( 187, 105 );
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size( 75, 23 );
            this.yesButton.TabIndex = 0;
            this.yesButton.Text = "Yes";
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // noButton
            // 
            this.noButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.noButton.DialogResult = System.Windows.Forms.DialogResult.No;
            this.noButton.Location = new System.Drawing.Point( 277, 105 );
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size( 75, 23 );
            this.noButton.TabIndex = 1;
            this.noButton.Text = "No";
            this.noButton.UseVisualStyleBackColor = true;
            // 
            // mainLabel
            // 
            this.mainLabel.AutoSize = true;
            this.mainLabel.Location = new System.Drawing.Point( 89, 28 );
            this.mainLabel.Name = "mainLabel";
            this.mainLabel.Size = new System.Drawing.Size( 387, 15 );
            this.mainLabel.TabIndex = 2;
            this.mainLabel.Text = "Are you really really  sure that you want to do something to this object?";
            // 
            // nameLabel
            // 
            this.nameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font( "Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.nameLabel.Location = new System.Drawing.Point( 224, 54 );
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size( 163, 16 );
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "Object NamGoes Here";
            // 
            // typeLabel
            // 
            this.typeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.typeLabel.Location = new System.Drawing.Point( 64, 55 );
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size( 157, 15 );
            this.typeLabel.TabIndex = 4;
            this.typeLabel.Text = "Object Type:";
            this.typeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // warningPictureBox
            // 
            this.warningPictureBox.ErrorImage = ((System.Drawing.Image)(resources.GetObject( "warningPictureBox.ErrorImage" )));
            this.warningPictureBox.Image = ((System.Drawing.Image)(resources.GetObject( "warningPictureBox.Image" )));
            this.warningPictureBox.InitialImage = ((System.Drawing.Image)(resources.GetObject( "warningPictureBox.InitialImage" )));
            this.warningPictureBox.Location = new System.Drawing.Point( 23, 19 );
            this.warningPictureBox.Name = "warningPictureBox";
            this.warningPictureBox.Size = new System.Drawing.Size( 32, 32 );
            this.warningPictureBox.TabIndex = 5;
            this.warningPictureBox.TabStop = false;
            // 
            // errorPictureBox
            // 
            this.errorPictureBox.ErrorImage = ((System.Drawing.Image)(resources.GetObject( "errorPictureBox.ErrorImage" )));
            this.errorPictureBox.Image = ((System.Drawing.Image)(resources.GetObject( "errorPictureBox.Image" )));
            this.errorPictureBox.InitialImage = ((System.Drawing.Image)(resources.GetObject( "errorPictureBox.InitialImage" )));
            this.errorPictureBox.Location = new System.Drawing.Point( 23, 19 );
            this.errorPictureBox.Name = "errorPictureBox";
            this.errorPictureBox.Size = new System.Drawing.Size( 32, 32 );
            this.errorPictureBox.TabIndex = 6;
            this.errorPictureBox.TabStop = false;
            this.errorPictureBox.Visible = false;
            // 
            // infoPictureBox
            // 
            this.infoPictureBox.ErrorImage = ((System.Drawing.Image)(resources.GetObject( "infoPictureBox.ErrorImage" )));
            this.infoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject( "infoPictureBox.Image" )));
            this.infoPictureBox.InitialImage = ((System.Drawing.Image)(resources.GetObject( "infoPictureBox.InitialImage" )));
            this.infoPictureBox.Location = new System.Drawing.Point( 23, 19 );
            this.infoPictureBox.Name = "infoPictureBox";
            this.infoPictureBox.Size = new System.Drawing.Size( 32, 32 );
            this.infoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.infoPictureBox.TabIndex = 7;
            this.infoPictureBox.TabStop = false;
            this.infoPictureBox.Visible = false;
            // 
            // questionPictureBox
            // 
            this.questionPictureBox.ErrorImage = ((System.Drawing.Image)(resources.GetObject( "questionPictureBox.ErrorImage" )));
            this.questionPictureBox.Image = ((System.Drawing.Image)(resources.GetObject( "questionPictureBox.Image" )));
            this.questionPictureBox.InitialImage = ((System.Drawing.Image)(resources.GetObject( "questionPictureBox.InitialImage" )));
            this.questionPictureBox.Location = new System.Drawing.Point( 23, 19 );
            this.questionPictureBox.Name = "questionPictureBox";
            this.questionPictureBox.Size = new System.Drawing.Size( 32, 32 );
            this.questionPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.questionPictureBox.TabIndex = 8;
            this.questionPictureBox.TabStop = false;
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.BackColor = System.Drawing.SystemColors.Control;
            this.helpButton.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.helpButton.Location = new System.Drawing.Point( 539, 3 );
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size( 24, 21 );
            this.helpButton.TabIndex = 13;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Visible = false;
            this.helpButton.Click += new System.EventHandler( this.helpButton_Click );
            // 
            // ConfirmDialog
            // 
            this.AcceptButton = this.yesButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.noButton;
            this.ClientSize = new System.Drawing.Size( 565, 140 );
            this.Controls.Add( this.noButton );
            this.Controls.Add( this.yesButton );
            this.Controls.Add( this.helpButton );
            this.Controls.Add( this.warningPictureBox );
            this.Controls.Add( this.infoPictureBox );
            this.Controls.Add( this.errorPictureBox );
            this.Controls.Add( this.nameLabel );
            this.Controls.Add( this.mainLabel );
            this.Controls.Add( this.questionPictureBox );
            this.Controls.Add( this.typeLabel );
            this.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ConfirmDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ConfirmDialog";
            ((System.ComponentModel.ISupportInitialize)(this.warningPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.questionPictureBox)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button noButton;
        private System.Windows.Forms.Label mainLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.PictureBox warningPictureBox;
        private System.Windows.Forms.PictureBox errorPictureBox;
        private System.Windows.Forms.PictureBox infoPictureBox;
        private System.Windows.Forms.PictureBox questionPictureBox;
        private System.Windows.Forms.Button helpButton;
    }
}