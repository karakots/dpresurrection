using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
/*using System.Configuration;

using BrandManager.Forms;
using BrandManager.Dialogues;

using MrktSimDb;

// using Common.Utilities;
using MrktSimDb.Metrics;

using MarketSimUtilities;

namespace BrandManager
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class BrandMan : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.LinkLabel setUpLink;
		private System.Windows.Forms.LinkLabel createScenarioLink;
		private System.Windows.Forms.LinkLabel metricLink;
		private System.Windows.Forms.LinkLabel runSimsLink;
		private System.Windows.Forms.LinkLabel resultsLink;
		private System.Windows.Forms.LinkLabel modScenarioLink;
		private System.Windows.Forms.LinkLabel baseScenarioLink;
		private System.Windows.Forms.LinkLabel newScenarioLink;
		private System.Windows.Forms.LinkLabel checkScenarioLink;
		private System.Windows.Forms.LinkLabel checkScenarioLink2;
		private System.Windows.Forms.LinkLabel addProductLink;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BrandMan()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// read in config options
			MrktSimControl.AppSettings = ConfigurationSettings.AppSettings;
			
			// initialize metric proper names
			Metric.TokenConvert +=new MrktSimDb.Metrics.Metric.Description(MrktSimControl.MrktSimMessage);

			// create controls
			this.SuspendLayout();

			setup = new Setup();
			this.Controls.Add(setup);
			this.setup.Dock = DockStyle.Fill;

			this.setUpLink.Links.Add(0,setUpLink.Text.Length, setup);

			setup.StateChanged +=new BrandManager.Forms.StateChange(SetUpChanged);
			
			scenarioView = new ScenarioView();
			this.Controls.Add(scenarioView);
			this.scenarioView.Dock = DockStyle.Fill;
			createScenarioLink.Links.Add(0,createScenarioLink.Text.Length, scenarioView);
			scenarioView.StateChanged +=new StateChange(setState);

			createScenario = new CreateScenario();
			this.Controls.Add(createScenario);
			this.createScenario.Dock = DockStyle.Fill;
			baseScenarioLink.Links.Add(0,baseScenarioLink.Text.Length, createScenario);
			createScenario.StateChanged +=new StateChange(setState);

			newScenario = new NewScenario();
			this.Controls.Add(newScenario);
			this.newScenario.Dock = DockStyle.Fill;
			newScenarioLink.Links.Add(0,newScenarioLink.Text.Length, newScenario);
			newScenario.StateChanged +=new StateChange(setState);
			newScenario.Done +=new TaskDone(done);

			checkScenario = new CheckScenario();
			this.Controls.Add(checkScenario);
			this.checkScenario.Dock = DockStyle.Fill;
			checkScenarioLink.Links.Add(0,checkScenarioLink.Text.Length, checkScenario);
			checkScenario.StateChanged +=new StateChange(setState);


			checkScenario2 = new CheckScenario();
			this.Controls.Add(checkScenario2);
			this.checkScenario2.Dock = DockStyle.Fill;
			checkScenarioLink2.Links.Add(0,checkScenarioLink2.Text.Length, checkScenario2);
			checkScenario2.StateChanged +=new StateChange(setState);

			editScenario = new EditScenario();
			this.Controls.Add(editScenario);
			this.editScenario.Dock = DockStyle.Fill;
			modScenarioLink.Links.Add(0,modScenarioLink.Text.Length, editScenario);
			editScenario.StateChanged +=new StateChange(setState);

			addCompetitor = new AddCompetitor();
			this.Controls.Add(addCompetitor);
			this.addCompetitor.Dock = DockStyle.Fill;
			addProductLink.Links.Add(0,addProductLink.Text.Length, addCompetitor);
			addCompetitor.StateChanged +=new StateChange(setState);

			selectMetrics = new SelectMetrics();
			this.Controls.Add(selectMetrics);
			this.selectMetrics.Dock = DockStyle.Fill;
			this.metricLink.Links.Add(0,metricLink.Text.Length, selectMetrics);
			selectMetrics.StateChanged +=new StateChange(setState);


			runSims =  new RunSims();
			this.Controls.Add(runSims);
			this.runSims.Dock = DockStyle.Fill;
			this.runSimsLink.Links.Add(0,runSimsLink.Text.Length, runSims);
			runSims.StateChanged +=new StateChange(setState);

			results  = new Forms.Results();
			this.Controls.Add(results);
			this.results.Dock = DockStyle.Fill;
			this.resultsLink.Links.Add(0,resultsLink.Text.Length, results);
			results.StateChanged +=new StateChange(setState);

			this.ResumeLayout(false);
			
			setInitialSelection();

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BrandMan));
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkScenarioLink2 = new System.Windows.Forms.LinkLabel();
			this.checkScenarioLink = new System.Windows.Forms.LinkLabel();
			this.newScenarioLink = new System.Windows.Forms.LinkLabel();
			this.modScenarioLink = new System.Windows.Forms.LinkLabel();
			this.baseScenarioLink = new System.Windows.Forms.LinkLabel();
			this.resultsLink = new System.Windows.Forms.LinkLabel();
			this.runSimsLink = new System.Windows.Forms.LinkLabel();
			this.metricLink = new System.Windows.Forms.LinkLabel();
			this.createScenarioLink = new System.Windows.Forms.LinkLabel();
			this.setUpLink = new System.Windows.Forms.LinkLabel();
			this.addProductLink = new System.Windows.Forms.LinkLabel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.addProductLink);
			this.panel1.Controls.Add(this.checkScenarioLink2);
			this.panel1.Controls.Add(this.checkScenarioLink);
			this.panel1.Controls.Add(this.newScenarioLink);
			this.panel1.Controls.Add(this.modScenarioLink);
			this.panel1.Controls.Add(this.baseScenarioLink);
			this.panel1.Controls.Add(this.resultsLink);
			this.panel1.Controls.Add(this.runSimsLink);
			this.panel1.Controls.Add(this.metricLink);
			this.panel1.Controls.Add(this.createScenarioLink);
			this.panel1.Controls.Add(this.setUpLink);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(184, 366);
			this.panel1.TabIndex = 1;
			// 
			// checkScenarioLink2
			// 
			this.checkScenarioLink2.DisabledLinkColor = System.Drawing.Color.Green;
			this.checkScenarioLink2.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.checkScenarioLink2.Location = new System.Drawing.Point(48, 160);
			this.checkScenarioLink2.Name = "checkScenarioLink2";
			this.checkScenarioLink2.Size = new System.Drawing.Size(120, 23);
			this.checkScenarioLink2.TabIndex = 9;
			this.checkScenarioLink2.TabStop = true;
			this.checkScenarioLink2.Text = "Review Scenario";
			this.checkScenarioLink2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
			// 
			// checkScenarioLink
			// 
			this.checkScenarioLink.DisabledLinkColor = System.Drawing.Color.Green;
			this.checkScenarioLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.checkScenarioLink.Location = new System.Drawing.Point(48, 104);
			this.checkScenarioLink.Name = "checkScenarioLink";
			this.checkScenarioLink.Size = new System.Drawing.Size(120, 23);
			this.checkScenarioLink.TabIndex = 8;
			this.checkScenarioLink.TabStop = true;
			this.checkScenarioLink.Text = "Review Scenario";
			this.checkScenarioLink.Visible = false;
			// 
			// newScenarioLink
			// 
			this.newScenarioLink.DisabledLinkColor = System.Drawing.Color.Green;
			this.newScenarioLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.newScenarioLink.Location = new System.Drawing.Point(48, 88);
			this.newScenarioLink.Name = "newScenarioLink";
			this.newScenarioLink.Size = new System.Drawing.Size(120, 23);
			this.newScenarioLink.TabIndex = 7;
			this.newScenarioLink.TabStop = true;
			this.newScenarioLink.Text = "New Scenario";
			this.newScenarioLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
			// 
			// modScenarioLink
			// 
			this.modScenarioLink.DisabledLinkColor = System.Drawing.Color.Green;
			this.modScenarioLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.modScenarioLink.Location = new System.Drawing.Point(48, 184);
			this.modScenarioLink.Name = "modScenarioLink";
			this.modScenarioLink.Size = new System.Drawing.Size(112, 23);
			this.modScenarioLink.TabIndex = 6;
			this.modScenarioLink.TabStop = true;
			this.modScenarioLink.Text = "Modify Scenario";
			this.modScenarioLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
			// 
			// baseScenarioLink
			// 
			this.baseScenarioLink.DisabledLinkColor = System.Drawing.Color.Green;
			this.baseScenarioLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.baseScenarioLink.Location = new System.Drawing.Point(24, 64);
			this.baseScenarioLink.Name = "baseScenarioLink";
			this.baseScenarioLink.Size = new System.Drawing.Size(104, 23);
			this.baseScenarioLink.TabIndex = 5;
			this.baseScenarioLink.TabStop = true;
			this.baseScenarioLink.Text = "Strategic Scenarios";
			this.baseScenarioLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
			// 
			// resultsLink
			// 
			this.resultsLink.DisabledLinkColor = System.Drawing.Color.Green;
			this.resultsLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.resultsLink.Location = new System.Drawing.Point(32, 296);
			this.resultsLink.Name = "resultsLink";
			this.resultsLink.Size = new System.Drawing.Size(80, 16);
			this.resultsLink.TabIndex = 4;
			this.resultsLink.TabStop = true;
			this.resultsLink.Text = "View Results";
			this.resultsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
			// 
			// runSimsLink
			// 
			this.runSimsLink.DisabledLinkColor = System.Drawing.Color.Green;
			this.runSimsLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.runSimsLink.Location = new System.Drawing.Point(32, 272);
			this.runSimsLink.Name = "runSimsLink";
			this.runSimsLink.Size = new System.Drawing.Size(96, 23);
			this.runSimsLink.TabIndex = 3;
			this.runSimsLink.TabStop = true;
			this.runSimsLink.Text = "Run Simulations";
			this.runSimsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
			// 
			// metricLink
			// 
			this.metricLink.DisabledLinkColor = System.Drawing.Color.Green;
			this.metricLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.metricLink.Location = new System.Drawing.Point(32, 232);
			this.metricLink.Name = "metricLink";
			this.metricLink.Size = new System.Drawing.Size(48, 23);
			this.metricLink.TabIndex = 2;
			this.metricLink.TabStop = true;
			this.metricLink.Text = "Metrics";
			this.metricLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
			// 
			// createScenarioLink
			// 
			this.createScenarioLink.DisabledLinkColor = System.Drawing.Color.Green;
			this.createScenarioLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.createScenarioLink.Location = new System.Drawing.Point(24, 136);
			this.createScenarioLink.Name = "createScenarioLink";
			this.createScenarioLink.Size = new System.Drawing.Size(80, 23);
			this.createScenarioLink.TabIndex = 1;
			this.createScenarioLink.TabStop = true;
			this.createScenarioLink.Text = "My Scenarios";
			this.createScenarioLink.VisitedLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));
			this.createScenarioLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
			// 
			// setUpLink
			// 
			this.setUpLink.DisabledLinkColor = System.Drawing.Color.Green;
			this.setUpLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.setUpLink.Location = new System.Drawing.Point(24, 32);
			this.setUpLink.Name = "setUpLink";
			this.setUpLink.Size = new System.Drawing.Size(48, 16);
			this.setUpLink.TabIndex = 0;
			this.setUpLink.TabStop = true;
			this.setUpLink.Text = "Set Up";
			this.setUpLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
			// 
			// addProductLink
			// 
			this.addProductLink.DisabledLinkColor = System.Drawing.Color.Green;
			this.addProductLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.addProductLink.Location = new System.Drawing.Point(48, 208);
			this.addProductLink.Name = "addProductLink";
			this.addProductLink.Size = new System.Drawing.Size(128, 23);
			this.addProductLink.TabIndex = 10;
			this.addProductLink.TabStop = true;
			this.addProductLink.Text = "Add competing  product";
			this.addProductLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
			// 
			// BrandMan
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(712, 366);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(720, 400);
			this.Name = "BrandMan";
			this.Text = "MarketSim Brand Manager";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.BrandMan_Closing);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new BrandMan());
		}

		#region wizard controls
		Setup setup;
		RunSims runSims;
		ScenarioView scenarioView;
		SelectMetrics selectMetrics;
		Forms.Results results;
		CreateScenario createScenario;
		EditScenario editScenario;
		NewScenario newScenario;
		CheckScenario checkScenario;
		CheckScenario checkScenario2;
		AddCompetitor addCompetitor;
		#endregion

		#region Link Label Code
		private LinkLabel curLink;

		private void selectCurLink()
		{
			((UserControl) curLink.Links[0].LinkData).BringToFront();
			curLink.Links[0].Enabled = false;
			curLink.BorderStyle = BorderStyle.FixedSingle;
		}

		private void unSelectCurLink()
		{
			curLink.Links[0].Enabled = true;
			curLink.BorderStyle = BorderStyle.None;
		}

		private void setInitialSelection()
		{
			curLink = setUpLink;

			if (setup.GotoNext())
			{
				curLink = baseScenarioLink;
			}

			selectCurLink();

			SetUpChanged();
		}

		private void SetUpChanged()
		{
			if (!setup.GotoNext())
			{
				createScenarioLink.Enabled = false;
				baseScenarioLink.Enabled = false;
				newScenarioLink.Enabled = false;
				checkScenarioLink.Enabled = false;
				checkScenarioLink2.Enabled = false;
				modScenarioLink.Enabled = false;
				metricLink.Enabled = false;
				runSimsLink.Enabled = false;
				resultsLink.Enabled = false;
				theDb = null;
				return;
			}

			theDb = setup.Db;

			this.scenarioView.Db = theDb;
			this.createScenario.Db = theDb;
			this.editScenario.Db = theDb;
			this.runSims.Db = theDb;
			this.newScenario.Db = theDb;
			this.checkScenario.Db = theDb;
			this.checkScenario2.Db = theDb;
			this.results.Db = theDb;
			
			setState();
		}

		private void setState()
		{
			// always enabled
			resultsLink.Enabled = true;

			// runnning a sim - cant do anything else for the moment
			if (!runSims.GotoNext())
			{
				
				runSimsLink.Enabled = true;

				// running sim turn everything else off
				setUpLink.Enabled = false;
				createScenarioLink.Enabled = false;
				baseScenarioLink.Enabled = false;
				modScenarioLink.Enabled = false;
				metricLink.Enabled = false;
				addProductLink.Enabled = false;
				
				return;
			}
			
			

			// if we are passed setup then we can create a new sccenario
			baseScenarioLink.Enabled = true;
			newScenarioLink.Enabled = false;
			checkScenarioLink.Enabled = false;

			if (curLink == baseScenarioLink)
			{
				if (createScenario.SelectedScenario != null)
				{
					newScenarioLink.Enabled = true;
				}
			}

			if (curLink == newScenarioLink)
			{	
				newScenario.BaselineScenario = createScenario.SelectedScenario;
				newScenarioLink.Enabled = true;
			}

			if (curLink == checkScenarioLink)
			{	
				checkScenario.CurrentScenario = newScenario.CurrentScenario;
				newScenarioLink.Enabled = false;
				checkScenarioLink.Enabled = true;
			}

			// this gets turned on when we enter My Scenarios
			modScenarioLink.Enabled = false;
			checkScenarioLink2.Enabled = false;
			addProductLink.Enabled = false;

			if (!scenarioView.GotoNext())
			{
				// no scenarios created
				// stop
				createScenarioLink.Enabled = false;
				metricLink.Enabled = false;
				runSimsLink.Enabled = false;
				return;
			}

			createScenarioLink.Enabled = true;

			if (curLink == createScenarioLink)
			{
				if (scenarioView.SelectedScenario != null)
				{
					modScenarioLink.Enabled = true;
					checkScenarioLink2.Enabled = true;
					addProductLink.Enabled = true;
				}
			}


			if (curLink == modScenarioLink)
			{
				checkScenarioLink2.Enabled = true;
				modScenarioLink.Enabled = true;
				addProductLink.Enabled = true;
				// get current sccenario from scenarioView
				editScenario.CurrentScenario = scenarioView.SelectedScenario;
			}

			if (curLink == checkScenarioLink2)
			{
				checkScenarioLink2.Enabled = true;
				modScenarioLink.Enabled = true;
				addProductLink.Enabled = true;
				// get current sccenario from scenarioView
				checkScenario2.CurrentScenario = scenarioView.SelectedScenario;
			}

			if (curLink == addProductLink)
			{
				checkScenarioLink2.Enabled = true;
				modScenarioLink.Enabled = true;
				addProductLink.Enabled = true;
				// get current sccenario from scenarioView
				checkScenario2.CurrentScenario = scenarioView.SelectedScenario;
			}


			if (!this.scenarioView.GotoNext())
			{
				metricLink.Enabled = false;
				runSimsLink.Enabled = false;
				return;
			}


			metricLink.Enabled = true;
			if(!selectMetrics.GotoNext())
			{
				runSimsLink.Enabled = false;
				return;
			}

			runSimsLink.Enabled = true;

			if (curLink == runSimsLink)
			{
				runSimsLink.Enabled = true;

				if (!runSims.GotoNext())
				{
					// running sim turn everything off
					setUpLink.Enabled = false;
					createScenarioLink.Enabled = false;
					baseScenarioLink.Enabled = false;
					modScenarioLink.Enabled = false;
					metricLink.Enabled = false;
				}
				else
				{
					setUpLink.Enabled = true;
					createScenarioLink.Enabled = true;
					baseScenarioLink.Enabled = true;
					metricLink.Enabled = true;
				}

				runSims.UpdateScenarioList();
			}
		}

		private void done()
		{
			unSelectCurLink();

			if (curLink == this.newScenarioLink)
			{
				curLink = this.checkScenarioLink2;
			}

			selectCurLink();
			
			setState();
		}

		private void LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			unSelectCurLink();
			
			curLink = (LinkLabel) sender;

			selectCurLink();
			
			setState();
		}


		#endregion

		#region UI
		private void BrandMan_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{

			if (!runSims.GotoNext())
			{
				MessageBox.Show("You must stop the running simulation before exiting");
				e.Cancel = true;
				theDb.Close();
				return;
			}

			DialogResult rslt = MessageBox.Show("Are you absolutely sure?", "Exit Brand Manager", MessageBoxButtons.YesNo);

			if (rslt == DialogResult.No)
			{
				e.Cancel = true;
				return;
			}

			if (theDb != null && theDb.HasChanges())
			{
				DialogResult saveRslt = MessageBox.Show("Do you wish to save the your work?", "Exiting Brand Manager", MessageBoxButtons.YesNo);

				if (saveRslt == DialogResult.Yes)
				{
					theDb.Update();
				}
			}

			if (theDb != null)
			{
				theDb.Close();
			}

			Setup.Settings.Save();
		}
		#endregion

		#region Private Data and Methods

		Database theDb = null;

		#endregion


	}
}*/
