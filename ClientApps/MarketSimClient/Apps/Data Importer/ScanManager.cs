using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;

using DataImporter.Library;
using DataImporter.Dialogs;
using DataImporter.ImportSettings;

namespace DataImporter
{
    public class ScanManager
    {
        private ProjectSettings currentProject;

        private string[] regularPriceSheetNames = new string[] { "non-promo unit price" };
        private string[] promoPriceSheetNames = new string[] { "any promo unit price" };
        private string[] promoPriceDistSheetNames = new string[] { "%acv any promo" };
        private string[] absPriceSheetNames = new string[] { "display price absolute" };
        private string[] absPriceDistSheetNames = new string[] { "%acv display price absolute", "%acv display price absolu" };

        private string[] displaySheetNames = new string[] { "%acv any display" };
        private string[] distributionSheetNames = new string[] { "%acv dist", "store coverage (max)" };
        private string[] realSalesSheetNames = new string[] { "units", "total sales in yen" };

        private Hashtable knownSheetNames;

        //private DataTable sheetNamesTable;
        //public DataTable SheetNamesTable {
        //    get {
        //        return sheetNamesTable;
        //    }
        //}

        private DataTable worksheetDisplayTable;
        private FileSettings fileSettings;
        //private string activeWorksheetName;
        //private int activeWorksheetIndex;
        //private Hashtable prefetchedWorksheetDataList;

        private static ExcelWriter2 reader;
        private static string readerFile;
        //private static string readerWorksheet;
        private static Form readerOwner;

        public static ExcelWriter2 Reader {
            get { return reader; }
            set { reader = value; }
        }

        public ScanManager( Form owner ) {
            readerOwner = owner;

            knownSheetNames = new Hashtable();
            knownSheetNames.Add( "Price (Regular)", regularPriceSheetNames );
            knownSheetNames.Add( "Price (Promo)", promoPriceSheetNames );
            knownSheetNames.Add( "Price (Absolute)", absPriceSheetNames );
            knownSheetNames.Add( "Price (% Promo)", promoPriceDistSheetNames );
            knownSheetNames.Add( "Price (% Absolute)", absPriceDistSheetNames );
            knownSheetNames.Add( "Display", displaySheetNames );
            knownSheetNames.Add( "Distribution", distributionSheetNames );
            knownSheetNames.Add( "Real Sales", realSalesSheetNames );
        }

        /// <summary>
        /// Scans all of the sections of the project for file format specifics.  Assumes the files have already been verified to exist.
        /// </summary>
        /// <param name="project"></param>
        public void ScanAllSections( ProjectSettings project ) {
            currentProject = project;
            for( int i = 0; i < currentProject.SectionCount; i++ ) {
                ProjectSettings.ProjectSection section = currentProject.GetSection( i );
                ScanSection( project, section );

                CloseReader();
            }
        }

        public void ScanSection( ProjectSettings project, ProjectSettings.ProjectSection section ) {
            currentProject = project;
            if( section.IsFileSet == false ) {
                ScanSingleFileSection( section );
            }
            else {
                ScanMultiFileSection( section );
            }
        }

        public bool ScanAllWorksheetDates( WorksheetSettings worksheetSettings ) {
            WorksheetScanner scanner = new WorksheetScanner( worksheetSettings.SheetName );
            bool success = scanner.ScanAllWorksheetDates( worksheetSettings, false );
            return success;
        }

        public bool VerifyAllWorksheetDates( WorksheetSettings worksheetSettings ) {
            WorksheetScanner scanner = new WorksheetScanner( worksheetSettings.SheetName );
            bool success = scanner.ScanAllWorksheetDates( worksheetSettings, true );
            return success;
        }

        public bool ScanAllWorksheetChannels( WorksheetSettings worksheetSettings ) {
            WorksheetScanner scanner = new WorksheetScanner( worksheetSettings.SheetName );
            bool success = scanner.ScanAllWorksheetChannels( worksheetSettings, false );
            return success;
        }

