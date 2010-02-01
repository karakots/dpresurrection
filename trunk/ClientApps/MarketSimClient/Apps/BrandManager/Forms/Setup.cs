using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MrktSimDb;
// using Common.Dialogs;
using BrandManager.Dialogues;

namespace BrandManager.Forms
{
	/// <summary>
	/// Summary description for Setup.
	/// </summary>
	public class Setup : System.Windows.Forms.UserControl, Wizard
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label connectionStatus;
		private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label selectedProject;
		private System.Windows.Forms.Label selectedModel;
		private System.Windows.Forms.Button projectSelectButton;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label selectedDirectory;
		private System.Windows.Forms.Button directorySelectButton;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox userName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox connectBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		static Setup()
		{
			Settings.Read("BrandManSetup.txt");
		}

		public Setup()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// start off with no database
			Db = null;

			// see if we have a connection
			msConnect = new MSConnect(Application.StartupPath);

			modelInfoDb = new ModelInfoDb();
			
			if (!msConnect.TestConnection())
			{
				Settings.Connected = false;
				projectSelectButton.Enabled = false;
			}
			else
			{
				Settings.Connected = true;
				connectionStatus.Text = "Connected";
				projectSelectButton.Enabled = true;
			

				if (Settings.Project != Database.AllID)
				{
					// try to read in project name
					modelInfoDb.Connection = msConnect.Connection;

					modelInfoDb.ReadProjects();

					MrktSimDb.ModelInfo.projectRow project = modelInfoDb.ModelData.project.FindByid(Settings.Project);

					if (project == null)
					{
						// this is a bad id for some reason
						Settings.Project = Database.AllID;
					}
					else
					{
						selectedProject.Text = project.name;
						modelInfoDb.ProjectID = project.id;

						// now read in database
						if (Settings.Model != Database.AllID)
						{
							// new empty database
							Db = new Database();
							Db.Connection = msConnect.Connection;
							Db.CurrentModel = Settings.Model;
			
							try
							{
								Db.OpenForAddOnly();

								Db.ReadModelForBrandManager();
								
								selectedModel.Text = Db.Model.model_name;
							}
							catch(Exception)
							{
								Settings.Model = Database.AllID;
							}
						}
					}
				}
			}

			if (Settings.LocalDirectory != null)
			{
				this.selectedDirectory.Text = Settings.LocalDirectory;
			}

