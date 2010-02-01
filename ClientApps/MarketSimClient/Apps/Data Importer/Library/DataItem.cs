using System;

namespace DataImporter.Library
{
    public class DataItem
    {
        public DateTime StartDate;
        public DateTime EndDate;

        public double Value1;
        public double Value2;

        public DataItem( DateTime start, DateTime end, double val ) {
            this.StartDate = start;
            this.EndDate = end;
            this.Value1 = val;
            this.Value2 = 0;
        }

        public DataItem( DateTime start, DateTime end, double val1, double val2 )
            : this( start, end, val1 ) {
            this.Value2 = val2;
        }

        public DataItem( DataItem itemToCopy ){
            this.StartDate = itemToCopy.StartDate;
            this.EndDate = itemToCopy.EndDate;
            this.Value1 = itemToCopy.Value1;
            this.Value2 = itemToCopy.Value2;
        }

    }
}
