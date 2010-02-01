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
	/// Summary description for SegmentPicker.
	/// </summary>
    public class SegmentPicker : UserControl
	{

        public void HideTitle() {
            this.channelLabel.Visible = false;
            this.segmentBox.Bounds = new Rectangle( 2, 2, this.Width - 4, this.Height - 4 );
        }

		public bool AllowAll
		{
			set
			{
				if (value)
				{
					segmentView.RowFilter = "";
				}
				else
				{
                    segmentView.RowFilter = "segment_id <> " + Database.AllID;
				}
			}
		}
		
		public int SegmentID
		{
			get
			{
				if (segmentBox.SelectedItem == null)
					return Database.AllID;

				MrktSimDBSchema.segmentRow row = (MrktSimDBSchema.segmentRow) 
					((DataRowView) segmentBox.SelectedItem).Row;

				return row.segment_id;
			}

			set
			{
                if (segmentBox.ValueMember != null &&
                   segmentBox.ValueMember != "")
					segmentBox.SelectedValue = value;
			}
		}

		private System.Windows.Forms.Label channelLabel;
		private System.Windows.Forms.ComboBox segmentBox;
		private System.Data.DataView segmentView;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		//private System.ComponentModel.Container components = null;

		public SegmentPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			segmentBox.DropDownStyle = ComboBoxStyle.DropDownList;
		}


        PCSModel theDb = null;
		public PCSModel Db
		{
			set
			{
                theDb = value;

				segmentView.Table = theDb.Data.segment;
				segmentBox.DataSource = segmentView;
				segmentBox.DisplayMember = "segment_name";
				segmentBox.ValueMember = "segment_id";
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.segmentBox = new System.Windows.Forms.ComboBox();
			this.channelLabel = new System.Windows.Forms.Label();
			this.segmentView = new System.Data.DataView();
			((System.ComponentModel.ISupportInitialize)(this.segmentView)).BeginInit();
			this.SuspendLayout();
			// 
			// segmentBox
			// 
			this.segmentBox.Location = new System.Drawing.Point(72, 0);
			this.segmentBox.Name = "segmentBox";
			this.segmentBox.Size = new System.Drawing.Size(144, 21);
			this.segmentBox.TabIndex = 3;
			// 
			// channelLabel
			// 
			this.channelLabel.Location = new System.Drawing.Point(0, 0);
			this.channelLabel.Name = "channelLabel";
			this.channelLabel.Size = new System.Drawing.Size(56, 16);
			this.channelLabel.TabIndex = 2;
			this.channelLabel.Text = "Segment";
			// 
			// SegmentPicker
			// 
			this.Controls.Add(this.segmentBox);
			this.Controls.Add(this.channelLabel);
			this.Name = "SegmentPicker";
			this.Size = new System.Drawing.Size(216, 24);
			((System.ComponentModel.ISupportInitialize)(this.segmentView)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
