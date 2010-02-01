using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace DataImporter.ImportSettings
{
    [XmlInclude( typeof( ImportSettings.ProjectSettings.ProductInfo ) ),
      XmlInclude( typeof( ImportSettings.ProjectSettings.BrandInfo ) ),
       XmlInclude( typeof( ImportSettings.ProjectSettings.ChannelInfo ) ),
       XmlInclude( typeof( ImportSettings.ProjectSettings.ImportFileInfo ) ),
       XmlInclude( typeof( ImportSettings.ProjectSettings.ProjectSection ) ),
       XmlInclude( typeof( ImportSettings.ProjectSettings.ImportItemInfo ) ),
       XmlInclude( typeof( ImportSettings.WorksheetSettings ) ),
       XmlInclude( typeof( ImportSettings.ProjectSettings.WorksheetNameInfo ) ),
       XmlInclude( typeof( Library.XPoint ) ),
       XmlInclude( typeof( ImportSettings.ProjectSettings.ProductScalingInfo ) )]
    public class ProjectSettings
    {
        public string ProjectName;
        public string ProjectFile;

        public ArrayList ProjectSections;             // a list of ProjectSection objects

        public string InputRootDirectory;
        public string OutputDirectory;

        public ArrayList ProductGroups;                //a list of GroupInfo objects
        public ArrayList Products;                        //a list of ProductInfo objects
        public ArrayList MediaItems;                     //a list of ProductInfo objects
        public ArrayList Brands;                           //a list of BrandInfo objects
        public ArrayList Channels;                         //a list of ChannelInfo objects
        public ArrayList WorksheetNames;              //a list of WorksheetNameInfo objects
        public ArrayList AllImportFiles;                   //a list of ImportFileInfo objects, which each contain a list of WorksheetSettings objects
        public ArrayList ProductScaling;                  //a list of ProductScalingInfo objects
        public ArrayList ExcludeProducts;                //a list of strings
        public ArrayList SpecifiedProducts;              //a list of strings

        public ArrayList AlternateSheetNameKeys;        // a list of sheet name strings
        public ArrayList AlternateSheetNameValues;     // a list of sets of sheet name alternates, separated by the | char

        public DateTime StartImportDate;
        public DateTime EndImportDate;
        public double ImportCompressionDeltaTolerance;
        public double ImportDataPriceScaling;                     // sample usage: to convert price per eq to price per oz

        public bool DateRelToIntervalPrefSet = false;
        public bool DateRelToiIntervalIsStart = false;

        public bool NormalizePrices = false;

        public string Description;

        public double DistributionAwareness;
        public double DisplayAwareness;
        public double MediaAwareness;
        public double CouponsAwareness;

        public double DistributionPersuasion;
        public double DisplayPersuasion;
        public double MediaPersuasion;
        public double CouponsPersuasion;

        private const double DEFAULT_MEDIA_AWARENESS = 0.6;
        private const double DEFAULT_MEDIA_PERSUASION = 0.0;
        private const double DEFAULT_AWARENESS = 0.1;
        private const double DEFAULT_PERSUASION = 0.1;

        private bool edited;

        // places that brand or channel info can be found
        public enum InfoSource
        {
            DirectoryName,
            FileName,
            WorksheetName,
            FileContents,
            UserSpecified,
            Unknown
        }

        // places that brand or channel info can be found
        public enum DataType
        {
            Display,
            Distribution,
            Media,
            Price,
            PriceRegular,
            PricePromo,
            PriceAbsolute,
            PromoPricePct,
            AbsolutePricePct,
            RealSales,
            Unknown
        }

        public ProjectSettings() {
            ProjectName = null;
            ProjectFile = null;

            InputRootDirectory = null;
            OutputDirectory = null;

            NormalizePrices = false;
            edited = false;

            ProjectSections = new ArrayList();

            ProductGroups = new ArrayList();
            Products = new ArrayList();
            MediaItems = new ArrayList();
            Brands = new ArrayList();
            Channels = new ArrayList();
            WorksheetNames = new ArrayList();
            AllImportFiles = new ArrayList();
            ProductScaling = new ArrayList();
            ExcludeProducts = new ArrayList();
            SpecifiedProducts = new ArrayList();

            StartImportDate = new DateTime( 2000, 1, 1 );
            EndImportDate = new DateTime( 2020, 1, 1 );
            Description = "";

            ImportDataPriceScaling = 1;

            DistributionAwareness = DEFAULT_AWARENESS;
            DisplayAwareness = DEFAULT_AWARENESS;
            MediaAwareness = DEFAULT_MEDIA_AWARENESS;
            CouponsAwareness = DEFAULT_MEDIA_AWARENESS;

            DistributionPersuasion = DEFAULT_PERSUASION;
            DisplayPersuasion = DEFAULT_PERSUASION;
            MediaPersuasion = DEFAULT_MEDIA_PERSUASION;
            CouponsPersuasion = DEFAULT_MEDIA_PERSUASION;
        }

        public void SetEdited() {
            edited = true;
        }

        public bool isEdited() {
            return edited;
        }

        public string PropertiesString() {
            string s = "   ?   ";
            return s;
        }

#region Section Utilities
        public ProjectSection AddSingleFileSection( string filePath, ProjectSettings.InfoSource brandInfoSource, ProjectSettings.InfoSource channelInfoSource ) {
            ProjectSection section = new ProjectSection( filePath, false, brandInfoSource, channelInfoSource );
            ProjectSections.Add( section );
            return section;
        }

        public ProjectSection AddMultiFileSection( string rootFolderPath, ProjectSettings.InfoSource brandInfoSource, ProjectSettings.InfoSource channelInfoSource ) {
            ProjectSection section = new ProjectSection( rootFolderPath, true, brandInfoSource, channelInfoSource );
            ProjectSections.Add( section );
            return section;
        }

        public ProjectSection GetSection( int sectionIndex ) {
            if( sectionIndex >= 0 && sectionIndex < ProjectSections.Count ) {
                return (ProjectSection)ProjectSections[ sectionIndex ];
            }
            else {
                return null;
            }
        }

        public bool DeleteSection( ProjectSection sectionToDelete ) {
            for( int i = 0; i < ProjectSections.Count; i++ ) {
                if( (ProjectSection)ProjectSections[ i ] == sectionToDelete ) {
                    ProjectSections.RemoveAt( i ); 
                    SetEdited();
                    return true;
                }
            }
            // no match
            return false;
        }

        public int SectionCount {
            get { return ProjectSections.Count; }
        }
#endregion

        public BrandInfo AddBrand( string importBrandIdentifier, InfoSource source ) {
            BrandInfo tstInfo = GetBrand( importBrandIdentifier, source );
            if( tstInfo != null ) {
                return tstInfo;
            }

            BrandInfo info = new BrandInfo( importBrandIdentifier, source, importBrandIdentifier );
            Brands.Add( info );
            edited = true;
            return info;
        }

        public BrandInfo GetBrand( string importBrandIdentifier, InfoSource source ) {
            return GetBrand( importBrandIdentifier, source, false );
        }

        public BrandInfo GetBrand( string importBrandIdentifier, InfoSource source, bool substringMatchOK ) {
            foreach( BrandInfo tstInfo in Brands ) {
                if( tstInfo.ImportName == importBrandIdentifier && tstInfo.Source == source ) {
                    return tstInfo;
                }
                if( substringMatchOK ) {
                    if( importBrandIdentifier.IndexOf( tstInfo.ImportName ) != -1 && tstInfo.Source == source ) {
                        return tstInfo;
                    }
                }
            }
            return null;
        }


        public ChannelInfo AddChannel( string importChanneldentifier, InfoSource source ) {
            ChannelInfo tstInfo = GetChannel( importChanneldentifier, source );
            if( tstInfo != null ) {
                return tstInfo;
            }

            ChannelInfo info = new ChannelInfo( importChanneldentifier, source, importChanneldentifier );
            Channels.Add( info );
            edited = true;
            return info;
        }

        public ChannelInfo GetChannel( string importChanneldentifier, InfoSource source ) {
            return GetChannel( importChanneldentifier, source, false );
        }

        public ChannelInfo GetChannel( string importChanneldentifier, InfoSource source, bool substringMatchOK ) {
            foreach( ChannelInfo tstInfo in Channels ) {
                if( tstInfo.ImportName == importChanneldentifier && tstInfo.Source == source ) {
                    return tstInfo;
                }
                if( substringMatchOK ) {
                    if( importChanneldentifier.IndexOf( tstInfo.ImportName ) != -1 && tstInfo.Source == source ) {
                        return tstInfo;
                    }
                }
            }
            return null;
        }

        public WorksheetNameInfo AddWorksheetInfo( string importDataTypeldentifier, InfoSource source, string outputDataTypeldentifier ) {
            WorksheetNameInfo tstInfo = GetWorksheetInfo( importDataTypeldentifier, source );
            if( tstInfo != null ) {
                return tstInfo;
            }

            WorksheetNameInfo info = new WorksheetNameInfo( importDataTypeldentifier, source, outputDataTypeldentifier );
            WorksheetNames.Add( info );
            edited = true;
            return info;
        }

        public WorksheetNameInfo GetWorksheetInfo( string importDataTypeldentifier, InfoSource source ) {
            return GetWorksheetInfo( importDataTypeldentifier, source, false );
        }

        public WorksheetNameInfo GetWorksheetInfo( string importDataTypeldentifier, InfoSource source, bool substringMatchOK ) {
            foreach( ProjectSettings.WorksheetNameInfo tstInfo in WorksheetNames ) {
                if( tstInfo.ImportName == importDataTypeldentifier && tstInfo.Source == source ) {
                    return tstInfo;
                }
                if( substringMatchOK ) {
                    if( importDataTypeldentifier.IndexOf( tstInfo.ImportName ) != -1 && tstInfo.Source == source ) {
                        return tstInfo;
                    }
                }
            }
            return null;
        }

        public DataType GetWorksheetType( string importDataTypeldentifier ) {
            WorksheetNameInfo info = GetWorksheetInfo( importDataTypeldentifier, InfoSource.WorksheetName );
            DataType typ = DataType.Unknown;
            if( info != null ) {
                switch( info.MarketSimName ) {
                    case "Display":
                        typ = DataType.Display;
                        break;
                    case "Distribution":
                        typ = DataType.Distribution;
                        break;
                    case "Media":
                        typ = DataType.Media;
                        break;
                    case "Real Sales":
                        typ = DataType.RealSales;
                        break;
                    case "Price":
                    case "Price (Regular)":
                    case "PriceReg":
                        typ = DataType.PriceRegular;
                        break;
                    case "Price (Promo)":
                    case "PricePromo":
                        typ = DataType.PricePromo;
                        break;
                    case "Price (Absolute)":
                        typ = DataType.PriceAbsolute;
                        break;
                    case "Price (% Promo)":
                    case "PromoPricePct":
                        typ = DataType.PromoPricePct;
                        break;
                    case "Price (% Absolute)":
                        typ = DataType.AbsolutePricePct;
                        break;
                }
            }
            return typ;
        }

        public void AddProduct( string importProductldentifier ) {
            AddProduct( importProductldentifier, importProductldentifier );
        }

        public void AddProduct( string importProductldentifier, string exportProductIdentifier ) {
            foreach( ProductInfo tstInfo in Products ) {
                if( tstInfo.ImportName == importProductldentifier ) {
                    // we already have this product
                    return;
                }
            }
            Products.Add( new ProductInfo( importProductldentifier, InfoSource.Unknown, exportProductIdentifier ) );
            edited = true;
        }

        public ProductInfo GetProduct( string importProductIdentifier ) {
            foreach( ProductInfo tstInfo in Products ) {
                // look for an exact match first
                if( tstInfo.ImportName == importProductIdentifier ) {
                    return tstInfo;
                }
                else if( tstInfo.ImportName.StartsWith( "All - " ) ) {
                    if( tstInfo.ImportName.Substring( 6 ) == importProductIdentifier ) {
                        return tstInfo;
                    }
                }
            }
            foreach( ProductInfo tstInfo in Products ) {
                if( tstInfo.ImportName.IndexOf( importProductIdentifier ) != -1 ) {
                    return tstInfo;
                }
            }
            return null;
        }

        public void AddMediaItem( string importItemdentifier ) {
            AddMediaItem( importItemdentifier, importItemdentifier, importItemdentifier );
        }

        public void AddMediaItem( string importItemIdentifier, string exportItemIdentifier, string exportChannelIdentifier ) {
            foreach( ProductInfo tstInfo in MediaItems ) {
                if( tstInfo.ImportName == importItemIdentifier ) {
                    // we already have this product
                    return;
                }
            }
            MediaItems.Add( new ProductInfo( importItemIdentifier, InfoSource.Unknown, exportItemIdentifier, exportChannelIdentifier ) );
            edited = true;
        }

        public ProductInfo GetMediaItem( string importItemIdentifier ) {
            foreach( ProductInfo tstInfo in MediaItems ) {
                // look for an exact match first
                if( tstInfo.ImportName == importItemIdentifier ) {
                    return tstInfo;
                }
                else if( tstInfo.ImportName.StartsWith( "All - " ) ) {
                    if( tstInfo.ImportName.Substring( 6 ) == importItemIdentifier ) {
                        return tstInfo;
                    }
                }
            }
            // see if the string is in there anywhere
            foreach( ProductInfo tstInfo in MediaItems ) {
                if( tstInfo.ImportName.IndexOf( importItemIdentifier ) != -1 ) {
                    return tstInfo;
                }
            }

            string brandProdSplitter = " - ";
            if( importItemIdentifier.IndexOf( brandProdSplitter ) != -1 ) {

                string br = importItemIdentifier.Substring( 0, importItemIdentifier.IndexOf( brandProdSplitter ) );
                string pr = importItemIdentifier.Substring( importItemIdentifier.IndexOf( brandProdSplitter ) + brandProdSplitter.Length );

                // handle special cases!
                if( br.EndsWith( " Brand" ) ) {
                    br = br.Substring( 0, br.Length - 6 );
                }
                else {
                    if( br == "St Ives" ) {
                        br = "StIves";
                    }
                }
                br = br.ToLower();
                pr = pr.ToLower();

                foreach( ProductInfo tstInfo in MediaItems ) {

                    string lowertst = tstInfo.ImportName.ToLower();

                    if( lowertst.IndexOf( br ) != -1 && lowertst.IndexOf( pr ) != -1 ) {
                        // ok if we found both the brand and the product within the string
                        return tstInfo;
                    }
                }
            }
            return null;
        }

        public bool IsProductMismatchOkay( string inputProductName1, string inputProductName2 ) {
            bool doCheck = true;

            if( SpecifiedProducts.Count > 0 ) {
                doCheck = false;
                foreach( string sp in SpecifiedProducts ) {
                    string altSp = sp;
                    if( sp.StartsWith( "All - " ) ) {
                        altSp = altSp.Substring( 6 );
                    }
                    if( inputProductName1.StartsWith( sp ) || inputProductName2.StartsWith( sp ) ||
                        inputProductName1.StartsWith( altSp ) || inputProductName2.StartsWith( altSp ) ) {
                        doCheck = true;
                        break;
                    }
                }
            }
            if( ExcludeProducts.Count > 0 ) {
                foreach( string xp in ExcludeProducts ) {
                    string altXp = xp;
                    if( xp.StartsWith( "All - " ) ) {
                        altXp = altXp.Substring( 6 );
                    }
                    if( inputProductName1.StartsWith( xp ) || inputProductName2.StartsWith( xp ) ||
                         inputProductName1.StartsWith( altXp ) || inputProductName2.StartsWith( altXp ) ) {
                        doCheck = false;
                        break;
                    }
                }
            }

            bool isOkay = true;
            if( doCheck ) {
                // see if these are two altername input products that match the same MarketSim product
                isOkay = false;
                ProductInfo oProd1 = GetProduct( inputProductName1 );
                ProductInfo oProd2 = GetProduct( inputProductName2 );
                if( oProd1 != null && oProd2 != null ) {
                    if( oProd1.MarketSimName == oProd2.MarketSimName ) {
                        isOkay = true;
                    }
                }
            }

            return isOkay;
        }

        public string AbsolutePathFor( string projectItemPath ) {
            return InputRootDirectory + "\\" + projectItemPath;
        }

        ////public ProjectSettings( string projectName, bool normalizePrices, string rootPath, string pathRelativeToRoot, bool pathIsFolder )
        public ProjectSettings( string projectName, bool normalizePrices, string dataRootFolder, string outputFolder )
            : this() {
            ProjectName = projectName;
            InputRootDirectory = dataRootFolder;
            NormalizePrices = normalizePrices;
            OutputDirectory = outputFolder;

            ProjectFile = dataRootFolder + "\\" + projectName + DataImporter.OutputFileExt;

            Console.WriteLine( "Created new Project: name = {0}, rootDir = {1}", ProjectName, InputRootDirectory );
        }

        #region Saving and Loading
        /// <summary>
        /// Factory method for getting a Settings object from a disk file, given the corresponding NITRO File path.
        /// </summary>
        /// <param name="settingsFilePath"></param>
        /// <returns></returns>
        public static ProjectSettings LoadProjectSettings( string settingsFilePath ) {
            Console.WriteLine( "ProjectSettings.LoadProjectSettings( {0} )...", settingsFilePath );

            ProjectSettings settings = null;

            if( File.Exists( settingsFilePath ) ) {
                FileStream fs = null;
                try {
                    XmlSerializer serializer = new XmlSerializer( typeof( ProjectSettings ) );
                    fs = new FileStream( settingsFilePath, FileMode.Open, FileAccess.Read );
                    settings = (ProjectSettings)serializer.Deserialize( fs );
                }
                catch( Exception ) {
                    MessageBox.Show( "Error: Unable to load settings from file: \r\n\r\n" + settingsFilePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                }
                finally {
                    if( fs != null ) {
                        fs.Close();
                    }
                }
            }
            else {
                MessageBox.Show( "Error: Unable to load settings from file.  File does not exist: \r\n\r\n" + settingsFilePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
            return settings;
        }

        /// <summary>
        /// Saves the settings for the project
        /// </summary>
        public string Save() {
            Console.WriteLine( "ProjectSettings.Save()..." );

            string settingsFilePath = ProjectFile;

            XmlSerializer serializer = new XmlSerializer( typeof( ProjectSettings ) );
            StreamWriter sw = new StreamWriter( new FileStream( settingsFilePath, FileMode.Create, FileAccess.Write ) );
            serializer.Serialize( sw, this );
            sw.Flush();
            sw.Close();
            edited = false;

            return settingsFilePath;
        }
        #endregion

        #region Helper (Embedded) Classes

        public class ImportFileInfo
        {
            public string ImportFilePath;
            public string OutputFilePath;
            
            public ArrayList WorksheetInfo;

            public ImportFileInfo() {
                ImportFilePath = null;
                WorksheetInfo = new ArrayList();
            }

            public ImportFileInfo( string filePath ) : base() {
                ImportFilePath = filePath;
            }
        }

        public class ProjectSection
        {
            public string SpecificFile;
            public string FleSetFolder;
            public int FileCount;
            public bool Scanned;
            public bool Valid;
            public ArrayList WorksheetSettingsList;       //a list of WorksheetSettings objects

            public bool IsFileSet {
                get { return (FleSetFolder != null); }
            }

            public InfoSource BrandSource;
            public InfoSource ChannelSource;
            public InfoSource DataTypeSource;
            public string SpecificBrand;
            public string SpecificChannel;

            [NonSerialized]
            public DateTime StartDate;
            [NonSerialized]
            public TimeSpan TimeStep;
            [NonSerialized]
            public int TimeStepCount;
            [NonSerialized]
            public DateTime EndDate;
            [NonSerialized]
            public bool WorksheetDatesSet;
            [NonSerialized]
            public bool WorksheetDatesValid;

            [NonSerialized]
            public ArrayList Variants;          // a list of strings specifying the variants (or other data item list headers) in files in this section
            [NonSerialized]
            public ArrayList Channels;
            [NonSerialized]
            public bool WorksheetVariantsSet;
            [NonSerialized]
            public bool WorksheetVariantsValid;
            [NonSerialized]
            public bool WorksheetChannelsSet;
            [NonSerialized]
            public bool WorksheetChannelsValid;

            public ProjectSection() {
                BrandSource = InfoSource.Unknown;
                ChannelSource = InfoSource.Unknown;
                DataTypeSource = InfoSource.WorksheetName;         // usually the worksheet name is the data type
                FileCount = 0;
                Scanned = false;
                Valid = false;
                WorksheetSettingsList = new ArrayList();
                WorksheetVariantsSet = false;
                WorksheetVariantsValid = false;
                WorksheetDatesSet = false;
                WorksheetDatesValid = false;
                WorksheetChannelsSet = false;
                WorksheetChannelsValid = false;
            }

            public string SectionPath {
                get {
                    if( this.IsFileSet ) {
                        return FleSetFolder;
                    }
                    else {
                        return SpecificFile;
                    }
                }
            }

            public void AddWorksheet( WorksheetSettings worksheetSettings ) {
                // remove the entry for the worksheet if it is there already
                for( int i = WorksheetSettingsList.Count - 1; i >= 0; i-- ){
                    WorksheetSettings existingWsSettings = (WorksheetSettings)WorksheetSettingsList[ i ];
                    if( existingWsSettings.SheetName == worksheetSettings.SheetName ) {
                        Console.WriteLine( "ProjectSection.AddWorksheet() removing existing worksheet named {0}", worksheetSettings.SheetName );
                        WorksheetSettingsList.RemoveAt( i );
                        break;
                    }
                }
                //add the worksheet entry
                WorksheetSettingsList.Add( worksheetSettings );
                Console.WriteLine( "ProjectSection.AddWorksheet() added worksheet named {0}; count = {1}", worksheetSettings.SheetName, WorksheetSettingsList .Count );
            }

            public string[] GetFiles( ProjectSettings project ) {
                string[] vals = null;
                if( IsFileSet == false ) {
                    vals = new string[ 1 ];
                    vals[ 0 ] = project.InputRootDirectory + "\\" + this.SpecificFile;
                }
                else {
                    string path = project.InputRootDirectory + "\\" + this.FleSetFolder;
                    vals = Directory.GetFiles( path, DataImporter.InputFilePattern, SearchOption.AllDirectories );
                }
                return vals;
            }
            
            public ProjectSection( string path, bool pathIsFolder, InfoSource brandInfoSounce, InfoSource channelInfoSounce )
                : this() {
                if( pathIsFolder ) {
                    FleSetFolder = path;
                }
                else {
                    SpecificFile = path;
                }

                BrandSource = brandInfoSounce;
                ChannelSource = channelInfoSounce;
            }

            public string PropertiesString() {
                string fmt = "\r\n     Project Section Properties: {0}    \r\n-----------------------------------------------\r\n\r\n    Path: {1}     "  +
                     "\r\n\r\n    Brand Source: {2}\r\n\r\n    Channel Source: {3}\r\n\r\n    Scanned: {4}\r\n    Validated: {5}\r\n\r\n" + 
                     "    File Count: {6}\r\n\r\n    Worksheet Setting Types: {7}\r\n\r\n";
                string fileIdent = "Individual File";
                if( this.IsFileSet ) {
                    fileIdent = "File Set";
                }

                string s = String.Format( fmt, fileIdent, SectionPath, BrandSource, ChannelSource, Scanned, Valid, FileCount, WorksheetSettingsList.Count );

                s += String.Format( "    Time Stap: {0:f0} days\r\n\r\n", TimeStep.TotalDays );
                return s;
            }
        }

        public class ProductInfo : ImportItemInfo
        {
            public ProductInfo() : base() { }
            public ProductInfo( string importName, ProjectSettings.InfoSource source, string marketSimName ) : base( importName, source, marketSimName ) { }
            public ProductInfo( string importName, ProjectSettings.InfoSource source, string marketSimName, string marketSimCampaign ) : 
                base( importName, source, marketSimName ) {
                this.MarketSimCampaign = marketSimCampaign;
            }
        }

        public class BrandInfo : ImportItemInfo
        {
           public BrandInfo() : base() { }
            public BrandInfo( string importName, ProjectSettings.InfoSource source, string marketSimName ) : base( importName, source, marketSimName ) { }
        }

        public class ChannelInfo : ImportItemInfo
        {
            public ChannelInfo() : base() { }
            public ChannelInfo( string importName, ProjectSettings.InfoSource source, string marketSimName ) : base( importName, source, marketSimName ) { }
        }

        public class WorksheetNameInfo : ImportItemInfo
        {
            public WorksheetNameInfo() : base() { }
            public WorksheetNameInfo( string importName, ProjectSettings.InfoSource source, string marketSimName ) : base( importName, source, marketSimName ) { }
        }

        public class ProductScalingInfo : ImportItemInfo
        {
            public ProductScalingInfo() : base() { }
            public ProductScalingInfo( string importName, ProjectSettings.InfoSource source, string scaling ) : base( importName, source, scaling ) { }
        }

        public class ImportItemInfo
        {
            public string ImportName;
            public InfoSource Source;
            public string MarketSimName;
            public string MarketSimCampaign;

            public ImportItemInfo() {
                ImportName = null;
                Source = InfoSource.Unknown;
                MarketSimName = null;
            }

            public ImportItemInfo( string importName, ProjectSettings.InfoSource source, string marketSimName ) {
                ImportName = importName;
                Source = source;
                MarketSimName = marketSimName;
            }
        }
        #endregion
    }
}
