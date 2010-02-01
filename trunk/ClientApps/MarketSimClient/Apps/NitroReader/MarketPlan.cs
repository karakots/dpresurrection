using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using NitroReader.Dialogs;
using NitroReader.Output;
using NitroReader.Library;

namespace NitroReader
{
    /// <summary>
    /// A MarketPlan object is used in conjunction with NITRO data translation and importing.
    /// </summary>
    public class MarketPlan
    {
        public string Name;
        public ArrayList Components;
        public DateTime StartDate;
        public DateTime EndDate;
        public ArrayList Variants;

        private OutputSheetHandlerInfo[] outputSheetHandlerInfo = new OutputSheetHandlerInfo[]{
            new OutputSheetHandlerInfo( "Products", 4, 2, null ),
            new OutputSheetHandlerInfo( "Price", 4, 1, new PriceSheetWriter() ),
            new OutputSheetHandlerInfo( "Display", 4, 1, new DisplaySheetWriter() ),
            new OutputSheetHandlerInfo( "Distribution", 4, 1, new DistributionSheetWriter() ),
            new OutputSheetHandlerInfo( "Real Sales Data", 4, 1, new RealSalesDataSheetWriter() )
        };

        public static ArrayList errors;
        public static ArrayList warnings;

        /// <summary>
        /// Create a new MarketPlan object.
        /// </summary>
        public MarketPlan() {
            Components = new ArrayList();
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MaxValue;
            Variants = new ArrayList();
            errors = new ArrayList();
            warnings = new ArrayList();
        }

        ///// <summary>
        ///// Returns the names of the sheets in the file the user has selected
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <returns></returns>
        //public string[] GetNitroSheetNames( ExcelInterface.ExcelReader2 reader ) {
        //    // get the names of the sheets (which are also the measure names)
        //    string[] sheetNames = reader.GetExcelSheetNames();
        //    return sheetNames;
        //}

        /// <summary>
        /// Validates a NITRO file by reading the dates headings and variant names and making sure all sheets are consistent.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>true if the file appears to ba a NITRO file</returns>
        /// <remarks>Calling ths method creates the Components and Variants lists, sets StartDate, EndDate, TimeStep, StepCount and
        /// sets component names.  Component Item values are not loaded.</remarks>
        public bool ValidateNitro( ExcelWriter2 reader, string fileName ) {

            errors = new ArrayList();
            warnings = new ArrayList();
            ArrayList knownDateSets = new ArrayList();

            reader.Open( fileName );
            string[] sheetNames = reader.GetSheetNames();
            // loop over the worksheets
            for( int i = 0; i < sheetNames.Length; i++ ) {

                // read one worksheet
                string sheetName = sheetNames[ i ];
                reader.SetSheet( sheetName );

                // create the plan component for the worksheet
                MarketPlan.Component comp = new MarketPlan.Component( sheetName );
                this.Components.Add( comp );

                // determine the date range for the component by reading the appropriate column headings
                string err = comp.ValidateNitroSheetDates( reader, knownDateSets );
                if( err != null ) {
                    errors.Add( err );
                }

                // determine the variant items in the file by reading the appropriate row headings
                string err2 = comp.ReadNitroSheetVariants( reader );
                if( err2 != null ) {
                    errors.Add( err2 );
                }

                // set the overall start/end date and verify that all components agree
                if( i == 0 ) {
                    // the first item sets the reference
                    this.StartDate = comp.StartDate;
                    this.EndDate = comp.EndDate;
                    this.Variants = comp.Variants;
                }
                else {
                    // check items after the first for agreement
                    // check start date
                    if( comp.StartDate != this.StartDate ) {
                        if( comp.StartDate < this.StartDate ) {
                            warnings.Add( String.Format( "Warning: The interval from {0} to {1} will be ignored since not all data covers this interval",
                                comp.StartDate.ToShortDateString(), this.StartDate.AddDays( - 1 ).ToShortDateString() ) );
                        }
                        else {
                            warnings.Add( String.Format( "Warning: The interval from {0} to {1} will be ignored since not all data covers this interval",
                               this.StartDate.ToShortDateString(), comp.StartDate.AddDays( -1 ).ToShortDateString() ) );
                            this.StartDate = comp.StartDate;
                        }
                    }

                    // check end date
                    if( comp.EndDate != this.EndDate ) {
                        if( comp.EndDate < this.EndDate ) {
                            warnings.Add( String.Format( "Warning: The interval from {0} to {1} will be ignored since not all data covers this interval",
                                comp.EndDate.AddDays( 1 ).ToShortDateString(), this.EndDate.ToShortDateString() ) );
                            this.EndDate = comp.EndDate;
                        }
                        else {
                            warnings.Add( String.Format( "Warning: The interval from {0} to {1} will be ignored since not all data covers this interval",
                               this.EndDate.AddDays( 1 ).ToShortDateString(), comp.EndDate.ToShortDateString() ) );
                        }
                    }

                    //check variants
                    //bool variantsMatch = true;
                    if( this.Variants.Count == comp.Variants.Count ) {

                        foreach( VariantInfo info in this.Variants ) {

                            bool foundThisOne = false;
                            foreach( VariantInfo compInfo in comp.Variants ) {

                                if( compInfo.Name == info.Name && compInfo.RowNumber == info.RowNumber ) {
                                    // found a match
                                    foundThisOne = true;
                                    break;
                                }
                            }
                            if( foundThisOne == false ) {
                                errors.Add( String.Format( "Variant name {0} not found in sheet {1}", info.Name, reader.SheetName ) );
                                //variantsMatch = false;
                                break;
                            }
                        }
                    }
                    else {
                        //variantsMatch = false;
                        errors.Add( String.Format( "Variant name count ({0}) on sheet {1} differs from count on first sheet ({2})",
                            reader.SheetName, comp.Variants.Count, this.Variants.Count  ) );
                    }
                }

            } // end of loop over worksheets

             return (errors.Count == 0);
        }

