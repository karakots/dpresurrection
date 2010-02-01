using System;

using System.Data;

using System.Collections;



namespace MrktSimDb

{

	/// <summary>

	/// Validates a model

	/// </summary>

	

	public class ModelValidation

	{

		public enum ErrorType

		{

			SegmentChannelChoice = 0

		}

		

		private const double eps = 1.0e-6;



		private ModelDb theDb;



		// private ArrayList errors;



		public ModelValidation(ModelDb db)

		{

			theDb = db;



		}



		public string[] Errors

		{

			get

			{

				string[] rval = {"none"};

				return rval;

			}

		}



		private void addError(ErrorType type , string what)

		{

		}



		public bool CheckSegments()

		{

			// Be sure the loyalty is between 0 and 10.



			return true;

		}



		public bool CheckPurchaseModel()

		{

			// For a Task-Based Buying scenario, are there any tasks defined?  

			// Is the “repurchase model” in the “Consumer Segments” sheet set to “t” (or “b”)?

			// For Periodic Repurchasing scenario 

			// (“f” or “n” repurchase model in the “Consumers & Segments” sheet), 

			// is the repurchase period or frequency set to a non-zero value?

			// For “r” repurchasing (line 18), 

			// is the “Average Max Units Purchased” (line 51) greater than zero?

			// Be sure  the parameters associated with the chosen repurchase model 

			// (fixed, nbd, or task-based) are entered.



			return true;

		}



		public bool CheckShareAndPenetration()

		{

			// Initial Share should not be higher than initial penetration

			// 



			return true;

		}



		public bool CheckChannels(bool fixit)

		{

			int numChannels = theDb.Data.segment_channel.Rows.Count;



			// nothing doing

			if (numChannels == 0)

				return true;



			// Be sure the percent of each segment shopping at each channel add up to 100 for each segment



			foreach (MrktSimDBSchema.segmentRow segment in theDb.Data.segment.Select("","", DataViewRowState.CurrentRows))

			{

				// check the channels for segment



				double prob = 0.0;

				string query = "segment_id = " + segment.segment_id;



				foreach (MrktSimDBSchema.segment_channelRow segchan in theDb.Data.segment_channel.Select(query,"", DataViewRowState.CurrentRows))

				{

					prob += double.Parse(segchan.probability_of_choice);

				}



				if ( prob != 1.0 )

				{

					// remember name of segment

					// don't forget to externalize

					addError(ErrorType.SegmentChannelChoice, segment.segment_name);



					if (fixit)

					{

						if (Math.Abs(prob) < eps)

						{

							// make all the same

							double sameVal = 1.0/numChannels;



							foreach (MrktSimDBSchema.segment_channelRow segchan in theDb.Data.segment_channel.Select(query,"", DataViewRowState.CurrentRows))

							{

								segchan.probability_of_choice = sameVal.ToString();

							}

						}

						else

						{

							// divide by prob

							foreach (MrktSimDBSchema.segment_channelRow segchan in theDb.Data.segment_channel.Select(query,"", DataViewRowState.CurrentRows))

							{

								double val = double.Parse(segchan.probability_of_choice);

								val /= prob;

								segchan.probability_of_choice = val.ToString();

							}

						}

					}

				}

			}



			return true;

		}

	}



	public class ScenarioValidation

	{

		private MrktSimDBSchema.scenarioRow theScenario;

		

		public ScenarioValidation(MrktSimDBSchema.scenarioRow scenario) 

		{

			theScenario = scenario;

		}



		public bool CheckPrices()

		{

			// Make sure prices probabilities add up to 1.

			// Need to talk to Ken about this one:

			// BOGO price entries may not have 0 entered in the “Price & %BOGO Skip” column.  



			return true;

		}



		public bool CheckAwareness()

		{



			// For non-task-based models, consumers need awareness of at least one product before they can go shopping.  

			// Awareness is usually generated in the mass-media sheet.  Check the dates of the mass-media items. 

			// Awareness is also generated through “initial share” values in the “Companies & Products” spreadsheet.  If these initial shares are all zero, and there are no mass-media items, there is nothing to make the consumers aware of any products.

			// Check should be

			// Not tasked Based && no initial awareness && no media awareness == no products sold





			return true;

		}

	}

}

