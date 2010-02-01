using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

using NitroReader.Dialogs;
using NitroReader.Library;

namespace NitroReader.Output
{
    class ReportGenerator
    {
        private static string reportFolder;
        private static string reportFilename;
        private static DateTime reportTime;

        private static string reportTemplate = "ReportTemplate.xls";
        private static string reportSheet = "Report";
        private static string reportNameHead = "NITRO Reader Report - ";
        private static string reportNameExt = ".xls";

        //private static string footer1 = "NITRO Reader is a product of DecisionPower Inc. www.decisionpower.com       (c)2007";

        public static void Init() {
            reportFolder = System.Environment.CurrentDirectory;
            reportFilename = null;
        }

        public static void ShowReport() {
            try {
                System.Diagnostics.Process.Start( reportFilename );
            }
            catch (Exception e ) {
                // unable to show report
                ConfirmForm cform = new ConfirmForm( String.Format( "Unable to display report file.\r\n\r\nError: {0}", e.Message ), "Error Showing Report" );
            }
        }

        public static bool GenerateReport( string inputFilename, string outputFilename, ArrayList productsAndGroups, MarketPlan marketPlan, 
                                            Settings settings, GeneralSettingsValues generalSettings, ExcelWriter2 writer ) {
            bool useTextFormat = generalSettings.TextReport;
            bool templateFound = false;
            string realReportTemplate = ProductFile.FilePath( reportTemplate, "templates" );
            if( useTextFormat == false ) {
                // copy an Excel template
                if( File.Exists( realReportTemplate ) == false ) {
                    string w = String.Format( "Unable to find report template.  Report generated in text format.\r\n\r\nExpected template file {0}", realReportTemplate );
                    MarketPlan.warnings.Add( w );
                    reportNameExt = ".txt";
                    useTextFormat = true;
                }
                else {
                    templateFound = true;
                }
            }

            // get a filename that we are sure is ok
            GenerateReportFileName();
            if( templateFound ) {
                File.Copy( realReportTemplate, reportFilename );
            }

            if( useTextFormat == false ) {
                if( writer != null ) {
                    writer.Kill();
                    writer = null;
                }
                writer = new ExcelWriter2();

                writer.Open( reportFilename, reportSheet );

                // actually write the report data

                writer.FillCell( 2, 2, reportTime.ToString( "MMM d, yyyy" ) );
                writer.FillCell( 2, 3, reportTime.ToString( "hh:mm:sstt" ) );
                writer.FillCell( 3, 2, inputFilename );
                writer.FillCell( 4, 2, outputFilename );
                writer.FillCell( 1, 5, marketPlan.Name );
                writer.FillCell( 2, 5, marketPlan.StartDate.ToShortDateString() );
                writer.FillCell( 2, 7, marketPlan.EndDate.ToShortDateString() );
                writer.FillCell( 6, 8, marketPlan.Components.Count.ToString() );
                writer.FillCell( 6, 2, marketPlan.Variants.Count.ToString() );

                int compRow = 8;
                int compCol = 7;
                for( int c = 0; c < marketPlan.Components.Count; c++ ) {
                    MarketPlan.Component comp = (MarketPlan.Component)marketPlan.Components[ c ];
                    writer.FillCell( compRow, compCol, comp.Name );
                    writer.FillCell( compRow, compCol + 1, comp.StartDate.ToShortDateString() );
                    writer.FillCell( compRow, compCol + 2, comp.EndDate.ToShortDateString() );
                    int wks = (int)(comp.TimeStep.TotalDays / 7);
                    writer.FillCell( compRow, compCol + 3, comp.StepCount.ToString() );
                    writer.FillCell( compRow, compCol + 4, wks.ToString() );
                    compRow += 1;
                }

 //               int varRow = compRow + 3;
                int varRow = 8;
                int varCol = 1;
                bool inGroup = false;
                for( int v = 0; v < productsAndGroups.Count; v++ ) {
                    MarketPlan.VariantInfo info = (MarketPlan.VariantInfo)productsAndGroups[ v ];
                    if( info.GroupIndex == -1 ) {
                        if( inGroup ) {
                            inGroup = false;
                            varRow += 1;
                        }
                        writer.FillCell( varRow, varCol, info.MarketSimName );
                        writer.FillCell( varRow, varCol + 2,  info.Name );
                        writer.FillCell( varRow, varCol + 3,  info.Volume.ToString() );
                    }
                    else {
                        if( info.IsGroup ) {
                            inGroup = true;
                            if( v > 0 ) {
                                varRow += 1;
                            }
                            double corr = ((Settings.GroupInfo)settings.Groups[ info.GroupIndex ]).Correlation * 100;
                            writer.CellBackColor( varRow, varCol, 8 );
                            writer.CellBackColor( varRow, varCol + 1, 8 );
                            writer.CellBackColor( varRow, varCol + 2, 8 );
                            writer.FillCell( varRow, varCol, info.MarketSimName );
                            writer.CellBackColor( varRow, varCol + 3, 8 );
                            writer.CellBackColor( varRow, varCol + 4, 8 );
                            writer.FillCell( varRow, varCol + 3, info.Volume.ToString() );
                            writer.FillCell( varRow, varCol + 4, corr.ToString() );
                        }
                        else {
                            writer.FillCell( varRow, varCol + 1, info.MarketSimName );
                            writer.FillCell( varRow, varCol + 2, info.Name );
                            writer.FillCell( varRow, varCol + 3, info.Volume.ToString() );
                        }
                    }
                    varRow += 1;
                }
                writer.SaveAndClose();
            }
            else {
                // generate a text report
                FileStream fs = new FileStream( reportFilename, FileMode.CreateNew, FileAccess.Write );
                StreamWriter sw = new StreamWriter( fs );

                sw.WriteLine( "NITRO Reader Processing Report  {0}" + reportTime.ToString() );
                sw.WriteLine();
                sw.WriteLine( "Input Path: {0}", inputFilename );
                sw.WriteLine( "Output Path: {0}", inputFilename );
                sw.WriteLine();
                sw.WriteLine( "Plan Name: {0}", marketPlan.Name );
                sw.WriteLine( "Start, End: {0}, {1}      Component Count: {2}    Variant Count: {3}", marketPlan.StartDate.ToShortDateString(), 
                    marketPlan.EndDate.ToShortDateString(), marketPlan.Components.Count, marketPlan.Variants.Count );

                string componentHeader = "  Name                           Start      End       Step (weeks)   Step Count";
                string componentHeader2 = "--------------------------     ---------- ---------  -------------  --------------";
                string componentFormat = "{0,-30} {1,-10} {2,-10}   {3,-13} {4}";

                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine( componentHeader );
                sw.WriteLine( componentHeader2 );

                for( int c = 0; c < marketPlan.Components.Count; c++ ) {
                    MarketPlan.Component comp = (MarketPlan.Component)marketPlan.Components[ c ];
                    string line = String.Format( componentFormat, comp.Name, comp.StartDate.ToShortDateString(), comp.EndDate.ToShortDateString(),
                        comp.TimeStep.TotalDays / 7, comp.StepCount );
                    sw.WriteLine( line );
                    //sw.WriteLine( "\r\nComponent Name: {0}", comp.Name );
                    //sw.WriteLine( "Time Step: {0}", comp.TimeStep.ToString() );
                    //sw.WriteLine( "Start: {0}", comp.StartDate.ToShortDateString() );
                    //sw.WriteLine( "End: {0}", comp.EndDate.ToShortDateString() );
                    //sw.WriteLine( "Step Count: {0}", comp.StepCount.ToString() );
                }

                string variantHeader = "  MarketSim Name                     NITRO Name                               Volume     Group Correlation";
                string variantHeader2 = "---------------------------------  -------------------------------------     ----------  -----------------";
                string variantItemHeader = "{0,-38} {1,-38} {2,-10}";
                string groupItemHeader = "{0,-30}                                                {2,-8}     {1,-2:f0}%";
                string variantSubitemHeader = "     {0,-35} {1,-35}  {2,-10}";

                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine( variantHeader );
                sw.WriteLine( variantHeader2 );

                bool inGroup = false;
                for( int v = 0; v < productsAndGroups.Count; v++ ) {
                    MarketPlan.VariantInfo info = (MarketPlan.VariantInfo)productsAndGroups[ v ];
                    string line = "";
                    if( info.GroupIndex == -1 ) {
                        if( inGroup ) {
                            inGroup = false;
                            sw.WriteLine();
                        }
                        line = String.Format( variantItemHeader, info.MarketSimName, info.Name, info.Volume );
                    }
                    else {
                        if( info.IsGroup ) {
                            inGroup = true;
                            sw.WriteLine();
                            double corr = ((Settings.GroupInfo)settings.Groups[ info.GroupIndex ]).Correlation * 100;
                            line = String.Format( groupItemHeader, info.MarketSimName, corr, info.Volume );
                        }
                        else {
                            line = String.Format( variantSubitemHeader, info.MarketSimName, info.Name, info.Volume );
                        }
                    }
                    sw.WriteLine( line );
                    //sw.WriteLine( "Variant Name: {0}", info.Name );
                    //sw.WriteLine( "MarketSimName: {0}", info.MarketSimName );
                    //sw.WriteLine( "Volume: {0}", info.Volume.ToString() );

                }
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            return true;
        }

        private static string GenerateReportFileName() {
            // we will identify each report by date/time and sequence
            int sanityCheckCount = 10;
            do {
                reportTime = DateTime.Now;
                string nowString = reportTime.ToString( "yyMMdd-hhmmss" );

                // the file name consiste of the fixed head, the date in sortable format, and the extension
                reportFilename = reportFolder + "\\" + reportNameHead + nowString + reportNameExt;
                if( File.Exists( reportFilename ) == false ) {
                    // we found a non-existent filename -- return it
                    return reportFilename;
                }
                // the name is used already! wait a moment and try again
                Thread.Sleep( 800 );
            }
            while( sanityCheckCount-- > 0 );
            return null;
        }
    }
}
