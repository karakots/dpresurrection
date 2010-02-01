using System;

using MrktSimDb;
using ExcelInterface;
using ErrorInterface;
using BrandManager.Forms;

namespace BrandManager.Utilities
{
	/// <summary>
	/// Summary description for ScenarioUtilities.
	/// </summary>
	public class ScenarioUtilities
	{
		public ScenarioUtilities()
		{
		}

		static public MrktSimDb.MrktSimDBSchema.scenarioRow CopyScenario(Database Db, MrktSimDb.MrktSimDBSchema.scenarioRow baseline, string fileName)
		{
			if (baseline == null)
				return null;

			// strip off extension
			string planName = fileName.Substring(0, fileName.Length - 4);

			int index = planName.LastIndexOf(@"\");

			planName = planName.Substring(index + 1, planName.Length - index - 1);

			planName = Setup.Settings.User + " " + planName; 

			PlanReader planReader = new PlanReader(Db, planName);
			planReader.OverWrite = -1;

			PlanType[] types = new PlanType[] {   PlanType.Price,
												  PlanType.Display,
												  PlanType.Distribution,
												  PlanType.Coupons,
												  PlanType.Market_Utility,
												  PlanType.Media };

			ErrorList errors = new ErrorList();
			foreach(PlanType type in types)
			{
				errors.addErrors(planReader.CreatePlan(fileName, type));
			}

			errors.Display();

			// Create Scenario from current scenario
			// construct name from doc name
			MrktSimDb.MrktSimDBSchema.scenarioRow copy = Db.CopyScenario(baseline, planName);

			copy.descr = "Strategic Scenario: " + baseline.name;

			planReader.createTopLevelMarketPlan(copy);

			copy.saved = false;
			copy.type = (byte)ScenarioType.Standard;

			return copy;
		}

		static public MrktSimDb.MrktSimDBSchema.scenarioRow CopyScenario(Database Db, MrktSimDb.MrktSimDBSchema.scenarioRow baseline)
		{
			if (baseline == null)
				return null;

			MrktSimDb.MrktSimDBSchema.scenarioRow copy = Db.CopyScenario(baseline, baseline.name);

			copy.descr = "Strategic Scenario: " + baseline.name;

			copy.saved = false;
			copy.type = (byte)ScenarioType.Standard;

			return copy;
		}

	}
}
