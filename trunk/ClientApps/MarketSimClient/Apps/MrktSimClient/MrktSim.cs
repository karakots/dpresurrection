using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;

using System.Data;
using System.Diagnostics;
using System.IO;
using MrktSimDb;
using MrktSimDb.Metrics;
using ModelView;
using Common;
using System.Configuration;
using System.Collections.Specialized;
using Common.Utilities;
using Common.Dialogs;
using Results;

using MarketSimUtilities;
using DatabaseManager;
using MarketSimSettings;
using MrktSimClient.Controls;
using Utilities;

using SimControl;

namespace MrktSimClient
{
	/// <summary>
	/// Summary description for Form.
	/// </summary>
	public class MrktSim : System.Windows.Forms.Form, Utilities.StatusShower
	{
        private static NameValueCollection MessageCollection;
        public static string Message( string token ) {
            if( MessageCollection == null )
                return token;

            string message = MessageCollection[ token ];

            if( message == null )
                return token;

            return message;
        }

        MsProjectNode currentNode = null;
		MSConnect msConnect = null;
        private MsDbNode dbNode;

        private string stopSimsMsgS = "WARNING: 1 simulation is currently running.\r\n" +
            "If the simulation is aborted, it cannot be resumed (you must start it again from the beginning).\r\n\r\nAbort the simulation now?";
        private string stopSimsMsg = "WARNING: {0} simulations are currently running.\r\n" +
            "If the simulations are aborted, they cannot be resumed (you must start each one again from the beginning).\r\n\r\nAbort all running simulations now?";
        private string stopSimsTitle = "Confirm Aborting Simulation";

        private string editScenarioStatusMsg = "Loading Scenario for Editing ({0})...";
        private string editScenarioStatusMsg2 = "Saving Scenario ({0})...";

        private string confirmEditScenarioMessage = "Open Scenario Editor?";
        private string confirmEditScenarioTitle = "";

        #region UI element declarations
        private IContainer components;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar toolStripProgressBar1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private SplitContainer splitContainer1;
        private TableLayoutPanel startUpPanel;
        private Panel panel1;
        private Label versionLabel;
        private LinkLabel dbConnectLink;
        private Label label1;
        private LinkLabel returnToDbLink;
        private TextBox welcomeBox;
        private TextBox returnBox;
        // private BannerControl bannerControl1;
        private Panel nodePanel;
        private ToolStripPanel BottomToolStripPanel;
        private ToolStripPanel TopToolStripPanel;
        private ToolStripPanel RightToolStripPanel;
        private ToolStripPanel LeftToolStripPanel;
        private ToolStripContentPanel ContentPanel;
        #endregion

        private Timer timer;
        private bool processActive = false;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel simCountLabel;
        private ToolStripButton backButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripEngineButton;
        private ToolStripButton toolStripDbButton;
        private ToolStripButton toolStripSettingsButton;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripLogoButton;
        private ToolStripButton toolStripEditModelButton;
        private ToolStripButton toolStripEditScenarioButton;
        private ToolStripComboBox toolStripNavBox;
        private ToolStripButton toolStripAddButton;

        private static int waitCursorLevel = 0;
        private ToolStripButton toolStripHelpButton;
        private Label label2;
        private ToolStripButton toolStripRefreshButton;
        private ToolStripSplitButton stopSimsButton;
        private BannerControl bannerControl1;

        public bool ProcessActive {
            set { processActive = value; }
            get { return processActive; }
        }

        private static MrktSim activeMrktSimForm;
        public static MrktSim ActiveMrktSimForm {
            get {
                return activeMrktSimForm; 
            }
        }

        #region Constructor

        public MrktSim() : this(Database.AppType.MarketSim)
        {
        }

