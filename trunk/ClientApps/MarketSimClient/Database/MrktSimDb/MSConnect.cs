using System;

using System.IO;

using System.Diagnostics;



using MrktSimDb;



namespace MrktSimDb
{
	/// <summary>
	/// Summary description for MSConnect.
	/// </summary>

	public class MSConnect
	{
        /// <summary>
        /// converts if needed
        /// </summary>
        /// <param name="startUpPath"></param>
        /// <returns> error if some problem in conversion </returns>
        public static string CheckConversion(string startUpPath, out string newConnectFile)
        {
            newConnectFile = null;

            string connectDir = startUpPath + @"\connect\";

            if (!Directory.Exists(connectDir))
            {
                // create the directory
                try
                {
                    Directory.CreateDirectory(connectDir);
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }

            string[] names = Directory.GetFiles(connectDir);

            if (names.Length > 0)
            {
                return null;
            }

            string origConnectFile = startUpPath + @"\" + "mrktsim.udl";

            FileInfo fi = new FileInfo(origConnectFile);

            if (fi.Exists)
            {
                // convert
                newConnectFile = connectDir + connectFileBaseName + ".udl";

                try
                {
                    File.Copy(origConnectFile, newConnectFile);
                }
                catch (Exception e2)
                {
                    return e2.Message;
                }
            }

            return null;
        }

        static string connectFileBaseName = "MrktsimConnection";

        public string NewConnectFile()
        {
            return NewConnectFile(path + ConnectFile);
        }

        
        public string NewConnectFile(string templateFile)
        {
            connectFile = connectFileBaseName + ".udl";
            int index = 0;
            bool done = false;

            string[] names = ConnectionFiles;

            while (!done)
            {
                done = true;

                foreach (string name in names)
                {
                    if (connectFile == name)
                    {
                        done = false;
                        index++;
                        connectFile = connectFileBaseName + index.ToString() + ".udl";
                    }
                }
            }

            if (templateFile != null)
            {
                File.Copy(templateFile, path + connectFile);
            }
            else
            {

                System.IO.StreamWriter connectWrite = null;

                try
                {
                    connectWrite = File.CreateText(path + connectFile);
                }
                catch (Exception)
                {
                    connectFile = null;
                    return null;
                }

                connectWrite.Flush();

                connectWrite.Close();
            }

            return connectFile;
        }

        public bool DeleteConnectFile(string filename)
        {
            FileInfo fi = new FileInfo(path + filename);

            if (fi.Exists)
            {
                fi.Delete();

                return true;
            }

            return false;
        }

		string connectFile = null;
        string path = null;

		private System.Data.OleDb.OleDbConnection mrktsimConnection = null;

		public System.Data.OleDb.OleDbConnection Connection
		{
			get
			{
				return mrktsimConnection;
			}
		}

        public string ConnectFile
        {
            get
            {
                return connectFile;
            }

            set
            {
                connectFile = value;
            }
        }


        public MSConnect( string startUpPath ) {
            path = startUpPath + @"\connect\";

            mrktsimConnection = new System.Data.OleDb.OleDbConnection();
        }

        // for scenario manager library use - JimJ
        public MSConnect( string connectFilePath, System.Data.OleDb.OleDbConnection connection ) {
            path = connectFilePath;
            mrktsimConnection = connection;
        }

        #region queries

        public string Server
        {
            get
            {
                try
                {
                    mrktsimConnection.ConnectionString = "File Name=" + path + connectFile + ";";
                }
                catch
                {
                    return null;
                }

                if (mrktsimConnection.DataSource == "")
                {
                    return null;
                }

                return mrktsimConnection.DataSource;
            }

        }

        public string ModelDb
        {
            get
            {
                try
                {
                    mrktsimConnection.ConnectionString = "File Name=" + path + connectFile + ";";
                }
                catch
                {
                    return  "<No ModelDb Selected>";;
                }

                if (mrktsimConnection.Database == "")
                {
                    return "<No Database Selected>";
                }

                return mrktsimConnection.Database;
            }
        }

        public bool ConnectFileExists()
        {
            if (connectFile != null)
            {
                FileInfo connectFileInfo = new FileInfo(connectFile);

                return connectFileInfo.Exists;
            }

            return false;
        }

        #endregion

        #region Test
        // if you do not care why it fails
		public bool TestConnection()
		{

			string dummy = null;

			bool convert = false;

			return TestConnection(out dummy, out convert);
		}


        public bool TestConnection(out string error)
        {
            bool convert = false;
            return TestConnection(out error, out convert);
        }

		public bool TestConnection(out string error, out bool convert)
		{
			bool canConnect;

			return TestConnection(out error, out canConnect, out convert);
		}

		public bool TestConnection(out string error, out bool canConnect, out bool convert)
		{
			canConnect = false;

			convert = false;

			error = "Unable to establish connection";

			mrktsimConnection.ConnectionString = "";

			try
			{
				mrktsimConnection.ConnectionString = "File Name=" + path + connectFile + ";";
			}

			catch (Exception e)
			{
				mrktsimConnection.ConnectionString = "";
                error = e.Message;

				return false;
			}

            if (mrktsimConnection.Database == null || mrktsimConnection.Database == "")
            {
                error = "Database not set in connection file";
                return false;
            }

                
            if (mrktsimConnection.DataSource == null || mrktsimConnection.DataSource == "")
            {
                error = "Data Source not set in connection file";
                return false;
            }

			// now try to perform a selection
			error = ProjectDb.testConnection(mrktsimConnection, out canConnect, out convert);


			if (error == null)
				return true;

			return false;
        }

      

        #endregion

        #region edit

        public bool EditConnection(out string error)
		{
			bool convert = false;

            return EditConnection(out error, out convert);
		}



        public bool EditConnection(out string error, out bool convert)
		{
			bool canConnect;

            return EditConnection(out error, out canConnect, out convert);
		}

        public bool EditConnection(out string error, out bool canConnect, out bool convert)
        {
            canConnect = false;

            convert = false;

            error = "Connection file not defined";

            if (connectFile == null || !connectFile.EndsWith(".udl"))
            {
                return false;
            }

			Process myProcess = new Process();

			myProcess.StartInfo.FileName = path + connectFile; 

			myProcess.Start();

			myProcess.WaitForExit();

			if (!TestConnection(out error, out canConnect, out convert))
			{
				return false;
			}

			return true;
        }

        #endregion

        /// <summary>
        /// returns the relative file
        /// </summary>
        public string[] ConnectionFiles
        {
            get
            { 
                int index;

                 string[] files = null;

                try
                {

                    files = Directory.GetFiles(path);
                }
                catch (DirectoryNotFoundException)
                {
                    files =  new string[] { };
                }


                // return the relative path

                for (int ii = 0; ii < files.Length; ++ii )
                {
                    string file = files[ii];


                    index = file.LastIndexOf('\\');


                    if (index >= 0 && index + 1 < file.Length)
                    {
                        files[ii] = file.Substring(index + 1);
                    }
                }

                return files;
            }
        }
	}

}

