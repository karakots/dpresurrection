using System;
using System.Collections;
using System.Text;
using System.Drawing;

namespace Results.Standardized
{
    public class MarketingIOPlotCurveInfo
    {
        private string curveName;
        private double valueOffset;
        private double maxDisplayedValue;
        private double minDisplayedValue;
        private double valueRange;
        private Color lineColor;
        private bool fillCurve;
        private bool fillBottom;
        private Color fillColor;
        private double transparancy;
        private double lineWidth;
        private double outlineWIdth;
        private Pen pen;
        private Brush brush;
        private int zIndex;

        public string CurveName {
            set { curveName = value; }
            get { return curveName; }
        }
        public double ValueOffset {
            set { valueOffset = value; }
            get { return valueOffset; }
        }
        public double MaxDisplayedValue {
            set { 
                maxDisplayedValue = value;
                this.valueRange = maxDisplayedValue - minDisplayedValue;
            }
            get { return maxDisplayedValue; }
        }
        public double MinDisplayedValue {
            set {
                minDisplayedValue = value;
                this.valueRange = maxDisplayedValue - minDisplayedValue;
            }
            get { return minDisplayedValue; }
        }
        public double ValueRange {
            get { return valueRange; }
        }
        public Color LineColor {
            get { return lineColor; }
        }
        public Color FillColor {
            get { return fillColor; }
        }
        public bool FillCurve {
            get { return fillCurve; }
        }
        public bool FillBottom {
            get { return fillBottom; }
        }
        public double Transparancy {
            get { return transparancy; }
        }
        public double LlineWidth {
            get { return lineWidth; }
        }
        public double OutlineWIdth {
            get { return outlineWIdth; }
        }
        public Pen Pen {
            get { return pen; }
        }
        public Brush Brush {
            get { return brush; }
        }
        public int ZIndex {
            get { return zIndex; }
        }

        public MarketingIOPlotCurveInfo( string name, double offset, double min, double max, Color color, int zIndex, double lineWidth ) {
            this.curveName = name;
            this.valueOffset = offset;
            this.minDisplayedValue = min;
            this.maxDisplayedValue = max;
            this.lineColor = color;
            this.fillColor = color;
            this.lineWidth = lineWidth;
            this.outlineWIdth = 1;
            this.transparancy = 0;
            this.fillCurve = false;
            this.fillBottom = false;
            this.zIndex = zIndex;
            this.lineWidth = lineWidth;

            this.valueRange = max - min;
            SetPenAndBrush();
        }

        public void SetFillled( double transparency ) {
            SetFillled( transparency, true );
        }

        public void SetFillled( double transparency, bool fillBottom ) {
            this.transparancy = transparency;
            this.fillCurve = true;
            this.fillBottom = fillBottom;

            int alpha = (int)Math.Round( transparancy * 255 / 100 );
            this.lineColor = Color.FromArgb( alpha, this.lineColor );
            this.fillColor = Color.FromArgb( alpha, this.fillColor );
            SetPenAndBrush();
        }

        private void SetPenAndBrush() {
            this.brush = new SolidBrush( this.fillColor );
            this.pen = new Pen( this.brush, (float)this.lineWidth );
        }
    }
}
