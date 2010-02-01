using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace Utilities.Graphing
{
    public partial class Plot : Form
    {
        public Plot() {
            InitializeComponent();

             this.plotControl.ZedGraphControl.IsShowPointValues = true;
             this.plotControl.ZedGraphControl.GraphPane.XAxis.Type = AxisType.Date;
             this.plotControl.ZedGraphControl.GraphPane.XAxis.Title = "Date";
             this.plotControl.ZedGraphControl.GraphPane.FontSpec.Size = 24;
             this.plotControl.ZedGraphControl.GraphPane.Legend.IsVisible = false;
             this.plotControl.ZedGraphControl.GraphPane.YAxis.IsVisible = true;
             this.plotControl.ZedGraphControl.GraphPane.YAxis.IsShowGrid = true;
             this.plotControl.ZedGraphControl.GraphPane.YAxis.IsShowMinorGrid = true;
             this.plotControl.ZedGraphControl.GraphPane.Y2Axis.IsVisible = false;
             this.plotControl.ZedGraphControl.GraphPane.Y2Axis.IsShowGrid = true;
             this.plotControl.ZedGraphControl.GraphPane.Y2Axis.IsShowMinorGrid = true;
             this.plotControl.ZedGraphControl.GraphPane.Y2Axis.Type = ZedGraph.AxisType.Linear;
             this.plotControl.ZedGraphControl.GraphPane.Y2Axis.IsShowTitle = true;
             this.plotControl.ZedGraphControl.GraphPane.Y2Axis.Title = "Percent";
             this.plotControl.ZedGraphControl.GraphPane.Y2Axis.Max = 100;
             this.plotControl.ZedGraphControl.GraphPane.Y2Axis.Min = 0;

             plotControl.AutoScaleAxis();

             if( this.CurveSelected != null ) {
                 this.plotControl.CurveSelected += new PlotControl.SelectedCurve( this.CurveSelected );
             }
             this.plotControl.PlotSelected += new PlotControl.SelectMe( plotControl_PlotSelected );
             this.plotControl.LegendVisibilitySet += new PlotControl.SetLegendVisibility( plotControl_SetLegendVisibility );

            this.plotControl.Send.Visible = true;
        }

        public bool ActivePlot {
            set {
                if( value ) {
                    this.plotControl.ZedGraphControl.GraphPane.FontSpec.Size = 12;
                    this.plotControl.ZedGraphControl.GraphPane.IsFontsScaled = false;
                    this.plotControl.ZedGraphControl.Cursor = System.Windows.Forms.Cursors.Arrow;
                    this.plotControl.Send.Visible = false;
                    //this.plotControl.ZedGraphControl.GraphPane.Legend.IsVisible = true;
                    this.plotControl.HideLegendItem.Visible = true;
                }
                else {
                    this.plotControl.ZedGraphControl.GraphPane.FontSpec.Size = 24;
                    this.plotControl.ZedGraphControl.GraphPane.IsFontsScaled = true;
                    this.plotControl.ZedGraphControl.Cursor = System.Windows.Forms.Cursors.Hand;
                    this.plotControl.Send.Visible = true;
                    //this.plotControl.ZedGraphControl.GraphPane.Legend.IsVisible = false;
                    this.plotControl.HideLegendItem.Visible = false;
                }

                this.plotControl.ZedGraphControl.Invalidate();
            }

            get {
                return !this.plotControl.Send.Visible;       // active plot has no visble send button
            }
        }

        public delegate void SelectMe( Plot me );
        public event SelectMe PlotSelected;
        public delegate void SelectedCurve( int index );
        public event SelectedCurve CurveSelected;
        public delegate void SetLegendVisibility( bool show );
        public event SetLegendVisibility LegendVisibilitySet;

        private void plotControl_PlotSelected( PlotControl selectedPlotControl ) {
            if( this.Dock == System.Windows.Forms.DockStyle.Fill ) {
                return;
            }

            if( PlotSelected != null ) {
                PlotSelected( this );
            }
        }

        private void plotControl_SetLegendVisibility( bool legendVisible ) {
            if( LegendVisibilitySet != null ) {
                LegendVisibilitySet( legendVisible );
            }
        }

        public bool TimeSeries {
            set { plotControl.TimeSeries = value; }
            get { return plotControl.TimeSeries; }
        }

        public bool ScatterPlot {
            set { plotControl.ScatterPlot = value; }
            get { return plotControl.ScatterPlot; }
        }

        public ZedGraph.CurveList Curves {
            get { return plotControl.Curves; }
        }

        public DataCurve Data {
            set { plotControl.Data = value; }
        }

        public void PushToBack( LineItem crv ) {
            plotControl.PushToBack( crv );
        }

        public bool MoveForward( LineItem crv ) {
            return plotControl.MoveForward( crv );
        }

        public bool MoveBackward( LineItem crv ) {
            return plotControl.MoveBackward( crv );
        }

        public void AutoScaleAxis() {
            plotControl.AutoScaleAxis();
        }

        public DateTime Start {
            set { plotControl.Start = value; }
            get { return plotControl.Start; }
        }

        public double MinX {
            set { plotControl.MinX = value; 
}
            get { return plotControl.MinX; }
        }

        public DateTime End {
            set { plotControl.End = value; }
            get { return plotControl.End; }
        }

        public double MaxX {
            set { plotControl.MaxX = value; }
            get { return plotControl.MaxX; }
        }

        public double Min {
            set { plotControl.Min = value; }
            get { return plotControl.Min; }
        }

        public double Max {
            set { plotControl.Max = value; }
            get { return plotControl.Max; }
        }

        public double PercentMin {
            set { plotControl.PercentMin = value; }
            get { return plotControl.PercentMin; }
        }

        public double PercentMax {
            set { plotControl.PercentMax = value; }
            get { return plotControl.PercentMax; }
        }

        public Bitmap Image {
            get { return plotControl.Image; }
        }

        public string Title {
            set { plotControl.Title = value; }
            get { return plotControl.Title; }
        }

        public string YAxis {
            set { plotControl.YAxis = value; }
            get { return plotControl.YAxis; }
        }

        public string XAxis {
            set { plotControl.XAxis = value; }
            get { return plotControl.XAxis; }
        }

        public void DataChanged() {
            plotControl.DataChanged();
        }

        public void Scale() {
            plotControl.Scale();
        }

        public void RemoveScaling() {
            plotControl.RemoveScaling();
        }

        public void Write( System.IO.StreamWriter writer ) {
            plotControl.Write( writer, false, null, null );
        }

        public void Write( System.IO.StreamWriter writer, bool useXYTableFormat, string xyTableHeading ) {
            plotControl.Write( writer, useXYTableFormat, xyTableHeading, null );
        }

        public void Write( ArrayList listOfPointLists ) {
            plotControl.Write( null, true, "", listOfPointLists );
        }

        public int CrvScale( int index ) {
            return plotControl.CrvScale( index );
        }

        public void SetCrvScale( int[] newCrvScales, int[] existingCrvScales ) {
            plotControl.SetCrvScale( newCrvScales, existingCrvScales );
        }

        public void ScaleCrv( int index, int power ) {
            plotControl.ScaleCrv( index, power );
        }

        public bool LegendVisible {
            get {
                return plotControl.LegendVisible;
            }
            set {
                plotControl.LegendVisible = value;
            }
        }
    }
}