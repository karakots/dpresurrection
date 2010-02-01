using System;
using System.Drawing;

namespace Utilities
{
    /// <summary>
    /// XPoint is a class suitable for locating a cell in an Excel worksheet (it eliminates any [x,y] vs. [row,col] confusion).
    /// Use this class with ExcelReaderWriter.
    /// </summary>
    public class ExcelPoint
    {
        private Point point;

        public int Row {
            get {
                return point.Y;       
            }
            set {
                point.Y = value;
            }
        }

        public int Col {
            get {
                return point.X;
            }
            set {
                point.X = value;
            }
        }

        public ExcelPoint() {
            point = new Point( 0, 0 );
        }

        public ExcelPoint( int row, int col ) {
            point = new Point( col, row );
        }
    }
}
