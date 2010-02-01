using System;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Windows.Forms;

using DataImporter.Library;
using DataImporter.ImportSettings;
using DataImporter.Dialogs;

namespace DataImporter
{
    class WorksheetScanner
    {
        private string sheetName;

        private object[ , ] sheetData;
        private Rectangle dataCoords;

        private static bool acceptAnyBlankCell = false;


        public WorksheetScanner( string sheetName ) {
            this.sheetName = sheetName;
        }

        // gets the sheet data according to the dataCoords 
        private void GetSheetData() {
            sheetData = (object[ , ])ScanManager.Reader.GetValues( dataCoords.Top, dataCoords.Left, dataCoords.Bottom, dataCoords.Right );
        }

        /// <summary>
        /// Scans the worksheet curently open by the ScanManager.Reader for data format.
        /// </summary>
        /// <returns></returns>
        public WorksheetSettings Scan( bool scanForChannel ) {
            dataCoords = new Rectangle( 1, 1, 20, 20 );
            GetSheetData();

            WorksheetSettings settings = new WorksheetSettings( sheetName );
            settings.Validated = false;

            // parse the data from the worksheet to determine the format
            DateTime firstDate;
            XPoint p;
            settings.FirstDateHeader = FindFirstDateHeader( out  p, out firstDate );

            if( settings.FirstDateHeader == null ) {
                return settings;
            }

            settings.FirstDateHeaderCell = p;

            // see if we can determine if dates are start or end of measurement intervals
            settings.DatesAreIntervalEnd = true;
            string h = settings.FirstDateHeader.ToLower();
            if( h.IndexOf( "start" ) != -1 || h.IndexOf( "begin" ) != -1 || h.IndexOf( "commence" ) != -1 || h.IndexOf( "from" ) != -1 ) {
                settings.DatesAreIntervalEnd = false;
            }


            // determine if the time advances horizontally or vertically
            bool horizFormat;
            TimeSpan timeStep;
            bool foundNextDate = FindSecondDateHeader( settings.FirstDateHeaderCell, firstDate, out timeStep, out horizFormat );

            Console.WriteLine( "FindSecondDateHeader() ... foundNextDate = {0}", foundNextDate );
            Console.WriteLine( "  horizFormat = {0}", horizFormat );
            Console.WriteLine( "  timeStep = {0:f2} days", timeStep.TotalDays );

            if( foundNextDate == false ) {
                return settings;
            }

            settings.TimeStepDays = timeStep.Days;
            settings.Horizontal = horizFormat;

            // locate the first SKU/group header
            XPoint p2;
            settings.FirstSkuHeader = FindSkuHeader( settings.FirstDateHeaderCell, horizFormat, out p2 );

            if( settings.FirstSkuHeader == null ) {
                return settings;
            }

            settings.FirstSkuHeaderCell = p2;

            // locate the first channel header
            if( scanForChannel ) {
                XPoint p3;
                settings.FirstChanHeader = FindChanHeader( settings.FirstSkuHeaderCell, horizFormat, out p3 );

                if( settings.FirstChanHeader == null ) {
                    settings.Validated = true;      // channels are optional
                    return settings;
                }

                settings.FirstChanHeaderCell = p3;
            }

            // the file looks seems to have reasonable headers
            settings.Validated = true;

            return settings;
        }

        // scan the data to find the first reasonable header for a date-interval column or row
        private string FindFirstDateHeader( out XPoint loc, out DateTime date ) {
            loc = new XPoint( -1, -1 );
            date = DateTime.MinValue;
            for( int row = 1; row < sheetData.GetLength( 0 ); row++ ) {
                for( int col = 1; col < sheetData.GetLength( 1 ); col++ ) {
                    object cellObj = sheetData[ row, col ];
                    if( cellObj is string ) {
                        Console.WriteLine( "Cell[ row={0}, col={1} ] = {2}", row, col, (string)cellObj );
                    }
                    if( ContainsDate( cellObj, out date ) == true ) {
                        Console.WriteLine( "Contains date! " + date.ToShortDateString() );
                        loc = new XPoint( row, col );
                        if( cellObj is string ) {
                            return (string)cellObj;
                        }
                        else {
                            return date.ToShortDateString();
                        }
                    }
                }
            }
            // no date header cell found
            return null;
        }

