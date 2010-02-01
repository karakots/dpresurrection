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
	/// Summary description for ModelSummary.
	/// </summary>
    public class ModelSummary : MrktSimControl
    {
        public override void Flush()
        {
            base.Flush();

            if( theDb.Model.start_date != startDate.Value )
            {
                theDb.Model.start_date = startDate.Value;
            }

            if( theDb.Model.end_date != endDate.Value )
            {
                theDb.Model.end_date = endDate.Value;
            }

            if( theDb.Model.pop_size != (int)populationSizeUpDown.Value )
            {
                theDb.Model.pop_size = (int)populationSizeUpDown.Value;
            }

            if( (double) shopWeight.Value != theDb.Model.season_freq_part )
            {
                theDb.Model.season_freq_part = (double) shopWeight.Value;
            }
        }

        public override void Refresh() {
            base.Refresh();

            // update the summary sheet
            /*string query = "brand_id <> " + ModelDb.AllID;
            numBrandsLabel.Text = theDb.Data.brand.Select(query,"",DataViewRowState.CurrentRows).Length.ToString();*/

            string query = "product_id <> " + ModelDb.AllID;
            numProductslabel.Text = theDb.Data.product.Select( query, "", DataViewRowState.CurrentRows ).Length.ToString();

            query = "segment_id <> " + ModelDb.AllID;
            numSegmentsLabel.Text = theDb.Data.segment.Select( query, "", DataViewRowState.CurrentRows ).Length.ToString();

            query = "channel_id <> " + ModelDb.AllID;
            numChannelsLabel.Text = theDb.Data.channel.Select( query, "", DataViewRowState.CurrentRows ).Length.ToString();

            // atrributes do not have ALL
            numAttributesLabel.Text = theDb.Data.product_attribute.Select( "", "", DataViewRowState.CurrentRows ).Length.ToString();

            query = "task_id <> " + ModelDb.AllID;
            numTasksLabel.Text = theDb.Data.task.Select( query, "", DataViewRowState.CurrentRows ).Length.ToString();

            startDate.Value = theDb.Model.start_date;
            endDate.Value = theDb.Model.end_date;
            populationSizeUpDown.Value = theDb.Model.pop_size;
            shopWeight.Value = (decimal) theDb.Model.season_freq_part;
            
        }

        private System.Windows.Forms.Label productSummary;
        private System.Windows.Forms.Label segmentSummary;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label brandSummary;
        private System.Windows.Forms.Label atributeSummary;
        private System.Windows.Forms.Label taskSummary;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox titleBox;
        private System.Windows.Forms.TextBox descBox;
        private System.Windows.Forms.Label numBrandsLabel;
        private System.Windows.Forms.Label numProductslabel;
        private System.Windows.Forms.Label numSegmentsLabel;
        private System.Windows.Forms.Label numAttributesLabel;
        private System.Windows.Forms.Label numTasksLabel;
        private System.Windows.Forms.Label numChannelsLabel;
        private System.Windows.Forms.Label label4;
        private NumericUpDown populationSizeUpDown;
        private Label label3;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private DateTime startStartDate = DateTime.MinValue;
        private Label shopWeightlabel;
        private NumericUpDown shopWeight;
        private DateTimePicker startDate;
        private DateTimePicker endDate;
        private Label label5;
        private Label label6;
        private DateTime startEndDate = DateTime.MinValue;

        public ModelSummary( ModelDb db )
            : base( db ) {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            startDate.Value = db.Model.start_date;
            endDate.Value = db.Model.end_date;

            titleBox.DataBindings.Add( "Text", theDb.Data.Model_info, "model_name" );
            descBox.DataBindings.Add( "Text", theDb.Data.Model_info, "descr" );
            populationSizeUpDown.DataBindings.Add( "Value", theDb.Data.Model_info, "pop_size" );
            this.Name = titleBox.Text;

            if( !ModelDb.Nimo )
            {
                this.shopWeight.Visible = false;
                this.shopWeightlabel.Visible = false;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing ) {
            if( disposing )
            {
                if( components != null )
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
        private void InitializeComponent() {
            this.productSummary = new System.Windows.Forms.Label();
            this.segmentSummary = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numChannelsLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numTasksLabel = new System.Windows.Forms.Label();
            this.numAttributesLabel = new System.Windows.Forms.Label();
            this.numSegmentsLabel = new System.Windows.Forms.Label();
            this.numProductslabel = new System.Windows.Forms.Label();
            this.numBrandsLabel = new System.Windows.Forms.Label();
            this.taskSummary = new System.Windows.Forms.Label();
            this.atributeSummary = new System.Windows.Forms.Label();
            this.brandSummary = new System.Windows.Forms.Label();
            this.titleBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.descBox = new System.Windows.Forms.TextBox();
            this.populationSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.shopWeightlabel = new System.Windows.Forms.Label();
            this.shopWeight = new System.Windows.Forms.NumericUpDown();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.populationSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shopWeight)).BeginInit();
            this.SuspendLayout();
            // 
            // productSummary
            // 
            this.productSummary.Location = new System.Drawing.Point( 3, 40 );
            this.productSummary.Name = "productSummary";
            this.productSummary.Size = new System.Drawing.Size( 61, 16 );
            this.productSummary.TabIndex = 0;
            this.productSummary.Text = "Products:";
            this.productSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // segmentSummary
            // 
            this.segmentSummary.Location = new System.Drawing.Point( 3, 88 );
            this.segmentSummary.Name = "segmentSummary";
            this.segmentSummary.Size = new System.Drawing.Size( 61, 16 );
            this.segmentSummary.TabIndex = 1;
            this.segmentSummary.Text = "Segments:";
            this.segmentSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.numChannelsLabel );
            this.groupBox1.Controls.Add( this.label4 );
            this.groupBox1.Controls.Add( this.numTasksLabel );
            this.groupBox1.Controls.Add( this.numAttributesLabel );
            this.groupBox1.Controls.Add( this.numSegmentsLabel );
            this.groupBox1.Controls.Add( this.numProductslabel );
            this.groupBox1.Controls.Add( this.numBrandsLabel );
            this.groupBox1.Controls.Add( this.taskSummary );
            this.groupBox1.Controls.Add( this.atributeSummary );
            this.groupBox1.Controls.Add( this.segmentSummary );
            this.groupBox1.Controls.Add( this.brandSummary );
            this.groupBox1.Controls.Add( this.productSummary );
            this.groupBox1.Location = new System.Drawing.Point( 32, 144 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 192, 160 );
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Category Structure";
            // 
            // numChannelsLabel
            // 
            this.numChannelsLabel.Location = new System.Drawing.Point( 80, 64 );
            this.numChannelsLabel.Name = "numChannelsLabel";
            this.numChannelsLabel.Size = new System.Drawing.Size( 100, 16 );
            this.numChannelsLabel.TabIndex = 11;
            this.numChannelsLabel.Text = "0";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point( 3, 64 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 58, 16 );
            this.label4.TabIndex = 10;
            this.label4.Text = "Channels:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTasksLabel
            // 
            this.numTasksLabel.Location = new System.Drawing.Point( 80, 136 );
            this.numTasksLabel.Name = "numTasksLabel";
            this.numTasksLabel.Size = new System.Drawing.Size( 100, 16 );
            this.numTasksLabel.TabIndex = 9;
            this.numTasksLabel.Text = "0";
            // 
            // numAttributesLabel
            // 
            this.numAttributesLabel.Location = new System.Drawing.Point( 80, 112 );
            this.numAttributesLabel.Name = "numAttributesLabel";
            this.numAttributesLabel.Size = new System.Drawing.Size( 100, 16 );
            this.numAttributesLabel.TabIndex = 8;
            this.numAttributesLabel.Text = "0";
            // 
            // numSegmentsLabel
            // 
            this.numSegmentsLabel.Location = new System.Drawing.Point( 80, 88 );
            this.numSegmentsLabel.Name = "numSegmentsLabel";
            this.numSegmentsLabel.Size = new System.Drawing.Size( 100, 16 );
            this.numSegmentsLabel.TabIndex = 7;
            this.numSegmentsLabel.Text = "0";
            // 
            // numProductslabel
            // 
            this.numProductslabel.Location = new System.Drawing.Point( 80, 40 );
            this.numProductslabel.Name = "numProductslabel";
            this.numProductslabel.Size = new System.Drawing.Size( 100, 16 );
            this.numProductslabel.TabIndex = 6;
            this.numProductslabel.Text = "0";
            // 
            // numBrandsLabel
            // 
            this.numBrandsLabel.Location = new System.Drawing.Point( 80, 16 );
            this.numBrandsLabel.Name = "numBrandsLabel";
            this.numBrandsLabel.Size = new System.Drawing.Size( 100, 16 );
            this.numBrandsLabel.TabIndex = 5;
            this.numBrandsLabel.Text = "0";
            // 
            // taskSummary
            // 
            this.taskSummary.Location = new System.Drawing.Point( 3, 136 );
            this.taskSummary.Name = "taskSummary";
            this.taskSummary.Size = new System.Drawing.Size( 61, 16 );
            this.taskSummary.TabIndex = 4;
            this.taskSummary.Text = "Tasks:";
            this.taskSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // atributeSummary
            // 
            this.atributeSummary.Location = new System.Drawing.Point( 3, 112 );
            this.atributeSummary.Name = "atributeSummary";
            this.atributeSummary.Size = new System.Drawing.Size( 61, 16 );
            this.atributeSummary.TabIndex = 3;
            this.atributeSummary.Text = "Attributes:";
            this.atributeSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // brandSummary
            // 
            this.brandSummary.Location = new System.Drawing.Point( 3, 16 );
            this.brandSummary.Name = "brandSummary";
            this.brandSummary.Size = new System.Drawing.Size( 61, 16 );
            this.brandSummary.TabIndex = 2;
            this.brandSummary.Text = "Brands:";
            this.brandSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // titleBox
            // 
            this.titleBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.titleBox.Location = new System.Drawing.Point( 144, 24 );
            this.titleBox.Name = "titleBox";
            this.titleBox.ReadOnly = true;
            this.titleBox.Size = new System.Drawing.Size( 336, 20 );
            this.titleBox.TabIndex = 3;
            this.titleBox.WordWrap = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 32, 24 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 56, 16 );
            this.label1.TabIndex = 4;
            this.label1.Text = "Title";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point( 32, 56 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 88, 16 );
            this.label2.TabIndex = 6;
            this.label2.Text = "Description";
            // 
            // descBox
            // 
            this.descBox.Location = new System.Drawing.Point( 144, 64 );
            this.descBox.Multiline = true;
            this.descBox.Name = "descBox";
            this.descBox.ReadOnly = true;
            this.descBox.Size = new System.Drawing.Size( 336, 56 );
            this.descBox.TabIndex = 7;
            // 
            // populationSizeUpDown
            // 
            this.populationSizeUpDown.Location = new System.Drawing.Point( 385, 232 );
            this.populationSizeUpDown.Maximum = new decimal( new int[] {
            1000000000,
            0,
            0,
            0} );
            this.populationSizeUpDown.Name = "populationSizeUpDown";
            this.populationSizeUpDown.Size = new System.Drawing.Size( 95, 20 );
            this.populationSizeUpDown.TabIndex = 8;
           
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 299, 236 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 80, 13 );
            this.label3.TabIndex = 9;
            this.label3.Text = "Population Size";
            // 
            // shopWeightlabel
            // 
            this.shopWeightlabel.AutoSize = true;
            this.shopWeightlabel.Location = new System.Drawing.Point( 243, 282 );
            this.shopWeightlabel.Name = "shopWeightlabel";
            this.shopWeightlabel.Size = new System.Drawing.Size( 136, 13 );
            this.shopWeightlabel.TabIndex = 11;
            this.shopWeightlabel.Text = "Seasonal Shopping Weight";
            // 
            // shopWeight
            // 
            this.shopWeight.Location = new System.Drawing.Point( 385, 280 );
            this.shopWeight.Name = "shopWeight";
            this.shopWeight.Size = new System.Drawing.Size( 95, 20 );
            this.shopWeight.TabIndex = 10;
            // 
            // startDate
            // 
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDate.Location = new System.Drawing.Point( 389, 148 );
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size( 91, 20 );
            this.startDate.TabIndex = 12;
            // 
            // endDate
            // 
            this.endDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endDate.Location = new System.Drawing.Point( 389, 180 );
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size( 91, 20 );
            this.endDate.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 317, 152 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 55, 13 );
            this.label5.TabIndex = 14;
            this.label5.Text = "Start Date";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point( 317, 184 );
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size( 52, 13 );
            this.label6.TabIndex = 15;
            this.label6.Text = "End Date";
            // 
            // ModelSummary
            // 
            this.Controls.Add( this.label6 );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.endDate );
            this.Controls.Add( this.startDate );
            this.Controls.Add( this.shopWeightlabel );
            this.Controls.Add( this.shopWeight );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.populationSizeUpDown );
            this.Controls.Add( this.descBox );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.titleBox );
            this.Controls.Add( this.groupBox1 );
            this.Controls.Add( this.label1 );
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "ModelSummary";
            this.Size = new System.Drawing.Size( 568, 384 );
            this.groupBox1.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.populationSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shopWeight)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }
        #endregion
    }
}
