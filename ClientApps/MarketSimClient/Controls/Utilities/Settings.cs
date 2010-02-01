using System;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace MarketSimSettings
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
	[SerializableAttribute]
	public class Settings<T> where T : new()
	{
        static Settings<T> localSettings = null;
        static private string settingsFilename = null;
        static private bool localStore = false;

        static public T Value
        {
            get
            {
                return localSettings.vals;
            }
        }

        
        // uses local users store on machine
        static public bool Read( string fileName )
        {
            settingsFilename = fileName;
            bool settingsLoadedFromFile = false;

            // try to read settings first
            try
            {
                IsolatedStorageFile getStore =
                    IsolatedStorageFile.GetUserStoreForAssembly();

                using (System.IO.Stream getStream = new IsolatedStorageFileStream(settingsFilename,
                           System.IO.FileMode.Open, getStore))
                {
                    IFormatter formatter = new BinaryFormatter();
                    localSettings = (Settings<T>) formatter.Deserialize(getStream);
                    settingsLoadedFromFile = true;
                }
            }
            catch (Exception)
            {
                localSettings = new Settings<T>();
            }
            return settingsLoadedFromFile;
        }

        // use this if the settings should be global apply for all users
        static public void ReadLocal(string fileName)
		{
            settingsFilename = fileName;
            localStore = true;

			try
			{
				using(System.IO.FileStream fi = new System.IO.FileStream(settingsFilename, System.IO.FileMode.Open))
				{
					IFormatter formatter = new BinaryFormatter();
                    localSettings = (Settings<T>) formatter.Deserialize(fi);
				}
				
			}
			catch(Exception)
			{
				// file did not exist - try to read setting this way
                localSettings = new Settings<T>();
                return;
			}
		}

        static public void Save()
        {
            if (localStore)
            {
                SaveLocal();
            }
            else
            {
                IsolatedStorageFile sendStore = IsolatedStorageFile.GetUserStoreForAssembly();

                using (System.IO.Stream sendStream = new IsolatedStorageFileStream(settingsFilename,
                           System.IO.FileMode.Create, sendStore))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(sendStream, localSettings);
                }
            }
        }

        static private void SaveLocal()
		{
			try
			{
				using(System.IO.FileStream fi = new System.IO.FileStream(settingsFilename, System.IO.FileMode.Create))
				{
					IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fi, localSettings);
				}
			}
			catch(Exception) {}
		}

        private T vals = new T();

        Settings(){}

        static Settings()
        {
            localSettings = new Settings<T>();
        }
	}
}