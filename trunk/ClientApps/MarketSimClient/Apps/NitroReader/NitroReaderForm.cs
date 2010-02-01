using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using NitroReader.Dialogs;
using NitroReader.Output;
using NitroReader.Library;

namespace NitroReader
{
    /// <summary>
    /// NitroReaderForm is the main form for the NITRO Reader application.
    /// </summary>
    public partial class NitroReaderForm : Form
    {
        private MarketPlan marketPlan;

        private string fileName;                // input file
        private string planFileName;          // output file

        private int nextComponentToLoad = 0;
        private ProgressDialog pForm;

        private static ExcelWriter2 reader;
        private static ExcelWriter2 writer;

        private System.Windows.Forms.Timer timer;
        private string templateFile = "OutputTemplate.xls";
//        private string nitroFileDirectory = "";
        private static GeneralSettingsValues generalFormSettings;
        private const string marketPlanNameFormat = "MarketSim - {0}";
        private static string SettingsFile = "DPNitroReaderSettings.nrs";        //settings for the app in general
        private Settings settings;                                                           //these pertain to the specific NITRO file loaded
        private ArrayList reportVariantsList;                                            //this gets refreshed along with the list view items - is a simlar list but includes non-visible items

        private Color[] groupColors = new Color[] {
            Color.FromArgb( 220, 255, 220 ),
            Color.FromArgb( 220, 220, 255 ),
            Color.FromArgb( 245, 245, 210 ),
            Color.FromArgb( 240, 210, 240 ),
            Color.FromArgb( 255, 230, 215 )
        };

        private string editingNitroName = null;                                             // used during name editing in the list view

        // display sorting settings
        private bool sortByNitroName = false;
        private bool sortByMarketSimName = true;
        private bool sortByVolume = false;
        private bool sortByBrand = false;
        private bool sortReversed = false;
        private int nitroNameColumn = 2;
        private int marketSimNameColumn = 1;
        private int volumeColumn = 3;
        private int brandColumn = 4;

        private int column0Width = 22;

        /// <summary>
        /// Creates a new Nitro Reader form.
        /// </summary>
        public NitroReaderForm() {

            InitializeComponent();

            column0Width = this.listView1.Columns[ 0 ].Width;

            generalFormSettings = GeneralSettingsValues.Load( SettingsFile );

            LoadCustomerIcon( generalFormSettings.CustomerIconFile ); 

            // the template may exist in the current directory (installed setup) or in the templates dir (development setup)
            this.templateFile = ProductFile.FilePath( System.Environment.CurrentDirectory + "\\" + this.templateFile, "templates" );

            DataLogger.Init();
            ReportGenerator.Init();
        }

        #region NITRO File Selection and Validation
        /// <summary>
        /// Allows the user to select a NITRO file to processs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectButton_Click( object sender, EventArgs e ) {

            if( (this.settings != null) && this.settings.Edited ) {
                ConfirmForm cform = new ConfirmForm( "Do you want to save the settings before opening a new file?", "Settings Changed" );
                DialogResult resp1 = cform.ShowDialog();
                if( resp1 == DialogResult.OK ) {
                    this.settings.Save( this.fileName, this.reportVariantsList );
                }
            }

            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Choose NITRO File to Process";
            fd.Filter = "Excel Files (*.xls)|*.xls";
            fd.InitialDirectory = generalFormSettings.NITROFileFolder;
            fd.RestoreDirectory = true;
            DialogResult resp = fd.ShowDialog( this );
            if( resp == DialogResult.OK ) {
                fileName = fd.FileName;
                FileInfo finfo = new FileInfo( fileName );
                generalFormSettings.NITROFileFolder = finfo.Directory.ToString() + "\\";
                this.nameTextBox.Text = "";       
                this.nameTextBox.Enabled = false;
                this.normalizeCheckBox.Enabled = false;
                this.warningsButton.Visible = false;
                this.nitroFileLabel.ForeColor = Color.Black;
                this.nitroFileLabel.Font = new Font( "Arial", 8 );
                this.nitroFileLabel.Text = "Processing...";
                this.infoLabel1.Text = "";
                this.infoLabel2.Text = "";
                this.listView1.Items.Clear();
                this.listView1.Enabled = false;

                // use a timer to trigger the next phase so the UI can update now
                timer = new System.Windows.Forms.Timer();
                timer.Interval = 1;
                timer.Tick += new EventHandler( ValidateFileNow );
                timer.Start();
            }
        }

        /// <summary>
        /// Actually validates a given  Excel file as a NITRO file by parsing the date-column headers and loading the product row names.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValidateFileNow( object sender, EventArgs e ) {
            timer.Stop();

            this.Cursor = Cursors.WaitCursor;

            this.nitroFileLabel.Font = new Font( "Arial", 8, FontStyle.Bold );
            SetElidedString( fileName, this.nitroFileLabel );

            this.marketPlan = new MarketPlan();

            if( reader != null ) {
                reader.Kill();
                reader = null;
            }
            reader = new ExcelWriter2();
            bool valid = marketPlan.ValidateNitro( reader, fileName );

