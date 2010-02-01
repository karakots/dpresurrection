using System;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using MrktSimDb;

using ErrorInterface;

namespace ExcelInterface
{
	#region old_code
	/// <summary>
	/// 
	/// </summary>
	public abstract class OldPlanReader
	{
		public static int NumFields(ModelDb.PlanType type)
		{
			switch (type)
			{
				case ModelDb.PlanType.Price:
					return 6;

				case ModelDb.PlanType.Distribution:
					return 7;

				case ModelDb.PlanType.Display:
					return 6;

				case ModelDb.PlanType.Media:
					return 7;

				case ModelDb.PlanType.Coupons :
					return 8;
			}

			return 0;
		}

		public OldPlanReader(ModelDb db, string inName)
		{
			theDb = db;
			baseName = inName;

			prodToPlan = new Hashtable();
		}
		
		System.Collections.Hashtable prodToPlan;

		private ModelDb theDb = null;
		private string baseName = null;

		protected string CreatePlanData(DataTable table, ModelDb.PlanType planType)
		{
			string error = "";
			string newError = null;
			int rowCount = 0;
			int numErrors = 0;

			foreach(DataRow row in table.Rows)
			{
				string[] values = new string[row.ItemArray.Length];

				for(int i = 0; i < row.ItemArray.Length; i++)
				{
					try
					{
						values[i] = row.ItemArray[i].ToString();
					}
					catch(Exception)
					{
						newError = "Null or unrecognized value encountered on row " + i;
						break;
					}
				}

				if(newError != null)
				{
					error += newError;
					break;
				}
				
				newError = CreatePlanDataRow(values, planType);
				
				if(newError != null)
				{
					if(numErrors < 5)
					{
						error += newError + " on row " + rowCount + "\n";
						numErrors++;
					}
					else if( numErrors == 5)
					{
						error += "No longer recording errors on this table";
						numErrors++;
					}

					newError = null;
				}

				rowCount++;
			}

			return error;
		}


	

		protected string CreatePlanData(string[] lines, ModelDb.PlanType planType)
		{
			string error = null;

			char[] tab = {'\t'};

			foreach(string line in lines)
			{
				error = CreatePlanData(line, planType);

				if (error != null)
					break;
			}

			return error;
		}



		protected string CreatePlanData(string line, ModelDb.PlanType planType)
		{
			if (line.Length == 0)
				return null;

			char[] tab = {'\t'};

			
			string[] valStrings = line.Split(tab);

			return CreatePlanDataRow(valStrings, planType);
		}

		protected string CreatePlanDataRow(string[] valStrings, ModelDb.PlanType planType)
		{
			if (!ModelDb.LegalName(valStrings[0]))
			{
				return "Names should only have alpha numeric characters in them";
			}

			MrktSimDBSchema.market_planRow curPlan = null;

			if (!prodToPlan.ContainsKey(valStrings[0]))
			{
				// check for product
				string query = "product_name = '" + valStrings[0] + "'";
				DataRow[] rows = theDb.Data.product.Select(query);

				if (rows.Length == 0)
				{
					return "Could not find product named " + valStrings[0];
				}

				// create a plan for this product
				curPlan = theDb.CreateMarketPlan(baseName, planType);
				curPlan.name = valStrings[0] + " - " + baseName;
				curPlan.product_id = (int) rows[0]["product_id"];

				prodToPlan.Add(valStrings[0], curPlan);
			}

			if (curPlan == null)
				curPlan = (MrktSimDBSchema.market_planRow) prodToPlan[valStrings[0]];

			string[] restOfValues = new string[valStrings.Length - 1];

			for(int ii = 0; ii < restOfValues.Length; ++ii)
				restOfValues[ii] = valStrings[ii + 1];

			string error = null;

			switch (planType)
			{
				case ModelDb.PlanType.Price :
					error = createPriceRow(curPlan, restOfValues);
					break;

				case ModelDb.PlanType.Distribution :
					error = createDistributionRow(curPlan, restOfValues);
					break;

				case ModelDb.PlanType.Display :
					error = createDisplayRow(curPlan, restOfValues);
					break;
				case ModelDb.PlanType.Media :
				case ModelDb.PlanType.Coupons :
					error = createMassMediaRow(curPlan, restOfValues);
					break;
				case ModelDb.PlanType.Market_Utility :
					error = createMarketUtilityRow(curPlan, restOfValues);
					break;
			}

			return error;
		}


