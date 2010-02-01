using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Common.Dialogs;
using ExcelInterface;
using MarketSimUtilities;

namespace Results
    
{
    public class SummaryReportGenerator
    {
        private const string CalibrationMasterFile = "Summary Report - Calibration Master.xls";
        private const string FWReportFile = "Summary Report - F-W Report.xls";
        private const string TrialReportFile = "Summary Report - Trial Report.xls";
        private const string primaryDirectory = "\\Templates\\";
        private const string secondaryDirectory = "\\..\\..\\Templates\\";

        private const int MaxTemplateRows = 30;
        private const int MaxTemplateCols = 20;

        private const int numDecimalPlaces = 3;
        private const string dateFormat = "MMM d yyyy";

        private static string primaryDirPath = null;
        private static string secondaryDirPath = null;

        /// <summary>
        /// Writes the summary report file to the given path.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="reportType"></param>
        /// <param name="perProductData"></param>
        /// <param name="perBrandData"></param>
        /// <returns></returns>
        public static bool WriteSummaryReport( string filePath, ResultsForm.SummaryReportType reportType, ArrayList perProductData, ArrayList perBrandData,
            ProductTree productTree, string simName, string[] runStrs, string dateRange, int popSize, bool[] normalizeColumn, bool[] columnForProductOnly, 
            string brandTypeString, out string errorDetails ) {

            errorDetails = "";
            string templatePath = TemplatePath( reportType );
            if( File.Exists( templatePath ) == false ) {
                errorDetails = "Template file not found.";
                return false;
            }

            // make a copy of the template
            try {
                File.Copy( templatePath, filePath, true );
            }
            catch( Exception ) {
                errorDetails = "Unable to copy template file to output (Likely cause: The file is already open in Excel)";
                return false;
            }

            ExcelReaderWriter excelReader = new ExcelReaderWriter();
            excelReader.Open( filePath );
            string[] sheetNames = excelReader.GetSheetNames();
            if( sheetNames.Length == 0 ) {
                errorDetails = "Zero worksheets in output file.";
                return false;
            }
            excelReader.SetSheet( sheetNames[ 0 ] );

            // set up the required number of worksheets
            if( runStrs.Length > 1 ) {
                for( int r = 0; r < runStrs.Length - 1; r++ ) {
                    excelReader.CopyWorksheetToEnd();
                }
            }

            // reload the sheet names
            sheetNames = excelReader.GetSheetNames();

            for( int ws = 0; ws < runStrs.Length; ws++ ) {
                excelReader.SetSheet( sheetNames[ ws ] );
                // expand the template and copy in the arays of metric data
                ExpandTemplateRows( excelReader, perProductData, perBrandData, MaxTemplateRows, MaxTemplateCols, productTree, ws, runStrs.Length,
                    normalizeColumn, columnForProductOnly, brandTypeString );

                // replace the individual items in the template
                Hashtable replacementsList = GenerateValueReplacementsList( simName, runStrs[ ws ], dateRange, popSize );
                FillTemplate( excelReader, replacementsList, MaxTemplateRows, MaxTemplateCols );
            }

            excelReader.SaveAndClose();
            return true;
        }

