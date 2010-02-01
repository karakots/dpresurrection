using System;
using System.Collections;
using System.Text;
using System.IO;
using Common.Dialogs;
using System.Windows.Forms;

using ExcelInterface;

namespace Results
{
    class ExcelTemplateFiller
    {
        private static int numDecimalPlaces = 3;

        private static string dateFormat = "MMM d yyyy";

        /// <summary>
        /// Replaces cells in the outputFilePath that contain keys in the replacementItemList with the corresponding values.
        /// </summary>
        /// <param name="outputFilePath"></param>
        /// <param name="replacementItemList"></param>
        /// <param name="maxRows"></param>
        /// <param name="maxCols"></param>
        /// <param name="errorDetails"></param>
        /// <returns></returns>
        public static bool FillTemplate( string outputFilePath, Hashtable replacementItemList, int maxRows, int maxCols, out string errorDetails ) {

            errorDetails = null;

            ExcelReaderWriter excelReader = new ExcelReaderWriter();

            excelReader.Open( outputFilePath );
            string[] sheetNames = excelReader.GetSheetNames();
            if( sheetNames.Length == 0 ){
                errorDetails = "No worksheets in output file: " + outputFilePath;
                return false;
            }
            excelReader.SetSheet( sheetNames[ 0 ] );

            object[,] sheetValues = (object[,])excelReader.GetValues( 1, 1, 1 + maxRows, 1 + maxCols );

            int nReplacements = 0;

            for( int row = 1; row <=maxRows; row++ ){
                for( int col = 1; col <=maxCols; col++ ){
                    object cellObj = sheetValues[ row, col ];

                    if( cellObj is string ){
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

            excelReader.CopyCells( 4, 2, 6, 3, 10, 4 );

            if( nReplacements > 0 ) {
                excelReader.SaveAndClose();
            }
            else {
                excelReader.Kill();
            }

            return true;
        }
    }
}
