using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for InputDouble.
	/// </summary>
	public class InputDouble : System.Windows.Forms.Form
	{
		public double Value
		{
			get
			{
				return (double) valueBox.Value;
			}

			set
			{
				valueBox.Value = (decimal) value;
			}
		}

		public double Max
		{
			set
			{
				this.valueBox.Maximum = (decimal) value;
			}
		}

		public double Min
		{
			set
			{
				this.valueBox.Minimum = (decimal) value;
			}
		}

		private System.Windows.Forms.NumericUpDown valueBox;
		private System.Windows.Forms.Button okButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InputDouble()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.valueBox = new System.Windows.Forms.NumericUpDown();
			this.okButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.valueBox)).BeginInit();
			this.SuspendLayout();
			// 
			// valueBox
			// 
			this.valueBox.Location = new System.Drawing.Point(8, 8);
			this.valueBox.Maximum = new System.Decimal(new int[] {
																	 2147483647,
																	 0,
																	 0,
																	 0});
			this.valueBox.Minimum = new System.Decimal(new int[] {
																	 -2147483648,
																	 0,
																	 0,
																	 -2147483648});
			this.valueBox.Name = "valueBox";
			this.valueBox.Size = new System.Drawing.Size(104, 20);
			this.valueBox.TabIndex = 0;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(128, 8);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(32, 18);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// InputDouble
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(170, 36);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.valueBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "InputDouble";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Enter Value";
			((System.ComponentModel.ISupportInitialize)(this.valueBox)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion


		private void okButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}
	}
}