            if( valid ) {
                foreach( MarketPlan.Component comp in this.marketPlan.Components ) {
                    if( comp.Name == "%ACV DIST" ) {
                        comp.Awareness = generalFormSettings.DistributionAwareness;
                        comp.Persuasion = generalFormSettings.DistributionPersuasion;
                    }
                    else {
                        comp.Awareness = generalFormSettings.DisplayAwareness;
                        comp.Persuasion = generalFormSettings.DisplayPersuasion;
                    }
                }
                settings = Settings.LoadSettingsForFile( fileName, marketPlan.Variants );

                FileInfo finfo = new FileInfo( fileName );
                this.listView1.Enabled = true;
                this.nameTextBox.Enabled = true;
                this.normalizeCheckBox.Checked = settings.NormalizeForNIMO;
                this.normalizeCheckBox.Enabled = true;
                if( settings.MarketPlanName.Length == 0 ) {
                    this.nameTextBox.Text = String.Format( marketPlanNameFormat, finfo.Name.Substring( 0, finfo.Name.Length - finfo.Extension.Length ) );
                }
                else {
                    this.nameTextBox.Text = settings.MarketPlanName;
                }
                settings.ClearEdited();
                this.marketPlan.Name = this.nameTextBox.Text;

                // update the data displayed in the UI
                this.nitroFileLabel.ForeColor = Color.DarkGreen;
                this.infoLabel1.ForeColor = Color.Black;
                this.infoLabel2.ForeColor = Color.Black;

                string info1 = String.Format( "{0} Measures: ", marketPlan.Components.Count );
                for( int i = 0; i < marketPlan.Components.Count; i++ ) {
                    info1 += ((MarketPlan.Component)marketPlan.Components[ i ]).Name;
                    if( i != (marketPlan.Components.Count - 1) ) {
                        info1 += ", ";
                    }
                }
                this.infoLabel1.Text = info1;
                this.infoLabel2.Text = String.Format( "Dates: {0} thru {1}",
                                        marketPlan.StartDate.ToShortDateString(), marketPlan.EndDate.ToShortDateString() );

                //read the data values from the UNITS sheet now, so we can have product ranking numbers to display prior to full-data processing
                MarketPlan.Component unitsComp = marketPlan.GetComponent( "UNITS" );

                if( unitsComp != null ) {
                    unitsComp.ReadData( reader );
                }
                else {
                    string w = "Warning: There is no UNITS sheet in the NITRO file.  Products will not be ranked.";
                    Console.WriteLine( w );
                    MarketPlan.warnings.Add( w );
                }

                UpdateVariantsListView( marketPlan );

                copySettingsButton.Enabled = true;
                goButton.Enabled = true;
                saveButton.Enabled = true;
            }
            else {
                // not a valid NITRO file
                this.nitroFileLabel.ForeColor = Color.Red;
                this.infoLabel1.ForeColor = Color.Red;
                this.infoLabel2.ForeColor = Color.Red;

                this.infoLabel1.Text = "Error: Not a valid NITRO File";
                if( MarketPlan.errors.Count > 0 ) {
                    this.infoLabel2.Text = (string)MarketPlan.errors[ 0 ];
                }
                else {
                    this.infoLabel2.Text = "";
                }

                UpdateVariantsListView( null );
            }

            // display the appropriate button for error/warning detail display
            if( MarketPlan.errors.Count > 0 ) {
                warningsButton.Text = "Show All Errors";
                warningsButton.BackColor = Color.Red;
                warningsButton.ForeColor = Color.Yellow;
            }
            else if( MarketPlan.warnings.Count > 0 ) {
                warningsButton.Text = "Show Warnings";
                warningsButton.BackColor = Color.Yellow;
                warningsButton.ForeColor = Color.Black;
            }
            if( MarketPlan.errors.Count + MarketPlan.warnings.Count > 0 ) {
                warningsButton.Visible = true;
            }

