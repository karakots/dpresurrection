//#define DEBUG_QUERY
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;

using MrktSimDb;
using Common.Utilities;
using MarketSimUtilities.MsTree;
using Common.Dialogs;

namespace MrktSimClient.Controls.MarketPlans
{
    /// <summary>
    /// MarketPlanControlFilter handles the formation of all necessary strings for filtering rows 
    /// in DataVlews for controls in the Market Plans form (MarketPlanControl2).
    /// </summary>
    public class MarketPlanControlFilter
    {
        private ModelDb.PlanType currentPlanType = ModelDb.PlanType.MarketPlan;
        private int selectedScenarioID;
        private int[] selectedMarketPlanComponentIDs;
        private int[] marketPlanIDs;
        private int[] productIDs;
        private int channelID;
        private DateTime startDisplayDate;
        private DateTime endDisplayDate;

        #region Properties

        public ModelDb.PlanType PlanType
        {
            set
            {
                currentPlanType = value;
            }

            get
            {
                return currentPlanType;
            }
        }

        /// <summary>
        /// Sets or gets the scenario ID.  Affects market plans filter only.
        /// </summary>
        public int SelectedScenarioID {
            get { return selectedScenarioID; }
            set { selectedScenarioID = value; }
        }

        /// <summary>
        /// Sets or gets the channel ID. Affects component-data filter only.
        /// </summary>
        public int ChannelID {
            get { return channelID; }
            set { channelID = value; }
        }

        /// <summary>
        /// Sets the list of selected products.
        /// </summary>
        public void SetProductIDs(ArrayList prods)
        {
            if ((prods == null) || (prods.Count == 0))
            {
                productIDs = null;
            }
            else
            {
                productIDs = new int[prods.Count];
                for (int i = 0; i < prods.Count; i++)
                {
                    DataRow row = (DataRow)prods[i];
                    productIDs[i] = (int)row["product_id"];
                }
            }
        }

        public int[] ProductIDs
        {
            get
            {
                return productIDs;
            }
        }
       

        /// <summary>
        /// Sets or gets the list of market plan IDs to include in the market plans filter.
        /// </summary>
        public int[] MarketPlanIDs 
        {
            get 
            { 
                return marketPlanIDs; 
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    marketPlanIDs = null;
                }
                else
                {
                    marketPlanIDs = value;
                }
            }
        }

        /// <summary>
        /// Sets or gets the list of market plan component IDs to include in the component data filter.
        /// </summary>
        public int[] SelectedMarketPlanComponentIDs {
            get { return selectedMarketPlanComponentIDs; }
            set {
                if( (value == null) || (value.Length == 0) ) {
                    selectedMarketPlanComponentIDs = null;
                }
                else {
                    selectedMarketPlanComponentIDs = value;
                }
            }
        }

        /// <summary>
        /// Sets or gets the list of market plan component IDs to include in the component data filter.
        /// </summary>
        public int[] SelectedProductIDs {
            get { return productIDs; }
            set {
                if( (value == null) || (value.Length == 0) ) {
                    productIDs = null;
                }
                else {
                    productIDs = value;
                }
            }
        }

        /// <summary>
        /// Sets or gets the display start date.  Affects the component-data filter only.
        /// </summary>
        public DateTime StartDate {
            get { return startDisplayDate; }
            set { startDisplayDate = value; }
        }

        /// <summary>
        /// Sets or gets the display end date.  Affects the component-data filter only.
        /// </summary>
        public DateTime EndDate {
            get { return endDisplayDate; }
            set { endDisplayDate = value; }
        }
        #endregion

        private DateTime defaultStartDate = new DateTime( 1960, 1, 1 );
        private DateTime defaultEndDate = new DateTime( 2160, 1, 1 ); 

