using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DataImporter.Library;
using DataImporter.Output2;
using DataImporter.ImportSettings;

namespace DataImporter
{
    class OutputManager
    {
        private static ExcelWriter2 writer;

        private static string primaryTemplateDir = "Templates";
        private static string secondaryTemplateDir = "..\\Templates";

        private static string displayTemplate = "Display.xls";
        private static string distributionTemplate = "Distribution.xls";
        private static string mediaTemplate = "Media.xls";
        private static string realSalesTemplate = "Real Sales.xls";
        private static string priceTemplate = "Price.xls";
        public static string AbsolutreStartPath;

        private static bool okToOverwrite = false;
        private static bool okToRename = false;

        public static bool Init( string programStartPath ) {
            string startDir = programStartPath.Substring( 0, programStartPath.LastIndexOf( "\\" ) );
            string templateRoot = startDir + "\\" + primaryTemplateDir +  "\\";
            string testDistributionTemplate = templateRoot + distributionTemplate;
            if( File.Exists( testDistributionTemplate ) == false ) {
                templateRoot = templateRoot = startDir + "\\" + secondaryTemplateDir + "\\";
                testDistributionTemplate = templateRoot + distributionTemplate;
                if( File.Exists( testDistributionTemplate ) == false ) {
                    string msg = String.Format( "\r\n    Error: Unable to find output file templates!     \r\n    Files should be in Templates subdir of {0} (or development loc)    ", startDir );
                    MessageBox.Show( msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return false;
                }
            }

            displayTemplate = templateRoot + displayTemplate;
            distributionTemplate = templateRoot + distributionTemplate;
            mediaTemplate = templateRoot + mediaTemplate;
            realSalesTemplate = templateRoot + realSalesTemplate;
            priceTemplate = templateRoot + priceTemplate;
            AbsolutreStartPath = startDir;
            writer = new ExcelWriter2();
            okToOverwrite = false;
            return true;
        }

        public static void WriteOutputFile( WorksheetSettings worksheetSettings, ProjectSettings currentProject ) {

            if( worksheetSettings.DataType == ProjectSettings.DataType.PromoPricePct && worksheetSettings.RelatedWorksheetIndexes == null ) {
                MessageBox.Show( "    Error: WriteOutputFile() called for % Promo without associated Price worksheets!     ", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
                return;
            }

            if( worksheetSettings.DataType == ProjectSettings.DataType.PricePromo ) {
                // promo price is handled only as a secondary sheet
                if( worksheetSettings.RelatedWorksheetIndexes == null ) {
                    MessageBox.Show( "    Error: WriteOutputFile() called for Promo Price without associated % Promo worksheets!     ", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
                }
                return;
            }

            if( worksheetSettings.DataType == ProjectSettings.DataType.PriceRegular ) {
                // regular price may be  a secondary sheet
                if( worksheetSettings.RelatedWorksheetIndexes != null ) {
                    return;
                }
            }

            if( worksheetSettings.DataType == ProjectSettings.DataType.PriceAbsolute ||
                worksheetSettings.DataType == ProjectSettings.DataType.AbsolutePricePct ) {

                MessageBox.Show( "    Error: Absolute Price sheet writing not implemented!    ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
                return;
            }

            if( worksheetSettings.GetSection().Channels.Count == 1 || worksheetSettings.GetSection().ChannelSource != ProjectSettings.InfoSource.FileContents ) {

                IMarketPlanSheetWriter sheetWriter = null;
                switch( worksheetSettings.DataType ) {
                    case ProjectSettings.DataType.Distribution:
                        sheetWriter = new DistributionSheetWriter();
                        break;
                    case ProjectSettings.DataType.Display:
                        sheetWriter = new DisplaySheetWriter();
                        break;
                    case ProjectSettings.DataType.Media:
                        sheetWriter = new MediaSheetWriter();
                        break;
                    case ProjectSettings.DataType.RealSales:
                        sheetWriter = new RealSalesSheetWriter();
                        break;
                    case ProjectSettings.DataType.PriceRegular:
                        sheetWriter = new SinglePriceSheetWriter();
                        break;
                    case ProjectSettings.DataType.PromoPricePct:
                        sheetWriter = new TwoPriceSheetWriter();
                        break;
                }

                string actualSaveFile = null;
                if( CreateEmptyOutputFile( worksheetSettings, out actualSaveFile ) == true ) {
                    Console.WriteLine( "\nWRITE OUTPUT - REGULAR: {0}\n", actualSaveFile );
                    writer.Open( actualSaveFile );
                    sheetWriter.WriteData( worksheetSettings, currentProject, writer, 0 );
                    Console.WriteLine( "Success: Wrote Output file!" );
                    writer.SaveAndClose();
                }
            }
            else {
                //
                Console.WriteLine( "\nWRITE OUTPUT - MULTI, {1} CHANS: {0}\n", worksheetSettings.OutputFile, worksheetSettings.GetSection().Channels.Count );

                string path = worksheetSettings.OutputFile.Substring( 0, worksheetSettings.OutputFile.LastIndexOf( "\\" ) + 1 );
                string fname = worksheetSettings.OutputFile.Substring( worksheetSettings.OutputFile.LastIndexOf( "\\" ) + 1 );
                fname = fname.Substring( 0, fname.LastIndexOf( "." ) );

                for( int chanIndex = 0; chanIndex < worksheetSettings.GetSection().Channels.Count; chanIndex++ ) {
                    string channel = (string)worksheetSettings.GetSection().Channels[ chanIndex ];
                    channel = currentProject.GetChannel( channel, worksheetSettings.GetSection().ChannelSource ).MarketSimName;
                    string savePath = null;
                    if( fname != "All" ) {
                        // file name has channel name for prefix
                        savePath = path + channel + " - " + fname + DataImporter.MarketSimFileExt;
                    }
                    else {
                        // file name is channel name
                        savePath = path + channel + DataImporter.MarketSimFileExt;
                    }

                    IMarketPlanSheetWriter sheetWriter = null;
                    switch( worksheetSettings.DataType ) {
                        case ProjectSettings.DataType.Distribution:
                            sheetWriter = new DistributionSheetWriter();
                            break;
                        case ProjectSettings.DataType.Display:
                            sheetWriter = new DisplaySheetWriter();
                            break;
                        case ProjectSettings.DataType.Media:
                            sheetWriter = new MediaSheetWriter();
                            break;
                        case ProjectSettings.DataType.RealSales:
                            sheetWriter = new RealSalesSheetWriter();
                            break;
                        case ProjectSettings.DataType.PriceRegular:
                            sheetWriter = new SinglePriceSheetWriter();
                            break;
                        case ProjectSettings.DataType.PromoPricePct:
                            sheetWriter = new TwoPriceSheetWriter();
                            break;
                    }
                    
                    string actualSavePath = null;
                    if( CreateEmptyOutputFile( savePath, worksheetSettings.DataType, out actualSavePath ) == true ) {

                        Console.WriteLine( "\nCHAN FILE {0}: {1}", chanIndex, actualSavePath );

                        writer.Open( actualSavePath );
                        sheetWriter.WriteData( worksheetSettings, currentProject, writer, chanIndex );
                        Console.WriteLine( "Success: Wrote Output file!" );
                        writer.SaveAndClose();
                    }
                }
            }

        }

        public static bool CreateEmptyOutputFile( WorksheetSettings worksheetSettings, out string actualPath ) {
            return CreateEmptyOutputFile( worksheetSettings.OutputFile, worksheetSettings.DataType, out actualPath );
        }

        public static bool CreateEmptyOutputFile( string path, ProjectSettings.DataType type, out string actualPath ) {
            actualPath = path;
            string template = null;
            switch( type ) {
                case ProjectSettings.DataType.Display:
                    template = displayTemplate;
                    break;
                case ProjectSettings.DataType.Distribution:
                    template = distributionTemplate;
                    break;
                case ProjectSettings.DataType.Media:
                    template = mediaTemplate;
                    break;
                case ProjectSettings.DataType.RealSales:
                    template = realSalesTemplate;
                    break;
                case ProjectSettings.DataType.Price:
                case ProjectSettings.DataType.PriceRegular:
                case ProjectSettings.DataType.PromoPricePct:
                    template = priceTemplate;
                    break;
            }
            string fileDir = path.Substring( 0, path.LastIndexOf( "\\" ) );
            if( Directory.Exists( fileDir ) == false ) {
                Directory.CreateDirectory( fileDir );
            }
            // check for files
            if( File.Exists( template ) == false ) {
                string msg = String.Format( "\n    Error: Template file not found!    \n\n    File: {0}    \n", template );
                MessageBox.Show( msg, "Error Creating Output", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return false;
            }

            if( File.Exists( path ) == true ) {
                if( okToOverwrite == false && okToRename == false ) {

                    Dialogs.YesNoAllEtcForm okForm = new Dialogs.YesNoAllEtcForm( path );
                    DialogResult resp = okForm.ShowDialog();
                    if( resp == DialogResult.Cancel ) {
                        return false;
                    }
                    if( okForm.YesToAll ) {
                        okToOverwrite = true;
                    }
                    else if( okForm.No ) {
                        return false;
                    }
                    else if( okForm.Rename ) {
                        okToRename = true;
                    }

                    ////string msg = String.Format( "\n    Warning : Output file exists already.    \n\n    File: {0}    \n\n" +
                    ////    "OK to overwrite out files?    \n    (Yes --> yes to all)\n(Cancel --> yes, this file only)\n", path );
                    ////DialogResult resp = MessageBox.Show( msg, "Error Creating Output", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error );
                    ////if( resp == DialogResult.No ) {
                    ////    return false;
                    ////}
                    ////if( resp == DialogResult.Yes ) {
                    ////    okToOverwrite = true;
                    ////}
                }

                if( okToOverwrite == true ) {
                    try {
                        File.Delete( path );
                    }
                    catch( IOException ) {
                        string msg = String.Format( "\r\n     Error: Unable to delete existing file to be replaced!    \r\n\r\n    File: {0}    \r\n\r\n", path );
                        MessageBox.Show( msg, "Error Creating Output", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        return false;
                    }
                }
                else if( okToRename == true ) {
                    string pathHead = path.Substring( 0, path.Length - DataImporter.MarketSimFileExt.Length );
                    for( int n = 1; n < 9999; n++ ) {
                        path = String.Format( "{0}-{1}{2}", pathHead, n, DataImporter.MarketSimFileExt );
                        if( File.Exists( path ) == false ) {
                            break;
                        }
                    }
                }
            }
            File.Copy( template, path );
            actualPath = path;
            return true;
        }
    }
}