		private string createPriceRow(MrktSimDBSchema.market_planRow thePlan, string[] items)
		{
            string error = null;

			if (items.Length < NumFields((ModelDb.PlanType) thePlan.type))
				return "Expecting six columns of data";

			// first find channel
			string query = "channel_name = '" + items[0] + "'";
			DataRow[] rows = theDb.Data.channel.Select(query);

			if (rows.Length == 0)
			{
				return "could not find channel named " + items[0];
			}
			
			int channel_id = (int) rows[0]["channel_id"];

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.product_channelRow price = theDb.CreateProductChannel(product_id, channel_id);

			theDb.MarketPlanID = curPlanId;

			try
			{
				price.price = Double.Parse(items[1]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			// there is no try - only do
            string priceTypeName = items[2];

            priceTypeName = priceTypeName.Trim();

            string priceQuery = "name = '" + priceTypeName + "'";
            DataRow[] priceRows = theDb.Data.price_type.Select( priceQuery, "", DataViewRowState.CurrentRows );
            if( priceRows.Length > 0 )
            {
                price.price_type = ((MrktSimDBSchema.price_typeRow)rows[0]).id;
            }
            else
            {
                // make this unpromoted warn -- once
                if( error == null )
                {
                    error = "Could not find price type " + priceTypeName;
                }

                price.price_type = Database.AllID;
            }

			try
			{
				price.percent_SKU_in_dist = Double.Parse(items[3]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			
			try
			{
				price.start_date = DateTime.Parse(items[4]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				price.end_date = DateTime.Parse(items[5]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			return error;
		}
		

		private string createDisplayRow(MrktSimDBSchema.market_planRow thePlan, string[] items)
		{
			if (items.Length != NumFields((ModelDb.PlanType)  thePlan.type))
				return "Expecting six columns of data";

			// first find channel
			string query = "channel_name = '" + items[0] + "'";
			DataRow[] rows = theDb.Data.channel.Select(query);

			if (rows.Length == 0)
			{
				return "could not find channel named " + items[0];
			}
			
		
			int channel_id = (int) rows[0]["channel_id"];

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.displayRow display = theDb.CreateDisplay(product_id, channel_id);

			theDb.MarketPlanID = curPlanId;

			try
			{
				display.attr_value_F = Double.Parse(items[1]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				display.message_awareness_probability = Double.Parse(items[2]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			
			try
			{
				display.message_persuation_probability = Double.Parse(items[3]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				display.start_date = DateTime.Parse(items[4]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				display.end_date = DateTime.Parse(items[5]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			return null;
		}

		
		private string createDistributionRow(MrktSimDBSchema.market_planRow thePlan, string[] items)
		{
			if (items.Length < NumFields((ModelDb.PlanType)  thePlan.type))
				return "Expecting seven columns of data";

			// first find channel
			string query = "channel_name = '" + items[0] + "'";
			DataRow[] rows = theDb.Data.channel.Select(query);

			if (rows.Length == 0)
			{
				return "could not find channel named " + items[0];
			}
			
			int channel_id = (int) rows[0]["channel_id"];

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.distributionRow distribution = theDb.CreateDistribution(product_id, channel_id);

			theDb.MarketPlanID = curPlanId;

			try
			{
				distribution.attr_value_F = Double.Parse(items[1]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				distribution.attr_value_G = Double.Parse(items[2]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				distribution.message_awareness_probability = Double.Parse(items[3]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			
			try
			{
				distribution.message_persuation_probability = Double.Parse(items[4]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				distribution.start_date = DateTime.Parse(items[5]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				distribution.end_date = DateTime.Parse(items[6]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			return null;
		}
		
	
		private string createMassMediaRow(MrktSimDBSchema.market_planRow thePlan, string[] items)
		{
			// we do not use cost per impression right now
			// const bool useCost = false;
			const bool doNotSeperateCoupons = false;

			if (items.Length < NumFields((ModelDb.PlanType)  thePlan.type))
				return "Missing data from row";

			// first find segment
			int index = 0;

			string query = "segment_name = '" + items[index] + "'";
			DataRow[] rows = theDb.Data.segment.Select(query);

			if (rows.Length == 0)
			{
				return "could not find segment named " + items[index];
			}
			index++;
			
			int segment_id = (int) rows[0]["segment_id"];

			// first find channel
			query = "channel_name = '" + items[index] + "'";
			rows = theDb.Data.channel.Select(query);

			if (rows.Length == 0)
			{
				return "could not find channel named " + items[index];
			}
			index++;
			
			int channel_id = (int) rows[0]["channel_id"];

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.mass_mediaRow media = theDb.CreateMassMedia(product_id, channel_id, segment_id);

			theDb.MarketPlanID = curPlanId;

			// type
			media.media_type = items[index];
			index++;

			// check for coupons
			bool Coupon = media.media_type == "C";

			// GRP or coupon reach
			try
			{
				media.attr_value_G = Double.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			index++;

			// cost per advertisement
			// if (useCost)
			//			{
			//				try
			//				{
			//					media.attr_value_H = Double.Parse(items[index]);
			//				}
			//				catch(System.Exception e)
			//				{
			//					return e.Message;
			//				}
			//				index++;
			//			}

			// coupon redemption rate
			if (doNotSeperateCoupons || Coupon)
			{
				try
				{
					media.attr_value_I = Double.Parse(items[index]);
				}
				catch(System.Exception e)
				{
					return e.Message;
				}
				index++;
			}


			if (index == items.Length)
				return "Missing data from row";
			// awareness
			try
			{
				media.message_awareness_probability = Double.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			index++;
			
			if (index == items.Length)
				return "Missing data from row";
			// persuasion
			try
			{
				media.message_persuation_probability = Double.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			index++;

			if (index == items.Length)
				return "Missing data from row";
			// start date
			try
			{
				media.start_date = DateTime.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			index++;

			if (index == items.Length)
				return "Missing data from row";
			// end data
			try
			{
				media.end_date = DateTime.Parse(items[index]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			index++;

			return null;
		}


		private string createMarketUtilityRow(MrktSimDBSchema.market_planRow thePlan, string[] items)
		{
			if (items.Length < NumFields((ModelDb.PlanType)  thePlan.type))
				return "Expecting eight columns of data";

			// first find segment
			int index = 0;

			string query = "segment_name = '" + items[index] + "'";
			DataRow[] rows = theDb.Data.segment.Select(query);

			if (rows.Length == 0)
			{
				return "could not find segment named " + items[index];
			}
			index++;
			
			int segment_id = (int) rows[0]["segment_id"];

			// first find channel
			query = "channel_name = '" + items[0] + "'";
			rows = theDb.Data.channel.Select(query);

			if (rows.Length == 0)
			{
				return "could not find channel named " + items[0];
			}
			
		
			int channel_id = (int) rows[0]["channel_id"];

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.market_utilityRow market_utility = theDb.CreateMarketUtility(product_id, channel_id, segment_id);

			theDb.MarketPlanID = curPlanId;

			try
			{
				market_utility.percent_dist = Double.Parse(items[1]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				market_utility.awareness = Double.Parse(items[2]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}
			
			try
			{
				market_utility.persuasion = Double.Parse(items[3]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				market_utility.utility = Double.Parse(items[4]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				market_utility.start_date = DateTime.Parse(items[5]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			try
			{
				market_utility.end_date = DateTime.Parse(items[6]);
			}
			catch(System.Exception e)
			{
				return e.Message;
			}

			return null;
		}


	}

	#endregion

	public class PlanReader
	{

		private Hashtable myProdToPlan;
		private string myBaseName;
		private ModelDb theDb;
		private int overWrite;
		public int OverWrite
		{
			set
			{
				overWrite = value;
			}
		}

		public PlanReader(ModelDb db, string base_name)
		{
			myProdToPlan = new Hashtable();
			myBaseName = base_name;
			theDb = db;
			overWrite = 0;
		}


        public ErrorList CreatePlan( string workbook_path, ModelDb.PlanType planType, bool reportMissingSheet ) {
            ArrayList createdPlanIDs = new ArrayList();
            return CreatePlan( workbook_path, planType, reportMissingSheet, out createdPlanIDs );
        }

        public ErrorList CreatePlan( string workbook_path, ModelDb.PlanType planType, bool reportMissingSheet, out ArrayList createdPlanRows )
		{
            Console.WriteLine( ">>> CreatePlan( {0}, {1}, {2} )", workbook_path.Substring( workbook_path.LastIndexOf( "\\" ) ) + 1, planType.ToString(), reportMissingSheet );

			string table_name = "";
            createdPlanRows = new ArrayList();
            ErrorList errors = new ErrorList();
			switch (planType)
			{
				case ModelDb.PlanType.MarketPlan :
					break;
				case ModelDb.PlanType.Price :
					table_name = "Price";
					break;
				case ModelDb.PlanType.Display :
					table_name = "Display";
					break;
				case ModelDb.PlanType.Distribution :
					table_name = "Distribution";
					break;
				case ModelDb.PlanType.Media :
					table_name = "Media";
					break;
				case ModelDb.PlanType.Market_Utility :
					table_name = "MarketUtility";
					break;
				case ModelDb.PlanType.Coupons :
					table_name = "Coupons";
					break;
				case ModelDb.PlanType.ProdEvent :
					table_name = "External Factors";
					break;
				default :
					errors.addError(null, "Object not found", "Could not find plan type" + planType);
					return errors;
			}

			DataTable table;
            errors.addErrors( ExcelReader.ReadTable( workbook_path, table_name, "M", reportMissingSheet, out table ) );

			if(errors.Count > 0)
			{
				return errors;
			}

			if (table != null)
			{
                errors.addErrors( CreatePlanData( table, planType, out createdPlanRows ) );
                Console.WriteLine( "      CreatePlan() created {0} rows", createdPlanRows.Count );
			}

			return errors;
		}

 		private ErrorList CreatePlanData(DataTable table, ModelDb.PlanType planType, out ArrayList createdPlanRows )
        {
            Console.WriteLine( ">>>>> CreatePlanData( {0} )", planType.ToString() );

			ErrorList errors = new ErrorList();
            createdPlanRows = new ArrayList();
			bool useCampaign = true;


			foreach(DataRow row in table.Select("","",DataViewRowState.CurrentRows))
			{
			
				MrktSimDBSchema.market_planRow curPlan = null;
				string product = null;
				string campaign = "";
				

				try
				{
					product = row["product"].ToString();
				}
				catch(System.Exception)
				{
					errors.addError(row, "Object not found", "Could not find product column in " + table.TableName);
					continue;
				}

				if(useCampaign)
				{
					try
					{
						campaign = row["campaign"].ToString();
					}
					catch(System.Exception)
					{
						// errors.addError(row, "Object not found", "Could not find campaign column in " + table.TableName + ". Not using campaigns");
						useCampaign = false;
						campaign = "";
					}
				}

				if (!myProdToPlan.ContainsKey(row["product"].ToString() + campaign + planType))
				{
					// check for product
					string query = "product_name = '" + row["product"] + "'";
					string planName = null;
					DataRow[] rows = theDb.Data.product.Select(query,"",DataViewRowState.CurrentRows);

					if (rows.Length == 0)
					{
						errors.addError(table,"Object not found","Could not find product named " + row["product"] + " in table: " + table.TableName);
						continue;
					}

                    if( planType != ModelDb.PlanType.ProdEvent ) {
                        if( useCampaign ) {
                            planName = myBaseName + " - " + campaign + " - " + rows[ 0 ][ "product_name" ];
                        }
                        else {
                            planName = myBaseName + " - " + rows[ 0 ][ "product_name" ];
                        }

                        if( !checkMarketPlan( planName, planType, out curPlan ) ) {
                            curPlan = theDb.CreateMarketPlan( planName, planType );
                            curPlan.product_id = (int)rows[ 0 ][ "product_id" ];
                        }
                    }
                    else {
                        // importing an external factors file
                        if( useCampaign ) {
                            planName = string.Format( "{0} - external factors - {1} - {2}", myBaseName, campaign,  (string)rows[ 0 ][ "product_name" ] );
                        }
                        else {
                            planName = string.Format( "{0} - external factors - {1}", myBaseName, (string)rows[ 0 ][ "product_name" ] );
                        }

                        if( !checkMarketPlan( planName, planType, out curPlan ) ) {
                            curPlan = theDb.CreateMarketPlan( planName, planType );
                            curPlan.product_id = (int)rows[ 0 ][ "product_id" ];
                        }
                    }

					myProdToPlan.Add(row["product"].ToString() + campaign + planType, curPlan);
				}
				else
				{
					curPlan = (MrktSimDBSchema.market_planRow) myProdToPlan[row["product"].ToString() + campaign + planType];
				}

				switch (planType)
				{
					case ModelDb.PlanType.Price :
						errors.addErrors(createPriceRow(curPlan, row));
						break;

					case ModelDb.PlanType.Distribution :
						errors.addErrors(createDistributionRow(curPlan, row));
						break;

					case ModelDb.PlanType.Display :
						errors.addErrors(createDisplayRow(curPlan, row));
						break;
					case ModelDb.PlanType.Media :
						errors.addErrors(createMassMediaRow(curPlan, row));
						break;
					case ModelDb.PlanType.Coupons :
						errors.addErrors(createCouponRow(curPlan, row));
						break;
					case ModelDb.PlanType.Market_Utility :
						errors.addErrors(createMarketUtilityRow(curPlan, row));
						break;
					case ModelDb.PlanType.ProdEvent :
						errors.addErrors(createExternalFactorRow(curPlan, row));
						break;
				}

                if( createdPlanRows.Contains( curPlan ) == false ) {
                    createdPlanRows.Add( curPlan );
                }

			}
            Console.WriteLine( "      CreatePlanData() created {0} rows", createdPlanRows.Count );
            return errors;
		}

        public void createTopLevelMarketPlan( MrktSimDBSchema.scenarioRow scenario ) {
            createTopLevelMarketPlan( scenario, true );
        }

		public void createTopLevelMarketPlan(MrktSimDBSchema.scenarioRow scenario, bool warnIfPlanExistsAlready )
		{
            Console.WriteLine( ">>>>>>      createTopLevelMarketPlan( scenario )" );

            Hashtable prodToHighLevelPlan = new Hashtable();
			MrktSimDBSchema.market_planRow curPlan =  null;
			foreach(MrktSimDBSchema.market_planRow row in myProdToPlan.Values)
			{
				if(row.type != (byte) ModelDb.PlanType.ProdEvent)
				{

					if(prodToHighLevelPlan.ContainsKey(row.product_id))
					{
						curPlan = (MrktSimDBSchema.market_planRow) prodToHighLevelPlan[row.product_id];
					}
					else
					{
						MrktSimDBSchema.productRow prod = theDb.Data.product.FindByproduct_id(row.product_id);
                        if( warnIfPlanExistsAlready == false ) {
                            this.overWrite = 1;   // prevent the dialog from being shown by second and subsequent files in a group
                        }

                        if( !checkMarketPlan( myBaseName + " - " + prod.product_name, ModelDb.PlanType.MarketPlan, out curPlan ) ) {
                            curPlan = theDb.CreateMarketPlan( myBaseName + " - " + prod.product_name, ModelDb.PlanType.MarketPlan );
                            curPlan.product_id = row.product_id;
                        }

						prodToHighLevelPlan.Add(row.product_id, curPlan);
					}
					// add component to plan
					theDb.CreatePlanRelation(curPlan, row);

					// add to scenario
					if (scenario != null)
					{
						theDb.AddMarketPlanToScenario(scenario, curPlan);
					}
				}
			}
		}

		private bool checkMarketPlan(string plan_name, ModelDb.PlanType plan_type, out MrktSimDBSchema.market_planRow plan)
		{
            Console.WriteLine( "> > > > > checkMarketPlan( {0}, {1} )", plan_name, plan_type.ToString() );

			plan = null;
			byte type = (byte) plan_type;
			string query = "name = '" + plan_name + "' AND type = " + type;
			DataRow[] rows = theDb.Data.market_plan.Select(query,"",DataViewRowState.CurrentRows);
			if( rows.Length == 0)
			{
				return false;
			}
			else
			{
				plan = (MrktSimDBSchema.market_planRow) rows[0];
				if(overWrite == 0)
				{
                    string msg = String.Format( "\r\n    The \" {0}\" Market Plan exists already.   Imported data will be added to the data for this plan.    \r\n\r\n" +
                        "OK to add the imported data to the plan?     \r\n\r\n", plan_name );
                    DialogResult rslt = MessageBox.Show( msg, "Dupilicate Data", MessageBoxButtons.YesNo, MessageBoxIcon.Warning );
					if(rslt == DialogResult.Yes)
					{
						overWrite = 1;
					}
					else
					{

						overWrite = -1;
					}
				}
				if(overWrite == 1)
				{
					ClearData(plan, plan_type);       
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		private void ClearData(MrktSimDBSchema.market_planRow plan, ModelDb.PlanType plan_type)
		{
			DataRow[] rows = null;
			DataTable table = null;
			string query = "market_plan_id = " + plan.id;
			switch (plan_type)
			{
				case ModelDb.PlanType.MarketPlan :
					return;
				case ModelDb.PlanType.Price :
					table = theDb.Data.product_channel;
					break;
				case ModelDb.PlanType.Distribution :
					table = theDb.Data.distribution;
					break;
				case ModelDb.PlanType.Display :
					table = theDb.Data.display;
					break;
				case ModelDb.PlanType.Media :
					table = theDb.Data.mass_media;
					break;
				case ModelDb.PlanType.Coupons :
					table = theDb.Data.mass_media;
					break;
				case ModelDb.PlanType.Market_Utility :
					table = theDb.Data.market_utility;
					break;
				case ModelDb.PlanType.ProdEvent :
					table = theDb.Data.product_event;
					break;
			}

			// theDb.ReadPlanData(plan);

			rows = table.Select(query,"",DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				row.Delete();
			}
		}

		private bool checkChannel(string channel_name, out int channel_id)
		{
			channel_id = -1;
			string query = "channel_name = '" + channel_name + "'";
			DataRow[] rows = theDb.Data.channel.Select(query,"",DataViewRowState.CurrentRows);

			if (rows.Length == 0)
			{
				return false;
			}
			else
			{
				channel_id = (int) rows[0]["channel_id"];
				return true;
			}
		}

		private bool checkSegment(string segment_name, out int segment_id)
		{
			segment_id = -1;
			string query = "segment_name = '" + segment_name + "'";
			DataRow[] rows = theDb.Data.segment.Select(query);

			if (rows.Length == 0)
			{
				return false;
			}
			else
			{
				segment_id = (int) rows[0]["segment_id"];
				return true;
			}
		}

		private ErrorList createPriceRow(MrktSimDBSchema.market_planRow thePlan, DataRow row)
		{
			ErrorList errors = new ErrorList();
			// first find channel
			int channel_id;
			string channel = "";
			try
			{
				channel = row["Channel"].ToString();
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Object not found", e.Message);
				return errors;
			}
			if(!checkChannel(channel, out channel_id))
			{
				errors.addError(row, "Object not found", "Could not find channel: " + row["channel"]);
				return errors;
			}

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.product_channelRow price = theDb.CreateProductChannel(product_id, channel_id);

			theDb.MarketPlanID = curPlanId;

			try
			{
				price.price = Double.Parse(row["Price"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			// Change here from a string ptype to identified for price_type
			try
			{
                string ptype = row["Purchase Type"].ToString().Trim().ToLower();

                if( ptype == "unpromoted" )
                {
                    price.price_type = Database.AllID;
                }
                else
                {
                    MrktSimDBSchema.price_typeRow promo = null;

                    foreach( MrktSimDBSchema.price_typeRow tmp in theDb.Data.price_type )
                    {
                        if( tmp.name.ToLower() == ptype.ToLower() )
                        {
                            promo = tmp;
                            break;
                        }
                    }

                    price.price_type = promo.id;
                }
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				price.percent_SKU_in_dist = Double.Parse(row["% Distr"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}
			
			try
			{
				price.start_date = DateTime.Parse(row["Start Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				price.end_date = DateTime.Parse(row["End Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			return errors;
		}


		private ErrorList createDisplayRow(MrktSimDBSchema.market_planRow thePlan, DataRow row)
		{
			ErrorList errors = new ErrorList();
			// first find channel
			int channel_id;
			string channel = "";
			try
			{
				channel = row["Channel"].ToString();
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Object not found", e.Message);
				return errors;
			}
			if(!checkChannel(channel, out channel_id))
			{
				errors.addError(row, "Object not found", "Could not find channel: " + row["channel"]);
				return errors;
			}

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.displayRow display = theDb.CreateDisplay(product_id, channel_id);

			theDb.MarketPlanID = curPlanId;

			try
			{
				display.attr_value_F = Double.Parse(row["% Display"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				display.message_awareness_probability = Double.Parse(row["Aware Prob"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}
			
			try
			{
				display.message_persuation_probability = Double.Parse(row["Persuasion"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				display.start_date = DateTime.Parse(row["Start Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				display.end_date = DateTime.Parse(row["End Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			return errors;
		}
		

		private ErrorList createDistributionRow(MrktSimDBSchema.market_planRow thePlan, DataRow row)
		{
			ErrorList errors = new ErrorList();
			// first find channel
			int channel_id;
			string channel = "";
			try
			{
				channel = row["Channel"].ToString();
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Object not found", e.Message);
				return errors;
			}
			if(!checkChannel(channel, out channel_id))
			{
				errors.addError(row, "Object not found", "Could not find channel: " + row["channel"]);
				return errors;
			}

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.distributionRow distribution = theDb.CreateDistribution(product_id, channel_id);

			theDb.MarketPlanID = curPlanId;

			try
			{
				distribution.attr_value_F = Double.Parse(row["% Dist"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				distribution.attr_value_G = Double.Parse(row["% Init"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				distribution.message_awareness_probability = Double.Parse(row["Aware Prob"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}
			
			try
			{
				distribution.message_persuation_probability = Double.Parse(row["Persuasion"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				distribution.start_date = DateTime.Parse(row["Start Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				distribution.end_date = DateTime.Parse(row["End Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			return errors;
		}


		private ErrorList createMassMediaRow(MrktSimDBSchema.market_planRow thePlan, DataRow row)
		{
			ErrorList errors = new ErrorList();

			int segment_id;
			string segment = "";
			try
			{
				segment = row["Segment"].ToString();
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Object not found", e.Message);
				return errors;
			}
			if(!checkSegment(segment, out segment_id))
			{
				errors.addError(row, "Object not found", "Could not find segment: " + row["Segment"]);
				return errors;
			}

			int channel_id;
			string channel = "";
			try
			{
				channel = row["Channel"].ToString();
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Object not found", e.Message);
				return errors;
			}
			if(!checkChannel(channel, out channel_id))
			{
				errors.addError(row, "Object not found", "Could not find channel: " + row["channel"]);
				return errors;
			}

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.mass_mediaRow media = theDb.CreateMassMedia(product_id, channel_id, segment_id);

			theDb.MarketPlanID = curPlanId;

			// type
			media.media_type = row["Type"].ToString();

			// GRP or coupon reach
			try
			{
				media.attr_value_G = Double.Parse(row["GRP"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			
			try
			{
				media.message_awareness_probability = Double.Parse(row["Aware Prob"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}
			
		
			try
			{
				media.message_persuation_probability = Double.Parse(row["Persuasion"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}
			
			try
			{
				media.start_date = DateTime.Parse(row["Start Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}
			
			try
			{
				media.end_date = DateTime.Parse(row["End Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			return errors;
		}


		private ErrorList createCouponRow(MrktSimDBSchema.market_planRow thePlan, DataRow row)
		{
			ErrorList errors = new ErrorList();

			int segment_id;
			string segment = "";
			try
			{
				segment = row["Segment"].ToString();
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Object not found", e.Message);
				return errors;
			}
			if(!checkSegment(segment, out segment_id))
			{
				errors.addError(row, "Object not found", "Could not find segment: " + row["Segment"]);
				return errors;
			}

			int channel_id;
			string channel = "";
			try
			{
				channel = row["Channel"].ToString();
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Object not found", e.Message);
				return errors;
			}
			if(!checkChannel(channel, out channel_id))
			{
				errors.addError(row, "Object not found", "Could not find channel: " + row["channel"]);
				return errors;
			}

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.mass_mediaRow media = theDb.CreateMassMedia(product_id, channel_id, segment_id);

			theDb.MarketPlanID = curPlanId;

			// type
			try
			{
				media.media_type = row["Type"].ToString();

			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			// reach
			try
			{
				media.attr_value_G = Double.Parse(row["Reach"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			
			try
			{
				media.attr_value_I = Double.Parse(row["Redemption Rate"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}


			try
			{
				media.message_awareness_probability = Double.Parse(row["Aware Prob"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}
			
		
			try
			{
				media.message_persuation_probability = Double.Parse(row["Persuasion"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}
			
			try
			{
				media.start_date = DateTime.Parse(row["Start Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}
			
			try
			{
				media.end_date = DateTime.Parse(row["End Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			return errors;
		}


		private ErrorList createMarketUtilityRow(MrktSimDBSchema.market_planRow thePlan, DataRow row)
		{
			
			ErrorList errors = new ErrorList();

			int segment_id;
			string segment = "";
			try
			{
				segment = row["Segment"].ToString();
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Object not found", e.Message);
				return errors;
			}
			if(!checkSegment(segment, out segment_id))
			{
				errors.addError(row, "Object not found", "Could not find segment: " + row["Segment"]);
				return errors;
			}

			int channel_id;
			string channel = "";
			try
			{
				channel = row["Channel"].ToString();
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Object not found", e.Message);
				return errors;
			}
			if(!checkChannel(channel, out channel_id))
			{
				errors.addError(row, "Object not found", "Could not find channel: " + row["channel"]);
				return errors;
			}

			int product_id = thePlan.product_id;
			int curPlanId = theDb.MarketPlanID;
			theDb.MarketPlanID = thePlan.id;

			MrktSimDBSchema.market_utilityRow market_utility = theDb.CreateMarketUtility(product_id, channel_id, segment_id);

			theDb.MarketPlanID = curPlanId;

			try
			{
				market_utility.percent_dist = Double.Parse(row["Distribution"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				market_utility.awareness = Double.Parse(row["Awareness"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}
			
			try
			{
				market_utility.persuasion = Double.Parse(row["Persuasion"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				market_utility.utility = Double.Parse(row["Utility"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				market_utility.start_date = DateTime.Parse(row["Start Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				market_utility.end_date = DateTime.Parse(row["End Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			return errors;
		}

		private ErrorList createExternalFactorRow(MrktSimDBSchema.market_planRow thePlan, DataRow row)
		{
			
			ErrorList errors = new ErrorList();

			int segment_id;
			string segment = "";
			try
			{
				segment = row["Segment"].ToString();
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Object not found", e.Message);
				return errors;
			}
			if(!checkSegment(segment, out segment_id))
			{
				errors.addError(row, "Object not found", "Could not find segment: " + row["Segment"]);
				return errors;
			}

			int channel_id;
			string channel = "";
			try
			{
				channel = row["Channel"].ToString();
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Object not found", e.Message);
				return errors;
			}
			if(!checkChannel(channel, out channel_id))
			{
				errors.addError(row, "Object not found", "Could not find channel: " + row["channel"]);
				return errors;
			}

			int product_id = thePlan.product_id;

            MrktSimDBSchema.product_eventRow external_factor = theDb.CreateProductEvent(thePlan.id, product_id, channel_id, segment_id);

			try
			{
				external_factor.demand_modification = Double.Parse(row["Demand"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

            try
            {
                string valString =  row["Type"].ToString();
                ModelDb.ProductEventType val = (ModelDb.ProductEventType) Enum.Parse( typeof( ModelDb.ProductEventType ), valString );

                external_factor.type = (byte) val;
            }
            catch( System.Exception e )
            {
                errors.addError( row, "Parse error", e.Message );
            }
            

			try
			{
				external_factor.start_date = DateTime.Parse(row["Start Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			try
			{
				external_factor.end_date = DateTime.Parse(row["End Date"].ToString());
			}
			catch(System.Exception e)
			{
				errors.addError(row, "Parse error", e.Message);
			}

			return errors;
		}
	}
}