        private bool FindSecondDateHeader( XPoint firstDatePoint, DateTime firstDate, out TimeSpan timeStep, out bool horizontalFormat ) {
            timeStep = new TimeSpan();
            horizontalFormat = true;

            object nextHCell = sheetData[ firstDatePoint.Row, firstDatePoint.Col + 1 ];     // we read an extra row/col so this is always OK
            object nextVCell = sheetData[ firstDatePoint.Row + 1, firstDatePoint.Col ];

            DateTime nextDate;

            if( ContainsDate( nextHCell, out nextDate ) == true ) {
                horizontalFormat = true;
                timeStep = nextDate - firstDate;
                return true;
            }
            else if( ContainsDate( nextVCell, out nextDate ) == true ) {
                horizontalFormat = false;
                timeStep = nextDate - firstDate;
                return true;
            }
            else {
                return false;
            }
        }

        // scan the data to find the first reasonable header for a data (SKU or group) column or row
        private string FindSkuHeader( XPoint firstDateHeaderLoc, bool horizontalFormat, out XPoint loc ) {
            XPoint adjLoc = null;
            // find the expected posiiton of the first header and the first data item for that header
            if( horizontalFormat ) {
                loc = new XPoint( firstDateHeaderLoc.Row + 1, firstDateHeaderLoc.Col - 1 );
                adjLoc = new XPoint( firstDateHeaderLoc.Row + 1, firstDateHeaderLoc.Col );
            }
            else {
                loc = new XPoint( firstDateHeaderLoc.Row - 1, firstDateHeaderLoc.Col + 1 );
                adjLoc = new XPoint( firstDateHeaderLoc.Row, firstDateHeaderLoc.Col + 1 );
            }

            // we'll look a few cells beyond the expected position
            int tryStep = 5;
            do {
                object cellObj = sheetData[ loc.Row, loc.Col ];
                object adjCellObj = sheetData[ adjLoc.Row, adjLoc.Col ];

                string cellStr = null;
                if( cellObj is string ) {
                    cellStr = (string)cellObj;
                    cellStr = cellStr.Trim();
                }

                // if the header is a non-blank string and the first data item is also non-null, we found a good header
                if( cellStr != null && cellStr.Length > 0 && adjCellObj != null ) {
                    // done
                    return cellStr;
                }

                // if we didn't hit a header on the previous try, get ready to check the next cell
                if( horizontalFormat ) {
                    loc = new XPoint( loc.Row + 1, loc.Col );
                    adjLoc = new XPoint( adjLoc.Row + 1, adjLoc.Col );
                }
                else {
                    loc = new XPoint( loc.Row, loc.Col + 1 );
                    adjLoc = new XPoint( adjLoc.Row, adjLoc.Col + 1 );
                }
            } while( --tryStep > 0 );

            // no header cell found
            return null;
        }

        // scan the data to find the first reasonable cell for a channel (row set) identirier
        private string FindChanHeader( XPoint firstSkuHeaderLoc, bool horizontalFormat, out XPoint loc ) {
            loc = null;
            // find the expected posiiton of the first channel ID 
            if( firstSkuHeaderLoc.Col < 2 || firstSkuHeaderLoc.Row < 2 ) {
                return null;
            }

            XPoint chanLoc = new XPoint( firstSkuHeaderLoc.Row - 1, firstSkuHeaderLoc.Col - 1 );
            object cellObj = sheetData[ chanLoc.Row, chanLoc.Col ];

            string cellStr = null;
            if( cellObj is string ) {
                cellStr = (string)cellObj;
                cellStr = cellStr.Trim();
            }

            // if the cell is a non-blank string, we found a good channel ID
            if( cellStr != null && cellStr.Length > 0 ) {
                // done
                loc = chanLoc;
                return cellStr;
            }

            //??? not sure if/how to search further if channel ID is not in the expected location

            // no header cell found
            return null;
        }

