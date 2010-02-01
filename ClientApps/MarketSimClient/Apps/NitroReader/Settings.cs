using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace NitroReader
{
    /// <summary>
    /// Settings encapsulates the settings that are associated with a particular NITRO file.  
    /// The settings can be saved to (pr loaded from) a disk file using an XML serializer.
    /// </summary>
    [XmlInclude( typeof( NitroReader.Settings.GroupInfo ) ), XmlInclude( typeof( NitroReader.Settings.NameInfo ) )]
    public class Settings
    {
        public bool NormalizeForNIMO = false;
        public string MarketPlanName = "";
        public ArrayList Groups;                        //a list of GroupInfo objects
        public ArrayList MarketSimNames;          //a list of NameInfo objects
        public ArrayList Brands;                         //a list of BrandInfo objects

        private bool edited;                              // not serialized

        /// <summary>
        /// Creates a new settings object.
        /// </summary>
        public Settings() {
            Groups = new ArrayList();
            MarketSimNames = new ArrayList();
            Brands = new ArrayList();
            edited = false;
        }

        /// <summary>
        /// Sets the flag indicating the settings are edited.
        /// </summary>
        public void SetEdited() {
            edited = true;
        }

        /// <summary>
        /// Clears the flag indicating the settings are edited.
        /// </summary>
        public void ClearEdited() {
            edited = false;
        }

        /// <summary>
        /// Returns true if the settings have been edited.
        /// </summary>
        public bool Edited {
            get {
                return edited;
            }
        }

        /// <summary>
        /// Returns the MarketSim name for the given NITRO name.
        /// </summary>
        /// <param name="nitroName"></param>
        /// <returns></returns>
        public string GetMarketSimName( string nitroName ) {
            string s = nitroName;
            foreach( NameInfo ninfo in this.MarketSimNames ) {
                if( ninfo.NitroName == nitroName ) {
                    s = ninfo.MarketSimName;
                    break;
                }
            }
            return s;
        }

        /// <summary>
        /// Sets the MarketSim name for a given NITRO name.
        /// </summary>
        /// <param name="marketSimName"></param>
        /// <param name="nitroName"></param>
        /// <returns>false if the given MarketSim name is already in use</returns>
        public bool SetMarketSimName( string marketSimName, string nitroName ) {
            foreach( NameInfo ninfo in this.MarketSimNames ) {
                if( ninfo.MarketSimName == marketSimName ) {
                    // this name is already being used
                    return false;
                }
            }
            foreach( NameInfo ninfo in this.MarketSimNames ) {
                if( ninfo.NitroName == nitroName ) {
                    // this name is already mapped to a MarketSim name
                    ninfo.MarketSimName = marketSimName;
                    edited = true;
                    return true;
                }
            }

            NameInfo newNameInfo = new NameInfo( marketSimName, nitroName );
            this.MarketSimNames.Add( newNameInfo );
            edited = true;
            return true;
        }

        /// <summary>
        /// Changes the name of a group.
        /// </summary>
        /// <param name="newGroupName"></param>
        /// <param name="oldGroupName"></param>
        /// <returns></returns>
        public bool RenameGroup( string newGroupName, string oldGroupName ) {
            if( newGroupName == oldGroupName ) {
                return true;
            }

            foreach( GroupInfo ginfo in this.Groups ) {
                if( ginfo.Name == oldGroupName ) {
                    ginfo.Name = newGroupName;
                    edited = true;
                    return true;
                }
            }
            // no match for the old name was found if we come here
            return false;
        }

        #region Saving and Loading
        /// <summary>
        /// Factory method for getting a Settings object from a disk file, given the corresponding NITRO File path.
        /// </summary>
        /// <param name="nitroFilePath"></param>
        /// <returns></returns>
        public static Settings LoadSettingsForFile( string nitroFilePath, ArrayList variants ) {
            Console.WriteLine( "Settings.LoadSettingsForFile()..." );

            string settingsFilePath = GetSettingsFilePath( nitroFilePath );

            Settings settings = null;
            if( File.Exists( settingsFilePath ) ) {
                FileStream fs = null;
                try {
                    XmlSerializer serializer = new XmlSerializer( typeof( Settings ) );
                    fs = new FileStream( settingsFilePath, FileMode.Open, FileAccess.Read );
                    settings = (Settings)serializer.Deserialize( fs );

                    settings.ConvertGroupNamesToIndexes( variants );
                    settings.RemoveUnusedNameMappings( variants );
                }
                catch( Exception) {
                    // the version of the settings file is likely incompatible (old)!
                    settings = new Settings();
                }
                finally {
                    if( fs != null ) {
                        fs.Close();
                    }
                }
            }
            else {
                settings = new Settings();
            }
            return settings;
        }

        /// <summary>
        /// Saves the settings for the current NITRO file's processing.
        /// </summary>
        public string Save( string nitroFilePath, ArrayList variants ) {
            Console.WriteLine( "Settings.Save()..." );

            //be sure we can apply the saved settings to a different NITRO file
            //this.ConvertGroupIndexesToNames( variants );

            string settingsFilePath = GetSettingsFilePath( nitroFilePath );

            XmlSerializer serializer = new XmlSerializer( typeof( Settings ) );
            StreamWriter sw = new StreamWriter( new FileStream( settingsFilePath, FileMode.Create, FileAccess.Write ) );
            serializer.Serialize( sw, this );
            sw.Flush();
            sw.Close();
            edited = false;

            return settingsFilePath;
        }

        private static string GetSettingsFilePath( string nitroFilePath ) {
            FileInfo info = new FileInfo( nitroFilePath );
            string s = nitroFilePath.Substring( 0, nitroFilePath.Length - info.Extension.Length ) + ".nrs";
            if( info.DirectoryName == null || info.DirectoryName.Length == 0 ) {
                s = System.Environment.CurrentDirectory + "\\" + s;
            }
            return s;
        }
        #endregion

        ///// <summary>
        ///// Call this method just before saving settings to a file. Makes the group specs importable for other NITRO files.
        ///// </summary>
        ///// <remarks>This creates group item references that are independent of the NITRO file</remarks>
        //private void ConvertGroupIndexesToNames( ArrayList variants ) {
        //    Console.WriteLine( "ConvertGroupIndexesToNames()..." );
        //    foreach( GroupInfo ginfo in this.Groups ) {
        //        Console.WriteLine( " Group: {0}", ginfo.Name );
        //        ginfo.itemNitroNames = new ArrayList();
        //        foreach( int itemIndex in ginfo.ItemIndexes ) {
        //            MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)variants[ itemIndex ];
        //            string itemNitroName = vinfo.Name;
        //            ginfo.itemNitroNames.Add( itemNitroName );
        //            Console.WriteLine( "  index={0} saved as \"{1}\"", itemIndex, vinfo.Name );
        //        }
        //    }
        //}

        /// <summary>
        /// Call this method just after loading settings from a file.
        /// </summary>
        /// <remarks>This creates group item references (indexes) specific for this file by using the NITRO names in the saved group settings.
        /// If a group ends up having zero items, it is deleted.</remarks>
        public void ConvertGroupNamesToIndexes( ArrayList variants ) {
            Console.WriteLine( "ConvertGroupNamesToIndexes()..." );
            foreach( GroupInfo ginfo in this.Groups ) {
                Console.WriteLine( " Group: {0}", ginfo.Name );
                if( ginfo.itemNitroNames.Count == 0 ) {
                    Console.WriteLine( "  no NITRO names specified (old rev) -- using index values from settings file" );
                    continue;    // backwards compatibility - keep group indexes if there is no name info for grouped items (???!!!)
                }
                ginfo.ItemIndexes = new ArrayList();                              // we will replace these values
                foreach( string itemName in ginfo.itemNitroNames ) {
                    for( int indx = 0; indx < variants.Count; indx++ ) {
                        MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)variants[ indx ];
                        if( vinfo.Name == itemName ) {
                            ginfo.ItemIndexes.Add( indx );
                            Console.WriteLine( "  restoring \"{0}\" as index={1}", vinfo.Name, indx );
                            break;
                        }
                    }
                }
            }
            for( int g = this.Groups.Count - 1; g >= 0; g-- ) {
                GroupInfo ginfo = (GroupInfo)this.Groups[ g ];
                if( ginfo.ItemIndexes.Count == 0 ) {
                    Console.WriteLine( " Removing empty group: {0} ", ginfo.Name );
                    this.Groups.RemoveAt( g );
                }
            }
        }

        /// <summary>
        /// Call this method just after loading settings from a file.  Removes unused NameInfo objects.
        /// </summary>
        public void RemoveUnusedNameMappings( ArrayList variants ) {
        }

        /// <summary>
        /// GroupInfo contains the information needed to describe a group of products.
        /// </summary>
        public class GroupInfo
        {
            public string Name;
            public double Volume;
            public ArrayList ItemIndexes;
            public ArrayList itemNitroNames;    // used only when applying settings from one NITRO file to a different file 
            public bool Expanded;
            public double Correlation;

            // needed for serialization
            public GroupInfo()
                : this( "group", 0 ) {
            }

            public GroupInfo( string name, double volume ) {
                this.Name = name;
                this.Volume = volume;
                this.ItemIndexes = new ArrayList();
                this.itemNitroNames = new ArrayList();
                this.Expanded = true;
                this.Correlation = 1.0;
            }
        }

        /// <summary>
        /// NameInfo comtains the infomrtion needed to map a NITRO product name to a MarketSim product name.
        /// </summary>
        /// <remarks>Also contains Brand and Channel info which is used for saving/loading.</remarks>
        public class NameInfo
        {
            public string NitroName;
            public string MarketSimName;
            public string Brand;
            public string Channel;

            // needed for serialization
            public NameInfo() {
            }

            public NameInfo( string marketSimName, string nitroName ) {
                this.MarketSimName = marketSimName;
                this.NitroName = nitroName;
            }
        }
        
        /// <summary>
        /// BrandInfo contains the information needed to describe a brand
        /// </summary>
        public class BrandInfo
        {
            public string Brand;

            /// <summary>
            /// The list of products that are known to have been of this brand.  Only used when appiying one NITRO file's settings to another file.
            /// </summary>
            public ArrayList BrandProductNitroNames;

            public BrandInfo( string marketSimName, string nitroName ) {
                this.BrandProductNitroNames = new ArrayList();
            }
        }
    }
}