        public MrktSim(Database.AppType app)
		{
            // set the application type first
            // so all components know what we are
            Database.Application = app;

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            setIcon( app );

            if( app == Database.AppType.DevlTest )
            {

                MrktSim.DevlMode = true;
            }

            DataLogger.Init();
            Results.SummaryReportGenerator.Init();

            activeMrktSimForm = this;

            // check before closing
            this.Closing += new CancelEventHandler(MrktSim_Closing);
        
            // Settings
            Settings<ClientSettings>.Read( "ClientSettings.config" );

            // initialize metric proper names
            Metric.TokenConvert += new MrktSimDb.Metrics.Metric.Description(MrktSimControl.MrktSimMessage);

            // who am i
            this.Text = Database.AppName;
            int index = Application.ProductVersion.LastIndexOf('.');

            if (index < 3)
                index = Application.ProductVersion.Length;

            versionLabel.Text = Database.AppName + " version " + Application.ProductVersion.Substring( 0, index );
            returnToDbLink.Visible = false;
            returnBox.Visible = false;

            // read in application settings

            MessageCollection = System.Configuration.ConfigurationManager.AppSettings;

            // this is so controls can access the same config file
            MrktSimControl.AppSettings = System.Configuration.ConfigurationManager.AppSettings;

            // check if we are converting from 2.2 connection method to 2.3 method
            string defaultFileName;
            string convertError = MSConnect.CheckConversion(Application.StartupPath, out defaultFileName);

            if( convertError != null )
            {
                MessageBox.Show(convertError);
            }
            else if (defaultFileName != null)
            {    
                Settings<ClientSettings>.Value.ConnectFile = defaultFileName;
            }

            // we have a default name for connection
            msConnect = new MSConnect(Application.StartupPath);
            msConnect.ConnectFile = Settings<ClientSettings>.Value.ConnectFile;

            // can we connect?
            if (!msConnect.TestConnection())
            {
                Settings<ClientSettings>.Value.ObjectId = Database.AllID;
                Settings<ClientSettings>.Value.Type = MsProjectNode.Type.Db;
                
                dbOpen(false);
            }
            else if( !this.initializeDb() ) {
                Settings<ClientSettings>.Value.ObjectId = Database.AllID;
                Settings<ClientSettings>.Value.Type = MsProjectNode.Type.Db;
            }
            else {

                int testId = Settings<ClientSettings>.Value.ObjectId;
                object testtype = Settings<ClientSettings>.Value.Type;
                object testnewtype = Settings<ClientSettings>.Value.Type;

                MsProjectNode node = dbNode.Find( Settings<ClientSettings>.Value.ObjectId, Settings<ClientSettings>.Value.Type );

                // restore the nav bar items from the settings
                string[] navBarNames = Settings<ClientSettings>.Value.NavBarStrings;
                if( navBarNames != null ) {
                    int nNavItems = navBarNames.Length;
                    int[] navBarIDs = Settings<ClientSettings>.Value.NavBarNodeIds;
                    MsProjectNode.Type[] navBarTypes = Settings<ClientSettings>.Value.NavBarNodeTypes;

                    this.navBarItemNodes = new ArrayList();
                    ArrayList validNavBarItemNames = new ArrayList();
                    for( int i = 0; i < nNavItems; i++ ) {
                        MsProjectNode navNode = dbNode.Find( navBarIDs[ i ], navBarTypes[ i ] );
                        if( navNode != null ) {
                            validNavBarItemNames.Add( navBarNames[ i ] );
                            this.navBarItemNodes.Add( navNode );
                        }
                    }
                    navBarNames = new string[ validNavBarItemNames.Count ];
                    validNavBarItemNames.CopyTo( navBarNames );
                    toolStripNavBox.Items.AddRange( navBarNames );
                }

                if( node != null ) {
                    ActivateNode( node );
                }
                else {
                    dbOpen( false );
                }

                checkSimStatus();
            }

       

            // set up location and size from settings
            int wid = Settings<ClientSettings>.Value.MainFrameBounds.Width;
            int ht = Settings<ClientSettings>.Value.MainFrameBounds.Height;
            int x = Settings<ClientSettings>.Value.MainFrameBounds.X;
            int y = Settings<ClientSettings>.Value.MainFrameBounds.Y;
            if( x == 0 && y == 0 ) {
                x = 57;                     // reasonable defaults
                y = 61;
                wid = 878;
                ht = 581;
            }
            this.Location = new Point( x, y );
            if( wid >= this.MinimumSize.Width && ht >= this.MinimumSize.Height &&
                ((x + wid) < SystemInformation.WorkingArea.Width) &&
                ((y + ht) < SystemInformation.WorkingArea.Height) ) {
                this.Size = new Size( wid, ht );
            }
            else {
                // saved bounds won't fit on the screen!
                this.Location = new Point( 10, 10 );
                this.Size = this.MinimumSize;
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if( args.Length == 0 )
            {
                return;
            }

            string appType = args[0];

            Array arr = Enum.GetValues( typeof( Database.AppType ) );

            Database.AppType runType = Database.AppType.MarketSim;

            bool found = false;

            foreach( Database.AppType type in arr )
            {
                if( appType == Enum.GetName( typeof( Database.AppType ), type ) )
                {
                    runType = type;
                    found = true;
                    break;
                }
            }

            if( found )
            {
                Application.Run( new MrktSim( runType ) );
            }
            else
            {
                MessageBox.Show( "Application Type error: " + appType );
            }
        }

        private void setIcon(Database.AppType app)
        {
            string upward = @"..\..\";
            string FileName = "";
            switch(app)
            {
                case Database.AppType.MarketSim:
                    return;
                case Database.AppType.DevlTest:
                    return;
                case Database.AppType.NIMO:
                    FileName = "NIMO.ico";
                    break;
            }

            FileInfo fi = new FileInfo( FileName );

            if( fi.Exists )
            {
                this.Icon = new Icon( fi.FullName );
            }
            else
            {
                //MessageBox.Show( "Could not find file: " + fi.FullName );
                fi = new FileInfo(  upward + FileName );

                if( fi.Exists )
                {
                    this.Icon = new Icon( fi.FullName );
                }
                else
                {
                  //  MessageBox.Show( "Could not find file: " + fi.FullName );
                }
            }
        }

        #endregion

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MrktSim ) );
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.simCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.stopSimsButton = new System.Windows.Forms.ToolStripSplitButton();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.startUpPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.dbConnectLink = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.returnToDbLink = new System.Windows.Forms.LinkLabel();
            this.welcomeBox = new System.Windows.Forms.TextBox();
            this.returnBox = new System.Windows.Forms.TextBox();
            this.bannerControl1 = new MrktSimClient.Controls.BannerControl();
            this.nodePanel = new System.Windows.Forms.Panel();
            this.timer = new System.Windows.Forms.Timer( this.components );
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.backButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripRefreshButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripEngineButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSettingsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripDbButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripEditModelButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripEditScenarioButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripNavBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripAddButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLogoButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripHelpButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.startUpPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.simCountLabel,
            this.stopSimsButton} );
            this.statusStrip1.Location = new System.Drawing.Point( 0, 421 );
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size( 887, 22 );
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.AutoSize = false;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size( 100, 16 );
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size( 0, 17 );
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size( 838, 17 );
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "Simulations Running:";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // simCountLabel
            // 
            this.simCountLabel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.simCountLabel.Name = "simCountLabel";
            this.simCountLabel.Size = new System.Drawing.Size( 13, 17 );
            this.simCountLabel.Text = "0";
            this.simCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // stopSimsButton
            // 
            this.stopSimsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopSimsButton.DropDownButtonWidth = 0;
            this.stopSimsButton.Image = ((System.Drawing.Image)(resources.GetObject( "stopSimsButton.Image" )));
            this.stopSimsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopSimsButton.Name = "stopSimsButton";
            this.stopSimsButton.Size = new System.Drawing.Size( 21, 20 );
            this.stopSimsButton.Text = "Stop Simulations";
            this.stopSimsButton.ButtonClick += new System.EventHandler( this.stopAllSimulationsToolStripMenuItem_Click );
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point( 0, 0 );
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding( 3, 0, 0, 0 );
            this.BottomToolStripPanel.Size = new System.Drawing.Size( 0, 0 );
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.BackColor = System.Drawing.Color.White;
            this.TopToolStripPanel.Location = new System.Drawing.Point( 0, 0 );
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding( 3, 0, 0, 0 );
            this.TopToolStripPanel.Size = new System.Drawing.Size( 0, 0 );
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point( 0, 0 );
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding( 3, 0, 0, 0 );
            this.RightToolStripPanel.Size = new System.Drawing.Size( 0, 0 );
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.BackColor = System.Drawing.Color.White;
            this.LeftToolStripPanel.Location = new System.Drawing.Point( 0, 0 );
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding( 3, 0, 0, 0 );
            this.LeftToolStripPanel.Size = new System.Drawing.Size( 0, 0 );
            // 
            // ContentPanel
            // 
            this.ContentPanel.AutoScroll = true;
            this.ContentPanel.Size = new System.Drawing.Size( 905, 507 );
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 50 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.startUpPanel );
            this.splitContainer1.Panel1.Controls.Add( this.nodePanel );
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size( 887, 371 );
            this.splitContainer1.SplitterDistance = 316;
            this.splitContainer1.TabIndex = 11;
            // 
            // startUpPanel
            // 
            this.startUpPanel.BackColor = System.Drawing.Color.White;
            this.startUpPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.startUpPanel.ColumnCount = 2;
            this.startUpPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Absolute, 160F ) );
            this.startUpPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle() );
            this.startUpPanel.Controls.Add( this.panel1, 0, 0 );
            this.startUpPanel.Controls.Add( this.bannerControl1, 1, 0 );
            this.startUpPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startUpPanel.Location = new System.Drawing.Point( 0, 0 );
            this.startUpPanel.Name = "startUpPanel";
            this.startUpPanel.RowCount = 1;
            this.startUpPanel.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.startUpPanel.Size = new System.Drawing.Size( 887, 371 );
            this.startUpPanel.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add( this.label2 );
            this.panel1.Controls.Add( this.versionLabel );
            this.panel1.Controls.Add( this.dbConnectLink );
            this.panel1.Controls.Add( this.label1 );
            this.panel1.Controls.Add( this.returnToDbLink );
            this.panel1.Controls.Add( this.welcomeBox );
            this.panel1.Controls.Add( this.returnBox );
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point( 4, 4 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 154, 363 );
            this.panel1.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point( 0, 153 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 67, 13 );
            this.label2.TabIndex = 7;
            this.label2.Text = "                    ";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.versionLabel.Font = new System.Drawing.Font( "Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.versionLabel.Location = new System.Drawing.Point( 0, 350 );
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size( 144, 13 );
            this.versionLabel.TabIndex = 3;
            this.versionLabel.Text = "MarketSim version 2.3.00";
            // 
            // dbConnectLink
            // 
            this.dbConnectLink.AutoSize = true;
            this.dbConnectLink.BackColor = System.Drawing.Color.White;
            this.dbConnectLink.Dock = System.Windows.Forms.DockStyle.Top;
            this.dbConnectLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.dbConnectLink.Location = new System.Drawing.Point( 0, 140 );
            this.dbConnectLink.Name = "dbConnectLink";
            this.dbConnectLink.Size = new System.Drawing.Size( 117, 13 );
            this.dbConnectLink.TabIndex = 1;
            this.dbConnectLink.TabStop = true;
            this.dbConnectLink.Text = "Connect to Database...";
            this.dbConnectLink.VisitedLinkColor = System.Drawing.Color.Blue;
            this.dbConnectLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.dbConnectLink_LinkClicked );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point( 0, 127 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 67, 13 );
            this.label1.TabIndex = 5;
            this.label1.Text = "                    ";
            // 
            // returnToDbLink
            // 
            this.returnToDbLink.AutoSize = true;
            this.returnToDbLink.Dock = System.Windows.Forms.DockStyle.Top;
            this.returnToDbLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.returnToDbLink.Location = new System.Drawing.Point( 0, 114 );
            this.returnToDbLink.Name = "returnToDbLink";
            this.returnToDbLink.Size = new System.Drawing.Size( 103, 13 );
            this.returnToDbLink.TabIndex = 4;
            this.returnToDbLink.TabStop = true;
            this.returnToDbLink.Text = "Return to <Name>...";
            this.returnToDbLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.returnToDbLink_LinkClicked );
            // 
            // welcomeBox
            // 
            this.welcomeBox.BackColor = System.Drawing.Color.White;
            this.welcomeBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.welcomeBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.welcomeBox.Location = new System.Drawing.Point( 0, 57 );
            this.welcomeBox.Multiline = true;
            this.welcomeBox.Name = "welcomeBox";
            this.welcomeBox.ReadOnly = true;
            this.welcomeBox.Size = new System.Drawing.Size( 154, 57 );
            this.welcomeBox.TabIndex = 2;
            this.welcomeBox.Text = "Welcome to MarketSim.\r\nPlease click the link below to connect to a database.\r\n";
            // 
            // returnBox
            // 
            this.returnBox.BackColor = System.Drawing.Color.White;
            this.returnBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.returnBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.returnBox.Location = new System.Drawing.Point( 0, 0 );
            this.returnBox.Multiline = true;
            this.returnBox.Name = "returnBox";
            this.returnBox.ReadOnly = true;
            this.returnBox.Size = new System.Drawing.Size( 154, 57 );
            this.returnBox.TabIndex = 6;
            this.returnBox.Text = "You can return to the database you were working with or you can connect to a new " +
                "database.";
            // 
            // bannerControl1
            // 
            this.bannerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bannerControl1.Location = new System.Drawing.Point( 165, 4 );
            this.bannerControl1.Name = "bannerControl1";
            this.bannerControl1.Size = new System.Drawing.Size( 718, 363 );
            this.bannerControl1.TabIndex = 11;
            // 
            // nodePanel
            // 
            this.nodePanel.AutoSize = true;
            this.nodePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodePanel.Location = new System.Drawing.Point( 0, 0 );
            this.nodePanel.Name = "nodePanel";
            this.nodePanel.Size = new System.Drawing.Size( 887, 371 );
            this.nodePanel.TabIndex = 0;
            // 
            // timer
            // 
            this.timer.Interval = 3000;
            this.timer.Tick += new System.EventHandler( this.timer_tick );
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.ForeColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 30, 25 );
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size( 32, 32 );
            this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.backButton,
            this.toolStripRefreshButton,
            this.toolStripSeparator1,
            this.toolStripEngineButton,
            this.toolStripSettingsButton,
            this.toolStripDbButton,
            this.toolStripEditModelButton,
            this.toolStripEditScenarioButton,
            this.toolStripNavBox,
            this.toolStripAddButton,
            this.toolStripLogoButton,
            this.toolStripHelpButton} );
            this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size( 887, 50 );
            this.toolStrip1.TabIndex = 3;
            // 
            // backButton
            // 
            this.backButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.backButton.Image = global::MrktSimClient.Properties.Resources.returnbutton;
            this.backButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size( 36, 47 );
            this.backButton.Text = "toolStripButton1";
            this.backButton.Click += new System.EventHandler( this.backButton_Click );
            // 
            // toolStripRefreshButton
            // 
            this.toolStripRefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripRefreshButton.Image = global::MrktSimClient.Properties.Resources.refresh;
            this.toolStripRefreshButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripRefreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripRefreshButton.Name = "toolStripRefreshButton";
            this.toolStripRefreshButton.Size = new System.Drawing.Size( 36, 47 );
            this.toolStripRefreshButton.Text = "Refresh";
            this.toolStripRefreshButton.Click += new System.EventHandler( this.toolStripRefreshButton_Click );
            // 
            // toolStripEngineButton
            // 
            this.toolStripEngineButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripEngineButton.Image = ((System.Drawing.Image)(resources.GetObject( "toolStripEngineButton.Image" )));
            this.toolStripEngineButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripEngineButton.Margin = new System.Windows.Forms.Padding( 0, 4, 4, 10 );
            this.toolStripEngineButton.Name = "toolStripEngineButton";
            this.toolStripEngineButton.Size = new System.Drawing.Size( 36, 36 );
            this.toolStripEngineButton.Text = "toolStripButton1";
            this.toolStripEngineButton.ToolTipText = "Configure Simulation Engine";
            this.toolStripEngineButton.Click += new System.EventHandler( this.configureSimulationEngineToolStripMenuItem_Click );
            // 
            // toolStripSettingsButton
            // 
            this.toolStripSettingsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSettingsButton.Image = ((System.Drawing.Image)(resources.GetObject( "toolStripSettingsButton.Image" )));
            this.toolStripSettingsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSettingsButton.Margin = new System.Windows.Forms.Padding( 0, 4, 4, 10 );
            this.toolStripSettingsButton.Name = "toolStripSettingsButton";
            this.toolStripSettingsButton.Size = new System.Drawing.Size( 36, 36 );
            this.toolStripSettingsButton.Text = "toolStripButton3";
            this.toolStripSettingsButton.ToolTipText = "Settings";
            this.toolStripSettingsButton.Click += new System.EventHandler( this.toolStripSettingsButton_Click );
            // 
            // toolStripDbButton
            // 
            this.toolStripDbButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDbButton.Enabled = false;
            this.toolStripDbButton.Image = ((System.Drawing.Image)(resources.GetObject( "toolStripDbButton.Image" )));
            this.toolStripDbButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDbButton.Margin = new System.Windows.Forms.Padding( 0, 4, 4, 10 );
            this.toolStripDbButton.Name = "toolStripDbButton";
            this.toolStripDbButton.Size = new System.Drawing.Size( 36, 36 );
            this.toolStripDbButton.Text = "toolStripButton2";
            this.toolStripDbButton.ToolTipText = "Database Utilities";
            this.toolStripDbButton.Click += new System.EventHandler( this.toolStripDbButton_Click );
            // 
            // toolStripEditModelButton
            // 
            this.toolStripEditModelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripEditModelButton.Image = ((System.Drawing.Image)(resources.GetObject( "toolStripEditModelButton.Image" )));
            this.toolStripEditModelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripEditModelButton.Margin = new System.Windows.Forms.Padding( 15, 4, 4, 10 );
            this.toolStripEditModelButton.Name = "toolStripEditModelButton";
            this.toolStripEditModelButton.Size = new System.Drawing.Size( 36, 36 );
            this.toolStripEditModelButton.Text = "toolStripButton1";
            this.toolStripEditModelButton.ToolTipText = "Edit Model";
            this.toolStripEditModelButton.Click += new System.EventHandler( this.toolStripEditModelButton_Click );
            // 
            // toolStripEditScenarioButton
            // 
            this.toolStripEditScenarioButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripEditScenarioButton.Image = ((System.Drawing.Image)(resources.GetObject( "toolStripEditScenarioButton.Image" )));
            this.toolStripEditScenarioButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripEditScenarioButton.Margin = new System.Windows.Forms.Padding( 0, 4, 4, 10 );
            this.toolStripEditScenarioButton.Name = "toolStripEditScenarioButton";
            this.toolStripEditScenarioButton.Size = new System.Drawing.Size( 36, 36 );
            this.toolStripEditScenarioButton.Text = "toolStripButton1";
            this.toolStripEditScenarioButton.ToolTipText = "Edit Scenario";
            this.toolStripEditScenarioButton.Click += new System.EventHandler( this.toolStripEditScenarioButton_Click );
            // 
            // toolStripNavBox
            // 
            this.toolStripNavBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.toolStripNavBox.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.toolStripNavBox.Margin = new System.Windows.Forms.Padding( 15, 0, 1, 0 );
            this.toolStripNavBox.MaxDropDownItems = 20;
            this.toolStripNavBox.Name = "toolStripNavBox";
            this.toolStripNavBox.Size = new System.Drawing.Size( 421, 50 );
            this.toolStripNavBox.SelectedIndexChanged += new System.EventHandler( this.toolStripNavBox_SelectedIndexChanged );
            this.toolStripNavBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.toolStripNavBox_KeyPress );
            // 
            // toolStripAddButton
            // 
            this.toolStripAddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripAddButton.Image = ((System.Drawing.Image)(resources.GetObject( "toolStripAddButton.Image" )));
            this.toolStripAddButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAddButton.Name = "toolStripAddButton";
            this.toolStripAddButton.Size = new System.Drawing.Size( 30, 47 );
            this.toolStripAddButton.Text = "Add";
            this.toolStripAddButton.ToolTipText = "Add the current view to the dropdown list so you can easily return";
            this.toolStripAddButton.Click += new System.EventHandler( this.toolStripAddButton_Click );
            // 
            // toolStripLogoButton
            // 
            this.toolStripLogoButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLogoButton.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripLogoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLogoButton.Image = ((System.Drawing.Image)(resources.GetObject( "toolStripLogoButton.Image" )));
            this.toolStripLogoButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripLogoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripLogoButton.Margin = new System.Windows.Forms.Padding( 6, 1, 6, 2 );
            this.toolStripLogoButton.Name = "toolStripLogoButton";
            this.toolStripLogoButton.Size = new System.Drawing.Size( 36, 47 );
            this.toolStripLogoButton.Text = "Revealing the future!";
            this.toolStripLogoButton.Click += new System.EventHandler( this.toolStripLogoButton_Click );
            // 
            // toolStripHelpButton
            // 
            this.toolStripHelpButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripHelpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripHelpButton.Image = ((System.Drawing.Image)(resources.GetObject( "toolStripHelpButton.Image" )));
            this.toolStripHelpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripHelpButton.Margin = new System.Windows.Forms.Padding( 0, 1, 5, 2 );
            this.toolStripHelpButton.Name = "toolStripHelpButton";
            this.toolStripHelpButton.Size = new System.Drawing.Size( 36, 47 );
            this.toolStripHelpButton.Text = "toolStripButton1";
            this.toolStripHelpButton.ToolTipText = "MarketSim Help";
            this.toolStripHelpButton.Click += new System.EventHandler( this.toolStripHelpButton_Click );
            // 
            // MrktSim
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.ClientSize = new System.Drawing.Size( 887, 443 );
            this.Controls.Add( this.splitContainer1 );
            this.Controls.Add( this.toolStrip1 );
            this.Controls.Add( this.statusStrip1 );
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.MinimumSize = new System.Drawing.Size( 895, 470 );
            this.Name = "MrktSim";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MarketSim";
            this.Load += new System.EventHandler( this.MrktSim_Load );
            this.SizeChanged += new System.EventHandler( this.MrktSim_SizeChanged );
            this.Shown += new System.EventHandler( this.MrktSim_Shown );
            this.LocationChanged += new System.EventHandler( this.MrktSim_LocationChanged );
            this.statusStrip1.ResumeLayout( false );
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.ResumeLayout( false );
            this.startUpPanel.ResumeLayout( false );
            this.startUpPanel.PerformLayout();
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout( false );
            this.toolStrip1.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();

		}

		#endregion

		// utility to check if selected node is a model node

        /// <summary>
        /// read database - peform checks
        /// </summary>
        /// <returns></returns>
        private bool initializeDb()
        {
            ProjectDb theDb = new ProjectDb();
            theDb.Connection = msConnect.Connection;
            theDb.ProjectID = Database.AllID;

            // no longer require all sims to be stopped
            //int numSims = theDb.NumQueuedOrRunningSims();

            //if( numSims > 0 ) {
            //    DialogResult rslt = MessageBox.Show("There appear to be simulations running on the current database. Do you wish to stop all simulations?", "Stop Running Simulations", MessageBoxButtons.YesNo);
            //    if (rslt != DialogResult.Yes) {
            //        dbOpen( false );
            //        dbNode = null;
            //        return false;
            //    }
            //}

            //checkForLockedModels( theDb );

            //theDb.DequeueAllSims();
            //theDb.AllSimsStopped();

            theDb.Refresh();

            dbNode = new MsDbNode(theDb);
            
            dbOpen(true);

            return true;
        }


        public void ActivateNode( MsProjectNode node ) {
            ActivateNode( node, false );
        }

        public void ActivateNode( MsProjectNode node, bool backRequest )
        {
            UserControl backRequestingControl = null;

            this.SuspendLayout();

            if (currentNode != null)
            {
                backRequestingControl = currentNode.Control;
                nodePanel.Controls.Remove(currentNode.Control);
                currentNode.ActiveNode = false;
            }

            currentNode = node;

            if (currentNode == null)
            {  
                this.ResumeLayout(false);
                return;
            }

            Settings<ClientSettings>.Value.ObjectId = currentNode.Id;
            Settings<ClientSettings>.Value.Type = currentNode.NodeType;

            currentNode.ActiveNode = true;

            UserControl control = node.Control;
            control.Tag = backRequestingControl;

            if (!nodePanel.Controls.Contains(control))
            {
                nodePanel.Controls.Add(control);
            }

            control.Dock = DockStyle.Fill;
            control.BringToFront();
            control.Refresh();

            if (currentNode == dbNode)
            {
                this.backButton.Text = "Close " + currentNode.Text;
            }
            else if (currentNode.ParentNode != null)
            {
                this.backButton.Text = "Return To " + currentNode.ParentNode.Text;
            }

            if( backRequest == false ) {
                if( currentNode is MsScenarioNode ) {
                    this.ActiveScenarioNode = (MsScenarioNode)currentNode;
                }
                else if( currentNode is MsModelNode ) {
                    this.ActiveModelNode = (MsModelNode)currentNode;
                }
                else if( currentNode is MsProjectNode ) {
                    this.ActiveProjectNode = (MsProjectNode)currentNode;
                }
            }

            this.ResumeLayout(false);
		}

		private bool checkDataQueryUser()
		{
            if (dbNode != null && dbNode.OpenModel())
			{
				string msg = "Please close all editing sessions before exiting";
				string caption = "Closing MarketSim";

				MessageBox.Show(this, msg, caption);

				return false;
			}

			return true;
		}

        public bool CloseDb()
        {
            if (!checkDataQueryUser())
                return false;

            ActivateNode(null);

            dbOpen(false);

            return true;
        }

        public void ResetUI() {

            MessageBox.Show( MrktSim.Message( "object.deleted" ) );

            this.dbNode.Db.Refresh();

            this.dbNode.RefreshTree();

            ActivateNode( dbNode );
        }

		private void dbOpen(bool open)
		{
            if (!open)
            {
                nodePanel.Controls.Clear();
                this.startUpPanel.BringToFront();

                this.backButton.Enabled = false;

                this.toolStripDbButton.Enabled = false;

            }
            else
            {
                this.nodePanel.BringToFront();
                this.backButton.Enabled = true;

              this.toolStripDbButton.Enabled = true;
               
            }
		}

        public bool CalibrationControlsVisible {
            get {
                return Settings<MrktSim.ClientSettings>.Value.CalibrationControlsVisible;
            }
        }

        private void UpdateBoundsSettings() {
            if( (this.WindowState != FormWindowState.Maximized) && (this.WindowState != FormWindowState.Minimized) ) {
                Settings<MrktSim.ClientSettings>.Value.MainFrameBounds = this.Bounds;
            }
        }

		private void exitItem_Click(object sender, System.EventArgs e)
		{		
			this.Close();
		}

		private void MrktSim_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            UpdateBoundsSettings();

            if( this.WindowState == FormWindowState.Maximized ) {
                Settings<MrktSim.ClientSettings>.Value.MainFrameMaximized = true;
            }
            else {
                Settings<MrktSim.ClientSettings>.Value.MainFrameMaximized = false;
            }

            MarketSimSettings.Settings<MrktSim.ClientSettings>.Save();

            if (checkSimStatus())
            {
                MessageBox.Show("You have running simulations, please shut down simulations before exiting.");

                e.Cancel = true;

                return;
            }

            if (!checkDataQueryUser())
            {
                e.Cancel = true;
            }       
		}

        // deprecated - SSN
		// I am throwing this in as a check for the non-interprise version
		// will remove if we move central storage
        //private void checkForLockedModels(ProjectDb db)
        //{
        //    if (db.LockedModels())
        //    {
        //        DialogResult rslt = MessageBox.Show("There are locked models or simulations in this database, Do you wish to unlock them?", "Unlock models and scenarios", MessageBoxButtons.YesNo);
        //        if (rslt == DialogResult.Yes)
        //            db.UnLockModels();
        //    }
        //}
		
        #region Settings

        [SerializableAttribute]
        public class ClientSettings 
        {
            private string connectFile = "nofile.udl";
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

            private int id = Database.AllID;
            public int ObjectId
            {
                get
                {
                    return id;
                }

                set
                {
                    id = value;
                }
            }

            private MsProjectNode.Type type = MsProjectNode.Type.Db;
            public MsProjectNode.Type Type
            {
                get
                {
                    return type;
                }

                set
                {
                    type = value;
                }
            }

            private Rectangle mainFrameBounds = new Rectangle( 0, 0, 0, 0 );
            public Rectangle MainFrameBounds {
                get {
                    return mainFrameBounds;
                }

                set {
                    mainFrameBounds = value;
                }
            }

            private bool mainFrameMaximized = false;
            public bool MainFrameMaximized {
                get {
                    return mainFrameMaximized;
                }

                set {
                    mainFrameMaximized = value;
                }
            }

            private Rectangle modelFrameBounds = new Rectangle( 0, 0, 0, 0 );
            public Rectangle ModelFrameBounds {
                get {
                    return modelFrameBounds;
                }

                set {
                    modelFrameBounds = value;
                }
            }

            private bool modelFrameMaximized = false;
            public bool ModelFrameMaximized {
                get {
                    return modelFrameMaximized;
                }

                set {
                    modelFrameMaximized = value;
                }
            }

            private int modelScenarioPanelHeight = -1;
            public int ModelScenarioPanelHeight {
                get {
                    return modelScenarioPanelHeight;
                }

                set {
                    modelScenarioPanelHeight = value;
                }
            }

            private int modelFiltersPanelWidth = -1;
            public int ModelFiltersPanelWidth {
                get {
                    return modelFiltersPanelWidth;
                }

                set {
                    modelFiltersPanelWidth = value;
                }
            }

            private int modelPlansPanelHight = -1;
            public int ModelPlansPanelHight {
                get {
                    return modelPlansPanelHight;
                }

                set {
                    modelPlansPanelHight = value;
                }
            }

            private int modelToplLevelPlansPanelWidth = -1;
            public int ModelToplLevelPlansPanelWidth {
                get {
                    return modelToplLevelPlansPanelWidth;
                }

                set {
                    modelToplLevelPlansPanelWidth = value;
                }
            }

            private int modelScenarioPanelHeightMax = -1;
            public int ModelScenarioPanelHeightMax {
                get {
                    return modelScenarioPanelHeightMax;
                }

                set {
                    modelScenarioPanelHeightMax = value;
                }
            }

            private int modelFiltersPanelWidthMax = -1;
            public int ModelFiltersPanelWidthMax {
                get {
                    return modelFiltersPanelWidthMax;
                }

                set {
                    modelFiltersPanelWidthMax = value;
                }
            }

            private int modelPlansPanelHightMax = -1;
            public int ModelPlansPanelHightMax {
                get {
                    return modelPlansPanelHightMax;
                }

                set {
                    modelPlansPanelHightMax = value;
                }
            }

            private int modelToplLevelPlansPanelWidthMax = -1;
            public int ModelToplLevelPlansPanelWidthMax {
                get {
                    return modelToplLevelPlansPanelWidthMax;
                }

                set {
                    modelToplLevelPlansPanelWidthMax = value;
                }
            }

            private int modelEditorVisibleTab = 0;
            public int ModelEditorVisibleTab {
                get {
                    return modelEditorVisibleTab;
                }

                set {
                    modelEditorVisibleTab = value;
                }
            }

            private bool calibrationControlsVisible = false;
            public bool CalibrationControlsVisible {
                get {
                    return calibrationControlsVisible;
                }

                set {
                    calibrationControlsVisible = value;
                }
            }

            private string customizationCode = "";
            /// <summary>
            /// For 2.3, this can be either "NIMO" or blank.
            /// </summary>
            public string CustomizationCode {
                get {

                    return customizationCode;
                }

                set {
                    customizationCode = value;
                }
            }

            private string[] navBarStrings;
            public string[] NavBarStrings {
                get {
                    return navBarStrings;
                }

                set {
                    navBarStrings = value;
                }
            }

            private int[] navBarNodeIds;
            public int[] NavBarNodeIds {
                get {
                    return navBarNodeIds;
                }

                set {
                    navBarNodeIds = value;
                }
            }

            private MsProjectNode.Type[] navBarNodeTypes;
            public MsProjectNode.Type[] NavBarNodeTypes {
                get {
                    return navBarNodeTypes;
                }

                set {
                    navBarNodeTypes = value;
                }
            }

            public ClientSettings() { }
        }

        #endregion

        private void backButton_Click(object sender, EventArgs e)
        {
            if (currentNode == this.dbNode)
            {
                if (this.CloseDb())
                {
                    welcomeBox.Visible = false;
                    // db has been formally closed but they can reopen easily enough

                    returnBox.Visible = true;
                    returnToDbLink.Visible = true;
                    returnToDbLink.Text = "Return to " + dbNode.ToString();

                    if (simControl != null && simControl.RunningSims())
                    {
                        this.dbConnectLink.Enabled = false;
                    }
                    else
                    {
                        this.dbConnectLink.Enabled = true;
                    }
                }
            }
            else if (currentNode != null)
            {
                // change the current node
                ActivateNode( currentNode.ParentNode, true );
            }
        }

        private void dbConnectLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (checkSimStatus())
            {

                MessageBox.Show("Please stop simulations before changing databases");

                return;
            }

            DbMan dlg = new DbMan();

            dlg.ConnectFile = Settings<ClientSettings>.Value.ConnectFile;

            DialogResult rslt = dlg.ShowDialog();

            if (rslt == DialogResult.OK)
            {
                Settings<ClientSettings>.Value.ConnectFile = dlg.ConnectFile;

                msConnect.ConnectFile = Settings<ClientSettings>.Value.ConnectFile;

                string error = null;

                if (!msConnect.TestConnection(out error))
                {
                    MessageBox.Show(error);
                    welcomeBox.Visible = true;

                    returnBox.Visible = false;
                    returnToDbLink.Visible = false;
                    ActivateNode(null);
                    dbOpen(false);
                }
                else
                {
                    if(! this.initializeDb() ) {
                       
                        welcomeBox.Visible = true;
                        returnBox.Visible = false;
                        returnToDbLink.Visible = false;
                    }

                    ActivateNode( dbNode );
                }
            }
        }

        private void returnToDbLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ActivateNode(dbNode);

            dbOpen(true);
        }

        #region StatusShower Members
        public int WaitCursorLevel {
            set {
                MrktSim.waitCursorLevel = value;
            }
            get {
                return MrktSim.waitCursorLevel;
            }
        }

        public void SetStatus( string txt, int percent ) {
            this.toolStripStatusLabel1.Text = txt;
            this.toolStripProgressBar1.Value = percent;
            if( percent > 0 ) {
                this.processActive = true;          // set the flag indicating that a background process is actfive (most menus s/b disabled if it is true)
                this.toolStripProgressBar1.Visible = true;
                this.toolStripEditModelButton.Enabled = false;
                this.toolStripEditScenarioButton.Enabled = false;
            }
            else {
                this.processActive = false;
                this.toolStripProgressBar1.Visible = false;     // hide the progress bar if percent == 0 (show status text only)
            }
        }

        public void ClearStatus() {
            
            this.toolStripStatusLabel1.Text = "";
            this.toolStripProgressBar1.Visible = false;
            this.toolStripProgressBar1.Value = 0;
            this.processActive = false;
            if( this.activeModelNode != null  && this.activeModelNode.Enabled ) {
                this.toolStripEditModelButton.Enabled = true;
            }
            if( this.activeScenarioNode != null && this.activeModelNode != null && this.activeModelNode.Enabled) {
                this.toolStripEditScenarioButton.Enabled = true;
            }

            this.Cursor = Cursors.Default;
        }
        #endregion

        private void MrktSim_SizeChanged( object sender, EventArgs e ) {
            UpdateBoundsSettings();
        }

        private void MrktSim_LocationChanged( object sender, EventArgs e ) {
            UpdateBoundsSettings();
        }

        private void MrktSim_Load( object sender, EventArgs e ) {
            if( Settings<ClientSettings>.Value.MainFrameMaximized == true ) {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        #region SimController

     
        SimControlForm simControl = null;

        ArrayList runningControls = new ArrayList();

        private bool createSimControl()
        {
            if (simControl == null)
            {
                simControl = new SimControlForm(true, Settings<ClientSettings>.Value.ConnectFile);

                if( simControl.SimLocked ) {
                    simControl.Dispose();
                    simControl = null;
                    return false;
                }
                else {
                    simControl.ControlBox = true;
                    simControl.Visible = false;
                    simControl.UpdateSimState += new SimControlForm.SimStateChanged( simControl_UpdateSimState );
                    return true;
                }
            }

            return true;
        }

     

        /// <summary>
        /// Enables controller for running a simulation
        /// </summary>
        /// <param name="control"></param>
        public void RunSim()
        {
            if( createSimControl() ) {

                while( simControl.CheckQueue() ) {
                    // maybe some message to user in status?
                }

                checkSimStatus();
            }
        }

        /// <summary>
        /// Allows the user to stop all running simulations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopAllSimulationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int simCount = 0;
            string msg = stopSimsMsgS;
            if( simControl != null ) {
                simCount= simControl.RunningSimsCount();
            }
            if( simCount == 0 ) {

                checkSimStatus();
                return;
            }
            else if( simCount != 1 ) {
                msg = String.Format( stopSimsMsg, simCount );
            }

            ConfirmDialog cdlg = new ConfirmDialog( msg, stopSimsTitle );
            cdlg.SetOkCancelButtonStyle();
            cdlg.Height += 30;
            cdlg.Width += 125;
            DialogResult resp = cdlg.ShowDialog();
            if( resp != DialogResult.OK ) {

                checkSimStatus();
                return;
            }

            lock (lockObject)
            {
                timer.Stop();

                if (simControl != null)
                {
                    simControl.KillAll();

                    refreshSimulationTablesToolStripMenuItem_Click(sender, e);
                }

                checkSimStatus();
            }
        }

        private void configureSimulationEngineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (simControl == null)
            {
                if( !createSimControl() ) {
                    return;
                }
            }

            simControl.SimConfig();

            checkSimStatus();
        }

        private bool checkSimStatus()
        {
            int simsCount = 0;

            if (simControl != null)
            {
                simsCount = simControl.RunningSimsCount();
                if( simsCount == 0 )
                {
                    simControl.Dispose();
                    simControl = null;
                }
            }

            this.simCountLabel.Text = simsCount.ToString();      // update status bar display

            if( this.activeModelNode != null ) {
                if( this.activeModelNode.Control != null ) {
                    ModelControl modelControl = (ModelControl)this.activeModelNode.Control;
                    modelControl.RefreshCheckpoint( true );
                }
            }

            //if( this.activeScenarioNode != null ) {
            //    if( this.activeScenarioNode.Control != null ) {
            //        ScenarioControl scenarioControl = (ScenarioControl)this.activeScenarioNode.Control;
            //        scenarioControl.UpdateDisplays();
            //    }
            //}

            if( simControl == null )
            {
                this.stopSimsButton.Enabled = false;

                timer.Stop();       
                return false;
            }
            this.stopSimsButton.Enabled = true;
            return true;
        }

        #endregion

        #region Simulation Updating

        object lockObject = new object();

        ArrayList SimsToUpdate = new ArrayList();

        void simControl_UpdateSimState(int id)
        {
            if (this.InvokeRequired)
            {
                SimControlForm.SimStateChanged update = new SimControlForm.SimStateChanged(simControl_UpdateSimState);
                this.Invoke(update, new object[] {id });
            }
            else
            {
                lock (lockObject)
                {
                    if (SimsToUpdate.IndexOf(id) < 0)
                    {
                        SimsToUpdate.Add(id);
                    }

                    if (!timer.Enabled)
                    {
                        timer.Start();
                    }
                }
            }
        }


        private void timer_tick(object sender, EventArgs e)
        {
            lock (lockObject)
            {
                timer.Stop();

                foreach (int sim_id in SimsToUpdate)
                {
                    MrktSimDBSchema.simulationRow sim = dbNode.Db.Data.simulation.FindByid(sim_id);

                    if (sim != null)
                    {
                        int state = dbNode.Db.CheckSimStatus(sim_id);
                        sim.sim_num = state;
                    }
                }

                SimsToUpdate.Clear();
        
                dbNode.Db.Data.AcceptChanges();

                checkSimStatus();
                if( this.activeScenarioNode != null && this.activeScenarioNode.Control != null ) {
                    this.activeScenarioNode.Control.Refresh();
                }
            }
        }

        private void refreshSimulationTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (lockObject)
            {
                timer.Stop();

                foreach (MrktSimDBSchema.simulationRow sim in dbNode.Db.Data.simulation.Select())
                {
                    int state = dbNode.Db.CheckSimStatus(sim.id);
                    sim.sim_num = state;
                }

                this.dbNode.Db.RefreshTable(dbNode.Db.Data.sim_queue, null);

                SimsToUpdate.Clear();
   

                dbNode.Db.Data.AcceptChanges();
            }
        }

        #endregion


        /// <summary>
        /// Shows a dialog allowing the user to do general-level database operations such as clearing all results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripDbButton_Click( object sender, EventArgs e ) {
            int simsCount = 0;
            bool simRunning = false;
            if( simControl != null ){
                simsCount = simControl.RunningSimsCount();
                if( simsCount > 0 ){
                    simRunning = true;
                }
            }

            Common.Dialogs.DbUtils ddlg = new DbUtils( dbNode.Db, dbNode.Text, !simRunning );

            ddlg.ShowDialog();
        }

        /// <summary>
        /// Shows a dialog allowing the user to set  overall options for the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripSettingsButton_Click( object sender, EventArgs e ) {

            Common.Dialogs.MrktSimSettings sdlg = new MrktSimSettings( Settings<MrktSim.ClientSettings>.Value.CalibrationControlsVisible, 
                ModelDb.Nimo == false );

            DialogResult resp = sdlg.ShowDialog();

            if( resp == DialogResult.OK ) {

                // get new values from dialog
                Settings<MrktSim.ClientSettings>.Value.CalibrationControlsVisible = sdlg.EnableCalibrationControls;
            }
        }

        private void ConfigureDevlMode( string confgurationString ) {
            if( confgurationString == null ) {
                return;
            }

            string[] configCodes = confgurationString.Split( ',', ' ' );

            bool configDevlMode = false;
            int configDevlModeNum = -1;

            foreach( string configCode in configCodes ) {
                if( configCode.ToUpper() == "DEVL" ) {
                    configDevlMode = true;
                    configDevlModeNum = 0;
                }
                else if( configCode.ToUpper().StartsWith( "DEVL" )) {
                    configDevlMode = true;
                    configDevlModeNum = 0;
                    string codeVal = configCode.Substring( 4 );
                    try {
                        configDevlModeNum = int.Parse( codeVal );
                    }
                    catch( Exception ) {
                    }
                }
            }

            MrktSim.DevlMode = configDevlMode;
            MrktSim.DevlModeNum = configDevlModeNum;
            ConfigureForDevl( configDevlMode );
        }

        /// <summary>
        /// Configure items that are still in development as needed 
        /// </summary>
        /// <param name="inDevlMode"></param>
        /// <returns></returns>
        private void ConfigureForDevl( bool inDevlMode ) {
            //this.toolStripAddButton.Visible = inDevlMode;
            //this.toolStripEditModelButton.Visible = inDevlMode;
            //this.toolStripEditScenarioButton.Visible = inDevlMode;
            //this.toolStripNavBox.Visible = inDevlMode;
        }

        private void toolStripLogoButton_Click( object sender, EventArgs e ) {
            string versionStr = Application.ProductVersion;
            About aboutDlg = new About( versionStr, Database.AppName );
            aboutDlg.ShowDialog();
        }

        // needed for nav bar, edit model button and edit scenario button
        public static bool DevlMode = false;
        public static int DevlModeNum = 0;

        private string currentDatabaseName = null;
        private string currentProjectName = null;
        private string currentModelName = null;
        private string currentScenarioName = null;
        private MsModelNode activeModelNode = null;
        private MsScenarioNode activeScenarioNode = null;
        private MsProjectNode activeProjectNode = null;
        private ArrayList navBarItemNodes = new ArrayList();

        public MsProjectNode ActiveProjectNode {
            set {
                this.activeProjectNode = value;
                if( value == null ) {
                    return; 
                }

                this.activeModelNode = null;
                this.activeScenarioNode = null;
                this.currentScenarioName = null;
                this.currentModelName = null;
                this.currentProjectName = activeProjectNode.Text;

                if( activeProjectNode.ParentNode != null ) {
                    MsDbNode activeDatabaseNode = (MsDbNode)activeProjectNode.ParentNode;
                    this.currentDatabaseName = activeDatabaseNode.Text;
                }
                else {
                    this.currentDatabaseName = "";
                }

                this.toolStripEditScenarioButton.Enabled = false;
                this.toolStripEditModelButton.Enabled = false;

                UpdateNavigationBarDisplay();
            }
        }

        public MsModelNode ActiveModelNode {
            set {
                this.activeModelNode = value;

                this.activeScenarioNode = null;
                this.currentScenarioName = null;
                this.currentModelName = activeModelNode.Text;
                this.activeProjectNode = (MsProjectNode)activeModelNode.ParentNode;

                if( activeProjectNode != null )
                {
                    this.currentProjectName = activeProjectNode.Text;
                    MsDbNode activeDatabaseNode = (MsDbNode)activeProjectNode.ParentNode;
                    this.currentDatabaseName = activeDatabaseNode.Text;
                }
                

                SetToolstripButtonEnabling();

                UpdateNavigationBarDisplay();
            }
        }

        public MsScenarioNode ActiveScenarioNode {
            set { 
                this.activeScenarioNode = value;

                this.currentScenarioName = this.activeScenarioNode.Text;
                this.activeModelNode = (MsModelNode)this.activeScenarioNode.ParentNode;
                this.currentModelName = activeModelNode.Text;
                this.activeProjectNode = (MsProjectNode)activeModelNode.ParentNode;

                if( activeProjectNode != null )
                {
                    this.currentProjectName = activeProjectNode.Text;
                    MsDbNode activeDatabaseNode = (MsDbNode)activeProjectNode.ParentNode;
                    this.currentDatabaseName = activeDatabaseNode.Text;
                }

                SetToolstripButtonEnabling();

                UpdateNavigationBarDisplay();
            }
        }

        public void SetToolstripButtonEnabling() {
            SetToolstripButtonEnabling( false );
        }

        public void SetToolstripButtonEnabling( bool reactivateCurrentNode ) {

            this.toolStripEditScenarioButton.Enabled = false;
            this.toolStripEditModelButton.Enabled = (this.activeModelNode != null) && this.activeModelNode.Enabled;

            if( this.toolStripEditModelButton.Enabled ) {
                if( this.currentNode is MsScenarioNode ) {
                    this.toolStripEditScenarioButton.Enabled = true;
                }
                else {
                    ModelControl mc = (ModelControl)this.activeModelNode.Control;
                    if( mc != null ) {
                        this.toolStripEditScenarioButton.Enabled = mc.ScenarioIsSelected;
                    }
                }
            }

            if( reactivateCurrentNode ) {
                Status status = new Status( this );
                status.UpdateUIAndContinue( DoReactivateCurrentNode, "updating...", 50 );          
            }
        }

        private void DoReactivateCurrentNode() {
            ActivateNode( currentNode, true );
            Status.ClearStatus( this );
        }

        private void UpdateNavigationBarDisplay() {
            
            string loc = "//" + dbNode.Db.Connection.DataSource;

            if( currentDatabaseName != null ) {

                loc += "/" + currentDatabaseName;

                if( currentProjectName != null ) {
                    loc += "/" + currentProjectName;

                    if( currentModelName != null ) {
                        loc += "/" + currentModelName;

                        if( currentScenarioName != null ) {
                            loc += "/" + currentScenarioName;
                        }
                    }
                }
            }
            else {
                loc += "/";
            }

            this.toolStripNavBox.SelectedIndexChanged -= new EventHandler( toolStripNavBox_SelectedIndexChanged );
            this.toolStripNavBox.Text = loc;
            this.toolStripNavBox.SelectedIndexChanged += new EventHandler( toolStripNavBox_SelectedIndexChanged );
         }

        private void toolStripEditModelButton_Click( object sender, EventArgs e ) {
            if( this.activeModelNode != null && this.processActive == false ){
                ModelControl mc = (ModelControl)activeModelNode.Control;
                toolStripEditModelButton.Enabled = false;
                this.ProcessActive = true;         // disable all menus immediately
                mc.OpenModelFrom( this );
            }
        }

        int maxNavBarItems = 10;

        private void toolStripAddButton_Click( object sender, EventArgs e ) {
            bool addedNode = false;
            if( this.activeScenarioNode != null ) {
                this.navBarItemNodes.Insert( 0, this.activeScenarioNode );
                addedNode = true;
            }
            else if( this.activeModelNode != null ) {
                this.navBarItemNodes.Insert( 0, this.activeModelNode );
                addedNode = true;
            }
            else if( this.activeProjectNode != null ) {
                this.navBarItemNodes.Insert( 0, this.activeProjectNode );
                addedNode = true;
            }

            if( addedNode ) {
                // add the item to the list if it has a corresponding node
                this.toolStripNavBox.Items.Insert( 0, this.toolStripNavBox.Text );

                // remove the last item if we are over the limt
                if( this.navBarItemNodes.Count > maxNavBarItems ) {
                    this.navBarItemNodes.RemoveAt( maxNavBarItems );
                    this.toolStripNavBox.Items.RemoveAt( maxNavBarItems );
                }

                // save these items
                ArrayList navBarPaths = new ArrayList();
                ArrayList navBarNodeIds = new ArrayList();
                ArrayList navBarNodeTypes = new ArrayList();
                int nItems = this.toolStripNavBox.Items.Count;
                for( int i = 0; i < nItems; i++ ) {
                    navBarPaths.Add( (string)this.toolStripNavBox.Items[ i ] );
                    MsProjectNode mspNode = (MsProjectNode)this.navBarItemNodes[ i ];
                    navBarNodeIds.Add( mspNode.Id );
                    navBarNodeTypes.Add( mspNode.NodeType );

                    Settings<ClientSettings>.Value.NavBarStrings = new string[ nItems ];
                    Settings<ClientSettings>.Value.NavBarNodeIds = new int[ nItems ];
                    Settings<ClientSettings>.Value.NavBarNodeTypes = new MsProjectNode.Type[ nItems ];

                    navBarPaths.CopyTo( Settings<ClientSettings>.Value.NavBarStrings );
                    navBarNodeIds.CopyTo( Settings<ClientSettings>.Value.NavBarNodeIds );
                    navBarNodeTypes.CopyTo( Settings<ClientSettings>.Value.NavBarNodeTypes );
                }
            }
        }

        private void toolStripNavBox_SelectedIndexChanged( object sender, EventArgs e ) {
            if( toolStripNavBox.SelectedIndex == -1 ){
                return;
            }
            MsProjectNode selectedNode = (MsProjectNode)this.navBarItemNodes[ toolStripNavBox.SelectedIndex ];
            ActivateNode( selectedNode );
            string nodeInfo = "null";
            if( selectedNode != null ){
                nodeInfo = selectedNode.ToString();
            }
            //Console.WriteLine( "Nav Bar Index Changed: selectedNode = {0}", nodeInfo );
            toolStrip1.Focus();     //un-highlight the text in the nav box by taking away the focus
        }

        private void toolStripEditScenarioButton_Click( object sender, EventArgs e ) {
            EditScenario();
        }

        private ModelDb editScenarioDb;
        private EditScenario editDlg;

        /// <summary>
        /// Allows the user to edit the currently selected scenario.
        /// </summary>
        private void EditScenario() {
            if( this.activeScenarioNode == null ) {
                return;
            }
            ConfirmDialog cdlg = new ConfirmDialog( confirmEditScenarioMessage, confirmEditScenarioTitle );
            cdlg.SetOkCancelButtonStyle();
            cdlg.SelectQuestionIcon();
            cdlg.Width -= 270;
            DialogResult resp = cdlg.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            editScenarioDb = new ModelDb();
            editScenarioDb.Connection = this.activeModelNode.Db.Connection;
            editScenarioDb.ModelID = this.activeModelNode.Id;
 
            this.ProcessActive = true;         // disable all menus immediately

            Status status = new Status( this );
            System.Threading.Thread backgroundThread = status.StartBackgroundThread( PrepareForEditingScenario );

            // update the status while the background thread runs
            string statusMsg = String.Format( editScenarioStatusMsg, activeScenarioNode.Scenario.name );
            //status.UpdateUIAndContinue( EditScenario_Part2, statusMsg, 50 );
            status.UpdateUIAndContinue( EditScenario_Part2, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void PrepareForEditingScenario() {

            Utilities.Status.SetWaitCursor( this );

            editScenarioDb.Refresh();

            editDlg = new EditScenario( null, editScenarioDb, true );
            editDlg.CurrentScenario = activeScenarioNode.Scenario;

            MarketPlanControlRelater relater = new MarketPlanControlRelater( editScenarioDb );
            ArrayList used = relater.GetPlansForScenario( activeScenarioNode.Scenario.scenario_id );
            editDlg.SetUsedList( used );

            Utilities.Status.ClearWaitCursor( this );

        }

        private void EditScenario_Part2()
        {
            ClearStatus();

            DialogResult resp = editDlg.ShowDialog();

            if( resp != DialogResult.OK ) {
                return;
            }
            
            this.ProcessActive = true;         // disable all menus immediately
            Status status = new Status( this );
            System.Threading.Thread backgroundThread = status.StartBackgroundThread( FinishEditingScenario );

            // update the status while the background thread runs
            string statusMsg = String.Format( editScenarioStatusMsg2, activeScenarioNode.Scenario.name );
            status.UpdateUIAndContinue( ClearStatus, backgroundThread, 200, statusMsg, 1, 60, "Processing..." );
        }

        private void FinishEditingScenario() {

            Utilities.Status.SetWaitCursor( this );

            ArrayList removedIDs = editDlg.RemovedUsedIDs;
            ArrayList addedIDs = editDlg.AddedUsedIDs;

            if( removedIDs.Count > 0 || addedIDs.Count > 0 ) {
                // we have items to add or remove from scenario_market_plans
                MrktSimClient.Controls.MarketPlans.MarketPlanScenarioPicker.AddAndRemovePlans( addedIDs, removedIDs, activeScenarioNode.Scenario, editScenarioDb );
            }
            // save the changes
            editScenarioDb.Update();


            Utilities.Status.ClearWaitCursor( this );

        }

        //Don't let the user type anything into the nav bar
        private void toolStripNavBox_KeyPress( object sender, KeyPressEventArgs e ) {
            e.Handled = true;
        }

        private void toolStripHelpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, "MainForm" );
        }

        private void MrktSim_Shown( object sender, EventArgs e ) {
        }

        private void toolStripRefreshButton_Click( object sender, EventArgs e ) {

            dbNode.Db.Refresh();
            dbNode.RefreshTree();

            if( currentNode != null && currentNode.Control != null )
            {
                this.currentNode.Control.Refresh();
            }
            
        }
    }
}