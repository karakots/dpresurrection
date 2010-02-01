using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO; // file find
using System.Diagnostics; // process

using SimControlMethods;

using MarketSimSettings;

using DatabaseManager;

using DPLicense;

namespace SimControl.Dialogues
{
	/// <summary>
	/// Summary description for Setup.
	/// </summary>
	public class Setup : System.Windows.Forms.Form
	{
		private const string EngineConnected = "Simulation Engine connected to database";
		private const string EngineNotConnected = "Simulation Engine NOT connected to database";
        private const string ResetEngine = "Reset Engine Data Source";
        private const string CreateEngine = "Create Engine Data Source";

        private string dbConnectFile = null;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label engineName;
		private System.Windows.Forms.Label pathName;
		private System.Windows.Forms.Button setEngineButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label dbName;
		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.Button resetDataSourceButton;
		private System.Windows.Forms.NumericUpDown timeControl;
		private System.Windows.Forms.Label refreshLabel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown numSims;
		private System.Windows.Forms.Label numWeeksLabel_1;
		private System.Windows.Forms.NumericUpDown numWeeks;
		private System.Windows.Forms.Label numWeeksLabel_2;
        private System.Windows.Forms.Label engineConnection;
        private Button DoneButton;
        private Label label4;
        private Label licenseLabel;
        private Button LicenseBtn;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public string DbConnectFile
        {
            get
            {
                return this.dbConnectFile;
            }

            set
            {
                dbConnectFile = value;
            }
        }

		public Setup(string connectFile, bool doNotAllowDbChange)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            DbConnectFile = connectFile;

            this.engineName.Text = Settings<SimControlForm.SimControlSettings>.Value.SimFile;
            this.pathName.Text = Settings<SimControlForm.SimControlSettings>.Value.SimDirectory;

			this.dbName.Text = SimController.ModelName;

            this.timeControl.Value = Settings<SimControlForm.SimControlSettings>.Value.RefreshRate;

            this.numSims.Value = Settings<SimControlForm.SimControlSettings>.Value.NumSims;
            this.numWeeks.Value = Settings<SimControlForm.SimControlSettings>.Value.NumWeeks;

            string user = Settings<SimControlForm.SimControlSettings>.Value.UserName;
            string key = Settings<SimControlForm.SimControlSettings>.Value.LicenseKey;

            if (SimControlForm.CheckLicense(user, key))
            {
                licenseLabel.Text = user;
            }
            else
            {
                licenseLabel.Text = "Invalid";
            }

            if (doNotAllowDbChange)
            {
                
                this.numWeeks.Visible = false;
                this.connectButton.Visible = false;

                this.numWeeksLabel_1.Visible = false;
                this.numWeeksLabel_2.Visible = false;

                this.timeControl.Visible = false;
                this.refreshLabel.Visible = false;
            }