        /// <summary>
        /// Reads the actual component measure (data) values from a validated NITRO file.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>true if the data was read successfully</returns>
        /// <remarks>ValidateNitro() must be called before this method.</remarks>
        public bool ReadNitro( ExcelWriter2 reader ) {
            bool readOK = true;

            for( int i = 0; i < this.Components.Count; i++ ) {
                MarketPlan.Component comp = (MarketPlan.Component)this.Components[ i ];
                bool ok = comp.ReadData( reader );
                if( ok == false ) {
                    readOK = false;
                    break;
                }
            }

            return readOK;
        }

        /// <summary>
        /// Returns true if its okat to go ahead and write the output without further tests.  Deletes the file if it is already present and the user OKs.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="templateFile"></param>
        public bool OkToWriteExcel( string filename ) {
            if( File.Exists( filename ) ) {
                string msg = String.Format( "    Do you want to replace the existing file named \"{0}\" ?    ", filename );
                ConfirmForm cform = new ConfirmForm( msg, "Confirm Overwrite" );
                DialogResult resp = cform.ShowDialog();
                if( resp != DialogResult.OK ) {
                    // preserve the existing file
                    return false;    
                }
                else {
                    // remove an existing file
                    try {
                        File.Delete( filename );
                    }
                    catch( Exception e ) {
                        string m2 = String.Format( "Error: Unable to delete file.\r\n{0}", e.Message );
                        ConfirmForm cform2 = new ConfirmForm( m2, "Delete Failed" );
                        cform2.HideCancel();
                        cform2.ShowDialog();
                        return false;
                    }
                    return true;
                }
            }
            else {
                // the file does not exist
                return true;
            }
        }

