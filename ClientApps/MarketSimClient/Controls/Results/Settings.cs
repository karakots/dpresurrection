using System;

using System.IO.IsolatedStorage;

using System.Runtime.Serialization;

using System.Runtime.Serialization.Formatters;

using System.Runtime.Serialization.Formatters.Binary;

using System.Collections;



namespace Results
{
	[SerializableAttribute]
	public class SummaryOption
	{
		public string[] checkedSummaryItems;

		public string[] checkedTotalsItems;

		public SummaryOption()
		{
			checkedSummaryItems = null;

			checkedTotalsItems = null;
		}
	}



	[SerializableAttribute]
	public class GraphOption
	{
		public bool productPerGraph;

		public bool segmentPerGraph;

		public bool sumSegments;

		public bool sumProducts;


		// public string[] tokens;
		public GraphOption()
		{
			// default

			productPerGraph = true;

			segmentPerGraph = false;

			sumSegments = true;

			sumProducts = false;

			// tokens = null;
		}

	}

	/// <summary>
	/// Summary description for Settings.
	/// </summary>

	[SerializableAttribute]
	public class Settings
    {
        #region fields for saving
        private NamedSettings[] namedSettings;
        private string currentNamedSetting;

        //private GraphOption graphOption;

        //private SummaryOption summaryOption;
        #endregion

        #region public access to saved fields
        static public NamedSettings[] NamedResultsSettings {
            get { return localSettings.namedSettings; }
            set { localSettings.namedSettings = value; }
        }

        static public string CurrentNamedSetting {
            get { return localSettings.currentNamedSetting; }
            set { localSettings.currentNamedSetting = value; }
        }
        
        //static public SummaryOption DefaultSummaryOption
        //{
        //    get
        //    {
        //        return localSettings.summaryOption;
        //    }

        //    set
        //    {
        //        localSettings.summaryOption = value;
        //    }
        //}

        //static public GraphOption DefaultGraphOption
        //{
        //    get
        //    {
        //        return localSettings.graphOption;
        //    }

        //    set
        //    {
        //        localSettings.graphOption = value;
        //    }
        //}

        #endregion

        static Settings localSettings = null;

		static Settings()
		{
			localSettings = new Settings();
		}



		// this is a singleton
		private Settings()
		{
            //graphOption = null;

            //summaryOption = null;
            
            namedSettings = new NamedSettings[ 0 ];
		}



		public static void Read()
		{
			// try to read settings first
			try
			{
				IsolatedStorageFile getStore = IsolatedStorageFile.GetUserStoreForAssembly();

				using( System.IO.Stream getStream = new IsolatedStorageFileStream("Results.txt", System.IO.FileMode.Open, getStore) )
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
			IsolatedStorageFile sendStore = IsolatedStorageFile.GetUserStoreForAssembly();

			using( System.IO.Stream sendStream = new IsolatedStorageFileStream("Results.txt", System.IO.FileMode.Create, sendStore) )
			{
				IFormatter formatter = new BinaryFormatter();

				formatter.Serialize(sendStream, localSettings);
			}
		}

	}

}

