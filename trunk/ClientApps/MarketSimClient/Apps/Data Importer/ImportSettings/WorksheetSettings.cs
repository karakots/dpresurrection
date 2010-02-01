using System;
using System.Collections;
using System.Text;
using System.Drawing;

using DataImporter.Library;

namespace DataImporter.ImportSettings
{
    /// <summary>
    /// Settings for a general data file
    /// </summary>
    public class WorksheetSettings 
    {
        public ProjectSettings.BrandInfo Brand;
        public ProjectSettings.ChannelInfo Channel;
        public ProjectSettings.DataType DataType;

        public string SheetName;
        public bool Validated;                  // if true, the other values are also valid

        public XPoint FirstDateHeaderCell;
        public XPoint FirstSkuHeaderCell;
        public XPoint FirstChanHeaderCell;

        public bool Horizontal;                 // time progresion can be horizontal or vertical
        public bool DatesAreIntervalEnd;    // dates can be specify start or end of interval
        public bool ScanAllDateHeaders;
        public ArrayList AllDateHeaders;

        public TimeSpan TimeStep;
        public int TimeStepDays {
            set { TimeStep = new TimeSpan( value, 0, 0, 0 ); }
            get { return (int)TimeStep.TotalDays; }
        }

        public string FirstDateHeader;
        public string FirstSkuHeader;
        public string FirstChanHeader;

        public ArrayList RelatedWorksheetIndexes;       // if non-null, this worksheet is part of a multi-sheet set

        public string Campaign;
        public string Comments;

        [NonSerialized]
        public int VariantCount;
        [NonSerialized]
        public ArrayList Variants;

        [NonSerialized]
        public string OutputFile;

        [NonSerialized]
        public string InputFile;

        private ProjectSettings.ProjectSection sectionInfo;

        public ProjectSettings.ProjectSection GetSection() {
            return sectionInfo;
        }

        public void SetSection( ProjectSettings.ProjectSection section ) {
            sectionInfo = section;
        }

        private ArrayList[][] data;             // indexes are channel, variant

        public DataItem GetDataItem( int variantIndx, int timeStep ) {
            return GetDataItem( 0, variantIndx, timeStep );
        }

        public DataItem GetDataItem( int channelIndex, int variantIndx, int timeStep ) {
            ArrayList a = (ArrayList)data[ channelIndex ][ variantIndx ];
            return (DataItem)a[ timeStep ];
        }

        public ArrayList GetData( int variantIndx ) {
            return data[ 0 ][ variantIndx ];
        }

        public ArrayList GetData( int channelIndex, int variantIndx ) {
            return data[ channelIndex ][ variantIndx ];
        }

        public int DataCount {
            get {
                if( data != null && data.Length > 0 && data[ 0 ].Length > 0 ) {
                    ArrayList a = (ArrayList)data[ 0 ][ 0 ];
                    return a.Count;
                }
                else {
                    return -1;
                }
            }
        }

        public void SetData( int variantIndx, ArrayList newList ) {
            SetData( 0, variantIndx, newList );
        }

        public void SetData( int channelIndex, int variantIndx, ArrayList newList ) {
            data[ channelIndex ][ variantIndx ] = newList;
        }

        public void InitializeData( int variantCount ) {
            InitializeData( 1, variantCount );
        }

        public void InitializeData( int channelCount,  int variantCount ) {
            data = new ArrayList[ channelCount ][];
            for( int c = 0; c < channelCount; c++ ) {
                data[ c ] = new ArrayList[ variantCount ];
                for( int v = 0; v < variantCount; v++ ) {
                    data[ c ][ v ] = new ArrayList();
                }
            }
        }

        ////private ArrayList[] data;

        ////public DataItem GetData( int variantIndx, int timeStep ) {
        ////    ArrayList a = (ArrayList)data[ variantIndx ];
        ////    return (DataItem)a[ timeStep ];
        ////}

        ////public ArrayList GetData( int variantIndx ) {
        ////    return data[ variantIndx ];
        ////}

        ////public int DataCount {
        ////    get {
        ////        if( data != null && data.Length > 0 ) {
        ////            ArrayList a = (ArrayList)data[ 0 ];
        ////            return a.Count;
        ////        }
        ////        else {
        ////            return -1;
        ////        }
        ////    }
        ////}

        ////public void SetData( int variantIndx, ArrayList newList ) {
        ////    data[ variantIndx ] = newList;
        ////}

        ////public void InitializeData( int variantCount ) {
        ////    data = new ArrayList[ variantCount ];
        ////    for( int i = 0; i < variantCount; i++ ) {
        ////        data[ i ] = new ArrayList();
        ////    }
        ////}

