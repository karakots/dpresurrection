using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Common.Utilities;
using MrktSimDb;
using MarketSimUtilities;

namespace Common
{
	/// <summary>
	/// Summary description for SocialNetwork.
	/// </summary>
	public class SocialNetwork : MrktSimControl
	{

		public override MrktSimDb.ModelDb Db
		{
			set
			{
				base.Db = value;

				mrktSimGrid.Table = theDb.Data.segment_network;

				fromSegmentPicker.Db = theDb;
				toSegmentPicker.Db = theDb;
				parameterBox.DataSource = theDb.Data.network_parameter;
				parameterBox.DisplayMember = "name";

				if (parameterBox.SelectedIndex < 0)
					addNetworkButton.Enabled = false;
				else
					addNetworkButton.Enabled = true;

				createTableStyle();
			}
		}

		public override void Refresh()
		{
			base.Refresh ();

			if (parameterBox.SelectedIndex < 0)
				addNetworkButton.Enabled = false;
			else
				addNetworkButton.Enabled = true;
		}


		public override bool Suspend
		{
			get
			{
				return base.Suspend;
			}
			set
			{
				base.Suspend = value;

				mrktSimGrid.Suspend = value;
			}
		}

		public override void Flush()
		{
			mrktSimGrid.Flush();
		}


		private MrktSimGrid mrktSimGrid;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button addNetworkButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private Common.Utilities.SegmentPicker toSegmentPicker;
		private Common.Utilities.SegmentPicker fromSegmentPicker;
		private System.Windows.Forms.ComboBox parameterBox;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SocialNetwork()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            this.toSegmentPicker.HideTitle();
            this.fromSegmentPicker.HideTitle();
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
            this.mrktSimGrid = new MarketSimUtilities.MrktSimGrid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.parameterBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toSegmentPicker = new Common.Utilities.SegmentPicker();
            this.fromSegmentPicker = new Common.Utilities.SegmentPicker();
            this.addNetworkButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mrktSimGrid
            // 
            this.mrktSimGrid.DescribeRow = null;
            this.mrktSimGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mrktSimGrid.EnabledGrid = true;
            this.mrktSimGrid.Location = new System.Drawing.Point( 0, 128 );
            this.mrktSimGrid.Name = "mrktSimGrid";
            this.mrktSimGrid.RowFilter = null;
            this.mrktSimGrid.RowID = null;
            this.mrktSimGrid.RowName = null;
            this.mrktSimGrid.Size = new System.Drawing.Size( 520, 192 );
            this.mrktSimGrid.Sort = "";
            this.mrktSimGrid.TabIndex = 0;
            this.mrktSimGrid.Table = null;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.parameterBox );
            this.groupBox1.Controls.Add( this.label3 );
            this.groupBox1.Controls.Add( this.label2 );
            this.groupBox1.Controls.Add( this.label1 );
            this.groupBox1.Controls.Add( this.toSegmentPicker );
            this.groupBox1.Controls.Add( this.fromSegmentPicker );
            this.groupBox1.Controls.Add( this.addNetworkButton );
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point( 0, 0 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 520, 128 );
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Social Network";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point( 39, 98 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 100, 16 );
            this.label3.TabIndex = 6;
            this.label3.Text = "Network Model";
            // 
            // parameterBox
            // 
            this.parameterBox.Location = new System.Drawing.Point( 136, 96 );
            this.parameterBox.Name = "parameterBox";
            this.parameterBox.Size = new System.Drawing.Size( 152, 21 );
            this.parameterBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point( 273, 30 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 100, 16 );
            this.label2.TabIndex = 4;
            this.label2.Text = "Segment Listening";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 8, 30 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 100, 16 );
            this.label1.TabIndex = 3;
            this.label1.Text = "Segment Talking";
            // 
            // toSegmentPicker
            // 
            this.toSegmentPicker.Location = new System.Drawing.Point( 273, 48 );
            this.toSegmentPicker.Name = "toSegmentPicker";
            this.toSegmentPicker.SegmentID = -1;
            this.toSegmentPicker.Size = new System.Drawing.Size( 216, 24 );
            this.toSegmentPicker.TabIndex = 2;
            // 
            // fromSegmentPicker
            // 
            this.fromSegmentPicker.Location = new System.Drawing.Point( 8, 48 );
            this.fromSegmentPicker.Name = "fromSegmentPicker";
            this.fromSegmentPicker.SegmentID = -1;
            this.fromSegmentPicker.Size = new System.Drawing.Size( 216, 24 );
            this.fromSegmentPicker.TabIndex = 1;
            // 
            // addNetworkButton
            // 
            this.addNetworkButton.Location = new System.Drawing.Point( 328, 96 );
            this.addNetworkButton.Name = "addNetworkButton";
            this.addNetworkButton.Size = new System.Drawing.Size( 168, 23 );
            this.addNetworkButton.TabIndex = 0;
            this.addNetworkButton.Text = "Add to Network";
            this.addNetworkButton.Click += new System.EventHandler( this.addNetworkButton_Click );
            // 
            // SocialNetwork
            // 
            this.Controls.Add( this.mrktSimGrid );
            this.Controls.Add( this.groupBox1 );
            this.Name = "SocialNetwork";
            this.Size = new System.Drawing.Size( 520, 320 );
            this.groupBox1.ResumeLayout( false );
            this.ResumeLayout( false );

		}
		#endregion

		private void createTableStyle()
		{
			mrktSimGrid.Clear();

			mrktSimGrid.AddTextColumn("from_segment_name", true);
			mrktSimGrid.AddTextColumn("to_segment_name", true);

			mrktSimGrid.AddComboBoxColumn("network_param", theDb.Data.network_parameter, "name", "id");
			// mrktSimGrid.AddTextColumn("parameter_name", true);

			mrktSimGrid.Reset();
		}

		private void addNetworkButton_Click(object sender, System.EventArgs e)
		{
			MrktSimDBSchema.segmentRow from = theDb.Data.segment.FindBysegment_id(fromSegmentPicker.SegmentID);
			MrktSimDBSchema.segmentRow to = theDb.Data.segment.FindBysegment_id(toSegmentPicker.SegmentID);
			MrktSimDBSchema.network_parameterRow param = (MrktSimDBSchema.network_parameterRow) ((DataRowView) parameterBox.SelectedItem).Row;
			
			theDb.CreateSegmentNetwork(from, to, param);
		}
	}

	
}
