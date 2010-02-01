using System;
using System.Collections.Generic;
using System.Text;

namespace Results
{
    [SerializableAttribute]
    public class NamedSettings
    {
        private string settingsName;
        private string settingsDescription;
        private string controllerClass;
        private string rendererClass;

        private string[] settingNames;
        private object[] settingValues;

        public NamedSettings() {
        }

        public NamedSettings( NamedSettings settingsToCopy, string newName, string newDescription ) : this() {
            this.SettingsName = newName;
            this.SettingsDescription = newDescription;

            if( settingsToCopy.settingNames == null ) {
                settingsToCopy.settingNames = new string[ 0 ];
                settingsToCopy.settingValues = new object[ 0 ];
            }

            settingNames = new string[ settingsToCopy.settingNames.Length ];
            settingValues = new object[ settingsToCopy.settingNames.Length ];

            for( int i = 0; i < settingsToCopy.settingNames.Length; i++ ) {
                settingNames[ i ] = settingsToCopy.settingNames[ i ];
                settingValues[ i ] = settingsToCopy.settingValues[ i ];
            }
        }

        public string SettingsName {
            get { return settingsName; }
            set { settingsName = value; }
        }

        public string SettingsDescription {
            get { return settingsDescription; }
            set { settingsDescription = value; }
        }

        public string ControllerClass {
            get { return controllerClass; }
            set { controllerClass = value; }
        }

        public string RendererClass {
            get { return rendererClass; }
            set { rendererClass = value; }
        }

        public string[] SettingNames {
            get { return settingNames; }
            set { settingNames = value; }
        }

        public object[] SettingValues {
            get { return settingValues; }
            set { settingValues = value; }
        }

        public void SetSetting( string settingName, object settingValue ) {

            if( settingNames == null ) {
                // this is the first setting
                settingNames = new string[ 1 ];
                settingNames[ 0 ] = settingName;
                settingValues = new object[ 1 ];
                settingValues[ 0 ] = settingValue;
                return;
            }

            for( int i = 0; i < settingNames.Length; i++ ) {
                if( settingNames[ i ] == settingName ) {
                    SettingValues[ i ] = settingValue;
                    return;
                }
            }

            // create a new setting
            Array.Resize( ref settingNames, settingNames.Length + 1 );
            Array.Resize( ref settingValues, settingValues.Length + 1 );

            settingNames[ settingNames.Length - 1 ] = settingName;
            settingValues[ settingNames.Length - 1 ] = settingValue;
            return;    
        }

        public object GetSetting( string settingName ) {
            if( settingNames != null ) {
                for( int i = 0; i < settingNames.Length; i++ ) {
                    if( settingNames[ i ] == settingName ) {
                        return SettingValues[ i ];
                    }
                }
            }
            return null;         // setting name not in list
        }

        public string GetString( string settingName ) {
            return (string)GetSetting( settingName );
        }

        public bool GetBool( string settingName ) {
            if( GetSetting( settingName ) != null ) {
                return (bool)GetSetting( settingName );
            }
            else {
                return false;
            }
        }

        public double GetDouble( string settingName ) {
            if( GetSetting( settingName ) != null ) {
                return (double)GetSetting( settingName );
            }
            else {
                return 0.0;
            }
        }

        public int GetInt( string settingName ) {
            if( GetSetting( settingName ) != null ) {
                return (int)GetSetting( settingName );
            }
            else {
                return 0;
            }
        }

        public DateTime GetDateTime( string settingName ) {
            if( GetSetting( settingName ) != null ) {
                return (DateTime)GetSetting( settingName );
            }
            else {
                return DateTime.MinValue;
            }
        }

        public void SetString( string settingName, string settingValue ) {
            SetSetting( settingName, settingValue );
        }

        public void SetBool( string settingName, bool settingValue ) {
            SetSetting( settingName, settingValue );
        }

        public void SetDouble( string settingName, double settingValue ) {
            SetSetting( settingName, settingValue );
        }

        public void SetInt( string settingName, int settingValue ) {
            SetSetting( settingName, settingValue );
        }

        public void SetDateTime( string settingName, DateTime settingValue ) {
            SetSetting( settingName, settingValue );
        }
    }
}
