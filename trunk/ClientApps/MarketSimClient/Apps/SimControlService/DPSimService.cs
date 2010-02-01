using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;

namespace SimControlService
{
	public class DPSimService : System.ServiceProcess.ServiceBase
	{
		Process simControllerProcess;

		private System.Diagnostics.EventLog eventLog;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DPSimService()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			if (!System.Diagnostics.EventLog.SourceExists("SimController")) 
			{         
				System.Diagnostics.EventLog.CreateEventSource(
					"SimController", "Simulation Control");
			}
			eventLog.Source = "SimController";
			eventLog.Log = "Simulation Control";

			simControllerProcess = new Process();
			simControllerProcess.StartInfo.FileName = @"SimControl.exe";
			simControllerProcess.StartInfo.WorkingDirectory = @"C:\Program Files\DecisionPower\SimController";
			simControllerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			simControllerProcess.StartInfo.Arguments = "-start";
		}

		// The main entry point for the process
		static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
	
			// More than one user Service may run within the same process. To add
			// another service to this process, change the following line to
			// create a second service object. For example,
			//
			//   ServicesToRun = new System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
			//
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new DPSimService() };

			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.eventLog = new System.Diagnostics.EventLog();
			((System.ComponentModel.ISupportInitialize)(this.eventLog)).BeginInit();
			// 
			// DPSimService
			// 
			this.ServiceName = "SimService";
			((System.ComponentModel.ISupportInitialize)(this.eventLog)).EndInit();

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

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			try
			{
				simControllerProcess.Start();
				eventLog.WriteEntry("Starting Simulation Controller");
			}
			catch(Exception e)
			{
				eventLog.WriteEntry("Error Starting Simulation Controller: " + e.Message, EventLogEntryType.Error);
			}
		}
 
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			try
			{
				simControllerProcess.Kill();
				eventLog.WriteEntry("Stopping Simulation Controller");

				// no kill any simulations running
				Process[] procArray = System.Diagnostics.Process.GetProcesses();

				foreach(Process proc in procArray)
				{
					if (proc.ProcessName == "System" || proc.ProcessName == "Idle")
						continue;

					if (proc.HasExited)
						continue;

					if (proc.MainModule != null && proc.MainModule.FileName == @"C:\Program Files\DecisionPower\MarketSimEngine\MSServer.exe")
					{
						try
						{
							proc.Kill();
							eventLog.WriteEntry("Stopped process: " + proc.MainModule.FileName);
						}
						catch(Exception killError)
						{
							eventLog.WriteEntry("Error Stopping Simulation: " + killError.Message, EventLogEntryType.Error);
						}
					}
				}
			}
			catch(Exception e)
			{
				eventLog.WriteEntry("Error Stopping Simulation Controller: " + e.Message, EventLogEntryType.Error);
			}
		}
	}
}
