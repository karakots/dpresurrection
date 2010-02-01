using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;
using MathUtility;
using Utilities.Graphing;

namespace MrktSimClient.Controls.Dialogs.Calibration
{
    public partial class TemplateCalibration : UserControl
    {
        private DataTable aTable;
        private MrktSimDBSchema.simulationRow currentSimulation = null;

        private int currentRun = -1;
        public int Run {
            set {
                currentRun = value;
                
                // set other values that depend on run
            }

            get {
                return currentRun;
            }
        }

        private CallibrationDb theDb;
        public CallibrationDb Db
        {
            get
            {
                return theDb;
            }

            set
            {
                if (value == null)
                    return;

                theDb = value;

                currentSimulation = theDb.Data.simulation.FindByid(value.Id);

                //initParmBox();
            }
        }

        public TemplateCalibration()
        {
            InitializeComponent();

            aTable = new DataTable( "TableName" );

            // add columns
            aTable.Columns.Add( "XXXX_id", typeof( int ) );
            aTable.Columns.Add( "XXXX", typeof( string ) );

            aTable.Columns.Add( "current_VALUE", typeof( double ) );
            aTable.Columns.Add( "step_size", typeof( double ) );
            aTable.Columns.Add( "suggested_VALUE", typeof( double ) );

            this.mrktSimGrid.Table = aTable;
           // mrktSimGrid.ReadOnly = true;
            mrktSimGrid.DescriptionWindow = false;

            plotControl1.Title = "????????????";
            plotControl1.Y2Axis = "";

            createTableStyle();

            applyButton.Enabled = false;
        }

        private void createTableStyle()
        {
            mrktSimGrid.Clear();

            mrktSimGrid.AddTextColumn("XXXX",true);
            DataGridTextBoxColumn col = mrktSimGrid.AddNumericColumn( "current_VALUE", true );
            col.Format = "N8";

            col = mrktSimGrid.AddNumericColumn( "step_size", true );
            col.Format = "N8";

            mrktSimGrid.Reset();
        }


        // local variables for charting purposes go here
       
        private void initParmBox() {
            aTable.Clear();

            // Add records to table here

            aTable.AcceptChanges();
        }

        private void solveButton_Click( object sender, EventArgs e ) {
            if( Run == -1 )
                return;

            int run_id = Run;
           
            // compute compute compute


            aTable.Clear();

            // load up table with values

            aTable.AcceptChanges();


            if( !appliedOnce ) {
                applyButton.Enabled = true;
            }

        }

        private bool appliedOnce = false;
        private void applyButton_Click(object sender, EventArgs e)
        {
            string error = null;

            foreach (DataRow pref in aTable.Rows)
            {
                // update parameters or ... model?
            }

            if( error != null ) {
                MessageBox.Show( error );
            }


            aTable.Clear();
            appliedOnce = true;

            applyButton.Enabled = false;
        }


        // what to chart

        private void updatePlot() {

            //double max = double.MinValue;
            //double min= double.MaxValue;

            this.plotControl1.ClearAllGraphs();

            plotControl1.TimeSeries = true;
            this.plotControl1.AutoScaleAxis();

            plotControl1.YAxis = "??????";

            plotControl1.Title = "??????";
            plotControl1.Y2Axis = "";
            plotControl1.XAxis = "Date";
            plotControl1.ScatterPlot = false;


            DataCurve crv = new DataCurve( 10 );

            //for( int tt = 0; tt < dates.Count; ++tt )
            //{

            //    if( max < series[tt] )
            //    {
            //        max = series[tt];
            //    }

            //    if( min > series[tt] )
            //    {
            //        min = series[tt];

            //    }

            //    crv.Add( dates[tt], series[tt] );
            //}

            crv.Label = "NAME";
            crv.Units = "#";
            crv.Color = Color.Red;

            plotControl1.Data = crv;

            plotControl1.DataChanged();
            

            plotControl1.MakeUnique();

            plotControl1.Start = DateTime.Now;
            plotControl1.End = DateTime.Now.AddYears(1);

            plotControl1.Refresh();

        }

        private void refreshBut_Click( object sender, EventArgs e ) {

            if( Run == -1 )
                return;

            updatePlot();
        }

    }
}
