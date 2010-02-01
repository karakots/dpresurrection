using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MarketSimUtilities;

using MrktSimDb;

namespace Common.Utilities
{
	/// <summary>
	/// Summary description for TaskPicker.
	/// </summary>
	public class TaskPicker : MrktSimControl
	{

		public delegate void CurrentTask(int productID);

		public event CurrentTask TaskChanged;

		public int TaskID
		{
			get
			{
				if (taskBox.SelectedValue == null)
					return AllID;

				return (int) taskBox.SelectedValue;
			}

			set
			{
				taskBox.SelectedValue = value;
			}
		}

		private System.Windows.Forms.ComboBox taskBox;
		private System.Windows.Forms.Label label2;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TaskPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public override ModelDb Db
		{
			set
			{
				base.Db = value;

				Suspend = true;
				taskBox.DataSource = theDb.Data;
				taskBox.DisplayMember = "task.task_name";
				taskBox.ValueMember = "task.task_id";
				Suspend = false;
			}
		}

//		public TaskPicker(ModelDb db) : base(db)
//		{
//			// This call is required by the Windows.Forms Form Designer.
//			InitializeComponent();
//
//			taskBox.DataSource = theDb.Data;
//			taskBox.DisplayMember = "task.task_name";
//			taskBox.SelectedValue = "task.task_id";
//
//		}

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.taskBox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// taskBox
			// 
			this.taskBox.Location = new System.Drawing.Point(72, 0);
			this.taskBox.Name = "taskBox";
			this.taskBox.Size = new System.Drawing.Size(144, 21);
			this.taskBox.TabIndex = 1;
			this.taskBox.Text = "Select Task";
			this.taskBox.SelectedIndexChanged += new System.EventHandler(this.taskBox_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Task";
			// 
			// TaskPicker
			// 
			this.Controls.Add(this.label2);
			this.Controls.Add(this.taskBox);
			this.Name = "TaskPicker";
			this.Size = new System.Drawing.Size(216, 24);
			this.ResumeLayout(false);

		}
		#endregion

		private void taskBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (Suspend)
				return;

			if (TaskChanged != null)
				TaskChanged(TaskID);
		}
	}
}
