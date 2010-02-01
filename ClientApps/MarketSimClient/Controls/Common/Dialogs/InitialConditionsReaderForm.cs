using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for InitialConditionsReaderForm.
	/// </summary>
	public class InitialConditionsReaderForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox ValueTypeBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Button CanButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InitialConditionsReaderForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			ValueTypeBox.SelectedIndex = 0;
		}

		public int ValueType
		{
			get
			{
				return ValueTypeBox.SelectedIndex;
			}
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
			this.ValueTypeBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.CanButton = new System.Windows.Forms.Button();
			this.OKButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ValueTypeBox
			// 
			this.ValueTypeBox.Items.AddRange(new object[] {
															  "All",
															  "Initial Share",
															  "Initial Penetration",
															  "Initial Persuasion",
															  "Initial Awareness"});
			this.ValueTypeBox.Location = new System.Drawing.Point(16, 32);
			this.ValueTypeBox.Name = "ValueTypeBox";
			this.ValueTypeBox.Size = new System.Drawing.Size(224, 21);
			this.ValueTypeBox.TabIndex = 0;
			this.ValueTypeBox.Text = "Select...";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(176, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Which initial condition(s):";
			// 
			// CanButton
			// 
			this.CanButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CanButton.Location = new System.Drawing.Point(80, 72);
			this.CanButton.Name = "CanButton";
			this.CanButton.TabIndex = 2;
			this.CanButton.Text = "Cancel";
			// 
			// OKButton
			// 
			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKButton.Location = new System.Drawing.Point(168, 72);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(72, 23);
			this.OKButton.TabIndex = 3;
			this.OKButton.Text = "OK";
			// 
			// InitialConditionsReaderForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(256, 110);
			this.ControlBox = false;
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.CanButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ValueTypeBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "InitialConditionsReaderForm";
			this.Text = "Initial Condition Reader";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
