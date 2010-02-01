using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MarketSimUtilities
{
	/// <summary>
	/// Summary description for InputString.
	/// </summary>
	public class InputString : System.Windows.Forms.Form
	{
		public string InputText
		{
			get
			{
				return this.textBox1.Text;
			}

            set
            {
                this.textBox1.Text = value;
            }
		}



		private System.Windows.Forms.TextBox textBox1;
        private Button OKButton;
        private Button cancelBut;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InputString()
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.cancelBut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.textBox1.Location = new System.Drawing.Point( 0, 0 );
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size( 433, 22 );
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Enter expression :         % is  Wildcard";
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.Location = new System.Drawing.Point( 362, 31 );
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size( 57, 23 );
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler( this.OKButton_Click );
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.Location = new System.Drawing.Point( 288, 31 );
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size( 57, 23 );
            this.cancelBut.TabIndex = 2;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            this.cancelBut.Click += new System.EventHandler( this.cancelBut_Click );
            // 
            // InputString
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.ClientSize = new System.Drawing.Size( 433, 62 );
            this.ControlBox = false;
            this.Controls.Add( this.cancelBut );
            this.Controls.Add( this.OKButton );
            this.Controls.Add( this.textBox1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size( 1000, 100 );
            this.MinimumSize = new System.Drawing.Size( 300, 45 );
            this.Name = "InputString";
            this.Text = "Enter Expression to Match";
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void cancelBut_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
        }
	}
}