			this.userName.Text = Settings.User;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.connectionStatus = new System.Windows.Forms.Label();
			this.connectButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.selectedProject = new System.Windows.Forms.Label();
			this.selectedModel = new System.Windows.Forms.Label();
			this.projectSelectButton = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.selectedDirectory = new System.Windows.Forms.Label();
			this.directorySelectButton = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.userName = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.connectBox = new System.Windows.Forms.GroupBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.connectBox.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Status:";
			// 
			// connectionStatus
			// 
			this.connectionStatus.Location = new System.Drawing.Point(80, 24);
			this.connectionStatus.Name = "connectionStatus";
			this.connectionStatus.Size = new System.Drawing.Size(88, 24);
			this.connectionStatus.TabIndex = 1;
			this.connectionStatus.Text = "Not Connected";
			// 
			// connectButton
			// 
			this.connectButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.connectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.connectButton.Location = new System.Drawing.Point(392, 24);
			this.connectButton.Name = "connectButton";
			this.connectButton.Size = new System.Drawing.Size(75, 24);
			this.connectButton.TabIndex = 2;
			this.connectButton.Text = "Connect...";
			this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "Current Project:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "Current Model:";
			// 
			// selectedProject
			// 
			this.selectedProject.Location = new System.Drawing.Point(120, 24);
			this.selectedProject.Name = "selectedProject";
			this.selectedProject.Size = new System.Drawing.Size(344, 23);
			this.selectedProject.TabIndex = 5;
			this.selectedProject.Text = "<Select Project>";
			// 
			// selectedModel
			// 
			this.selectedModel.Location = new System.Drawing.Point(120, 48);
			this.selectedModel.Name = "selectedModel";
			this.selectedModel.Size = new System.Drawing.Size(336, 16);
			this.selectedModel.TabIndex = 6;
			this.selectedModel.Text = "<Select Model>";
			// 
			// projectSelectButton
			// 
			this.projectSelectButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.projectSelectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.projectSelectButton.Location = new System.Drawing.Point(304, 72);
			this.projectSelectButton.Name = "projectSelectButton";
			this.projectSelectButton.Size = new System.Drawing.Size(160, 23);
			this.projectSelectButton.TabIndex = 7;
			this.projectSelectButton.Text = "Select Project and Model...";
			this.projectSelectButton.Click += new System.EventHandler(this.projectSelectButton_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(24, 24);
			this.label6.Name = "label6";
			this.label6.TabIndex = 9;
			this.label6.Text = "Default Directory:";
			// 
			// selectedDirectory
			// 
			this.selectedDirectory.Location = new System.Drawing.Point(128, 24);
			this.selectedDirectory.Name = "selectedDirectory";
			this.selectedDirectory.Size = new System.Drawing.Size(336, 24);
			this.selectedDirectory.TabIndex = 10;
			this.selectedDirectory.Text = "<none selected>";
			// 
			// directorySelectButton
			// 
			this.directorySelectButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.directorySelectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.directorySelectButton.Location = new System.Drawing.Point(392, 56);
			this.directorySelectButton.Name = "directorySelectButton";
			this.directorySelectButton.TabIndex = 11;
			this.directorySelectButton.Text = "Browse...";
			this.directorySelectButton.Click += new System.EventHandler(this.directorySelectButton_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 23);
			this.label4.TabIndex = 12;
			this.label4.Text = "User Name:";
			// 
			// userName
			// 
			this.userName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.userName.Location = new System.Drawing.Point(120, 24);
			this.userName.Name = "userName";
			this.userName.TabIndex = 13;
			this.userName.Text = "";
			this.userName.TextChanged += new System.EventHandler(this.userName_TextChanged);
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(24, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(248, 23);
			this.label5.TabIndex = 14;
			this.label5.Text = "Brand Manager Setup";
			// 
			// connectBox
			// 
			this.connectBox.Controls.Add(this.connectButton);
			this.connectBox.Controls.Add(this.connectionStatus);
			this.connectBox.Controls.Add(this.label1);
			this.connectBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.connectBox.Location = new System.Drawing.Point(24, 48);
			this.connectBox.Name = "connectBox";
			this.connectBox.Size = new System.Drawing.Size(480, 56);
			this.connectBox.TabIndex = 15;
			this.connectBox.TabStop = false;
			this.connectBox.Text = "Step 1: Connect to a database";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.selectedModel);
			this.groupBox1.Controls.Add(this.projectSelectButton);
			this.groupBox1.Controls.Add(this.selectedProject);
			this.groupBox1.Location = new System.Drawing.Point(24, 128);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(480, 104);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Step 2: Select project and model";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.directorySelectButton);
			this.groupBox2.Controls.Add(this.selectedDirectory);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Location = new System.Drawing.Point(24, 256);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(480, 88);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Step 3: Set default directory";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.userName);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Location = new System.Drawing.Point(24, 368);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(480, 56);
			this.groupBox3.TabIndex = 18;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Step 4: Enter user name";
			// 
			// Setup
			// 
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.connectBox);
			this.Controls.Add(this.label5);
			this.Name = "Setup";
			this.Size = new System.Drawing.Size(544, 464);
			this.connectBox.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

	
		#region member data
		MSConnect msConnect = null;
		private Database database = null;
		private ModelInfoDb modelInfoDb;
		public Database Db
		{
			get
			{
				return database;
			}

			set
			{
				database = value;
			}
		}


		#endregion
		
		#region Wizard Members

