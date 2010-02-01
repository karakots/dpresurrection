using System;
using System.Text;
using System.Data;

using System.Collections.Generic;
using MrktSimDb;
using ErrorInterface;


namespace ExcelInterface
{
	public class MrktSimTableMapper
	{
		public class ColumnMap
		{
			public string mrktSimName = null;
			public string excelName = null;

			public ColumnMap(string mrktsim, string excel)
			{
				mrktSimName = mrktsim;
				excelName = excel;
			}
		}

		public static Dictionary<string, string> SegmentMap()
		{

            Dictionary<string, string> map = new Dictionary<string, string>();

			map.Add("segment_size", "size");

            if( Database.Nimo )
            {
                map.Add( "gamma_location_parameter_a", "gamma a" );

                map.Add(  "gamma_shape_parameter_k", "gamma k" );
            }
            else
            {

              map.Add( "repurchase_period_frequency", "purchase freq" );

               map.Add( "repurchase_frequency_variation", "purchase var" );
            }

			map.Add( "loyalty", "channel loyalty");

			map.Add( "avg_max_units_purch", "units per purchase");

            if( Database.Nimo )
            {
                map.Add( "display_utility", "display utility" );

                map.Add( "min_freq", "min percentile" );

                map.Add( "max_freq", "max percentile" );
                map.Add( "reference_price", "Flow Through Weight" );
            }

			map.Add( "max_display_hits_per_trip", "max display hits");

			return map;
		}
	}


	/// <summary>
	/// Turns a table into segments
	/// </summary>
	public class SegmentReader
	{
		ModelDb theDb;

        public SegmentReader(ModelDb db)
		{
			theDb = db;
		}

		public ErrorList ReadSegment(DataTable table)
		{
			ErrorList errors = new ErrorList();

			foreach(DataRow row in table.Select("","", DataViewRowState.CurrentRows))
			{
				// get the name field
				string name = null;

				try
				{
					name = (string) row["name"];
				}
				catch
				{
					errors.addError(null, "Cannot read column", "Bad data or missing name column");
				}

				if (name == null || name.Length == 0)
				{
					// we are done apparently
					break;
				}
				
				// check if segment exists already
				// if so then simply update the data
				DataRow[] segments = theDb.Data.segment.Select("segment_name = '" + name + "'", "", DataViewRowState.CurrentRows);

				DataRow segment = null;
				if (segments.Length > 0)
				{
					segment = segments[0];
				}
				else
				{
					segment = theDb.CreateSegment(name);
				}

                Dictionary<string,string> mrktSimToExcel = MrktSimTableMapper.SegmentMap();
                foreach( string mrktSimName in mrktSimToExcel.Keys)
				{
					object obj = null;
					try
					{
						 obj = row[mrktSimToExcel[mrktSimName]];
					}
					catch(Exception) // no harm no foul
                    {
                        errors.addError(obj, "Invalid Data", "Bad data for segment " + name + " in column " + mrktSimToExcel[mrktSimName]);
                    }

					if (obj != null)
					{

						try
						{
							segment[mrktSimName] = obj;
						}
						catch
						{
                            errors.addError( obj, "Invalid Data", "Bad data for segment " + name + " Data: " + obj.ToString() );
						}
					}
				}
			}

			return errors;
		}
	}
}