        /// <summary>
        /// Creates a new market plan control-filter manager object.
        /// </summary>
        public MarketPlanControlFilter() {
            marketPlanIDs = null;
            selectedScenarioID = -1;
            selectedMarketPlanComponentIDs = null;
            channelID = -1;
            startDisplayDate = defaultStartDate;
            endDisplayDate = defaultEndDate;
        }

        public void Reset()
        {
            marketPlanIDs = null;
            selectedScenarioID = -1;
            selectedMarketPlanComponentIDs = null;
            channelID = -1;
        }

        /// <summary>
        /// Adds the specified market plan ID to the list of market plans shown in the market plan picker
        /// </summary>
        /// <param name="addedMarketPlanID"></param>
        public void AddMarketPlanID( int addedMarketPlanID ) {
            if( marketPlanIDs != null ) {
                int[] newIDs = new int[ marketPlanIDs.Length + 1 ];
                Array.Copy( marketPlanIDs, 0, newIDs, 0, marketPlanIDs.Length );
                newIDs[ marketPlanIDs.Length ] = addedMarketPlanID;
                marketPlanIDs = newIDs;
            }
            else {
                // this is the first one
                marketPlanIDs = new int[ 1 ];
                marketPlanIDs[ 0 ] = addedMarketPlanID;
            }
        }

        /// <summary>
        /// Removes the specified market plan ID from the list of market plans shown in the market plan picker
        /// </summary>
        /// <param name="addedMarketPlanID"></param>
        /// <remarks>Assumes the id will be in the list.</remarks>
        public void RemoveMarketPlanID( int removedMarketPlanID ) {
            if( marketPlanIDs.Length > 1 ) {
                int[] newIDs = new int[ marketPlanIDs.Length - 1 ];
                int indx = 0;
                for( int i = 0; i < marketPlanIDs.Length; i++ ) {
                    if( marketPlanIDs[ i ] != removedMarketPlanID ) {
                        newIDs[ indx ] = marketPlanIDs[ i ];
                        indx++;
                    }
                }
                marketPlanIDs = newIDs;
            }
            else {
                // removing the last one
                marketPlanIDs = null;
            }
        }

        /// <summary>
        /// A query for external factors pertinent to the currently selected market plan(s)
        /// </summary>
        /// <returns></returns>
        public string ExternalFactorsQuery( int topLevelMarketPlanID ) {
            ////bool nothingSelected = false;
            ////if( marketPlanIDs == null ) {
            ////    // nothing is selected
            ////    nothingSelected = true;
            ////    //return "market_plan_id = -1";
            ////    //return null;    
            ////}

            ////string query;
            ////if( nothingSelected == false ) {
            ////    query = "(";
            ////    for( int i = 0; i < marketPlanIDs.Length - 1; i++ ) {
            ////        query += String.Format( "{0} = {1} OR ", "market_plan_id", marketPlanIDs[ i ] );
            ////    }
            ////    query += String.Format( "{0} = {1})", "market_plan_id", marketPlanIDs[ marketPlanIDs.Length - 1 ] );
            ////}
            ////else {
            ////    query = "market_plan_id = -1";
            ////}
            string query = "market_plan_id = " + topLevelMarketPlanID;

            if( channelID != Database.AllID ) {
                query += String.Format( " AND channel_id = {0}", channelID );
            }

            // add the date range
            if( endDisplayDate != defaultEndDate ) {
                query += " AND start_date <= '" + endDisplayDate.ToShortDateString() + "'";
            }
            if( startDisplayDate != defaultStartDate ) {
                query += " AND end_date >= '" + startDisplayDate.ToShortDateString() + "'";
            }

#if DEBUG_QUERY
           Console.WriteLine( "Filter.ExternalFactorsQuery() returns:\r\n" + query );
#endif
            return query;
        }

