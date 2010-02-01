using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace NitroReader
{
    public class GeneralSettingsValues
    {
        public string NITROFileFolder;
        public string CustomerIconFile;
        public double DisplayAwareness;
        public double DisplayPersuasion;
        public double DistributionAwareness;
        public double DistributionPersuasion;
        public bool ShowReportOnCompletion;
        public bool TextReport;

        public GeneralSettingsValues() {
            NITROFileFolder = null;
            CustomerIconFile = null;
            DisplayAwareness = 0.25;                      //magic default value
            DisplayPersuasion = 0.0;                        //magic default value
            DistributionAwareness = 0.03;                //magic default value
            DistributionPersuasion = 0.20;                //magic default value
            ShowReportOnCompletion = true;
            TextReport = false;
        }

        #region Saving and Loading
        /// <summary>
        /// Factory method for getting a Settings object from a disk file, given the corresponding NITRO File path.
        /// </summary>
        /// <param name="nitroFilePath"></param>
        /// <returns></returns>
        public static GeneralSettingsValues Load( string filePath ) {
            Console.WriteLine( "GeneralSettingsValues.Load()..." );

            GeneralSettingsValues gsettings = null;
            if( File.Exists( filePath ) ) {
                FileStream fs = null;
                try {
                    XmlSerializer serializer = new XmlSerializer( typeof( GeneralSettingsValues ) );
                    fs = new FileStream( filePath, FileMode.Open, FileAccess.Read );
                    gsettings = (GeneralSettingsValues)serializer.Deserialize( fs );
                }
                catch( Exception ) {
                    // the version of the settings file is likely incompatible (old)!
                    gsettings = new GeneralSettingsValues();
                }
                finally {
                    if( fs != null ) {
                        fs.Close();
                    }
                }
            }
            else {
                gsettings = new GeneralSettingsValues();
            }
            return gsettings;
        }

        /// <summary>
        /// Saves the settings for the current NITRO file's processing.
        /// </summary>
        public string Save( string filePath ) {
            Console.WriteLine( "GeneralSettingsValues.Save()..." );

            XmlSerializer serializer = new XmlSerializer( typeof( GeneralSettingsValues ) );
            StreamWriter sw = new StreamWriter( new FileStream( filePath, FileMode.Create, FileAccess.Write ) );
            serializer.Serialize( sw, this );
            sw.Flush();
            sw.Close();

            return filePath;
        }
        #endregion
    }
}