        /// <summary>
        /// Writes the market plan data to an Excel file that can be imported into MarketSim.  The file starts as a copy of a template, then the data is inserted.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="templateFile"></param>
        ///<remarks>Assumes that the given filename does not exist already (i.e. that OkToWriteExcel() has ben called first).</remarks>
        public bool WriteExcel( string filename, string templateFile, Settings settings, ExcelWriter2  writer ) {
            string fullpath = filename;
            File.Copy( templateFile, fullpath, true );

            // be sure we start with a fresh writer object
            if( writer != null ) {
                writer.Kill();
                writer = null;
            }
            writer = new ExcelWriter2();
            // open the newly copied file
            writer.Open( fullpath, outputSheetHandlerInfo[ 0 ].Name );

            //create the products list now
            int visrow = 0;
            for( int row = 0; row < this.Variants.Count; row++ ) {
                VariantInfo vinfo = (VariantInfo)this.Variants[ row ];
                if( (vinfo.GroupIndex != -1) && (vinfo.IsGroup == false) ) {
                    // the item is in a group - skip it
                    continue;
                }
                writer.FillCell( visrow + outputSheetHandlerInfo[ 0 ].DataStartRow, outputSheetHandlerInfo[ 0 ].DataStartColumn,
                    settings.GetMarketSimName( vinfo.Name ) );
                visrow++;
            }

            //create the other data worksheets
            for( int sheet = 1; sheet < outputSheetHandlerInfo.Length; sheet++ ) {
                OutputSheetHandlerInfo outInfo = outputSheetHandlerInfo[ sheet ];
                outInfo.WorksheetWriter.WriteData( outInfo.Name, outInfo.DataStartRow, outInfo.DataStartColumn, this, settings, writer );
            }

            //make the Products sheet (the first sheet) the current sheet again so that one is shown when the file is opened
            writer.SetSheet( "Products" );

            writer.SaveAndClose();

            return true;
        }

        /// <summary>
        /// Returns the Component with the given name.  
        /// </summary>
        /// <param name="componentName"></param>
        /// <returns>null if the named component isn't found</returns>
        /// <remarks>Needed since we can't be sure of the sequence of the components.</remarks>
        public MarketPlan.Component GetComponent( string componentName ) {
            MarketPlan.Component namedComponent = null;
            foreach( Component comp in this.Components ) {
                if( comp.Name == componentName ) {
                    namedComponent = comp;
                    break;
                }
            }
            return namedComponent;
        }

        /// <summary>
        /// Returns the input string, with enclosing single quotes if the string contains any blanks.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        string QuoteIfNecessary( string s ) {
            if( s.IndexOf( " " ) != -1 ) {
                return "'" + s + "'";        //enclose a string with embedded blank(s) in single quotes
            }
            else {
                return s;
            }
        }

        /// <summary>
        /// Writes the errors to the console
        /// </summary>
        public void WriteErrorsToConsole( bool warningsToo ) {
            for( int i = 0; i < errors.Count; i++ ) {
                Console.WriteLine( (string)errors[ i ] );
            }
            if( warningsToo && (warnings.Count > 0) ) {
                Console.WriteLine( "\r\nWarnings:\r\n" );
                for( int i = 0; i < warnings.Count; i++ ) {
                    Console.WriteLine( (string)warnings[ i ] );
                }
            }
        }

        /// <summary>
        /// VariantInfo contains the information needed to identify and locate a given variant.
        /// </summary>
        public class VariantInfo
        {
            public string MarketSimName;
            public string Name;
            public string Brand;
            public int RowNumber;
            public Color BackColor;
            public bool IsGroup;
            public bool GroupExpanded;       // relevant only if IsGroup == true
            public bool Visible;                    // relevant only for subnodes
            public int GroupIndex;
            public int Volume;

            public VariantInfo( string name, int row ) {
                this.Name = name;
                this.RowNumber = row;
                this.MarketSimName = name;        //use same name as in NITRO by default
                this.IsGroup = false;
                this.BackColor = Color.White;
                this.Visible = true;
                this.GroupIndex = -1;
                this.Volume = 0;
            }
        }

