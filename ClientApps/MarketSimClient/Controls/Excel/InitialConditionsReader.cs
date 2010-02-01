using System;
using System.Collections;
using System.Data;
using MrktSimDb;
using ErrorInterface;

namespace ExcelInterface
{
	/// <summary>
	/// Summary description for InitialConditionsReader.
	/// </summary>
	public class InitialConditionsReader
	{
		public enum InitialConditionValueType
		{
			Share,
			Penetration,
			Persuasion,
			Awareness
		}

		Database theDb;


		public InitialConditionsReader(Database db)
		{
			theDb = db;
		}

		public ErrorList ReadInitialConditions(DataTable table, InitialConditionValueType type)
		{
			ErrorList errors = new ErrorList();
			double val = 0;
			string prod_name;
			int prod_id;
			string query;
			string value_type;

			switch(type)
			{
				case InitialConditionValueType.Share:
					value_type = "initial_share";
					break;
				case InitialConditionValueType.Penetration:
					value_type = "penetration";
					break;
				case InitialConditionValueType.Persuasion:
					value_type = "persuasion";
					break;
				case InitialConditionValueType.Awareness:
					value_type = "brand_awareness";
					break;
				default:
					errors.addError(new Error(type, "Unknown Type", "Unknown type used"));
					return errors;
			}

			DataRow[] segments = theDb.Data.segment.Select("", "", DataViewRowState.CurrentRows);
			foreach(DataRow seg_row in segments)
			{
				string seg_name = (string)seg_row["segment_name"];
				int seg_id = (int)seg_row["segment_id"];
				if(seg_id < 0)
				{
					continue;
				}
				foreach(DataRow row in table.Select())
				{
                    val = -1;
                    try
                    {
                        val = (double)row["All"];
                    }
                    catch (Exception)
                    {
                    }
                    if (val == -1)
                    {
                        try
                        {
                            val = (double)row[seg_name];
                        }
                        catch (Exception)
                        {
                            errors.addError(new Error(null, "Object not found", "Could not find segment: " + seg_name + " in the Excel file"));
                            continue;
                        }  
                    }

					try
					{
						prod_name = (string)row["Product"];
					}
					catch(Exception)
					{
						errors.addError(new Error(null, "Object not found", "Could not find Product column in the Excel file"));
						return errors;
					}
					query = "product_name = '" + prod_name + "'";
					DataRow[] rows = theDb.Data.product.Select(query,"",DataViewRowState.CurrentRows);
					if(rows.Length > 0)
					{
						prod_id = (int)rows[0]["product_id"];
					}
					else
					{
						errors.addError(new Error(null, "Object Not Found","Could not find product: " + prod_name + " in the database"));
						continue;
					}
					query = "segment_id = " + seg_id + " AND product_id = " + prod_id;
					rows = theDb.Data.share_pen_brand_aware.Select(query, "", DataViewRowState.CurrentRows);
					if(rows.Length > 0)
					{
						rows[0][value_type] = val;
					}
				}
			}

			return errors;
		}
	}
}
