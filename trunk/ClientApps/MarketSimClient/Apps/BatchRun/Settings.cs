using System;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace BatchRun
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
	[SerializableAttribute]
	public class Settings
	{
		// fields for saving
		private string userCode = null;
		private string licenseKey = null;
		private string msServerDirectory;
		private string simEngine = null;

		static public string SimEngine
		{
			get
			{
				return localSettings.simEngine;
			}

			set
			{
				localSettings.simEngine = value;
			}
		}

		static public string DefaultServerDirectory
		{
			get
			{
				return localSettings.msServerDirectory;
			}

			set
			{
				localSettings.msServerDirectory = value;
			}
		}

		static public string UserCode
		{
			set
			{
				localSettings.userCode = value;
			}

			get
			{
				return localSettings.userCode;
			}
		}

		static public string LicenseKey
		{
			set
			{
				localSettings.licenseKey = value;
			}

			get
			{
				return localSettings.licenseKey;
			}
		}

		static Settings localSettings = null;
		static Settings()
		{
			localSettings = new Settings();
		}

		// this is a singleton
		private Settings()
		{
			userCode = null;
			licenseKey = null;
			msServerDirectory = null;
			simEngine = null;
		}

		public static void Read()
		{
			// try to read settings first
			try
			{
				IsolatedStorageFile getStore =
					IsolatedStorageFile.GetUserStoreForAssembly();

				using( System.IO.Stream getStream = new IsolatedStorageFileStream("BatchRun.txt",
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

			using( System.IO.Stream sendStream = new IsolatedStorageFileStream("BatchRun.txt",
					   System.IO.FileMode.Create, sendStore) )
			{
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(sendStream, localSettings);
			}
		}
	}
}
