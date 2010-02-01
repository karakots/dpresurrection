using System;
using System.IO;
using System.Data.OleDb;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MrktSimDb;
using MarketSimSettings;

namespace DatabaseManager
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class DbMan : System.Windows.Forms.Form
	{
        public string ConnectFile
        {
            get
            {
                return msConnect.ConnectFile;
            }

            set
            {
                msConnect.ConnectFile = value;
            }
        }

        string connectError = " - connection error";

		// this is version of this database
		int major = 0;
		int minor = 0;
		int release = 0;

        private string initString = "\r\nTo get started: Click on the connection button\r\n\r\n > Select a MarketSim Database\r\n\r\n > If no MarketSim Database available then select 'master' as the database\r\n";

        private string dbConvertNowMessage = "      Update the \"{0}\" Database to version {2} now?      \r\n\r\n" +
            "      (Current \"{0}\" Database version is {1})    ";
        private string dbConvertNowTitle = "Confirm Database Update";

		// current version of db needed by software
		int currMajor;
		int currMinor;
		int currRelease;
        MSConnect msConnect = null;
        private System.Windows.Forms.Label currentVersionLabel;
		private System.Windows.Forms.TextBox infoBox;
        private System.Windows.Forms.Panel panel1;
        private SplitContainer splitContainer1;
        private TreeView dbTree;
        private Button cancelBut;
        private Button okBut;
        private Button newConnectBut;
        private ContextMenuStrip dbMenu;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem newDatabaseToolStripMenuItem;
        private ToolStripMenuItem convertToolStripMenuItem;
        private ToolStripMenuItem shrinkToolStripMenuItem;
        private ToolStripMenuItem deleteDatabaseToolStripMenuItem;
        private ToolStripMenuItem deleteConnectionToolStripMenuItem;
        private ToolTip TreelTip;
        private IContainer components;

		public DbMan()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


			ProjectDb.GetCurrentDbVersion(out currMajor, out currMinor, out currRelease);
			this.currentVersionLabel.Text = "Current MarketSim Database Version" +
                "......     " + currMajor + "." + currMinor + "." + currRelease;

            msConnect = new MSConnect(Application.StartupPath);

            createDbTree();

            this.okBut.Enabled = false;

            this.editToolStripMenuItem.Visible = false;
            this.shrinkToolStripMenuItem.Visible = false;
            this.convertToolStripMenuItem.Visible = false;
            this.deleteDatabaseToolStripMenuItem.Visible = false;


            this.newDatabaseToolStripMenuItem.Visible = false;
            this.deleteConnectionToolStripMenuItem.Visible = false;


            dbTree.ExpandAll();

            if (dbTree.Nodes.Count == 0)
            {
                this.TreelTip.Active = false;
                infoBox.Text += initString;
            }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.currentVersionLabel = new System.Windows.Forms.Label();
            this.infoBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.newConnectBut = new System.Windows.Forms.Button();
            this.cancelBut = new System.Windows.Forms.Button();
            this.okBut = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dbTree = new System.Windows.Forms.TreeView();
            this.dbMenu = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shrinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TreelTip = new System.Windows.Forms.ToolTip( this.components );
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.dbMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // currentVersionLabel
            // 
            this.currentVersionLabel.Location = new System.Drawing.Point( 46, 12 );
            this.currentVersionLabel.Name = "currentVersionLabel";
            this.currentVersionLabel.Size = new System.Drawing.Size( 275, 16 );
            this.currentVersionLabel.TabIndex = 3;
            this.currentVersionLabel.Text = "Current MarketSim database  Version.....   2.0.9";
            // 
            // infoBox
            // 
            this.infoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoBox.Location = new System.Drawing.Point( 0, 0 );
            this.infoBox.Multiline = true;
            this.infoBox.Name = "infoBox";
            this.infoBox.ReadOnly = true;
            this.infoBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoBox.Size = new System.Drawing.Size( 407, 106 );
            this.infoBox.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.newConnectBut );
            this.panel1.Controls.Add( this.cancelBut );
            this.panel1.Controls.Add( this.okBut );
            this.panel1.Controls.Add( this.currentVersionLabel );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point( 0, 216 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 407, 71 );
            this.panel1.TabIndex = 8;
            // 
            // newConnectBut
            // 
            this.newConnectBut.Location = new System.Drawing.Point( 38, 40 );
            this.newConnectBut.Name = "newConnectBut";
            this.newConnectBut.Size = new System.Drawing.Size( 142, 23 );
            this.newConnectBut.TabIndex = 7;
            this.newConnectBut.Text = "Create New Connection";
            this.newConnectBut.UseVisualStyleBackColor = true;
            this.newConnectBut.Click += new System.EventHandler( this.newConnectBut_Click );
            // 
            // cancelBut
            // 
            this.cancelBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBut.Location = new System.Drawing.Point( 264, 40 );
            this.cancelBut.Name = "cancelBut";
            this.cancelBut.Size = new System.Drawing.Size( 58, 23 );
            this.cancelBut.TabIndex = 6;
            this.cancelBut.Text = "Cancel";
            this.cancelBut.UseVisualStyleBackColor = true;
            this.cancelBut.Click += new System.EventHandler( this.cancelBut_Click );
            // 
            // okBut
            // 
            this.okBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBut.Location = new System.Drawing.Point( 347, 40 );
            this.okBut.Name = "okBut";
            this.okBut.Size = new System.Drawing.Size( 44, 23 );
            this.okBut.TabIndex = 5;
            this.okBut.Text = "OK";
            this.okBut.UseVisualStyleBackColor = true;
            this.okBut.Click += new System.EventHandler( this.okBut_Click );
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.dbTree );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.infoBox );
            this.splitContainer1.Size = new System.Drawing.Size( 407, 216 );
            this.splitContainer1.SplitterDistance = 106;
            this.splitContainer1.TabIndex = 9;
            // 
            // dbTree
            // 
            this.dbTree.ContextMenuStrip = this.dbMenu;
            this.dbTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbTree.Location = new System.Drawing.Point( 0, 0 );
            this.dbTree.Name = "dbTree";
            this.dbTree.Size = new System.Drawing.Size( 407, 106 );
            this.dbTree.TabIndex = 0;
            this.TreelTip.SetToolTip( this.dbTree, "Right Click for Options" );
            this.dbTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.dbTree_AfterSelect );
            // 
            // dbMenu
            // 
            this.dbMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.newDatabaseToolStripMenuItem,
            this.convertToolStripMenuItem,
            this.shrinkToolStripMenuItem,
            this.deleteConnectionToolStripMenuItem,
            this.deleteDatabaseToolStripMenuItem} );
            this.dbMenu.Name = "dbMenu";
            this.dbMenu.Size = new System.Drawing.Size( 174, 136 );
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size( 173, 22 );
            this.editToolStripMenuItem.Text = "Edit Connection...";
            this.editToolStripMenuItem.Click += new System.EventHandler( this.editToolStripMenuItem_Click );
            // 
            // newDatabaseToolStripMenuItem
            // 
            this.newDatabaseToolStripMenuItem.Name = "newDatabaseToolStripMenuItem";
            this.newDatabaseToolStripMenuItem.Size = new System.Drawing.Size( 173, 22 );
            this.newDatabaseToolStripMenuItem.Text = "New Database";
            this.newDatabaseToolStripMenuItem.Click += new System.EventHandler( this.newDatabaseToolStripMenuItem_Click );
            // 
            // convertToolStripMenuItem
            // 
            this.convertToolStripMenuItem.Name = "convertToolStripMenuItem";
            this.convertToolStripMenuItem.Size = new System.Drawing.Size( 173, 22 );
            this.convertToolStripMenuItem.Text = "Convert";
            this.convertToolStripMenuItem.Click += new System.EventHandler( this.convertToolStripMenuItem_Click );
            // 
            // shrinkToolStripMenuItem
            // 
            this.shrinkToolStripMenuItem.Name = "shrinkToolStripMenuItem";
            this.shrinkToolStripMenuItem.Size = new System.Drawing.Size( 173, 22 );
            this.shrinkToolStripMenuItem.Text = "Shrink";
            this.shrinkToolStripMenuItem.Click += new System.EventHandler( this.shrinkToolStripMenuItem_Click );
            // 
            // deleteConnectionToolStripMenuItem
            // 
            this.deleteConnectionToolStripMenuItem.Name = "deleteConnectionToolStripMenuItem";
            this.deleteConnectionToolStripMenuItem.Size = new System.Drawing.Size( 173, 22 );
            this.deleteConnectionToolStripMenuItem.Text = "Delete Connection";
            this.deleteConnectionToolStripMenuItem.Click += new System.EventHandler( this.deleteConnectionToolStripMenuItem_Click );
            // 
            // deleteDatabaseToolStripMenuItem
            // 
            this.deleteDatabaseToolStripMenuItem.Name = "deleteDatabaseToolStripMenuItem";
            this.deleteDatabaseToolStripMenuItem.Size = new System.Drawing.Size( 173, 22 );
            this.deleteDatabaseToolStripMenuItem.Text = "Delete Database";
            this.deleteDatabaseToolStripMenuItem.Click += new System.EventHandler( this.deleteDatabaseToolStripMenuItem_Click );
            // 
            // DbMan
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.ClientSize = new System.Drawing.Size( 407, 287 );
            this.ControlBox = false;
            this.Controls.Add( this.splitContainer1 );
            this.Controls.Add( this.panel1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size( 415, 250 );
            this.Name = "DbMan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MarketSim Connection Manager";
            this.panel1.ResumeLayout( false );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout( false );
            this.dbMenu.ResumeLayout( false );
            this.ResumeLayout( false );

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new DbMan());
		}

        private bool checkConnection() {
            return checkConnection( false );
        }

        private bool checkConnection( bool offerUpdateNow )
		{
            string errorString = null;
			bool convert = false;
			bool canConnect;

			bool noError = msConnect.TestConnection(out errorString, out canConnect, out convert);

            if( offerUpdateNow && noError == false && canConnect == true && convert == true ) {
                //ask if the user wants to update the database
                ProjectDb.GetDbVersion( msConnect.Connection, out major, out minor, out release );
                string dbVersion = major + "." + minor + "." + release;
                string currVersion = currMajor + "." + currMinor + "." + currRelease;
                string msg = String.Format( dbConvertNowMessage, dbTree.SelectedNode.Text, dbVersion, currVersion );
                DialogResult resp = MessageBox.Show( this, msg, dbConvertNowTitle, 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1 );
                if( resp == DialogResult.OK ) {
                    convertToolStripMenuItem_Click( null, null );
                }
            }
            else {
                setConnectionData( errorString, noError, canConnect, convert );
            }

			return noError;
		}

        private void setConnectionData(string errorString, bool noError, bool canConnect, bool convert)
        {
            infoBox.Clear();

            this.Text = "MarketSim Database Utility - " + msConnect.Connection.Database;
            this.okBut.Enabled = false;

            if (canConnect)
            {
               this.newDatabaseToolStripMenuItem.Enabled = true;

                this.okBut.Enabled = true;
            }
            else
            {
              this.newDatabaseToolStripMenuItem.Enabled = false;
            }

            if (!noError && !convert)
            {
                infoBox.Text += "\r\n There has been an error connecting to the database:\r\n " + errorString;
                this.convertToolStripMenuItem.Enabled = false;
                this.shrinkToolStripMenuItem.Enabled = false;
            }
            else
            {
                // get current version

                ProjectDb.GetDbVersion(msConnect.Connection, out major, out minor, out release);

                string dbVersion = major + "." + minor + "." + release;

                this.convertToolStripMenuItem.Enabled = true;
                this.shrinkToolStripMenuItem.Enabled = true;

                if (convert)
                {
                    infoBox.Text += "\r\n This MarketSim database version: " + dbVersion + " must be upgraded in order to run with the current version of MarketSim";
                }
                else if (major < currMajor ||
                    minor < currMinor ||
                    release < currRelease)
                {
                    infoBox.Text += "\r\n This MarketSim database version: " + dbVersion + " is outof date. ";
                    infoBox.Text += "\r\n It is recommended that you upgrade your MarketSim database in order to run with the current version of MarketSim";
                }
                else
                {
                    infoBox.Text += "\r\n MarketSim database is up to date";
                    this.convertToolStripMenuItem.Enabled = false;
                }
            }
        }

	

		private bool runScriptFile(string scriptFile)
		{
			System.IO.TextReader sqlReader = null;
			try
			{
				sqlReader = File.OpenText(scriptFile);
			}
			catch(Exception fileErr) 
			{
				infoBox.Text += "\r\n" + fileErr.Message;
				return false;
			}

			string convertScript = sqlReader.ReadToEnd();

			sqlReader.Close();

			// remove characters that cause us trouble
			convertScript = convertScript.Replace('\r', ' ');
			convertScript = convertScript.Replace('\n', ' ');
			convertScript = convertScript.Replace('\t', ' ');

			convertScript = convertScript.Replace("GO", "\n");

			char[] newLine = {'\n'};
			string[] sqlCommands = convertScript.Split(newLine);

			

			bool noError = true;
			OleDbTransaction transaction = msConnect.Connection.BeginTransaction();
			OleDbCommand aCommand = new OleDbCommand("", msConnect.Connection, transaction);
			aCommand.CommandTimeout = 0;

			foreach( string sqlCommand in sqlCommands)
			{
				if (sqlCommand.Length == 0)
					continue;

				aCommand.CommandText = sqlCommand;

				try
				{
					aCommand.ExecuteNonQuery();
				}
				catch(System.Data.OleDb.OleDbException oops)
				{
					infoBox.Text += "\r\n Sql error : \r\n" + oops.Message;
					noError = false;
					break;
				}
			}

			if (noError)
				transaction.Commit();
			else
				transaction.Rollback();

		

			return noError;
        }



        #region Tree
        /// <summary>
        /// Adds one connection file to tree
        /// </summary>
        /// <param name="file"></param>
        private TreeNode addFile(string file)
        {
            string server;
            string db;

            msConnect.ConnectFile = file;

            server = msConnect.Server;

            db = msConnect.ModelDb;

            if (server == null || db == null)
            {
                msConnect.DeleteConnectFile(file);

                return null;
            }

            // find server in nodes

            TreeNode serverNode = null;

            foreach (TreeNode aServerNode in dbTree.Nodes)
            {
                if (server == aServerNode.Text)
                {
                    serverNode = aServerNode;
                }
            }

            if (serverNode == null)
            {
                // add a new server
                serverNode = new TreeNode(server);
                serverNode.ForeColor = Color.Blue;

                dbTree.Nodes.Add(serverNode);
            }

            // now add database to server
            TreeNode dbNode = new TreeNode(db);
            dbNode.ForeColor = Color.Blue;

            dbNode.Tag = file;

            serverNode.Nodes.Add(dbNode);

            return dbNode;
        }

        /// <summary>
        /// Creates the tree of servers and 
        /// </summary>
        private void createDbTree()
        {
            string[] files = msConnect.ConnectionFiles;

            foreach (string file in files)
            {
                string errorString;
                bool canConnect;
                bool convert;

                msConnect.ConnectFile = file;

                infoBox.Text = "Connecting to " + msConnect.ModelDb + " on " + msConnect.Server + "\r\n";

                bool noError = msConnect.TestConnection(out errorString, out canConnect, out convert);

                TreeNode addedNode = addFile(file);

                if (!(noError || (canConnect && convert)))
                {
                    addedNode.Text += connectError;
                    addedNode.ForeColor = Color.Red;
                }
            }
        }
        #endregion


        private void okBut_Click(object sender, EventArgs e)
        {
            TreeNode node = dbTree.SelectedNode;

            if (node != null && node.Tag != null)
            {
                msConnect.ConnectFile = node.Tag.ToString();

                if (msConnect.TestConnection())
                {
                    this.okBut.Enabled = true;
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private void cancelBut_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;
        }

        private void dbTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (dbTree.Nodes.Count > 0)
            {
                this.TreelTip.Active = true;
            }

            infoBox.Clear();
            
            this.okBut.Enabled = false;

            TreeNode node = dbTree.SelectedNode;

            if (node.Tag != null && !node.Text.EndsWith(connectError))
            {
                this.editToolStripMenuItem.Visible = true;
                this.shrinkToolStripMenuItem.Visible = true;
                this.convertToolStripMenuItem.Visible = true;
                this.newDatabaseToolStripMenuItem.Visible = true;
                this.deleteDatabaseToolStripMenuItem.Visible = true;
                this.deleteConnectionToolStripMenuItem.Visible = true;

                msConnect.ConnectFile = node.Tag.ToString();

                checkConnection( true );
            }
            else if (node.Tag != null)
            {
                this.editToolStripMenuItem.Visible = true;
                this.shrinkToolStripMenuItem.Visible = false;
                this.convertToolStripMenuItem.Visible = false;
                this.newDatabaseToolStripMenuItem.Visible = false;
                this.deleteDatabaseToolStripMenuItem.Visible = false;
                this.deleteConnectionToolStripMenuItem.Visible = true;

                msConnect.ConnectFile = node.Tag.ToString();
            }
            else
            {
                this.editToolStripMenuItem.Visible = false;
                this.shrinkToolStripMenuItem.Visible = false;
                this.convertToolStripMenuItem.Visible = false;
                this.deleteConnectionToolStripMenuItem.Visible = false;

                this.deleteDatabaseToolStripMenuItem.Visible = false;

                string serverFile = getServerFile(node);

                if (serverFile != null)
                {
                    infoBox.Text = "Successfully connected to server";
                    this.newDatabaseToolStripMenuItem.Visible = true;
                }
                else
                {
                    infoBox.Text = "Cannot connect to server";
                    this.newDatabaseToolStripMenuItem.Visible = false;
                }

            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = dbTree.SelectedNode;

            if (node == null || node.Tag == null)
            {
                return;
            }

            string error;
            bool canConnect;
            bool convert;

            bool noError = msConnect.EditConnection(out error, out canConnect, out convert);

            if (noError || (canConnect && convert))
            {
                node.ForeColor = Color.Blue;
            }

            // check if this node has changed
            TreeNode parent = node.Parent;

            if (parent.Text != msConnect.Server)
            {
                removeNode(node);

                if (noError || (canConnect && convert))
                {
                    TreeNode newNode = addFile(msConnect.ConnectFile);

                    dbTree.SelectedNode = newNode;
                }
            }
            else
            {
                node.Text = msConnect.ModelDb;
            }

            setConnectionData(error, noError, canConnect, convert);
        }

        private void newConnectBut_Click(object sender, EventArgs e)
        {
            string serverFile = getServerFile(dbTree.SelectedNode);

            if (serverFile == null)
            {
                msConnect.NewConnectFile(Application.StartupPath + @"\default.udl");
            }
            else
            {
                msConnect.ConnectFile = serverFile;

                msConnect.NewConnectFile();
            }
   
            string error;
            bool canConnect;
            bool convert;

            bool noError = msConnect.EditConnection(out error, out canConnect, out convert);

            if (noError || (canConnect && convert))
            {
                if (addFile(msConnect.ConnectFile) != null)
                {
                    setConnectionData(error, noError, canConnect, convert);
                }
                else
                {
                    infoBox.Text += "\r\n Error creating connection \r\n";
                }
            }
            else if (canConnect && msConnect.Connection.Database == "master")
            {
                DialogResult rslt = MessageBox.Show(this, msConnect.Connection.Database + " is not a MarketSim database, do you wish to create a MarketSim Database?", "Create Database?", MessageBoxButtons.YesNo);

                if (rslt == DialogResult.Yes)
                {
                    // create a database and then connect to it
                    string tempConnectFile = msConnect.ConnectFile;

                    TreeNode tempNode = addFile(tempConnectFile);
                    this.dbTree.SelectedNode = tempNode;

                    newDatabaseToolStripMenuItem_Click(this, e);

                    msConnect.DeleteConnectFile(tempConnectFile);
                   
                    removeNode(tempNode);

                }
                else
                {
                    msConnect.DeleteConnectFile(msConnect.ConnectFile);
                }
            }
            else
            {
                infoBox.Text += "\r\n Error creating connection: " + error + "\r\n";
                 msConnect.DeleteConnectFile(msConnect.ConnectFile);
            }
        }

      

        private void newDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            NewDb dlg = new NewDb();

            DialogResult rslt = dlg.ShowDialog(this);

            if (rslt == DialogResult.Cancel)
                return;

            // get name of database
            string dbName = dlg.DbName;

            infoBox.Text = "Creating database " + dbName;

            // get connection from a viable child

            string serverFile = getServerFile(dbTree.SelectedNode);

            if (serverFile == null)
            {
                // we need to create a connection to a server
                // we will set the database to master
                return;

            }

            msConnect.ConnectFile = serverFile;

            msConnect.Connection.Open();

            bool noError = true;

            OleDbCommand aCommand = new OleDbCommand("CREATE DATABASE " + dbName, msConnect.Connection);
            aCommand.CommandTimeout = 0;
            try
            {
                aCommand.ExecuteNonQuery();
            }
            catch (System.Data.OleDb.OleDbException oops)
            {
                infoBox.Text += "\r\n Error creating ModelDb \r\n" + oops.Message;
                noError = false;
            }

            if (noError)
            {
                // make recorcy model simple
                aCommand.CommandText = "ALTER DATABASE " + dbName + " SET RECOVERY SIMPLE";
                try
                {
                    aCommand.ExecuteNonQuery();
                }
                catch (System.Data.OleDb.OleDbException oops)
                {
                    infoBox.Text += "\r\n Error setting recovery model - please check server administration\r\n" + oops.Message;             
                }
            }

            if (noError)
            {
                aCommand.CommandText = "USE " + dbName;

                try
                {
                    aCommand.ExecuteNonQuery();
                }
                catch (System.Data.OleDb.OleDbException oops)
                {
                    infoBox.Text += "\r\n Error setting new ModelDb \r\n" + oops.Message;
                    noError = false;
                }
            }


           

            if (noError == false)
            {
                msConnect.Connection.Close();
                return;
            }

            bool scriptOk = runScriptFile(Application.StartupPath + @"\scripts\mrktsimdb.sql");

            msConnect.Connection.Close();

            if (scriptOk)
            {
              

                infoBox.Text += "\r\n ModelDb Created \r\n";
                infoBox.Text += "\r\n Please select database \r\n";
                infoBox.Refresh();

                string file = msConnect.NewConnectFile();

                string error;
                bool canConnect;
                bool convert;

                noError = msConnect.EditConnection(out error, out canConnect, out convert);

                if (noError || (canConnect && convert))
                {
                    TreeNode node = addFile(msConnect.ConnectFile);

                    setConnectionData(error, noError, canConnect, convert);

                    if (node != null)
                    {
                        dbTree.SelectedNode = node;

                        convertToolStripMenuItem_Click(sender, e);
                    }
                }
                else
                {
                    infoBox.Text += "\r\n Error connecting: " + error + " \r\n";
                }
            }
        }

        private string getServerFile(TreeNode serverNode)
        {
            string error;
            bool canConnect;
            bool convert;

            if (serverNode == null)
                return null;

            if (serverNode.Tag != null)
            {
                msConnect.ConnectFile = serverNode.Tag.ToString();

                msConnect.TestConnection(out error, out canConnect, out convert);

                if (canConnect)
                {
                    return msConnect.ConnectFile;
                }
            }

            foreach (TreeNode dbNode in serverNode.Nodes)
            {
                if (dbNode.Tag != null && !dbNode.Text.EndsWith(connectError))
                {
                    msConnect.ConnectFile = dbNode.Tag.ToString();

                    msConnect.TestConnection(out error, out canConnect, out convert);

                    if (canConnect)
                    {
                        return msConnect.ConnectFile;
                    }
                }
            }

            return null;
        }


        #region convert
        private void convertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool noError = true;

            infoBox.Text = "";

            while (noError &&
                (major < this.currMajor || minor < currMinor || release < currRelease))
            {
                // convert to next version
                noError = convert();

                if (noError)
                    noError = ProjectDb.GetDbVersion(msConnect.Connection, out major, out minor, out release);
            }


            // check if conversion was done correctly
            if (noError)
            {
                if (checkConnection())
                {
                    infoBox.Text += "\r\n ModelDb succesfully converted.";
                }
            }
            else
            {
                infoBox.Text += "\r\n Please contact Decision Power";
                this.convertToolStripMenuItem.Enabled = false;
                this.shrinkToolStripMenuItem.Enabled = false;
            }
        }

            
        private bool convert()
		{
			int nextMajor = major;
			int nextMinor = minor;
			int nextRelease = release;
			
			//try a new Relase
			++nextRelease;
			if(!tryConvert(nextMajor, nextMinor, nextRelease))
			{
				//try new minor
				nextRelease = 0;
				++nextMinor;
				if(!tryConvert(nextMajor, nextMinor, nextRelease))
				{
					//try new major
					nextMinor = 0;
					++nextMajor;
					if(!tryConvert(nextMajor, nextMinor, nextRelease))
					{
						return false;
					}
				}

			}

			return true;
        }

        private bool tryConvert(int nextMajor, int nextMinor, int nextRelease)
        {
            string fileName = Application.StartupPath + @"\scripts\msdb_conversion_" + major + "." + minor + "." + release + "_" + nextMajor + "." + nextMinor + "." + nextRelease + ".sql";

            System.IO.TextReader sqlReader = null;
            try
            {
                infoBox.Text += "\r\n running " + fileName + "\r\n";
                sqlReader = File.OpenText(fileName);
            }
            catch (Exception)
            {
                return false;
            }

            msConnect.Connection.Open();
            bool rval = runScriptFile(fileName);
            msConnect.Connection.Close();

            return rval;
        }

        #endregion


        private void shrinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // first back up LOG
            bool noError = true;

            msConnect.Connection.Open();

            string dbName = msConnect.Connection.Database;

            string backup = "BACKUP LOG " + dbName + " WITH NO_LOG";

            OleDbCommand aCommand = new OleDbCommand(backup, msConnect.Connection);
            aCommand.CommandTimeout = 0;
            try
            {
                aCommand.ExecuteNonQuery();
            }
            catch (System.Data.OleDb.OleDbException oops)
            {
                infoBox.Text += "\r\n Error truncating Transaction log file \r\n" + oops.Message;
                noError = false;
            }

            if (noError)
            {
                // shrink database
                aCommand.CommandText = "DBCC SHRINKDATABASE (" + dbName + ", TRUNCATEONLY)";

                try
                {
                    aCommand.ExecuteNonQuery();
                }
                catch (System.Data.OleDb.OleDbException oops)
                {
                    infoBox.Text += "\r\n Error shrinking ModelDb \r\n" + oops.Message;
                    noError = false;
                }
            }

            msConnect.Connection.Close();

            if (noError)
            {
                infoBox.Text += "\r\n ModelDb shrink successfull \r\n";
            }
        }

        private void deleteDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool noError = true;
            string dbName = msConnect.Connection.Database;

            DialogResult rslt = MessageBox.Show("This will delete the database and any data in it, this operation cannot be undone", "Delete Datbase", MessageBoxButtons.OKCancel);

            if (rslt == DialogResult.Cancel)
                return;

            OleDbCommand aCommand = new OleDbCommand("USE master", msConnect.Connection);
            
            aCommand.CommandTimeout = 0;

            msConnect.Connection.Open();

            try
            {
                aCommand.ExecuteNonQuery();
            }
            catch (System.Data.OleDb.OleDbException oops)
            {
                infoBox.Text += "\r\n Error deleting database - cannot find master db \r\n" + oops.Message;
                noError = false;
            }

            if (noError)
            {
                aCommand.CommandText = "DROP DATABASE " + dbName;

                try
                {
                    aCommand.ExecuteNonQuery();
                }
                catch (System.Data.OleDb.OleDbException oops)
                {
                    infoBox.Text += "\r\n Error deleting ModelDb \r\n" + oops.Message;
                    noError = false;
                }
            }

            msConnect.Connection.Close();

            if (noError)
            {  
                TreeNode node = dbTree.SelectedNode;

                string connectFile = node.Tag.ToString();

                msConnect.DeleteConnectFile(connectFile);

                removeNode(node);
            }

           

        }

        private void deleteConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = dbTree.SelectedNode;

            if (node == null || node.Tag == null)
            {
                return;
            }

            msConnect.DeleteConnectFile(msConnect.ConnectFile);

            removeNode(node);
        }

        private void removeNode(TreeNode node)
        {
            TreeNode parent = node.Parent;

            // remove node from parent
            dbTree.SelectedNode = parent;

            parent.Nodes.Remove(node);

            // check if parent has no children

            if (parent.Nodes.Count == 0)
            {
                parent.Remove();
            }

            if (dbTree.Nodes.Count == 0)
            {
                this.TreelTip.Active = false;
                infoBox.Text += initString;
            }
        }
	}
}
