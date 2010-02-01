using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using Utilities;
using MrktSimDb;
using BrandManager.Forms;
using BrandManager.Dialogues;
using MarketSimUtilities;

namespace BrandManager
{
	/// <summary>
	/// Summary description for WizTest.
	/// </summary>
	public class WizTest : System.Windows.Forms.Form
	{
		private LinkButton SetUpLink;
		private LinkButton StartLink;
		private LinkButton SelectScenarioLink;
		private LinkButton ModScenarioLink;
		private LinkButton ReviewScenarioLink;
		// private LinkButton MetricsLink; not used for now
		private LinkButton RunLink;
		private LinkButton ResultsLink;
		private Setup SetUpControl;
		private CheckScenario CheckScenarioControl;
		private SelectScenario SelectScenarioControl;
		private CreateScenario CreateScenarioControl;
		private ScenarioView ScenarioViewControl;
		private EditChoice01 EditChoice01Control;
		private EditChoice02 EditChoice02Control;
		private EditChoice03 EditChoice03Control;
		private AddCompetitor AddCompetitorControl;
		private AdvancedOptions AdvancedOptionsControl;
		private EditScenario EditScenarioControl;
		private ResultsViewing ResultsViewingControl;
		private ResultsToFile ResultsToFileControl;
		private ResultChoice01 ResultChoice01Control;
		private ResultChoice02 ResultChoice02Control;
		private RunSims RunSimsControl;
		//private SelectMetrics SelectMetricsControl;
		private NewScenario NewScenarioControl;
		private SaveScenario SaveScenarioControl;



		private ArrayList links;
		private ArrayList cntrls;
		private System.Windows.Forms.Label Next;
		private System.Windows.Forms.Label Back;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label SelectLabel;
		private System.Windows.Forms.Label UnSelectLabel;
		private System.Windows.Forms.Label minButton;
		private System.Windows.Forms.Label killButton;
		private System.Windows.Forms.LinkLabel HelpLink;

		private enum wiz
		{
			setup,
			selectscenario01,
			selectscenario02,
			selectscenario03,
			selectscenario04,
			editscenario01,
			editscenario02,
			editscenario03,
			editscenario04,
			reviewscenario01,
			//reviewscenario02,
			//reviewscenario03,
			newscenario,
			//metrics,
			simulate,
			results01,
			results02,
			results03,
			results04,
			saving,
			lastControl // always the last control
		}

		private int place;

		public WizTest()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			MrktSimControl.AppSettings = ConfigurationSettings.AppSettings;

			HelpLink.Links.Add(0,16,"http://www.decisionpower.com/support/index.php");

			System.Drawing.Bitmap Img = (Bitmap)this.BackgroundImage;
			this.TransparencyKey = Img.GetPixel(0,0);

			links = new ArrayList();

			cntrls = new ArrayList();

			for(int i = 0; i < (int)wiz.lastControl; i++)
			{
				cntrls.Add(0);
			}

			place = (int)wiz.setup;

			SetUpControl = new Setup();
			cntrls[(int)wiz.setup] = SetUpControl;

			SelectScenarioControl = new SelectScenario();
			cntrls[(int)wiz.selectscenario01] = SelectScenarioControl;

			CreateScenarioControl = new CreateScenario();
			cntrls[(int)wiz.selectscenario02] = CreateScenarioControl;

			NewScenarioControl = new NewScenario();
			cntrls[(int)wiz.selectscenario03] = NewScenarioControl;

			ScenarioViewControl = new ScenarioView();
			cntrls[(int)wiz.selectscenario04] = ScenarioViewControl;

			EditChoice01Control = new EditChoice01();
			cntrls[(int)wiz.editscenario01] = EditChoice01Control;

			EditScenarioControl = new EditScenario();
			cntrls[(int)wiz.editscenario02] = EditScenarioControl;

			EditChoice02Control = new EditChoice02();
			cntrls[(int)wiz.editscenario03] = EditChoice02Control;

			AddCompetitorControl = new AddCompetitor();
			AdvancedOptionsControl = new AdvancedOptions();
			cntrls[(int)wiz.editscenario04] = AdvancedOptionsControl;

			CheckScenarioControl = new CheckScenario();
			cntrls[(int)wiz.reviewscenario01] = CheckScenarioControl;
			
			EditChoice03Control = new EditChoice03();
			cntrls[(int)wiz.newscenario] = EditChoice03Control;

