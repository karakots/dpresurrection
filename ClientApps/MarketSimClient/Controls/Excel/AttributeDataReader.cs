using System;
using System.Text;
using System.Data;
using MrktSimDb;
using ErrorInterface;

namespace ExcelInterface
{
    public class AttributeDataReader
    {
        public enum AttributeValueType
        {
            PreVal,
            PostVal,
            PreAndPost,
            PriceUtil
        }

        ModelDb theDb;

        public AttributeDataReader(ModelDb db)
        {
            theDb = db;
        }

       
        /// <summary>
        ///  Creates attributes are in database as needed
        /// returns list of attributes
        /// </summary>
        private MrktSimDBSchema.product_attributeRow[] CreateAttributes(DataTable table)
        {
            // no attibutes
            // first column is either product or segment
            if (table.Columns.Count < 2)
                return null;

            char[] whitespace = new char[] {' '};
			string[] ban = {"'", "\n", "\t", "\r"};

			// count number of attributes
			string[] attrNames = new string[table.Columns.Count - 1];

			int count = 0;
			for (int i = 1; i < table.Columns.Count; i++)
			{
				string attribute = table.Columns[i].ColumnName;

				
				foreach(string remove in ban)
				{
					attribute = attribute.Replace(remove, "");
				}

				if (attribute == null)
				{
					break;
				}

				if (attribute == "end")
				{
					break;
				}

				attribute = attribute.TrimStart(whitespace);
				attribute = attribute.TrimEnd(whitespace);

				if (attribute.Length == 0)
				{
					break;
				}

				attrNames[count] = attribute;

				count++;
			}

			if (count == 0)
			{
				return null;
			}

            MrktSimDBSchema.product_attributeRow[] attributes = new MrktSimDBSchema.product_attributeRow[count];

            for (int i = 0; i < count; i++)
            {
                string attribute = attrNames[i];

				if (attribute == null)
				{
					break;
				}

                attribute = attribute.TrimStart(whitespace);
                attribute = attribute.TrimEnd(whitespace);

				

                string query = "product_attribute_name = '" + attribute + "'";
                DataRow[] rows = theDb.Data.product_attribute.Select(query, "", DataViewRowState.CurrentRows);
                if (rows.Length > 0)
                {
                    attributes[i] = (MrktSimDBSchema.product_attributeRow)rows[0];
                }
                else
                {
                    attributes[i] = theDb.CreateProductAttribute(attribute, ModelDb.AttributeType.Standard);
                }
            }

            return attributes;
        }


        /// <summary>
        /// reads in attribute values from table
        /// first column is product
        /// then each column is an attirbute value of designated type
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public ErrorList ReadProductAttributeValues(DataTable table, DateTime startDate, AttributeValueType valType)
        {
            ErrorList errors = new ErrorList();

            MrktSimDBSchema.product_attributeRow[] attributes = CreateAttributes(table);

            if (attributes == null)
            {
                // nothing doing
                errors.addError(null, "No Attributes", "No attributes listed");
            }

            foreach (DataRow row in table.Select("", "", DataViewRowState.CurrentRows))
            {
                int product_id = Database.AllID;
                string product = null;

                try
                {
                    product = (string)row["Product"];
                }
                catch
                {
                    errors.addError(null, "No Product", "Cannot read product from worksheet");
                    break;
                }

                string query = "product_name = '" + product + "'";

                DataRow[] products = theDb.Data.product.Select(query, "", DataViewRowState.CurrentRows);
                if (products.Length > 0)
                {
                    product_id = (int)products[0]["product_id"];
                }
                else
                {
                    errors.addError(null, "No Product", "Cannot find  product named " + product);
                    break;
                }

                foreach (MrktSimDBSchema.product_attributeRow attribute in attributes)
                {
                    double val = 0.0;
                    try
                    {
                        val = (double)row[attribute.product_attribute_name];
                    }
                    catch
                    {

                        errors.addError(null, "Invalid Product Attribute value", "Cannot read attribute value for attribute: " + attribute.product_attribute_name);
                        break;
                    }

                  
                    MrktSimDBSchema.product_attribute_valueRow pav_row = theDb.CreateProductAttributeValue(product_id, attribute.product_attribute_id, startDate);

                    if (valType == AttributeValueType.PreVal || valType == AttributeValueType.PreAndPost)
                    {
                        pav_row.pre_attribute_value = val;
                    }

                    if (valType == AttributeValueType.PostVal || valType == AttributeValueType.PreAndPost)
                    {
                        pav_row.post_attribute_value = val;
                    }
                }
            }

            return errors;
        }


        public ErrorList ReadSegmentAttributeValues(DataTable table, DateTime startDate, AttributeValueType valType)
        {
            ErrorList errors = new ErrorList();

            MrktSimDBSchema.product_attributeRow[] attributes = CreateAttributes(table);

            if (attributes == null)
            {
                // nothing doing
                errors.addError(null, "No Attributes", "No attributes listed");
            }

            foreach (DataRow row in table.Select("", "", DataViewRowState.CurrentRows))
            {
                int segment_id = Database.AllID;
                string segment = null;

                try
                {
                    segment = (string)row["Segment"];
                }
                catch
                {
                    errors.addError(null, "No Segment", "Cannot read segment from worksheet");
                    break;
                }

                string query = "segment_name = '" + segment + "'";
                DataRow[] segments = theDb.Data.segment.Select(query, "", DataViewRowState.CurrentRows);
                if (segments.Length > 0)
                {
                    segment_id = (int)segments[0]["segment_id"];
                }
                else
                {
                    errors.addError(null, "No Segment", "Cannot find  segment named " + segment);
                    break;
                }

                foreach (MrktSimDBSchema.product_attributeRow attribute in attributes)
                {
                    double val = 0;

                    try
                    {
                        val = (double)row[attribute.product_attribute_name];
                    }
                    catch
                    {

                        errors.addError(null, "Invalid Segment Attribute value", "Cannot read attribute value for attribute: " + attribute.product_attribute_name);
                        break;
                    }


                    MrktSimDBSchema.consumer_preferenceRow pref_row = theDb.CreateConsumerPreference(segment_id, attribute.product_attribute_id, startDate);

                    if (valType == AttributeValueType.PreVal || valType == AttributeValueType.PreAndPost)
                    {
                        pref_row.pre_preference_value = val;
                    }

                    if (valType == AttributeValueType.PostVal || valType == AttributeValueType.PreAndPost)
                    {
                        pref_row.post_preference_value = val;
                    }

                    if (valType == AttributeValueType.PriceUtil)
                    {
                        
                        pref_row.price_sensitivity = val;
                    }
                }
            }

            return errors;
        }		
    }
}
