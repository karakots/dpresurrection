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
	/// Summary description for ChannelPicker.
	/// </summary>
    public class ChannelPicker : UserControl
	{
		public int ChannelID
		{
			get
			{
				if (channelBox.SelectedItem == null)
					return Database.AllID;

				MrktSimDBSchema.channelRow row = (MrktSimDBSchema.channelRow) 
					((DataRowView) channelBox.SelectedItem).Row;

				return row.channel_id;
			}

			set
			{
                if (channelBox.ValueMember != null  &&
                    channelBox.ValueMember != "")
				channelBox.SelectedValue = value;
			}
		}

		public bool AlLowAll
		{
			set
			{
				if (!value)
					channelView.RowFilter = "channel_id <> " + Database.AllID;
			}
		}

		private System.Windows.Forms.Label channelLabel;
		private System.Windows.Forms.ComboBox channelBox;
		private System.Data.DataView channelView;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		//private System.ComponentModel.Container components = null;

		public ChannelPicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

        PCSModel theDb = null;

		public PCSModel Db
		{
			set
			{
                theDb = value;

				channelView.Table = theDb.Data.channel;
                channelBox.DisplayMember = "channel_name";
                channelBox.ValueMember = "channel_name";    //!!!jimj test
            }
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.channelLabel = new System.Windows.Forms.Label();
			this.channelBox = new System.Windows.Forms.ComboBox();
			this.channelView = new System.Data.DataView();
			((System.ComponentModel.ISupportInitialize)(this.channelView)).BeginInit();
			this.SuspendLayout();
			// 
			// channelLabel
			// 
			this.channelLabel.Location = new System.Drawing.Point(0, 0);
			this.channelLabel.Name = "channelLabel";
			this.channelLabel.Size = new System.Drawing.Size(48, 16);
			this.channelLabel.TabIndex = 0;
			this.channelLabel.Text = "Channel";
			// 
			// channelBox
			// 
			this.channelBox.DataSource = this.channelView;
			this.channelBox.Location = new System.Drawing.Point(72, 0);
			this.channelBox.Name = "channelBox";
			this.channelBox.Size = new System.Drawing.Size(144, 21);
			this.channelBox.TabIndex = 1;
			// 
			// ChannelPicker
			// 
			this.Controls.Add(this.channelBox);
			this.Controls.Add(this.channelLabel);
			this.Name = "ChannelPicker";
			this.Size = new System.Drawing.Size(216, 24);
			((System.ComponentModel.ISupportInitialize)(this.channelView)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
