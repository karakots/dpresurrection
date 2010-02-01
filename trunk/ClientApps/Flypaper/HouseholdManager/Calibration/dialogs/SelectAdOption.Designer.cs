namespace Calibration.dialogs
{
    partial class SelectAdOption
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelBut = new System.Windows.Forms.Button();
            this.OptionCombo = new System.Windows.Forms.ComboBox();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point( 145, 52 );
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size( 75, 23 );
            this.OkButton.TabIndex = 11;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // CancelBut
            // 
            this.CancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBut.Location = new System.Drawing.Point( 64, 52 );
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size( 75, 23 );
            this.CancelBut.TabIndex = 10;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            // 
            // OptionCombo
            // 
            this.OptionCombo.FormattingEnabled = true;
            this.OptionCombo.Location = new System.Drawing.Point( 12, 25 );
            this.OptionCombo.Name = "OptionCombo";
            this.OptionCombo.Size = new System.Drawing.Size( 208, 21 );
            this.OptionCombo.TabIndex = 12;
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point( 12, 9 );
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size( 87, 13 );
            this.InfoLabel.TabIndex = 13;
            this.InfoLabel.Text = "Select Ad Option";
            // 
            // SelectAdOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 229, 92 );
            this.ControlBox = false;
            this.Controls.Add( this.InfoLabel );
            this.Controls.Add( this.OptionCombo );
            this.Controls.Add( this.OkButton );
            this.Controls.Add( this.CancelBut );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SelectAdOption";
            this.Text = "SelectAdOption";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelBut;
        private System.Windows.Forms.ComboBox OptionCombo;
        private System.Windows.Forms.Label InfoLabel;
    }
}