			//METRICS NOT CURRENTLY USED
			//SelectMetricsControl = new SelectMetrics();
			//cntrls[(int)wiz.metrics] = SelectMetricsControl;

			RunSimsControl = new RunSims();
			cntrls[(int)wiz.simulate] = RunSimsControl;
			
			ResultsViewingControl = new BrandManager.Forms.ResultsViewing();
			cntrls[(int)wiz.results02] = ResultsViewingControl;

			ResultsToFileControl = new BrandManager.Forms.ResultsToFile();
			cntrls[(int)wiz.results04] = ResultsToFileControl;

			ResultChoice01Control = new BrandManager.Forms.ResultChoice01();
			cntrls[(int)wiz.results01] = ResultChoice01Control;

			ResultChoice02Control = new BrandManager.Forms.ResultChoice02();
			cntrls[(int)wiz.results03] = ResultChoice02Control;

			SaveScenarioControl = new SaveScenario();
			cntrls[(int)wiz.saving] = SaveScenarioControl;
			
			
			


			SetUpLink = new LinkButton();
			SetUpLink.Name = "SETUP";
			SetUpLink.Text = "MODIFY SETTINGS";
			links.Add(SetUpLink);

			StartLink = new LinkButton();
			StartLink.Name = "HOME";
			StartLink.Text = "START";
			links.Add(StartLink);

			SelectScenarioLink = new LinkButton();
			SelectScenarioLink.Name = "SELECT SCENARIO";
			SelectScenarioLink.Text = "SELECT SCENARIO";
			links.Add(SelectScenarioLink);

//			ModScenarioLink = new LinkButton();
//			ModScenarioLink.Name = "EDIT SCENARIO";
//			ModScenarioLink.Text = "EDIT SCENARIO";
//			links.Add(ModScenarioLink);

			ReviewScenarioLink = new LinkButton();
			ReviewScenarioLink.Name = "REVIEW SCENARIO";
			ReviewScenarioLink.Text = "REVIEW SCENARIO";
			links.Add(ReviewScenarioLink);

			//METRICS NOT CURRENTLY USED
			//MetricsLink = new LinkButton(CheckScenarioControl);
			//MetricsLink.Name = "Metrics";
			//MetricsLink.Text = "Metrics";
			//links.Add(MetricsLink);

			RunLink = new LinkButton();
			RunLink.Name = "RUN SIM";
			RunLink.Text = "RUN SIM";
			links.Add(RunLink);

			ResultsLink = new LinkButton();
			ResultsLink.Name = "RESULTS";
			ResultsLink.Text = "VIEW RESULTS";
			links.Add(ResultsLink);


			int location = 88;
			int index = 0;

			foreach(LinkButton button in links)
			{
				Controls.Add(button);

				button.Location = new System.Drawing.Point(12, location);
				button.FlatStyle = FlatStyle.Flat;
				button.Size = new System.Drawing.Size(145, 23);
				button.TabIndex = index;
				button.BorderStyle = BorderStyle.None;
				button.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
				button.SelectState = SelectLabel;
				button.UnSelectState = UnSelectLabel;
				button.Deselect();
				//button.Click += new System.EventHandler(Click);
				location += 32;
				index += 1;
			}

			foreach(UserControl cntrl in cntrls)
			{
				//cntrl.Dock = DockStyle.Fill;
				cntrl.Height = 470;
				cntrl.Width = 610;
				cntrl.Top = 26;
				cntrl.Left = 166;
				cntrl.BackColor = Color.White;
				cntrl.ForeColor = Color.Black;
				((Wizard)cntrl).Done += new BrandManager.Forms.Finished(Finished);
				//cntrl.MouseDown += new System.Windows.Forms.MouseEventHandler(WizTest_MouseDown);
				//cntrl.MouseMove += new System.Windows.Forms.MouseEventHandler(WizTest_MouseMove);
				Controls.Add(cntrl);
			}

			//I really hate having to do this but...
			this.ResultsToFileControl.EditScenarioControl = this.EditScenarioControl;

			place = (int)wiz.setup;
			Back.Visible = false;
			SelectLink(this.SetUpLink);
			((UserControl)cntrls[place]).BringToFront();

			Next_Click(null,null);
		}

		private Database theDb;

