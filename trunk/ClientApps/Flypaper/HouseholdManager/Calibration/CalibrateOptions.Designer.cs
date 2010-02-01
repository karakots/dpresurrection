namespace Calibration
{
    partial class CalibrateOptions
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( CalibrateOptions ) );
            this.cancel_button = new System.Windows.Forms.Button();
            this.TypeCombo = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.OptionsList = new System.Windows.Forms.ListView();
            this.NameColumn = new System.Windows.Forms.ColumnHeader();
            this.IDColumn = new System.Windows.Forms.ColumnHeader();
            this.CurrValueColumn = new System.Windows.Forms.ColumnHeader();
            this.OperatorColumn = new System.Windows.Forms.ColumnHeader();
            this.ModifierColumn = new System.Windows.Forms.ColumnHeader();
            this.NewValueColumn = new System.Windows.Forms.ColumnHeader();
            this.ValueCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.OperatorCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ModifierUpDown = new System.Windows.Forms.NumericUpDown();
            this.toolTips = new System.Windows.Forms.ToolTip( this.components );
            this.UpdateButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ModifierUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // cancel_button
            // 
            this.cancel_button.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cancel_button.Location = new System.Drawing.Point( 528, 393 );
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size( 130, 23 );
            this.cancel_button.TabIndex = 1;
            this.cancel_button.Text = "Undo All Changes";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler( this.cancel_button_Click );
            // 
            // TypeCombo
            // 
            this.TypeCombo.FormattingEnabled = true;
            this.TypeCombo.Location = new System.Drawing.Point( 12, 12 );
            this.TypeCombo.Name = "TypeCombo";
            this.TypeCombo.Size = new System.Drawing.Size( 121, 21 );
            this.TypeCombo.TabIndex = 14;
            this.TypeCombo.SelectedIndexChanged += new System.EventHandler( this.TypeCombo_SelectedIndexChanged );
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point( 139, 15 );
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size( 31, 13 );
            this.label9.TabIndex = 19;
            this.label9.Text = "Type";
            // 
            // OptionsList
            // 
            this.OptionsList.AutoArrange = false;
            this.OptionsList.CheckBoxes = true;
            this.OptionsList.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.IDColumn,
            this.CurrValueColumn,
            this.OperatorColumn,
            this.ModifierColumn,
            this.NewValueColumn} );
            this.OptionsList.FullRowSelect = true;
            this.OptionsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.OptionsList.Location = new System.Drawing.Point( 12, 39 );
            this.OptionsList.Name = "OptionsList";
            this.OptionsList.Size = new System.Drawing.Size( 727, 347 );
            this.OptionsList.TabIndex = 20;
            this.OptionsList.UseCompatibleStateImageBehavior = false;
            this.OptionsList.View = System.Windows.Forms.View.Details;
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "Name";
            this.NameColumn.Width = 210;
            // 
            // IDColumn
            // 
            this.IDColumn.Text = "ID";
            this.IDColumn.Width = 75;
            // 
            // CurrValueColumn
            // 
            this.CurrValueColumn.Text = "Current Value";
            this.CurrValueColumn.Width = 125;
            // 
            // OperatorColumn
            // 
            this.OperatorColumn.Text = "Operator";
            // 
            // ModifierColumn
            // 
            this.ModifierColumn.Text = "Modifier";
            this.ModifierColumn.Width = 125;
            // 
            // NewValueColumn
            // 
            this.NewValueColumn.Text = "New Value";
            this.NewValueColumn.Width = 125;
            // 
            // ValueCombo
            // 
            this.ValueCombo.FormattingEnabled = true;
            this.ValueCombo.Location = new System.Drawing.Point( 176, 12 );
            this.ValueCombo.Name = "ValueCombo";
            this.ValueCombo.Size = new System.Drawing.Size( 121, 21 );
            this.ValueCombo.TabIndex = 21;
            this.ValueCombo.SelectedIndexChanged += new System.EventHandler( this.ValueCombo_SelectedIndexChanged );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 303, 15 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 34, 13 );
            this.label1.TabIndex = 22;
            this.label1.Text = "Value";
            // 
            // SaveButton
            // 
            this.SaveButton.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.SaveButton.Location = new System.Drawing.Point( 682, 393 );
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size( 57, 23 );
            this.SaveButton.TabIndex = 23;
            this.SaveButton.Text = "Save";
            this.toolTips.SetToolTip( this.SaveButton, "Updates checkd media with new values" );
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler( this.applyBut_Click );
            // 
            // OperatorCombo
            // 
            this.OperatorCombo.FormattingEnabled = true;
            this.OperatorCombo.Items.AddRange( new object[] {
            "x",
            "+",
            "="} );
            this.OperatorCombo.Location = new System.Drawing.Point( 343, 12 );
            this.OperatorCombo.Name = "OperatorCombo";
            this.OperatorCombo.Size = new System.Drawing.Size( 48, 21 );
            this.OperatorCombo.TabIndex = 25;
            this.OperatorCombo.SelectedIndexChanged += new System.EventHandler( this.OperatorCombo_SelectedIndexChanged );
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 397, 15 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 48, 13 );
            this.label2.TabIndex = 26;
            this.label2.Text = "Operator";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 577, 17 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 44, 13 );
            this.label3.TabIndex = 28;
            this.label3.Text = "Modifier";
            // 
            // ModifierUpDown
            // 
            this.ModifierUpDown.DecimalPlaces = 5;
            this.ModifierUpDown.Increment = new decimal( new int[] {
            1,
            0,
            0,
            131072} );
            this.ModifierUpDown.Location = new System.Drawing.Point( 451, 12 );
            this.ModifierUpDown.Minimum = new decimal( new int[] {
            100,
            0,
            0,
            -2147483648} );
            this.ModifierUpDown.Name = "ModifierUpDown";
            this.ModifierUpDown.Size = new System.Drawing.Size( 120, 20 );
            this.ModifierUpDown.TabIndex = 29;
            this.ModifierUpDown.Value = new decimal( new int[] {
            10000,
            0,
            0,
            262144} );
            this.ModifierUpDown.ValueChanged += new System.EventHandler( this.ModifierUpDown_ValueChanged );
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point( 664, 10 );
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size( 75, 23 );
            this.UpdateButton.TabIndex = 30;
            this.UpdateButton.Text = "Update";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler( this.UpdateButton_Click );
            // 
            // CalibrateOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 759, 428 );
            this.Controls.Add( this.UpdateButton );
            this.Controls.Add( this.ModifierUpDown );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.OperatorCombo );
            this.Controls.Add( this.SaveButton );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.ValueCombo );
            this.Controls.Add( this.OptionsList );
            this.Controls.Add( this.label9 );
            this.Controls.Add( this.TypeCombo );
            this.Controls.Add( this.cancel_button );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalibrateOptions";
            this.Text = "Media Calibration";
            ((System.ComponentModel.ISupportInitialize)(this.ModifierUpDown)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.ComboBox TypeCombo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListView OptionsList;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.ColumnHeader IDColumn;
        private System.Windows.Forms.ComboBox ValueCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader CurrValueColumn;
        private System.Windows.Forms.ColumnHeader OperatorColumn;
        private System.Windows.Forms.ColumnHeader ModifierColumn;
        private System.Windows.Forms.ColumnHeader NewValueColumn;
        private System.Windows.Forms.Button SaveButton;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.ComboBox OperatorCombo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown ModifierUpDown;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.Button UpdateButton;
    }
}