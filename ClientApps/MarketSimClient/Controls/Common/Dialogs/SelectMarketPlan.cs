using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using MarketSimUtilities;


namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for SelectMarketPlan.
	/// </summary>
	public class SelectMarketPlan : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button cancelButton;
		private MrktSimGrid mrktSimGrid1;
		private System.Windows.Forms.Button acceptButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SelectMarketPlan(DataTable aTable)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			mrktSimGrid1.Table = aTable;

			mrktSimGrid1.DescriptionWindow = false;

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SelectMarketPlan));
			this.panel1 = new System.Windows.Forms.Panel();
			this.acceptButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.mrktSimGrid1 = new MrktSimGrid();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.cancelButton);
			this.panel1.Controls.Add(this.acceptButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 182);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(416, 40);
			this.panel1.TabIndex = 0;
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.acceptButton.Location = new System.Drawing.Point(88, 8);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.TabIndex = 0;
			this.acceptButton.Text = "Accept";
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.Location = new System.Drawing.Point(240, 8);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// mrktSimGrid1
			// 
			this.mrktSimGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mrktSimGrid1.Location = new System.Drawing.Point(0, 0);
			this.mrktSimGrid1.Name = "mrktSimGrid1";
			this.mrktSimGrid1.RowFilter = null;
			this.mrktSimGrid1.RowID = null;
			this.mrktSimGrid1.RowName = null;
			this.mrktSimGrid1.Size = new System.Drawing.Size(416, 182);
			this.mrktSimGrid1.Sort = "";
			this.mrktSimGrid1.TabIndex = 1;
			// 
			// SelectMarketPlan
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 222);
			this.ControlBox = false;
			this.Controls.Add(this.mrktSimGrid1);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SelectMarketPlan";
			this.Text = "Select Market Plans to Copy";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		
		#region fields
		private ArrayList list = null;
		private bool multiSelect = true;
		#endregion

		#region public access

		public bool AllowMultiSelect
		{
			set
			{
				multiSelect = value;
			}

			get
			{
				return multiSelect;
			}
		}

		public ArrayList SelectedItems
		{
			get
			{
				return list;
			}
		}

		public MrktSimGrid Grid
		{
			get
			{
				return mrktSimGrid1;
			}
		}

		#endregion

		
		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void acceptButton_Click(object sender, System.EventArgs e)
		{
			// collect up selected items
			list = mrktSimGrid1.GetSelected();


			if (!AllowMultiSelect &&
				(list == null || list.Count != 1))
			{
				MessageBox.Show("Please select one item");
				return;
			}
			else if (list == null || list.Count == 0)
			{
				MessageBox.Show("Please select one or more items");
				return;
			}

			this.DialogResult = DialogResult.OK;
		}
	}
}