        public string PropertiesString() {
            string s = String.Format( "\n    Worksheet Properties: {0}    \n    -----------------------------\n", this.SheetName );
            s += String.Format( "    Validated: {0}       Horizontal: {1}    \n", Validated, Horizontal );
            s += String.Format( "    Dates are end of intervals: {0}    \n", DatesAreIntervalEnd );
            s += String.Format( "    First Date Header Cell row,col= {0},{1}   Val: {2}   \n", FirstDateHeaderCell.Row, FirstDateHeaderCell.Col, FirstDateHeader );
            s += String.Format( "    First Variant Header Cell row,col= {0},{1}   Val: {2}   \n", FirstSkuHeaderCell.Row, FirstSkuHeaderCell.Col, FirstSkuHeader );
            s += String.Format( "    First Channel Header Cell row,col= {0},{1}   Val: {2}   \n", FirstChanHeaderCell.Row, FirstChanHeaderCell.Col, FirstChanHeader );
            s += String.Format( "    Time Step: {0:f0}  days     Data Type: {1}   \n", TimeStep.TotalDays, DataType.ToString() );
            s += String.Format( "    Scan all date headers: {0}    \n", ScanAllDateHeaders );
            if( RelatedWorksheetIndexes == null ) {
                s += "    Has related worksheets: false\n";
            }
            else {
                s += "    Has related worksheets: true\n";
            }
            if( Brand != null ) {
                s += String.Format( "    Brand: {0}   \n", Brand.ImportName );
            }
            else {
                s += String.Format( "    Brand: NULL   \n" );
            }
            if( Channel != null ) {
                s += String.Format( "    Channel: {0}   \n", Channel.ImportName );
            }
            else {
                s += String.Format( "    Channel: NULL   \n" );
            }
            if( OutputFile != null ) {
                s += String.Format( "    Output File: {0}   \n", OutputFile );
            }
            else {
                s += String.Format( "    Output File: NULL   \n" );
            }
            if( InputFile != null ) {
                s += String.Format( "    Input File: {0}   \n", InputFile );
            }
            else {
                s += String.Format( "    Input File: NULL   \n" );
            }
            if( sectionInfo != null ) {
                s += String.Format( "    Section Info: set   \n" );
            }
            else {
                s += String.Format( "    Section Info: NOT SET   \n" );
            }
            if( data != null ) {
                s += String.Format( "    Section Data: set   \n" );
            }
            else {
                s += String.Format( "    Section Data: NULL   \n" );
            }
            if( Campaign != null ) {
                s += String.Format( "    Campaign: {0}   \n", Campaign );
            }
            else {
                s += String.Format( "    Campaign: NULL   \n" );
            }
            if( Comments != null ) {
                s += String.Format( "    Comments: {0}   \n", Comments );
            }
            else {
                s += String.Format( "    Comments: NULL   \n" );
            }
            s += "\n\n";
            return s;
        }

        public WorksheetSettings( string worksheetName ) : this() {
            SheetName = worksheetName;
        }
        
        public WorksheetSettings() {
            SheetName = null;
            Horizontal = true;
            Validated = false;
            DatesAreIntervalEnd = true;
            ScanAllDateHeaders = false;
            FirstDateHeaderCell = new XPoint( -1, -1 );
            FirstSkuHeaderCell = new XPoint( -1, -1 );
            FirstChanHeaderCell = new XPoint( -1, -1 );
            TimeStep = new TimeSpan();
            FirstDateHeader = null;
            FirstSkuHeader = null;
            FirstChanHeader = null;
            DataType = ProjectSettings.DataType.Unknown;
            data = null;
            RelatedWorksheetIndexes = null;
        }

        public WorksheetSettings( WorksheetSettings objectToCopyFrom ) {
            SheetName = objectToCopyFrom.SheetName;
            Horizontal = objectToCopyFrom.Horizontal;
            Validated = objectToCopyFrom.Validated;
            DatesAreIntervalEnd = objectToCopyFrom.DatesAreIntervalEnd;
            ScanAllDateHeaders = objectToCopyFrom.ScanAllDateHeaders;
            AllDateHeaders = objectToCopyFrom.AllDateHeaders;
            FirstDateHeaderCell = new XPoint( objectToCopyFrom.FirstDateHeaderCell.Row, objectToCopyFrom.FirstDateHeaderCell.Col );
            FirstSkuHeaderCell = new XPoint( objectToCopyFrom.FirstSkuHeaderCell.Row, objectToCopyFrom.FirstSkuHeaderCell.Col );
            FirstChanHeaderCell = new XPoint( objectToCopyFrom.FirstChanHeaderCell.Row, objectToCopyFrom.FirstChanHeaderCell.Col );
            TimeStep = objectToCopyFrom.TimeStep;
            FirstDateHeader = objectToCopyFrom.FirstDateHeader;
            FirstSkuHeader = objectToCopyFrom.FirstSkuHeader;
            Brand = objectToCopyFrom.Brand;
            Channel = objectToCopyFrom.Channel;
            DataType = objectToCopyFrom.DataType;
            sectionInfo = objectToCopyFrom.sectionInfo;
            data = objectToCopyFrom.data;
            RelatedWorksheetIndexes = objectToCopyFrom.RelatedWorksheetIndexes;
            Campaign = objectToCopyFrom.Campaign;
            Comments = objectToCopyFrom.Comments;
        }
    }
}