        /// <summary>
        /// Expands the template to have the roght number pf brand and product rows; fills the newly created rows with values.
        /// </summary>
        /// <param name="excelReader"></param>
        /// <param name="perProductData"></param>
        /// <param name="perBrandData"></param>
        /// <param name="maxInRows"></param>
        /// <param name="maxInCols"></param>
        /// <param name="productTree"></param>
        private static void ExpandTemplateRows( ExcelReaderWriter excelReader, ArrayList perProductData, ArrayList perBrandData, int maxInRows, int maxInCols,
            ProductTree productTree, int run, int numRuns, bool[] normalizeColumn, bool[] columnForProductOnly, string brandTypeString ) {

            // determine how many brands/products we have been given to report on
            int nBrands = 0;
            int nProducts = 0;
            if( perProductData != null && perProductData.Count > 0 ) {
                ArrayList dataList = (ArrayList)perProductData[ 0 ];
                nProducts = dataList.Count;
            }
            if( perBrandData != null && perBrandData.Count > 0 ) {
                ArrayList dataList = (ArrayList)perBrandData[ 0 ];
                nBrands = dataList.Count;
            }

            // scan the existing worksheet data to find the brand and product rows
            object[ , ] rawData = (object[ , ])excelReader.GetValues( 1, 1, maxInRows, maxInCols );
            int prodRow = 0;
            double prodRowHeight = 0;
            int brandRow = 0;
            double brandRowHeight = 0;
            int maxCol = 0;
            ArrayList rowItems = new ArrayList();

            for( int row = 1; row <= maxInRows; row++ ) {
                if( rawData[ row, 1 ] is string ) {
                    string s =  rawData[ row, 1 ] as string;
                    if( s.StartsWith( "#ProductName" ) ) {
                        prodRow = row;
                        prodRowHeight = excelReader.GetRowHeight( prodRow );
                        // also count columns on this row
                        do {
                            maxCol += 1;
                            object rd = rawData[ row, maxCol ];
                            if( rd != null ) {
                                if( rd is string ) {
                                    rowItems.Add( rd );
                                }
                                else {
                                    rowItems.Add( "--FORMULA--" );
                                }
                            }
                        }
                        while( rawData[ row,maxCol ] != null );
                    }
                    if( s.StartsWith( "#BrandName" ) ) {
                        brandRow = row;
                        brandRowHeight = excelReader.GetRowHeight( brandRow );
                    }
                }
            }

            // figure out the column layout using the items on the product row
            int[] metricIndxForCol = new int[ rowItems.Count + 2 ];
            metricIndxForCol[ 1 ] = -1;        // the name column
            for( int m = 1; m < rowItems.Count; m++ ) {
                string s = (string)rowItems[ m ];
                if( s.StartsWith( "#" ) ) {
                    s = s.Substring( 1 );
                    int valcol = int.Parse( s );
                    metricIndxForCol[ m + 1 ] = ((valcol - 1) * numRuns) + 1 + run;
                }
                else {
                    metricIndxForCol[ m + 1 ] = -1;
                }
            }

            int blankRow = (int)Math.Max( prodRow, brandRow ) + 1;

            // get the list of all items to possibly generate, with products under their brands
            productTree.SelectByType( brandTypeString );
            ArrayList brandsAndProds = productTree.CheckedProducts;

            // ----------  generate a report containing both brands and products -----------
            if( nBrands > 0 && nProducts > 0 ) {
                int startRow = brandRow;
                int prodNum = 0;
                int brandNum = 0;

                // first copy the product rows into position (must be done before processing brands since the product template row may be overwritten with a brand row)
                for( int i = 0; i < brandsAndProds.Count; i++ ) {        
                    MrktSimDb.MrktSimDBSchema.productRow row = (MrktSimDb.MrktSimDBSchema.productRow)brandsAndProds[ i ];
                    MrktSimDb.MrktSimDBSchema.product_typeRow typerow = row.product_typeRow;
                    if( typerow.type_name != brandTypeString ) {      // this is a product (or SKU, variant)
                        int targetRow = startRow + i;
                        if( i > 1 ) {                                         // entry 0 should always be a brand; entry 1 is the existing first product row
                            excelReader.CopyCells( prodRow, 1, prodRow, maxCol + 1, targetRow, 1 );
                            excelReader.RowHeight( targetRow, prodRowHeight );
                        }

                        object[] nameObj = new object[ 1 ];
                        nameObj[ 0 ] = row.product_name;
                        excelReader.SetValues( targetRow, 1, targetRow, 1, nameObj );

                        object[] valObj = new object[ 1 ];
                        ArrayList vals = (ArrayList)perProductData[ prodNum ];
                        for( int c = 2; c < maxCol; c++ ) {

                            int valIndx = metricIndxForCol[ c ];
                            if( valIndx >= 0 && valIndx < vals.Count ) {
                                valObj[ 0 ] = vals[ valIndx ];
                                if( valObj[ 0 ] is double ) {
                                    if( normalizeColumn[ c ] ) {
                                        valObj[ 0 ] = ((double)valObj[ 0 ]) / 100.0;
                                    }
                                    excelReader.SetValues( targetRow, c, targetRow, c, valObj );
                                }
                            }
                        }
                        prodNum += 1;
                    }
                }

                // ...now copy the brands into their appropriate positions
                for( int i = 0; i < brandsAndProds.Count; i++ ) {        
                    MrktSimDb.MrktSimDBSchema.productRow row = (MrktSimDb.MrktSimDBSchema.productRow)brandsAndProds[ i ];
                    MrktSimDb.MrktSimDBSchema.product_typeRow typerow = row.product_typeRow;
                    if( typerow.type_name == brandTypeString ) {
                        int targetRow = startRow + i;
                        if( i > 0 ) {                                         // entry 0 should always be a brand
                            excelReader.CopyCells( brandRow, 1, brandRow, maxCol + 1, targetRow, 1 );
                            excelReader.RowHeight( targetRow, brandRowHeight );
                        }
                        object[] nameObj = new object[ 1 ];
                        nameObj[ 0 ] = row.product_name;
                        excelReader.SetValues( targetRow, 1, targetRow, 1, nameObj );

                        object[] valObj = new object[ 1 ];
                        ArrayList vals = (ArrayList)perBrandData[ brandNum ];
                        for( int c = 2; c < maxCol; c++ ) {

                            int valIndx = metricIndxForCol[ c ];
                            if( valIndx >= 0 && valIndx < vals.Count ) {
                                valObj[ 0 ] = vals[ valIndx ];
                                if( valObj[ 0 ] is double ) {
                                    if( normalizeColumn[ c ] ) {
                                        valObj[ 0 ] = ((double)valObj[ 0 ]) / 100.0;
                                    }
                                    if( columnForProductOnly[ c ] == false ) {
                                        excelReader.SetValues( targetRow, c, targetRow, c, valObj );
                                    }
                                    else {
                                        // leave the template value as-is
                                    }
                                }
                            }
                        }
                        brandNum += 1;
                    }
                }
            }
            // ----------  generate a report containing only brands -----------
            else if( nBrands > 0 ) {
                int brandNum = 0;
                excelReader.CopyCells( blankRow, 1, blankRow, maxCol + 1, prodRow, 1 );        // in case there is just 1 brand
                int targetRow = brandRow;
                for( int i = 0; i < brandsAndProds.Count; i++ ) {        // entry 0 should always be a brand
                    MrktSimDb.MrktSimDBSchema.productRow row = (MrktSimDb.MrktSimDBSchema.productRow)brandsAndProds[ i ];
                    MrktSimDb.MrktSimDBSchema.product_typeRow typerow = row.product_typeRow;
                    if( typerow.type_name == brandTypeString ) {
                        if( i > 0 ) {
                            excelReader.CopyCells( brandRow, 1, brandRow, maxCol + 1, targetRow, 1 );
                            excelReader.RowHeight( targetRow, brandRowHeight );
                        }
                        object[] nameObj = new object[ 1 ];
                        nameObj[ 0 ] = row.product_name;
                        excelReader.SetValues( targetRow, 1, targetRow, 1, nameObj );

                        object[] valObj = new object[ 1 ];
                        ArrayList vals = (ArrayList)perBrandData[ brandNum ];
                        for( int c = 2; c < maxCol; c++ ) {

                            int valIndx = metricIndxForCol[ c ];
                            if( valIndx >= 0 && valIndx < vals.Count ) {
                                valObj[ 0 ] = vals[ valIndx ];
                                if( valObj[ 0 ] is double ) {
                                    if( normalizeColumn[ c ] ) {
                                        valObj[ 0 ] = ((double)valObj[ 0 ]) / 100.0;
                                    }
                                    if( columnForProductOnly[ c ] == false ) {
                                        excelReader.SetValues( targetRow, c, targetRow, c, valObj );
                                    }
                                    else {
                                        // leave the template value as-is
                                    }
                                }
                            }
                        }

                        brandNum += 1;
                        targetRow += 1;
                    }
                }
            }
            // ----------  generate a report containing only products -----------
            else if( nProducts > 0 ) {
                int prodNum = 0;
                excelReader.CopyCells( prodRow, 1, prodRow, maxCol + 1, brandRow, 1 );        // brandRow now contains product proto row
                excelReader.CopyCells( blankRow, 1, blankRow, maxCol + 1, prodRow, 1 );        
                int targetRow = brandRow;
                for( int i = 0; i < brandsAndProds.Count; i++ ) {        // entry 0 should always be a brand
                    MrktSimDb.MrktSimDBSchema.productRow row = (MrktSimDb.MrktSimDBSchema.productRow)brandsAndProds[ i ];
                    MrktSimDb.MrktSimDBSchema.product_typeRow typerow = row.product_typeRow;
                    if( typerow.type_name != brandTypeString ) {
                        if( i > 0 ) {
                            excelReader.CopyCells( brandRow, 1, brandRow, maxCol + 1, targetRow, 1 );
                            excelReader.RowHeight( targetRow, prodRowHeight );
                        }
                        object[] nameObj = new object[ 1 ];
                        nameObj[ 0 ] = row.product_name;
                        excelReader.SetValues( targetRow, 1, targetRow, 1, nameObj );

                        object[] valObj = new object[ 1 ];
                        ArrayList vals = (ArrayList)perProductData[ prodNum ];
                        for( int c = 2; c < maxCol; c++ ) {

                            int valIndx = metricIndxForCol[ c ];
                            if( valIndx >= 0 && valIndx < vals.Count ) {
                                valObj[ 0 ] = vals[ valIndx ];
                                if( valObj[ 0 ] is double ) {
                                    if( normalizeColumn[ c ] ) {
                                        valObj[ 0 ] = ((double)valObj[ 0 ]) / 100.0;
                                    }
                                    excelReader.SetValues( targetRow, c, targetRow, c, valObj );
                                }
                            }
                        }
                        prodNum += 1;
                        targetRow += 1;
                    }
                }
            }
        }

