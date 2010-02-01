using System;
using System.Collections;
using System.Text;

using DataImporter.ImportSettings;

namespace DataImporter
{
    public class FileSettings
    {
        private string sourcePath;
        private bool allWorksheetsSimilar;
        private ArrayList worksheetSettingsList;

        public string SourcePath {
            get { return sourcePath; }
            set { sourcePath = value; }
        }

        public bool AllWorksheetsSimilar {
            get { return allWorksheetsSimilar; }
            set { allWorksheetsSimilar = value; }
        }

        public ArrayList WorksheetSettings {
            get { return worksheetSettingsList; }
            set { worksheetSettingsList = value; }
        }

        public WorksheetSettings CommonWorksheetSettings {
            get {
                if( allWorksheetsSimilar == true && worksheetSettingsList.Count > 0 ) {
                    return (WorksheetSettings)worksheetSettingsList[ 0 ];
                }
                else {
                    return null;
                }
            }
        }

        public FileSettings( string sourcePath ) {
            this.sourcePath = sourcePath;
            allWorksheetsSimilar = true;
            worksheetSettingsList = new ArrayList();
        }

        public WorksheetSettings GetWorksheetSettings( string sheetName ) {
            foreach( WorksheetSettings ws in worksheetSettingsList ) {
                if( ws.SheetName == sheetName ) {
                    // found the named sheet
                    return ws;
                }
            }
            // no match
            return null;
        }
    }
}
