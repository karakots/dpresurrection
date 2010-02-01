using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MarketSimUtilities;
using MrktSimDb;

namespace MrktSimClient.Controls.MarketPlans
{
	/// <summary>
	/// Summary description for ChannelPicker.
	/// </summary>
    public class MarketPlanChannelPicker : UserControl
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

        public delegate void FireSelectedRowChanged( MrktSimDBSchema.channelRow channelRow );
        public event FireSelectedRowChanged SelectedRowChanged;
        
        private System.Windows.Forms.Label channelLabel;
		private System.Windows.Forms.ComboBox channelBox;
		private System.Data.DataView channelView;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
        //private System.ComponentModel.Container components = null;

        public MarketPlanChannelPicker()
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
            this.channelLabel.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.channelLabel.Location = new System.Drawing.Point( 2, 2 );
            this.channelLabel.Name = "channelLabel";
            this.channelLabel.Size = new System.Drawing.Size( 53, 16 );
            this.channelLabel.TabIndex = 0;
            this.channelLabel.Text = "Channel";
            // 
            // channelBox
            // 
            this.channelBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.channelBox.DataSource = this.channelView;
            this.channelBox.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.channelBox.Location = new System.Drawing.Point( 54, 2 );
            this.channelBox.Name = "channelBox";
            this.channelBox.Size = new System.Drawing.Size( 104, 22 );
            this.channelBox.TabIndex = 1;
            this.channelBox.SelectedIndexChanged += new System.EventHandler( this.channelBox_SelectedIndexChanged );
            // 
            // MarketPlanChannelPicker
            // 
            this.Controls.Add( this.channelBox );
            this.Controls.Add( this.channelLabel );
            this.MinimumSize = new System.Drawing.Size( 162, 26 );
            this.Name = "MarketPlanChannelPicker";
            this.Size = new System.Drawing.Size( 162, 26 );
            ((System.ComponentModel.ISupportInitialize)(this.channelView)).EndInit();
            this.ResumeLayout( false );

		}
		#endregion

        private void channelBox_SelectedIndexChanged( object sender, EventArgs e ) {
            if( channelBox.SelectedIndex == -1 ) {
                return;
            }

             MrktSimDBSchema.channelRow channel = (MrktSimDBSchema.channelRow)((DataRowView)channelBox.SelectedItem).Row;

            if( SelectedRowChanged != null ) {
                SelectedRowChanged( channel );
            }
        }
	}
}