		public Database Db
		{
			get
			{
				return theDb;
			}

			set
			{
				theDb = value;
			}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WizTest));
			this.UnSelectLabel = new System.Windows.Forms.Label();
			this.SelectLabel = new System.Windows.Forms.Label();
			this.Back = new System.Windows.Forms.Label();
			this.Next = new System.Windows.Forms.Label();
			this.minButton = new System.Windows.Forms.Label();
			this.killButton = new System.Windows.Forms.Label();
			this.HelpLink = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// UnSelectLabel
			// 
			this.UnSelectLabel.BackColor = System.Drawing.Color.White;
			this.UnSelectLabel.Enabled = false;
			this.UnSelectLabel.ForeColor = System.Drawing.Color.LightGray;
			this.UnSelectLabel.Image = ((System.Drawing.Image)(resources.GetObject("UnSelectLabel.Image")));
			this.UnSelectLabel.Location = new System.Drawing.Point(12, 232);
			this.UnSelectLabel.Name = "UnSelectLabel";
			this.UnSelectLabel.Size = new System.Drawing.Size(145, 23);
			this.UnSelectLabel.TabIndex = 2;
			this.UnSelectLabel.Text = "Unselected";
			this.UnSelectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.UnSelectLabel.Visible = false;
			// 
			// SelectLabel
			// 
			this.SelectLabel.BackColor = System.Drawing.SystemColors.Window;
			this.SelectLabel.Enabled = false;
			this.SelectLabel.ForeColor = System.Drawing.Color.Black;
			this.SelectLabel.Image = ((System.Drawing.Image)(resources.GetObject("SelectLabel.Image")));
			this.SelectLabel.Location = new System.Drawing.Point(12, 176);
			this.SelectLabel.Name = "SelectLabel";
			this.SelectLabel.Size = new System.Drawing.Size(145, 27);
			this.SelectLabel.TabIndex = 1;
			this.SelectLabel.Text = "Selected Label";
			this.SelectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.SelectLabel.Visible = false;
			// 
			// Back
			// 
			this.Back.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Back.Image = ((System.Drawing.Image)(resources.GetObject("Back.Image")));
			this.Back.Location = new System.Drawing.Point(600, 507);
			this.Back.Name = "Back";
			this.Back.Size = new System.Drawing.Size(75, 23);
			this.Back.TabIndex = 1;
			this.Back.Text = "<< Back";
			this.Back.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.Back.Click += new System.EventHandler(this.Back_Click);
			// 
			// Next
			// 
			this.Next.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Next.Image = ((System.Drawing.Image)(resources.GetObject("Next.Image")));
			this.Next.Location = new System.Drawing.Point(688, 507);
			this.Next.Name = "Next";
			this.Next.Size = new System.Drawing.Size(75, 23);
			this.Next.TabIndex = 0;
			this.Next.Text = "Next >>";
			this.Next.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.Next.Click += new System.EventHandler(this.Next_Click);
			// 
			// minButton
			// 
			this.minButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.minButton.Image = ((System.Drawing.Image)(resources.GetObject("minButton.Image")));
			this.minButton.Location = new System.Drawing.Point(742, 5);
			this.minButton.Name = "minButton";
			this.minButton.Size = new System.Drawing.Size(16, 16);
			this.minButton.TabIndex = 3;
			this.minButton.Click += new System.EventHandler(this.minButton_Click);
			// 
			// killButton
			// 
			this.killButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.killButton.Image = ((System.Drawing.Image)(resources.GetObject("killButton.Image")));
			this.killButton.Location = new System.Drawing.Point(762, 5);
			this.killButton.Name = "killButton";
			this.killButton.Size = new System.Drawing.Size(16, 16);
			this.killButton.TabIndex = 4;
			this.killButton.Click += new System.EventHandler(this.killButton_Click);
			// 
			// HelpLink
			// 
			this.HelpLink.Location = new System.Drawing.Point(32, 520);
			this.HelpLink.Name = "HelpLink";
			this.HelpLink.TabIndex = 5;
			this.HelpLink.TabStop = true;
			this.HelpLink.Text = "Help and Support";
			this.HelpLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.HelpLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HelpLink_LinkClicked);
			// 
			// WizTest
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(105)), ((System.Byte)(170)));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(793, 551);
			this.Controls.Add(this.HelpLink);
			this.Controls.Add(this.killButton);
			this.Controls.Add(this.minButton);
			this.Controls.Add(this.Back);
			this.Controls.Add(this.Next);
			this.Controls.Add(this.SelectLabel);
			this.Controls.Add(this.UnSelectLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(793, 551);
			this.MinimumSize = new System.Drawing.Size(793, 551);
			this.Name = "WizTest";
			this.Text = "BrandManager";
			this.TransparencyKey = System.Drawing.Color.Red;
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WizTest_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.WizTest_Closing);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WizTest_MouseMove);
			this.ResumeLayout(false);

		}
		#endregion

		private void SelectLink(LinkButton link)
		{
			foreach(LinkButton button in links)
			{
				button.Deselect();
				button.SendToBack();
			}

			link.Select();
			link.BringToFront();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new WizTest());
		}

		private void Next_Click(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			this.SuspendLayout();
			if(((Wizard)cntrls[place]).Next())
			{
				((Wizard)cntrls[place]).End();
			}
			else
			{
				this.Cursor = Cursors.Arrow;
				return;
			}

			Next.Text = "Next >>";
			
			switch(place)
			{
				case (int)wiz.setup:
					place = (int)wiz.selectscenario01;

					Db = SetUpControl.Db;
					SelectScenarioControl.Db = Db;
					CreateScenarioControl.Db = Db;
					ScenarioViewControl.Db = Db;
					EditScenarioControl.Db =Db;
					ResultsViewingControl.Db = Db;
					ResultsToFileControl.Db = Db;
					SaveScenarioControl.Db = Db;
					RunSimsControl.Db = Db;
					NewScenarioControl.Db = Db;
					CheckScenarioControl.Db = Db;
					AdvancedOptionsControl.Db = Db;
					//SelectMetricsControl.Db = Db;

					SelectLink(StartLink);

					Back.Enabled = true;
					Back.Text = "Settings";
					Back.Visible = true;
					break;
				case (int)wiz.selectscenario01:
					if(((SelectScenario)cntrls[place]).existingScenario)
					{
						SelectLink(SelectScenarioLink);
						place = (int)wiz.selectscenario04;
					}
					else
					{
						if(((SelectScenario)cntrls[place]).skiptosim)
						{
							SelectLink(this.RunLink);
							place = (int)wiz.simulate;
							Next.Text = "Done";
							Back.Visible = false;
						}
						else if (((SelectScenario)cntrls[place]).ViewResults)
						{
							place = (int)wiz.results01;
					
							SelectLink(this.ResultsLink);
						}
						else
						{
							SelectLink(SelectScenarioLink);
							place = (int)wiz.selectscenario02;
						}
					}
					Back.Text = "<<Back";
					break;
				case (int)wiz.selectscenario02:
					place = (int)wiz.selectscenario03;
					NewScenarioControl.BaselineScenario = CreateScenarioControl.SelectedScenario;
					break;
				case (int)wiz.selectscenario03:
					place = (int)wiz.editscenario04;
					Back.Visible = false;
					CheckScenarioControl.CurrentScenario = NewScenarioControl.CurrentScenario;
					EditScenarioControl.CurrentScenario = NewScenarioControl.CurrentScenario;
					AdvancedOptionsControl.CurrentScenario = NewScenarioControl.CurrentScenario;
					SelectLink(this.ReviewScenarioLink);
					break;
				case (int)wiz.selectscenario04:
					/*if(((EditChoice01)cntrls[(int)wiz.editscenario01]).Yes)
					{
						place = (int)wiz.editscenario02;
					}
					else
					{
						place = (int)wiz.editscenario01;
					}*/
					place = (int)wiz.reviewscenario01;
					CheckScenarioControl.CurrentScenario = ScenarioViewControl.SelectedScenario;
					EditScenarioControl.CurrentScenario = ScenarioViewControl.SelectedScenario;
					AdvancedOptionsControl.CurrentScenario = ScenarioViewControl.SelectedScenario;
					Back.Visible = false;
					Next.Text = "Done";
					SelectLink(this.ReviewScenarioLink);
					break;
				case (int)wiz.editscenario01:
					if(((EditChoice01)cntrls[place]).Yes)
					{
						place = (int)wiz.editscenario02;
					}
					else
					{
						if(((EditChoice02)cntrls[(int)wiz.editscenario03]).Yes)
						{
							place = (int)wiz.editscenario04;
						}
						else
						{
							place = (int)wiz.editscenario03;
						}
					}
					break;
				case (int)wiz.editscenario02:
					if(((EditChoice02)cntrls[(int)wiz.editscenario03]).Yes)
					{
						place = (int)wiz.editscenario04;
					}
					else
					{
						place = (int)wiz.editscenario03;
					}
					break;
				case (int)wiz.editscenario03:
					if(((EditChoice02)cntrls[(int)wiz.editscenario03]).Yes)
					{
						place = (int)wiz.editscenario04;
					}
					else
					{
						place = (int)wiz.reviewscenario01;
						Next.Text = "Done";
						SelectLink(this.ReviewScenarioLink);
					}
					break;
				case (int)wiz.editscenario04:
					place = (int)wiz.reviewscenario01;
					Back.Visible = true;
					Next.Text = "Done";
					SelectLink(this.ReviewScenarioLink);
					break;
				case (int)wiz.reviewscenario01:
					place = (int)wiz.selectscenario01;
					Back.Visible = true;
					Back.Text = "Settings";
					SelectLink(StartLink);
					break;
				case (int)wiz.newscenario:
					if(((EditChoice03)cntrls[place]).Yes)
					{
						place = (int)wiz.selectscenario01;
						
						SelectLink(StartLink);
					}
					else
					{
						place = (int)wiz.simulate;
						Next.Text = "Done";
						SelectLink(this.RunLink);
					}
					break;
				//METRICS NOT CURRENTLY USED
				//case (int)wiz.metrics:
				//	place = (int)wiz.simulate;
				//	SelectLink(this.RunLink);
				//	break;
				case (int)wiz.simulate:
					place = (int)wiz.selectscenario01;
					Back.Text = "Settings";
					Back.Visible = true;
					SelectLink(this.StartLink);
					break;
				case (int)wiz.results01:
					if(((ResultChoice01)cntrls[(int)wiz.results01]).TimeSeries)
					{
						place = (int)wiz.results02;
					}
					else
					{
						place = (int)wiz.results04;
					}
					Next.Text = "Done";
					break;
				case (int)wiz.results02:
					place = (int)wiz.selectscenario01;
					Back.Text = "Settings";
					SelectLink(this.StartLink);
					break;
				case (int)wiz.results03:
					place = (int)wiz.results04;
				break;
				case (int)wiz.results04:
					place = (int)wiz.selectscenario01;
					Back.Text = "Settings";
					SelectLink(this.StartLink);
					break;

				case (int)wiz.saving:
					place = (int)wiz.selectscenario01;
					Back.Text = "Settings";
					SelectLink(this.StartLink);
					break;

				default:
					place = (int)wiz.setup;
					break;
			}
			((UserControl)cntrls[place]).SuspendLayout();
				
			((UserControl)cntrls[place]).BringToFront();
			((Wizard)cntrls[place]).Start();

			((UserControl)cntrls[place]).ResumeLayout();
			this.ResumeLayout();
			this.Cursor = Cursors.Arrow;

		}

		private void Back_Click(object sender, System.EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			this.SuspendLayout();

			if(!((Wizard)cntrls[place]).Back())
			{
				this.Cursor = Cursors.Arrow;
				return;
			}

			Next.Text = "Next >>";

			switch(place)
			{
				case (int)wiz.selectscenario01:
					place = (int)wiz.setup;
					SelectLink(this.SetUpLink);
					//Back.Enabled = false;
					Back.Visible = false;
					break;
				case (int)wiz.selectscenario02:
				case (int)wiz.selectscenario04:
					place = (int)wiz.selectscenario01;
					Back.Text = "Settings";
					SelectLink(StartLink);
					break;
				case (int)wiz.selectscenario03:
					place = (int)wiz.selectscenario02;
					break;
				case (int)wiz.editscenario01:
				case (int)wiz.editscenario02:
					place = (int)wiz.selectscenario01;
					Back.Text = "Settings";
					SelectLink(this.StartLink);
					break;
				case (int)wiz.editscenario03:
				case (int)wiz.editscenario04:
					if(((EditChoice01)cntrls[(int)wiz.editscenario01]).Yes)
					{
						place = (int)wiz.editscenario02;
					}
					else
					{
						place = (int)wiz.editscenario01;
					}
					break;
				case (int)wiz.reviewscenario01:
					
					place = (int)wiz.editscenario04;
					Back.Visible = false;
					SelectLink(this.ReviewScenarioLink);
					break;
				case (int)wiz.newscenario:
					place = (int)wiz.reviewscenario01;
					break;
				//METRICS NOT CURRENTLY USED
				//case (int)wiz.metrics:
				//	if(((SelectScenario)cntrls[(int)wiz.selectscenario01]).skiptosim)
				//	{
				//		place = (int)wiz.selectscenario01;
				//		SelectLink(this.SelectScenarioLink);
				//	}
				//	else
				//	{
				//		place = (int)wiz.newscenario;
				//		SelectLink(this.ReviewScenarioLink);
				//	}
				//	break;
				case (int)wiz.simulate:
					if(((SelectScenario)cntrls[(int)wiz.selectscenario01]).skiptosim)
					{
						place = (int)wiz.selectscenario01;
						Back.Text = "Settings";
						SelectLink(this.StartLink);
					}
					else
					{
						place = (int)wiz.newscenario;
						SelectLink(this.ReviewScenarioLink);
					}
					break;
				case (int)wiz.results01:
					place = (int)wiz.selectscenario01;
					Back.Text = "Settings";
					SelectLink(this.StartLink);
					break;
				case (int)wiz.results02:
					place = (int)wiz.results01;
					break;
				case (int)wiz.results04:
					place = (int)wiz.results01;
					break;
				case (int)wiz.saving:
					if(((ResultChoice02)cntrls[(int)wiz.results03]).Yes)
					{
						place = (int)wiz.results04;
					}
					else
					{
						place = (int)wiz.results03;
					}
					Next.Enabled = true;
					break;
				default:
					place = (int)wiz.setup;
					break;
			}
				
			((UserControl)cntrls[place]).BringToFront();

			this.ResumeLayout();
			this.Cursor = Cursors.Arrow;

		}

		public void Finished()
		{
			this.Next_Click(null,null);
		}

		private void WizTest_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (theDb != null && Setup.Settings.Project != Database.AllID && Setup.Settings.Model != Database.AllID)
			{
				this.ScenarioViewControl.Flush();

				theDb.Update();

				theDb.ReadModelForBrandManager();

				string anyRows = "user_name = '" + Setup.Settings.User + "'" + " AND queued <> 1";

				DataRow[] scenarioRows = theDb.Data.scenario.Select(anyRows,"",DataViewRowState.CurrentRows);

				if (scenarioRows.Length != 0)
				{
					using (SaveScenariosDlg saveDlg = new SaveScenariosDlg())
					{
					
						saveDlg.Db = this.theDb;
						saveDlg.ShowDialog(this);
				
					}
				}


				string query = "user_name = '" + Setup.Settings.User + "'" + " AND saved = 0 AND queued <> 1";

				DataRow[] rows = theDb.Data.scenario.Select(query,"",DataViewRowState.CurrentRows);

				bool done = rows.Length == 0;

				this.Cursor = System.Windows.Forms.Cursors.Arrow;

				/*if (!done)
				{

					// ask user if they want to save or not or cancel
					DialogResult rslt = MessageBox.Show("Some of your scenarios are marked for deletion do you want to save them instead?","Save Scenarios",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Warning);
					if(rslt == DialogResult.Cancel)
					{
						e.Cancel = true;
						return;
					}
					if(rslt == DialogResult.No)
					{
						done = true;
					}
				}*/
		
				if (!done)
				{
					using (SaveScenariosDlg saveDlg = new SaveScenariosDlg())
					{
						saveDlg.Db = this.theDb;
		
						while (!done)
						{
							// there are scenarios to delete
							// keep asking user until they are satisfied

							

							ListOKCancel confirmlg = new ListOKCancel();

							confirmlg.Text = "Delete Scenarios?";
							confirmlg.Description = "The scenarios listed above will be deleted.";

							rows = theDb.Data.scenario.Select(query,"",DataViewRowState.CurrentRows);

							foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in rows)
							{
								confirmlg.AddItem(scenario.name);
							}

							DialogResult rslt = confirmlg.ShowDialog(this);

							if(rslt == DialogResult.OK)
							{
								done = true;
							}
							else
							{
								saveDlg.ShowDialog(this);
							}
						}
					}
				}

				foreach(MrktSimDb.MrktSimDBSchema.scenarioRow scenario in rows)
				{
						
					Db.killScenarioAndMarketPlans(scenario);
				}

				theDb.Update();

				theDb.Close();
			}

			Setup.Settings.Save();
		}

		public Point mouse_offset;

		private void WizTest_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			mouse_offset = new Point(-e.X, -e.Y);
		}

		private void WizTest_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) 
			{
				Point mousePos = Control.MousePosition;
				mousePos.Offset(mouse_offset.X, mouse_offset.Y);
				Location = mousePos;
			}
		}

		private void minButton_Click(object sender, System.EventArgs e)
		{
			this.WindowState=FormWindowState.Minimized;
		}

		private void killButton_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.Close();
		}

		private void HelpLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
		}



	}

	



	
}
