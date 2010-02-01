using System;
using System.Data;
using System.Collections.Generic;

using MrktSimDb;
using ErrorInterface;

namespace ExcelInterface
{
	/// <summary>
	/// Summary description for PasteData.
	/// </summary>
	public class ProductReader
	{
       
        static string packSizeHeader = "NIMO Pack Size";
        static string basePriceHeader = "Base Price";
        static string eqUnitsHeader = "Eq Units";

        static List<string> prodValues;

        static ProductReader()
        {
            prodValues = new List<string>();

            prodValues.Add( basePriceHeader );

            prodValues.Add( eqUnitsHeader );

            prodValues.Add( packSizeHeader );
        }

		ModelDb theDb;

		public ProductReader(ModelDb db)
		{
			theDb = db;
		}

		public ErrorList CreateProducts(DataTable table)
		{
			ErrorList errors = new ErrorList();

			foreach(DataRow row in table.Select("","",DataViewRowState.CurrentRows))
			{
                Dictionary<string, string> typeOfProduct = new Dictionary<string, string>();
                Dictionary<string, string> colVal = new Dictionary<string, string>();

                // collect up product types
                int i = 0;
                for( ; i < table.Columns.Count; i++ )
                {
                    if( table.Columns[i].ColumnName == "end" || 
                        table.Columns[i].ColumnName.Length == 0 || 
                        prodValues.Contains(table.Columns[i].ColumnName))
                    {
                        break;
                    }
                    else
                    {
                        typeOfProduct.Add( table.Columns[i].ColumnName, row[table.Columns[i].ColumnName].ToString() );
                    }
                }

                // collect up values
                for( ; i < table.Columns.Count; i++ )
                {
                    if( table.Columns[i].ColumnName == "end" || table.Columns[i].ColumnName.Length == 0 )
                    {
                        break;
                    }

                    if( prodValues.Contains( table.Columns[i].ColumnName ) )
                    {
                        colVal.Add( table.Columns[i].ColumnName, row[table.Columns[i].ColumnName].ToString() );
                    }
                }

                errors.addErrors( CreateProduct( typeOfProduct, colVal ) );

				if(errors.Count > 0)
				{
					break;
				}
			}

			return errors;
		}

        //JimJ
        public ErrorList RemoveProducts( DataTable table ) {
            ErrorList errors = new ErrorList();

            foreach( DataRow row in table.Select( "", "", DataViewRowState.CurrentRows ) ) {
                string[] products = new string[ row.ItemArray.Length ];
                for( int i = 0; i < row.ItemArray.Length; i++ ) {
                    products[ i ] = row.ItemArray[ i ].ToString();
                }
                errors.addErrors( RemoveProduct( products ) );

                if( errors.Count > 0 ) {
                    break;
                }
            }

            return errors;
        }		

		public ErrorList CreateProduct(Dictionary<string,string> rowValues, Dictionary<string,string> extras)
		{
			ErrorList errors = new ErrorList();
			MrktSimDBSchema.productRow parent = null;
			
            // there may be more row values then type values
            foreach(string typeName in rowValues.Keys)
			{
                string prodName = rowValues[typeName];
                if( prodName.Length == 0 )
				{
					break;
				}

                MrktSimDBSchema.productRow prod = productByName( prodName );
                MrktSimDBSchema.product_typeRow type = typeByName( typeName );

                // create type if needed
				if(type == null)
				{
					if(!ProjectDb.Nimo)
					{
                        type = theDb.CreateProductType( typeName );
					}
					else
					{
						errors.addError(null, "Nimo error", "Cannot create new product types in Nimo");
						return errors;
					}
				}

                // process product
                if( prod == null )
				{
                    if( parent == null )
                    {
                        // create new root
                        prod = theDb.CreateRootProduct( prodName, typeName );
                    }
                    else
                    {
                        // create new child of parent - replaces parent
                        prod = theDb.CreateChildProduct( parent.product_id, prodName, typeName );
                    }
				}
				else
                {
                    if( parent != null && !checkChild( parent, prod ) )
                    {
                        errors.addError( null, "Invalid Structure", "Tree Property Violated: " + parent.product_name + " and " + prod.product_name + " are not parent-child");
                        return errors;
                    }

                    prod.product_type = type.id;

                }

                parent = prod;
			}

            // assign last child values
            if( parent != null )
            {
                foreach( string valName in extras.Keys )
                {
                    string val = extras[valName];

                    if( val != "" )
                    {
                        if( valName == packSizeHeader )
                        {
                            // associate child tp this pack size
                            MrktSimDBSchema.pack_sizeRow packSize = packSizeByName( val );

                            parent.pack_size_id = packSize.id;
                        }
                        else if( valName == basePriceHeader )
                        {
                            parent.base_price = Double.Parse( val );
                        }
                        else if( valName == eqUnitsHeader )
                        {
                            parent.eq_units = Double.Parse( val );
                        }
                    }
                }
            }
			
			return errors;
		}

        //JimJ
        public ErrorList RemoveProduct( string[] hierarchy ) {
            ErrorList errors = new ErrorList();

            // product name is in the last column 
            MrktSimDBSchema.productRow prodRow = productByName( hierarchy[ hierarchy.Length - 1 ] );
            if( prodRow == null ) {
                return errors;
            }

            // also delete the corresponding product_tree entry
            string query = String.Format( "child_id = {0}", prodRow.product_id );
            MrktSimDBSchema.product_treeRow[] prodTreeRows = (MrktSimDBSchema.product_treeRow[])theDb.Data.product_tree.Select( query );
            if( prodTreeRows.Length == 1 ) {
                prodTreeRows[ 0 ].Delete();
            }
            prodRow.Delete();

            return errors;
        }

		private bool checkChild(MrktSimDBSchema.productRow parentRow, MrktSimDBSchema.productRow childRow)
		{
			//make sure child has correct parent
			string query = "parent_id = " + parentRow.product_id + " AND child_id = " + childRow.product_id;
			DataRow[] tempRow = theDb.Data.product_tree.Select(query,"",DataViewRowState.CurrentRows);
			if(tempRow.Length != 1)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		private MrktSimDBSchema.productRow productByName(string name)
		{
			string query = "product_name = '" + name + "'";
			DataRow[] rows = theDb.Data.product.Select(query,"",DataViewRowState.CurrentRows);
			if(rows.Length > 0)
			{
				return (MrktSimDBSchema.productRow) rows[0];
			}
			return null;
		}

		private MrktSimDBSchema.product_typeRow typeByName(string name)
		{
			string query = "type_name = '" + name + "'";
			DataRow[] rows = theDb.Data.product_type.Select(query,"",DataViewRowState.CurrentRows);
			if(rows.Length > 0)
			{
				return (MrktSimDBSchema.product_typeRow) rows[0];
			}
			return null;
		}

        private MrktSimDBSchema.pack_sizeRow packSizeByName( string name )
        {
            string query = "name = '" + name + "'";
            DataRow[] rows = theDb.Data.pack_size.Select( query, "", DataViewRowState.CurrentRows );
            if( rows.Length > 0 )
            {
                return (MrktSimDBSchema.pack_sizeRow)rows[0];
            }

            return theDb.Data.pack_size.FindByid(Database.AllID);
        }
	}
}
