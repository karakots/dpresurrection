/*using System;

using System.Data;

using MrktSimDb;



namespace ExcelInterface

{

	/// <summary>

	/// Summary description for PasteData.

	/// </summary>

	public class PastePlanData : PlanReader

	{

		public PastePlanData(ModelDb db, string inBaseName) : base(db, inBaseName)

		{

		}



		public string Paste(string text, PlanType planType)

		{

			char[] newLine = {'\r', '\n'};

			char[] tab = {'\t'};



			string[] lines = text.Split(newLine);



			// check for product as first item



			if (lines.Length == 0)

				return "No Data";



			return CreatePlanData(lines, planType);

		}

	}

}*/

