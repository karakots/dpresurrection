using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


using MrktSimDb;
using ErrorInterface;

namespace ExcelInterface
{
    public class SegmentPriceUitlityReader
    {
        ModelDb theDb;

        public SegmentPriceUitlityReader( ModelDb db )
        {
            theDb = db;
        }



        /// <summary>
        ///  Creates attributes are in database as needed
        /// returns list of attributes
        /// </summary>
        private MrktSimDBSchema.price_typeRow[] CreatePriceTypes( DataTable table )
        {
            // no attibutes
            // first column is either product or segment
            if( table.Columns.Count < 2 )
                return null;

            char[] whitespace = new char[] { ' ' };
            string[] ban = { "'", "\n", "\t", "\r" };

            // count number of attributes
            string[] priceNames = new string[table.Columns.Count - 1];

            int count = 0;
            for( int i = 1; i < table.Columns.Count; i++ )
            {
                string priceTypeName = table.Columns[i].ColumnName;


                foreach( string remove in ban )
                {
                    priceTypeName = priceTypeName.Replace( remove, "" );
                }

                if( priceTypeName == null )
                {
                    break;
                }

                if( priceTypeName == "end" )
                {
                    break;
                }

                priceTypeName = priceTypeName.TrimStart( whitespace );
                priceTypeName = priceTypeName.TrimEnd( whitespace );

                if( priceTypeName.Length == 0 )
                {
                    break;
                }

                priceNames[count] = priceTypeName;

                count++;
            }

            if( count == 0 )
            {
                return null;
            }

            MrktSimDBSchema.price_typeRow[] priceTypes = new MrktSimDBSchema.price_typeRow[count];

            for( int i = 0; i < count; i++ )
            {
                string priceTypeName = priceNames[i];

                if( priceTypeName == null )
                {
                    break;
                }

                priceTypeName = priceTypeName.TrimStart( whitespace );
                priceTypeName = priceTypeName.TrimEnd( whitespace );

                string query = "name = '" + priceTypeName + "'";
                DataRow[] rows = theDb.Data.price_type.Select( query, "", DataViewRowState.CurrentRows );
                if( rows.Length > 0 )
                {
                    priceTypes[i] = (MrktSimDBSchema.price_typeRow)rows[0];
                }
                else
                {
                    priceTypes[i] = theDb.CreatePriceType( priceTypeName );
                }
            }

            return priceTypes;
        }


        public ErrorList ReadInPriceUtilities( DataTable table )
        {
            ErrorList errors = new ErrorList();

            MrktSimDBSchema.price_typeRow[] priceTypes = CreatePriceTypes( table );

            if( priceTypes == null )
            {
                // nothing doing
                errors.addError( null, "No Price Types", "No price types listed" );
            }

            foreach( DataRow row in table.Select( "", "", DataViewRowState.CurrentRows ) )
            {
                int segment_id = Database.AllID;
                string segment = null;

                try
                {
                    segment = (string)row["Segment"];
                }
                catch
                {
                    errors.addError( null, "No Segment", "Cannot read segment from worksheet" );
                    break;
                }

                string query = "segment_name = '" + segment + "'";
                DataRow[] segments = theDb.Data.segment.Select( query, "", DataViewRowState.CurrentRows );
                if( segments.Length > 0 )
                {
                    segment_id = (int)segments[0]["segment_id"];
                }
                else
                {
                    errors.addError( null, "No Segment", "Cannot find  segment named " + segment );
                    break;
                }

                foreach( MrktSimDBSchema.price_typeRow priceType in priceTypes )
                {
                    double val = 0;

                    try
                    {
                        val = (double)row[priceType.name];
                    }
                    catch
                    {

                        errors.addError( null, "Invalid Segment Price utility value", "Cannot read utility value for price type: " + priceType.name );
                        break;
                    }

                    string suQuery = "segment_id = " + segment_id + " AND price_type_id = " + priceType.id;

                    System.Data.DataRow[] rows = theDb.Data.segment_price_utility.Select( suQuery, "", System.Data.DataViewRowState.CurrentRows );

                    if( rows.Length == 1 )
                    {
                        MrktSimDBSchema.segment_price_utilityRow util_row = (MrktSimDBSchema.segment_price_utilityRow)rows[0];

                        util_row.util = val;
                    }
                    else
                    {
                        errors.addError( null, "Segment Utility Error", "Cannot find utility for price type: " + priceType.name );
                    }
                }
            }

            return errors;
        }		


    }
}