        /// <summary>
        /// Sets up a table of replacements for the one-per-worksheet items.
        /// </summary>
        /// <param name="simName"></param>
        /// <param name="runStr"></param>
        /// <param name="dateRange"></param>
        /// <param name="popSize"></param>
        /// <returns></returns>
        private static Hashtable GenerateValueReplacementsList( string simName, string runStr, string dateRange, int popSize ) {
            Hashtable valList = new Hashtable();

            valList.Add( "#SimName", simName );
            valList.Add( "#RunNumber", runStr );
            valList.Add( "#DateRange", dateRange );
            valList.Add( "#NumAgents", popSize );

            return valList;
        }

        /// <summary>
        /// Initializes the full paths of the summary-report template files (call once during program initialization)
        /// </summary>
        public static void Init() {
            primaryDirPath = System.Environment.CurrentDirectory + primaryDirectory;
            secondaryDirPath = System.Environment.CurrentDirectory + secondaryDirectory;
        }

        /// <summary>
        /// Returns true if the given template is found
        /// </summary>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public static bool VerifyTemplateExists( ResultsForm.SummaryReportType reportType ) {
            string path = TemplatePath( reportType );
            return (path != null);
        }

        /// <summary>
        /// Locates the desired template, which may exist in one of two possible locations (the install location or the development/checkin location)
        /// </summary>
        /// <param name="reportType"></param>
        /// <returns></returns>
        private static string TemplatePath( ResultsForm.SummaryReportType reportType ) {
            string fileName = null;
            if( reportType == ResultsForm.SummaryReportType.CalibrationMaster ) {
                fileName = CalibrationMasterFile;
            }
            else if( reportType == ResultsForm.SummaryReportType.FWReport ) {
                fileName = FWReportFile;
            }
            else if( reportType == ResultsForm.SummaryReportType.TrialReport ) {
                fileName = TrialReportFile;
            }

            string testPath = primaryDirPath + fileName;
            if( File.Exists( testPath ) == true ) {
                return testPath;
            }

            string testPath2 = secondaryDirPath + fileName;
            if( File.Exists( testPath2 ) == true ) {
                return testPath2;
            }

            return null;
        }