        public bool VerifyAllWorksheetChannels( WorksheetSettings worksheetSettings ) {
            WorksheetScanner scanner = new WorksheetScanner( worksheetSettings.SheetName );
            bool success = scanner.ScanAllWorksheetChannels( worksheetSettings, true );
            return success;
        }

        public bool ScanAllWorksheetVariants( WorksheetSettings worksheetSettings, ProjectSettings projectSettings ) {
            WorksheetScanner scanner = new WorksheetScanner( worksheetSettings.SheetName );
            bool success = scanner.ScanAllWorksheetVariants( worksheetSettings, false, projectSettings );
            //if( success ) {
            //    success = scanner.ScanAllWorksheetChannels( worksheetSettings, false );
            //}
            return success;
        }

        public bool VerifyAllWorksheetVariants( WorksheetSettings worksheetSettings, ProjectSettings projectSettings ) {
            WorksheetScanner scanner = new WorksheetScanner( worksheetSettings.SheetName );
            bool success = scanner.ScanAllWorksheetVariants( worksheetSettings, true, projectSettings );
            return success;
        }

        public int GetVariantCount( WorksheetSettings worksheetSettings ) {
            WorksheetScanner scanner = new WorksheetScanner( worksheetSettings.SheetName );
            return scanner.GetVariantCount( worksheetSettings );
        }

        public int GetChannelCount( WorksheetSettings worksheetSettings ) {
            WorksheetScanner scanner = new WorksheetScanner( worksheetSettings.SheetName );
            return scanner.GetChannelCount( worksheetSettings );
        }

        public bool ReadWorksheetData( WorksheetSettings worksheetSettings, int variantCount, int channelCount, double compressionTolerance ) {
            WorksheetScanner scanner = new WorksheetScanner( worksheetSettings.SheetName );
            bool success = scanner.ReadWorksheetData( worksheetSettings, variantCount, channelCount, compressionTolerance );
            return success;
        }

