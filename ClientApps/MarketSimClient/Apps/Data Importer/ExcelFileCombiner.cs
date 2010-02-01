using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Windows.Forms;

using DataImporter.Library;

namespace DataImporter
{
    class ExcelFileCombiner
    {
        private bool overwriteOK = false;

        private ExcelWriter2 src1Reader;
        private ExcelWriter2 src2Reader;
        private ExcelWriter2 destReader;

        private int nFileDataRows = -1;

        public ExcelFileCombiner() {
            overwriteOK = false;
            src1Reader = new ExcelWriter2();
            src2Reader = new ExcelWriter2();
            destReader = new ExcelWriter2();
        }

        public void CombineFileValues( string src1, int sheetIndex1, string src2, int sheetIndex2, string dest, string destSheet, string rule ) {

            if( File.Exists( dest ) ) {        
                return;       //don't re-compute if the output exists already (using manual results-folder clearing)!!!!
            }

            if( (overwriteOK == false) && File.Exists( dest ) ) {
                string msg = String.Format( "   Warning: File exists already: {0}    \r\n\r\nOK to overwrite existing files?", dest );
                DialogResult resp = MessageBox.Show( msg, "Confirm Overwrite", MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
                if( resp != DialogResult.OK ){
                    return;
                }
                overwriteOK = true;
            }

            if( File.Exists( dest ) ) {
                File.Delete( dest );
            }
            if( dest.IndexOf( "\\" ) != -1 ) {
                string chkPath = dest.Substring( 0, dest.LastIndexOf( "\\" ) );
                if( Directory.Exists( chkPath ) == false ) {
                    Directory.CreateDirectory( chkPath );
                }
            }
            try {
                File.Copy( src2, dest );
                if( File.Exists( dest ) == false ) {
                    string msg = String.Format( "\r\n\r\n    Error: Copied destination does not exist: \r\n\r\n{0}", dest );
                    MessageBox.Show( msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                    return;
                }

                src1Reader.Open( src1 );
                string[] sheets1 = src1Reader.GetSheetNames();
                src1Reader.SetSheet( sheets1[ sheetIndex1 ] );

                src2Reader.Open( src2 );
                string[] sheets2 = src2Reader.GetSheetNames();
                src2Reader.SetSheet( sheets2[ sheetIndex2 ] );

                destReader.Open( dest );
                destReader.SetSheet( sheets2[ sheetIndex2 ] );
            }
            catch( Exception e ) {
                DialogResult resp = MessageBox.Show( "\r\n    Error: " + e.Message + "    \r\n\r\n    Continue Processing remaining files?    \r\n", 
                    "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
                if( resp != DialogResult.OK ) {
                    src1Reader.Kill();
                    src2Reader.Kill();
                    destReader.Kill();
                    Environment.Exit( 0 );
                }
            }

            //process the data!
            ProcessData( rule );
            destReader.SaveAndClose();

            src1Reader.Kill();
            src2Reader.Kill();
        }

        public void Close() {
        }

        private void ProcessData( string rule ){
            if( rule != "divide" ) {
                MessageBox.Show( "Unsupported Combining Rule in ExcelFileCombiner!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            //the data starts at cell 2,2 (Dentsu files)
            int row0 = 2;
            int col0 = 2;

            // scan the 1st row
            int nCols = 1;
            do {
                object cellObj = src1Reader.GetValue( row0, col0 + (nCols - 1) );
                if( (cellObj is double) == false ) {
                    break;
                }
                nCols += 1;
            }
            while( true );
            nCols -= 1;
            Console.WriteLine( " Found {0} Columns...", nCols );

            int nRows = 1;
            if( nFileDataRows == -1 ) {
                // scan the 1st col
                do {
                    object cellObj = src1Reader.GetValue( row0 + (nRows - 1), col0 );
                    if( (cellObj is double) == false ) {
                        break;
                    }
                    nRows += 1;
                }
                while( true );
                nRows -= 1;
                Console.WriteLine( " Found {0} Rows...", nRows );
                nFileDataRows = nRows;
            }
            else {
                // use
                nRows = nFileDataRows;
            }

            //now do the actual deed
           object[ , ] src1Data = (object[ , ])src1Reader.GetValues( row0, col0, row0 + nRows - 1, col0 + nCols - 1 );
           object[ , ] src2Data = (object[ , ])src2Reader.GetValues( row0, col0, row0 + nRows - 1, col0 + nCols - 1 );
           object[ , ] results = (object[ , ])destReader.GetValues( row0, col0, row0 + nRows - 1, col0 + nCols - 1 );

            for( int r = 1; r <= nRows; r++ ) {
                for( int c = 1; c <= nCols; c++ ) {
                    object o1 = src1Data[ r, c ];
                    object o2 = src2Data[ r, c ];

                    double v1 = (double)src1Data[ r, c ];
                    double v2 = (double)src2Data[ r, c ];
                    double resVal = 0.0;
                    if( v2 != 0.0 ) {
                        // we know the rule is "divide"
                        resVal = v1 / v2;
                    }
                    results[ r, c ] = resVal;
                }
            }

            destReader.SetValues( row0, col0,  row0 + nRows - 1, col0 + nCols - 1, results );
        }
    }
}
