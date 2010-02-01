using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;
using Common.Utilities;
using MarketSimUtilities.MsTree;
using MarketSimUtilities;

namespace Common
{
	/// <summary>
	/// Summary description for TaskControl.
	/// </summary>
	public class TaskControl : MrktSimControl
	{
		public override void Refresh()
		{
			base.Refresh();

			taskGrid.Refresh();
			taskProdGrid.Refresh();
			taskSegmentGrid.Refresh();
		}

		public override void Flush()
		{
			taskGrid.Flush();
			taskProdGrid.Flush();
			taskSegmentGrid.Flush();
		}

		public override bool Suspend
		{
			set
			{
				base.Suspend = value;

				taskGrid.Suspend = value;
				taskProdGrid.Suspend = value;
				taskSegmentGrid.Suspend = value;
			}
		}

		public override void Reset()
		{
			base.Reset ();
			this.createTableStyle();
		}

		private System.Windows.Forms.DateTimePicker startDateTimePicker;
		private System.Windows.Forms.DateTimePicker endDateTimePicker;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown suitMinUpDown;
		private System.Windows.Forms.NumericUpDown suitMaxUpDown;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox taskBox;
		private MrktSimGrid taskProdGrid;
		private MrktSimGrid taskSegmentGrid;
		private System.Windows.Forms.TabControl channelTabControl;
		private System.Windows.Forms.TabPage productPage;
		private System.Windows.Forms.TabPage segmentPage;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TabPage tasklPage;
		private MrktSimGrid taskGrid;
		private System.Windows.Forms.Button createTaskButton;
		private System.Windows.Forms.TextBox taskNameBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TaskControl(ModelDb db) : base(db)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			theDb.Data.product.RowChanged +=new DataRowChangeEventHandler(prodDataChanged);
			theDb.Data.segment.RowChanged +=new DataRowChangeEventHandler(segDataChanged);

			taskGrid.Table = theDb.Data.task;
			taskGrid.RowFilter = "task_id <> " + ModelDb.AllID;

			taskProdGrid.Table = theDb.Data.task_product_fact;
			taskSegmentGrid.Table = theDb.Data.task_rate_fact;

			// bind controls
			//taskNameBox.DataBindings.Add("Text", theDb.Data.task, "task_name");
			suitMinUpDown.Value = 0;
			suitMaxUpDown.Value = 1;
			suitMinUpDown.DataBindings.Add("Maximum", suitMaxUpDown, "Value");
			//suitMinUpDown.DataBindings.Add("Value", theDb.Data.task, "suitability_min");

			suitMaxUpDown.DataBindings.Add("Minimum", suitMinUpDown, "Value");
			//suitMaxUpDown.DataBindings.Add("Value", theDb.Data.task, "suitability_max");

			startDateTimePicker.Value = theDb.StartDate;
			endDateTimePicker.Value = theDb.EndDate;

			endDateTimePicker.DataBindings.Add("MinDate", startDateTimePicker, "Value");
			//endDateTimePicker.DataBindings.Add("Value", theDb.Data.task, "end_date");

			startDateTimePicker.DataBindings.Add("MaxDate", endDateTimePicker, "Value");
			//startDateTimePicker.DataBindings.Add("Value", theDb.Data.task, "start_date");

