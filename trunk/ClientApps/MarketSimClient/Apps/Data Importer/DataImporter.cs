//#define DEBUG_EARLY_EXIT
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;

using DataImporter.Dialogs;
using DataImporter.ImportSettings;
using DataImporter.Library;

namespace DataImporter
{
    public partial class DataImporter : Form
    {
        private ProjectSettings currentProject;

        private ScanManager scanManager;

        private ArrayList masterWorksheetsList;

        private const string title0Format = "MarketSim Data Importer";
        private const string titleFormat = "MarketSim Data Importer - {0}";

        public static string OutputFileExt = ".nrp";
        public static string MarketSimFileExt = ".xls";
        public static string InputFilePattern = "*.xls";
        public static string InputFileFilter = "Excel Files (*.xls)|*.xls";

        private TreeNode projectNode;
        private TreeNode brandsNode;
        private TreeNode channelsNode;
        private TreeNode filesNode;

        private Hashtable alternateWorksheetNames;

        //private MenuItem sectionContextMenuScanItem;

        public DataImporter() {
            InitializeComponent();
            currentProject = null;
            scanManager = new ScanManager( this );
            OutputManager.Init( Application.StartupPath );
            alternateWorksheetNames = new Hashtable();
        }

        private void newToolStripMenuItem_Click( object sender, EventArgs e ) {
            NewProject newProjectForm = new NewProject();
            DialogResult resp = newProjectForm.ShowDialog();
            if( resp == DialogResult.OK ) {
                currentProject = newProjectForm.ProjectSettings;
                UpdateProjectDisplay();
            }
        }

        private void UpdateProjectDisplay() {
            if( currentProject != null ) {
                SetTitle( currentProject.ProjectName );

                treeView.BeginUpdate();
                treeView.Nodes.Clear();
                projectNode = treeView.Nodes.Add( currentProject.ProjectName );
                if( currentProject.Description != "" ) {
                    projectNode.ToolTipText = currentProject.Description;
                    projectNode.Text = String.Format( "{0}  -- {1}", currentProject.ProjectName, currentProject.Description );
                }
                brandsNode = projectNode.Nodes.Add( "Brands - " + currentProject.Brands.Count.ToString() );
                channelsNode = projectNode.Nodes.Add( "Channels - " + currentProject.Channels.Count.ToString() );
                TreeNode n3 = projectNode.Nodes.Add( "Products - " + currentProject.Products.Count.ToString() );
                filesNode = projectNode.Nodes.Add( "Files - " + currentProject.AllImportFiles.Count.ToString() + " scanned" );

                int fileCount = 0;
                for( int i = 0; i < currentProject.ProjectSections.Count; i++ ) {
                    ProjectSettings.ProjectSection section = (ProjectSettings.ProjectSection)currentProject.ProjectSections[ i ];
                    string[] paths = section.GetFiles( currentProject );
                    string nodeName = null;
                    if( section.IsFileSet == false ) {
                        nodeName = "Section: " + section.SpecificFile;
                    }
                    else {
                        nodeName = "Section: " + section.FleSetFolder;
                        nodeName += String.Format( " ({0} files)", section.FileCount );
                    }
                    TreeNode sectionNode = filesNode.Nodes.Add( nodeName );
                    sectionNode.ContextMenu = new ContextMenu();
                    MenuItem mitem = null;
                    if( section.Scanned == false ) {
                        mitem = new MenuItem( "Scan Section Format...", SectionContextMenuScan );
                    }
                    else {
                        mitem = new MenuItem( "Re-Scan Section Format...", SectionContextMenuScan );
                    }
                    mitem.Tag = sectionNode;
                    sectionNode.ContextMenu.MenuItems.Add( mitem );

                    MenuItem mitem2 = new MenuItem( "Properties", ItemContextMenuProperties );
                    mitem2.Tag = sectionNode;
                    sectionNode.ContextMenu.MenuItems.Add( mitem2  );

                    // update the valid flag
                    bool sectionValid = true;
                    for( int w = 0; w < section.WorksheetSettingsList.Count; w++ ) {
                        WorksheetSettings ws = (WorksheetSettings)section.WorksheetSettingsList[ w ];
                        if( ws.Validated == false ) {
                            sectionValid = false;
                        }
                    }
                    section.Valid = sectionValid;

                    if( section.Scanned == false ) {
                        sectionNode.ForeColor = Color.DarkOrange;
                    }
                    else if( sectionValid == false ) {
                        sectionNode.ForeColor = Color.Blue;
                    }
                    else {
                        sectionNode.ForeColor = Color.Green;
                    }

                    sectionNode.Tag = section;
                    section.FileCount = paths.Length;
                    fileCount += section.FileCount;

                    TreeNode sectionWorksheetsNode = sectionNode.Nodes.Add( "Worksheets" );

                    for( int w = 0; w < section.WorksheetSettingsList.Count; w++ ) {
                        WorksheetSettings ws = (WorksheetSettings)section.WorksheetSettingsList[ w ];
                        TreeNode wsNode = sectionWorksheetsNode.Nodes.Add( ws.SheetName );
                        wsNode.Tag = ws;

                        wsNode.ContextMenu = new ContextMenu();
                        MenuItem mitem3 = new MenuItem( "Properties", ItemContextMenuProperties );
                        mitem3.Tag = wsNode;
                        wsNode.ContextMenu.MenuItems.Add( mitem3 );
                    }
                    sectionWorksheetsNode.Expand();
                }

                filesNode.Text = "Files - " + fileCount + " detected";

                projectNode.Expand();
                filesNode.Expand();
                treeView.EndUpdate();
            }
            else {
                // there is no  current project
                SetTitle( null );
                treeView.BeginUpdate();
                treeView.Nodes.Clear();
                treeView.EndUpdate();
            }
        }

        private void SectionContextMenuScan( object sender, EventArgs e ) {
            MenuItem mitem = sender as MenuItem;
            if( mitem != null ) {
                TreeNode node = (TreeNode)mitem.Tag;
                if( node.IsSelected == false ) {
                    treeView.SelectedNode = node;
                }
                scanToolStripMenuItem1_Click( sender, e );
            }
        }

        private void ItemContextMenuProperties( object sender, EventArgs e ) {
            MenuItem mitem = sender as MenuItem;
            if( mitem != null ) {
                TreeNode node = (TreeNode)mitem.Tag;
                if( node.IsSelected == false ) {
                    treeView.SelectedNode = node;
                }
                propertiesToolStripMenuItem_Click( sender, e );
            }
        }

        private void SetTitle( string projName ) {
            if( projName != null ) {
                this.Text = String.Format( titleFormat, projName );
            }
            else {
                this.Text = title0Format;
            }
        }

        private void saveAsToolStripMenuItem_Click( object sender, EventArgs e ) {
            string curFile = currentProject.ProjectFile;
            string curDir = "";
            if( currentProject.ProjectFile.IndexOf( "\\" ) != -1 ) {
                curFile = currentProject.ProjectFile.Substring( currentProject.ProjectFile.LastIndexOf( "\\" ) + 1 );
                curDir = currentProject.ProjectFile.Substring( 0, currentProject.ProjectFile.LastIndexOf( "\\" ) + 1 );
            }
            SaveProjectAsForm sForm = new SaveProjectAsForm( currentProject.ProjectName, curFile, currentProject.NormalizePrices );

            DialogResult resp = sForm.ShowDialog();
            if( resp == DialogResult.OK ) {
                currentProject.ProjectName = sForm.ProjectName;
                currentProject.ProjectFile = curDir + sForm.ProjectFile;
                currentProject.NormalizePrices = sForm.ProjectPricesNormalized;

                ConverAlternateSheetNamesHashtableToLists();
                currentProject.Save();
            }
        }

