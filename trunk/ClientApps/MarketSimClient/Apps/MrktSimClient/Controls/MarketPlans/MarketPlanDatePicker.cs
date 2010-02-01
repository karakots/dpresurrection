using System;

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MarketSimUtilities;
using MrktSimDb;
using Utilities;

namespace MrktSimClient.Controls.MarketPlans
{
    class MarketPlanDatePicker : MrktSimControl
    {

        private ComboBox rangeTypeComboBox;

        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;
        private Label label2;
        private Label label3;
        private int fullHeight = -1;

        private int zoomRangeDays = 2;

        private DateTime viewStartDate;
        private Label label1;
        private DateTime viewEndDate;

        public delegate void FireDateRangeChanged( DateTime startDate, DateTime endDate );
        public event FireDateRangeChanged DateViewRangeChanged;

        public MarketPlanDatePicker()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();


            this.rangeTypeComboBox.SelectedIndex = 0;
		}

        public override MrktSimDb.ModelDb Db {
            set {
                base.Db = value;
                dateTimePicker1.Value = theDb.Model.start_date;
                dateTimePicker2.Value = theDb.Model.end_date;
            }
        }

        public void Clear() {
            rangeTypeComboBox.SelectedIndex = 0;
        }

        		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        private void InitializeComponent() {
            this.rangeTypeComboBox = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rangeTypeComboBox
            // 
            this.rangeTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rangeTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rangeTypeComboBox.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.rangeTypeComboBox.FormattingEnabled = true;
            this.rangeTypeComboBox.Items.AddRange( new object[] {
            "All",
            "Range of Dates",
            "On a Date",
            "Before a Date",
            "After a Date"} );
            this.rangeTypeComboBox.Location = new System.Drawing.Point( 54, 8 );
            this.rangeTypeComboBox.Name = "rangeTypeComboBox";
            this.rangeTypeComboBox.Size = new System.Drawing.Size( 104, 22 );
            this.rangeTypeComboBox.TabIndex = 2;
            this.rangeTypeComboBox.SelectedIndexChanged += new System.EventHandler( this.rangeTypeComboBox_SelectedIndexChanged );
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.dateTimePicker1.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point( 50, 33 );
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size( 99, 20 );
            this.dateTimePicker1.TabIndex = 3;
            this.dateTimePicker1.ValueChanged += new System.EventHandler( this.dateTimePicker1_ValueChanged );
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker2.CalendarFont = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.dateTimePicker2.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point( 50, 55 );
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size( 99, 20 );
            this.dateTimePicker2.TabIndex = 4;
            this.dateTimePicker2.ValueChanged += new System.EventHandler( this.dateTimePicker2_ValueChanged );
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label2.Location = new System.Drawing.Point( 14, 36 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 31, 14 );
            this.label2.TabIndex = 5;
            this.label2.Text = "From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label3.Location = new System.Drawing.Point( 15, 57 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 19, 14 );
            this.label3.TabIndex = 6;
            this.label3.Text = "To";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 3, 3 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 40, 15 );
            this.label1.TabIndex = 7;
            this.label1.Text = "Dates";
            // 
            // MarketPlanDatePicker
            // 
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.dateTimePicker2 );
            this.Controls.Add( this.dateTimePicker1 );
            this.Controls.Add( this.rangeTypeComboBox );
            this.Margin = new System.Windows.Forms.Padding( 0 );
            this.MinimumSize = new System.Drawing.Size( 157, 32 );
            this.Name = "MarketPlanDatePicker";
            this.Size = new System.Drawing.Size( 162, 82 );
            this.ResumeLayout( false );
            this.PerformLayout();

        }
                #endregion

        /// <summary>
        /// Responds to a change in the date range settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Fires a DateViewRangeChanged event unless the selected row index is -1.</remarks>
        private void HandleDateRangeChanged() {
            if( theDb == null ) {
                return;
            }

            switch( rangeTypeComboBox.SelectedIndex ) {
                case 0:
                    viewStartDate = theDb.Model.start_date;
                    viewEndDate = theDb.Model.end_date;
                    break;
                case 1:
                    viewStartDate = dateTimePicker1.Value;
                    viewEndDate = dateTimePicker2.Value;
                    break;
                case 2:
                    viewStartDate = dateTimePicker1.Value.AddDays( -zoomRangeDays / 2 );
                    viewEndDate = dateTimePicker1.Value.AddDays( zoomRangeDays / 2 );
                    break;
                case 3:
                    viewStartDate = theDb.Model.start_date;
                    viewEndDate = dateTimePicker1.Value;
                    break;
                case 4:
                    viewStartDate = dateTimePicker1.Value;
                    viewEndDate = theDb.Model.end_date;
                    break;
            }

            if( DateViewRangeChanged != null ) {
                DateViewRangeChanged( viewStartDate, viewEndDate );
            }
        }

        private void rangeTypeComboBox_SelectedIndexChanged( object sender, EventArgs e ) {
            if( this.fullHeight == -1 ) {
                // save the full height the first time we come here
                this.fullHeight = this.Height;
            }
            switch( rangeTypeComboBox.SelectedIndex ) {
                case 0:
                    dateTimePicker1.Visible = false;
                    label2.Text = "";
                    dateTimePicker2.Visible = false;
                    label3.Text = "";
                    if( this.Parent != null ) {
                        this.Parent.Height = fullHeight - 44;
                    }
                    break;
                case 1:
                    dateTimePicker1.Visible = true;
                    label2.Text = "From";
                    dateTimePicker2.Visible = true;
                    label3.Text = "To";
                    this.Parent.Height = fullHeight;
                    break;
                case 2:
                    dateTimePicker1.Value = DateTime.Now;
                    dateTimePicker1.Visible = true;
                    label2.Text = "On";
                    dateTimePicker2.Visible = false;
                    label3.Text = "";
                    this.Parent.Height = fullHeight - 22;
                    break;
                case 3:
                    dateTimePicker1.Value = DateTime.Now;
                    dateTimePicker1.Visible = true;
                    label2.Text = "Before";
                    dateTimePicker2.Visible = false;
                    label3.Text = "";
                    this.Parent.Height = fullHeight - 22;
                    break;
                case 4:
                    dateTimePicker1.Value = DateTime.Now;
                    dateTimePicker1.Visible = true;
                    label2.Text = "After";
                    dateTimePicker2.Visible = false;
                    label3.Text = "";
                    this.Parent.Height = fullHeight - 22;
                    break;
            }
            HandleDateRangeChanged();
        }

        private void dateTimePicker1_ValueChanged( object sender, EventArgs e ) {
            HandleDateRangeChanged();
        }

        private void dateTimePicker2_ValueChanged( object sender, EventArgs e ) {
            HandleDateRangeChanged();
        }
    }
}