		public bool Next()
		{
			if ( Settings.Connected &&
				Settings.Project != Database.AllID &&
				Settings.Model != Database.AllID &&
				Settings.LocalDirectory != null &&
				Db != null &&
				Settings.User != null &&
				Settings.User.Length != 0)
			{
				if(Settings.User == "admin" || Settings.User == "")
				{
					MessageBox.Show("Invalid User Name", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information);	
					return false;
				}
				return true;
			}
			MessageBox.Show("Please connect to a database, select a model, and a default directory", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return false;
		}

		public bool Back()
		{
			// TODO:  Add AddCompetitor.Back implementation
			return false;
		}

		public void Start()
		{
		}

		public void End()
		{
		}

		public event BrandManager.Forms.Finished Done;

		#endregion

		#region UI Methods
		private void connectButton_Click(object sender, System.EventArgs e)
		{
			string error = null;

			if (!msConnect.Connect(out error))
			{
				MessageBox.Show(error);
				Settings.Connected = false;
				this.connectionStatus.Text = "Not Connected";
				projectSelectButton.Enabled = false;
			}
			else
			{
				//				// check for engine connect file
				//				string msEngineConnectFile = Application.StartupPath + @"\..\MarketSimEngine\dbconnect";
				//				FileInfo fi = new FileInfo(msEngineConnectFile);
				//				if (fi.Exists)
				//				{
				//					// ask user if she wants to reset connection for sim engine
				//					DialogResult res = MessageBox.Show(this, "Do you wish to reset the Simulation Engine?", "Reset Engine", MessageBoxButtons.YesNo);
				//
				//					if (res == DialogResult.Yes)
				//					{
				//						try
				//						{
				//							fi.Delete();
				//						}
				//						catch(Exception oops)
				//						{
				//							MessageBox.Show("Error removing sim engine connection file: System error = " + oops.Message);
				//						}
				//					}
				//				}
				// test -- Now I am connected

				Settings.Connected = true;
				this.connectionStatus.Text = "Connected";
				// this is a bad id for some reason
				Settings.Project = Database.AllID;
				Settings.Model = Database.AllID;

				selectedProject.Text = "<Select Project>";
				selectedModel.Text = "<Select Model>";
				projectSelectButton.Enabled = true;
			}
		}

		private void projectSelectButton_Click(object sender, System.EventArgs e)
		{
			
			modelInfoDb.Connection = msConnect.Connection;

			modelInfoDb.ReadProjects();

			OpenTable projDlg = new OpenTable(modelInfoDb.ModelData.project, "name");

			DialogResult rslt = projDlg.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				// Settings.Project = dlg.ProjectID;

				MrktSimDb.ModelInfo.projectRow project = (MrktSimDb.ModelInfo.projectRow) projDlg.SelectedRow();

				// MrktSimDb.ModelInfo.projectRow project = modelInfoDb.ModelData.project.FindByid(Settings.Project);

				if (project == null)
				{
					// this is a bad id for some reason
					Settings.Project = Database.AllID;
					Settings.Model = Database.AllID;

					selectedProject.Text = "<Select Project>";
					selectedModel.Text = "<Select Model>";
				}
				else
				{
					if (Settings.Project != project.id)
					{
						Settings.Model = Database.AllID;
						selectedModel.Text = "<Select Model>";
					}

					selectedProject.Text = project.name;
					Settings.Project = project.id;
				
					// now select model for this project
					modelInfoDb.ProjectID = Settings.Project;
					modelInfoDb.ReadModels();

					OpenTable modelDlg = new OpenTable(modelInfoDb.ModelData.Model_info, "model_name");

					modelDlg.Text = "Select Model";

					rslt = modelDlg.ShowDialog();

					if (rslt == DialogResult.OK)
					{
						Settings.Model = Database.AllID;
						selectedModel.Text = "<Select Model>";

						// Settings.Project = dlg.ProjectID;
						MrktSimDb.ModelInfo.Model_infoRow model = (MrktSimDb.ModelInfo.Model_infoRow) modelDlg.SelectedRow();

						// MrktSimDb.ModelInfo.projectRow project = modelInfoDb.ModelData.project.FindByid(Settings.Project);

						if (model != null)
						{
							Settings.Model = model.model_id;

							Db = new Database();
							Db.Connection = msConnect.Connection;
							Db.CurrentModel = Settings.Model;
			
							try
							{
								Db.OpenForAddOnly();

								Db.ReadModelForBrandManager();
								
								selectedModel.Text = Db.Model.model_name;
								Settings.Model = Db.Model.model_id;
							}
							catch(Exception modelErr)
							{
								MessageBox.Show("Error opening model: " + modelErr.ToString());
							}
						}
					}
				}
			}

			if (Settings.Project == Database.AllID)
			{	
				selectedProject.Text = "<No Project Selected>";
			}

		}

		private void directorySelectButton_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog folderBrowse = new FolderBrowserDialog();

			DialogResult rslt = folderBrowse.ShowDialog();

			if (rslt == DialogResult.OK)
			{
				Settings.LocalDirectory = folderBrowse.SelectedPath;
				selectedDirectory.Text = Settings.LocalDirectory;
			}
		}
	
		
		#endregion

		private void button2_Click(object sender, System.EventArgs e)
		{
		
		}

		private void userName_TextChanged(object sender, System.EventArgs e)
		{
			Settings.User = userName.Text;
		}

		#region Settings
		/// <summary>
		/// Everyone should have Settings.
		/// </summary>
		[SerializableAttribute]
			public class Settings : MarketSimSettings.Settings
		{
			private bool connected = false;
			static public bool Connected
			{
				get
				{
					return ((Settings) localSettings).connected;
				}

				set
				{
					((Settings) localSettings).connected = value;
				}
			}

			private int databaseID;
			static public int Model
			{
				get
				{
					return ((Settings) localSettings).databaseID;
				}

				set
				{
					((Settings) localSettings).databaseID = value;
				}
			}

			private int projectID;
			static public int Project
			{
				get
				{
					return ((Settings) localSettings).projectID;
				}

				set
				{
					((Settings) localSettings).projectID = value;
				}
			}

			private string localDirectory = null;
			static public string LocalDirectory
			{
				get
				{
					return ((Settings) localSettings).localDirectory;
				}

				set
				{
					((Settings) localSettings).localDirectory = value;
				}
			}

			private string user;
			static public string User
			{
				get
				{
					return ((Settings) localSettings).user;
				}

				set
				{
					((Settings) localSettings).user = value;
				}
			}

			private string product_string;
			static public string Products
			{
				get
				{
					return ((Settings) localSettings).product_string;
				}

				set
				{
					((Settings) localSettings).product_string = value;
				}
			}

			static Settings()
			{
				localSettings = new Settings();
			}
		
			// this is a singleton
			private Settings()
			{
				connected = false;
				databaseID = Database.AllID;
				projectID =  Database.AllID;

				localDirectory = null;
				user = null;
				product_string = null;
			}
		}

		#endregion
	}
}