        public bool ScanAllWorksheetDates( WorksheetSettings worksheetSettings, bool verifyOnly ) {
            Console.WriteLine( "\nScanAllWorksheetDates --> {0}, {1}", worksheetSettings.SheetName, verifyOnly );

            XPoint cell = worksheetSettings.FirstDateHeaderCell;
            DateTime cellDate;
            if( ContainsDate( cell, out cellDate ) == false ) {
                return false;
            }
            DateTime startlDate = cellDate;
            ArrayList dates = new ArrayList();
            dates.Add( startlDate );

            XPoint nextCell = new XPoint( cell.Row, cell.Col );
            int timeStepCount = 1;
            if( worksheetSettings.ScanAllDateHeaders == true ) {
                worksheetSettings.AllDateHeaders = new ArrayList();
                worksheetSettings.AllDateHeaders.Add( startlDate );
            }
            DateTime endDate = startlDate;
            do {
                if( worksheetSettings.Horizontal ) {
                    nextCell = new XPoint( cell.Row, cell.Col + 1 );
                }
                else {
                    nextCell = new XPoint( cell.Row + 1, cell.Col );
                }

                if( ContainsDate( nextCell, out cellDate ) == false ) {
                    // hit the end of the dates 
                    break;
                }
                if( worksheetSettings.ScanAllDateHeaders == true ) {
                    worksheetSettings.AllDateHeaders.Add( cellDate );
                }

                endDate = cellDate;
                timeStepCount += 1;
                dates.Add( cellDate );
                cell = nextCell;

            } while( true );

            if( worksheetSettings.ScanAllDateHeaders == false ) {
                // verify that all time steps are equal
                for( int i = 1; i < timeStepCount; i++ ) {
                    DateTime d1 = (DateTime)dates[ i - 1 ];
                    DateTime d2 = (DateTime)dates[ i ];
                    TimeSpan ts = d2 - d1;
                    int nDays = (int)ts.TotalDays;
                    int secDays = worksheetSettings.TimeStepDays;

                    if( nDays != secDays ) {
                        if( (nDays < 28) || (nDays > 31) || (nDays < 28) || (nDays > 31) ) {     // 1-month intervals aren't all equal
                            string msg = String.Format( "    Error validating sheet dates in worksheet \"{0}\"    \r\n\r\n" +
                                "Interval {1} to {2} is not equal to the section time step ({3} days)   \r\n\r\n    File: {4}   \r\n",
                                worksheetSettings.SheetName, d2, d1, worksheetSettings.TimeStepDays, worksheetSettings.InputFile );
                            MessageBox.Show( msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                            return false;
                        }
                    }
                }

                // see if we are only verifying a worksheet (because the section's first worksheet has already has been scanned for dates)
                if( verifyOnly == true ) {
                    bool ok = (startlDate == worksheetSettings.GetSection().StartDate) &&
                        (endDate == worksheetSettings.GetSection().EndDate) &&
                        (timeStepCount == worksheetSettings.GetSection().TimeStepCount);

                    worksheetSettings.GetSection().WorksheetDatesValid = ok;
                    Console.WriteLine( "DONE (validate)! All Worksheet Dates OK = {0}", ok );
                    Console.WriteLine( " ... start date = {0}", startlDate.ToShortDateString() );
                    return ok;
                }
            }
            else {
                // this is a scan-all-dates worksheet
                if( verifyOnly == true ) {
                    worksheetSettings.GetSection().WorksheetDatesValid = true;
                    Console.WriteLine( "DONE (validate)! Scanned ALL worksheet dates ( {0} )", worksheetSettings.AllDateHeaders.Count );
                }
            }

            // if we get here we aren't just verifying = set the section info now
            worksheetSettings.GetSection().StartDate = startlDate;
            worksheetSettings.GetSection().EndDate = endDate;
            worksheetSettings.GetSection().TimeStep = new TimeSpan( worksheetSettings.TimeStepDays, 0, 0, 0 );
            if( worksheetSettings.TimeStepDays <= 28 && worksheetSettings.TimeStepDays >= 31 ) {
                worksheetSettings.GetSection().TimeStep = new TimeSpan( 30, 0, 0, 0 );
            }
            worksheetSettings.GetSection().TimeStepCount = timeStepCount;
            worksheetSettings.GetSection().WorksheetDatesSet = true;
            worksheetSettings.GetSection().WorksheetDatesValid = true;

            if( worksheetSettings.ScanAllDateHeaders == false ) {
                Console.WriteLine( "DONE! All Worksheet Dates scanned: start date = {0},    time step count = {1}", startlDate.ToShortDateString(), timeStepCount );
            }
            else {
                Console.WriteLine( "DONE! All Worksheet Dates scanned and saved: start date = {0}, {1} date headers", startlDate.ToShortDateString(), timeStepCount );
            }

            return true;
        }

        public bool ScanAllWorksheetVariants( WorksheetSettings worksheetSettings, bool verifyOnly, ProjectSettings projectSettings ) {
            Console.WriteLine( "\nScanAllWorksheetVariants --> {0}, {1}", worksheetSettings.SheetName, verifyOnly );

            XPoint cell = worksheetSettings.FirstSkuHeaderCell;
            Console.WriteLine( "...Checking row, col = {0}, {1}", cell.Row, cell.Col );
            string cellValue;
            if( CellIsNonBlankString( cell, out cellValue ) == false ) {
                return false;
            }
            ArrayList variants = new ArrayList();
            variants.Add( cellValue );

            XPoint nextCell = new XPoint( cell.Row, cell.Col );
            do {
                if( worksheetSettings.Horizontal ) {
                    nextCell = new XPoint( cell.Row + 1, cell.Col );
                }
                else {
                    nextCell = new XPoint( cell.Row, cell.Col + 1 );
                }

                if( CellIsNonBlankString( nextCell, out cellValue ) == false ) {
                    // hit the end of the varants 
                    break;
                }

                variants.Add( cellValue );
                cell = nextCell;

            } while( true );

            // see if we are only verifying a worksheet (because the section's first worksheet has already has been scanned for varants)
            if( verifyOnly == true ) {
                bool ok = true;
                if( variants.Count != worksheetSettings.GetSection().Variants.Count ) {
                    return false;
                }

               
                for( int i = 0; i < variants.Count; i++ ) {
                    if( (string)variants[ i ] != (string)worksheetSettings.GetSection().Variants[ i ] ) {
                        // found a mismatch -- recheck to be sure that these 
                        bool recheck = projectSettings.IsProductMismatchOkay( (string)variants[ i ], (string)worksheetSettings.GetSection().Variants[ i ] );
                        if( recheck == false ) {

                            Console.WriteLine( "Mismatched variants: {0}, {1}", (string)variants[ i ], (string)worksheetSettings.GetSection().Variants[ i ] );

                            ok = false;
                        }
                    }
                }
                worksheetSettings.GetSection().WorksheetVariantsValid = ok;
                return ok;
            }

            // if we get here we aren't just verifying = set the section info now
            worksheetSettings.GetSection().Variants = variants;
            worksheetSettings.GetSection().WorksheetVariantsSet = true;
            worksheetSettings.GetSection().WorksheetVariantsValid = true;

            Console.WriteLine( "DONE! All Worksheet Variants: count = {0}", variants.Count );

            return true;
        }

        public bool ScanAllWorksheetChannels( WorksheetSettings worksheetSettings, bool verifyOnly ) {
            Console.WriteLine( "\nScanAllWorksheetChannels --> {0}, {1}", worksheetSettings.SheetName, verifyOnly );
            int variantCount = worksheetSettings.VariantCount;

            XPoint cell = worksheetSettings.FirstChanHeaderCell;
            Console.WriteLine( "...Checking row, col = {0}, {1}", cell.Row, cell.Col );
            string cellValue;
            if( CellIsNonBlankString( cell, out cellValue ) == false ) {
                return false;
            }
            ArrayList channels = new ArrayList();
            channels.Add( cellValue );

            XPoint nextCell = new XPoint( cell.Row, cell.Col );
            do {
                if( worksheetSettings.Horizontal ) {
                    nextCell = new XPoint( cell.Row + variantCount + 1, cell.Col );
                }
                else {
                    nextCell = new XPoint( cell.Row, cell.Col + variantCount + 1 );
                }

                if( CellIsNonBlankString( nextCell, out cellValue ) == false ) {
                    // hit the end of the channels 
                    break;
                }

                channels.Add( cellValue );
                cell = nextCell;

            } while( true );

            // see if we are only verifying a worksheet (because the section's first worksheet has already has been scanned for varants)
            if( verifyOnly == true ) {
                bool ok = true;
                for( int i = 0; i < channels.Count; i++ ) {
                    if( (string)channels[ i ] != (string)worksheetSettings.GetSection().Channels[ i ] ){
                        // found a mismatch
                        ok = false;
                    }
                }
                worksheetSettings.GetSection().WorksheetChannelsValid = ok;
                return ok;
            }

            // if we get here we aren't just verifying = set the section info now
            worksheetSettings.GetSection().Channels = channels;
            worksheetSettings.GetSection().WorksheetChannelsSet = true;
            worksheetSettings.GetSection().WorksheetChannelsValid = true;

            Console.WriteLine( "All Worksheet Channels: count = {0}", channels.Count );

            return true;
        }

        public int GetVariantCount( WorksheetSettings worksheetSettings ) {
            int variantCount = worksheetSettings.GetSection().Variants.Count;
            if( (worksheetSettings.GetSection().BrandSource == ProjectSettings.InfoSource.DirectoryName) ||
                (worksheetSettings.GetSection().BrandSource == ProjectSettings.InfoSource.FileName) ) {
                // each file may have its own number of variants in these cases!
                variantCount = ScanToEndOfVariants( worksheetSettings );
                Console.WriteLine( "  --> File {0} has {1} SKUs...",
                    worksheetSettings.InputFile.Substring( worksheetSettings.InputFile.LastIndexOf( "\\" ) + 1 ), variantCount );
            }
            return variantCount;
        }

        public int GetChannelCount( WorksheetSettings worksheetSettings ) {
            int channelCount = worksheetSettings.GetSection().Channels.Count;
            return channelCount;
        }

        public bool ReadWorksheetData( WorksheetSettings worksheetSettings, int variantCount, int channelCount, double compressionTolerance ) {
            XPoint dataStartCell = new XPoint( worksheetSettings.FirstSkuHeaderCell.Row, worksheetSettings.FirstSkuHeaderCell.Col );
            int dataRows = -1;
            int dataCols = -1;

            Console.WriteLine( "\nReadWorksheetData( ws = {0} )", worksheetSettings.SheetName );

            if( worksheetSettings.Horizontal ) {
                dataStartCell = new XPoint( dataStartCell.Row, worksheetSettings.FirstDateHeaderCell.Col );
                dataRows = variantCount;
                if( channelCount > 1 ) {
                    dataRows += (variantCount + 1) * (channelCount - 1);        // allow for blank rows between channel blocks
                }
                dataCols = worksheetSettings.GetSection().TimeStepCount;
            }
            else {
                // vertical format
                dataStartCell = new XPoint( worksheetSettings.FirstDateHeaderCell.Row, dataStartCell.Col );
                dataRows = worksheetSettings.GetSection().TimeStepCount;
                dataCols = variantCount;
                if( channelCount > 1 ) {
                    dataCols += (variantCount + 1) * (channelCount - 1);      // allow for blank cols between channel blocks
                }
            }
            int endRow = dataStartCell.Row + dataRows;
            int endCol = dataStartCell.Col + dataCols;

            // initialize the data array
            worksheetSettings.InitializeData( channelCount, variantCount );
            Console.WriteLine( "Initialized data for {0} rows", variantCount );

            // read the data from the Excel worksheet now
            this.dataCoords = new Rectangle( dataStartCell.Col, dataStartCell.Row, dataCols, dataRows );
            GetSheetData();
            double cellValue = -1;
            int channelIndex = 0;

            //MAIN LOOP  convert the Excel data to DataItem objects
            for( int cellRow = dataStartCell.Row; cellRow < endRow; cellRow++ ) {

                if( worksheetSettings.Horizontal == false ) {
                    channelIndex = 0;
                }
                for( int cellCol = dataStartCell.Col; cellCol < endCol; cellCol++ ) {

                    // get a cell object
                    int valRow = cellRow - dataStartCell.Row + 1;
                    int valCol = cellCol - dataStartCell.Col + 1;
                    object valObj = sheetData[ valRow, valCol ];

                    int dateStepNum = -1;
                    int varIndex = -1;
                    if( worksheetSettings.Horizontal ) {
                        dateStepNum = valCol;
                        varIndex = valRow - 1;
                    }
                    else {
                        dateStepNum = valRow;
                        varIndex = valCol - 1;
                    }
                    varIndex = varIndex % (variantCount + 1);

                    bool expectBlankCell = false;
                    if( varIndex == variantCount ) {
                        // this cell is between channels and should be blank
                        expectBlankCell = true;
                        if( worksheetSettings.Horizontal ) {
                            if( valCol == 1 ) {
                                channelIndex += 1;
                            }
                        }
                        else { // vertical format
                            channelIndex += 1;
                        }
                    }

                    //!!!DEBUG
                    if( valCol == 1 ) {
                        //Console.WriteLine( "channelIndex = {0}, varIndex = {1} expectBlank = {2}", channelIndex, varIndex, expectBlankCell );
                    }

                    if( (valObj == null) || (valObj is DBNull) ) {
                        // a blank cell -- make it 0

                        if( expectBlankCell ) {
                            continue;
                        }
                        cellValue = 0;
                        if( acceptAnyBlankCell == false ) {
                            string msg = String.Format( "\r\n    Warning: Blank cell found in ReadData( row,col= {0},{1} )    \r\n    Sheet: {2}    \r\n    OK to replace this cell with 0 ?",
                                cellRow, cellCol, worksheetSettings.SheetName );
                            YesNoAllForm ynForm = new YesNoAllForm( msg, "Warning" );
                            DialogResult resp = ynForm.ShowDialog();
                            if( resp == DialogResult.No ) {
                                return false;
                            }
                            else if( resp == DialogResult.Ignore ) {     // yes to all
                                acceptAnyBlankCell = true;
                            }
                        }
                    }
                    else {
                        if( valObj is double ) {
                            cellValue = (double)valObj;
                        }
                        else if( valObj is string ) {
                            string tmp = (string)valObj;
                            tmp = tmp.Trim();

                            if( tmp == "-" ) {
                                // a dash in a cell is the standard NITRO way of indicating "nearly zero".
                                cellValue = 0;
                            }
                            else if( tmp.Length == 0 ) {
                                // a cell containing only blanks!
                                cellValue = 0;
                                string msg = String.Format( "\r\n    Warning: Blank cell found in ReadData( row,col= {0},{1} )  Sheet: {2}    \r\n\r\n    \r\nContinue?",
                                    cellRow, cellCol, worksheetSettings.SheetName );
                                DialogResult resp = MessageBox.Show( msg, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation );
                                if( resp != DialogResult.OK ) {
                                    return false;
                                }
                            }
                        }
                    }

                    // create the DataItem object and store it in the Data array
                    // determine the date for this point
                    DateTime d0 = worksheetSettings.GetSection().StartDate;
                    if( worksheetSettings.ScanAllDateHeaders == false ) {
                        if( worksheetSettings.DatesAreIntervalEnd == true ) {
                            d0 -= worksheetSettings.GetSection().TimeStep;
                        }
                        else {
                            d0 -= new TimeSpan( 1, 0, 0, 0 );
                        }
                        for( int t = 0; t < dateStepNum - 1; t++ ) {
                            d0 += worksheetSettings.GetSection().TimeStep;
                        }
                    }
                    else {
                        // each date header has been scanned
                        d0 = (DateTime)worksheetSettings.AllDateHeaders[ dateStepNum - 1 ];
                    }

                    DateTime d1 = d0;
                    if( worksheetSettings.GetSection().TimeStep.Days > 0 ) {
                        d1 = d0 + worksheetSettings.GetSection().TimeStep;
                    }
                    else {
                        // negative days are an encoding for months
                        int monthStep = -worksheetSettings.GetSection().TimeStep.Days;
                        int d1year = d0.Year;
                        int d1month = d0.Month + 1;
                        if( d1month == 13 ) {
                            d1month = 1;
                            d1year += 1;
                        }
                        int d1day = d0.Day;
                        // just in case... !!! we aren't really handling the case well where the dates are the ends of specified months
                        if( d1day > 28 ) {
                            // we will get LOTS of these messages!
                            Console.WriteLine( "WARNING: End-of-month dates not accurately handled (truncating all months to end on he 28th) -- need code update!" );
                            d1day = 28;
                        }
                        d1 = new DateTime( d1year, d1month, d1day );
                        d1 = d1.AddDays( -1 );
                    }

                    if( worksheetSettings.DatesAreIntervalEnd == true ) {
                        d0 += new TimeSpan( 1, 0, 0, 0 );
                    }

                    // now we know the cell value and dates -- add it to the appropriate master list
                    DataItem dataItem = new DataItem( d0, d1, cellValue );
                    worksheetSettings.GetData( channelIndex, varIndex ).Add( dataItem );

                    //if( varIndex == 0 ) {
                    //    Console.WriteLine( "Added data item: start, end = {0},  {1}", dataItem.StartDate.ToShortDateString(), dataItem.EndDate.ToShortDateString() );
                    //}
                }
            }

            Console.WriteLine( "read data -- variantCount = {0}", variantCount );

            if( worksheetSettings.DataType == ProjectSettings.DataType.Distribution || 
                worksheetSettings.DataType == ProjectSettings.DataType.RealSales ||
               worksheetSettings.DataType == ProjectSettings.DataType.Price ) {

                if( compressionTolerance != 0 ) {
                    CompressData( worksheetSettings, compressionTolerance, variantCount );
                }
            }
            return true;
        }

        // counts the number of variants
        private int ScanToEndOfVariants( WorksheetSettings worksheetSettings ) {
            int numVariants = 0;
            worksheetSettings.Variants = new ArrayList();

            string dum = null;
            XPoint cell = new XPoint( worksheetSettings.FirstSkuHeaderCell.Row, worksheetSettings.FirstSkuHeaderCell.Col );
            while( CellIsNonBlankString( cell, out dum ) ) {
                worksheetSettings.Variants.Add( dum );
                numVariants++;
                if( worksheetSettings.Horizontal ) {
                    cell = new XPoint( cell.Row + 1, cell.Col );
                }
                else {
                    cell = new XPoint( cell.Row, cell.Col + 1 );
                }
            }

            return numVariants;
        }

        private bool CellIsNonBlankString( XPoint cell, out string cellValue ) {
            object cellObj = ScanManager.Reader.GetValue( cell.Row, cell.Col );
            cellValue = null;
            bool isSet = false;
            if( cellObj != null && cellObj is string ) {
                cellValue = (string)cellObj;
                cellValue = cellValue.Trim();
                if( cellValue.Length > 0 ) {
                    isSet = true;
                }
            }
            return isSet;
        }

        private string TimeSpanString( TimeSpan span ) {
            int nDays = (int)span.TotalDays;
            string timeStepString = null;
            if( nDays % 365 == 0 ) {
                if( nDays == 365 ) {
                    timeStepString = "1 year";
                }
                else {
                    timeStepString = String.Format( "{0} years", nDays / 365 );
                }
            }
            else if( nDays % 7 == 0 ) {
                if( nDays == 7 ) {
                    timeStepString = "1 week";
                }
                else {
                    timeStepString = String.Format( "{0} weeks", nDays / 7 );
                }
            }
            else {
                if( nDays == 1 ) {
                    timeStepString = "1 day";
                }
                else {
                    timeStepString = String.Format( "{0} days", nDays );
                }
            }
            return timeStepString;
        }

        //reads the cell and determines if it is a date
        private bool ContainsDate( XPoint cell, out DateTime date ) {
            object cellObj = ScanManager.Reader.GetValue( cell.Row, cell.Col );
            return ContainsDate( cellObj, out date );
        }

        public static bool ContainsDate( object cellObj, out DateTime date ) {
            date = DateTime.MinValue;
            if( cellObj is double ) {
                // Excel encodes dates internally as numbers.  1/1/2000 = 36526  (36527 is Jan 2 2000)
                double numval = (double)cellObj;
                double minDate = 36526;
                double maxDate = minDate + (365 * 15);     // valid dates are 2000-2015
                if( numval >= minDate && numval <= maxDate ) {
                    // convert back to DateTime
                    date = new DateTime( 2000, 1, 1 );
                    date = date.AddDays( numval - minDate );
                    return true;
                }
                // double is not in range to be a date
                return false;
            }
            else if( cellObj is string ) {

                //Console.WriteLine( "Check for Date: " + (string)cellObj );

                string cellVal = (string)cellObj;
                cellVal = cellVal.Trim();

                string sfxVal = cellVal;
                if( cellVal.IndexOf( " " ) != -1 ) {
                    sfxVal = cellVal.Substring( cellVal.LastIndexOf( " " ) + 1 );
                }
                if( sfxVal.StartsWith( "'" ) && sfxVal.Length > 1 ) {
                    sfxVal = sfxVal.Substring( 1 );
                }

                try {
                    date = DateTime.Parse( sfxVal );
                    //Console.WriteLine( "Found Date: " + date.ToShortDateString() );
                    return true;
                }
                catch( Exception ) {
                    // not a string ending in a date

                    // perhaps it is a numeric date format?
                    try {
                        int intDate = int.Parse( sfxVal );
                        int nowYear = DateTime.Now.Year;
                        int maxReasonableSimYear = nowYear + 10;

                        if( intDate >= 19950000 && intDate < maxReasonableSimYear * 10000 ) {
                            int dateYr = intDate / 10000;
                            intDate -= dateYr * 10000;
                            int dateMo = intDate / 100;
                            int dateDay = intDate % 100;
                            date = new DateTime( dateYr, dateMo, dateDay );
                            //Console.WriteLine( "Found Date: " + date.ToShortDateString() );
                            return true;
                        }
                    }
                    catch( Exception ) {
                        // perhaps the line looks like "JAN 2005 DOLS (000)"
                        if( cellVal.IndexOf( " " ) != -1 ) {
                            string s1 = cellVal.Substring( 0, cellVal.IndexOf( " " ) ).ToUpper();
                            string s2 = cellVal.Substring( cellVal.IndexOf( " " ) + 1 );
                            if( s2.IndexOf( " " ) != -1 ) {
                                s2 = s2.Substring( 0, s2.IndexOf( " " ) );
                                if( s2.Length == 4 && (s2.StartsWith( "200" ) || s2.StartsWith( "201" ) ) && Char.IsDigit( s2[ 3 ] ) ) {
                                    // the second word is  a valid year, so checl the first word
                                    if( s1 == "JAN" || s1 == "FEB" || s1 == "MAR" || s1 == "APR" || s1 == "MAY" || s1 == "JUN" ||
                                        s1 == "JUL" || s1 == "AUG" || s1 == "SEP" || s1 == "OCT" || s1 == "NOV" || s1 == "DEC" ) {

                                        date = DateTime.Parse( s1 + " 1, " + s2 );
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private DataItem[] onDeckData;
        private ArrayList[] compressedData;

        private void CompressData( WorksheetSettings worksheetSettings, double compressionTolerance, int nVariants ) {
            Console.WriteLine( "\nCompress Data( {0} ) -->>> Commencing...", compressionTolerance );
            double compressionRatio = 0;

            onDeckData = new DataItem[ nVariants ];
            compressedData = new ArrayList[ nVariants ];

            for( int v = 0; v < nVariants; v++ ) {
                compressedData[ v ] = new ArrayList();
            }

            int initialCount = worksheetSettings.DataCount;
            int compressedIndex = 0;

            onDeckData = new DataItem[ nVariants ];
            int skipCount = 0;

            SetOnDeckData( 0, nVariants, worksheetSettings );

            for( int row = 1; row < initialCount; row++ ) {

                bool inTolerance = InBallPark( row, compressionTolerance, nVariants, worksheetSettings );

                if( inTolerance ) {
                    skipCount += 1;
                    AverageWithtOnDeckData( row, skipCount, nVariants, worksheetSettings );
                }
                else {
                    WriteOutOnDeckData( nVariants );
                    SetOnDeckData( row, nVariants, worksheetSettings );
                    skipCount = 0;
                    compressedIndex += 1;
                }
            }

            WriteOutOnDeckData( nVariants );
            compressedIndex += 1;

            compressionRatio = (initialCount - compressedIndex) / (double)initialCount;
            Console.WriteLine( "\n...Done. Compressed by {0}%.", compressionRatio * 100 );

            for( int v = 0; v < nVariants; v++ ) {
                worksheetSettings.SetData( v, compressedData[ v ] );
            }
        }

        private void SetOnDeckData( int sourceTimeStep, int nVariants, WorksheetSettings worksheetSettings ) {
            for( int v = 0; v < nVariants; v++ ) {
                onDeckData[ v ] = new DataItem( worksheetSettings.GetDataItem( v, sourceTimeStep ) );
            }
       }

        private bool InBallPark( int sourceTimeStep, double compressionTolerance, int nVariants, WorksheetSettings worksheetSettings ) {
            for( int v = 0; v < nVariants; v++ ) {
                DataItem testData = new DataItem( worksheetSettings.GetDataItem( v, sourceTimeStep ) );

                double delta1 = Math.Abs( onDeckData[ v ].Value1 - testData.Value1 );
                if( onDeckData[ v ].Value1 > 10 ) {
                    delta1 /= onDeckData[ v ].Value1;
                }

                double delta2 = Math.Abs( onDeckData[ v ].Value2 - testData.Value2 );
                if( onDeckData[ v ].Value2 > 10 ) {
                    delta2 /= onDeckData[ v ].Value2;
                }

                if( delta1 > compressionTolerance || delta2 > compressionTolerance ) {
                    return false;
                }
            }
            return true;
        }

        private void AverageWithtOnDeckData( int sourceTimeStep, int skipCount, int nVariants, WorksheetSettings worksheetSettings ) {
            double nPoints = skipCount + 1;
            double r1 = (nPoints - 1) / nPoints;
            double r2 = 1 / nPoints;
            for( int v = 0; v < nVariants; v++ ) {
                onDeckData[ v ].Value1 = (onDeckData[ v ].Value1 * r1) + (worksheetSettings.GetDataItem( v, sourceTimeStep ).Value1 * r2);
                onDeckData[ v ].Value2 = (onDeckData[ v ].Value2 * r1) + (worksheetSettings.GetDataItem( v, sourceTimeStep ).Value2 * r2);
                onDeckData[ v ].EndDate = worksheetSettings.GetDataItem( v, sourceTimeStep ).EndDate;
            }
        }

        private void WriteOutOnDeckData( int nVariants ) {
            for( int v = 0; v < nVariants; v++ ) {
                compressedData[ v ].Add( onDeckData[ v ] );
            }
        }
    }
}
