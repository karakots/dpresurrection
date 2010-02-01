using System;

using System.Data;
using MrktSimDb;
using ErrorInterface;

namespace ExcelInterface
{
	/// <summary>
	/// Summary description for ProductSizeReader.
	/// </summary>
	
	public class ProductSizeReader
	{
		Database theDb;

		public ProductSizeReader(Database db)
		{
			theDb = db;
		}

		

		public ErrorList Read(DataTable table)
		{
			ErrorList errors = new ErrorList();

			if (table.Columns.Count < 2)
			{
				return errors;
			}

			string[] channels = new string[table.Columns.Count - 1];
			for(int i = 1; i < table.Columns.Count; i++)
			{
				channels[i- 1] = table.Columns[i].ColumnName;
			}

			string query = "";
			double prod_size = 1;
			int channel_id = 0;
			int product_id = 0;
			MrktSimDBSchema.product_channel_sizeRow size_row;

			foreach(DataRow row in table.Select("","",DataViewRowState.CurrentRows))
			{
				channel_id = 0;
				product_id = 0;

				string product = (string)row["Product"];
				query = "product_name = '" + product + "'";
				DataRow[] rows = theDb.Data.product.Select(query,"",DataViewRowState.CurrentRows);
				
				if(rows.Length > 0)
				{
					product_id = (int)rows[0]["product_id"];
				}
				else
				{
					continue;
				}

				foreach(string channel in channels)
				{
				
					try
					{
						prod_size = (double)row[channel];
					}
					catch
					{
						continue;
					}

					query = "channel_name = '" + channel + "'";
					rows = theDb.Data.channel.Select(query,"",DataViewRowState.CurrentRows);
					if(rows.Length > 0)
					{
						channel_id = (int)rows[0]["channel_id"];
					}
					else
					{
						continue;
					}

					query = "product_id = " + product_id + " AND channel_id = " + channel_id;
					rows = theDb.Data.product_channel_size.Select(query,"",DataViewRowState.CurrentRows);
					if(rows.Length > 0)
					{
						size_row = (MrktSimDBSchema.product_channel_sizeRow)rows[0];
						size_row.prod_size = prod_size;
					}
				}
			}

			return errors;
		}		
	}
}

