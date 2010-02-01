using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;

using MarketSimUtilities;
namespace Common.Utilities
{
	/// <summary>
	/// Summary description for StartEndDate.
	/// </summary>
    public class StartEndDate : UserControl
	{
        public delegate void ValueChanged( DateTime newValue );
        public ValueChanged StartValueChanged;
        public ValueChanged EndValueChanged;

		// binds control to database
		// seperated out from dbChanged for this control
		// because we generally do not always want to bind to the model dates
		// but in the case of the model summary we do
		public void BindToDb(PCSModel db)
		{
			startDate.DataBindings.Clear();
			startDate.DataBindings.Add("Value", db.Data.Model_info, "start_date");

			endDate.DataBindings.Clear();
			endDate.DataBindings.Add("Value", db.Data.Model_info, "end_date");
		}


		public DateTime Start
		{
			get
			{
				return startDate.Value;
			}

			set
			{
				startDate.Value = value;
			}
		}

		public DateTime End
		{
			get
			{
				return endDate.Value;
			}

			set
			{
				endDate.Value = value;
			}
		}	

		private System.Windows.Forms.DateTimePicker startDate;
		private System.Windows.Forms.DateTimePicker endDate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		// private System.ComponentModel.Container components = null;

		public StartEndDate()
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

				startDate.Value = theDb.StartDate;
				endDate.Value = theDb.EndDate;
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startDate
            // 
            this.startDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDate.Location = new System.Drawing.Point( 64, 0 );
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size( 96, 20 );
            this.startDate.TabIndex = 0;
            this.startDate.ValueChanged += new System.EventHandler( this.startDate_ValueChanged );
            // 
            // endDate
            // 
            this.endDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.endDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endDate.Location = new System.Drawing.Point( 64, 24 );
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size( 96, 20 );
            this.endDate.TabIndex = 1;
            this.endDate.ValueChanged += new System.EventHandler( this.endDate_ValueChanged );
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 0, 0 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 56, 16 );
            this.label1.TabIndex = 2;
            this.label1.Text = "Start Date";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point( 0, 24 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 56, 16 );
            this.label2.TabIndex = 3;
            this.label2.Text = "End Date";
            // 
            // StartEndDate
            // 
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.endDate );
            this.Controls.Add( this.startDate );
            this.Name = "StartEndDate";
            this.Size = new System.Drawing.Size( 160, 48 );
            this.ResumeLayout( false );

		}
		#endregion

        private void startDate_ValueChanged( object sender, EventArgs e ) {
            if( StartValueChanged != null ) {
                StartValueChanged( startDate.Value );
            }
        }

        private void endDate_ValueChanged( object sender, EventArgs e ) {
            if( EndValueChanged != null ) {
                EndValueChanged( endDate.Value );
            }
        }
	}
}
