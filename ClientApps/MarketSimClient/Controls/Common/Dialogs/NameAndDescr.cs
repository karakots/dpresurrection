using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Common.Dialogs
{
	/// <summary>
	/// Name and describe a NewObject.
	/// </summary>
	public class NameAndDescr : System.Windows.Forms.Form
	{
		private string typeString = null;

		public string Type
		{
			get
			{
				return typeString;
			}

			set
			{
				typeString = value;

				this.Text = "New " + Type;
			}
		}

		public string ObjName
		{
			get
			{
				return this.nameBox.Text;
			}

			set
			{
				nameBox.Text = value;
			}
		}

		public string ObjDescription
		{
			get
			{
				return this.descrBox.Text;
			}

			set
			{
				descrBox.Text = value;
			}
		}

		public bool Editing
		{
			set
			{
				if (value)
				{
					this.Text = "Edit " + Type;
				}
				else
				{
					this.Text = "New " + Type;
				}
			}
		}

		private System.Windows.Forms.TextBox nameBox;
		private System.Windows.Forms.TextBox descrBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NameAndDescr()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.Text = "New " + Type;
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
			this.nameBox = new System.Windows.Forms.TextBox();
			this.descrBox = new System.Windows.Forms.TextBox();
			this.label = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.acceptButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// nameBox
			// 
			this.nameBox.Location = new System.Drawing.Point(104, 16);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(152, 20);
			this.nameBox.TabIndex = 0;
			this.nameBox.Text = "";
			// 
			// descrBox
			// 
			this.descrBox.Location = new System.Drawing.Point(104, 56);
			this.descrBox.Multiline = true;
			this.descrBox.Name = "descrBox";
			this.descrBox.Size = new System.Drawing.Size(152, 96);
			this.descrBox.TabIndex = 1;
			this.descrBox.Text = "";
			// 
			// label
			// 
			this.label.Location = new System.Drawing.Point(16, 16);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(48, 23);
			this.label.TabIndex = 2;
			this.label.Text = "Name";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "Description";
			// 
			// acceptButton
			// 
			this.acceptButton.Location = new System.Drawing.Point(40, 176);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.TabIndex = 4;
			this.acceptButton.Text = "Accept";
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(176, 176);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// NameAndDescr
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 222);
			this.ControlBox = false;
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label);
			this.Controls.Add(this.descrBox);
			this.Controls.Add(this.nameBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "NameAndDescr";
			this.Text = "New  Project";
			this.ResumeLayout(false);

		}
		#endregion

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void acceptButton_Click(object sender, System.EventArgs e)
		{
			// validate name

			char[] illegal = {',', '\'', '"', '.', ';'};

			int index = nameBox.Text.IndexOfAny(illegal);

			if ( index < 0 )
				this.DialogResult = DialogResult.OK;
			else
				MessageBox.Show(this, "Illegal characters in name.");
		}
	}
}
