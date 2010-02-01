namespace Calibration.dialogs
{
    partial class GetString
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
            this.InfoLabel = new System.Windows.Forms.Label();
            this.ValueTypeLabel = new System.Windows.Forms.Label();
            this.StringValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point( 152, 53 );
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size( 75, 23 );
            this.OkButton.TabIndex = 9;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // CancelBut
            // 
            this.CancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBut.Location = new System.Drawing.Point( 71, 53 );
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size( 75, 23 );
            this.CancelBut.TabIndex = 8;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point( 10, 11 );
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size( 79, 13 );
            this.InfoLabel.TabIndex = 7;
            this.InfoLabel.Text = "Get New Value";
            // 
            // ValueTypeLabel
            // 
            this.ValueTypeLabel.AutoSize = true;
            this.ValueTypeLabel.Location = new System.Drawing.Point( 11, 29 );
            this.ValueTypeLabel.Name = "ValueTypeLabel";
            this.ValueTypeLabel.Size = new System.Drawing.Size( 65, 13 );
            this.ValueTypeLabel.TabIndex = 5;
            this.ValueTypeLabel.Text = "Enter Value:";
            // 
            // StringValue
            // 
            this.StringValue.Location = new System.Drawing.Point( 82, 27 );
            this.StringValue.Name = "StringValue";
            this.StringValue.Size = new System.Drawing.Size( 144, 20 );
            this.StringValue.TabIndex = 10;
            // 
            // GetString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 238, 91 );
            this.ControlBox = false;
            this.Controls.Add( this.StringValue );
            this.Controls.Add( this.OkButton );
            this.Controls.Add( this.CancelBut );
            this.Controls.Add( this.InfoLabel );
            this.Controls.Add( this.ValueTypeLabel );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "GetString";
            this.Text = "GetString";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelBut;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Label ValueTypeLabel;
        private System.Windows.Forms.TextBox StringValue;
    }
}