			checkForDataSource();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.engineName = new System.Windows.Forms.Label();
            this.pathName = new System.Windows.Forms.Label();
            this.setEngineButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dbName = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.resetDataSourceButton = new System.Windows.Forms.Button();
            this.timeControl = new System.Windows.Forms.NumericUpDown();
            this.refreshLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numSims = new System.Windows.Forms.NumericUpDown();
            this.numWeeksLabel_1 = new System.Windows.Forms.Label();
            this.numWeeks = new System.Windows.Forms.NumericUpDown();
            this.numWeeksLabel_2 = new System.Windows.Forms.Label();
            this.engineConnection = new System.Windows.Forms.Label();
            this.DoneButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.licenseLabel = new System.Windows.Forms.Label();
            this.LicenseBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.timeControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSims)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWeeks)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point( 16, 16 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 72, 23 );
            this.label1.TabIndex = 0;
            this.label1.Text = "SimEngine";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point( 16, 40 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 32, 23 );
            this.label2.TabIndex = 1;
            this.label2.Text = "Path";
            // 
            // engineName
            // 
            this.engineName.Location = new System.Drawing.Point( 80, 16 );
            this.engineName.Name = "engineName";
            this.engineName.Size = new System.Drawing.Size( 256, 23 );
            this.engineName.TabIndex = 2;
            this.engineName.Text = "MarketSimEngine.exe";
            // 
            // pathName
            // 
            this.pathName.Location = new System.Drawing.Point( 80, 40 );
            this.pathName.Name = "pathName";
            this.pathName.Size = new System.Drawing.Size( 376, 16 );
            this.pathName.TabIndex = 3;
            this.pathName.Text = "C:\\Program Files\\DiscesionPower\\MarketSimEngine";
            // 
            // setEngineButton
            // 
            this.setEngineButton.Location = new System.Drawing.Point( 83, 59 );
            this.setEngineButton.Name = "setEngineButton";
            this.setEngineButton.Size = new System.Drawing.Size( 144, 23 );
            this.setEngineButton.TabIndex = 4;
            this.setEngineButton.Text = "Set Simulation Engine..";
            this.setEngineButton.Click += new System.EventHandler( this.setEngineButton_Click );
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point( 16, 146 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 56, 16 );
            this.label3.TabIndex = 5;
            this.label3.Text = "Database:";
            // 
            // dbName
            // 
            this.dbName.Location = new System.Drawing.Point( 80, 139 );
            this.dbName.Name = "dbName";
            this.dbName.Size = new System.Drawing.Size( 312, 23 );
            this.dbName.TabIndex = 6;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point( 83, 171 );
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size( 75, 23 );
            this.connectButton.TabIndex = 7;
            this.connectButton.Text = "Connect..";
            this.connectButton.Click += new System.EventHandler( this.connectButton_Click );
            // 
            // resetDataSourceButton
            // 
            this.resetDataSourceButton.Location = new System.Drawing.Point( 83, 111 );
            this.resetDataSourceButton.Name = "resetDataSourceButton";
            this.resetDataSourceButton.Size = new System.Drawing.Size( 160, 23 );
            this.resetDataSourceButton.TabIndex = 8;
            this.resetDataSourceButton.Text = "Reset Engine Data Source..";
            this.resetDataSourceButton.Click += new System.EventHandler( this.resetDataSourceButton_Click );
            // 
            // timeControl
            // 
            this.timeControl.Location = new System.Drawing.Point( 208, 255 );
            this.timeControl.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.timeControl.Name = "timeControl";
            this.timeControl.Size = new System.Drawing.Size( 56, 20 );
            this.timeControl.TabIndex = 9;
            this.timeControl.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.timeControl.ValueChanged += new System.EventHandler( this.timeControl_ValueChanged );
            // 
            // refreshLabel
            // 
            this.refreshLabel.Location = new System.Drawing.Point( 16, 257 );
            this.refreshLabel.Name = "refreshLabel";
            this.refreshLabel.Size = new System.Drawing.Size( 104, 23 );
            this.refreshLabel.TabIndex = 10;
            this.refreshLabel.Text = "Refresh (seconds)";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point( 16, 280 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 184, 23 );
            this.label5.TabIndex = 11;
            this.label5.Text = "Number of concurrent simulations";
            // 
            // numSims
            // 
            this.numSims.Location = new System.Drawing.Point( 208, 278 );
            this.numSims.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numSims.Name = "numSims";
            this.numSims.Size = new System.Drawing.Size( 56, 20 );
            this.numSims.TabIndex = 12;
            this.numSims.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numSims.ValueChanged += new System.EventHandler( this.numSims_ValueChanged );
            // 
            // numWeeksLabel_1
            // 
            this.numWeeksLabel_1.Location = new System.Drawing.Point( 16, 303 );
            this.numWeeksLabel_1.Name = "numWeeksLabel_1";
            this.numWeeksLabel_1.Size = new System.Drawing.Size( 152, 23 );
            this.numWeeksLabel_1.TabIndex = 13;
            this.numWeeksLabel_1.Text = "Display Simulations for last";
            // 
            // numWeeks
            // 
            this.numWeeks.Location = new System.Drawing.Point( 208, 301 );
            this.numWeeks.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numWeeks.Name = "numWeeks";
            this.numWeeks.Size = new System.Drawing.Size( 56, 20 );
            this.numWeeks.TabIndex = 14;
            this.numWeeks.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numWeeks.ValueChanged += new System.EventHandler( this.numWeeks_ValueChanged );
            this.numWeeks.Leave += new System.EventHandler( this.numSims_ValueChanged );
            // 
            // numWeeksLabel_2
            // 
            this.numWeeksLabel_2.Location = new System.Drawing.Point( 270, 303 );
            this.numWeeksLabel_2.Name = "numWeeksLabel_2";
            this.numWeeksLabel_2.Size = new System.Drawing.Size( 56, 23 );
            this.numWeeksLabel_2.TabIndex = 15;
            this.numWeeksLabel_2.Text = "weeks";
            // 
            // engineConnection
            // 
            this.engineConnection.Location = new System.Drawing.Point( 80, 85 );
            this.engineConnection.Name = "engineConnection";
            this.engineConnection.Size = new System.Drawing.Size( 272, 23 );
            this.engineConnection.TabIndex = 16;
            this.engineConnection.Text = "Simulation Engine NOT connected to database";
            // 
            // DoneButton
            // 
            this.DoneButton.Location = new System.Drawing.Point( 441, 304 );
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size( 58, 28 );
            this.DoneButton.TabIndex = 18;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler( this.DoneButton_Click );
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 16, 217 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 47, 13 );
            this.label4.TabIndex = 19;
            this.label4.Text = "License:";
            // 
            // licenseLabel
            // 
            this.licenseLabel.Location = new System.Drawing.Point( 85, 217 );
            this.licenseLabel.Name = "licenseLabel";
            this.licenseLabel.Size = new System.Drawing.Size( 91, 23 );
            this.licenseLabel.TabIndex = 20;
            this.licenseLabel.Text = "Expired";
            // 
            // LicenseBtn
            // 
            this.LicenseBtn.Location = new System.Drawing.Point( 182, 212 );
            this.LicenseBtn.Name = "LicenseBtn";
            this.LicenseBtn.Size = new System.Drawing.Size( 99, 23 );
            this.LicenseBtn.TabIndex = 21;
            this.LicenseBtn.Text = "Enter License";
            this.LicenseBtn.UseVisualStyleBackColor = true;
            this.LicenseBtn.Click += new System.EventHandler( this.LicenceBtn_Click );
            // 
            // Setup
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.ClientSize = new System.Drawing.Size( 522, 344 );
            this.ControlBox = false;
            this.Controls.Add( this.LicenseBtn );
            this.Controls.Add( this.licenseLabel );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.DoneButton );
            this.Controls.Add( this.engineConnection );
            this.Controls.Add( this.numWeeksLabel_2 );
            this.Controls.Add( this.numWeeks );
            this.Controls.Add( this.numWeeksLabel_1 );
            this.Controls.Add( this.numSims );
            this.Controls.Add( this.label5 );
            this.Controls.Add( this.refreshLabel );
            this.Controls.Add( this.timeControl );
            this.Controls.Add( this.resetDataSourceButton );
            this.Controls.Add( this.connectButton );
            this.Controls.Add( this.dbName );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.setEngineButton );
            this.Controls.Add( this.pathName );
            this.Controls.Add( this.engineName );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Setup";
            this.Text = "Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.Setup_FormClosing );
            ((System.ComponentModel.ISupportInitialize)(this.timeControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSims)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWeeks)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

		private void connectButton_Click(object sender, System.EventArgs e)
		{
            DbMan dlg = new DbMan();

            dlg.ConnectFile = DbConnectFile;
              
            DialogResult rslt = dlg.ShowDialog();

            if (rslt == DialogResult.OK)
            {
                string error = null;

                if (!SimController.OpenModel(Application.StartupPath, dlg.ConnectFile, out error))
                {
                    MessageBox.Show(error);
                    this.dbName.Text = "Not Connected";
                }
                else
                {
                    this.dbName.Text = SimController.ModelName;
                    DbConnectFile = dlg.ConnectFile;
                }
            }

            checkForDataSource();
		}

		private FileInfo checkForDataSource()
		{
			// check for engine connect file
            string msEngineConnectFile = Settings<SimControlForm.SimControlSettings>.Value.SimDirectory + @"\connect\" + SimController.EngineConnectFile;
			FileInfo fi =  new FileInfo(msEngineConnectFile);

        
            if (fi.Exists)
            {
                this.engineConnection.Text = EngineConnected;
                this.resetDataSourceButton.Text = ResetEngine;
            }
            else
            {
                this.engineConnection.Text = EngineNotConnected;
                this.resetDataSourceButton.Text = CreateEngine;
            }

            if( dbConnectFile == null ) {
                this.resetDataSourceButton.Enabled = false;
                this.setEngineButton.Enabled = false;
            }
            else {
                this.resetDataSourceButton.Enabled = true;
                this.setEngineButton.Enabled = true;
            }


            return fi;
		}

		private void resetDataSourceButton_Click(object sender, System.EventArgs e)
		{
			FileInfo fi = checkForDataSource();

			if (fi.Exists)
			{
				fi.Delete();
			}

            try {
                // start up the sim engine to set the datasource
                Process newProcess = new Process();
                newProcess.StartInfo.FileName = Settings<SimControlForm.SimControlSettings>.Value.SimFile;
                newProcess.StartInfo.WorkingDirectory = Settings<SimControlForm.SimControlSettings>.Value.SimDirectory;
                
                // newProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                newProcess.StartInfo.Arguments = "-f " + SimController.EngineConnectFile + " -s " + -2;

                newProcess.Start();

                newProcess.WaitForExit();

                int rval = newProcess.ExitCode;

                if( rval > 0 ) {
                    MessageBox.Show( "MarketSim Engine successfully connected" );
                }
                else {
                    MessageBox.Show( "Error connecting MarketSim Engine" );
                }
            }
            catch( Exception err ) {
                MessageBox.Show( err.Message ); 
            }

			checkForDataSource();
		}

		private void setEngineButton_Click(object sender, System.EventArgs e)
		{
			// try to find a simulation engine
			System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			openFileDlg.CheckFileExists = true;
			openFileDlg.CheckPathExists = true;

			openFileDlg.FileName = Settings<SimControlForm.SimControlSettings>.Value.SimFile;
			openFileDlg.InitialDirectory = Settings<SimControlForm.SimControlSettings>.Value.SimDirectory;

			openFileDlg.DefaultExt = ".exe";
			openFileDlg.Filter = "MarketSim Simulation File (*.exe)|*.exe";

			DialogResult rslt = openFileDlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				string fileName = openFileDlg.FileName;

				int trimDex = fileName.LastIndexOf(@"\");

				this.pathName.Text = fileName.Substring(0, trimDex);
				Settings<SimControlForm.SimControlSettings>.Value.SimDirectory = pathName.Text;
				
				if (trimDex < fileName.Length)
				{
					this.engineName.Text = fileName.Substring(trimDex + 1);
					Settings<SimControlForm.SimControlSettings>.Value.SimFile = engineName.Text;
				}

				// check if we should enable the reset button
				this.resetDataSourceButton.Enabled = true;
				checkForDataSource();
			}
		}

		private void timeControl_ValueChanged(object sender, System.EventArgs e)
		{
			Settings<SimControlForm.SimControlSettings>.Value.RefreshRate = (int) timeControl.Value;
		}

		private void numWeeks_ValueChanged(object sender, System.EventArgs e)
		{
			Settings<SimControlForm.SimControlSettings>.Value.NumWeeks = (int) numWeeks.Value;
		}

		private void numSims_ValueChanged(object sender, System.EventArgs e)
		{
			Settings<SimControlForm.SimControlSettings>.Value.NumSims = (int) numSims.Value;
		}

        private void Setup_FormClosing( object sender, FormClosingEventArgs e ) {
            // save the settings whenever the user has specified a non-blank sim engine name
            if( this.pathName.Text.Trim().Length > 0 ) {
                Settings<SimControlForm.SimControlSettings>.Save();
            }
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void LicenceBtn_Click(object sender, EventArgs e)
        {
            LicenseDialog dlg = new LicenseDialog();
            DialogResult rslt = dlg.ShowDialog();
            if (rslt == DialogResult.OK)
            {
                if (SimControlForm.CheckLicense(dlg.UserName, dlg.LicenseKey))
                {
                    Settings<SimControlForm.SimControlSettings>.Value.UserName = dlg.UserName;
                    Settings<SimControlForm.SimControlSettings>.Value.LicenseKey = dlg.LicenseKey;
                    licenseLabel.Text = dlg.UserName;
                }
                else
                {
                    DialogResult mrslt = MessageBox.Show("Invalid or Expired license entered.", "Invalid License", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (mrslt == DialogResult.OK)
                    {
                        Settings<SimControlForm.SimControlSettings>.Value.UserName = dlg.UserName;
                        Settings<SimControlForm.SimControlSettings>.Value.LicenseKey = dlg.LicenseKey;
                        licenseLabel.Text = "Invalid";
                    }
                }

            }
        }
	}
}
