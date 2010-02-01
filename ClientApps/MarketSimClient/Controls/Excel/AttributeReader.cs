using System;
using System.Data;
using MrktSimDb;
using ErrorInterface;

namespace ExcelInterface
{
	/// <summary>
	/// Summary description for AttributeReader.
	/// </summary>
	public class AttributeReader
	{
		ModelDb theDb;

		public AttributeReader(ModelDb db)
		{
			theDb = db;
		}

		

		public ErrorList ReadInAttributes(DataTable table)
		{
			ErrorList errors = new ErrorList();

			foreach(DataRow row in table.Select("","",DataViewRowState.CurrentRows))
			{
				string attribute = (string)row["Attribute"];
				createAttribute(attribute);
			}

			string query = "";
			DateTime start_date = new DateTime();
			double pre_val = 0;
			double post_val = 0;
			int attrib_id = 0;
			int product_id = 0;
			MrktSimDBSchema.product_attribute_valueRow pav_row;

			foreach(DataRow row in table.Select("","",DataViewRowState.CurrentRows))
			{
				start_date = new DateTime(1900,1,1,0,0,0);
				pre_val = Double.NaN;
				post_val = Double.NaN;
				attrib_id = 0;
				product_id = 0;
				string attribute = (string)row["Attribute"];
				string product = (string)row["Product"];
				
				try
				{
					start_date = DateTime.Parse(row["Start Date"].ToString());
				}
				catch
				{
				}
				try
				{
					pre_val = (double)row["Pre Value"];
				}
				catch
				{
				}
				try
				{
					post_val = (double)row["Post Value"];
				}
				catch
				{
				}
				query = "product_attribute_name = '" + attribute + "'";
				DataRow[] rows = theDb.Data.product_attribute.Select(query,"",DataViewRowState.CurrentRows);
				if(rows.Length > 0)
				{
					attrib_id = (int)rows[0]["product_attribute_id"];
				}
				else
				{
				}
				query = "product_name = '" + product + "'";
				rows = theDb.Data.product.Select(query,"",DataViewRowState.CurrentRows);
				if(rows.Length > 0)
				{
					product_id = (int)rows[0]["product_id"];
				}
				else
				{
				}
				query = "product_id = " + product_id + " AND product_attribute_id = " + attrib_id;
				rows = theDb.Data.product_attribute_value.Select(query,"",DataViewRowState.CurrentRows);
				if(rows.Length > 0)
				{
					pav_row = (MrktSimDBSchema.product_attribute_valueRow)rows[0];
					pav_row.start_date = (start_date.Year == 1900?pav_row.start_date:start_date);
					pav_row.pre_attribute_value = (Double.IsNaN(pre_val)?pav_row.pre_attribute_value:pre_val);
					pav_row.post_attribute_value = (Double.IsNaN(post_val)?pav_row.post_attribute_value:post_val);
				}

			}

			return errors;
		}		


		private MrktSimDBSchema.product_attributeRow createAttribute(string name)
		{
			string query = "product_attribute_name = '" + name + "'";
			
			DataRow[] rows = theDb.Data.product_attribute.Select(query,"",DataViewRowState.CurrentRows);
	
			if(rows.Length > 0)
			{
				return (MrktSimDBSchema.product_attributeRow)rows[0];
			}
			else
			{
				return theDb.CreateProductAttribute(name, ModelDb.AttributeType.Standard);
			}
		}

		private MrktSimDBSchema.product_attribute_valueRow getProductAttributeValueRow(string product, string attribute, ErrorList errors)
		{
			string query = "product_name = '" + product +"'";
			int prod_id;
			int attrib_id;
			DataRow[] row = theDb.Data.product.Select(query,"",DataViewRowState.CurrentRows);
			if(row.Length > 0)
			{
				prod_id = (int)row[0]["product_id"];
			}
			else
			{
				errors.addError(new Error(row,"Object Not Found", "Could not find product: " + product));
				return null;
			}
			query = "product_attribute_name = '" + attribute +"'";
			row = theDb.Data.product_attribute.Select(query,"",DataViewRowState.CurrentRows);
			if(row.Length > 0)
			{
				attrib_id = (int)row[0]["product_attribute_id"];
			}
			else
			{
				errors.addError(new Error(row,"Object Not Found", "Could not find attribute: " + attribute));
				return null;
			}
			query = "product_id = " + prod_id + " AND product_attribute_id = " + attrib_id;
			row = theDb.Data.product_attribute_value.Select(query,"",DataViewRowState.CurrentRows);
			if(row.Length > 0)
			{
				return (MrktSimDBSchema.product_attribute_valueRow)row[0];
			}
			else
			{
				errors.addError(new Error(row,"Object Not Found", "Could not find attribute value: " + attribute + " for " + product));
				return null;
			}
		}
	}
}