			createTableStyle();
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.suitMaxUpDown = new System.Windows.Forms.NumericUpDown();
			this.suitMinUpDown = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.endDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.startDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.taskNameBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.taskBox = new System.Windows.Forms.GroupBox();
			this.taskSegmentGrid = new MrktSimGrid();
			this.taskProdGrid = new MrktSimGrid();
			this.channelTabControl = new System.Windows.Forms.TabControl();
			this.tasklPage = new System.Windows.Forms.TabPage();
			this.taskGrid = new MrktSimGrid();
			this.productPage = new System.Windows.Forms.TabPage();
			this.segmentPage = new System.Windows.Forms.TabPage();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.createTaskButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.suitMaxUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.suitMinUpDown)).BeginInit();
			this.taskBox.SuspendLayout();
			this.channelTabControl.SuspendLayout();
			this.tasklPage.SuspendLayout();
			this.productPage.SuspendLayout();
			this.segmentPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(256, 88);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 16);
			this.label5.TabIndex = 8;
			this.label5.Text = "Max Suitability";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(256, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "Min Suitability";
			// 
			// suitMaxUpDown
			// 
			this.suitMaxUpDown.Location = new System.Drawing.Point(344, 88);
			this.suitMaxUpDown.Minimum = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.suitMaxUpDown.Name = "suitMaxUpDown";
			this.suitMaxUpDown.Size = new System.Drawing.Size(56, 20);
			this.suitMaxUpDown.TabIndex = 6;
			this.suitMaxUpDown.Value = new System.Decimal(new int[] {
																		1,
																		0,
																		0,
																		0});
			// 
			// suitMinUpDown
			// 
			this.suitMinUpDown.Location = new System.Drawing.Point(344, 48);
			this.suitMinUpDown.Maximum = new System.Decimal(new int[] {
																		  1,
																		  0,
																		  0,
																		  0});
			this.suitMinUpDown.Name = "suitMinUpDown";
			this.suitMinUpDown.Size = new System.Drawing.Size(56, 20);
			this.suitMinUpDown.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(40, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "End Date";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(40, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Start Date";
			// 
			// endDateTimePicker
			// 
			this.endDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.endDateTimePicker.Location = new System.Drawing.Point(120, 88);
			this.endDateTimePicker.Name = "endDateTimePicker";
			this.endDateTimePicker.Size = new System.Drawing.Size(104, 20);
			this.endDateTimePicker.TabIndex = 2;
			this.endDateTimePicker.Value = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
			// 
			// startDateTimePicker
			// 
			this.startDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.startDateTimePicker.Location = new System.Drawing.Point(120, 48);
			this.startDateTimePicker.Name = "startDateTimePicker";
			this.startDateTimePicker.Size = new System.Drawing.Size(104, 20);
			this.startDateTimePicker.TabIndex = 1;
			this.startDateTimePicker.Value = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
			// 
			// taskNameBox
			// 
			this.taskNameBox.Location = new System.Drawing.Point(120, 16);
			this.taskNameBox.Name = "taskNameBox";
			this.taskNameBox.Size = new System.Drawing.Size(272, 20);
			this.taskNameBox.TabIndex = 9;
			this.taskNameBox.Text = "";
			this.taskNameBox.TextChanged += new System.EventHandler(this.taskNameBox_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(40, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 10;
			this.label1.Text = "Task Name";
			// 
			// taskBox
			// 
			this.taskBox.Controls.Add(this.createTaskButton);
			this.taskBox.Controls.Add(this.label4);
			this.taskBox.Controls.Add(this.suitMaxUpDown);
			this.taskBox.Controls.Add(this.suitMinUpDown);
			this.taskBox.Controls.Add(this.label3);
			this.taskBox.Controls.Add(this.label2);
			this.taskBox.Controls.Add(this.label1);
			this.taskBox.Controls.Add(this.startDateTimePicker);
			this.taskBox.Controls.Add(this.taskNameBox);
			this.taskBox.Controls.Add(this.endDateTimePicker);
			this.taskBox.Controls.Add(this.label5);
			this.taskBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.taskBox.Location = new System.Drawing.Point(0, 0);
			this.taskBox.Name = "taskBox";
			this.taskBox.Size = new System.Drawing.Size(640, 128);
			this.taskBox.TabIndex = 1;
			this.taskBox.TabStop = false;
			this.taskBox.Text = "Tasks";
			// 
			// taskSegmentGrid
			// 
			this.taskSegmentGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.taskSegmentGrid.Location = new System.Drawing.Point(0, 0);
			this.taskSegmentGrid.Name = "taskSegmentGrid";
			this.taskSegmentGrid.RowFilter = "";
			this.taskSegmentGrid.Size = new System.Drawing.Size(632, 206);
			this.taskSegmentGrid.TabIndex = 2;
			// 
			// taskProdGrid
			// 
			this.taskProdGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.taskProdGrid.Location = new System.Drawing.Point(0, 0);
			this.taskProdGrid.Name = "taskProdGrid";
			this.taskProdGrid.RowFilter = "";
			this.taskProdGrid.Size = new System.Drawing.Size(632, 206);
			this.taskProdGrid.TabIndex = 0;
			// 
			// channelTabControl
			// 
			this.channelTabControl.Controls.Add(this.tasklPage);
			this.channelTabControl.Controls.Add(this.productPage);
			this.channelTabControl.Controls.Add(this.segmentPage);
			this.channelTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.channelTabControl.Location = new System.Drawing.Point(0, 128);
			this.channelTabControl.Name = "channelTabControl";
			this.channelTabControl.SelectedIndex = 0;
			this.channelTabControl.Size = new System.Drawing.Size(640, 232);
			this.channelTabControl.TabIndex = 2;
			// 
			// tasklPage
			// 
			this.tasklPage.Controls.Add(this.taskGrid);
			this.tasklPage.Location = new System.Drawing.Point(4, 22);
			this.tasklPage.Name = "tasklPage";
			this.tasklPage.Size = new System.Drawing.Size(632, 206);
			this.tasklPage.TabIndex = 2;
			this.tasklPage.Text = "Tasks";
			// 
			// taskGrid
			// 
			this.taskGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.taskGrid.Location = new System.Drawing.Point(0, 0);
			this.taskGrid.Name = "taskGrid";
			this.taskGrid.RowFilter = "";
			this.taskGrid.Size = new System.Drawing.Size(632, 206);
			this.taskGrid.TabIndex = 0;
			// 
			// productPage
			// 
			this.productPage.Controls.Add(this.taskProdGrid);
			this.productPage.Location = new System.Drawing.Point(4, 22);
			this.productPage.Name = "productPage";
			this.productPage.Size = new System.Drawing.Size(632, 206);
			this.productPage.TabIndex = 0;
			this.productPage.Text = "Product Suitability";
			// 
			// segmentPage
			// 
			this.segmentPage.Controls.Add(this.taskSegmentGrid);
			this.segmentPage.Location = new System.Drawing.Point(4, 22);
			this.segmentPage.Name = "segmentPage";
			this.segmentPage.Size = new System.Drawing.Size(632, 206);
			this.segmentPage.TabIndex = 1;
			this.segmentPage.Text = "Segment Rates";
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 128);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(640, 3);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// createTaskButton
			// 
			this.createTaskButton.Location = new System.Drawing.Point(448, 88);
			this.createTaskButton.Name = "createTaskButton";
			this.createTaskButton.TabIndex = 11;
			this.createTaskButton.Text = "Create task";
			this.createTaskButton.Click += new System.EventHandler(this.createTaskButton_Click);
			// 
			// TaskControl
			// 
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.channelTabControl);
			this.Controls.Add(this.taskBox);
			this.Name = "TaskControl";
			this.Size = new System.Drawing.Size(640, 360);
			((System.ComponentModel.ISupportInitialize)(this.suitMaxUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.suitMinUpDown)).EndInit();
			this.taskBox.ResumeLayout(false);
			this.channelTabControl.ResumeLayout(false);
			this.tasklPage.ResumeLayout(false);
			this.productPage.ResumeLayout(false);
			this.segmentPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	

		private void createTableStyle()
		{
			this.SuspendLayout();

			taskProdGrid.Clear();
			taskSegmentGrid.Clear();
			taskGrid.Clear();

			//channel table

			taskGrid.AddTextColumn("task_name");
			taskGrid.AddTextColumn("suitability_min");
			taskGrid.AddTextColumn("suitability_max");
			taskGrid.AddTextColumn("start_date");
			taskGrid.AddTextColumn("end_date");

			// 
			// task product table
			// 

			taskProdGrid.AddTextColumn("task_name", true);
			taskProdGrid.AddTextColumn("product_name", true);
			taskProdGrid.AddTextColumn("pre_use_upsku");
			taskProdGrid.AddTextColumn("post_use_upsku");
			taskProdGrid.AddTextColumn("suitability");
			
			// 
			// task segment table
			// 

			taskSegmentGrid.AddTextColumn("task_name", true);
			taskSegmentGrid.AddTextColumn("segment_name", true);
			taskSegmentGrid.AddDateColumn("start_date");
			taskSegmentGrid.AddDateColumn("end_date");

			string[] periods = {"Day", "Week", "Month", "Year"};
			taskSegmentGrid.AddComboBoxColumn("time_period", periods);

			taskSegmentGrid.AddTextColumn("task_rate");

			taskGrid.Reset();
			taskProdGrid.Reset();
			taskSegmentGrid.Reset();

			this.ResumeLayout(false);
		}

		private void prodDataChanged(object sender, System.Data.DataRowChangeEventArgs e)
		{
			if (!Suspend)
				taskProdGrid.Refresh();
		}

		private void segDataChanged(object sender, System.Data.DataRowChangeEventArgs e)
		{
			if (!Suspend)
				taskSegmentGrid.Refresh();
		}

		private void taskNameBox_TextChanged(object sender, System.EventArgs e)
		{
			if (taskNameBox.Text == null ||
				taskNameBox.Text == "")
				createTaskButton.Enabled = false;
			else
				createTaskButton.Enabled = true;
		}

		private void createTaskButton_Click(object sender, System.EventArgs e)
		{
			if (taskNameBox.Text == null ||
				taskNameBox.Text == "")
				return;

			theDb.CreateTask(taskNameBox.Text);

			taskNameBox.Text = null;

			createTaskButton.Enabled = false;
		}
	}
}