        /// <summary>
        /// A query for one id of a particular type
        /// </summary>
        /// <param name="planType"></param>
        /// <returns></returns>
        public string MarketSingleComponentDataQuery( int id, ModelDb.PlanType planType ) {
            string query;

            query = "market_plan_id = " + id;


            if( planType == ModelDb.PlanType.Media ) {
                query += " AND (media_type = 'V' OR media_type = 'A')";
            }
            else if( planType == ModelDb.PlanType.Coupons ) {
                query += " AND (media_type = 'C' OR media_type = 'S')";
            }

            if( channelID != Database.AllID ) {
                query += String.Format( " AND channel_id = {0}", channelID );
            }

            // add the date range
            if( endDisplayDate != defaultEndDate ) {
                query += " AND start_date <= '" + endDisplayDate.ToShortDateString() + "'"; 
            }
            if( startDisplayDate != defaultStartDate ) {
                query += " AND end_date >= '" + startDisplayDate.ToShortDateString() + "'";
            }

            return query;
        }

        public string MarketPlanDataQuery()
        {
            return MarketPlanDataQuery(currentPlanType);
        }

        /// <summary>
        /// Returns the query string appropriate for getting the current data-grid data.
        /// </summary>
        /// <returns></returns>
        public string MarketPlanDataQuery(ModelDb.PlanType planType) {
            bool nothingSelected = false;
            if( selectedMarketPlanComponentIDs == null ) {
                // nothing is selected
                nothingSelected = true;
                //return "market_plan_id = -1";
                //return null;    
            }

            string query;
            if( nothingSelected == false ) {
                query = "(";
                for( int i = 0; i < selectedMarketPlanComponentIDs.Length - 1; i++ ) {
                    query += String.Format( "{0} = {1} OR ", "market_plan_id", selectedMarketPlanComponentIDs[ i ] );
                }
                query += String.Format( "{0} = {1})", "market_plan_id", selectedMarketPlanComponentIDs[ selectedMarketPlanComponentIDs.Length - 1 ] );
            }
            else {
                query = "market_plan_id = -1";
            }

            if( planType == ModelDb.PlanType.Media ) {
				query += " AND (media_type = 'V' OR media_type = 'A')";
			}
            else if( planType == ModelDb.PlanType.Coupons )
			{
				query += " AND (media_type = 'C' OR media_type = 'S')";
			}

            // we will just filter on the component ids
            // SSN 3/7/2007
            // add the list of products
            //if( productIDs != null ) {
            //    query = AddAndOrList( query, productIDs, "product_id" );
            //}

            // SSN try to use Database.AllID whenever possible or makes sense
            // hint: it makes sense when it really does refer to All items
            // add the channel ID, if it is set
            if( channelID != Database.AllID ) {
                query += String.Format( " AND channel_id = {0}", channelID );
            }

            // add the date range
            if( endDisplayDate != defaultEndDate ) {
                query += " AND start_date <= '" + endDisplayDate.ToShortDateString() + "'";
            }
            if( startDisplayDate != defaultStartDate ) {
                query += " AND end_date >= '" + startDisplayDate.ToShortDateString() + "'";
            }

#if DEBUG_QUERY
           Console.WriteLine( "Filter.MarketPlanDataQuery() returns:\r\n" + query );
#endif
            return query;
        }

        /// <summary>
        /// Appends a list of col_name=int_value pairs to a row filter string.  
        /// </summary>
        /// <param name="query">The starting row filter</param>
        /// <param name="values">Values to allow in the specified column.</param>
        /// <param name="colName">Name of the data coiumn</param>
        /// <returns>The row fiilter with the new values appended.</returns>
        private string AddAndOrList( string query, int[] values, string colName ) {
            if( values.Length == 1 ) {
                query += String.Format( " AND {0} = {1}", colName, values[ 0 ] );
            }
            else {
                query += " AND (";
                for( int i = 0; i < values.Length - 1; i++ ) {
                    query += String.Format( "{0} = {1} OR ", colName, values[ i ] );
                }
                query += String.Format( "{0} = {1})", colName, values[ values.Length - 1 ] );
            }
            return query;
        }
    }
}