        /// <summary>
        /// Encapsulization of information needed to create a particular output sheet.
        /// </summary>
        public class OutputSheetHandlerInfo
        {
            public string Name;
            public int DataStartRow;
            public int DataStartColumn;
            public IMarketPlanSheetWriter WorksheetWriter;

            public OutputSheetHandlerInfo( string name, int dataStartRow, int dataStartColumn, IMarketPlanSheetWriter worksheetWriter ) {
                this.Name = name;
                this.DataStartRow = dataStartRow;
                this.DataStartColumn = dataStartColumn;
                this.WorksheetWriter = worksheetWriter;
            }
        }

        /// <summary>
        /// Component represents a market plan component.
        /// </summary>
        public class Component
        {
            public string Name;
            public string Type;
            public ArrayList[] Data;
            public DateTime StartDate;
            public DateTime EndDate;
            public TimeSpan TimeStep;
            public int StepCount;
            public ArrayList Variants;
            public ArrayList Channels;
            public double Awareness;
            public double Persuasion;

            private bool row1HasDates = false;         // flag indicating alternate Excel format
            public int StartDataRow = 4;

            public Component( string name ) {
                this.Name = name;
                this.Variants = new ArrayList();
                this.Channels = new ArrayList();
                this.Awareness = 0.1;                  
                this.Persuasion = 0.1;                  
            }