        /// <summary>
        /// Replaces items in the worksheet currently open by the ExcelReaderWriter.
        /// </summary>
        /// <param name="excelReader"></param>
        /// <param name="replacementItemList"></param>
        /// <param name="maxRows"></param>
        /// <param name="maxCols"></param>
        /// <param name="errorDetails"></param>
        /// <returns></returns>
        private static bool FillTemplate( ExcelReaderWriter excelReader, Hashtable replacementItemList, int maxRows, int maxCols ) {

            object[ , ] sheetValues = (object[ , ])excelReader.GetValues( 1, 1, 1 + maxRows, 1 + maxCols );

            int nReplacements = 0;

            for( int row = 1; row <= maxRows; row++ ) {
                for( int col = 1; col <= maxCols; col++ ) {
                    object cellObj = sheetValues[ row, col ];

                    if( cellObj is string ) {
                        // all Excel values come in as strings
                        string cellStr = (string)cellObj;

                        // see if this is a value we have in our replacement set
                        object replacementObj = replacementItemList[ cellStr ];

                        if( replacementObj != null ) {
                            // replace the cell value
                            object[ , ] obj = new object[ 1, 1 ];

                            if( replacementObj is string ) {
                                string s = (string)replacementObj;
                                obj[ 0, 0 ] = s;
                            }
                            else if( replacementObj is int ) {
                                string s = String.Format( "{0}", (int)replacementObj );
                                obj[ 0, 0 ] = s;
                            }
                            else if( replacementObj is double ) {
                                string fmt = "{0:f" + numDecimalPlaces.ToString() + "}";
                                string s = String.Format( fmt, (double)replacementObj );
                                obj[ 0, 0 ] = s;
                            }
                            else if( replacementObj is DateTime ) {

                                DateTime dt = (DateTime)replacementObj;
                                string s = dt.ToString( dateFormat );
                                obj[ 0, 0 ] = s;
                            }

                            excelReader.SetValues( row, col, row, col, obj );
                            nReplacements += 1;
                        }
                    }
                }
            }

            return true;
        }
    }
}
