namespace Calibration.dialogs
{
    partial class SelectRegion
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
            this.InfoLabel = new System.Windows.Forms.Label();
            this.RegionCombo = new System.Windows.Forms.ComboBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelBut = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point( 12, 9 );
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size( 78, 13 );
            this.InfoLabel.TabIndex = 17;
            this.InfoLabel.Text = "Current Region";
            // 
            // RegionCombo
            // 
            this.RegionCombo.FormattingEnabled = true;
            this.RegionCombo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RegionCombo.Location = new System.Drawing.Point( 12, 43 );
            this.RegionCombo.Name = "RegionCombo";
            this.RegionCombo.Size = new System.Drawing.Size( 208, 21 );
            this.RegionCombo.TabIndex = 16;
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point( 145, 70 );
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size( 75, 23 );
            this.OkButton.TabIndex = 15;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // CancelBut
            // 
            this.CancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBut.Location = new System.Drawing.Point( 64, 70 );
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size( 75, 23 );
            this.CancelBut.TabIndex = 14;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 12, 27 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 102, 13 );
            this.label1.TabIndex = 18;
            this.label1.Text = "Select New Region:";
            // 
            // SelectRegion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 228, 106 );
            this.ControlBox = false;
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.InfoLabel );
            this.Controls.Add( this.RegionCombo );
            this.Controls.Add( this.OkButton );
            this.Controls.Add( this.CancelBut );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SelectRegion";
            this.Text = "Select New Region";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.ComboBox RegionCombo;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelBut;
        private System.Windows.Forms.Label label1;
    }
}