            /// <summary>
            /// Validates the date headings of the appropriate Excel worksheet assuming a standard NITRO file format.
            /// </summary>
            /// <param name="reader"></param>
            /// <returns>null if the read was a success; otherwise the error description is returned</returns>
            public string ValidateNitroSheetDates(  ExcelWriter2 reader, ArrayList knownDateSets ) {
                int sheetNameCellRow = 1;
                int sheetNameCellCol = 3;
                int date0CellRow = 2;
                int date0CellCol = 3;

                Console.WriteLine( "ValidateNitroSheetDates()..." );
                int date0CellRow_Format2 = 1;
                int date0CellCol_Format2 = 3;

                string header1 = "WEEK ENDING ";
                string header2 = "4 WEEKS ENDING ";

                this.StepCount = 0;
                this.row1HasDates = false;       // the default format
                reader.SetSheet( this.Name );

                object testSObj = reader.GetValue( sheetNameCellRow, sheetNameCellCol );

                if( (testSObj == null) || (testSObj is DBNull) ) {
                    return String.Format( "Sheet name not found (cell empty). Cell X,Y = {1},{0}, Sheet= {2}", sheetNameCellRow, sheetNameCellCol, reader.SheetName );
                }
                string shtName = (string)testSObj;

                string testsht26 = shtName;     //Excel sheet names are sometimes truncated at 25 chars in length
                if( shtName.Length > 25 ) {
                    testsht26 = shtName.Substring( 0, 25 );
                }

                string test = null;
                string header = null;

                if( (this.Name == shtName) || (this.Name == testsht26) ) {
                    // looking like a NITRO file so far...look for the date headers
                    this.Name = shtName;          // re-set the name just in case it had been truncated as a sheet name

                    object testObj = reader.GetValue( date0CellRow, date0CellCol );
                    if( (testObj == null) || (testObj is DBNull) ) {
                        return String.Format( "Date column heading not found (cell empty). Cell X,Y = {1},{0}, Sheet= {2}", date0CellRow, date0CellCol, reader.SheetName );
                    }
                    test = (string)testObj;

                    if( test.StartsWith( header1 ) ) {
                        this.StartDate = DateTime.Parse( test.Substring( header1.Length ) ).AddDays( -7 );
                        this.TimeStep = new TimeSpan( 7, 0, 0, 0 );
                        header = header1;
                    }
                    else if( test.StartsWith( header2 ) ) {
                        this.StartDate = DateTime.Parse( test.Substring( header2.Length ) ).AddDays( -28 );
                        this.TimeStep = new TimeSpan( 28, 0, 0, 0 );
                        header = header2;
                    }
                    else {
                        return String.Format( "Date column heading invalid. Cell X,Y = {1},{0}, Sheet= {2}", date0CellRow, date0CellCol, reader.SheetName );
                    }
                }
                else {
                    // we didn't find the sheet name in the top row - see if perhaps this is the format without a sheet name row
                    this.row1HasDates = true;     // set the alternate format flag
                    StartDataRow = 3;
                    date0CellRow = date0CellRow_Format2;
                    date0CellCol = date0CellCol_Format2;
                    object testObj = reader.GetValue( date0CellRow, date0CellCol );
                    if( (testObj == null) || (testObj is DBNull) ) {
                        return String.Format( "Date column heading not found (cell empty). Cell X,Y = {1},{0}, Sheet= {2}", date0CellRow, date0CellCol, reader.SheetName );
                    }
                    test = (string)testObj;

                    if( test.StartsWith( header1 ) ) {
                        this.StartDate = DateTime.Parse( test.Substring( header1.Length ) ).AddDays( -7 );
                        this.TimeStep = new TimeSpan( 7, 0, 0, 0 );
                        header = header1;
                    }
                    else if( test.StartsWith( header2 ) ) {
                        this.StartDate = DateTime.Parse( test.Substring( header2.Length ) ).AddDays( -28 );
                        this.TimeStep = new TimeSpan( 28, 0, 0, 0 );
                        header = header2;
                    }
                    else {
                        return String.Format( "Date column heading invalid. Cell X,Y = {1},{0}, Sheet= {2}", date0CellRow, date0CellCol, reader.SheetName );
                    }
                }

                // see if this is a repeat of a previously-seen date headings set
                bool seenBefore = false;
                object[] seenBeforeInfo = null;
                foreach( object[] dsinfo in knownDateSets ) {
                    string knownHdr0 = (string)dsinfo[ 0 ];
                    if( knownHdr0 == test ) {
                        seenBeforeInfo = dsinfo;
                        seenBefore = true;
                    }
                }

                if( seenBefore == false ) {
                    int dataCellRow = date0CellRow;
                    int dataCellCol = date0CellCol;
                    string hdrStr = test;
                    //process the remaining date headers
                    do {
                        dataCellCol += 1;
                        object hdrObj = reader.GetValue( dataCellRow, dataCellCol );
                        if( (hdrObj == null) || (hdrObj is DBNull) ) {
                            // we've hit the end of the date headings - the last hdrStr was the end date
                            this.EndDate = DateTime.Parse( hdrStr.Substring( header.Length ) );
                            break;
                        }

                        hdrStr = (string)hdrObj;
                        if( hdrStr.StartsWith( header ) == false ) {
                            // we hit a non-date-header item before a blank cell
                            return String.Format( "Date column heading invalid. Cell X,Y = {1},{0}, Sheet= {2}", dataCellRow, dataCellCol, reader.SheetName );
                            //return String.Format( "Date column heading invalid. Cell = {0}, Sheet= {1}, Value = {2}", date0Cell, reader.SheetName, hdrStr );
                        }

                        // this date header is valid
                        this.StepCount += 1;
                    } while( true );

                    object[] newDsinfo = new object[ 5 ];
                    newDsinfo[ 0 ] = test;
                    newDsinfo[ 1 ] = dataCellCol - 1;
                    newDsinfo[ 2 ] = hdrStr;
                    newDsinfo[ 3 ] = this.EndDate;
                    newDsinfo[ 4 ] = this.StepCount;
                    knownDateSets.Add( newDsinfo );
                }
                else {
                    // we've seen this one before
                    Console.WriteLine( "Seen this date set before...skippimg..." );
                    this.EndDate = (DateTime)seenBeforeInfo[ 3 ];
                    this.StepCount = (int)seenBeforeInfo[ 4 ];
                    int lastCol = (int)seenBeforeInfo[ 1 ];
                    string lastStr = (string)seenBeforeInfo[ 2 ];

                    // doublecheck...
                    object hdrXObj = reader.GetValue( date0CellRow, lastCol );
                    if( (hdrXObj == null) || (hdrXObj is DBNull) ) {
                        return String.Format( "Date column heading invalid (empty). Cell X,Y = {1},{0}, Sheet= {2}.  Expected: {3}",
                            date0CellRow, lastCol, reader.SheetName, lastStr );
                    }

                    string hdrXStr = (string)hdrXObj;
                    if( hdrXStr != lastStr ) {
                        // we hit a non-mathcing item in the last cell
                        return String.Format( "Date column heading invalid. Cell X,Y = {1},{0}, Sheet= {2}.  Expected: {3}",
                            date0CellRow, lastCol, reader.SheetName, lastStr );
                    }
                    //...and make sure the next cell is empty
                    hdrXObj = reader.GetValue( date0CellRow, lastCol +1 );
                    if( (hdrXObj != null) && ((hdrXObj is DBNull) == false) ) {
                        return String.Format( "Date column heading invalid (expected an empty cell). Cell X,Y = {1},{0}, Sheet= {2}.  Expected: {3}\r\nMeasure sheets do not have consistent end dates!",
                            date0CellRow, lastCol, reader.SheetName, lastStr );
                    }
                }

                return null;
            }