        private void openImportProjectToolStripMenuItem_Click( object sender, EventArgs e ) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Import Project";
            ofd.Filter = "MarketSim Import Projects (*.nrp)|*.nrp";
            DialogResult resp = ofd.ShowDialog();
            if( resp == DialogResult.OK ) {

                if( currentProject != null && currentProject.isEdited() ) {
                    string msg = String.Format( "Save changes to {0}?", currentProject.ProjectName );
                    DialogResult resp2 = MessageBox.Show( msg, "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
                    if( resp2 == DialogResult.Yes ) {
                        saveToolStripMenuItem_Click( sender, e );
                    }
                    else {
                        return;
                    }
                }

                currentProject = ProjectSettings.LoadProjectSettings( ofd.FileName );
                SetAlternateSheetNamesHashtable();
                if( currentProject != null ) {
                    currentProject.ProjectFile = ofd.FileName;
                }
                SetWorksheetProjectReferences();
                UpdateProjectDisplay();
            }
        }

        private void saveToolStripMenuItem_Click( object sender, EventArgs e ) {
            if( currentProject.ProjectFile == null ) {
                // do a "save as" if the file name hasn't been set yet
                saveAsToolStripMenuItem_Click( sender, e );
            }
            else {
                ConverAlternateSheetNamesHashtableToLists();
                currentProject.Save();
            }
        }