            this.Cursor = Cursors.Default;
            //ProgressDialog.HideProgressForm();
            WriteStatusToConsole( valid, marketPlan, reader );
        }
        #endregion

        #region Display-Related Methods
        /// <summary>
        /// Updates the variants list to reflect the given MarketPlan object.
        /// </summary>
        /// <param name="plan"></param>
        private void UpdateVariantsListView( MarketPlan plan ) {
            this.listView1.BeginUpdate();
            this.listView1.Items.Clear();
            reportVariantsList = new ArrayList();

            if( plan == null || plan.Variants == null || plan.Variants.Count == 0 ) {
                this.listView1.EndUpdate();
                return;
            }

            // get the data we will display prepared to be sorted
            MarketPlan.VariantInfo[] variantsAndGroups = new MarketPlan.VariantInfo[ plan.Variants.Count + settings.Groups.Count ];
            plan.Variants.CopyTo( variantsAndGroups );

            // add the groups to the end of the variants lists
            int lastiVariantRowNumber = variantsAndGroups[ plan.Variants.Count - 1 ].RowNumber;
            for( int k = 0; k < settings.Groups.Count; k++ ) {
                Settings.GroupInfo ginfo = (Settings.GroupInfo)settings.Groups[ k ];
                MarketPlan.VariantInfo gvinfo = new MarketPlan.VariantInfo( ginfo.Name, k + lastiVariantRowNumber );
                gvinfo.GroupIndex = k;
                gvinfo.IsGroup = true;
                gvinfo.BackColor = ColorForGroup( k );
                gvinfo.GroupExpanded = ginfo.Expanded;
                variantsAndGroups[ k + plan.Variants.Count ] = gvinfo;
            }

            // accumulate the sales units totals of each product
            MarketPlan.Component unitsComp = plan.GetComponent( "UNITS" );

            int[] totalUnits = null;
            double[] sortUnits = null;
            //string[] sortItems = null;
            string[] sortNames = null;
            double scaling = 1;
            string groupSortPrefix = " ";                   // groups sort ahead of all non-groups
            string groupSortSuffix = " AAAAAA";        // group names sort ahead of their grouped items
            if( this.sortReversed ) {
                groupSortSuffix = "zzzzzzzzz";
            }

            totalUnits = new int[ variantsAndGroups.Length ];
            sortUnits = new double[ variantsAndGroups.Length ];
            sortNames = new string[ variantsAndGroups.Length ];
            int v = 0;

            for( ; v < plan.Variants.Count; v++ ) {
                totalUnits[ v ] = 0;

                // if we loaded a units sheet, total the volume over all time steps
                if( unitsComp != null ) {
                    ArrayList items = unitsComp.Data[ v ];
                    for( int i = 0; i < items.Count; i++ ) {
                        MarketPlan.Component.Item item = (MarketPlan.Component.Item)items[ i ];
                        totalUnits[ v ] += (int)(item.Value1);   //sales units values should always be integers (???)
                    }
                }

                // set up the basic (non-grouped item) sort key value
                MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)plan.Variants[ v ];
                if( this.sortByNitroName ) {
                    sortNames[ v ] = vinfo.Name;
                }
                else if( this.sortByMarketSimName ) {
                    sortNames[ v ] = settings.GetMarketSimName( vinfo.Name );
                }
                else if( this.sortByBrand ) {
                    sortNames[ v ] = vinfo.Brand;
                }
                else if( this.sortByVolume ) {
                    sortUnits[ v ] = totalUnits[ v ];
                }
            }

            // accumulate group totals from item totalUnits values
            for( int g = 0; g < settings.Groups.Count; g++, v++ ) {
                totalUnits[ v ] = 0;
                Settings.GroupInfo ginfo = (Settings.GroupInfo)settings.Groups[ g ];
                for( int i = 0; i < ginfo.ItemIndexes.Count; i++ ) {
                    int groupedItemIndex = (int)ginfo.ItemIndexes[ i ];
                    variantsAndGroups[ groupedItemIndex ].BackColor = ColorForGroup( g );
                    totalUnits[ v ] += totalUnits[ groupedItemIndex ];
                }
                ginfo.Volume = totalUnits[ v ];

                if( this.sortByNitroName ) {
                    sortNames[ v ] = groupSortPrefix + ginfo.Name + groupSortSuffix;
                }
                else if( this.sortByMarketSimName ) {
                    sortNames[ v ] = groupSortPrefix + ginfo.Name + groupSortSuffix;
                }
                else if( this.sortByBrand ) {
                    sortNames[ v ] = groupSortPrefix + " " + groupSortSuffix;
                }
                else if( this.sortByVolume ) {
                    sortUnits[ v ] = totalUnits[ v ];       // if sorting by volume, groups show up in their volume slot
                }
            }

            double maxval = 0;
            for( int i2 = 0; i2 < variantsAndGroups.Length; i2++ ) {
                if( totalUnits[ i2 ] > maxval ) {
                    maxval = totalUnits[ i2 ];
                }
            }
            //              double nonGroupValue = maxval * 100;
            //              for( int i3 = 0; i3 < variantsAndGroups.Length; i3++ ) {
            //                  if( variantsAndGroups[ i3 ].IsGroup == false ) {
            ////                      sortUnits[ i3 ] += nonGroupValue;                   // add a large value to non-groups so groups show up last
            //                  }
            //              }

            // set background color for grouped items
            for( int g = 0; g < settings.Groups.Count; g++ ) {
                Settings.GroupInfo ginfo = (Settings.GroupInfo)settings.Groups[ g ];
                for( int i = 0; i < ginfo.ItemIndexes.Count; i++ ) {
                    int groupedItemIndex = (int)ginfo.ItemIndexes[ i ];
                    variantsAndGroups[ groupedItemIndex ].BackColor = ColorForGroup( g );
                    variantsAndGroups[ groupedItemIndex ].Visible = ginfo.Expanded;
                    variantsAndGroups[ groupedItemIndex ].GroupIndex = g;

                    // create a sort key that puts a grouped item inside its group
                    if( this.sortByNitroName ) {
                        sortNames[ groupedItemIndex ] = groupSortPrefix + ginfo.Name + variantsAndGroups[ groupedItemIndex ].Name;
                    }
                    else if( this.sortByMarketSimName ) {
                        sortNames[ groupedItemIndex ] = groupSortPrefix + ginfo.Name + variantsAndGroups[ groupedItemIndex ].Name;
                    }
                    else if( this.sortByBrand ) {
                        sortNames[ groupedItemIndex ] = groupSortPrefix + " ";
                    }
                    else if( this.sortByVolume ) {
                        double sortKey = -1;
                        if( this.sortReversed ) {
                            sortKey = (ginfo.Volume) + ((double)totalUnits[ groupedItemIndex ] / (10.0 * maxval));
                        }
                        else {
                            sortKey = (ginfo.Volume - 1.0) + ((double)totalUnits[ groupedItemIndex ] / (10.0 * maxval));
                        }
                        sortUnits[ groupedItemIndex ] = sortKey;
                    }
                }
            }

            //// sort the items
            bool reverse = this.sortReversed;
            if( this.sortByVolume ) {
                double[] sortUnitsCopy = new double[ sortUnits.Length ];
                Array.Copy( sortUnits, sortUnitsCopy, sortUnits.Length );

                Array.Sort( sortUnits, variantsAndGroups );
                Array.Sort( sortUnitsCopy, totalUnits );

                reverse = !this.sortReversed;         // standard numeric sort is reversed (large to small) relative to alpha sort order
            }
            else {
                string[] sortNamesCopy = new string[ sortNames.Length ];
                Array.Copy( sortNames, sortNamesCopy, sortNames.Length );

                Array.Sort( sortNames, variantsAndGroups );
                Array.Sort( sortNamesCopy, totalUnits );
            }
            if( reverse ) {
                Array.Reverse( totalUnits );
                Array.Reverse( variantsAndGroups );
            }

            // determine the appropriate scale factor for display
            scaling = 1;
            string scalingString = "";

            if( maxval >= 10000000 ) {
                scaling = 10000;
                scalingString = "(10K)";
            }
            else if( maxval >= 1000000 ) {
                scaling = 1000;
                scalingString = "(1K)";
            }
            else if( maxval >= 100000 ) {
                scaling = 100;
                scalingString = "(100)";
            }
            else if( maxval >= 10000 ) {
                scaling = 10;
                scalingString = "(10)";
            }

            string s = String.Format( "Volume {0}", scalingString );
            listView1.Columns[ 3 ].Text = String.Format( "Volume {0}", scalingString );

            // -----------     now  actually add the display items to the ListView itself    ---------------
            for( int i = 0; i < variantsAndGroups.Length; i++ ) {
                variantsAndGroups[ i ].Volume = totalUnits[ i ];
                reportVariantsList.Add( variantsAndGroups[ i ] );          // keep a list of all items for the report
                if( variantsAndGroups[ i ].Visible == false ) {
                    continue;
                }
                ListViewItem row = new ListViewItem();
                row.Tag = variantsAndGroups[ i ];
                if( variantsAndGroups[ i ].IsGroup ) {
                    if( variantsAndGroups[ i ].GroupExpanded == true ) {
                        row.ImageIndex = 1;        // the open-folder icon
                    }
                    else {
                        row.ImageIndex = 0;        // the closed-folder icon
                    }
                }
                else {
                    row.ImageIndex = -1;           // no icon
                }
                row.BackColor = variantsAndGroups[ i ].BackColor;
                if( variantsAndGroups[ i ].IsGroup == false ) {
                    row.SubItems.Add( MarketSimName( variantsAndGroups[ i ].Name ) );
                    row.SubItems.Add( variantsAndGroups[ i ].Name );
                }
                else {
                    // a group
                    row.SubItems.Add( variantsAndGroups[ i ].Name );
                    //row.SubItems.Add( " --- " );
                    row.SubItems.Add( (string)null );
                }
                row.Text = MarketSimName( variantsAndGroups[ i ].Name );

                string unitsStr = " ";
                if( unitsComp != null ) {
                    unitsStr = String.Format( "{0:f0}", totalUnits[ i ] / scaling );
                    //unitsStr = String.Format( "{0:f5}", sortUnits[ i ] );      // debug
                }
                //else {
                //    unitsStr = sortItems[ i ];    //debug
                //}
                row.SubItems.Add( unitsStr );
                listView1.Items.Add( row );
            }
            this.listView1.EndUpdate();
        }

        /// <summary>
        /// Returns the background color for the given group index
        /// </summary>
        /// <param name="groupIndx"></param>
        /// <returns></returns>
        private Color ColorForGroup( int groupIndx ) {
            return this.groupColors[ groupIndx % this.groupColors.Length ];
        }

        /// <summary>
        /// Returns the name the user has defined to replace the given NITRO-file product name.
        /// </summary>
        /// <param name="nitroName"></param>
        /// <returns></returns>
        private string MarketSimName( string nitroName ) {
            string msName = nitroName;
            if( this.settings != null ) {
                msName = settings.GetMarketSimName( nitroName );
            }
            return msName;
        }

        /// <summary>
        ///  Sets a text box with a string that cannot overflow the area.  Overlong strings have the beginning replaced with "...".
        /// </summary>
        /// <param name="fullString"></param>
        /// <param name="labelToSet"></param>
        /// <returns></returns>
        private string SetElidedString( string fullString, Label labelToSet ) {

            Graphics g = Graphics.FromHwnd( this.Handle );
            Font font = labelToSet.Font;
            int maxWidth = labelToSet.Width - 29;

            int wid = (int)(g.MeasureString( fullString, font ).Width);
            if( wid <= maxWidth ) {
                labelToSet.Text = fullString;
                return fullString;      // already short enough
            }
            else {
                string ms = null;
                while( fullString.Length > 10 ) {      // limit is sanity check only
                    fullString = fullString.Substring( 1 );
                    ms = "..." + fullString;
                    int wid2 = (int)(g.MeasureString( ms, font ).Width);
                    if( wid2 <= maxWidth ) {
                        labelToSet.Text = ms;
                        return ms;
                    }
                }
                // just in case
                labelToSet.Text = ms;
                return ms;
            }
        }
        #endregion

        private DateTime startProcessingTime;

        #region Processing Methods
        /// <summary>
        /// Starts the actual processing of the data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goButton_Click( object sender, EventArgs e ) {
            startProcessingTime = DateTime.Now;
            planFileName = String.Format( "{0}.xls", this.marketPlan.Name );
            if( generalFormSettings.NITROFileFolder.Length > 0 ) {
                planFileName = generalFormSettings.NITROFileFolder + "\\" + String.Format( "{0}.xls", this.marketPlan.Name );
            }

            if( this.marketPlan.OkToWriteExcel( planFileName ) ) {      // make sure that we can write the Excel file
                nextComponentToLoad = 0;

                pForm = new ProgressDialog();
                MarketPlan.Component comp0 = (MarketPlan.Component)marketPlan.Components[ 0 ];
                pForm.Info = String.Format( "Processing measure 1 of {0}  ({1})...", marketPlan.Components.Count, comp0.Name );
                pForm.ProgressPercent = 10;
               // pForm.StartPosition = FormStartPosition.CenterParent;
                pForm.Show( this );

                // use a timer to trigger the next phase so the UI can update now
                timer = new System.Windows.Forms.Timer();
                timer.Interval = 1;
                timer.Tick += new EventHandler( ReadNextComponent );
                timer.Start();
            }
        }

        /// <summary>
        /// Reads data for the next component (worksheet) from the NITRO file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadNextComponent( object sender, EventArgs e ){
            this.Cursor = Cursors.WaitCursor;
            timer.Stop();
            if( pForm.Visible == false ) {
                return;
            }

            if( nextComponentToLoad == marketPlan.Components.Count ) {
                // we are done loading the components -- move on
                timer = new System.Windows.Forms.Timer();
                timer.Interval = 1;
                timer.Tick += new EventHandler( WritePlanToFile );
                timer.Start();

                pForm.Info = "Writing Output File...";
                pForm.ProgressPercent = 70;
                return;
            }

            // read the data
            MarketPlan.Component comp = (MarketPlan.Component)marketPlan.Components[ nextComponentToLoad ];
            comp.ReadData( reader );
           // comp.WriteDataToConsole();                 //!!!DEBUG
            nextComponentToLoad += 1;
            pForm.Info = String.Format( "Processing measure {0} of {1}  ({2})...", nextComponentToLoad + 1, marketPlan.Components.Count, comp.Name );
            int ppct = 10 + (int)Math.Round( 50.0 * (double)nextComponentToLoad / (double)marketPlan.Components.Count );    // max is 60% (=10+50)
            pForm.ProgressPercent = ppct;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1;
            timer.Tick += new EventHandler( ReadNextComponent );
            timer.Start();
        }

        /// <summary>
        /// Actually writes the processed data to the output file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WritePlanToFile( object sender, EventArgs e ) {
            timer.Stop();

            reader.Kill();
            reader = null;

            Combiner.CombineItems( this.marketPlan, this.settings );      //!!THIS CHANGES THE MARKET PLAN COMPONENT DATA -- exit is madatory once it is called (after saving results!)

            this.marketPlan.WriteExcel( this.planFileName, this.templateFile, settings, writer );
            this.settings.Save( this.fileName, this.reportVariantsList );
            ReportGenerator.GenerateReport( this.fileName, this.planFileName, this.reportVariantsList, this.marketPlan, this.settings, generalFormSettings, writer );

            pForm.Info = String.Format( "Processing Complete" );
            pForm.Info0 = "Done";
            pForm.ProgressPercent = 100;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler( WrapupWritingPlanToFile );
            timer.Start();
        }

        /// <summary>
        /// Actually writes the processed data to the output file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WrapupWritingPlanToFile( object sender, EventArgs e ) {
            timer.Stop();
            pForm.Close();

            TimeSpan processingTime = DateTime.Now - this.startProcessingTime;

            string logMessage = String.Format( "Processed: {0}\r\nMarketSim File Created: {1}\r\nVarient Count: {2}  Processing Time: {3:f3} s",
                this.fileName, this.planFileName, this.marketPlan.Variants.Count, processingTime.TotalSeconds );

            DataLogger.Log( logMessage );

            Done2 dlg = new Done2( this.marketPlan.Name, this.fileName, this.marketPlan.Variants.Count, processingTime, generalFormSettings );
            dlg.ShowDialog();
            if( generalFormSettings.ShowReportOnCompletion ) {
                ReportGenerator.ShowReport();
            }
            this.Close();
        }
        #endregion

        #region Event Handlers. Saving and Loading.

        /// <summary>
        /// Allows the user to select a NITRO file for which the associated settings file will be loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copySettingsButton_Click( object sender, EventArgs e ) {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Choose NITRO File to Import Settings From";
            fd.Filter = "Excel Files (*.xls)|*.xls";

            DialogResult resp = fd.ShowDialog( this );
            if( resp == DialogResult.OK ) {
                string curFileName = settings.MarketPlanName;
                // clear the current grouped-state setting of the variants
                for( int v = marketPlan.Variants.Count - 1; v >= 0; v-- ) {
                    MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)marketPlan.Variants[ v ];
                    if( vinfo.IsGroup ) {
                        // totally remove any groups
                        vinfo = null;
                        marketPlan.Variants.RemoveAt( v );
                        continue;
                    }
                    vinfo.GroupIndex = -1;
                    vinfo.BackColor = Color.White;
                }

                this.settings = Settings.LoadSettingsForFile( fd.FileName, marketPlan.Variants );
                settings.MarketPlanName = curFileName;    // restore the file name
                this.nameTextBox.Text = settings.MarketPlanName;
                this.normalizeCheckBox.Checked = settings.NormalizeForNIMO;
                this.settings.SetEdited();   // they are edited with respect to the NITRO file they are loaded against now
                this.UpdateVariantsListView( this.marketPlan );
            }
        }

        /// <summary>
        /// Closes the form after saving all settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click( object sender, EventArgs e ) {
            if( settings != null ) {
                settings.Save( this.fileName, this.reportVariantsList );
                Saved2 dlg = new Saved2( this.fileName );
                dlg.ShowDialog();
            }
        }

        /// <summary>
        /// Close the form without saving settings or processing data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click( object sender, EventArgs e ) {
            this.Close();
        }

        /// <summary>
        /// Gets the form ready to start work.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NitroReader_Load( object sender, EventArgs e ) {

            ////!!!DEBUG/TEST!!!
            //Test1 test = new Test1();
            //test.ShowDialog();
            //System.Environment.Exit( 2 );


            this.nitroFileLabel.Text = "    Ready.  Click the \"Select...\" button to choose a NITRO File to process.";
            this.fileName = this.nitroFileLabel.Text;
            this.nitroFileLabel.Font = new Font( "Arial", 8, FontStyle.Bold );
            this.infoLabel1.Text = "";
            this.infoLabel2.Text = "";

            this.nameTextBox.Text = "";
            this.nameTextBox.Enabled = false;
            this.normalizeCheckBox.Enabled = false;

            this.listView1.Items.Clear();
            this.listView1.Enabled = false;

             if( File.Exists( this.templateFile ) == false ) {
                //!!Error no template file!!
                ConfirmForm eForm = new ConfirmForm( "   NITRO Reader Startup Error: Template file not found.\r\nTo correct the problem, reinstall the program or copy the template file from your original installation.   \r\n\r\nTemplate File: " + this.templateFile, 
                    "NITRO Reader Startup Error" );
                eForm.HideCancel();
                eForm.TextColor = Color.Red;
                eForm.ShowDialog( this );
                this.selectButton.Enabled = false;
                this.Close();
            }
        }

        /// <summary>
        /// Shows a list of the current errors (if any) and warnings (if any).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void warningsButton_Click( object sender, EventArgs e ) {
            if( marketPlan != null ) {
                string name = "Errors";
                string title = "Errors:";
                string errs = "";

                // accumulate errors
                if( MarketPlan.errors.Count > 0 ) {
                    if( MarketPlan.warnings.Count > 0 ) {
                        name = "Errors/Warnings";
                        title = "Errors and Warnings:";
                        errs = "Errors:\r\n";
                    }
                    foreach( string s in MarketPlan.errors ) {
                        errs += s + "\r\n";
                    }
                }

                // accumulate warnings
                if( MarketPlan.warnings.Count > 0 ) {
                    if( MarketPlan.errors.Count > 0 ) {
                        errs += "\r\nWarnings:\r\n";
                    }
                    else {
                        // warnings only
                        name = "Warnings";
                        title = "Warnings:";
                    }
                    foreach( string s in MarketPlan.warnings ) {
                        errs += s + "\r\n";
                    }
                }
                InfoForm warningForm = new InfoForm( name, title, errs );
                warningForm.ShowDialog( this );
            }
        }


        /// <summary>
        /// Adjusts the UI in response to a size change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NitroReaderForm_SizeChanged( object sender, EventArgs e ) {
            SetElidedString( fileName, this.nitroFileLabel );
        }

        /// <summary>
        /// Automatically saves the form settings when the form closes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NitroReaderForm_FormClosing( object sender, FormClosingEventArgs e ) {
            if( (this.settings != null) && this.settings.Edited ) {
                ConfirmForm cform = new ConfirmForm( "Do you want to save the settings?", "Settings Changed" );
                DialogResult resp = cform.ShowDialog();
                if( resp == DialogResult.OK ) {
                    this.settings.Save( this.fileName, this.reportVariantsList );
                }
            }
            generalFormSettings.Save( SettingsFile );
            if( reader != null ) {
                reader.Kill();
                reader = null;
            }
        }
        #endregion

        #region Debug/Test
        // test/debug method
        private void WriteStatusToConsole( bool valid, MarketPlan marketPlan, ExcelWriter2 reader ) {
            return;

            if( valid == false ) {
                Console.WriteLine( "- - - - ERROR: Market Plan File Validation Failure.  File: {0}\r\n", reader.ExcelFilename );
            }
            else {
                Console.WriteLine( "- - - - SUCCESS: Market Plan File Validated OK!    File: {0}", reader.ExcelFilename );
                Console.WriteLine( "\r\nStart: {0}     End: {1}", marketPlan.StartDate.ToShortDateString(), marketPlan.EndDate.ToShortDateString() );

                Console.WriteLine( "\r\nComponents: " + marketPlan.Components.Count );
                for( int i = 0; i < marketPlan.Components.Count; i++ ) {
                    MarketPlan.Component comp = (MarketPlan.Component)marketPlan.Components[ i ];
                    Console.WriteLine( "\r\n  Component: {0}    Steps: {1}    Step Size: {2}", comp.Name, comp.StepCount, comp.TimeStep.ToString() );
                    Console.WriteLine( "     Start Date: {0}    End Date: {1}", comp.StartDate.ToShortDateString(), comp.EndDate.ToShortDateString() );
                }

                Console.WriteLine( "\r\nVariants: " + marketPlan.Variants.Count );
                for( int i = 0; i < marketPlan.Variants.Count; i++ ) {
                    MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)marketPlan.Variants[ i ];
                    Console.WriteLine( "   {0}    (row  {1})", vinfo.Name, vinfo.RowNumber );
                }
            }

            // write out errors and/or warnings
            marketPlan.WriteErrorsToConsole( true );
        }
        #endregion

        /// <summary>
        /// Keeps the settings object in sync with the UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void normalizeCheckBox_CheckedChanged( object sender, EventArgs e ) {
            if( settings != null ) {
                settings.NormalizeForNIMO = normalizeCheckBox.Checked;
                settings.SetEdited();
            }
        }

        /// <summary>
        /// Keeps the settings object in sync with the UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nameTextBox_TextChanged( object sender, EventArgs e ) {
            if( settings != null ) {
                settings.MarketPlanName = nameTextBox.Text;
                settings.SetEdited();
                marketPlan.Name = settings.MarketPlanName;
            }
        }

        /// <summary>
        /// Updates the UI in response to a change in the set of selected items in the list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_SelectedIndexChanged( object sender, EventArgs e ) {
            bool onlyUngroupedItemsSelected = false;
            bool onlyGroupedItemsSelected = false;
            bool exactlyOneItemSelected = (listView1.SelectedIndices.Count == 1);

            if( listView1.SelectedIndices.Count > 0 ) {
                ListViewItem item0 = listView1.Items[ (int)listView1.SelectedIndices[ 0 ] ];
                MarketPlan.VariantInfo vinfo0 = (MarketPlan.VariantInfo)item0.Tag;
                if( vinfo0.IsGroup == false ) {
                    if( vinfo0.GroupIndex == -1 ) {
                        onlyUngroupedItemsSelected = true;
                    }
                    else {
                        onlyGroupedItemsSelected = true;
                    }

                    for( int i = 1; i < listView1.SelectedIndices.Count; i++ ) {
                        ListViewItem item = listView1.Items[ (int)listView1.SelectedIndices[ i ] ];
                        MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)item.Tag;
                        if( vinfo.IsGroup ) {
                            onlyUngroupedItemsSelected = false;
                            onlyGroupedItemsSelected = false;
                            break;
                        }
                        else {
                            if( vinfo0.GroupIndex == -1 ) {
                                if( onlyGroupedItemsSelected ) {
                                    onlyGroupedItemsSelected = false;
                                    break;
                                }
                            }
                            else {
                                if( onlyUngroupedItemsSelected ) {
                                    onlyUngroupedItemsSelected = false;
                                    break;
                                }
                            }
                       }
                    }
                }
            }

            this.groupButton.Enabled = onlyUngroupedItemsSelected;
            this.ungroupButton.Enabled = onlyGroupedItemsSelected;
            this.renameButton.Enabled = exactlyOneItemSelected;
        }

        /// <summary>
        /// Opens or closes a group item that the user has double-clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_DoubleClick( object sender, EventArgs e ) {

            ListView sitem = (ListView)sender;
            ListViewItem item = null;
            int selectedIndex = -1;
            if( sitem.SelectedItems != null ) {
                item = (ListViewItem)sitem.SelectedItems[ 0 ];
                selectedIndex = sitem.SelectedIndices[ 0 ];
            }
            if( item == null ) {
                //ignore double-click on a non-item
                return;
            }
            if( item.ImageIndex == -1 ) {
                // double-click on a non-group
//                RenameItem( item );
                item.BeginEdit();
                return;
            }
            MarketPlan.VariantInfo info = (MarketPlan.VariantInfo)item.Tag;
            Settings.GroupInfo ginfo = (Settings.GroupInfo)settings.Groups[ info.GroupIndex ];
            if( item.ImageIndex == 0 ) {
                // the double-clicked item was a closed group -- open it
                ginfo.Expanded = true;
                settings.SetEdited();
                UpdateVariantsListView( this.marketPlan );
                int lastIGrpIndex = (int)Math.Min( selectedIndex + ginfo.ItemIndexes.Count + 3, listView1.Items.Count - 1 );
                if( (lastIGrpIndex >= 0) && (lastIGrpIndex < this.listView1.Items.Count) ) {
                    ListViewItem selItem = this.listView1.Items[ lastIGrpIndex ];
                    selItem.EnsureVisible();
                }
            }
            else if( item.ImageIndex == 1 ) {
                // the double-clicked item was an open group -- close it
                //HideGroup( selectedIndex, ginfo.ItemIndexes.Count, info.GroupIndex );
                ginfo.Expanded = false;
                settings.SetEdited();
                UpdateVariantsListView( this.marketPlan );
                int lastIGrpIndex = (int)Math.Min( selectedIndex + ginfo.ItemIndexes.Count + 3, listView1.Items.Count - 1 );
                if( (lastIGrpIndex >= 0) && (lastIGrpIndex < this.listView1.Items.Count) ) {
                    ListViewItem selItem = this.listView1.Items[ lastIGrpIndex ];
                    selItem.EnsureVisible();
                }
            }
        }

        private void HideGroup( int groupIndex, int subitemCount, int group ) {
            this.listView1.BeginUpdate();
            for( int i = subitemCount - 1; i >= 0;  i-- ) {
                this.listView1.Items.RemoveAt( groupIndex + i + 1 );
            }
            this.listView1.EndUpdate();
        }

        private void RenameItem( ListViewItem item ) {
            MarketSimName ndlg = new MarketSimName( item.SubItems[ 1 ].Text, item.SubItems[ 2 ].Text, this.marketPlan, this.settings );
            DialogResult resp = ndlg.ShowDialog( this );
            if( resp == DialogResult.OK ) {
                // set the marketSimName
                bool ok = settings.SetMarketSimName( ndlg.NameForMarketSim, item.SubItems[ 1 ].Text );
                if( ok ) {
                    UpdateVariantsListView( this.marketPlan );
                }
                else {
                    string msg = String.Format( "     The name {0} has already been used.  Choose a different name.      ", ndlg.NameForMarketSim );
                    ConfirmForm cform = new ConfirmForm( msg, "Name Used Already" );
                    cform.HideCancel();
                    cform.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Allows the user to put the currently selected items into a group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupButton_Click( object sender, EventArgs e ) {
            if( settings.Groups.Count > 0 ) {
                Group2 grpDlg = new Group2( this.listView1, this.marketPlan, this.settings );
                DialogResult resp = grpDlg.ShowDialog( this );
                if( resp == DialogResult.OK ) {
                    // the dialog itself will have already changed the relevant settings values
                    UpdateVariantsListView( this.marketPlan );
                }
            }
            else {
                // there are 0 groups currently defined
                NewGroup ngDlg = new NewGroup( this.marketPlan, this.settings );
                DialogResult resp = ngDlg.ShowDialog( this );
                if( resp == DialogResult.OK ) {
                    Settings.GroupInfo newGroupInfo = new Settings.GroupInfo();
                    newGroupInfo.Name = ngDlg.GroupName;
                    newGroupInfo.Correlation = ngDlg.Correlation;
                    //add the selected items to the new group
                    foreach( ListViewItem litem in this.listView1.SelectedItems ) {
                        MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)litem.Tag;
                        int itemToAddIndx = vinfo.RowNumber;
                        newGroupInfo.ItemIndexes.Add( itemToAddIndx );
                        newGroupInfo.itemNitroNames.Add( vinfo.Name );       // add the nitro name too so we can safely import/load settings
                    }
                    settings.Groups.Add( newGroupInfo );
                    settings.SetEdited();
                    UpdateVariantsListView( this.marketPlan );
                }
            }
        }

        /// <summary>
        /// Allows the user to remove the currently selected items form their group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ungroupButton_Click( object sender, EventArgs e ) {
            Ungroup ungrpDlg = new Ungroup( this.listView1, this.settings );
            DialogResult resp = ungrpDlg.ShowDialog( this );
            if( resp == DialogResult.OK ) {
                // the dialog itself will have already changed the relevant settings values
                UpdateVariantsListView( this.marketPlan );
            }
        }

        /// <summary>
        /// Allows the user to delete a group by selecting it and hitting the Detete key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_KeyDown( object sender, KeyEventArgs e ) {
            if( e.KeyCode == Keys.Delete ) {
                if( listView1.SelectedItems.Count != 1 ) {
                    return;
                }
                ListViewItem litem = (ListViewItem)listView1.SelectedItems[ 0 ];
                MarketPlan.VariantInfo gvinfo = (MarketPlan.VariantInfo)litem.Tag;
                if( gvinfo.IsGroup == false ) {
                    // don't delete non-group items
                    return;
                }
                int groupToDelete = gvinfo.GroupIndex;
                Settings.GroupInfo ginfo = (Settings.GroupInfo)settings.Groups[ groupToDelete ];

                DialogResult resp = DialogResult.None;
                if( ginfo.ItemIndexes.Count == 0 ) {
                    string msg = String.Format( "      OK to delete the \"{0}\" group?      ", ginfo.Name );
                    ConfirmForm cform = new ConfirmForm( msg, "Confirm Group Delete" );
                    resp = cform.ShowDialog();
                }
                else {
                    string msg = String.Format( "      OK to delete the \"{0}\" group?\r\n\r\n      Deleting this group will return the {1} items it contains to the main list.      ",
                        ginfo.Name, ginfo.ItemIndexes.Count );
                    ConfirmForm cform = new ConfirmForm( msg, "Confirm Group Delete" );
                    resp = cform.ShowDialog();
               }
                if( resp == DialogResult.OK ) {
                    settings.Groups.RemoveAt( groupToDelete );
                    // recover any items that were in the deleted group
                    foreach( MarketPlan.VariantInfo vinfo in marketPlan.Variants ) {
                        if( vinfo.GroupIndex == groupToDelete ) {
                            vinfo.GroupIndex = -1;
                            vinfo.BackColor = Color.White;
                        }
                    }
                    UpdateVariantsListView( this.marketPlan );
                }
            }
        }

        /// <summary>
        /// Responds to a double-click on the logo by opening the general-settings dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logoPictureBox_DoubleClick( object sender, EventArgs e ) {
            GeneralSettings sdlg = new GeneralSettings( generalFormSettings );
            DialogResult resp = sdlg.ShowDialog( this );
            if( resp == DialogResult.OK ) {
                // the dialog will have changed the generalFormSettings values at this point
                if( this.marketPlan != null ) {
                    foreach( MarketPlan.Component comp in this.marketPlan.Components ) {
                        if( comp.Name == "%ACV DIST" ) {
                            comp.Awareness = generalFormSettings.DistributionAwareness;
                            comp.Persuasion = generalFormSettings.DistributionPersuasion;
                        }
                        else {
                            comp.Awareness = generalFormSettings.DisplayAwareness;
                            comp.Persuasion = generalFormSettings.DisplayPersuasion;
                        }
                    }
                }
                LoadCustomerIcon( generalFormSettings.CustomerIconFile );
            }
        }

        /// <summary>
        /// Sets the customer logo icon to the image in the named file.
        /// </summary>
        /// <param name="iconPath"></param>
        private void LoadCustomerIcon( string iconPath ) {
            bool loadedOK = false;
            if( (iconPath != null) && File.Exists( iconPath ) ) {
                try {
                    Image newIcon = Image.FromFile( iconPath );
                    this.customerLogoPictureBox.Image = newIcon;
                    loadedOK = true;
                }
                catch( Exception ) {
                    // not a valid Icon file
                }
            }
            if( !loadedOK ) {
                this.customerLogoPictureBox.Image = null;
            }
        }

        /// <summary>
        /// The user can double-click on the customer logo if it is visible -- same as double-clicking the main logo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customerLogoPictureBox_DoubleClick( object sender, EventArgs e ) {
            if( this.customerLogoPictureBox.Image != null ) {
                logoPictureBox_DoubleClick( sender, e );
            }
        }

        private void renameButton_Click( object sender, EventArgs e ) {
            if( this.listView1.SelectedItems.Count > 0 ) {
                ListViewItem item = this.listView1.SelectedItems[ 0 ];
                RenameItem( item );
            }
        }

        /// <summary>
        /// Handles the preparation for puttinng a row into edit mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_BeforeLabelEdit( object sender, LabelEditEventArgs e ) {
            editingNitroName = listView1.Items[ e.Item ].SubItems[ 1 ].Text;
            listView1.Items[ e.Item ].SubItems[ 1 ].Text = "";
        }

        /// <summary>
        /// Handles a row going out of edit mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_AfterLabelEdit( object sender, LabelEditEventArgs e ) {
            if( (e.CancelEdit == false) && (e.Label != null) ) {

                bool ok = NitroReader.Dialogs.MarketSimName.NameIsAvailable( e.Label, this.marketPlan, this.settings );
                if( ok ) {
                    string newMktSimName = e.Label;
                    listView1.Items[ e.Item ].SubItems[ 1 ].Text = newMktSimName;
                    listView1.Items[ e.Item ].Text = newMktSimName;

                    MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)listView1.Items[ e.Item ].Tag;
                    if( vinfo.IsGroup == false ) {
                        settings.SetMarketSimName( newMktSimName, listView1.Items[ e.Item ].SubItems[ 2 ].Text );
                    }
                    else {
                        settings.RenameGroup( newMktSimName, editingNitroName );
                    }
                }
                else {
                    string msg = String.Format( "The name {0} has already been used.  Choose a different name.", e.Label );
                    ConfirmForm cform = new ConfirmForm( msg, "Name Used Already" );
                    cform.HideCancel();
                    cform.ShowDialog();
                    listView1.Items[ e.Item ].SubItems[ 1 ].Text = editingNitroName;
                    listView1.Items[ e.Item ].Text = editingNitroName;
                }

            }
            else {
                // edit cancelled
                listView1.Items[ e.Item ].SubItems[ 1 ].Text = editingNitroName;
                listView1.Items[ e.Item ].Text = editingNitroName;
            }
        }

        #region   Experimental Stuff
        //double xofstp = 20;       // % from LH side
        //double yofstp = 10;        // % from bottom

        //double theta1 = 29;      // degrees
        //double theta2 = 7;        // degrees

        //double rot = -3;             // degrees


        private void NitroReaderForm_Paint( object sender, PaintEventArgs e ) {
            //Graphics g = e.Graphics;
            ////int h = e.ClipRectangle.Height;
            ////int w = e.ClipRectangle.Width;
            //int h = this.Height;
            //int w = this.Width;

            ////double cx = xofst;
            ////double cy = e.ClipRectangle.Height - yofst;
            //double cx = (xofstp / 100) * w;
            //double cy = h * (1 - (yofstp / 100));

            //double a1 = (theta1 + rot) * Math.PI / 180;
            //double a2 = (theta2 + rot) * Math.PI / 180;

            //double p1y = cy - (cx / Math.Tan( a1 ));
            //double p2y = cy + cx * Math.Tan( a2 );

            //double p3x = cx + (h - cy) * Math.Tan( a1 );
            //double p4y = cy - (w - cx) * Math.Tan( a2 );

            //Point p0 = new Point( (int)Math.Round( cx ), (int)Math.Round( cy ) );
            //Point p1 = new Point( 0, (int)Math.Round( p1y ) );
            //Point p2 = new Point( 0, (int)Math.Round( p2y ) );
            //Point p3 = new Point( (int)Math.Round( p3x ), h );
            //Point p4 = new Point( w, (int)Math.Round( p4y ) );
            //Point p5 = new Point( w, h );

            //Brush b = new SolidBrush( Color.FromArgb( 0, 105, 170 ) );
            //Brush b2 = new SolidBrush( Color.FromArgb( 175, 189, 32 ) );

            //Point[] region1 = new Point[ 3 ];
            //Point[] region2 = new Point[ 4 ];

            //region1[ 0 ] = p1;
            //region1[ 1 ] = p0;
            //region1[ 2 ] = p2;

            //region2[ 0 ] = p4;
            //region2[ 1 ] = p0;
            //region2[ 2 ] = p3;
            //region2[ 3 ] = p5;

            ////g.FillRectangle( b, 0, (int)Math.Round( cy ), (int)Math.Round( cx ), h - (int)Math.Round( cy ) );

            //g.FillPolygon( b, region1 );
            //g.FillPolygon( b2, region2 );
        }

        private void groupBox3_Paint( object sender, PaintEventArgs e ) {

        }
        #endregion

        /// <summary>
        /// Responds to a click on a column heading by sorting the display appropriately
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_ColumnClick( object sender, ColumnClickEventArgs e ) {
            bool changed = true;

            if( e.Column == this.nitroNameColumn ) {
                if( this.sortByNitroName == false ) {
                    // change to now sort by nitro name
                    this.sortByNitroName = true;
                    this.sortByMarketSimName = false;
                    this.sortByVolume = false;
                    this.sortByBrand = false;
                    this.sortReversed = false;
                }
                else {
                    // reverse sort sequence
                    this.sortReversed = !this.sortReversed;
                }
            }
            else if( e.Column == this.marketSimNameColumn ) {
                if( this.sortByMarketSimName == false ) {
                    // change to now sort by MarketSim name
                    this.sortByNitroName = false;
                    this.sortByMarketSimName = true;
                    this.sortByVolume = false;
                    this.sortByBrand = false;
                    this.sortReversed = false;
                }
                else {
                    // reverse sort sequence
                    this.sortReversed = !this.sortReversed;
                }
            }
            else if( e.Column == this.volumeColumn ) {
                if( this.sortByVolume == false ) {
                    // change to now sort by volume
                    this.sortByNitroName = false;
                    this.sortByMarketSimName = false;
                    this.sortByVolume = true;
                    this.sortByBrand = false;
                    this.sortReversed = false;
                }
                else {
                    // reverse sort sequence
                    this.sortReversed = !this.sortReversed;
                }
            }
            else if( e.Column == this.brandColumn ) {
                if( this.sortByBrand == false ) {
                    // change to now sort by brand name
                    this.sortByNitroName = false;
                    this.sortByMarketSimName = false;
                    this.sortByVolume = false;
                    this.sortByBrand = true;
                    this.sortReversed = false;
                }
                else {
                    // reverse sort sequence
                    this.sortReversed = !this.sortReversed;
                }
            }
            else {
                // an inactive column heading was cliicked
                changed = false;
                Console.WriteLine( "Unknown column Clicked: " + e.Column );
            }

            if( changed ) {
                // set the sort-key icon
                for( int c = 0; c < listView1.Columns.Count; c++ ) {
                    ColumnHeader ch = listView1.Columns[ c ];
                    ch.ImageIndex = -1;
                }
                bool sortUp = this.sortReversed;
                if( this.sortByVolume ) {
                    sortUp = !sortUp;
                }
                if( sortUp ) {
                    listView1.Columns[ e.Column ].ImageIndex = 3;
                }
                else {
                    listView1.Columns[ e.Column ].ImageIndex = 2;
                }

                this.UpdateVariantsListView( this.marketPlan );
            }
        }

        private void listView1_ColumnWidthChanging( object sender, ColumnWidthChangingEventArgs e ) {
            if( e.ColumnIndex == 0 ) {
                e.Cancel = true;
                e.NewWidth = column0Width;       
            }
        }
    }
}