            /// <summary>
            /// Reads the Variants list from the appropriate cells in the appropriate sheet.
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            public string ReadNitroSheetVariants( ExcelWriter2 reader ) {
                int channel0CellRow = 3;
                int channel0CellCol = 1;
                int variant0CellRow = 4;
                int variant0CellCol = 2;
                StartDataRow = 4;

                if( this.row1HasDates ) {       // alternate format - move up a row
                    variant0CellRow -= 1;
                    channel0CellRow -= 1;
                    StartDataRow -= 1;
               }
  
                this.Variants = new ArrayList();
                reader.SetSheet( this.Name );

                object testCObj = reader.GetValue( channel0CellRow, channel0CellCol );
                if( (testCObj == null) || (testCObj is DBNull) ) {
                    return String.Format( "Channel name not found (cell empty). Cell  X,Y = {0},{1}, Sheet= {2}", channel0CellCol, channel0CellRow, reader.SheetName );
                }
                string channel = (string)testCObj;
                this.Channels.Add( channel );

                object testObj = reader.GetValue( variant0CellRow, variant0CellCol );
                if( (testObj == null) || (testObj is DBNull) ) {
                    return String.Format( "Variant name not found (cell empty). Cell  X,Y = {0},{1}, Sheet= {2}", variant0CellCol, variant0CellRow, reader.SheetName );
                }
                string test = (string)testObj;
                VariantInfo info = new VariantInfo( test, 0 );
                this.Variants.Add( info );

                int dataCellRow = variant0CellRow;
                int dataCellCol = variant0CellCol;
                do {
                    dataCellRow += 1;
                    object varObj = reader.GetValue( dataCellRow, dataCellCol );
                    if( (varObj == null) || (varObj is DBNull) ) {
                        // we've hit the end of the varisnts list
                        break;
                    }

                    string varName = (string)varObj;
                    VariantInfo vinfo = new VariantInfo( varName, dataCellRow - variant0CellRow );
                    foreach( VariantInfo testVariant in this.Variants ) {
                        if( testVariant.Name == vinfo.Name ) {
                            return String.Format( "Duplicate variant name found ({3})! Cell  X,Y = {0},{1}, Sheet= {2}", dataCellCol, dataCellRow, reader.SheetName, vinfo.Name );
                        }
                    }
                    this.Variants.Add( vinfo );

                } while( true );

                return null;
            }

