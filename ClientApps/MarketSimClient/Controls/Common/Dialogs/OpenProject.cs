using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;

using System.Data;



using MrktSimDb;



namespace Common.Dialogs

{

	/// <summary>

	/// Summary description for OpenProject.

	/// </summary>

	public class OpenProject : System.Windows.Forms.Form

	{

		ProjectDb theDb;



		public ProjectDb Db 

		{

			set

			{

				theDb = value;



				projectView.Table = value.Data.project;

				projectList.DisplayMember = "name";

				projectList.Refresh();



				descrBox.DataBindings.Add("Text", projectView, "descr");

			}

		}



		public int ProjectID

		{

			get

			{

				if (projectList.SelectedItem == null)

					return MrktSimDb.Database.AllID;



				MrktSimDBSchema.projectRow row = (MrktSimDBSchema.projectRow)

					((DataRowView) projectList.SelectedItem).Row;



				return row.id;

			}

		}

		private System.Windows.Forms.Button acceptButton;

		private System.Data.DataView projectView;

		private System.Windows.Forms.ListBox projectList;

		private System.Windows.Forms.Button newButton;

		private System.Windows.Forms.Button button2;

		private System.Windows.Forms.TextBox descrBox;

		private System.Windows.Forms.Button editButton;

		private System.Windows.Forms.Button cancelButton;

		/// <summary>

		/// Required designer variable.

		/// </summary>

		private System.ComponentModel.Container components = null;



		public OpenProject()

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();

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

			this.acceptButton = new System.Windows.Forms.Button();

			this.projectView = new System.Data.DataView();

			this.projectList = new System.Windows.Forms.ListBox();

			this.newButton = new System.Windows.Forms.Button();

			this.editButton = new System.Windows.Forms.Button();

			this.button2 = new System.Windows.Forms.Button();

			this.descrBox = new System.Windows.Forms.TextBox();

			this.cancelButton = new System.Windows.Forms.Button();

			((System.ComponentModel.ISupportInitialize)(this.projectView)).BeginInit();

			this.SuspendLayout();

			// 

			// acceptButton

			// 

			this.acceptButton.Location = new System.Drawing.Point(256, 184);

			this.acceptButton.Name = "acceptButton";

			this.acceptButton.TabIndex = 1;

			this.acceptButton.Text = "Accept";

			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);

			// 

			// projectList

			// 

			this.projectList.DataSource = this.projectView;

			this.projectList.Location = new System.Drawing.Point(24, 16);

			this.projectList.Name = "projectList";

			this.projectList.Size = new System.Drawing.Size(160, 134);

			this.projectList.TabIndex = 3;

			this.projectList.DoubleClick += new System.EventHandler(this.acceptButton_Click);

			// 

			// newButton

			// 

			this.newButton.Location = new System.Drawing.Point(248, 24);

			this.newButton.Name = "newButton";

			this.newButton.Size = new System.Drawing.Size(88, 23);

			this.newButton.TabIndex = 4;

			this.newButton.Text = "New Project...";

			this.newButton.Click += new System.EventHandler(this.newButton_Click);

			// 

			// editButton

			// 

			this.editButton.Location = new System.Drawing.Point(248, 72);

			this.editButton.Name = "editButton";

			this.editButton.Size = new System.Drawing.Size(88, 23);

			this.editButton.TabIndex = 5;

			this.editButton.Text = "Edit Project";

			this.editButton.Click += new System.EventHandler(this.button1_Click);

			// 

			// button2

			// 

			this.button2.Location = new System.Drawing.Point(248, 120);

			this.button2.Name = "button2";

			this.button2.Size = new System.Drawing.Size(88, 23);

			this.button2.TabIndex = 6;

			this.button2.Text = "Delete";

			this.button2.Click += new System.EventHandler(this.button2_Click);

			// 

			// descrBox

			// 

			this.descrBox.Location = new System.Drawing.Point(24, 176);

			this.descrBox.Multiline = true;

			this.descrBox.Name = "descrBox";

			this.descrBox.ReadOnly = true;

			this.descrBox.Size = new System.Drawing.Size(164, 72);

			this.descrBox.TabIndex = 7;

			this.descrBox.Text = "";

			// 

			// cancelButton

			// 

			this.cancelButton.Location = new System.Drawing.Point(256, 216);

			this.cancelButton.Name = "cancelButton";

			this.cancelButton.TabIndex = 8;

			this.cancelButton.Text = "Cancel";

			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);

			// 

			// OpenProject

			// 

			this.AcceptButton = this.acceptButton;

			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);

			this.ClientSize = new System.Drawing.Size(368, 262);

			this.ControlBox = false;

			this.Controls.Add(this.cancelButton);

			this.Controls.Add(this.descrBox);

			this.Controls.Add(this.button2);

			this.Controls.Add(this.editButton);

			this.Controls.Add(this.newButton);

			this.Controls.Add(this.projectList);

			this.Controls.Add(this.acceptButton);

			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;

			this.MaximizeBox = false;

			this.MinimizeBox = false;

			this.Name = "OpenProject";

			this.Text = "Open Project";

			((System.ComponentModel.ISupportInitialize)(this.projectView)).EndInit();

			this.ResumeLayout(false);



		}

		#endregion



		private void acceptButton_Click(object sender, System.EventArgs e)

		{

			this.DialogResult = DialogResult.OK;

		}



		private void newButton_Click(object sender, System.EventArgs e)

		{

			NameAndDescr dlg = new NameAndDescr();

			dlg.Type = "Project";



			DialogResult rslt = dlg.ShowDialog();



			if (rslt == DialogResult.OK)

			{

				MrktSimDBSchema.projectRow proj = theDb.CreateProject(dlg.ObjName, dlg.ObjDescription);



				theDb.Update();

			}



			dlg.Dispose();

		}



		// edit project

		private void button1_Click(object sender, System.EventArgs e)

		{

			

			MrktSimDBSchema.projectRow proj = getSelectedProject();



			if (proj != null)

			{

				NameAndDescr dlg = new NameAndDescr();



				dlg.Type = "Project";



				dlg.ObjName = proj.name;



				dlg.ObjDescription = proj.descr;



				dlg.Editing = true;



				DialogResult rslt = dlg.ShowDialog();



				if (rslt == DialogResult.OK)

				{

					proj.name = dlg.ObjName;



					proj.descr = dlg.ObjDescription;



					theDb.Update();

				}

			}

		}



		private MrktSimDBSchema.projectRow getSelectedProject()

		{

			return (MrktSimDBSchema.projectRow) ((DataRowView) projectList.SelectedItem).Row;

		}



		// this is the delete button

		private void button2_Click(object sender, System.EventArgs e)

		{

			DialogResult rslt = MessageBox.Show("This will delete all models and results in the project", "Delete Project?",MessageBoxButtons.OKCancel);



			if (rslt == DialogResult.OK)

			{

				MrktSimDBSchema.projectRow proj = getSelectedProject();



				proj.Delete();



				theDb.Update();

			}

		}



		private void cancelButton_Click(object sender, System.EventArgs e)

		{

			this.DialogResult = DialogResult.Cancel;

		}

	}

}