        private void closeToolStripMenuItem_Click( object sender, EventArgs e ) {
            bool okToClose = true;
            if( currentProject != null && currentProject.isEdited() ) {
                string msg = String.Format( "Save changes to {0}?", currentProject.ProjectName );
                DialogResult resp = MessageBox.Show( msg, "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
                if( resp == DialogResult.Yes ) {
                    saveToolStripMenuItem_Click( sender, e );
                }

                if( resp == DialogResult.Cancel ) {
                    okToClose = false;
                }
            }

            if( okToClose ) {
                currentProject = null;
                UpdateProjectDisplay();
            }
        }

        private bool closeChecked = false;

        private void exitToolStripMenuItem_Click( object sender, EventArgs e ) {
            bool okToClose = true;
            if( currentProject != null && currentProject.isEdited() ) {
                string msg = String.Format( "Save changes to {0}?", currentProject.ProjectName );
                DialogResult resp = MessageBox.Show( msg, "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
                if( resp == DialogResult.Yes ) {
                    saveToolStripMenuItem_Click( sender, e );
                }

                if( resp == DialogResult.Cancel ) {
                    okToClose = false;
                }
            }

            if( okToClose ) {
                closeChecked = true;
                this.Close();
            }
        }

        private void DataImporter_FormClosing( object sender, FormClosingEventArgs e ) {

            scanManager.CloseReader();  // prevent rogue Excel processes

            if( closeChecked == false && currentProject != null && currentProject.isEdited() ) {
                 string msg = String.Format( "Save changes to {0}?", currentProject.ProjectName );
                DialogResult resp = MessageBox.Show( msg, "Save Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
                if( resp == DialogResult.Yes ) {
                    ConverAlternateSheetNamesHashtableToLists();
                    currentProject.Save();
                }
            }
        }

        private void fileGroupToolStripMenuItem_Click( object sender, EventArgs e ) {
            if( currentProject == null ) {
                return;
            }

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select folder that contains the file set";
            fbd.SelectedPath = currentProject.InputRootDirectory;
            DialogResult resp = fbd.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            if( fbd.SelectedPath.StartsWith( currentProject.InputRootDirectory ) == false ) {
                string msg = String.Format( "Error: The data folder must be  a subfolder of the project input folder, or the same as the input folder.  Input Folder:\r\n\r\n{0}",
                    currentProject.InputRootDirectory );
                MessageBox.Show( msg, "Illegal File Location", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            string relFilePath = "";
            if( fbd.SelectedPath.Length > currentProject.InputRootDirectory.Length ){
                relFilePath = fbd.SelectedPath.Substring( currentProject.InputRootDirectory.Length + 1 );
            }

            AddFolderForm addFolderDlg = new AddFolderForm( relFilePath );
            DialogResult resp2 = addFolderDlg.ShowDialog();
            if( resp2 != DialogResult.OK ) {
                return;
            }

            currentProject.ProjectSections.Add( addFolderDlg.ProjectSection );
            UpdateProjectDisplay();
        }

        private void individualFileToolStripMenuItem_Click( object sender, EventArgs e ) {
            if( currentProject == null ){
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select File to Add";
            ofd.Filter = InputFileFilter;
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = currentProject.InputRootDirectory;
            DialogResult resp = ofd.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            if( ofd.FileName.StartsWith( currentProject.InputRootDirectory + "\\" ) == false ) {
                string msg = String.Format( "Error: The data file must be in the project input folder (or a subfolder).  Input Folder:\r\n\r\n{0}", 
                    currentProject.InputRootDirectory );
                MessageBox.Show( msg, "Illegal File Location", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            string relFilePath = ofd.FileName.Substring( currentProject.InputRootDirectory.Length + 1 );
            AddFileForm addFilerDlg = new AddFileForm( relFilePath );
            DialogResult resp2 = addFilerDlg.ShowDialog();
            if( resp2 != DialogResult.OK ) {
                return;
            }

            currentProject.ProjectSections.Add( addFilerDlg.ProjectSection );
            UpdateProjectDisplay();
        }

        private void scanAllToolStripMenuItem_Click( object sender, EventArgs e ) {
             if( currentProject == null ){
                return;
            }

            bool scannedAlready = false;
            foreach( ProjectSettings.ProjectSection sec in currentProject.ProjectSections ) {
                if( sec.Scanned == true ) {
                    scannedAlready = true;
                    break;
                }
            }

            if( scannedAlready == true ) {
                DialogResult resp = MessageBox.Show( "This will regenerate the lists of brands, channels, and variants.  \r\nAll existing settings will be replaced.\r\n\r\nProceed?",
                    "Confirm Scan All", MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
                if( resp != DialogResult.OK ) {
                    return;
                }
            }

            scanManager.ScanAllSections( currentProject );
            UpdateProjectDisplay();
        }

        private void propertiesToolStripMenuItem_Click( object sender, EventArgs e ) {
            TreeNode selectedNode = treeView.SelectedNode;
            if( selectedNode == null ) {
                return;
            }

            if( selectedNode.Parent == filesNode ) {
                // this is a section node
                ProjectSettings.ProjectSection projSection = (ProjectSettings.ProjectSection)selectedNode.Tag;
                ShowSectionProperties( projSection );
                return;
            }
            else if( selectedNode == projectNode ) {
                // this is the project node
                ShowProjectProperties( currentProject );
                return;
            }
            else if( selectedNode == brandsNode ) {
                // this is the brands node
                ShowBrandsProperties( currentProject );
                return;
            }
            else if( selectedNode == channelsNode ) {
                // this is the channels node
                ShowChannelsProperties( currentProject );
                return;
            }
            else if( selectedNode.Parent != null && selectedNode.Parent.Text == "Worksheets" ) {
                // this is a worksheet node
                ShowWorksheetSettingsProperties( (WorksheetSettings)selectedNode.Tag );
                return;
            }
        }

        private void ShowProjectProperties( ProjectSettings proj ) {
            string projectProperties = proj.PropertiesString();
            MessageBox.Show( projectProperties, "Properties", MessageBoxButtons.OK );
        }

        private void ShowBrandsProperties( ProjectSettings proj ) {
            string props = "\r\nBrands in Project\r\n\r\n-------------------------------\r\n\r\n";
            foreach( ProjectSettings.BrandInfo info in proj.Brands ) {
                props += String.Format( "{0} - {1}\r\n", info.ImportName, info.Source );
            }
            props += "\r\n";
            MessageBox.Show( props, "Properties", MessageBoxButtons.OK );
        }

        private void ShowChannelsProperties( ProjectSettings proj ) {
            string props = "\r\nChannels in Project\r\n\r\n-------------------------------\r\n\r\n";
            foreach( ProjectSettings.ChannelInfo info in proj.Channels ) {
                props += String.Format( "{0} - {1}\r\n", info.ImportName, info.Source );
            }
            props += "\r\n";
            MessageBox.Show( props, "Properties", MessageBoxButtons.OK );
        }

        private void ShowSectionProperties( ProjectSettings.ProjectSection section ) {
            string sectionProperties = section.PropertiesString();
            MessageBox.Show( sectionProperties, "Properties", MessageBoxButtons.OK );
        }

        private void ShowWorksheetSettingsProperties( WorksheetSettings wsSettings ) {
            string wsProperties = wsSettings.PropertiesString();
            MessageBox.Show( wsProperties, "Properties", MessageBoxButtons.OK );
        }

        private void scanToolStripMenuItem1_Click( object sender, EventArgs e ) {
            TreeNode selectedNode = treeView.SelectedNode;
            if( selectedNode == null || selectedNode.Parent != null && selectedNode.Parent != filesNode ) {
                return;
            }
            // this is a section node
            ProjectSettings.ProjectSection projSection = (ProjectSettings.ProjectSection)selectedNode.Tag;

            if( projSection.Scanned == true ) {
                DialogResult resp = MessageBox.Show( "Are you sure you want to re-scan this Project Section? \r\nExisting settings will be lost.\r\n\r\nProceed?",
                    "Confirm Scan", MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
                if( resp != DialogResult.OK ) {
                    return;
                }
            }

            scanManager.ScanSection( currentProject, projSection );
            UpdateProjectDisplay();
        }

        private string OutputFileFor( string inputFile, ProjectSettings.DataType dataType ) {
            string opath = currentProject.OutputDirectory + "\\";
            switch( dataType ) {
                case ProjectSettings.DataType.Display:
                    opath += "Display\\";
                    break;
                case ProjectSettings.DataType.Distribution:
                    opath += "Distribution\\";
                    break;
                case ProjectSettings.DataType.Media:
                    opath += "Media\\";
                    break;
                //case ProjectSettings.DataType.Price:
                case ProjectSettings.DataType.PriceAbsolute:
                case ProjectSettings.DataType.PricePromo:
                case ProjectSettings.DataType.PriceRegular:
                case ProjectSettings.DataType.PromoPricePct:
                case ProjectSettings.DataType.AbsolutePricePct:
                    opath += "Price\\";
                    break;
                case ProjectSettings.DataType.RealSales:
                    opath += "Real Sales\\";
                    break;
                case ProjectSettings.DataType.Unknown:
                    opath += "Unknown\\";
                    break;
            }
            opath += inputFile;
            return opath;
        }

        private void importDataToolStripMenuItem_Click( object sender, EventArgs e ) {
            ImportAll();
        }

        private void GenerateMasterWorksheetsList() {
            GenerateMasterWorksheetsList( null, null );
        }

        private void GenerateMasterWorksheetsList( string[] types, string[] channels ) {
            masterWorksheetsList = new ArrayList();
            //Console.WriteLine( "\nImport Data:" );

 #if DEBUG_EARLY_EXIT
           int debugEarlyExit = 6;
#endif

            // import each section
            for( int s = 0; s < currentProject.SectionCount; s++ ) {
                ProjectSettings.ProjectSection section = currentProject.GetSection( s );
                //Console.WriteLine( "\n  Section: " + section.SectionPath );
                section.WorksheetVariantsSet = false;
                section.WorksheetVariantsValid = true;
                section.WorksheetDatesSet = false;
                section.WorksheetDatesValid = true;

                string[] sectionFiles = section.GetFiles( currentProject );
                foreach( string path in sectionFiles ) {

#if DEBUG_EARLY_EXIT
                    if( path.IndexOf( "Meguricha" ) != -1 ){
                        continue;
                    }
#endif
                    //Console.WriteLine( "\n    File: " + path + "\n" );

                    foreach( WorksheetSettings worksheetSettingsSample in section.WorksheetSettingsList ) {
                        WorksheetSettings worksheetSettings = new WorksheetSettings( worksheetSettingsSample );

                        string dir = path.Substring( 0, path.LastIndexOf( "\\" ) );
                        string file = path.Substring( path.LastIndexOf( "\\" ) + 1 );
                        if( section.BrandSource == ProjectSettings.InfoSource.DirectoryName ) {
                            worksheetSettings.Brand = currentProject.GetBrand( dir, section.BrandSource, true );
                        }
                        else if( section.BrandSource == ProjectSettings.InfoSource.FileName ) {
                            worksheetSettings.Brand = currentProject.GetBrand( path, section.BrandSource, true );
                        }
                        else if( section.BrandSource == ProjectSettings.InfoSource.WorksheetName) {
                            worksheetSettings.Brand = currentProject.GetBrand( worksheetSettings.Brand.ImportName, ProjectSettings.InfoSource.WorksheetName, false );
                        }

                        if( section.ChannelSource == ProjectSettings.InfoSource.DirectoryName ) {
                            worksheetSettings.Channel = currentProject.GetChannel( dir, section.ChannelSource, true );
                        }
                        else if( section.ChannelSource == ProjectSettings.InfoSource.FileName ) {
                            worksheetSettings.Channel = currentProject.GetChannel( path, section.ChannelSource, true );
                        }
                        else if( section.ChannelSource == ProjectSettings.InfoSource.UserSpecified ) {
                            worksheetSettings.Channel = currentProject.GetChannel( section.SpecificChannel, section.ChannelSource, true ); 
                        }

                        if( worksheetSettings.DataType == ProjectSettings.DataType.Unknown ) {    // leave data type if it is set already
                            if( section.DataTypeSource == ProjectSettings.InfoSource.WorksheetName ) {
                                worksheetSettings.DataType = currentProject.GetWorksheetType( worksheetSettings.SheetName );
                            }
                        }   

                        worksheetSettings.InputFile = path;

                        if( worksheetSettings.Channel == null && worksheetSettings.Brand == null ) {
                            // this worksheet has the brand and channel information in the data (brand may be implied by variant)
                            file = "All" + MarketSimFileExt;
                        }
                        else {
                            if( worksheetSettings.Channel.MarketSimName == "All" ) {
                                file = worksheetSettings.Brand.MarketSimName + MarketSimFileExt;
                            }
                            else {
                                file = worksheetSettings.Brand.MarketSimName + " - " + worksheetSettings.Channel.MarketSimName + MarketSimFileExt;
                            }
                        }

                        worksheetSettings.OutputFile = OutputFileFor( file, worksheetSettings.DataType );
                        worksheetSettings.SetSection( section );

                        bool selectedType = true;
                        if( types != null ) {
                            selectedType = false;
                            foreach( string type in types ) {
                                if( type == worksheetSettings.DataType.ToString() ) {
                                    selectedType = true;
                                    break;
                                }
                                
                            }
                        }

                        bool selectedChannel = true;
                        if( channels != null ) {
                            selectedChannel = false;
                            foreach( string channel in channels ) {
                                if( channel == worksheetSettings.Channel.MarketSimName ) {
                                    selectedChannel = true;
                                    break;
                                }
                            }
                        }

                        if( selectedType == false || selectedChannel == false ) {
                            // skip this item since it isn't a selected kind
                            continue;
                        }
                        masterWorksheetsList.Add( worksheetSettings );


#if DEBUG_EARLY_EXIT
                        if( debugEarlyExit-- <= 0 ) {
                            Console.WriteLine( "TAKING EARLY EXYT!!!" );
                            return;
                        }
#endif

                        //Console.WriteLine( String.Format( "      Worksheet: Type {0}, Sheet Name \"{1}\",", worksheetSettings.DataType, worksheetSettings.SheetName ) );
                        //if( worksheetSettings.Brand != null ) {
                        //    Console.WriteLine( String.Format( "        Brand \"{0}\" - {1},", worksheetSettings.Brand.ImportName, worksheetSettings.Brand.Source ) );
                        //}
                        //else {
                        //    Console.WriteLine( "        Brand: NULL," );
                        //}
                        //if( worksheetSettings.Channel != null ) {
                        //    Console.WriteLine( String.Format( "        Channel \"{0}\" - {1}", worksheetSettings.Channel.ImportName, worksheetSettings.Channel.Source ) );
                        //}
                        //else {
                        //    Console.WriteLine( "    Channel: NULL" );
                        //}
                        //Console.WriteLine( "Output File: {0}\n", worksheetSettings.OutputFile );
                    }
                }
            }

        }

        private void SetWorksheetProjectReferences(){
            for( int s = 0; s < currentProject.SectionCount; s++ ) {
                ProjectSettings.ProjectSection section = currentProject.GetSection( s );
                foreach( WorksheetSettings worksheetSettings in section.WorksheetSettingsList ) {
                    // NB: We can't just add the section as a regular property of the worksheetSettings since the serializer complains 
                    //    about a circular reference even if it is given a [NonSerialized] attribute.
                    worksheetSettings.SetSection( section );   
                }
            }
        }

        private void ImportAll() {
            if( ValidateAllWorksheets( true ) ) {
                UpdateProjectDisplay();
                MessageBox.Show( "\n\n      Import Operation Completed    \n\n", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
        }

        private void WriteAllOutputFiles() {

            // bring the current project up to data (we may need to access data from multiple worksheets for outputs like promo/unpromo price)
            for( int s = 0; s < currentProject.SectionCount; s++ ) {
                currentProject.GetSection( s ).WorksheetSettingsList.Clear();
                for( int w = 0; w < masterWorksheetsList.Count; w++ ) {
                    WorksheetSettings worksheetSettings = (WorksheetSettings)masterWorksheetsList[ w ];
                    currentProject.GetSection( s ).WorksheetSettingsList.Add( worksheetSettings );
                }
            }

            for( int w = 0; w < masterWorksheetsList.Count; w++ ) {
                WorksheetSettings worksheetSettings = (WorksheetSettings)masterWorksheetsList[ w ];
                OutputManager.WriteOutputFile( worksheetSettings, currentProject );
            }
        }

        private bool ValidateAllWorksheets( bool loadDataToo ) {
            ArrayList chans = new ArrayList();
            foreach( ProjectSettings.ChannelInfo cinfo in currentProject.Channels ) {
                if( chans.Contains( cinfo.MarketSimName ) == false ) {
                    chans.Add( cinfo.MarketSimName );
                }
            }

            StartImportForm impForm = new StartImportForm( currentProject.StartImportDate, currentProject.EndImportDate,
                currentProject.ImportCompressionDeltaTolerance, currentProject.ImportDataPriceScaling, chans, !loadDataToo );
            DialogResult resp = impForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return false;
            }
            if( currentProject.StartImportDate != impForm.StartDate ) {
                currentProject.StartImportDate = impForm.StartDate;
                currentProject.SetEdited();
            }
            if( currentProject.EndImportDate != impForm.EndDate ) {
                currentProject.EndImportDate = impForm.EndDate;
                currentProject.SetEdited();
            }
            if( currentProject.ImportCompressionDeltaTolerance != impForm.Tolerance ) {
                currentProject.ImportCompressionDeltaTolerance = impForm.Tolerance;
                currentProject.SetEdited();
            }
            if( currentProject.ImportDataPriceScaling != impForm.Scaling ) {
                currentProject.ImportDataPriceScaling = impForm.Scaling;
                currentProject.SetEdited();
            }

            GenerateMasterWorksheetsList( impForm.DataTypes, impForm.Channels );

            // kick off the processing
            ProcessWorksheet( 0, loadDataToo, masterWorksheetsList );

            ////ProcessWorksheetDelegate pwDelegate = new ProcessWorksheetDelegate( ProcessWorksheet );
            //////!!! need to solve cross-thread issues to use (actually to reset) the wait cursor...not sure why it is okay to set toolstrip text, but not cursor
            //////this.Cursor = Cursors.WaitCursor;
            ////IAsyncResult result = pwDelegate.BeginInvoke( 0, loadDataToo, masterWorksheetsList, null, null );
            ////Console.WriteLine( "..... First worksheet being processed (count = {0}.................", masterWorksheetsList.Count );
            ////toolStripStatusLabel1.Text = String.Format( "Processing sheet 1 of {0}...", masterWorksheetsList.Count );
            //////Thread.Sleep( 0 );
            //////int threadid;
            //////pwDelegate.EndInvoke( result );
            //////this.Cursor = Cursors.Default;
            return true;
        }

        private void ProcessNextWorksheet( int w, bool loadDataToo ) {
            if( w < this.masterWorksheetsList.Count - 1 ) {
                w += 1;
                Console.WriteLine( "..... {0} of {1} worksheets being processed .................", w + 1, masterWorksheetsList.Count );
                ProcessWorksheet( w, loadDataToo, masterWorksheetsList );

                //////toolStripStatusLabel1.Text = String.Format( "Processing sheet {0} of {1}...", w + 1, masterWorksheetsList.Count );
                //////ProcessWorksheetDelegate pwDelegate = new ProcessWorksheetDelegate( ProcessWorksheet );
                //////pwDelegate.BeginInvoke( w, loadDataToo, masterWorksheetsList, null, null );
            }
            else {

                if( loadDataToo ) {
                    Console.WriteLine( "..... writing output data files ................." );
                    WriteAllOutputFiles();
                }
                Console.WriteLine( "..... Finished processing worksheets!! ................." );

                toolStripStatusLabel1.Text = "";
                CloseWorksheets();
                //this.Cursor = Cursors.Default;
            }
        }

        //////private delegate void ProcessWorksheetDelegate( int w, bool loadDataToo, ArrayList worksheetsList );
        //////private delegate void WorksheetProcessDoneDelegate( int w, bool loadDataToo );

        private void ProcessWorksheet( int w, bool loadDataToo, ArrayList worksheetsList ) {
            WorksheetSettings worksheetSettings = (WorksheetSettings)worksheetsList[ w ];
            worksheetSettings.Validated = false;
            OpenWorksheet( worksheetSettings );

            // check the dates in all of the fiiles
            if( worksheetSettings.GetSection().WorksheetDatesSet == false ) {
                bool firstSheetDatesValid = ScanAllWorksheetDates( worksheetSettings );
                if( firstSheetDatesValid == false ) {
                    worksheetSettings.GetSection().WorksheetDatesValid = false;
                    string msg = String.Format( "    Error: Bad date(s) found while validating worksheet.    \r\n    Worksheet: {0}", worksheetSettings.SheetName );
                    MessageBox.Show( msg, "Invalid Date Specification", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
            }
            else {
                bool sheetDatesValid = VerifyAllWorksheetDates( worksheetSettings );
                if( sheetDatesValid == false ) {
                    worksheetSettings.GetSection().WorksheetDatesValid = false;
                    string msg = String.Format( "    Error: Bad date(s) found while validating worksheet (sheet does not match section).    \r\n    Worksheet: {0}", worksheetSettings.SheetName );
                    MessageBox.Show( msg, "Invalid Date Specification", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
            }

            if( worksheetSettings.GetSection().WorksheetVariantsSet == false ) {
                bool firstSheetVariantsValid = ScanAllWorksheetVariants( worksheetSettings, currentProject );
                if( firstSheetVariantsValid == false ) {
                    worksheetSettings.GetSection().WorksheetVariantsValid = false;
                    string msg = String.Format( "    Error: Bad variant(s) found while validating worksheet.    \r\n    Worksheet: {0}", worksheetSettings.SheetName );
                    MessageBox.Show( msg, "Invalid Variant Specification", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
            }
            else {
                if( (worksheetSettings.GetSection().BrandSource != ProjectSettings.InfoSource.DirectoryName) &&
                    (worksheetSettings.GetSection().BrandSource != ProjectSettings.InfoSource.FileName) ) {
                    bool sheetVariantsValid = VerifyAllWorksheetVariants( worksheetSettings );
                    if( sheetVariantsValid == false ) {
                        worksheetSettings.GetSection().WorksheetVariantsValid = false;
                        string msg = String.Format( "    Error: Bad variant(s) found while validating worksheet (sheet does not match section).    \r\n    Worksheet: {0}", worksheetSettings.SheetName );
                        MessageBox.Show( msg, "Invalid Variant Specification", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        return;
                    }
                }
                else {
                    // if the brand source is the directory name or file name, then there can be different variants in different files!
                }
            }

            int variantCount = GetVariantCount( worksheetSettings );
            worksheetSettings.VariantCount = variantCount;

            if( worksheetSettings.GetSection().ChannelSource == ProjectSettings.InfoSource.FileContents ) {
                if( worksheetSettings.GetSection().WorksheetChannelsSet == false ) {
                    bool firstSheetChansValid = ScanAllWorksheetChannels( worksheetSettings );
                    if( firstSheetChansValid == false ) {
                        worksheetSettings.GetSection().WorksheetChannelsValid = false;
                        string msg = String.Format( "    Error: Bad channel(s) found while validating worksheet.    \r\n    Worksheet: {0}", worksheetSettings.SheetName );
                        MessageBox.Show( msg, "Invalid Channel Specification", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        return;
                    }
                    else {
                        for( int c = 0; c < worksheetSettings.GetSection().Channels.Count; c++ ) {
                            currentProject.AddChannel( (string)worksheetSettings.GetSection().Channels[ c ], worksheetSettings.GetSection().ChannelSource );
                        }
                    }
                }
                else {
                    bool firstSheetChansValid = VerifyAllWorksheetChannels( worksheetSettings );
                    if( firstSheetChansValid == false ) {
                        worksheetSettings.GetSection().WorksheetChannelsValid = false;
                        string msg = String.Format( "    Error: Bad channel(s) found while validating worksheet (sheet does not match section).    \r\n    Worksheet: {0}", worksheetSettings.SheetName );
                        MessageBox.Show( msg, "Invalid Channel Specification", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        return;
                    }
                }
            }
            ProjectSettings.ProjectSection section = worksheetSettings.GetSection();
            int channelCount = 1;
            if( section.Channels != null ) {
                channelCount = GetChannelCount( worksheetSettings );
            }
            if( section.ChannelSource == ProjectSettings.InfoSource.UserSpecified && channelCount == 0 ) {
                channelCount = 1;
            }

            ////if( (section.BrandSource == ProjectSettings.InfoSource.DirectoryName) ||
            ////    (section.BrandSource == ProjectSettings.InfoSource.FileName) ) {

            for( int i = 0; i < variantCount; i++ ) {
                string variantId = null;
                if( worksheetSettings.Variants != null ) {
                    variantId = worksheetSettings.Brand.ImportName + " - " + (string)worksheetSettings.Variants[ i ];
                }
                else {
                    variantId = worksheetSettings.Brand.ImportName + " - " + (string)section.Variants[ i ];
                }
                currentProject.AddProduct( variantId );
                currentProject.AddMediaItem( variantId );
            }

            ////}
            worksheetSettings.Validated = true;

            if( loadDataToo ) {
                ReadWorksheetData( worksheetSettings, variantCount, channelCount );

                ////!!! for function with PRICE data we will need to implement a mechanism to defer output until we have all data from this sheet!!!!!
                ////OutputManager.WriteOutputFile( worksheetSettings, currentProject );
            }

            ProcessNextWorksheet( w, loadDataToo );
            //WorksheetProcessDoneDelegate pwnDelegate = new WorksheetProcessDoneDelegate( ProcessNextWorksheet );
            //pwnDelegate.BeginInvoke( w, loadDataToo, null, null );
        }

        private bool OpenWorksheet( WorksheetSettings worksheetSettings ) {
            scanManager.OpenReader( worksheetSettings.InputFile );
            string[] fileWorksheets = ScanManager.Reader.GetSheetNames();

            ArrayList otherKnownNames = (ArrayList)alternateWorksheetNames[ worksheetSettings.SheetName ];

            foreach( string sheet in fileWorksheets ) {
                if( worksheetSettings.SheetName == sheet ) {
                    // the desired sheet is indeed in the file
                    Console.WriteLine( "OpenWorksheet() setting sheet name. Name = \"{0}\"", sheet );
                    ScanManager.Reader.SetSheet( sheet );
                    return true;
                }
                // also try alternate names, if any
                if( otherKnownNames != null ) {
                    foreach( string otherName in otherKnownNames ) {
                        if( otherName == sheet ) {
                            // the desired sheet is indeed in the file, with an alternate name
                            Console.WriteLine( "OpenWorksheet() using alternate sheet name! Name = \"{0}\"", sheet );
                            ScanManager.Reader.SetSheet( sheet );
                            return true;
                        }
                    }
                }
            }
            // we didn't find the named worksheet!
            AlternateWorksheetNameForm altForm = new AlternateWorksheetNameForm( worksheetSettings, fileWorksheets );
            DialogResult resp = altForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return false;
            }
            // assign the alternate name
            if( alternateWorksheetNames.ContainsKey( worksheetSettings.SheetName ) == false ) {
                ArrayList altNames = new ArrayList();
                altNames.Add( altForm.SelectedWorksheetName );
                alternateWorksheetNames.Add( worksheetSettings.SheetName, altNames );
            }
            else {
                ArrayList altNames = (ArrayList)alternateWorksheetNames[ worksheetSettings.SheetName ];
                altNames.Add( altForm.SelectedWorksheetName );
            }
            // set the sheet
            ScanManager.Reader.SetSheet( altForm.SelectedWorksheetName );
            return true;
        }

        private void CloseWorksheets(){
            scanManager.CloseReader();
        }

        private bool ScanAllWorksheetDates( WorksheetSettings worksheetSettings ) {
            return scanManager.ScanAllWorksheetDates( worksheetSettings );
        }

        private bool VerifyAllWorksheetDates( WorksheetSettings worksheetSettings ) {
            return scanManager.VerifyAllWorksheetDates( worksheetSettings );
        }

        private bool ScanAllWorksheetChannels( WorksheetSettings worksheetSettings ) {
            return scanManager.ScanAllWorksheetChannels( worksheetSettings );
        }

        private bool VerifyAllWorksheetChannels( WorksheetSettings worksheetSettings ) {
            return scanManager.VerifyAllWorksheetChannels( worksheetSettings );
        }

        private bool ScanAllWorksheetVariants( WorksheetSettings worksheetSettings, ProjectSettings projectSettings ) {
            return scanManager.ScanAllWorksheetVariants( worksheetSettings, projectSettings );
        }

        private bool VerifyAllWorksheetVariants( WorksheetSettings worksheetSettings ) {
            return scanManager.VerifyAllWorksheetVariants( worksheetSettings, currentProject );
        }

        private int GetVariantCount( WorksheetSettings worksheetSettings ) {
            return scanManager.GetVariantCount( worksheetSettings );
        }

        private int GetChannelCount( WorksheetSettings worksheetSettings ) {
            return scanManager.GetChannelCount( worksheetSettings );
        }

        private bool ReadWorksheetData( WorksheetSettings worksheetSettings, int variantCount, int channelCount ) {
            return scanManager.ReadWorksheetData( worksheetSettings, variantCount, channelCount, currentProject.ImportCompressionDeltaTolerance );
        }

        private void masterWorksheetsListToolStripMenuItem_Click( object sender, EventArgs e ) {
            GenerateMasterWorksheetsList();
            StreamWriter sw = new StreamWriter( "C:\\Documents and Settings\\jim\\My Documents\\DecisionPower\\Dentsu Project\\testdat.txt" );

            for( int i = 0; i < masterWorksheetsList.Count; i++ ) {
                WorksheetSettings ws = (WorksheetSettings)masterWorksheetsList[ i ];

                sw.WriteLine( String.Format( "\r\n      Worksheet: Type {0}, Sheet Name \"{1}\",", ws.DataType, ws.SheetName ) );
                sw.WriteLine( "Input File: {0}\r\n", ws.InputFile );
                if( ws.Brand != null ) {
                    sw.WriteLine( String.Format( "        Brand \"{0}\" - {1} --> \"{2}\",", ws.Brand.ImportName, ws.Brand.Source, ws.Brand.MarketSimName ) );
                }
                else {
                    sw.WriteLine( "        Brand: NULL," );
                }
                if( ws.Channel != null ) {
                    sw.WriteLine( String.Format( "        Channel \"{0}\" - {1} --> \"{2}\"", ws.Channel.ImportName, ws.Channel.Source, ws.Channel.MarketSimName ) );
                }
                else {
                    sw.WriteLine( "    Channel: NULL" );
                }
                sw.WriteLine( "Output File: {0}\r\n", ws.OutputFile );
            }
            sw.Flush();
            sw.Close();

            MessageBox.Show( "Master Worksheets List Written to testdat.txt", "Done", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
        }

        private void brandsToolStripMenuItem_Click( object sender, EventArgs e ) {
            NameMappingForm mappingForm = new NameMappingForm( currentProject.Brands, "Set MarketSim Brands" );
            DialogResult resp = mappingForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
            currentProject.Brands = mappingForm.NameMappingList;
            currentProject.SetEdited();
        }

        private void channelsToolStripMenuItem_Click( object sender, EventArgs e ) {
            NameMappingForm mappingForm = new NameMappingForm( currentProject.Channels, "Set MarketSim Channels" );
            DialogResult resp = mappingForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
            currentProject.Channels = mappingForm.NameMappingList;
            currentProject.SetEdited();
        }

        private void variantsToolStripMenuItem_Click( object sender, EventArgs e ) {
            NameMappingForm mappingForm = new NameMappingForm( currentProject.Products, "Set MarketSim Products" );
            mappingForm.HideSourceCol();
            DialogResult resp = mappingForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
            currentProject.Products = mappingForm.NameMappingList;
            currentProject.SetEdited();
        }

        private void scanWorksheetForVariantsToolStripMenuItem_Click( object sender, EventArgs e ) {
            ValidateAllWorksheets( false );
            UpdateProjectDisplay();
            MessageBox.Show( "\n\n      Variant Scan Completed    \n\n", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }

        // load alternateWorksheetNames Hashtable from (serialized) ArrayLists
        private void SetAlternateSheetNamesHashtable() {
            this.alternateWorksheetNames = new Hashtable();
            for( int i = 0; i < currentProject.AlternateSheetNameKeys.Count; i++ ) {
                string key = (string)currentProject.AlternateSheetNameKeys[ i ];
                string allAlts = (string)currentProject.AlternateSheetNameValues[ i ];
                string[] alts = allAlts.Split( '|' );
                ArrayList altsList = new ArrayList();
                foreach( string alt in alts ){
                    altsList.Add( alt );
                }
                alternateWorksheetNames.Add( key, altsList );
            }
        }

        // save alternateWorksheetNames Hashtable to ArrayLists (which can be serialized)
        private void ConverAlternateSheetNamesHashtableToLists() {
            currentProject.AlternateSheetNameKeys = new ArrayList();
            currentProject.AlternateSheetNameValues = new ArrayList();

            foreach( string key in this.alternateWorksheetNames.Keys ) {
                currentProject.AlternateSheetNameKeys.Add( key );
                ArrayList alts = (ArrayList)alternateWorksheetNames[ key ];
                string allAlts = "";
                for( int i = 0; i < alts.Count; i++ ) {
                    string s = (string)alts[ i ];
                    allAlts += s;
                    if( i != (alts.Count - 1) ) {
                        allAlts += "|";
                    }
                }
                currentProject.AlternateSheetNameValues.Add( allAlts );
            }
        }

        private void variantSettingAidToolStripMenuItem_Click( object sender, EventArgs e ) {
            VariantNamingAidForm namingForm = new VariantNamingAidForm( currentProject );
            DialogResult resp = namingForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
        }

        private void combineSectionFilesWithToolStripMenuItem_Click( object sender, EventArgs e ) {
            TreeNode selectedNode = treeView.SelectedNode;
            if( selectedNode == null || selectedNode.Parent != null && selectedNode.Parent != filesNode ) {
                MessageBox.Show( "Selected node not a section!" );
                return;
            }
            // this is a section node
            ProjectSettings.ProjectSection projSection = (ProjectSettings.ProjectSection)selectedNode.Tag;

            FileCombinerForm combForm = new FileCombinerForm( currentProject.ProjectFile, projSection );
            DialogResult resp = combForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            if( combForm.CombiningRuleIndex != 0 ) {
                MessageBox.Show( "\r\n    Error: Selected Combining RuleNot Implemented!    \r\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            // we have a good list of files
            ExcelFileCombiner combiner = new ExcelFileCombiner();
            ArrayList fileSetList = combForm.FilesList;
            for( int i = 0; i < fileSetList.Count; i++ ){
                string[] vals = (string[])fileSetList[ i ];

                Console.WriteLine( "\r\nCombining Files: {0} of {1}", i + 1, fileSetList.Count );
                Console.WriteLine( "Src 1 -- {0}", vals[ 0 ] );
                Console.WriteLine( "Src 2 -- {0}", vals[ 1 ] );
                Console.WriteLine( "Out   -- {0}", vals[ 2 ] );

                combiner.CombineFileValues( vals[ 0 ],  0, vals[ 1 ], 0, vals[ 2 ], "Price", "divide" );
            }
            combiner.Close();
        }

        private void variantScalingToolStripMenuItem_Click( object sender, EventArgs e ) {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Scaling Information  File";
            ofd.Filter = "Excel Files (*.xls)|*.xls";
            DialogResult resp = ofd.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            ExcelWriter2 scalingReader = new ExcelWriter2();
            scalingReader.Open( ofd.FileName, "Scaling" );
            currentProject.ProductScaling = new ArrayList();
            int row = 2;
            int col = 3;
            int valcol = 5;
            do {
                object obj = scalingReader.GetValue( row, col );
                if( obj as string == null ) {
                    break;
                }
                string nStr = (string)obj;

                object sobj = scalingReader.GetValue( row, valcol );
                if( sobj is double == false ) {
                    break;
                }
                double scaling = (double)sobj;

                 currentProject.ProductScaling.Add( new ProjectSettings.ProductScalingInfo( nStr, ProjectSettings.InfoSource.Unknown, scaling.ToString() ) );
                row += 1;
            }
            while( true );
            scalingReader.Kill();
            currentProject.SetEdited();

            NameMappingForm mappingForm = new NameMappingForm( currentProject.ProductScaling, "Set Product Volume Scalings" );
            mappingForm.HideSourceCol();
            //mappingForm.ValidateEntriesAdDouble = true;
            DialogResult resp2 = mappingForm.ShowDialog();
            if( resp2 != DialogResult.OK ) {
                return;
            }

            currentProject.ProductScaling = mappingForm.NameMappingList;
        }

        private void variantScalingToolStripMenuItem1_Click( object sender, EventArgs e ) {
            NameMappingForm mappingForm = new NameMappingForm( currentProject.ProductScaling, "Set Product Volume Scalings" );
            mappingForm.HideSourceCol();
            //mappingForm.ValidateEntriesAdDouble = true;
            DialogResult resp2 = mappingForm.ShowDialog();
            if( resp2 != DialogResult.OK ) {
                return;
            }

            currentProject.ProductScaling = mappingForm.NameMappingList;
        }

        private void loadVariamtsFromToolStripMenuItem_Click( object sender, EventArgs e ) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Project to Load Variants";
            ofd.Filter = "MarketSim Import Projects (*.nrp)|*.nrp";
            DialogResult resp = ofd.ShowDialog();
            if( resp == DialogResult.OK ) {
                ProjectSettings otherProject = ProjectSettings.LoadProjectSettings( ofd.FileName );
                foreach( ProjectSettings.ProductInfo newIinfo in otherProject.Products ) {
                    bool found = false;
                    foreach( ProjectSettings.ProductInfo pinfo in currentProject.Products ) {
                        if( pinfo.ImportName == newIinfo.ImportName && pinfo.Source == newIinfo.Source ) {
                            pinfo.MarketSimName = newIinfo.MarketSimName;
                            found = true;
                            break;
                        }
                    }
                    if( found == false ) {
                        currentProject.Products.Add( new ProjectSettings.ProductInfo( newIinfo.ImportName, newIinfo.Source, newIinfo.MarketSimName ) );
                    }
                }
            }
        }

        private void compareVariantsFromToolStripMenuItem_Click( object sender, EventArgs e ) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Project to Compare Variants";
            ofd.Filter = "MarketSim Import Projects (*.nrp)|*.nrp";
            DialogResult resp = ofd.ShowDialog();
            if( resp == DialogResult.OK ) {
                ProjectSettings otherProject = ProjectSettings.LoadProjectSettings( ofd.FileName );

                ArrayList missing = new ArrayList();
                ArrayList nonextra = new ArrayList();
                for( int i = 0; i < otherProject.Products.Count; i++ ) {
                    ProjectSettings.ProductInfo newIinfo = (ProjectSettings.ProductInfo)otherProject.Products[ i ];
                    bool found = false;
                    for( int j = 0; j < otherProject.Products.Count; j++ ) {
                        ProjectSettings.ProductInfo pIinfo = (ProjectSettings.ProductInfo)currentProject.Products[ j ];
                        if( pIinfo.ImportName == newIinfo.ImportName && pIinfo.Source == newIinfo.Source ) {
                            if( pIinfo.MarketSimName == newIinfo.MarketSimName ) {
                                found = true;
                                if( nonextra.Contains( j ) == false ) {
                                    nonextra.Add( j );
                                }
                                break;
                            }
                        }
                    }
                    if( found == false ) {
                        missing.Add( i );
                    }
                }

                if( nonextra.Count == otherProject.Products.Count && missing.Count == 0 ) {
                    MessageBox.Show( "\r\n   Success:  Project Products are Identical    \r\n", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information );
                    return;
                }

                string msg = string.Format( "\r\n    Error: Products not same in compared project.    \r\n\r\n    Missing: {0};  Extra: {1}", missing.Count,
                    otherProject.Products.Count - nonextra.Count );

                for( int m = 0; m < missing.Count; m++ ) {
                    int mi = (int)missing[ m ];
                    string mnam = ((ProjectSettings.ProductInfo)otherProject.Products[ mi ] ).MarketSimName;
                    msg += "    " + mnam + "    \r\n";
                }
                ArrayList extra = new ArrayList();
                for( int ex = 0; ex < otherProject.Products.Count; ex++ ){
                    extra.Add( ex );
                }
                for( int x = 0; x < nonextra.Count; x++ ) {
                    extra.Remove( (int)nonextra[ x ] );
                }
                msg += "\r\n\r\n";

                for( int m = 0; m < extra.Count; m++ ) {
                    int mi = (int)extra[ m ];
                    string mnam = ((ProjectSettings.ProductInfo)otherProject.Products[ mi ]).MarketSimName;
                    msg += "    " + mnam + "    \r\n";
                }
                MessageBox.Show( msg, "Products Differ", MessageBoxButtons.OK, MessageBoxIcon.Information );

            }
        }

        private void loadBrandsFromToolStripMenuItem_Click( object sender, EventArgs e ) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Project to Load Brands";
            ofd.Filter = "MarketSim Import Projects (*.nrp)|*.nrp";
            DialogResult resp = ofd.ShowDialog();
            if( resp == DialogResult.OK ) {
                ProjectSettings otherProject = ProjectSettings.LoadProjectSettings( ofd.FileName );
                foreach( ProjectSettings.BrandInfo newIinfo in otherProject.Brands ) {
                    bool found = false;
                    foreach( ProjectSettings.BrandInfo pinfo in currentProject.Brands ) {
                        if( pinfo.ImportName == newIinfo.ImportName && pinfo.Source == newIinfo.Source ) {
                            pinfo.MarketSimName = newIinfo.MarketSimName;
                            found = true;
                            break;
                        }
                    }
                    if( found == false ) {
                        currentProject.Brands.Add( new ProjectSettings.BrandInfo( newIinfo.ImportName, newIinfo.Source, newIinfo.MarketSimName ) );
                    }
                }
            }
        }

        private void loadChannelsFromToolStripMenuItem_Click( object sender, EventArgs e ) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Project to Load Channels";
            ofd.Filter = "MarketSim Import Projects (*.nrp)|*.nrp";
            DialogResult resp = ofd.ShowDialog();
            if( resp == DialogResult.OK ) {
                ProjectSettings otherProject = ProjectSettings.LoadProjectSettings( ofd.FileName );
                foreach( ProjectSettings.ChannelInfo newIinfo in otherProject.Channels ) {
                    bool found = false;
                    foreach( ProjectSettings.ChannelInfo pinfo in currentProject.Channels ) {
                        if( pinfo.ImportName == newIinfo.ImportName && pinfo.Source == newIinfo.Source ) {
                            pinfo.MarketSimName = newIinfo.MarketSimName;
                            found = true;
                            break;
                        }
                    }
                    if( found == false ) {
                        currentProject.Channels.Add( new ProjectSettings.ChannelInfo( newIinfo.ImportName, newIinfo.Source, newIinfo.MarketSimName ) );
                    }
                }
            }
        }

        private void outputFolderToolStripMenuItem_Click( object sender, EventArgs e ) {
            OutputFolderEditForm oForm = new OutputFolderEditForm( currentProject.OutputDirectory );
            DialogResult resp = oForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            if( currentProject.OutputDirectory != oForm.OutputFolder ) {
                currentProject.OutputDirectory = oForm.OutputFolder;
                currentProject.SetEdited();
            }
        }

        private void excludeVariantsToolStripMenuItem_Click( object sender, EventArgs e ) {
            ExcludeVariantsForm exForm = new ExcludeVariantsForm( currentProject.ExcludeProducts );
            DialogResult resp = exForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
            currentProject.ExcludeProducts = exForm.ExcludeItems;
            currentProject.SetEdited();
        }

        private void specififyVariantsToProcessToolStripMenuItem_Click( object sender, EventArgs e ) {
            SpecifyVariantsForm svForm = new SpecifyVariantsForm( currentProject.SpecifiedProducts );
            DialogResult resp = svForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
            currentProject.SpecifiedProducts = svForm.SpecifiedItems;
            currentProject.SetEdited();
        }

        private void deleteToolStripMenuItem_Click( object sender, EventArgs e ) {
            TreeNode selectedNode = treeView.SelectedNode;
            if( selectedNode == null || selectedNode.Parent != null && selectedNode.Parent != filesNode ) {
                MessageBox.Show( "Selected item can not be removed from the project." );
                return;
            }
            // this is a section node
            ProjectSettings.ProjectSection projSection = (ProjectSettings.ProjectSection)selectedNode.Tag;

            DialogResult resp = MessageBox.Show( "OK to remove the selected section from the project?", "Confirm Remove", MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
            if( resp != DialogResult.OK ) {
                return;
            }

            bool ok = currentProject.DeleteSection( projSection );
            UpdateProjectDisplay();
            if( ok ) {
                MessageBox.Show( "Section Removed Successfully" );
            }
        }

        private void descriptionToolStripMenuItem_Click( object sender, EventArgs e ) {
            if( currentProject == null ) {
                MessageBox.Show( "No Current Project" );
                return;
            }

            DescriptionForm descFrom = new DescriptionForm( currentProject.ProjectName, currentProject.Description );
            DialogResult resp = descFrom.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            if( currentProject.Description == descFrom.Description && currentProject.ProjectName == descFrom.ProjectName ) {
                //no change
                return;
            }

            currentProject.ProjectName = descFrom.ProjectName;
            currentProject.Description = descFrom.Description;
            currentProject.SetEdited();
            UpdateProjectDisplay();
        }

        private void awarenessPersuasionToolStripMenuItem_Click( object sender, EventArgs e ) {
            AwarenessPersuasionForm awForm = new AwarenessPersuasionForm();

            awForm.DisplayAwareness = currentProject.DisplayAwareness;
            awForm.DistributionAwareness = currentProject.DistributionAwareness;
            awForm.MediaAwareness = currentProject.MediaAwareness;
            awForm.CouponsAwareness = currentProject.CouponsAwareness;
            awForm.DisplayPersuasion = currentProject.DisplayPersuasion;
            awForm.DistributionPersuasion = currentProject.DistributionPersuasion;
            awForm.MediaPersuasion = currentProject.MediaPersuasion;
            awForm.CouponsPersuasion = currentProject.CouponsPersuasion;

            DialogResult resp = awForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            currentProject.DisplayAwareness = awForm.DisplayAwareness;
            currentProject.DistributionAwareness = awForm.DistributionAwareness;
            currentProject.MediaAwareness = awForm.MediaAwareness;
            currentProject.CouponsAwareness = awForm.CouponsAwareness;
            currentProject.DisplayPersuasion = awForm.DisplayPersuasion;
            currentProject.DistributionPersuasion = awForm.DistributionPersuasion;
            currentProject.MediaPersuasion = awForm.MediaPersuasion;
            currentProject.CouponsPersuasion = awForm.CouponsPersuasion;

            currentProject.SetEdited();
        }

        private void treeView_AfterSelect( object sender, TreeViewEventArgs e ) {

        }

        private void mediaItemsToolStripMenuItem_Click( object sender, EventArgs e ) {
            MediaNameMappingForm mappingForm = new MediaNameMappingForm( currentProject.MediaItems, "Set MarketSim Media Items" );
            DialogResult resp = mappingForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
            currentProject.MediaItems = mappingForm.NameMappingList;
            currentProject.SetEdited();
        }
    }
}