            /// <summary>
            /// Read the actual data values from the appropriate cells in the appropriate sheet.
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            public bool ReadData( ExcelWriter2 reader ) {
                bool hadErrors = false;
                reader.SetSheet( this.Name );
                this.Data = new ArrayList[ this.Variants.Count ];

                for( int i = 0; i < this.Variants.Count; i++ ) {
                    MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)this.Variants[ i ];

                    DateTime d0 = this.StartDate;
                    DateTime d1 = d0.Add( this.TimeStep );
                    this.Data[ i ] = new ArrayList();
                    double val = -1;

                    int cellRow = vinfo.RowNumber + this.StartDataRow;
                    int cellCol = 3;

                    object[ , ] valObjs = (object[ , ])reader.GetValues( cellRow, cellCol, cellRow, cellCol + this.StepCount );

                    for( int k = 0; k <= this.StepCount; k++ ) {
                        object valObj = valObjs[ 1, k + 1 ];
//                        object valObj = reader.GetValue( cellRow, cellCol );


                        if( (valObj == null) || (valObj is DBNull) ) {
                            // a blank cell -- make it 0
                            val = 0;
                            MarketPlan.warnings.Add( String.Format( "Warning: Blank cell found in ReadData( X,Y= {1},{0} )  Sheet: {2}", cellRow, cellCol, reader.SheetName ) );
                        }
                        else {
                            if( valObj is double ) {
                                val = (double)valObj;
                            }
                            else if( valObj is string ) {
                                string tmp = (string)valObj;
                                tmp = tmp.Trim();

                                if( tmp == "-" ) {
                                    // a dash in a cell is the standard NITRO way of indicating "nearly zero".
                                    val = 0;
                                }
                                else if( tmp.Length == 0 ) {
                                    // a cell containing only blanks!
                                    val = 0;
                                    MarketPlan.warnings.Add( String.Format( "Warning: Blank cell found in ReadData( X,Y= {1},{0} )  Sheet: {2}", cellRow, cellCol, reader.SheetName ) );
                                }
                            }
                        }
                        // now we know the cell value -- add it
                        Item item = new Item( d0, d1, val );
                        Data[ i ].Add( item );

                        //get ready for the next cell
                        cellCol += 1;
                        d0 = d0.Add( this.TimeStep );
                        d1 = d1.Add( this.TimeStep );
                    }
                }
                return hadErrors;
            }

            public bool WriteCSVFile() {
                string csvFilename = this.Name + ".cmp";
                StreamWriter sw = new StreamWriter( new FileStream( csvFilename, FileMode.Create, FileAccess.Write ) );
                
                // write the special first line
                sw.WriteLine( String.Format( "'{0}'", this.Name ) );

                // write the data lines
                for( int i = 0; i < this.Variants.Count; i++ ){
                    VariantInfo info = (VariantInfo)this.Variants[ i ];
                     sw.Write( "{0},", info.Name );
                   for( int k = 0; k < this.StepCount; k++ ){
                       Item data = (Item)(this.Data[ i ][ k ]);
                       sw.WriteLine( "{0},{1},{2}", data.Value1, data.StartDate.ToShortDateString(), data.EndDate.ToShortDateString() );
                   }
                }
                sw.Flush();
                sw.Close();
                return true;
            }

            /// <summary>
            /// Writes data values to the console.
            /// </summary>
            public void WriteDataToConsole() {
                Console.WriteLine( "Data for {0}:", this.Name );
                for( int varindx = 0; varindx < this.Variants.Count; varindx++ ) {
                    Console.WriteLine( "\r\n {0}:", ((MarketPlan.VariantInfo)this.Variants[ varindx ]).Name );
                    for( int i = 0; i < Data[ varindx ].Count; i++ ) {
                        Item dataItem = (Item)Data[ varindx ][ i ];
                        Console.Write( "{0}, ", dataItem.Value1 );
                    }
                }
                Console.WriteLine( "" );
            }

            public class Item
            {
                public DateTime StartDate;
                public DateTime EndDate;

                public double Value1;
                public double Value2;

                public Item( DateTime start, DateTime end, double val ) {
                    this.StartDate = start;
                    this.EndDate = end;
                    this.Value1 = val;
                    this.Value2 = 0;
                }

                public Item( DateTime start, DateTime end, double val1, double val2 ) : this( start, end, val1 ){
                    this.Value2 = val2;
                }
            }
        }
    }
}
