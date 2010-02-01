using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;

using System.Text.RegularExpressions;





namespace DatabaseManager

{

	/// <summary>

	/// Summary description for NewDb.

	/// </summary>

	public class NewDb : System.Windows.Forms.Form

	{

		private System.Windows.Forms.TextBox dbNameBox;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Button acceptButtoon;

		private System.Windows.Forms.Button cancelButton;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;



		public NewDb()

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDb));
            this.dbNameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.acceptButtoon = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dbNameBox
            // 
            this.dbNameBox.Location = new System.Drawing.Point(128, 8);
            this.dbNameBox.MaxLength = 25;
            this.dbNameBox.Name = "dbNameBox";
            this.dbNameBox.Size = new System.Drawing.Size(232, 20);
            this.dbNameBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Database  Name";
            // 
            // acceptButtoon
            // 
            this.acceptButtoon.Location = new System.Drawing.Point(173, 48);
            this.acceptButtoon.Name = "acceptButtoon";
            this.acceptButtoon.Size = new System.Drawing.Size(187, 23);
            this.acceptButtoon.TabIndex = 2;
            this.acceptButtoon.Text = "Create connection to database...";
            this.acceptButtoon.Click += new System.EventHandler(this.acceptButtoon_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(52, 48);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // NewDb
            // 
            this.AcceptButton = this.acceptButtoon;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(378, 80);
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButtoon);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dbNameBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewDb";
            this.Text = "New ModelDb";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion



		public string DbName

		{

			get

			{

				return this.dbNameBox.Text;

			}

		}



		private void acceptButtoon_Click(object sender, System.EventArgs e)

		{

			if (this.dbNameBox.Text.Length > 0)

			{

				Regex alphaNumericPattern=new Regex("[^a-zA-Z0-9]");



				if (alphaNumericPattern.IsMatch(this.dbNameBox.Text))

				{

					MessageBox.Show("Dababase name should be alpha numeric");

					return;

				}

			}

			else

			{

				MessageBox.Show("Please enter a name for the database");

				return;

			}



			this.DialogResult = DialogResult.OK;

		}



		private void cancelButton_Click(object sender, System.EventArgs e)

		{

			this.DialogResult = DialogResult.Cancel;

		}

	}

}

