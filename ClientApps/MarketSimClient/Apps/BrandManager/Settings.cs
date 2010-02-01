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
	public abstract class Settings
	{
		static private string settingsFilename;
		static protected Settings initialize()
		{
			return null;
		}

		static protected Settings localSettings = null;

		// derived class should create a localSettings of proper type
//		static MySettings()
//		{
//			localSettings = new MySettings();
//		}
//
//		// this is a singleton
//		private MySettings()
//		{
//			// instantiate default values for settings
//		}

		public static void Read(string filename)
		{
			settingsFilename = filename;

			// try to read settings first
			try
			{
				IsolatedStorageFile getStore =
					IsolatedStorageFile.GetUserStoreForAssembly();

				using( System.IO.Stream getStream = new IsolatedStorageFileStream(settingsFilename,
						   System.IO.FileMode.Open, getStore) )
				{
					IFormatter formatter = new BinaryFormatter();
					localSettings = (Settings) formatter.Deserialize(getStream);
				}
			}
			catch(Exception)
			{
			}
		}

		public static void Save()
		{
			IsolatedStorageFile sendStore =
				IsolatedStorageFile.GetUserStoreForAssembly();

			using( System.IO.Stream sendStream = new IsolatedStorageFileStream(settingsFilename,
					   System.IO.FileMode.Create, sendStore) )
			{
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(sendStream, localSettings);
			}
		}
	}
}