namespace Calibration.dialogs
{
    partial class GetDouble
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
            this.ValueTypeLabel = new System.Windows.Forms.Label();
            this.DoubleValue = new System.Windows.Forms.NumericUpDown();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.CancelBut = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DoubleValue)).BeginInit();
            this.SuspendLayout();
            // 
            // ValueTypeLabel
            // 
            this.ValueTypeLabel.AutoSize = true;
            this.ValueTypeLabel.Location = new System.Drawing.Point( 10, 27 );
            this.ValueTypeLabel.Name = "ValueTypeLabel";
            this.ValueTypeLabel.Size = new System.Drawing.Size( 65, 13 );
            this.ValueTypeLabel.TabIndex = 0;
            this.ValueTypeLabel.Text = "Enter Value:";
            // 
            // DoubleValue
            // 
            this.DoubleValue.DecimalPlaces = 5;
            this.DoubleValue.Location = new System.Drawing.Point( 81, 25 );
            this.DoubleValue.Name = "DoubleValue";
            this.DoubleValue.Size = new System.Drawing.Size( 145, 20 );
            this.DoubleValue.TabIndex = 1;
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point( 9, 9 );
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size( 79, 13 );
            this.InfoLabel.TabIndex = 2;
            this.InfoLabel.Text = "Get New Value";
            // 
            // CancelBut
            // 
            this.CancelBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBut.Location = new System.Drawing.Point( 70, 51 );
            this.CancelBut.Name = "CancelBut";
            this.CancelBut.Size = new System.Drawing.Size( 75, 23 );
            this.CancelBut.TabIndex = 3;
            this.CancelBut.Text = "Cancel";
            this.CancelBut.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point( 151, 51 );
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size( 75, 23 );
            this.OkButton.TabIndex = 4;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // GetDouble
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 235, 82 );
            this.ControlBox = false;
            this.Controls.Add( this.OkButton );
            this.Controls.Add( this.CancelBut );
            this.Controls.Add( this.InfoLabel );
            this.Controls.Add( this.DoubleValue );
            this.Controls.Add( this.ValueTypeLabel );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "GetDouble";
            this.Text = "GetDouble";
            ((System.ComponentModel.ISupportInitialize)(this.DoubleValue)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ValueTypeLabel;
        private System.Windows.Forms.NumericUpDown DoubleValue;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Button CancelBut;
        private System.Windows.Forms.Button OkButton;
    }
}