        /// <summary>
        /// Scans a file to determine the arrangement of data in its worksheets.
        /// </summary>
        /// <param name="section"></param>
        private void ScanSingleFileSection( ProjectSettings.ProjectSection section ) {
            string sectionFilePath = currentProject.AbsolutePathFor( section.SpecificFile );
            fileSettings = new FileSettings( sectionFilePath );
            bool brandsFromWorksheets = (section.BrandSource == ProjectSettings.InfoSource.WorksheetName);
            bool channelsFromData = (section.ChannelSource == ProjectSettings.InfoSource.FileContents);

            OpenReader( sectionFilePath );

            CreateWorksheetTable();
            AddWorksheetsToDisplayTable( brandsFromWorksheets );

            IWorksheetSelectingForm selForm = null;
            ProjectSettings.DataType fileDataType = ProjectSettings.DataType.Unknown;

            if( brandsFromWorksheets ) {
                //show the type-selection form
                SetFileTypeForm typeForm = new SetFileTypeForm( section.SpecificFile );
                DialogResult resp1 = typeForm.ShowDialog();
                if( resp1 != DialogResult.OK ) {
                    return;
                }
                fileDataType = typeForm.DataType;
                section.DataTypeSource = ProjectSettings.InfoSource.UserSpecified;

                //show the worksheet selection form
                selForm = new SelectBrandWorksheetsForm( section.SpecificFile, worksheetDisplayTable );
            }
            else {
                selForm = new SelectWorksheetsForm( section.SpecificFile, worksheetDisplayTable );
            }
            DialogResult resp = ((Form)selForm).ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
            string[] selectedSheetNames = GetSelectedWorksheets( selForm.SheetNamesTable, !brandsFromWorksheets );   //!!! last arg was "false" in the version used to process Dentsu phase II !!!
            if( selectedSheetNames.Length == 0 ) {
                return;
            }
            ProjectSettings.DataType[] selectedSheetTypes = GetSelectedWorksheetTypes( selForm.SheetNamesTable );
            Array.Sort( selectedSheetNames, selectedSheetTypes );

            //scan each selected worksheet
            for( int i = 0; i < selectedSheetNames.Length; i++ ) {

                // scan the sheet
                string shtName = selectedSheetNames[ i ];
                if( brandsFromWorksheets ) {
                    ProjectSettings.BrandInfo brand = currentProject.AddBrand( shtName, section.BrandSource );
                }
                WorksheetScanner scanner = new WorksheetScanner( shtName );
                ScanManager.Reader.SetSheet( shtName );
                WorksheetSettings wsSettings = scanner.Scan( channelsFromData );
                wsSettings.SetSection( section );

                // show the scan results and allow user adjustments
                WorksheetSettingsForm wsForm = new WorksheetSettingsForm( section.SpecificFile, wsSettings );
                DialogResult resp3 = wsForm.ShowDialog();
                if( resp3 != DialogResult.OK ) {
                    return;
                }
                wsSettings = wsForm.WorksheetSettings;         // possibly update based on user inputs

                if( brandsFromWorksheets ) {
                    wsSettings.DataType = fileDataType;

                    ProjectSettings.BrandInfo brnd = currentProject.AddBrand( shtName, section.BrandSource );       // sheet name = brand id
                    wsSettings.Brand = brnd;
                }
                else {
                    // use the data type set in the worksheet-selection dialog
                    wsSettings.DataType = selectedSheetTypes[ i ];
                }

                if( section.BrandSource == ProjectSettings.InfoSource.FileContents ) {
                    ProjectSettings.BrandInfo brnd = currentProject.AddBrand( "All", section.BrandSource );       // sheet name = brand id
                    wsSettings.Brand = brnd;
                }

                if( section.ChannelSource == ProjectSettings.InfoSource.UserSpecified ) {
                    string specChannel = section.SpecificChannel;
                    ProjectSettings.ChannelInfo chan = currentProject.AddChannel( specChannel, section.ChannelSource );
                    wsSettings.Channel = chan;
                }
                else if( section.ChannelSource == ProjectSettings.InfoSource.FileContents ) {
                    // the channel info is in the file data
                    ProjectSettings.ChannelInfo chan = currentProject.AddChannel( "All", section.ChannelSource );
                    wsSettings.Channel = chan;
                }

                if( wsSettings.DataType == ProjectSettings.DataType.Media ) {
                    if( wsSettings.Campaign == null ) {
                        wsSettings.Campaign = "";
                    }
                    MediaCampaignForm campForm = new MediaCampaignForm( wsSettings.Campaign );
                    DialogResult resp5 = campForm.ShowDialog();
                    if( resp5 == DialogResult.OK ) {
                        wsSettings.Campaign = campForm.Campaign;
                    }
                }
                section.AddWorksheet( wsSettings );

                // see if we can copy the settings from the first sheet to the other ones
                if( i == 0 && selectedSheetNames.Length > 1 ) {
                    DialogResult resp4 = MessageBox.Show( "   Do the other worksheets in this file use the same format?    ",
                        "Format Check", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
                    if( resp4 == DialogResult.Yes ) {

                        for( int j = 1; j < selectedSheetNames.Length; j++ ) {
                            WorksheetSettings wsSettings2 = new WorksheetSettings( wsSettings );

                            wsSettings2.SheetName = selectedSheetNames[ j ];

                            if( brandsFromWorksheets ) {
                                ProjectSettings.BrandInfo brnd2 = currentProject.AddBrand( wsSettings2.SheetName, section.BrandSource );   // sheet name = brand id
                                wsSettings2.Brand = brnd2;
                            }
                            else {
                                wsSettings2.DataType = selectedSheetTypes[ j ];
                            }

                            section.AddWorksheet( wsSettings2 );
                        }
                        break;
                    }
                }
            }

            // determine if any of the worksheets are part of multi-sheet groups
            for( int w = 0; w < section.WorksheetSettingsList.Count; w++ ) {
                WorksheetSettings ws = (WorksheetSettings)section.WorksheetSettingsList[ w ];
                ws.RelatedWorksheetIndexes = null;
            }

            for( int w = 0; w < section.WorksheetSettingsList.Count; w++ ) {
                WorksheetSettings ws = (WorksheetSettings)section.WorksheetSettingsList[ w ];

                // if we have a % promo price sheet (requisite for promo prices), then we also must have promo and unpromo prices
                if( ws.DataType == ProjectSettings.DataType.PromoPricePct ) {
                    int wPct = w;
                    int wPromo = -1;
                    int wUnpromo = -1;
                    for( int w2 = 0; w2 < section.WorksheetSettingsList.Count; w2++ ) {
                        if( w2 == w ) {
                            continue;
                        }
                        WorksheetSettings ws2 = (WorksheetSettings)section.WorksheetSettingsList[ w2 ];

                        if( ws2.DataType == ProjectSettings.DataType.PriceRegular ) {
                            wUnpromo = w2;
                        }
                        else if( ws2.DataType == ProjectSettings.DataType.PricePromo ) {
                            wPromo = w2;
                        }
                    }
                    if( wPromo == -1 ) {
                        MessageBox.Show( "    Error: Section includes % Promo worksheet but has no Promo Price worksheet.   ", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error );
                        return;
                    }
                    if( wUnpromo == -1 ) {
                        MessageBox.Show( "    Error: Section includes % Promo worksheet but has no Unpromo Price worksheet.   ", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error );
                        return;
                    }

                    ws.RelatedWorksheetIndexes = new ArrayList();                        // %promo is the primary worksheet of the group -- add subindexes
                    ws.RelatedWorksheetIndexes.Add( wPromo );
                    ws.RelatedWorksheetIndexes.Add( wUnpromo );

                     // empty list means another worksheet is the primary
                    ((WorksheetSettings)section.WorksheetSettingsList[ wUnpromo ]).RelatedWorksheetIndexes = new ArrayList();
                    ((WorksheetSettings)section.WorksheetSettingsList[ wPromo ]).RelatedWorksheetIndexes = new ArrayList(); 

                    MessageBox.Show( "     Group Detected: Promo-Price Worksheet Group Found!    ", "Worksheet Group OK", MessageBoxButtons.OK, MessageBoxIcon.Information );
                }
            }


            section.Scanned = true;
        }

        private void ScanMultiFileSection( ProjectSettings.ProjectSection section ) {
            string secDir = currentProject.AbsolutePathFor( section.FleSetFolder );
            string[] files = Directory.GetFiles( secDir, DataImporter.InputFilePattern, SearchOption.AllDirectories );
            string sectionFilePath = files[ 0 ];
            fileSettings = new FileSettings( sectionFilePath );

            if( section.ChannelSource == ProjectSettings.InfoSource.DirectoryName || section.BrandSource == ProjectSettings.InfoSource.DirectoryName ) {
                string[] dirs = Directory.GetDirectories( secDir, "*", SearchOption.AllDirectories );
                string[] itemIdentifiers = GetUniquePartsOf( dirs );
                Array.Sort( itemIdentifiers );
                for( int i = 0; i < itemIdentifiers.Length; i++ ) {
                    if( section.ChannelSource == ProjectSettings.InfoSource.DirectoryName ) {
                        currentProject.AddChannel( itemIdentifiers[ i ], section.ChannelSource );
                    }
                    if( section.BrandSource == ProjectSettings.InfoSource.DirectoryName ) {
                        currentProject.AddBrand( itemIdentifiers[ i ], section.BrandSource );
                    }
                }
            }

            if( section.ChannelSource == ProjectSettings.InfoSource.FileName || section.BrandSource == ProjectSettings.InfoSource.FileName ) {
                ArrayList flist = new ArrayList();
                foreach( string f in files ) {
                    string fname = f;
                    if( f.IndexOf( "\\" ) != -1 ) {
                        fname = f.Substring( f.LastIndexOf( "\\" ) + 1 );
                        flist.Add( fname );
                    }
                }
                string[] farray = new string[ flist.Count ];
                flist.CopyTo( farray );
                Array.Sort( farray );
                for( int i = 0; i < farray.Length; i++ ) {
                    if( section.ChannelSource == ProjectSettings.InfoSource.FileName ) {
                        currentProject.AddChannel( farray[ i ], section.ChannelSource );
                    }
                    if( section.BrandSource == ProjectSettings.InfoSource.FileName ) {
                        currentProject.AddBrand( farray[ i ], section.BrandSource );
                    }
                }
            }

            OpenReader( sectionFilePath );

            CreateWorksheetTable();
            AddWorksheetsToDisplayTable( false );

            // show the worksheet-selection form
            SelectWorksheetsForm selForm = new SelectWorksheetsForm( section.FleSetFolder, worksheetDisplayTable );
            DialogResult resp = selForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
            string[] selectedSheetNames = GetSelectedWorksheets( selForm.SheetNamesTable, true );
            if( selectedSheetNames.Length == 0 ) {
                return;
            }
            ProjectSettings.DataType[] selectedSheetTypes = GetSelectedWorksheetTypes( selForm.SheetNamesTable );
            Array.Sort( selectedSheetNames, selectedSheetTypes );

            //scan each selected worksheet
            for( int i = 0; i < selectedSheetNames.Length; i++ ) {

                // scan the sheet
                string shtName = selectedSheetNames[ i ];
                WorksheetScanner scanner = new WorksheetScanner( shtName );
                ScanManager.Reader.SetSheet( shtName );
                WorksheetSettings wsSettings = scanner.Scan( false );


                // show the scan results and allow user adjustments
                string sectionFileRelPath = sectionFilePath.Substring( currentProject.InputRootDirectory.Length + 1 );
                WorksheetSettingsForm wsForm = new WorksheetSettingsForm( sectionFileRelPath, wsSettings );
                DialogResult resp3 = wsForm.ShowDialog();
                if( resp3 != DialogResult.OK ) {
                    return;
                }
                wsSettings = wsForm.WorksheetSettings;         // possibly update based on user inputs

                if( section.ChannelSource == ProjectSettings.InfoSource.UserSpecified ) {
                    ProjectSettings.ChannelInfo specChannel = currentProject.AddChannel( section.SpecificChannel, ProjectSettings.InfoSource.UserSpecified );
                    wsSettings.Channel = specChannel;
                }
                if( section.BrandSource == ProjectSettings.InfoSource.UserSpecified ) {
                    ProjectSettings.BrandInfo specBrand = currentProject.AddBrand( section.SpecificBrand, ProjectSettings.InfoSource.UserSpecified );
                    wsSettings.Brand = specBrand;
                }

                wsSettings.DataType = selectedSheetTypes[ i ];
                // brand and channel are set when scanning (since they depend on the specific file being processed)
                section.AddWorksheet( wsSettings );

                // see if we can copy the settings from the first sheet to the other ones
                if( i == 0 && selectedSheetNames.Length > 1 ) {
                    DialogResult resp4 = MessageBox.Show( "   Do the other worksheets in this file use the same format?    ",
                        "Format Check", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
                    if( resp4 == DialogResult.Yes ) {

                        for( int j = 1; j < selectedSheetNames.Length; j++ ) {
                            WorksheetSettings wsSettings2 = new WorksheetSettings( wsForm.WorksheetSettings );
                            wsSettings2.SheetName = selectedSheetNames[ j ];
                            wsSettings2.DataType = selectedSheetTypes[ j ];
                            section.AddWorksheet( wsSettings2 );
                        }
                        break;
                    }
                }
            }
            section.Scanned = true;
        }

        private string[] GetSelectedWorksheets( DataTable sheetNamesDisplayTable, bool addToMasterWorksheetTable ) {
            ArrayList selSheets = new ArrayList();
            for( int i = 0; i < sheetNamesDisplayTable.Rows.Count; i++ ) {
                DataRow row = sheetNamesDisplayTable.Rows[ i ];
                bool add = (bool)row[ "ImportCol" ];
                if( add ) {
                    string name = (string)row[ "NameCol" ];
                    selSheets.Add( name );
                    if( addToMasterWorksheetTable ) {
                        string msName = (string)row[ "TypeCol" ];
                        currentProject.AddWorksheetInfo( name, ProjectSettings.InfoSource.WorksheetName, msName );
                    }
                }
            }
            string[] sheets = new string[ selSheets.Count ];
            selSheets.CopyTo( sheets );
            return sheets;
        }

        private ProjectSettings.DataType[] GetSelectedWorksheetTypes( DataTable sheetNamesDisplayTable ) {
            ArrayList selSheetTypes = new ArrayList();
            for( int i = 0; i < sheetNamesDisplayTable.Rows.Count; i++ ) {
                DataRow row = sheetNamesDisplayTable.Rows[ i ];
                bool add = (bool)row[ "ImportCol" ];
                if( add ) {
                    string impTypeStr = (string)row[ "TypeCol" ];
                    ProjectSettings.DataType type = GetTypeFor( impTypeStr );
                    selSheetTypes.Add( type );
                }
            }
            ProjectSettings.DataType[] types = new ProjectSettings.DataType[ selSheetTypes.Count ];
            selSheetTypes.CopyTo( types );
            return types;
        }

        private ProjectSettings.DataType GetTypeFor( string sheetName ) {
            switch( sheetName ) {
                case "Display":
                    return ProjectSettings.DataType.Display;
                case "Distribution":
                    return ProjectSettings.DataType.Distribution;
                case "Media":
                    return ProjectSettings.DataType.Media;
                //case "Coupons":
                //    return ProjectSettings.DataType.Coupons;
                case "Real Sales":
                    return ProjectSettings.DataType.RealSales;
                case "Price":
                    return ProjectSettings.DataType.Price;
                case "Price (Regular)":
                    return ProjectSettings.DataType.PriceRegular;
                case "Price (Promo)":
                    return ProjectSettings.DataType.PricePromo;
                case "Promo Price (% Promo)":
                    return ProjectSettings.DataType.PromoPricePct;
            }
            return ProjectSettings.DataType.Unknown;
        }

        private string[] GetUniquePartsOf( string[] paths ) {
            if( paths.Length < 2 ){
                return paths;
            }
            string hdr = paths[ 0 ]; 
            do {
                if( hdr.IndexOf( "\\" ) == -1 ) {
                    hdr = "";
                    break;
                }

                string tstHdr = hdr.Substring( 0, hdr.LastIndexOf( "\\" ) + 1);
                bool ok = true;
                for( int i = 2; i < paths.Length; i++ ) {
                    if( paths[ i ].StartsWith( tstHdr ) == false ) {
                        ok = false;
                        break;
                    }
                }
                if( ok ) {
                    hdr = tstHdr;
                    break;
                }
            } while( hdr.Length > 0 );

            string[] vals = new string[ paths.Length ];
            for( int i = 0; i < paths.Length; i++ ) {
                vals[ i ] = paths[ i ].Substring( hdr.Length );
            }
            return vals;
        }

        public void OpenReader( string path ) {
            if( File.Exists( path ) == false ) {
               string msg = String.Format( "\r\n     ERROR: Attempt to open non-existent file!    \r\n\r\n    File: {0}", path );
               MessageBox.Show( msg, "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
               return;
            }

            if( ScanManager.Reader != null && path == readerFile ) {
                // file is already open
                return;
            }
            readerFile = path;
            //////readerOwner.Cursor = Cursors.WaitCursor;

            CloseReader();

            ScanManager.Reader = new ExcelWriter2();
            ScanManager.Reader.Open( readerFile );

            //////readerOwner.Cursor = Cursors.Default;
        }

        public void CloseReader() {
            if( ScanManager.Reader != null ) {
                ScanManager.Reader.Kill();
                ScanManager.Reader = null;
            }
        }

        private void CreateWorksheetTable() {
            this.worksheetDisplayTable = new DataTable( "WorksheetTable" );
            worksheetDisplayTable.Columns.Add( "NameCol", typeof( string ) );
            worksheetDisplayTable.Columns.Add( "ImportCol", typeof( bool ) );
            worksheetDisplayTable.Columns.Add( "TypeCol", typeof( string ) );
        }

        private void AddWorksheetsToDisplayTable( bool checkAll ) {
            string[] sheetNames = ScanManager.Reader.GetSheetNames();

            if( sheetNames.Length == 0 ) {
                MessageBox.Show( "Error: No worksheets found in file.", "No Worksheets", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            Array.Sort( sheetNames );

            for( int i = 0; i < sheetNames.Length; i++ ) {
                DataRow drow = worksheetDisplayTable.NewRow();
                drow[ "NameCol" ] = sheetNames[ i ];
                drow[ "ImportCol" ] = false;
                drow[ "TypeCol" ] = "?";
                // see if we can determine the sheet data type
                string s = sheetNames[ i ].ToLower();
                foreach( string valType in knownSheetNames.Keys ) {
                    string[] vals = (string[])knownSheetNames[ valType ];
                    foreach( string testval in vals ) {
                        if( s == testval ) {
                            drow[ "TypeCol" ] = valType;
                            drow[ "ImportCol" ] = true;
                        }
                    }
                }
                if( checkAll ) {
                    drow[ "ImportCol" ] = true;
                }

                worksheetDisplayTable.Rows.Add( drow );
            }
        }

        ////public int ShowScanForm( int formNum ) {
        ////    int nextFormNum = -1;
        ////    DialogResult resp = DialogResult.None;
        ////    switch( formNum ) {
        ////        case 1:
        ////            SelectWorksheets selForm = new SelectWorksheets( fileSettings.SourcePath, worksheetDisplayTable );
        ////            resp = form2.ShowDialog();
        ////            if( resp == DialogResult.OK ) {
        ////                worksheetDisplayTable = form2.SheetNamesTable;
        ////                activeWorksheetName = form2.FirstSheetName;
        ////                activeWorksheetIndex = form2.FirstSheetIndex;
        ////                prefetchedWorksheetDataList = form2.PrefetchSheetDataList;
        ////            }
        ////            break;
        ////        case 3:
        ////            object[ , ] prefetchedWorksheetData = (object[ , ])prefetchedWorksheetDataList[ activeWorksheetName ];
        ////            WizardForm3 form3 = new WizardForm3( worksheetDisplayTable, fileSettings, activeWorksheetName, prefetchedWorksheetData );
        ////            resp = form3.ShowDialog();
        ////            break;
        ////        case 4:
        ////            int sheetCount = fileSettings.WorksheetSettings.Count;
        ////            WizardForm4 form4 = new WizardForm4( fileSettings.SourcePath, sheetCount, "???" );
        ////            resp = form4.ShowDialog();
        ////            break;
        ////    }

        ////    if( resp == DialogResult.OK ) {
        ////        nextFormNum = formNum + 1;

        ////        if( formNum == 3 ) {
        ////            nextFormNum = 3;
        ////            if( activeWorksheetIndex == worksheetDisplayTable.Rows.Count - 1 ) {
        ////                // did the last worksheet
        ////                nextFormNum = 4;
        ////            }
        ////            else {
        ////                // find the next
        ////                int nextIndx = -1;
        ////                for( int i = activeWorksheetIndex + 1; i < worksheetDisplayTable.Rows.Count; i++ ) {
        ////                    bool useSheet = (bool)worksheetDisplayTable.Rows[ i ][ "ImportCol" ];
        ////                    if( useSheet ) {
        ////                        nextIndx = i;
        ////                        break;
        ////                    }
        ////                }
        ////                if( nextIndx >= 0 ) {
        ////                    activeWorksheetIndex = nextIndx;
        ////                    activeWorksheetName = (string)worksheetDisplayTable.Rows[ activeWorksheetIndex ][ "NameCol" ];
        ////                }
        ////                else {
        ////                    // there were no more selected worksheets
        ////                    nextFormNum = 4;
        ////                }
        ////            }
        ////        }

        ////        // see if we are all done
        ////        if( formNum == 4 ) {
        ////            nextFormNum = -1;
        ////        }
        ////    }
        ////    else if( resp == DialogResult.Retry ) {
        ////        nextFormNum = formNum - 1;
        ////    }

        ////    // close the file reader if there is no next form
        ////    if( nextFormNum == -1 && reader != null ) {
        ////        reader.Kill();
        ////        reader = null;
        ////    }
        ////    return nextFormNum;
        ////}
    }
}
