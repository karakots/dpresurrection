namespace TestScenarioManager
{
    partial class DataForm
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
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.typeLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.startDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.endDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.valueNumLabel = new System.Windows.Forms.Label();
            this.awareLabel = new System.Windows.Forms.Label();
            this.persLabel = new System.Windows.Forms.Label();
            this.persuasionTextBox = new System.Windows.Forms.TextBox();
            this.awarenessTextBox = new System.Windows.Forms.TextBox();
            this.distLabel = new System.Windows.Forms.Label();
            this.priceLabel = new System.Windows.Forms.Label();
            this.priceTypeLabel = new System.Windows.Forms.Label();
            this.grpsLabel = new System.Windows.Forms.Label();
            this.mediaTypeLabel = new System.Windows.Forms.Label();
            this.marketUtilLabel = new System.Windows.Forms.Label();
            this.distTextBox = new System.Windows.Forms.TextBox();
            this.priceTextBox = new System.Windows.Forms.TextBox();
            this.ptypeTextBox = new System.Windows.Forms.TextBox();
            this.grpsTextBox = new System.Windows.Forms.TextBox();
            this.mtypeTextBox = new System.Windows.Forms.TextBox();
            this.utilityTextBox = new System.Windows.Forms.TextBox();
            this.prevButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.productLabel = new System.Windows.Forms.Label();
            this.postUseUtilLabel = new System.Windows.Forms.Label();
            this.postUseUtilityTextBox = new System.Windows.Forms.TextBox();
            this.displayTextBox = new System.Windows.Forms.TextBox();
            this.displayLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Location = new System.Drawing.Point( 19, 351 );
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size( 133, 23 );
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save These Values";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler( this.saveButton_Click );
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 301, 351 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Close";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 16, 14 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 88, 13 );
            this.label1.TabIndex = 2;
            this.label1.Text = "Component Type";
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point( 110, 14 );
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size( 63, 13 );
            this.typeLabel.TabIndex = 3;
            this.typeLabel.Text = "<type here>";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 16, 86 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 32, 13 );
            this.label3.TabIndex = 4;
            this.label3.Text = "Start:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 19, 115 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 29, 13 );
            this.label4.TabIndex = 5;
            this.label4.Text = "End:";
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.Location = new System.Drawing.Point( 54, 81 );
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new System.Drawing.Size( 200, 20 );
            this.startDateTimePicker.TabIndex = 6;
            // 
            // endDateTimePicker
            // 
            this.endDateTimePicker.Location = new System.Drawing.Point( 55, 110 );
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.Size = new System.Drawing.Size( 200, 20 );
            this.endDateTimePicker.TabIndex = 7;
            // 
            // valueNumLabel
            // 
            this.valueNumLabel.AutoSize = true;
            this.valueNumLabel.Location = new System.Drawing.Point( 16, 36 );
            this.valueNumLabel.Name = "valueNumLabel";
            this.valueNumLabel.Size = new System.Drawing.Size( 64, 13 );
            this.valueNumLabel.TabIndex = 8;
            this.valueNumLabel.Text = "Value -  of 0";
            // 
            // awareLabel
            // 
            this.awareLabel.AutoSize = true;
            this.awareLabel.Location = new System.Drawing.Point( 19, 148 );
            this.awareLabel.Name = "awareLabel";
            this.awareLabel.Size = new System.Drawing.Size( 62, 13 );
            this.awareLabel.TabIndex = 9;
            this.awareLabel.Text = "Awareness:";
            // 
            // persLabel
            // 
            this.persLabel.AutoSize = true;
            this.persLabel.Location = new System.Drawing.Point( 159, 148 );
            this.persLabel.Name = "persLabel";
            this.persLabel.Size = new System.Drawing.Size( 62, 13 );
            this.persLabel.TabIndex = 10;
            this.persLabel.Text = "Persuasion:";
            // 
            // persuasionTextBox
            // 
            this.persuasionTextBox.Location = new System.Drawing.Point( 228, 144 );
            this.persuasionTextBox.Name = "persuasionTextBox";
            this.persuasionTextBox.Size = new System.Drawing.Size( 64, 20 );
            this.persuasionTextBox.TabIndex = 11;
            // 
            // awarenessTextBox
            // 
            this.awarenessTextBox.Location = new System.Drawing.Point( 90, 143 );
            this.awarenessTextBox.Name = "awarenessTextBox";
            this.awarenessTextBox.Size = new System.Drawing.Size( 62, 20 );
            this.awarenessTextBox.TabIndex = 12;
            // 
            // distLabel
            // 
            this.distLabel.AutoSize = true;
            this.distLabel.Location = new System.Drawing.Point( 19, 179 );
            this.distLabel.Name = "distLabel";
            this.distLabel.Size = new System.Drawing.Size( 62, 13 );
            this.distLabel.TabIndex = 13;
            this.distLabel.Text = "Distribution:";
            // 
            // priceLabel
            // 
            this.priceLabel.AutoSize = true;
            this.priceLabel.Location = new System.Drawing.Point( 19, 213 );
            this.priceLabel.Name = "priceLabel";
            this.priceLabel.Size = new System.Drawing.Size( 34, 13 );
            this.priceLabel.TabIndex = 14;
            this.priceLabel.Text = "Price:";
            // 
            // priceTypeLabel
            // 
            this.priceTypeLabel.AutoSize = true;
            this.priceTypeLabel.Location = new System.Drawing.Point( 126, 212 );
            this.priceTypeLabel.Name = "priceTypeLabel";
            this.priceTypeLabel.Size = new System.Drawing.Size( 61, 13 );
            this.priceTypeLabel.TabIndex = 15;
            this.priceTypeLabel.Text = "Price Type:";
            // 
            // grpsLabel
            // 
            this.grpsLabel.AutoSize = true;
            this.grpsLabel.Location = new System.Drawing.Point( 19, 247 );
            this.grpsLabel.Name = "grpsLabel";
            this.grpsLabel.Size = new System.Drawing.Size( 38, 13 );
            this.grpsLabel.TabIndex = 16;
            this.grpsLabel.Text = "GRPs:";
            // 
            // mediaTypeLabel
            // 
            this.mediaTypeLabel.AutoSize = true;
            this.mediaTypeLabel.Location = new System.Drawing.Point( 137, 246 );
            this.mediaTypeLabel.Name = "mediaTypeLabel";
            this.mediaTypeLabel.Size = new System.Drawing.Size( 63, 13 );
            this.mediaTypeLabel.TabIndex = 17;
            this.mediaTypeLabel.Text = "Media Type";
            // 
            // marketUtilLabel
            // 
            this.marketUtilLabel.AutoSize = true;
            this.marketUtilLabel.Location = new System.Drawing.Point( 19, 278 );
            this.marketUtilLabel.Name = "marketUtilLabel";
            this.marketUtilLabel.Size = new System.Drawing.Size( 71, 13 );
            this.marketUtilLabel.TabIndex = 18;
            this.marketUtilLabel.Text = "Market Utility:";
            // 
            // distTextBox
            // 
            this.distTextBox.Location = new System.Drawing.Point( 90, 175 );
            this.distTextBox.Name = "distTextBox";
            this.distTextBox.Size = new System.Drawing.Size( 62, 20 );
            this.distTextBox.TabIndex = 19;
            // 
            // priceTextBox
            // 
            this.priceTextBox.Location = new System.Drawing.Point( 59, 209 );
            this.priceTextBox.Name = "priceTextBox";
            this.priceTextBox.Size = new System.Drawing.Size( 62, 20 );
            this.priceTextBox.TabIndex = 20;
            // 
            // ptypeTextBox
            // 
            this.ptypeTextBox.Location = new System.Drawing.Point( 189, 209 );
            this.ptypeTextBox.Name = "ptypeTextBox";
            this.ptypeTextBox.Size = new System.Drawing.Size( 77, 20 );
            this.ptypeTextBox.TabIndex = 21;
            // 
            // grpsTextBox
            // 
            this.grpsTextBox.Location = new System.Drawing.Point( 64, 243 );
            this.grpsTextBox.Name = "grpsTextBox";
            this.grpsTextBox.Size = new System.Drawing.Size( 62, 20 );
            this.grpsTextBox.TabIndex = 22;
            // 
            // mtypeTextBox
            // 
            this.mtypeTextBox.Location = new System.Drawing.Point( 208, 242 );
            this.mtypeTextBox.Name = "mtypeTextBox";
            this.mtypeTextBox.Size = new System.Drawing.Size( 64, 20 );
            this.mtypeTextBox.TabIndex = 23;
            // 
            // utilityTextBox
            // 
            this.utilityTextBox.Location = new System.Drawing.Point( 90, 272 );
            this.utilityTextBox.Name = "utilityTextBox";
            this.utilityTextBox.Size = new System.Drawing.Size( 62, 20 );
            this.utilityTextBox.TabIndex = 24;
            // 
            // prevButton
            // 
            this.prevButton.Location = new System.Drawing.Point( 310, 24 );
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size( 58, 23 );
            this.prevButton.TabIndex = 25;
            this.prevButton.Text = "Prev";
            this.prevButton.UseVisualStyleBackColor = true;
            this.prevButton.Click += new System.EventHandler( this.prevButton_Click );
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point( 310, 48 );
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size( 58, 23 );
            this.nextButton.TabIndex = 26;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler( this.nextButton_Click );
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 17, 58 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 47, 13 );
            this.label5.TabIndex = 27;
            this.label5.Text = "Product:";
            // 
            // productLabel
            // 
            this.productLabel.AutoSize = true;
            this.productLabel.Location = new System.Drawing.Point( 67, 58 );
            this.productLabel.Name = "productLabel";
            this.productLabel.Size = new System.Drawing.Size( 69, 13 );
            this.productLabel.TabIndex = 28;
            this.productLabel.Text = "<prod value>";
            // 
            // postUseUtilLabel
            // 
            this.postUseUtilLabel.AutoSize = true;
            this.postUseUtilLabel.Location = new System.Drawing.Point( 159, 178 );
            this.postUseUtilLabel.Name = "postUseUtilLabel";
            this.postUseUtilLabel.Size = new System.Drawing.Size( 81, 13 );
            this.postUseUtilLabel.TabIndex = 29;
            this.postUseUtilLabel.Text = "Post-Use Utility:";
            // 
            // postUseUtilityTextBox
            // 
            this.postUseUtilityTextBox.Location = new System.Drawing.Point( 242, 175 );
            this.postUseUtilityTextBox.Name = "postUseUtilityTextBox";
            this.postUseUtilityTextBox.Size = new System.Drawing.Size( 62, 20 );
            this.postUseUtilityTextBox.TabIndex = 30;
            // 
            // displayTextBox
            // 
            this.displayTextBox.Location = new System.Drawing.Point( 90, 298 );
            this.displayTextBox.Name = "displayTextBox";
            this.displayTextBox.Size = new System.Drawing.Size( 62, 20 );
            this.displayTextBox.TabIndex = 32;
            // 
            // displayLabel
            // 
            this.displayLabel.AutoSize = true;
            this.displayLabel.Location = new System.Drawing.Point( 19, 304 );
            this.displayLabel.Name = "displayLabel";
            this.displayLabel.Size = new System.Drawing.Size( 44, 13 );
            this.displayLabel.TabIndex = 31;
            this.displayLabel.Text = "Display:";
            // 
            // DataForm
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size( 380, 379 );
            this.Controls.Add( this.displayTextBox );
            this.Controls.Add( this.displayLabel );
            this.Controls.Add( this.postUseUtilityTextBox );
            this.Controls.Add( this.postUseUtilLabel );
            this.Controls.Add( this.productLabel );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.nextButton );
            this.Controls.Add( this.prevButton );
            this.Controls.Add( this.utilityTextBox );
            this.Controls.Add( this.mtypeTextBox );
            this.Controls.Add( this.grpsTextBox );
            this.Controls.Add( this.ptypeTextBox );
            this.Controls.Add( this.priceTextBox );
            this.Controls.Add( this.distTextBox );
            this.Controls.Add( this.marketUtilLabel );
            this.Controls.Add( this.mediaTypeLabel );
            this.Controls.Add( this.grpsLabel );
            this.Controls.Add( this.priceTypeLabel );
            this.Controls.Add( this.priceLabel );
            this.Controls.Add( this.distLabel );
            this.Controls.Add( this.awarenessTextBox );
            this.Controls.Add( this.persuasionTextBox );
            this.Controls.Add( this.persLabel );
            this.Controls.Add( this.awareLabel );
            this.Controls.Add( this.valueNumLabel );
            this.Controls.Add( this.endDateTimePicker );
            this.Controls.Add( this.startDateTimePicker );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.typeLabel );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.saveButton );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Component Data";
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker startDateTimePicker;
        private System.Windows.Forms.DateTimePicker endDateTimePicker;
        private System.Windows.Forms.Label valueNumLabel;
        private System.Windows.Forms.Label awareLabel;
        private System.Windows.Forms.Label persLabel;
        private System.Windows.Forms.TextBox persuasionTextBox;
        private System.Windows.Forms.TextBox awarenessTextBox;
        private System.Windows.Forms.Label distLabel;
        private System.Windows.Forms.Label priceLabel;
        private System.Windows.Forms.Label priceTypeLabel;
        private System.Windows.Forms.Label grpsLabel;
        private System.Windows.Forms.Label mediaTypeLabel;
        private System.Windows.Forms.Label marketUtilLabel;
        private System.Windows.Forms.TextBox distTextBox;
        private System.Windows.Forms.TextBox priceTextBox;
        private System.Windows.Forms.TextBox ptypeTextBox;
        private System.Windows.Forms.TextBox grpsTextBox;
        private System.Windows.Forms.TextBox mtypeTextBox;
        private System.Windows.Forms.TextBox utilityTextBox;
        private System.Windows.Forms.Button prevButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label productLabel;
        private System.Windows.Forms.Label postUseUtilLabel;
        private System.Windows.Forms.TextBox postUseUtilityTextBox;
        private System.Windows.Forms.TextBox displayTextBox;
        private System.Windows.Forms.Label displayLabel;
    }
}