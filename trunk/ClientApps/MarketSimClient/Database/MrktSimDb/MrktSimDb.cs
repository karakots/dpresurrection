using System;
using System.Collections;

using System.Data;
using System.Data.OleDb;

namespace MrktSimDb
{

	

	/// <summary>
	/// Provides interface into the database
	/// Creating Simulations, Opening, Closing, Deleting
	/// and Updating
	/// exposes a strongly typed Dataset for use by applications
	/// </summary>
	/// 

	public class ModelDb : PCSModel
    {
        #region Types
  
        public enum NetworkType
        {
            TalkAnyTime = 0,
            PurchasedTriggered
        }

        public enum ProductEventType
        {
            UnitsPurchased = 0,
            Frequency,
            PriceSensitivity,
            FrequencyAndUnits
            
        }

        public enum ExternalDataType
        {
            Sales = 1,
            Awareness
        }

        public enum AttributeType
        {
            Standard = 0,
            NIMORestage = 1000 // NIMO only
        }
            

        public static DataTable market_plan_type = new DataTable("PlanType");
        public static DataTable network_type = new DataTable("NetWorkType");
        public static DataTable product_event_type = new DataTable("ProductEventType");
        public static DataTable external_data_type = new DataTable("ExternalDataType");
        public static DataTable attribute_type = new DataTable( "AttributeType" );

        private static void create_external_data_types()
        {
            external_data_type.Columns.Add("type", typeof(string));
            external_data_type.Columns.Add("descr", typeof(string));
            external_data_type.Columns.Add("sim_type", typeof(string));
            external_data_type.Columns.Add("average", typeof(bool));

            DataColumn idCol = external_data_type.Columns.Add("id", typeof(ExternalDataType));
            external_data_type.PrimaryKey = new DataColumn[] { idCol };

            DataRow row = null;

            row = external_data_type.NewRow();
            row["type"] = "real_sales";
            row["descr"] = "Real Sales";
            row["sim_type"] = "num_sku_bought";
            row["average"] = false;
            row["id"] = ExternalDataType.Sales;
            external_data_type.Rows.Add(row);

            row = external_data_type.NewRow();
            row["type"] = "real_awareness";
            row["descr"] = "Real Awareness";
            row["sim_type"] = "percent_aware_sku_cum";
            row["average"] = true;
            row["id"] = ExternalDataType.Awareness;
            external_data_type.Rows.Add(row);

            external_data_type.AcceptChanges();
        }

        private static void create_product_event_types()
        {
            product_event_type.Columns.Add("type", typeof(string));
            DataColumn idCol = product_event_type.Columns.Add("id", typeof(ProductEventType));
            product_event_type.PrimaryKey = new DataColumn[] { idCol };

            DataRow row = null;

            row = product_event_type.NewRow();
            row["type"] = "Units Purchased";
            row["id"] = ProductEventType.UnitsPurchased;
            product_event_type.Rows.Add(row);

            row = product_event_type.NewRow();
            row["type"] = "Frequency";
            row["id"] = ProductEventType.Frequency;
            product_event_type.Rows.Add(row);

            row = product_event_type.NewRow();
            row["type"] = "Price Sensitivity";
            row["id"] = ProductEventType.PriceSensitivity;
            product_event_type.Rows.Add(row);

            row = product_event_type.NewRow();
            row["type"] = "Units and Frequency";
            row["id"] = ProductEventType.FrequencyAndUnits;
            product_event_type.Rows.Add( row );

            product_event_type.AcceptChanges();
        }

        private static void create_network_types()
        {
            network_type.Columns.Add("name", typeof(string));
            DataColumn idCol = network_type.Columns.Add("id", typeof(NetworkType));
            network_type.PrimaryKey = new DataColumn[] { idCol };

            DataRow row = null;

            row = network_type.NewRow();
            row["name"] = "Purchased Triggered";
            row["id"] = NetworkType.PurchasedTriggered;
            network_type.Rows.Add(row);

            row = network_type.NewRow();
            row["name"] = "Talk AnyTime";
            row["id"] = NetworkType.TalkAnyTime;
            network_type.Rows.Add(row);

            network_type.AcceptChanges();
        }

        private static void create_plan_types()
        {
            market_plan_type.Columns.Add("type", typeof(string));
            DataColumn idCol = market_plan_type.Columns.Add("id", typeof(PlanType));
            market_plan_type.PrimaryKey = new DataColumn[] { idCol };

            DataRow row = null;

            row = market_plan_type.NewRow();
            row["type"] = "Market Plan";
            row["id"] = PlanType.MarketPlan;
            market_plan_type.Rows.Add(row);

            row = market_plan_type.NewRow();
            row["type"] = "Price";
            row["id"] = PlanType.Price;
            market_plan_type.Rows.Add(row);

            row = market_plan_type.NewRow();
            row["type"] = "Distribution";
            row["id"] = PlanType.Distribution;
            market_plan_type.Rows.Add(row);

            row = market_plan_type.NewRow();
            row["type"] = "Display";
            row["id"] = PlanType.Display;
            market_plan_type.Rows.Add(row);

            row = market_plan_type.NewRow();
            row["type"] = "Media";
            row["id"] = PlanType.Media;
            market_plan_type.Rows.Add(row);

            row = market_plan_type.NewRow();
            row["type"] = "Market Utility";
            row["id"] = PlanType.Market_Utility;
            market_plan_type.Rows.Add(row);

            row = market_plan_type.NewRow();
            row["type"] = "Coupons";
            row["id"] = PlanType.Coupons;
            market_plan_type.Rows.Add(row);

            row = market_plan_type.NewRow();
            row["type"] = "External Factors";
            row["id"] = PlanType.ProdEvent;
            market_plan_type.Rows.Add(row);

            market_plan_type.AcceptChanges();
        }

        private static void create_attribute_types() {
            attribute_type.Columns.Add( "type", typeof( string ) );
            DataColumn idCol = attribute_type.Columns.Add( "id", typeof( AttributeType ) );
            attribute_type.PrimaryKey = new DataColumn[] { idCol };

            DataRow row = null;

            row = attribute_type.NewRow();
            row[ "type" ] = "Standard";
            row[ "id" ] = AttributeType.Standard;
            attribute_type.Rows.Add( row );

            row = attribute_type.NewRow();
            row[ "type" ] = "NIMO Restage";
            row[ "id" ] = AttributeType.NIMORestage;
            attribute_type.Rows.Add( row );

            attribute_type.AcceptChanges();
        }

        static ModelDb()
        {
            create_plan_types();
            create_network_types();
            create_product_event_types();
            create_external_data_types();
            create_attribute_types();
        }

        #endregion

        #region Model Utilities

        // copy the marketplans from one model to another
        static public bool CopyMarketPlans(ModelDb fromDb, ModelDb toDb, MrktSimDBSchema.market_planRow[] plans)
        {
            // perform copy of market plans
            int numPlans = fromDb.Data.market_plan.Rows.Count;

            System.Collections.Hashtable prodMap = CreateMap(fromDb.Data.product, toDb.Data.product, "product_name", "product_id");
            System.Collections.Hashtable segMap = CreateMap(fromDb.Data.segment, toDb.Data.segment, "segment_name", "segment_id");
            System.Collections.Hashtable chanMap = CreateMap(fromDb.Data.channel, toDb.Data.channel, "channel_name", "channel_id");

            // error with mapping
            if (prodMap == null ||
                segMap == null ||
                chanMap == null)
            {
                return false;
            }

            System.Collections.Hashtable planMap = new System.Collections.Hashtable();

            // copy plans
            foreach (MrktSimDBSchema.market_planRow fromPlan in plans)
            {
                MrktSimDBSchema.market_planRow toPlan = toDb.CreateMarketPlan(fromPlan.name, (PlanType)fromPlan.type);

                planMap.Add(fromPlan.id, toPlan.id);

                // map the products and 

                int product_id = (int)prodMap[fromPlan.product_id];
                int segment_id = (int)prodMap[fromPlan.segment_id];
                int channel_id = (int)prodMap[fromPlan.channel_id];

                toPlan.product_id = product_id;
                toPlan.segment_id = segment_id;
                toPlan.channel_id = channel_id;
                toPlan.start_date = fromPlan.start_date;
                toPlan.end_date = fromPlan.end_date;

                toPlan.descr = fromPlan.descr;
                toPlan.type = fromPlan.type;
            }

            // copy tree - this is the heirarchy of plans
            foreach (MrktSimDBSchema.market_plan_treeRow fromTree in fromDb.Data.market_plan_tree)
            {
                if (!planMap.ContainsKey(fromTree.parent_id))
                    continue;

                // copy child over
                MrktSimDBSchema.market_planRow fromPlan = fromDb.Data.market_plan.FindByid(fromTree.child_id);

                // perform copy
                MrktSimDBSchema.market_planRow toPlan = toDb.CreateMarketPlan(fromPlan.name, (PlanType)fromPlan.type);

                planMap.Add(fromPlan.id, toPlan.id);

                // map the products and 

                int product_id = (int)prodMap[fromPlan.product_id];
                int segment_id = (int)prodMap[fromPlan.segment_id];
                int channel_id = (int)prodMap[fromPlan.channel_id];

                toPlan.product_id = product_id;
                toPlan.segment_id = segment_id;
                toPlan.channel_id = channel_id;
                toPlan.start_date = fromPlan.start_date;
                toPlan.end_date = fromPlan.end_date;

                toPlan.descr = fromPlan.descr;
                toPlan.type = fromPlan.type;

                int parent_id = (int)planMap[fromTree.parent_id];
                int child_id = (int)planMap[fromTree.child_id];

                toDb.CreatePlanRelation(parent_id, child_id);
            }

            // finally copy each of the component details

            // price
            foreach (MrktSimDBSchema.product_channelRow fromComp in fromDb.Data.product_channel)
            {
                if (!planMap.ContainsKey(fromComp.market_plan_id))
                    continue;

                int product_id = (int)prodMap[fromComp.product_id];
                int channel_id = (int)chanMap[fromComp.channel_id];

                // sets the current plan
                toDb.MarketPlanID = (int)planMap[fromComp.market_plan_id];
                MrktSimDBSchema.product_channelRow toComp = toDb.CreateProductChannel(product_id, channel_id);

                foreach (DataColumn col in fromDb.Data.product_channel.Columns)
                {
                    // skip prod chan

                    if (col.ColumnName == "record_id" ||
                        col.ColumnName == "model_id" ||
                        col.ColumnName == "product_id" ||
                        col.ColumnName == "channel_id" ||
                        col.ColumnName == "market_plan_id" ||
                        col.ReadOnly)
                        continue;

                    toComp[col.ColumnName] = fromComp[col];
                }
            }

            // distribution
            foreach (MrktSimDBSchema.distributionRow fromComp in fromDb.Data.distribution)
            {
                if (!planMap.ContainsKey(fromComp.market_plan_id))
                    continue;

                int product_id = (int)prodMap[fromComp.product_id];
                int channel_id = (int)chanMap[fromComp.channel_id];

                // sets the current plan
                toDb.MarketPlanID = (int)planMap[fromComp.market_plan_id];
                MrktSimDBSchema.distributionRow toComp = toDb.CreateDistribution(product_id, channel_id);

                foreach (DataColumn col in fromDb.Data.distribution.Columns)
                {
                    // skip prod chan

                    if (col.ColumnName == "record_id" ||
                        col.ColumnName == "model_id" ||
                        col.ColumnName == "product_id" ||
                        col.ColumnName == "channel_id" ||
                        col.ColumnName == "market_plan_id" ||
                        col.ReadOnly)
                        continue;

                    toComp[col.ColumnName] = fromComp[col];
                }
            }

            // display
            foreach (MrktSimDBSchema.displayRow fromComp in fromDb.Data.display)
            {
                if (!planMap.ContainsKey(fromComp.market_plan_id))
                    continue;

                int product_id = (int)prodMap[fromComp.product_id];
                int channel_id = (int)chanMap[fromComp.channel_id];

                // sets the current plan
                toDb.MarketPlanID = (int)planMap[fromComp.market_plan_id];
                MrktSimDBSchema.displayRow toComp = toDb.CreateDisplay(product_id, channel_id);

                foreach (DataColumn col in fromDb.Data.display.Columns)
                {
                    // skip prod chan seg

                    if (col.ColumnName == "record_id" ||
                        col.ColumnName == "model_id" ||
                        col.ColumnName == "product_id" ||
                        col.ColumnName == "segment_id" ||
                        col.ColumnName == "channel_id" ||
                        col.ColumnName == "market_plan_id" ||
                        col.ReadOnly)
                        continue;

                    toComp[col.ColumnName] = fromComp[col];
                }
            }

            // mass media
            foreach (MrktSimDBSchema.mass_mediaRow fromComp in fromDb.Data.mass_media)
            {
                if (!planMap.ContainsKey(fromComp.market_plan_id))
                    continue;

                int product_id = (int)prodMap[fromComp.product_id];
                int segment_id = (int)segMap[fromComp.segment_id];
                int channel_id = (int)chanMap[fromComp.channel_id];

                // sets the current plan
                toDb.MarketPlanID = (int)planMap[fromComp.market_plan_id];

                MrktSimDBSchema.mass_mediaRow toComp = toDb.CreateMassMedia(product_id, segment_id, channel_id);

                foreach (DataColumn col in fromDb.Data.mass_media.Columns)
                {
                    // skip prod chan seg

                    if (col.ColumnName == "record_id" ||
                        col.ColumnName == "model_id" ||
                        col.ColumnName == "product_id" ||
                        col.ColumnName == "segment_id" ||
                        col.ColumnName == "channel_id" ||
                        col.ColumnName == "market_plan_id" ||
                        col.ReadOnly)
                        continue;

                    toComp[col.ColumnName] = fromComp[col];
                }
            }

            // really not a market plan
            // external events

            //			foreach(MrktSimDBSchema.product_eventRow fromComp in fromDb.Data.product_event)
            //			{
            //				if (!planMap.ContainsKey(fromComp.market_plan_id))
            //					continue;
            //
            //				int product_id = (int) prodMap[fromComp.product_id];
            //				int segment_id = (int) segMap[fromComp.segment_id];
            //				int channel_id = (int) chanMap[fromComp.channel_id];
            //
            //				// sets the current plan
            //				toDb.MarketPlanID = (int) planMap[fromComp.market_plan_id];
            //
            //				MrktSimDBSchema.product_eventRow toComp = toDb.CreateProductEvent(product_id, segment_id, channel_id);
            //
            //				foreach(DataColumn col in fromDb.Data.product_event.Columns)
            //				{
            //					// skip prod chan seg
            //
            //					if (col.ColumnName == "product_id" ||
            //						col.ColumnName == "segment_id" ||
            //						col.ColumnName == "channel_id" ||
            //						col.ColumnName == "market_plan_id")
            //						continue;
            //
            //					toComp[col] = fromComp[col];
            //				}
            //			}

            return true;
        }


        private static System.Collections.Hashtable CreateMap(DataTable fromTable, DataTable toTable, string colKey, string val)
        {
            System.Collections.Hashtable map = new System.Collections.Hashtable();

            foreach (DataRow fromRow in fromTable.Rows)
            {
                // find proper item in toTable
                object key = fromRow[colKey];

                bool found = false;

                foreach (DataRow toRow in toTable.Rows)
                {
                    if (fromRow[colKey].ToString() == toRow[colKey].ToString())
                    {
                        map.Add(fromRow[val], toRow[val]);
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return null;
            }

            return map;
        }

#region Data Transfer Tools
        public DataTable ExtractComponentData( int scenarioID, int productID, int channelID, PlanType planType, int productType,
                                                                    DataColumn[] columns, DateTime start, DateTime end ) {
            ArrayList dummy;
            return ExtractComponentData( scenarioID, productID, channelID, planType, productType, columns, start, end, out dummy, false );
        }

        public ArrayList FindPlansFor( int destScenarioID, int productID, int channelID, PlanType planType, int productType, DateTime start, DateTime end ) {
            ArrayList plans = new ArrayList();
            ArrayList plansLists;
            ExtractComponentData( destScenarioID, productID, channelID, planType, productType, null, start, end, out plansLists, true );
            foreach( MrktSimDBSchema.market_planRow[] planRowsArray in plansLists ) {
                foreach( MrktSimDBSchema.market_planRow plan in planRowsArray ) {
                    plans.Add( plan );
                }
            }
            return plans;
        }

        #region Data Transfer Extraction
        private DataTable ExtractComponentData( int scenarioID, int productID, int channelID, PlanType planType, int productType,
            DataColumn[] columns, DateTime start, DateTime end, out ArrayList planItems, bool getComponentsOnly ) {
            planItems = new ArrayList();

            DataTable outTable = new DataTable( "extracted_data" );

            MrktSimDBSchema.scenario_market_planRow[] allPlans =
                (MrktSimDBSchema.scenario_market_planRow[]) Data.scenario_market_plan.Select( "scenario_id = " + scenarioID.ToString() );
            ArrayList planComps = new ArrayList();
            int totalComps = 0;
            for( int i = 0; i < allPlans.Length; i++ ) {
                string planQuery = String.Format( "parent_id = {0}", allPlans[ i ].market_plan_id );
                //Console.WriteLine( "Plan Query: " + planQuery );
                MrktSimDBSchema.market_plan_treeRow[] thisPlanComps = (MrktSimDBSchema.market_plan_treeRow[])Data.market_plan_tree.Select( planQuery );
                totalComps += thisPlanComps.Length;
                planComps.Add( thisPlanComps );
            }
            if( totalComps == 0 ) {
                return outTable;
            }

            //for best performance, pre-select possible components
            string candidateQuery = String.Format( "type = {0}", (byte)planType );
            if( productID != ModelDb.AllID ) {
                candidateQuery += String.Format( " AND product_id = {0}", productID );
            }
            if( channelID != ModelDb.AllID ) {
                candidateQuery += String.Format( " AND (channel_id = {0} OR channel_id = -1)", channelID );  //need to handle -1 since Dentsu components don't have channel set
                //candidateQuery += String.Format( " AND (channel_id = {0})", channelID );
            }
            candidateQuery += String.Format( " AND end_date >= '{0}' AND start_date <= '{1}'", start.ToShortDateString(), end.ToShortDateString() );

            MrktSimDBSchema.market_planDataTable candidateComponents = new MrktSimDBSchema.market_planDataTable();
            MrktSimDBSchema.market_planRow[] candidateComponentRows = (MrktSimDBSchema.market_planRow[])Data.market_plan.Select( candidateQuery );
            foreach( MrktSimDBSchema.market_planRow cRow in candidateComponentRows ) {
                DataRow newRow = candidateComponents.NewRow();
                foreach( DataColumn col in candidateComponents.Columns ) {
                    newRow[ col.ColumnName ] = cRow[ col.ColumnName ];
                }
                if( cRow.channel_id == -1 ) {
                    // perhaps this component isn't really an all-channels item - verify in the data
                    int realChannelID = GetChannelIDFromData( cRow );
                    if( realChannelID != -1 && realChannelID != channelID ) {
                        // not a good candidate after all
                        continue;
                    }
                    newRow[ "channel_id" ] = realChannelID;
                }
                candidateComponents.Rows.Add( newRow );
            }

            int totalItems = 0;
            for( int i = 0; i < planComps.Count; i++ ) {
                MrktSimDBSchema.market_plan_treeRow[] thisPlanComps = (MrktSimDBSchema.market_plan_treeRow[])planComps[ i ];
                if( thisPlanComps.Length == 0 ) {
                    continue;
                }

                string compQuery = "";
                foreach( MrktSimDBSchema.market_plan_treeRow comp in thisPlanComps ) {
                    compQuery += String.Format( "id = {0} OR ", comp.child_id );
                }
                compQuery = compQuery.Substring( 0, compQuery.Length - 4 );     // strip the final " or "

                MrktSimDBSchema.market_planRow[] allItems = (MrktSimDBSchema.market_planRow[])candidateComponents.Select( compQuery );
                totalItems += allItems.Length;
                planItems.Add( allItems );
            }
            if( totalItems == 0 ) {
                return outTable;
            }

            if( getComponentsOnly ) {     // we are done if we are just checking for suitable components
                return null;
            }

            DataTable sourceTable = null;
            switch( planType ) {
                case PlanType.Display:
                    sourceTable = Data.display;
                    break;
                case PlanType.Distribution:
                    sourceTable = Data.distribution;
                    break;
                case PlanType.Price:
                    sourceTable = Data.product_channel;
                    break;
                case PlanType.Media:
                    sourceTable = Data.mass_media;
                    break;
            }

            if( columns == null ) {
                columns = new DataColumn[ sourceTable.Columns.Count ];
                for( int c = 0; c < sourceTable.Columns.Count; c++ ) {
                    DataColumn scol = sourceTable.Columns[ c ];
                    columns[ c ] = scol;
                    DataColumn newCol = new DataColumn( scol.ColumnName, scol.DataType );
                    outTable.Columns.Add( newCol );
                }
            }
            else {
                foreach( DataColumn col in columns ) {
                    DataColumn newCol = new DataColumn( col.ColumnName, col.DataType );
                    outTable.Columns.Add( newCol );
                }
            }
          
            for( int i = 0; i < planItems.Count; i++ ) {
                MrktSimDBSchema.market_planRow[] thisPlanItems = (MrktSimDBSchema.market_planRow[])planItems[ i ];
                if( thisPlanItems.Length == 0 ) {
                    continue;
                }
                string dataQuery = "(";
                foreach( MrktSimDBSchema.market_planRow item in thisPlanItems ) {
                    dataQuery += String.Format( "market_plan_id = {0} OR ", item.id );
                }
                dataQuery = dataQuery.Substring( 0, dataQuery.Length - 4 );     // strip the final " or "
                dataQuery += String.Format( ") AND end_date >= '{0}' AND start_date <= '{1}'", start.ToShortDateString(), end.ToShortDateString() );
                if( channelID != ModelDb.AllID ) {
                    dataQuery += String.Format( " AND (channel_id = {0} OR channel_id = -1)", channelID );
                }

                DataRow[] itemData = sourceTable.Select( dataQuery, "start_date" );
                //Console.WriteLine( "Data Query:  \"{0}\", count = {1}", dataQuery, itemData.Length );

                // now we have all the components of the given type, productID, and channelID for the giren scenarioID

                for( int r= 0; r < itemData.Length; r++ ) {
                    DataRow newRow = outTable.NewRow();
                    foreach( DataColumn col in columns ) {
                        newRow[ col.ColumnName ] = itemData[ r ][ col.ColumnName ];
                    }
                    //Console.WriteLine( "ADD ROW!" );
                    outTable.Rows.Add( newRow );
                }
            }
            return outTable;
        }

        private int GetChannelIDFromData( MrktSimDBSchema.market_planRow cRow ) {
            // find out the real channel for a plan claiming to be "all channels"
            DataTable srcTable = null;
            if( cRow.type == (byte)ModelDb.PlanType.Display ) {
                srcTable = Data.display;
            }
            else if( cRow.type == (byte)ModelDb.PlanType.Distribution ) {
                srcTable = Data.distribution;
            }
            else if( cRow.type == (byte)ModelDb.PlanType.Media ) {
                srcTable = Data.mass_media;
            }
            else if( cRow.type == (byte)ModelDb.PlanType.Price ) {
                srcTable = Data.product_channel;
            }

            //??? how do we select just the first row of data????
            string dataQuery = String.Format( "market_plan_id = {0} AND start_date >= '{1}' AND end_date <= '{2}'", 
                cRow.id, cRow.start_date.ToShortDateString(), cRow.end_date.ToShortDateString() );
            DataRow[] dataRows = srcTable.Select( dataQuery );
            if( dataRows.Length > 0 ) {
                int channelId = (int)dataRows[ 0 ][ "channel_id" ];
                return channelId;
            }
            else {
                return cRow.id;
            }
        }
        #endregion

        #region Data Transfer Methods
        public int TransferComponentData( int sourceSenarioID, int destScenarioID, PlanType planType, int productType, DataColumn[] columns, DateTime start, DateTime end ) {
            if( sourceSenarioID == destScenarioID || sourceSenarioID == ModelDb.AllID || destScenarioID == ModelDb.AllID ) {
                return 0;
            }

            // get the list of channels
            MrktSimDBSchema.channelRow[] channels = (MrktSimDBSchema.channelRow[])Data.channel.Select();

            // get the list of products
            string levelQuery = "product_type = " + productType.ToString();
            MrktSimDBSchema.productRow[] products = (MrktSimDBSchema.productRow[])Data.product.Select( levelQuery );

            DateTime startTime = DateTime.Now;

            int totalItems = (products.Length - 1) * (channels.Length - 1);
            int itemNum = 0;
            // loop over all channels and products
            int rowsExtracted = 0;
            int rowsChanged = 0;
            for( int p = 0; p < products.Length; p++ ) {
                if( products[ p ].product_id == ModelDb.AllID ) {
                    continue;
                }
                for( int c = 0; c < channels.Length; c++ ) {
                    if( channels[ c ].channel_id == ModelDb.AllID ) {
                        continue;
                    }

                    itemNum += 1;
                    Console.WriteLine( "Transfer: Item {0} of {1}: {2} - {3}", itemNum, totalItems, products[ p ].product_name, channels[ c ].channel_name );

                    // see if there is any suitable destination plan
                    ArrayList destPlans = FindPlansFor( destScenarioID, products[ p ].product_id, channels[ c ].channel_id, planType, productType, start, end );
                    if( destPlans.Count == 0 ) {
                        continue;
                    }
                    ////Console.WriteLine( " X Checking: Getting components for \"{0}\", {1}  ... count = {2}", products[ p ].product_name, channels[ c ].channel_name, destPlans.Count );

                    ////Console.WriteLine( " X Transfer: Extracting data for \"{0}\", {1}", products[ p ].product_name, channels[ c ].channel_name );
                    DataTable updateValues = ExtractComponentData( sourceSenarioID, products[ p ].product_id, channels[ c ].channel_id, planType, productType, columns, start, end );
                    ////Console.WriteLine( "  ...extracted {0} rows", updateValues.Rows.Count );
                    rowsExtracted += updateValues.Rows.Count;

                    // transfer the data values
                    rowsChanged += TransferComponentDataValues( updateValues, destPlans, start, end );
                }
            }
            TimeSpan et2 = DateTime.Now - startTime;
            Console.WriteLine( "\r\n*** Transfer Complete: Extracted {0} rows;  Changed {1} rows.  Time = {2:f1} sec", rowsExtracted, rowsChanged, et2.TotalMilliseconds / 10000.0 );
            return rowsChanged;
        }

        public int TransferComponentDataValues( DataTable newValues, ArrayList destPlans, DateTime start, DateTime end ) {
            int transferCount = 0;
            foreach( MrktSimDBSchema.market_planRow destPlan in destPlans ) {
                DataTable srcTable = null;
                if( destPlan.type == (byte)ModelDb.PlanType.Display ) {
                    srcTable = Data.display;
                }
                else if( destPlan.type == (byte)ModelDb.PlanType.Distribution ) {
                    srcTable = Data.distribution;
                }
                else if( destPlan.type == (byte)ModelDb.PlanType.Media ) {
                    srcTable = Data.mass_media;
                }
                else if( destPlan.type == (byte)ModelDb.PlanType.Price ) {
                    srcTable = Data.product_channel;
                }

                string dataQuery = String.Format( "market_plan_id = {0} AND start_date <= '{1}' AND end_date >= '{2}'", 
                    destPlan.id, end.ToShortDateString(), start.ToShortDateString() );

                // get the data rows in the time window
                DataRow[] data = srcTable.Select( dataQuery );

                // set the new values in place
                for( int r = 0; r < data.Length; r++ ) {
                    DateTime rowStart = (DateTime)data[ r ][ "start_date" ];
                    DateTime rowEnd = (DateTime)data[ r ][ "end_date" ];

                    foreach( DataRow newValsRow in newValues.Rows ) {
                        DateTime valStart = (DateTime)newValsRow[ "start_date" ];
                        DateTime valEnd = (DateTime)newValsRow[ "end_date" ];

                        //change the value if there is ANY overlap in dates!
                        if( valEnd >= rowStart && valStart <= rowStart ) {
                            foreach( DataColumn col in newValues.Columns ) {
                                if( col.ColumnName == "start_date" || col.ColumnName == "end_date" ) {
                                    continue;
                                }

                                // change the data value!
                                data[ r ][ col.ColumnName ] = newValsRow[ col.ColumnName ];
                            }
                            transferCount += 1;
                        }
                    }
                }

            }
            return transferCount;
        }
        #endregion

        #region Replicate Data Methods
        private ArrayList componentsReplicatedAlready;

        public int ReplicateComponentData( int scenarioID, PlanType planType, int productType, DateTime start, DateTime end, int nMonthsPattern ) {
            if( scenarioID == ModelDb.AllID ) {
                return 0;
            }

            DataColumn[] columns = new DataColumn[ 4 ];
            columns[ 0 ] = new DataColumn( "message_awareness_probability", typeof( double ) );
            columns[ 1 ] = new DataColumn( "message_persuation_probability", typeof( double ) );
            columns[ 2 ] = new DataColumn( "start_date", typeof( DateTime ) );
            columns[ 3 ] = new DataColumn( "end_date", typeof( DateTime ) );

            // get the list of channels
            MrktSimDBSchema.channelRow[] channels = (MrktSimDBSchema.channelRow[])Data.channel.Select();

            // get the list of products
            string levelQuery = "product_type = " + productType.ToString();
            MrktSimDBSchema.productRow[] products = (MrktSimDBSchema.productRow[])Data.product.Select( levelQuery );

            int totalItems = (products.Length - 1) * (channels.Length - 1);
            int itemNum = 0;
            componentsReplicatedAlready = new ArrayList();
            // loop over all channels and products
            int rowsExtracted = 0;
            int rowsChanged = 0;
            for( int p = 0; p < products.Length; p++ ) {
                if( products[ p ].product_id == ModelDb.AllID ) {
                    continue;
                }
                for( int c = 0; c < channels.Length; c++ ) {
                    if( channels[ c ].channel_id == ModelDb.AllID ) {
                        continue;
                    }

                    itemNum += 1;
                    Console.WriteLine( "Replicate: Item {0} of {1}: {2} - {3}", itemNum, totalItems, products[ p ].product_name, channels[ c ].channel_name );

                    // get the plans that end on the appropriate date
                    DateTime endExistingData = start.AddDays( -1 );

                    ArrayList repPlans = FindPlansFor( scenarioID, products[ p ].product_id, channels[ c ].channel_id, planType, productType, endExistingData, endExistingData );
                    if( repPlans.Count == 0 ) {
                        continue;
                    }
                    //Console.WriteLine( " Checking: Getting components for \"{0}\", {1}  ... count = {2}", products[ p ].product_name, channels[ c ].channel_name, repPlans.Count );

                    DateTime repSourceEnd = start.AddDays( -1 );
                    DateTime repSourceStart = start;
                    if( nMonthsPattern > 0 ) {
                        repSourceStart = start.AddMonths( -nMonthsPattern );
                    }
                    else {
                        repSourceStart = repSourceEnd.AddDays( -1 );
                    }

                    //Console.WriteLine( " Replicate: Extracting data for \"{0}\", {1}", products[ p ].product_name, channels[ c ].channel_name );
                    DataTable replicateValues = ExtractComponentData( scenarioID, products[ p ].product_id, channels[ c ].channel_id, planType, productType, columns,
                        repSourceStart, repSourceEnd );
                    //Console.WriteLine( "  ...extracted {0} rows", replicateValues.Rows.Count );
                    rowsExtracted += replicateValues.Rows.Count;

                    //Console.WriteLine( " Replicating data for \"{0}\", {1}", products[ p ].product_name, channels[ c ].channel_name );
                    // replicate the data values
                    rowsChanged += ReplicateComponentDataValues( replicateValues, repPlans, start, end, nMonthsPattern );
                }
            }
            Console.WriteLine( "\r\n***Replication Compkete:  Extracted {0} rows;  Replicated {1} rows", rowsExtracted, rowsChanged );
            return rowsChanged;
        }

        public int ReplicateComponentDataValues( DataTable newValues, ArrayList repPlans, DateTime start, DateTime end, int nMonthsPattern ) {
            int repCount = 0;

            foreach( MrktSimDBSchema.market_planRow destPlan in repPlans ) {
                int planIDToReplicate = destPlan.id;
                bool doneAlready = false;
                foreach( int planAlreadyDone in componentsReplicatedAlready ) {
                    if( planAlreadyDone == planIDToReplicate ) {
                        doneAlready = true;
                        break;
                    }
                }
                if( doneAlready ) {
                    continue;
                }
                else {
                    componentsReplicatedAlready.Add( planIDToReplicate );
                }

                DataTable srcTable = null;
                if( destPlan.type == (byte)ModelDb.PlanType.Display ) {
                    srcTable = Data.display;
                }
                else if( destPlan.type == (byte)ModelDb.PlanType.Distribution ) {
                    srcTable = Data.distribution;
                }
                else if( destPlan.type == (byte)ModelDb.PlanType.Media ) {
                    srcTable = Data.mass_media;
                }
                else if( destPlan.type == (byte)ModelDb.PlanType.Price ) {
                    srcTable = Data.product_channel;
                }

                string destDataQuery = String.Format( "market_plan_id = {0} AND start_date >= '{1}' AND start_date <= '{2}'",
                    destPlan.id, start.ToShortDateString(), end.ToShortDateString() );

                // get the data rows in the time window
                DataRow[] destData = srcTable.Select( destDataQuery );

                int nRepeats = 0;
                int patternRow = 0;
                // set the new values in place
                for( int r = 0; r < destData.Length; r++ ) {
                    DateTime rowStart = (DateTime)destData[ r ][ "start_date" ];
                    DateTime rowEnd = (DateTime)destData[ r ][ "end_date" ];

                    DateTime valStart = rowStart;
                    DateTime valEnd = rowEnd;

                    do {
                        DataRow patRow = newValues.Rows[ patternRow ];

                        valStart = (DateTime)patRow[ "start_date" ];
                        valEnd = (DateTime)patRow[ "end_date" ];
                        valStart = valStart.AddMonths( (nRepeats + 1) * nMonthsPattern );
                        valEnd = valEnd.AddMonths( (nRepeats + 1) * nMonthsPattern );

                        bool thisIsTheValue = false;
                        if( valStart >= rowStart ) {
                            thisIsTheValue = true;
                        }
                        if( thisIsTheValue ) {
                            // set the new values
                            foreach( DataColumn col in newValues.Columns ) {
                                if( col.ColumnName == "start_date" || col.ColumnName == "end_date" ) {
                                    continue;
                                }

                                // change the data value!
                                destData[ r ][ col.ColumnName ] = patRow[ col.ColumnName ];
                            }
                            repCount += 1;
                            break;
                        }
                        patternRow += 1;
                        if( patternRow >= newValues.Rows.Count ) {
                            patternRow = 0;
                            nRepeats += 1;
                        }
                    }
                    while( true );
                }

            }
            return repCount;
        }
        #endregion

        #region Componenet Extension Methods
        private ArrayList componentsExtendedAlready;

        public int ExtendComponentData( int scenarioID, PlanType planType, int productType, DateTime start, DateTime end, int nMonthsPattern ) {
            if( scenarioID == ModelDb.AllID ) {
                return 0;
            }

            // get the list of channels
            MrktSimDBSchema.channelRow[] channels = (MrktSimDBSchema.channelRow[])Data.channel.Select();

            // get the list of products
            string levelQuery = "product_type = " + productType.ToString();
            MrktSimDBSchema.productRow[] products = (MrktSimDBSchema.productRow[])Data.product.Select( levelQuery );

            int totalItems = (products.Length - 1) * (channels.Length - 1);
            int itemNum = 0;
            componentsExtendedAlready = new ArrayList();

            // loop over all channels and products
            int rowsExtracted = 0;
            int rowsChanged = 0;
            for( int p = 0; p < products.Length; p++ ) {
                if( products[ p ].product_id == ModelDb.AllID ) {
                    continue;
                }
                for( int c = 0; c < channels.Length; c++ ) {
                    if( channels[ c ].channel_id == ModelDb.AllID ) {
                        continue;
                    }

                    itemNum += 1;
                    Console.WriteLine( "Extend: Item {0} of {1}: {2} - {3}", itemNum, totalItems, products[ p ].product_name, channels[ c ].channel_name );

                    // get the plans that end on the appropriate date (in the final month before the extension)
                    DateTime endExistingData = start.AddDays( -1 );
                    DateTime startExistingData = start.AddMonths( -1 );

                    ArrayList repPlans = FindPlansFor( scenarioID, products[ p ].product_id, channels[ c ].channel_id, planType, productType, startExistingData, endExistingData );
                    if( repPlans.Count == 0 ) {
                        continue;
                    }
                    //Console.WriteLine( " Checking: Getting components for \"{0}\", {1}  ... count = {2}", products[ p ].product_name, channels[ c ].channel_name, repPlans.Count );

                    DateTime repSourceEnd = start.AddDays( -1 );
                    DateTime repSourceStart = start;
                    if( nMonthsPattern > 0 ) {
                        repSourceStart = start.AddMonths( -nMonthsPattern );
                    }
                    else {
                        repSourceStart = repSourceEnd.AddDays( -1 );
                    }

                    //Console.WriteLine( " Replicate: Extracting data for \"{0}\", {1}", products[ p ].product_name, channels[ c ].channel_name );
                    //Console.WriteLine( "                Extract Range: {0} to {1}", repSourceStart.ToShortDateString(), repSourceEnd.ToShortDateString() );
                    DataTable replicateValues = ExtractComponentData( scenarioID, products[ p ].product_id, channels[ c ].channel_id, planType, productType, null,
                        repSourceStart, repSourceEnd );
                    //Console.WriteLine( "  ...extracted {0} rows", replicateValues.Rows.Count );
                    rowsExtracted += replicateValues.Rows.Count;

                    //Console.WriteLine( " Extending data for \"{0}\", {1}", products[ p ].product_name, channels[ c ].channel_name );
                    // replicate the data values
                    rowsChanged += ExtendComponentDataValues( replicateValues, repPlans, start, end, nMonthsPattern );
                }
            }
            Console.WriteLine( "\r\n***Plan Extension Complete: Extracted {0} rows;  Added {1} rows", rowsExtracted, rowsChanged );
            return rowsChanged;
        }

        public int ExtendComponentDataValues( DataTable newValues, ArrayList repPlans, DateTime start, DateTime end, int nMonthsPattern ) {
            int repCount = 0;

            DataRow firstRow = newValues.Rows[ 0 ];
            DataRow lastRow = newValues.Rows[ newValues.Rows.Count - 1 ];
            DateTime firstDate = (DateTime)firstRow[ "start_date" ];
            DateTime lastDate = (DateTime)lastRow[ "end_date" ];
            DateTime chkDate = lastDate.AddMonths( -nMonthsPattern ).AddDays( 1 );
            if( firstDate != chkDate ) {
                newValues.Rows[ 0 ][ "start_date" ] = chkDate;     // make sure the extended pieces fit together
            }

            foreach( MrktSimDBSchema.market_planRow destPlan in repPlans ) {
                int planIDToExtend = destPlan.id;
                //Console.WriteLine( "ExtendComponentDataValues() extending plan: \"{0}\"  (id = {1}, channel_id = {2})", destPlan.name, destPlan.id, destPlan.channel_id );
                bool doneAlready = false;
                foreach( int planAlreadyDone in componentsExtendedAlready ) {

                    if( planAlreadyDone == planIDToExtend ) {
                        doneAlready = true;
                        //Console.WriteLine( "SKIP" );
                        break;
                    }
                }
                if( doneAlready ) {
                    continue;
                }
                else {
                    componentsExtendedAlready.Add( planIDToExtend );
                }

                DataTable srcTable = null;
                if( destPlan.type == (byte)ModelDb.PlanType.Display ) {
                    srcTable = Data.display;
                }
                else if( destPlan.type == (byte)ModelDb.PlanType.Distribution ) {
                    srcTable = Data.distribution;
                }
                else if( destPlan.type == (byte)ModelDb.PlanType.Media ) {
                    srcTable = Data.mass_media;
                }
                else if( destPlan.type == (byte)ModelDb.PlanType.Price ) {
                    srcTable = Data.product_channel;
                }
                if( srcTable == null ) {
                    continue;
                }

                int patternRow = 0;
                int nRepeats = 0;
                do {
                    DataRow row = newValues.Rows[ patternRow ];
                    DataRow newRow = srcTable.NewRow();
                    ////Console.WriteLine( "  Replication -- Created new row.  Pattern row index = {0} of {1}", patternRow, newValues.Rows.Count );
                    DateTime st = DateTime.MinValue;
                    foreach( DataColumn col in srcTable.Columns ) {

                        if( col.ColumnName == "record_id" ) {
                            continue;
                        }

                        // copy the data value
                        newRow[ col.ColumnName ] = row[ col.ColumnName ];

                        if( col.ColumnName == "start_date" ) {

                            st = (DateTime)row[ col.ColumnName ];      // get existing date

                            // check for leap year
                            if( (st.Year % 4) == 0 && st.Month == 2 && st.Day == 29 ) {
                                st = new DateTime( st.Year, 2, 28 );
                            }

                            st = st.AddMonths( nMonthsPattern * (nRepeats + 1) );   // change the time

                            if( (st.Year % 4) == 0 && st.Month == 2 && st.Day == 28 ) {
                                st = new DateTime( st.Year, 2, 29 );
                            }
                            ////Console.WriteLine( " New row start = {0}", st.ToShortDateString() );

                            newRow[ col.ColumnName ] = st;           // set new date
                        }
                        if( col.ColumnName == "end_date" ) {
                            DateTime en = (DateTime)row[ col.ColumnName ];           // get existing date

                            // check for leap year
                            if( (en.Year % 4) == 0 && en.Month == 2 && en.Day == 29 ) {
                                en = new DateTime( en.Year, 2, 28 );
                            }

                            en = en.AddMonths( nMonthsPattern * (nRepeats + 1) );     // change the time

                            if( (en.Year % 4) == 0 && en.Month == 2 && en.Day == 28 ) {
                                en = new DateTime( en.Year, 2, 29 );
                            }
                            ////Console.WriteLine( " New row end = {0}", en.ToShortDateString() );

                            newRow[ col.ColumnName ] = en;                                   // set new date
                        }
                    } // end of loop over columns

                    if( st > end ) {
                        break;
                    }
                    srcTable.Rows.Add( newRow );
                    repCount += 1;

                    patternRow += 1;
                    if( patternRow >= newValues.Rows.Count ) {
                        patternRow = 0;
                        nRepeats += 1;
                    }
                } while( true );

            }
            return repCount;
        }
        #endregion

        #endregion

        #endregion

        #region Model Open/Close

        public bool SimulationRunning() {
            return this.ModelHasSimulationRunning( this.ModelID );
        }
		

		/// <summary>
		/// opens the base nodel
		/// </summary>
		/// <returns></returns>
		///
		public bool Open(out string who)
		{
            who = null;

			// checks model access
			if (this.Connection == null)
				return false;

			if (ModelID == AllID)
				return false;

			// already opened for writing
			if (ReadOnly)
				return false;

			if (ModelLocked(this.ModelID, out who))
			{
				ReadOnly = true;
			}
			else {

                ReadOnly = false;

                // lock the model if we are going to be editing it
                LockModel( this.ModelID, true );	
			}

			return true;
		}

		public void Close()
		{
			// no need to close if read only
			if (ReadOnly)
				return;

			// unlock the model if we are done editing it
			LockModel(ModelID, false);
		}

        #endregion

        #region Database abtraction
        protected override bool TableInSchema(DataTable table)
        {
            if (base.TableInSchema(table))
                return true;

            if (table == Data.product_type ||
                table == Data.product ||
                table == Data.product_tree ||
                table == Data.pack_size_dist ||
                table == Data.segment ||
                table == Data.channel ||

                // price_type
                table == Data.price_type ||
                table == Data.segment_price_utility ||

                 table == Data.product_channel_size ||
                table == Data.segment_channel ||
                table == Data.network_parameter ||
                table == Data.segment_network ||
                table == Data.product_attribute ||
                table == Data.product_attribute_value ||
                table == Data.consumer_preference ||
                table == Data.product_matrix ||

                // tasks (not used currently)
                table == Data.task ||
                table == Data.task_product_fact ||
                table == Data.task_rate_fact ||
                table == Data.share_pen_brand_aware ||

                // sceanrios and market plans
                table == Data.scenario ||
                table == Data.market_plan ||
                table == Data.scenario_market_plan ||
                table == Data.market_plan_tree ||
                table == Data.distribution ||
                table == Data.display ||
                table == Data.mass_media ||
                table == Data.market_utility ||
                table == Data.product_channel ||


                // external factors
                table == Data.product_event ||
                table == Data.task_event || // nout used currently

                // external data
                table == Data.external_data ||

                // model parameters
                 table == Data.model_parameter ||

                // simulations etc
                table == Data.simulation ||
                table == Data.scenario_parameter ||
                table == Data.scenario_metric ||
                table == Data.scenario_variable ||
                table == Data.scenario_simseed
                )

                return true;


            return false;
        }

        // filters on model_id
        protected override void InitializeSelectQuery()
        {
            base.InitializeSelectQuery();

            OleDbDataAdapter adapter = null;

            foreach (DataTable table in Data.Tables)
            {
                if (!base.TableInSchema(table) && TableInSchema(table))
                {
                    adapter = getAdapter(table);

                    if (table == Data.simulation)
                    {
                        adapter.SelectCommand.CommandText += " WHERE scenario_id IN " +
                            "(SELECT scenario_id from scenario WHERE model_id = " + ModelID +
                            ")";
                    }
                    else if (table == Data.scenario_metric)
                    {
                        adapter.SelectCommand.CommandText += " WHERE sim_id IN " +
                            "(SELECT id FROM simulation WHERE scenario_id IN " +
                            "(SELECT scenario_id from scenario WHERE model_id = " + ModelID +
                            "))";
                    }
                    else if( table == Data.scenario_variable ) {
                        adapter.SelectCommand.CommandText += " WHERE sim_id IN " +
                            "(SELECT id FROM simulation WHERE scenario_id IN " +
                            "(SELECT scenario_id from scenario WHERE model_id = " + ModelID +
                            "))";
                    }
                    else if( table == Data.scenario_simseed ) {
                        adapter.SelectCommand.CommandText += " WHERE sim_id IN " +
                            "(SELECT id FROM simulation WHERE scenario_id IN " +
                            "(SELECT scenario_id from scenario WHERE model_id = " + ModelID +
                            "))";
                    }
                    else if( table == Data.pack_size_dist )
                    {
                        adapter.SelectCommand.CommandText += " WHERE pack_size_id IN " +
                           "(SELECT id FROM pack_size WHERE model_id = " + ModelID +
                           ")";
                    }
                    else if( table == Data.segment_price_utility )
                    {
                        adapter.SelectCommand.CommandText += " WHERE price_type_id IN " +
                           "(SELECT id FROM price_type WHERE model_id = " + ModelID +
                           ")";
                    }
                    else
                    {
                        adapter.SelectCommand.CommandText += " WHERE model_id = " + ModelID;
                    }
                }
            }

            // addtional filtering
            adapter = getAdapter(Data.display);
            adapter.SelectCommand.CommandText += " AND (media_type <> 'D')";

            adapter = getAdapter(Data.distribution);
            adapter.SelectCommand.CommandText += " AND (media_type = 'D')";
        }

        #endregion

        // public methods

		

		// the current scenario ID
		private int scenario = AllID;
		public int CurrentScenario
		{
			set
			{
				scenario = value;
			}

			get
			{
				return scenario;
			}
		}

		
		
		private int currenMarketPlan = ModelDb.AllID;

		public int MarketPlanID
		{
			get
			{
				return currenMarketPlan;
			}

			set
			{
				currenMarketPlan = value;
			}
		}


		public ModelDb()
		{
		}

		public void RefreshModelPlusSimQueue()
		{
			if (Connection == null)
				return;

			Refresh();

			// tableAdapters[(int) TableIndex.sim_queue].Fill(Data.sim_queue);
			// tableAdapters[(int) TableIndex.sim_variable_value].Fill(Data.sim_variable_value);
		}

		public void RefreshModelPlusResults()
		{
			if (Connection == null)
				return;

			// RefreshModelPlusSimQueue();

			// tableAdapters[(int) TableIndex.results_std].Fill(Data.results_std);
			// tableAdapters[(int) TableIndex.sim_variable_value].Fill(Data.sim_variable_value);
		}

		public void ReadRunLog(int runID)
		{
            //if (Connection == null)
            //    return;

            //Data.Clear();

            //tableAdapters[(int) TableIndex.modelInfo].Fill(Data.Model_info);

            //// get model info
            //iModel = (MrktSimDBSchema.Model_infoRow) Data.Model_info.Rows.Find(ModelID);

            //createItemsAsAll();

            ////tableAdapters[(int) TableIndex.brand].Fill(Data.brand);
            //tableAdapters[(int) TableIndex.product_type].Fill(Data.product_type);
            //tableAdapters[(int) TableIndex.product].Fill(Data.product);
            //tableAdapters[(int) TableIndex.segment].Fill(Data.segment);
            //tableAdapters[(int) TableIndex.channel].Fill(Data.channel);

            ////
            //// we only read one run log at a time as it can create data bloat
            //// but we want to be able to reference the products, channels and segments
            //tableSelectCommands[(int)TableIndex.run_log].CommandText = @"SELECT run_log.run_id, calendar_date, product_id, segment_id, channel_id, comp_id, message_id, message " +
            //    "FROM run_log WHERE run_log.run_id = " + runID;

            //tableAdapters[(int)TableIndex.run_log].Fill(Data.run_log);
		}

		public void ReadScenario(int runID)
		{
			if (Connection == null)
				return;

			// check that model is ope
			if (Model == null)
				return;			
		}

		// Update the database from memory
        /// <summary>
        /// Updates the market plan.  Also copies and updates components if the plan is a top-level plan (only if days != 0 or the top-level plan 
        /// has a different product id as its components).  For a component, updates channel id and scenario id of the component data also.
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="days"></param>
		public void UpdateMarketPlan(MrktSimDBSchema.market_planRow plan, int days)
		{
			switch((PlanType) plan.type)
			{
				case PlanType.Price:
					updateComponentPlan(plan, Data.product_channel, days);
					break;
				case PlanType.Distribution:
					updateComponentPlan(plan, Data.distribution, days);
					break; 
				case PlanType.Display:
					updateComponentPlan(plan, Data.display, days);
					break; 
				case PlanType.Market_Utility:
					updateComponentPlan(plan, Data.market_utility, days);
					break; 
				case PlanType.Media:
					updateComponentPlan(plan, Data.mass_media, days);
					break; 
				case PlanType.ProdEvent:
					updateComponentPlan(plan, Data.product_event, days);
					break; 
				case PlanType.TaskEvent:
					// task_event are a little different
					string query = "market_plan_id = " + plan.id;

					DataRow[] rows = Data.task_event.Select(query, "", DataViewRowState.CurrentRows);

					foreach(DataRow row in rows)
					{
						row["task_id"] = plan.task_id;
					}
					break;

				case PlanType.MarketPlan:
					// we make a copy of all the plan associated to this plan
					updateTopMarketPlan(plan, days);
					break;
			}
		}

        /// <summary>
        /// Updates the given plan by making copies of the components and using the copies in place of the originals.
        /// Does nothing unless either a) days is not 0, or b) the new plan has a different product ID than its components.
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="days"></param>
		private void updateTopMarketPlan(MrktSimDBSchema.market_planRow plan, int days)
		{
			string query = "parent_id = " + plan.id;

			// select each component plan to this plan
			DataRow[] rows = Data.market_plan_tree.Select(query,"", DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				// check if the product_id matches the plan product_id
				MrktSimDBSchema.market_planRow orig = Data.market_plan.FindByid((int) row["child_id"]);

				if( days == 0 && orig.product_id == plan.product_id)
					continue;

				// make copy of this plan
				MrktSimDBSchema.market_planRow copy = CopyPlan(orig);

				// remove original component from market_plan
				row.Delete();

				// update copy to product_id
				copy.product_id = plan.product_id;

				copy.start_date = orig.start_date.AddDays(days);
				copy.end_date = orig.end_date.AddDays(days);

				UpdateMarketPlan(copy, days);

				// finally add to market_plan
				CreatePlanRelation(plan, copy);
			}
		}

        /// <summary>
        /// Updates the data for this component to match the given plan (product_id, channel_id, and scenario_id).  If days is not 0, the component data
        /// will be time-shifted by the specifed number of days.
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="table"></param>
        /// <param name="days"></param>
		private void updateComponentPlan(MrktSimDBSchema.market_planRow plan, DataTable table, int days)
		{
			string query = "market_plan_id = " + plan.id;

			DataRow[] rows;
			
			rows = table.Select(query, "", DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
                row[ "product_id" ] = plan.product_id;

                //JimJ 18Oct07
                if( table.Columns.Contains( "channel_id" ) ) {
                    row[ "channel_id" ] = plan.channel_id;
                }
                if( table.Columns.Contains( "segment_id" ) ) {
                    row[ "segment_id" ] = plan.segment_id;       
                }

				if (days != 0)
				{
					row["start_date"] = ((DateTime) row["start_date"]).AddDays(days);
					row["end_date"] = ((DateTime) row["end_date"]).AddDays(days);
				}
			}
		}


		// the copy of a model does not contail the All items.
		public ModelDb Copy()
		{
			ModelDb dbCopy = new ModelDb();

			dbCopy.Connection = this.Connection;
			dbCopy.ModelID = this.ModelID;

            // copies all data
            base.Copy(dbCopy);

            //// data must be copied in order of dependencies
            //copyTable(dbCopy.Data.project, this.Data.project);
            //copyTable(dbCopy.Data.Model_info, this.Data.Model_info);
            //copyTable(dbCopy.Data.scenario, this.Data.scenario);
            ////copyTable(dbCopy.Data.brand, this.Data.brand);
            //copyTable(dbCopy.Data.product_type, this.Data.product_type);
            //copyTable(dbCopy.Data.product, this.Data.product);
            //copyTable(dbCopy.Data.segment, this.Data.segment);
            //copyTable(dbCopy.Data.channel, this.Data.channel);
            //copyTable(dbCopy.Data.product_attribute, this.Data.product_attribute);
            //copyTable(dbCopy.Data.product_tree, this.Data.product_tree);

            //copyTable(dbCopy.Data.product_channel_size, this.Data.product_channel_size);
			
            //copyTable(dbCopy.Data.task, this.Data.task);
            //copyTable(dbCopy.Data.market_plan, this.Data.market_plan);
            //copyTable(dbCopy.Data.market_plan_tree, this.Data.market_plan_tree);
            //copyTable(dbCopy.Data.scenario_market_plan, this.Data.scenario_market_plan);
            //copyTable(dbCopy.Data.product_channel, this.Data.product_channel);
            //copyTable(dbCopy.Data.segment_channel, this.Data.segment_channel);
            //copyTable(dbCopy.Data.share_pen_brand_aware, this.Data.share_pen_brand_aware);
            //copyTable(dbCopy.Data.consumer_preference, this.Data.consumer_preference);
            //copyTable(dbCopy.Data.product_attribute_value, this.Data.product_attribute_value);
            //copyTable(dbCopy.Data.product_matrix, this.Data.product_matrix);
            //copyTable(dbCopy.Data.task_product_fact, this.Data.task_product_fact);
            //copyTable(dbCopy.Data.task_rate_fact, this.Data.task_rate_fact);
            //copyTable(dbCopy.Data.distribution, this.Data.distribution);
            //copyTable(dbCopy.Data.display, this.Data.display);
            //copyTable(dbCopy.Data.market_utility, this.Data.market_utility);
            //copyTable(dbCopy.Data.mass_media, this.Data.mass_media);
            //copyTable(dbCopy.Data.product_event, this.Data.product_event);
            //copyTable(dbCopy.Data.task_event, this.Data.task_event);

            //copyTable(dbCopy.Data.network_parameter, this.Data.network_parameter);
            //copyTable(dbCopy.Data.segment_network, this.Data.segment_network);
            //copyTable(dbCopy.Data.external_data, this.Data.external_data);

            //copyTable(dbCopy.Data.model_parameter, this.Data.model_parameter);

            //copyTable(dbCopy.Data.simulation, this.Data.simulation);
            //copyTable(dbCopy.Data.scenario_parameter, this.Data.scenario_parameter);

            ////copyTable(dbCopy.Data.sim_queue, this.Data.sim_queue);
            ////copyTable(dbCopy.Data.run_log, this.Data.run_log);
            //copyTable(dbCopy.Data.scenario_variable, this.Data.scenario_variable);

            //// copyTable(dbCopy.Data.sim_variable_value, this.Data.sim_variable_value);

            //copyTable(dbCopy.Data.scenario_simseed, this.Data.scenario_simseed);
            //copyTable(dbCopy.Data.scenario_metric, this.Data.scenario_metric);




			// reset scenarios that do not have results
			// this occurs because we might not have read the results in
			foreach(MrktSimDBSchema.simulationRow simulation in dbCopy.Data.simulation)
            {
                simulation.sim_num = -1;
			}

            string filter = "project_id = " + Model.project_id;
            dbCopy.Model.model_name = ModelDb.CreateUniqueName(Data.Model_info, "model_name", Model.model_name, filter);
          
            // we need to switch project on the model
            dbCopy.Data.EnforceConstraints = false;
            MrktSimDBSchema.projectRow currentProj = dbCopy.Model.projectRow;
            dbCopy.Model.project_id = Model.project_id;
            currentProj.id = dbCopy.Model.project_id;
            dbCopy.Data.EnforceConstraints = true;

            // we do not want to write the project to db.
            dbCopy.Data.project.AcceptChanges();

            // do not write "all" items
            dbCopy.AcceptItemsMarkedAll();

            // set these flags to appropriate default
            dbCopy.Model.locked = false;
            dbCopy.Model.read_only = false;
            dbCopy.ReadOnly = false;

			return dbCopy;
        }

        /// <summary>
        /// Makes a shallow copy of the scenario.  The copied scenario will contain references to the same market plans as the original.
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public MrktSimDBSchema.scenarioRow CopyScenario(MrktSimDBSchema.scenarioRow orig, string name)
        {
            // get a new name for scenario
            MrktSimDBSchema.scenarioRow scenario = CreateStandardScenario(name);

            scenario.descr = orig.descr;

            scenario.user_name = orig.user_name;

            foreach (MrktSimDBSchema.scenario_market_planRow mrktplnRef in orig.Getscenario_market_planRows())
            {
                AddMarketPlanToScenario(scenario, mrktplnRef.market_planRow);
            }

            return scenario;
        }

        /// <summary>
        /// Makes a deep copy of the scenario.  The copied scenario will contain copies of the original market plans, that in turm contain copies of the original components
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public MrktSimDBSchema.scenarioRow DeepCopyScenario( MrktSimDBSchema.scenarioRow orig, string name, bool copyComponents ) {
            // get a new name for scenario
            MrktSimDBSchema.scenarioRow scenario = CreateStandardScenario( name );

            scenario.descr = orig.descr;

            scenario.user_name = orig.user_name;

            foreach( MrktSimDBSchema.scenario_market_planRow mrktplnRef in orig.Getscenario_market_planRows() ) {

                MrktSimDBSchema.market_planRow copiedPlan = CopyPlan( mrktplnRef.market_planRow, copyComponents );

                AddMarketPlanToScenario( scenario, copiedPlan );
            }

            return scenario;
        }


        #region Product Tree Methods

   

        public MrktSimDBSchema.productRow ChangeProductType(int product_id, int type_id)
        {
            string query = "product_id = " + product_id;
            DataRow[] temp = Data.product.Select(query, "", DataViewRowState.CurrentRows);

            MrktSimDBSchema.productRow row = (MrktSimDBSchema.productRow)temp[0];
            row.product_type = type_id;
            return row;
        }

        public MrktSimDBSchema.productRow CreateChildProduct(int parentID, string name, string type_name)
        {
            string query = "type_name = '" + type_name + "'";
            DataRow[] tempRow = Data.product_type.Select(query, "", DataViewRowState.CurrentRows);
            return CreateChildProduct(parentID, name, Convert.ToInt32(tempRow[0]["id"].ToString()));
        }

        public MrktSimDBSchema.productRow CreateChildProduct(int parentID, string name, int product_type)
        {
            MrktSimDBSchema.productRow child = CreateProduct(name, product_type);

            MrktSimDBSchema.productRow parent = Data.product.FindByproduct_id(parentID);

            parent.brand_id = 0;
            child.brand_id = 1;


            //Add ProductTreeRow
            MrktSimDBSchema.product_treeRow treeRow = Data.product_tree.Newproduct_treeRow();
            treeRow.parent_id = parentID;
            treeRow.child_id = child.product_id;
            treeRow.model_id = ModelID;
            Data.product_tree.Addproduct_treeRow(treeRow);

            DeleteAuxForProduct(parent);
            CreateAuxForProduct(child);

            if (ProjectDb.Nimo && child.product_typeRow.type_name == "Product")
            {
                MrktSimDBSchema.product_attributeRow myAttr = CreateProductAttribute( child.product_name, ModelDb.AttributeType.Standard );

                // now make sure that all product_attributes are zero if not this product
                MrktSimDBSchema.product_attribute_valueRow[] prodAttrVals = myAttr.Getproduct_attribute_valueRows();

                foreach (MrktSimDBSchema.product_attribute_valueRow prodAttrVal in prodAttrVals)
                {
                    if (prodAttrVal.product_id == child.product_id)
                    {
                        prodAttrVal.post_attribute_value = 1;
                        prodAttrVal.pre_attribute_value = 1;
                    }
                    else
                    {
                        prodAttrVal.post_attribute_value = 0;
                        prodAttrVal.pre_attribute_value = 0;
                    }
                }
            }

            return child;
        }

        public MrktSimDBSchema.productRow CreateParentProduct(int childID, string name, string type_name)
        {
            string query = "type_name = '" + type_name + "'";
            DataRow[] tempRow = Data.product_type.Select(query, "", DataViewRowState.CurrentRows);
            return CreateChildProduct(childID, name, Convert.ToInt32(tempRow[0]["id"].ToString()));
        }

        public MrktSimDBSchema.productRow CreateParentProduct(int childID, string name, int product_type)
        {
            MrktSimDBSchema.productRow parent = CreateProduct(name, product_type);

            parent.brand_id = 0;

            //Add ProductTreeRow
            MrktSimDBSchema.product_treeRow treeRow = Data.product_tree.Newproduct_treeRow();
            treeRow.parent_id = parent.product_id;
            treeRow.child_id = childID;
            treeRow.model_id = ModelID;
            Data.product_tree.Addproduct_treeRow(treeRow);

            //CreateAuxForProduct(parent);			

            return parent;
        }

        public MrktSimDBSchema.productRow CreateRootProduct(string name, string type_name)
        {
            string query = "type_name = '" + type_name + "'";
            DataRow[] tempRow = Data.product_type.Select(query, "", DataViewRowState.CurrentRows);
            return CreateRootProduct(name, Convert.ToInt32(tempRow[0]["id"].ToString()));
        }

        public MrktSimDBSchema.productRow CreateRootProduct(string name, int product_type)
        {
            MrktSimDBSchema.productRow root = CreateProduct(name, product_type);

            root.brand_id = 1;

            CreateAuxForProduct(root);

            return root;
        }

        public void MoveProduct(int product_id, int new_parent_id)
        {
            string query = "product_id = " + product_id;
            MrktSimDBSchema.productRow myself = Data.product.FindByproduct_id(product_id);

            DataRow[] old_parent_trow = Data.product_tree.Select("child_id = " + product_id, "", DataViewRowState.CurrentRows);

            if (old_parent_trow.Length > 0)
            {
                MrktSimDBSchema.productRow old_parent = Data.product.FindByproduct_id((int)old_parent_trow[0]["parent_id"]);

                old_parent_trow[0].Delete();

                if (isLeaf(old_parent))
                {
                    old_parent.brand_id = 1;
                    CreateAuxForProduct(old_parent);
                }
            }

            if (new_parent_id != -1)
            {
                MrktSimDBSchema.productRow new_parent = Data.product.FindByproduct_id(new_parent_id);

                if (isLeaf(new_parent))
                {
                    new_parent.brand_id = 0;
                    DeleteAuxForProduct(new_parent);
                }

                CreateProductTree(new_parent_id, product_id);
            }
        }

        public bool isLeaf(MrktSimDBSchema.productRow row)
        {
            DataRow[] children = Data.product_tree.Select("parent_id = " + row.product_id, "", DataViewRowState.CurrentRows);

            if (children.Length > 0)
            {
                return false;
            }

            return true;
        }

        public void CleanProductAux()
        {
            DataRow[] rows = Data.product.Select("product_id <> -1", "", DataViewRowState.CurrentRows);
            foreach (MrktSimDBSchema.productRow row in rows)
            {
                if (isLeaf(row))
                {
                    row.brand_id = 1;
                }
                else
                {
                    row.brand_id = 0;
                    DeleteAuxForProduct(row);
                }
            }
        }

        public void DeleteAuxForProduct(MrktSimDBSchema.productRow row)
        {
            string query = "product_id = " + row.product_id;

            DataRow[] rows = Data.share_pen_brand_aware.Select(query, "", DataViewRowState.CurrentRows);

            foreach (MrktSimDBSchema.share_pen_brand_awareRow initialRow in rows)
            {
                initialRow.Delete();
            }

            rows = Data.task_product_fact.Select(query, "", DataViewRowState.CurrentRows);

            // task product
            foreach (MrktSimDBSchema.task_product_factRow taskRow in rows)
            {
                taskRow.Delete();
            }

            rows = Data.product_attribute_value.Select(query, "", DataViewRowState.CurrentRows);

            // products must have some value for product attributes
            foreach (MrktSimDBSchema.product_attribute_valueRow attributeRow in rows)
            {
                attributeRow.Delete();
            }

            rows = Data.market_plan.Select(query, "", DataViewRowState.CurrentRows);

            foreach (MrktSimDBSchema.market_planRow mpRow in rows)
            {
                mpRow.Delete();
            }

            // new product size
            // merged from 2.2
            // SSN 12/13/2007
            rows = Data.product_channel_size.Select(query, "", DataViewRowState.CurrentRows);
            foreach (MrktSimDBSchema.product_channel_sizeRow pszRow in rows)
            {
                pszRow.Delete();
            }

        }

        public void CreateAuxForProduct(MrktSimDBSchema.productRow row)
        {
            // update other tables with new product

            // initial values
            foreach (MrktSimDBSchema.segmentRow segmentRow in Data.segment.Rows)
            {
                if (segmentRow.RowState != DataRowState.Deleted &&
                    segmentRow.segment_id != AllID)
                    CreateInitial(row, segmentRow.segment_id);
            }

            /*
            // channel pricing
            foreach(MrktSimDBSchema.channelRow channelRow in Data.channel.Rows)
            {
                if (channelRow.RowState != DataRowState.Deleted &&
                    channelRow.channel_id != AllID)
                    CreateProductChannel(row.product_id, channelRow.channel_id);
            }
            */

            // task product
            //foreach (DataRow trow in Data.task.Select("task_id <> " + AllID, "", DataViewRowState.CurrentRows))
            //{
            //    MrktSimDBSchema.taskRow taskRow = (MrktSimDBSchema.taskRow)trow;

            //    createTaskProductEntry(row.product_id, taskRow.task_id);
            //}

            // products must have some value for product attributes
            foreach (DataRow parow in Data.product_attribute.Select("", "", DataViewRowState.CurrentRows))
            {
                MrktSimDBSchema.product_attributeRow prodAttrRow = (MrktSimDBSchema.product_attributeRow)parow;

                CreateProductAttributeValue(row.product_id, prodAttrRow.product_attribute_id, this.StartDate);
            }

            // products must have some size
            foreach (MrktSimDBSchema.channelRow chanRow in Data.channel.Select("", "", DataViewRowState.CurrentRows))
            {
                CreateProductChannelSize(row, chanRow);
            }

            // as a curtesy we create a marketing plan for each product
            /*MrktSimDBSchema.market_planRow mp = this.CreateMarketPlan("default", PlanType.MarketPlan);
            mp.product_id = row.product_id;

            MrktSimDBSchema.market_planRow price = this.CreateMarketPlan("default", PlanType.Price);
            price.product_id = row.product_id;

            MrktSimDBSchema.market_planRow dist = this.CreateMarketPlan("default", PlanType.Distribution);
            dist.product_id = row.product_id;

            MrktSimDBSchema.market_planRow display = this.CreateMarketPlan("default", PlanType.Display);
            display.product_id = row.product_id;

            MrktSimDBSchema.market_planRow market_utility = this.CreateMarketPlan("default", PlanType.Market_Utility);
            market_utility.product_id = row.product_id;

            MrktSimDBSchema.market_planRow media = this.CreateMarketPlan("default", PlanType.Media);
            media.product_id = row.product_id;*/
        }

        #endregion

        #region Public Object Creation Routines

        public MrktSimDBSchema.scenarioRow CreateStandardScenario(string name)
		{
			MrktSimDBSchema.scenarioRow row = Data.scenario.NewscenarioRow();

			row.name = CreateUniqueName(this.Data.scenario, "name", name, null);
			row.model_id = ModelID;
			row.descr = "";
			row.user_name = "admin";

			Data.scenario.AddscenarioRow(row);

			return row;
		}

        public MrktSimDBSchema.product_typeRow CreateProductType(string name)
        {
            MrktSimDBSchema.product_typeRow row = Data.product_type.Newproduct_typeRow();

            row.type_name = CreateUniqueName(Data.product_type, "type_name", name, "");
            row.model_id = ModelID;

            Data.product_type.Addproduct_typeRow(row);

            return row;
        }

        /// <summary>
        /// Creates the pack size AND the pack_dist rows that go with it
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numPoints"></param>
        /// <returns></returns>
        /// 
        public MrktSimDBSchema.price_typeRow CreatePriceType( string name)
        {
            MrktSimDBSchema.price_typeRow row = Data.price_type.Newprice_typeRow();

            row.name = CreateUniqueName( Data.price_type, "name", name, "" );
            row.model_id = ModelID;
            row.awareness = 0;
            row.persuasion = 0;
            row.BOGN = 0;
            row.relative = false;

            Data.price_type.Addprice_typeRow(row);

            // create a utility for each segment
            foreach( MrktSimDBSchema.segmentRow seg in Data.segment.Select("segment_id <> -1", "", DataViewRowState.CurrentRows))
            {
                // create segment_utility for seg and price type row
                createSegmentPriceUtility( seg, row );
            }

            return row;
        }

        private MrktSimDBSchema.segment_price_utilityRow createSegmentPriceUtility( MrktSimDBSchema.segmentRow seg, MrktSimDBSchema.price_typeRow price )
        {
            MrktSimDBSchema.segment_price_utilityRow row = Data.segment_price_utility.Newsegment_price_utilityRow();

            row.segment_id = seg.segment_id;
            row.price_type_id = price.id;
            row.util = 0;

            Data.segment_price_utility.Addsegment_price_utilityRow( row );

            return row;
        }

        public MrktSimDBSchema.pack_sizeRow CreatePackSize( string name, int numPoints )
        {
            MrktSimDBSchema.pack_sizeRow row = Data.pack_size.Newpack_sizeRow();

            row.name = CreateUniqueName( Data.pack_size, "name", name, "" );
            row.model_id = ModelID;

            Data.pack_size.Addpack_sizeRow( row );


            EditPackSize( row, numPoints );

            return row;
        }

        public int EditPackSize( MrktSimDBSchema.pack_sizeRow packSize, int numPoints )
        {
            // are we creating or losing entries
            int currNum = packSize.Getpack_size_distRows().Length;

            int rval = numPoints - currNum;

            if( rval > 0)
            {
                // create rval dist values
                // now create the dist points to go with it
                for( int ii = 0; ii < rval; ++ii )
                {
                    MrktSimDBSchema.pack_size_distRow distRow = Data.pack_size_dist.Newpack_size_distRow();

                    distRow.pack_size_id = packSize.id;
                    distRow.size = currNum + ii + 1;
                    distRow.dist = 0;

                    Data.pack_size_dist.Addpack_size_distRow( distRow );
                }

            }
            else if( rval < 0 )
            {
                string query = "size > " + numPoints.ToString();
                // remove dist values from end so new size is numPoints
                DataRow[] rows = Data.pack_size_dist.Select( query, "", DataViewRowState.CurrentRows );
                foreach( DataRow row in rows )
                {
                    row.Delete();
                }
            }

            return rval;
        }

        public double MeanPackSize( MrktSimDBSchema.pack_sizeRow packSize )
        {
            double ave = 0.0;

            foreach( MrktSimDBSchema.pack_size_distRow dist in packSize.Getpack_size_distRows() )
            {
                ave += dist.size * dist.dist / 100.0;
            }

            return ave;
        }

		private MrktSimDBSchema.productRow CreateProduct(string name, int product_type)
		{
			// create a new product
			// create product within this brand
			MrktSimDBSchema.productRow row = Data.product.NewproductRow();


			row.model_id = ModelID;
			row.brand_id = 0;

			// default values
			row.product_name = CreateUniqueName(Data.product, "product_name", name, "");
			row.type = "Initial";
			row.product_group = "1";
			row.related_group = "none";
			row.percent_relation = "irrellevant";
			row.cost = 0;
			row.initial_dislike_probability = 0;
			row.repeat_like_probability = 100;
			row.color = "Yellow";
			row.product_type = product_type;
            row.base_price = 1;
            row.eq_units = 1;
            row.pack_size_id = -1;

			Data.product.AddproductRow(row);

			return row;
		}

	

        public MrktSimDBSchema.product_channel_sizeRow CreateProductChannelSize(MrktSimDBSchema.productRow prod, MrktSimDBSchema.channelRow chan)
        {

            if (prod.brand_id != 1)
                return null;

            if (chan.channel_id == Database.AllID)
                return null;

            MrktSimDBSchema.product_channel_sizeRow pcsz = Data.product_channel_size.Newproduct_channel_sizeRow();

            pcsz.model_id = prod.model_id;
            pcsz.product_id = prod.product_id;
            pcsz.channel_id = chan.channel_id;
            pcsz.prod_size = 1;

            Data.product_channel_size.Addproduct_channel_sizeRow(pcsz);

            return pcsz;
        }

		public MrktSimDBSchema.product_treeRow CreateProductTree(int parentId, int childId)
		{
			//Add ProductTreeRow
			MrktSimDBSchema.product_treeRow treeRow = Data.product_tree.Newproduct_treeRow();
			treeRow.parent_id = parentId;
			treeRow.child_id = childId;
			treeRow.model_id = ModelID;
			Data.product_tree.Addproduct_treeRow(treeRow);

			return treeRow;
		}


		public MrktSimDBSchema.segmentRow CreateSegment(string name)
		{
			// create a new segment
			MrktSimDBSchema.segmentRow row = Data.segment.NewsegmentRow();

			// default values
			row.model_id = ModelID;
			row.segment_name = CreateUniqueName(Data.segment, "segment_name", name, "");
			row.segment_model = "m";
			row.color = "Red";
			row.show_current_share_pie_chart = "n  ";
			row.show_cmulative_share_chart = "y  ";
			row.segment_size = 100;
			row.growth_rate = 0;
			row.growth_rate_people_percent = "people";
			row.growth_rate_month_year = "month";
			row.compress_population = "n  ";
			row.variability = 20;
			row.price_disutility = 3;
			row.attribute_sensitivity = 1;
			row.persuasion_scaling = 0.1;
			row.display_utility = 0;
			row.display_utility_scaling_factor = 1;
			row.max_display_hits_per_trip = 999999;
			row.inertia = 0;
			row.repurchase = "y  ";

            row.repurchase_model = "N  ";
            row.gamma_location_parameter_a = 5;
			row.gamma_shape_parameter_k = 0.5;
			row.repurchase_period_frequency = 12;
			row.repurchase_frequency_variation = 1;
			row.repurchase_timescale = "Months";
			row.avg_max_units_purch = 1;
			row.shopping_trip_interval = 52;
			row.category_penetration = 100;
			row.category_rejection = 0;
			row.num_initial_buyers = 0;
			row.initial_buying_period = 10;
			row.seed_with_repurchasers = "y";
			row.use_budget = "n";
			row.budget = 100;
			row.budget_period = "Months";
			row.save_unspent = "n";
			row.initial_savings = 100;
			row.social_network_model = "Talk-Anytime";
			row.num_close_contacts = 0.0;
			row.prob_talking_close_contact_pre = 1;
			row.prob_talking_close_contact_post = 1;
			row.use_local ="n" ;
			row.num_distant_contacts = 0;
			row.prob_talk_distant_contact_pre = 1;
			row.prob_talk_distant_contact_post = 1;
			row.awareness_weight_personal_message = 0;
			row.pre_persuasion_prob = 0;
			row.post_persuasion_prob = 0;
			row.units_desired_trigger = 0;
			row.awareness_model = "Persuasion & Awareness";
			row.awareness_threshold = 0;
			row.awareness_decay_rate_pre = 0;
			row.awareness_decay_rate_post = 0;
			row.persuasion_decay_rate_pre = 0;
			row.persuasion_decay_rate_post = 0;
			row.persuasion_decay_method = "B";

            row.product_choice_model = "G";
			

			row.persuasion_score = "*";
			row.persuasion_value_computation = "Absolute";
			row.persuasion_contribution_overall_score = "+";
			row.utility_score = "*";
			row.combination_part_utilities = "Scaled Sum of Products";
			row.price_contribution_overall_score = "-";
			row.price_score = "*";
			row.price_value = "Absolute";
			row.reference_price = 0;
			row.choice_prob = "Logit";
			row.inertia_model = "Brand";
			row.error_term = "None";
			row.error_term_user_value = 1;
			row.loyalty = 0;
            row.min_freq = 0;
            row.max_freq = 9999;

            // NIMO defaults
            if (ProjectDb.Nimo)
            {
                row.repurchase_model = "R  ";
                row.product_choice_model = "N";
                row.awareness_decay_rate_pre = 0.4;
                row.awareness_decay_rate_post = 0;
                row.persuasion_decay_rate_pre = 1.53;
                row.persuasion_decay_rate_post = 1.53;

                row.min_freq = 0;
                row.max_freq = 100;
            }

			Data.segment.AddsegmentRow(row);

			// update other tables based on new row
			foreach(DataRow prow in Data.product.Select("product_id <> " + AllID, "", DataViewRowState.CurrentRows))
			{
				MrktSimDBSchema.productRow productRow = (MrktSimDBSchema.productRow) prow;
				
				CreateInitial(productRow, row.segment_id);
			}

			// segment channel
			foreach(DataRow crow in Data.channel.Select("channel_id <> " + AllID, "", DataViewRowState.CurrentRows))
			{
				MrktSimDBSchema.channelRow channelRow = (MrktSimDBSchema.channelRow) crow;
				
				CreateSegmentChannel(row.segment_id, channelRow.channel_id);
			}

			// task segment
			foreach(DataRow trow in Data.task.Select("task_id <> " + AllID, "", DataViewRowState.CurrentRows))
			{
				MrktSimDBSchema.taskRow taskRow = (MrktSimDBSchema.taskRow) trow;
				createTaskSegmentRate(row.segment_id, taskRow.task_id);
			}		

			// segments must have some value for product attributes
			foreach(DataRow parow in Data.product_attribute.Select("","", DataViewRowState.CurrentRows))
			{
				MrktSimDBSchema.product_attributeRow prodAttrRow = (MrktSimDBSchema.product_attributeRow) parow;

				CreateConsumerPreference(row.segment_id, prodAttrRow.product_attribute_id, StartDate);
			}

            foreach( MrktSimDBSchema.price_typeRow price in Data.price_type.Select( "id <> -1", "", DataViewRowState.CurrentRows ) )
            {
                createSegmentPriceUtility( row, price );
            }

			return row;
		}

	
		public MrktSimDBSchema.channelRow CreateChannel(string name)
		{
			// create a new channel
			MrktSimDBSchema.channelRow row = Data.channel.NewchannelRow();

			row.model_id = ModelID;
			
			// default values
			row.channel_name = CreateUniqueName(Data.channel, "channel_name", name, "");

			Data.channel.AddchannelRow(row);

			// update other tables with new channel
			foreach(MrktSimDBSchema.segmentRow segmentRow in Data.segment.Rows)
			{
				if (segmentRow.RowState != DataRowState.Deleted &&
					segmentRow.segment_id != AllID)
					CreateSegmentChannel(segmentRow.segment_id, row.channel_id);
			}

			// update other tables with new channel
			foreach(MrktSimDBSchema.productRow productRow in Data.product.Rows)
			{
				if (productRow.RowState != DataRowState.Deleted)
					CreateProductChannelSize(productRow, row);
			}

			return row;
		}


		public MrktSimDBSchema.taskRow CreateTask(string name)
		{
			MrktSimDBSchema.taskRow row = Data.task.NewtaskRow();

			// initialize
			row.model_id = ModelID;

			row.task_name = CreateUniqueName(Data.task, "task_name", name, "");

			row.start_date = this.StartDate;
			row.end_date = this.EndDate;
			row.suitability_min = 0;
			row.suitability_max = 1;

			Data.task.AddtaskRow(row);

			// add to task product matrix
			DataRow[] prows = Data.product.Select("product_id <> " + AllID, "", DataViewRowState.CurrentRows);

			foreach(DataRow prow in prows)
			{
				MrktSimDBSchema.productRow prodRow = (MrktSimDBSchema.productRow) prow;

				createTaskProductEntry(prodRow.product_id, row.task_id);
			}

			
			// add to task segment rate matrix
			DataRow[] srows = Data.segment.Select("segment_id <> " + AllID, "", DataViewRowState.CurrentRows);

			foreach(DataRow srow in srows)
			{
				MrktSimDBSchema.segmentRow segRow = (MrktSimDBSchema.segmentRow) srow;

				if (segRow.RowState != DataRowState.Deleted &&
					segRow.segment_id != AllID)
					createTaskSegmentRate(segRow.segment_id, row.task_id);
			}		
			
			return row;						   
		}


		public MrktSimDBSchema.product_attributeRow CreateProductAttribute(string name, AttributeType type)
		{
			MrktSimDBSchema.product_attributeRow row = Data.product_attribute.Newproduct_attributeRow();

			// initialize
			row.model_id = ModelID;

			row.product_attribute_name = CreateUniqueName(Data.product_attribute, "product_attribute_name", name, "");

			row.utility_min = 0;
			row.utility_max = 1;

			row.cust_pref_max = 10;
			row.cust_pref_min = -10;
			row.cust_tau = 0;
            row.type = (int) type;

			Data.product_attribute.Addproduct_attributeRow(row);

			
			// add to product utility is standard type
            if( type == AttributeType.Standard ) {
                foreach( DataRow prow in Data.product.Select( "product_id <> " + AllID + " AND brand_id = 1", "", DataViewRowState.CurrentRows ) ) {
                    MrktSimDBSchema.productRow prodRow = (MrktSimDBSchema.productRow)prow;

                    CreateProductAttributeValue( prodRow.product_id, row.product_attribute_id, StartDate );
                }
            }

			
			// add to segment preference
			foreach(DataRow srow in Data.segment.Select("segment_id <> " + AllID, "", DataViewRowState.CurrentRows))
			{
				MrktSimDBSchema.segmentRow segRow = (MrktSimDBSchema.segmentRow) srow;
				
				CreateConsumerPreference(segRow.segment_id, row.product_attribute_id, StartDate);
			}		
			
			return row;		
		}

		

		
		public MrktSimDBSchema.product_channelRow CreateProductChannel(int productID, int channelID)
		{
			// create a new brand
			MrktSimDBSchema.product_channelRow row = Data.product_channel.Newproduct_channelRow();

			row.market_plan_id = MarketPlanID;
			row.model_id = ModelID;
			row.product_id = productID;
			row.channel_id = channelID;
			row.markup = 0;
			row.price = 0;
			row.periodic_price = 0;
			row.how_often = "Month";
			row.percent_SKU_in_dist = 100;

            row.price_type = Database.AllID;

			// the default is the model start end
			row.start_date = StartDate;
			row.end_date = EndDate;
			
			Data.product_channel.Addproduct_channelRow(row);

			return row;
		}

		// default is all channels and segments and variable data
		public MrktSimDBSchema.mass_mediaRow CreateMassMedia(int productID, int channelID, int segmentID)
		{
			return CreateMassMedia(productID, channelID, segmentID, "V");
		}

		public MrktSimDBSchema.mass_mediaRow CreateMassMedia(int productID, int channelID, int segmentID, string mediaType)
		{
			// create a new brand
			MrktSimDBSchema.mass_mediaRow row = Data.mass_media.Newmass_mediaRow();

			row.market_plan_id = MarketPlanID;
			row.model_id = ModelID;
			row.product_id = productID;
			row.channel_id = channelID;
			row.segment_id = segmentID;
			row.media_type = mediaType;
			row.attr_value_G = 0.0;
			row.attr_value_H = 0.0;
			row.attr_value_I = 0.0;
			row.message_awareness_probability = 0.6;
			row.message_persuation_probability = 0.0;

			// the default is the model start end
			row.start_date = StartDate;
			row.end_date = EndDate;
			
			Data.mass_media.Addmass_mediaRow(row);

			return row;
		}

		public MrktSimDBSchema.distributionRow CreateDistribution(int productID, int channelID)
		{
			// create new ditribution row
			MrktSimDBSchema.distributionRow row = Data.distribution.NewdistributionRow();

			row.market_plan_id = MarketPlanID;
			row.model_id = ModelID;
			row.product_id = productID;
			row.channel_id = channelID;
			row.attr_value_F = 100.0;
			row.attr_value_G = 100.0;
			row.message_awareness_probability = 0.1;
			row.message_persuation_probability = 0.1;

			// the default is the model start end
			row.start_date = StartDate;
			row.end_date = EndDate;
			
			Data.distribution.AdddistributionRow(row);

			return row;
		}

		public MrktSimDBSchema.displayRow CreateDisplay(int productID, int channelID)
		{
			// create new ditribution row
			MrktSimDBSchema.displayRow row = Data.display.NewdisplayRow();

			row.market_plan_id = MarketPlanID;
			row.model_id = ModelID;
			row.product_id = productID;
			row.channel_id = channelID;
			row.media_type = "Y";
			row.attr_value_F = 10.0;
			row.message_awareness_probability = 0.1;
			row.message_persuation_probability = 0.1;

			// the default is the model start end
			row.start_date = StartDate;
			row.end_date = EndDate;
			
			Data.display.AdddisplayRow(row);

			return row;
		}

		
		public MrktSimDBSchema.market_utilityRow CreateMarketUtility(int productID, int channelID, int segmentID)
		{
			// create new ditribution row
			MrktSimDBSchema.market_utilityRow row = Data.market_utility.Newmarket_utilityRow();

			row.market_plan_id = MarketPlanID;
			row.model_id = ModelID;
			row.product_id = productID;
			row.channel_id = channelID;
			row.segment_id = segmentID;
			row.percent_dist = 10.0;
			row.awareness = 0.1;
			row.persuasion = 0.1;
			row.utility = 1.0;

			// the default is the model start end
			row.start_date = StartDate;
			row.end_date = EndDate;
			
			Data.market_utility.Addmarket_utilityRow(row);

			return row;
		}

		public MrktSimDBSchema.task_eventRow CreateTaskEvent(int taskID, int segmentID)
		{
			// create new ditribution row
			MrktSimDBSchema.task_eventRow row = Data.task_event.Newtask_eventRow();

			row.market_plan_id = MarketPlanID;
			row.model_id = ModelID;
			row.segment_id = segmentID;
			row.task_id = taskID;
			row.demand_modification = 0.0;

			// the default is the model start end
			row.start_date = StartDate;
			row.end_date = EndDate;
			
			Data.task_event.Addtask_eventRow(row);

			return row;
		}

		
		public MrktSimDBSchema.product_matrixRow CreateProductMatrix(int haveProductID, int wantProductID,
			string matrixValue)
		{
			// create a new brand
			MrktSimDBSchema.product_matrixRow row = Data.product_matrix.Newproduct_matrixRow();

			row.model_id = ModelID;
			row.have_product_id = haveProductID;
			row.want_product_id = wantProductID;
			row.value = matrixValue;

			Data.product_matrix.Addproduct_matrixRow(row);

			return row;
		}

		public  MrktSimDBSchema.product_attribute_valueRow CreateProductAttributeValue(
			int prodID,
			int prodAttrID,
			DateTime date)
		{
			// ensure uniqueness of date
			// first check if there is a date at exactly that time
			// the query
			string query = "product_id = " + prodID;
			query += " AND product_attribute_id = " + prodAttrID;
			query += " AND start_date = #" + date.Date.ToString("d") + "#";

			DataRow[] rows =  Data.product_attribute_value.Select(query,"", DataViewRowState.CurrentRows);

			if (rows.Length > 0 )
			{
				return (MrktSimDBSchema.product_attribute_valueRow) rows[0];
			}

			MrktSimDBSchema.product_attributeRow attrRow = Data.product_attribute.FindByproduct_attribute_id(prodAttrID);
			MrktSimDBSchema.product_attribute_valueRow row = Data.product_attribute_value.Newproduct_attribute_valueRow();

			// initialize
			row.model_id = ModelID;

			row.product_attribute_id = attrRow.product_attribute_id;
			row.product_id = prodID;
			row.pre_attribute_value = attrRow.utility_min;
			row.post_attribute_value = attrRow.utility_min;
			row.start_date = date.Date;
			row.has_attribute = true;

			Data.product_attribute_value.Addproduct_attribute_valueRow(row);

			return row;
		}

		public MrktSimDBSchema.consumer_preferenceRow CreateConsumerPreference(int segID, int attrID, DateTime date)
		{
			// ensure uniqueness of date
			// first check if there is a date at exactly that time
			// the query
			string query = "segment_id = " + segID;
			query += " AND product_attribute_id = " + attrID;
			query += " AND start_date = #" + date.Date.ToString("d") + "#";

			DataRow[] rows =  Data.consumer_preference.Select(query,"", DataViewRowState.CurrentRows);

			if (rows.Length > 0 )
			{
				return (MrktSimDBSchema.consumer_preferenceRow) rows[0];
			}

			MrktSimDBSchema.consumer_preferenceRow row = Data.consumer_preference.Newconsumer_preferenceRow();

			// initialize
			row.model_id = ModelID;

			row.product_attribute_id = attrID;
			row.segment_id = segID;
			row.pre_preference_value = 0;
			row.post_preference_value = 0;
            row.price_sensitivity = 0;
			row.start_date = date.Date;


			Data.consumer_preference.Addconsumer_preferenceRow(row);

			return row;
		}


		public MrktSimDBSchema.market_plan_treeRow CreatePlanRelation(MrktSimDBSchema.market_planRow parent, MrktSimDBSchema.market_planRow child)
		{
            if( parent.product_id != child.product_id ) {
                throw (new Exception( "Plans must have same products" ));
            }
            else {
                return CreatePlanRelation( parent.id, child.id );
            }
		}

		public MrktSimDBSchema.market_plan_treeRow CreatePlanRelation(int parent_id, int child_id)
		{
			// check if relation already exists

			string query = "parent_id = " + parent_id + " AND child_id = " + child_id;

			DataRow[] rows = Data.market_plan_tree.Select(query,"",DataViewRowState.CurrentRows);

			if (rows.Length > 0 )
				return (MrktSimDBSchema.market_plan_treeRow) rows[0];

			MrktSimDBSchema.market_plan_treeRow row = Data.market_plan_tree.Newmarket_plan_treeRow();

			row.model_id = ModelID;
			row.parent_id = parent_id;
			row.child_id = child_id;

			Data.market_plan_tree.Addmarket_plan_treeRow(row);

			return row;
		}

		public MrktSimDBSchema.network_parameterRow CreateNetworkParameter(string name)
		{
			// create with defaults let caller set as desired
			// create a new brand
			MrktSimDBSchema.network_parameterRow row = Data.network_parameter.Newnetwork_parameterRow();

			row.model_id = ModelID;
			row.name = CreateUniqueName(Data.network_parameter, "name", name, "");
			row.type = 0; // Talk Anytime
			row.awareness_weight = 0.0;
			row.num_contacts = 0;
			
			row.persuasion_pre_use = 0;
			row.persuasion_post_use = 0;
			row.prob_of_talking_pre_use = 0;
			row.prob_of_talking_post_use = 0;
			row.use_local = false;

			row.percent_talking = 0;
			row.neg_persuasion_reject = 0.0;
			row.neg_persuasion_post_use = 0.0;
			row.neg_persuasion_pre_use = 0.0;

			Data.network_parameter.Addnetwork_parameterRow(row);

			return row;
		}


		public MrktSimDBSchema.segment_networkRow CreateSegmentNetwork(
			MrktSimDBSchema.segmentRow from, 
			MrktSimDBSchema.segmentRow to, 
			MrktSimDBSchema.network_parameterRow param)
		{
			// check if a relation already exists

			string query = "from_id = " + from.segment_id + 
				" AND to_id = " + to.segment_id + " AND network_param = " + param.id;

			DataRow[] rows = Data.segment_network.Select(query,"",DataViewRowState.CurrentRows);

			if (rows.Length > 0 )
				return (MrktSimDBSchema.segment_networkRow) rows[0];

			MrktSimDBSchema.segment_networkRow row = Data.segment_network.Newsegment_networkRow();

			row.model_id = ModelID;
			row.from_id = from.segment_id;
			row.to_id = to.segment_id;
			row.network_param = param.id;

			Data.segment_network.Addsegment_networkRow(row);

			return row;
		}



		public MrktSimDBSchema.external_dataRow CreateExternalData(
			DateTime date,
			int product_id, int segment_id, int channel_id, int type)
		{
			// check if data exists at that time already exists

			string query = "product_id = " + product_id +
				" AND segment_id = " + segment_id +
				" AND channel_id = " + channel_id +
				" AND type = " + type +
				" AND calendar_date = #" + date.ToShortDateString() + "#";

			DataRow[] rows = Data.external_data.Select(query,"",DataViewRowState.CurrentRows);

			if (rows.Length > 0 )
				return (MrktSimDBSchema.external_dataRow) rows[0];

			MrktSimDBSchema.external_dataRow row = Data.external_data.Newexternal_dataRow();

			row.model_id = ModelID;
			row.product_id = product_id;
			row.segment_id = segment_id;
			row.channel_id = channel_id;
			row.type = type;
			row.calendar_date = date;

			// default
			row.value = 0.0;

			Data.external_data.Addexternal_dataRow(row);

			return row;
		}

	
		#endregion

	
		#region private Object Creation Routines

		

		private MrktSimDBSchema.segment_channelRow CreateSegmentChannel(int segmentID, int channelID)
		{
            if (segmentID == AllID)
                return null;

            if (channelID == AllID)
                return null;


            double prob = 100.0;

			MrktSimDBSchema.segment_channelRow row = Data.segment_channel.Newsegment_channelRow();

			row.model_id = ModelID;

			row.segment_id = segmentID;
			row.channel_id = channelID;
            row.probability_of_choice = prob.ToString();

			Data.segment_channel.Addsegment_channelRow(row);

			return row;
		}

        private MrktSimDBSchema.share_pen_brand_awareRow CreateInitial(MrktSimDBSchema.productRow prod, int segmentID)
        {
            if (prod.brand_id != 1)
                return null;

			// initial values table
			MrktSimDBSchema.share_pen_brand_awareRow initRow = Data.share_pen_brand_aware.Newshare_pen_brand_awareRow();

			initRow.model_id = ModelID;
            initRow.product_id = prod.product_id;
			initRow.segment_id = segmentID;
			initRow.initial_share = 100;
			initRow.penetration = 100;
			initRow.brand_awareness = 100;
			initRow.persuasion = 0;

			Data.share_pen_brand_aware.Addshare_pen_brand_awareRow(initRow);

			return initRow;
		}


		private MrktSimDBSchema.task_product_factRow createTaskProductEntry(int prodID, int taskID)
		{
			MrktSimDBSchema.task_product_factRow row = Data.task_product_fact.Newtask_product_factRow();

			row.model_id = ModelID;
			row.product_id = prodID;
			row.task_id = taskID;
			row.post_use_upsku = 1; 
			row.pre_use_upsku = 1;
			row.suitability = 1;
			
			Data.task_product_fact.Addtask_product_factRow(row);

			return row;
		}

		private MrktSimDBSchema.task_rate_factRow createTaskSegmentRate(int segID, int taskID)
		{
			MrktSimDBSchema.task_rate_factRow row = Data.task_rate_fact.Newtask_rate_factRow();

			row.model_id = ModelID;
			row.segment_id = segID;
			row.task_id = taskID;
			row.start_date = StartDate;
			row.end_date = EndDate;
			row.time_period = "Month";
			row.task_rate = 1;

			Data.task_rate_fact.Addtask_rate_factRow(row);

			return row;
		}

		#endregion

		#region Copy operations

        /// <summary>
        /// Makes a shallow copy of the scenario.  The copy references the same market plans as the original.  The copy 
        /// is performed immediately in the database, unlike CopyScenario( row, name ) which makes in-memory changes only.
        /// </summary>
        /// <param name="orig"></param>
        public void CopyScenario(MrktSimDBSchema.scenarioRow orig)
        {
            // get a new name for scenario
            MrktSimDBSchema.scenarioRow scenario = CreateStandardScenario(orig.name);

            scenario.descr = orig.descr;

            scenario.user_name = orig.user_name;

            Update();

            // copy marketing plan references
            genericCommand.CommandText = "INSERT INTO scenario_market_plan SELECT " + scenario.scenario_id +
                " AS scenario_id, market_plan_id, model_id FROM scenario_market_plan WHERE scenario_id = " + orig.scenario_id;

            Connection.Open();

            genericCommand.ExecuteNonQuery();

            Connection.Close();
        }

        /// <summary>
        /// Copies a plan.  If the given plan is a component plan, then the component data (all rows) is copied is well as the component row itself.
        /// If the given plan is a market plan, then the copied plan references the same components as the original.
        /// </summary>
        /// <returns></returns>
        public MrktSimDBSchema.market_planRow CopyPlan( MrktSimDBSchema.market_planRow plan ) {
            return CopyPlan( plan, false );
        }

        /// <summary>
        /// Copies a plan.  If the given plan is a component plan, then the component data (all rows) is copied is well as the component row itself.
        /// If the given plan is a market plan, then the copied plan references the same components as the original.
        /// </summary>
        /// <returns></returns>
        public MrktSimDBSchema.market_planRow CopyPlan( MrktSimDBSchema.market_planRow plan, bool copyComponentsAlso ) {
            MrktSimDBSchema.market_planRow newPlan = CreateMarketPlan( plan.name, (PlanType)plan.type );
			
			newPlan.descr = "Copy of " + plan.name;
			
			newPlan.product_id = plan.product_id;
			newPlan.channel_id = plan.channel_id;
			newPlan.segment_id = plan.segment_id;
			newPlan.task_id = plan.task_id;
			newPlan.type = plan.type;

			// now make sure any referenced data is copied as well
			// we could put a switch on plan type here if we wanted to speed things up mnutely
			
			// price data
			copyPriceData(plan.id, newPlan.id);
			copyDisplayData(plan.id, newPlan.id);
			copyDistributionData(plan.id, newPlan.id);
			copyMediaData(plan.id, newPlan.id);
			copyTaskEventData(plan.id, newPlan.id);
            copyProdTaskData( plan.id, newPlan.id );
            copyMarketUtilityData( plan.id, newPlan.id );
            if( plan.type == 0 ) {
                copyRelations( plan, newPlan, copyComponentsAlso );          // has no effect unless plan is top-level plan
            }

			return newPlan;
		}

        // create new relations in marketing plan tree
        private void copyRelations( MrktSimDBSchema.market_planRow orig, MrktSimDBSchema.market_planRow copy ) {
            copyRelations( orig, copy, false );
        }

		// create new relations in marketing plan tree, possibly copying the component data also
		private void copyRelations(MrktSimDBSchema.market_planRow orig, MrktSimDBSchema.market_planRow copy, bool copyComponents)
		{
			if (orig.id == copy.id)
				return;

			string query = "parent_id = " + orig.id;

			DataRow[] rows = Data.market_plan_tree.Select(query,"",DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				MrktSimDBSchema.market_planRow comp = Data.market_plan.FindByid((int) row["child_id"]);

                if( copyComponents ) {
                    MrktSimDBSchema.market_planRow copiedComp = CopyPlan( comp, false );
                    comp = copiedComp;
                }

				CreatePlanRelation(copy, comp);
			}

		}

		private void copyPriceData(int origID, int newID)
		{
			string query = "market_plan_id = " + origID;

			DataRow[] rows = Data.product_channel.Select(query, "", DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				MrktSimDBSchema.product_channelRow newRow = Data.product_channel.Newproduct_channelRow();
				foreach(DataColumn col in Data.product_channel.Columns)
				{
					if (col.ColumnName == "record_id")
						continue;

					if (col.ColumnName == "market_plan_id")
					{
						newRow[col] = newID;
						continue;
					}

					newRow[col] = row[col];
				}

				Data.product_channel.Addproduct_channelRow(newRow);
			}
		}

		private void copyDistributionData(int origID, int newID)
		{
			string query = "market_plan_id = " + origID;

			DataRow[] rows = Data.distribution.Select(query, "", DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				MrktSimDBSchema.distributionRow newRow = Data.distribution.NewdistributionRow();
				foreach(DataColumn col in Data.distribution.Columns)
				{
					if (col.ColumnName == "record_id")
						continue;

					if (col.ColumnName == "market_plan_id")
					{
						newRow[col] = newID;
						continue;
					}

					newRow[col] = row[col];
				}

				Data.distribution.AdddistributionRow(newRow);
			}
		}

		private void copyDisplayData(int origID, int newID)
		{
			string query = "market_plan_id = " + origID;

			DataRow[] rows = Data.display.Select(query, "", DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				MrktSimDBSchema.displayRow newRow = Data.display.NewdisplayRow();
				foreach(DataColumn col in Data.display.Columns)
				{
					if (col.ColumnName == "record_id")
						continue;

					if (col.ColumnName == "market_plan_id")
					{
						newRow[col] = newID;
						continue;
					}

					newRow[col] = row[col];
				}

				Data.display.AdddisplayRow(newRow);
			}
		}

		private void copyMarketUtilityData(int origID, int newID)
		{
			string query = "market_plan_id = " + origID;

			DataRow[] rows = Data.market_utility.Select(query, "", DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				MrktSimDBSchema.market_utilityRow newRow = Data.market_utility.Newmarket_utilityRow();
				foreach(DataColumn col in Data.market_utility.Columns)
				{
					if (col.ColumnName == "record_id")
						continue;

					if (col.ColumnName == "market_plan_id")
					{
						newRow[col] = newID;
						continue;
					}

					newRow[col] = row[col];
				}

				Data.market_utility.Addmarket_utilityRow(newRow);
			}
		}

		private void copyMediaData(int origID, int newID)
		{
			string query = "market_plan_id = " + origID;

			DataRow[] rows = Data.mass_media.Select(query, "", DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				MrktSimDBSchema.mass_mediaRow newRow = Data.mass_media.Newmass_mediaRow();
				foreach(DataColumn col in Data.mass_media.Columns)
				{
					if (col.ColumnName == "record_id")
						continue;

					if (col.ColumnName == "market_plan_id")
					{
						newRow[col] = newID;
						continue;
					}

					newRow[col] = row[col];
				}

				Data.mass_media.Addmass_mediaRow(newRow);
			}
		}

		private void copyTaskEventData(int origID, int newID)
		{
			string query = "market_plan_id = " + origID;

			DataRow[] rows = Data.task_event.Select(query, "", DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				MrktSimDBSchema.task_eventRow newRow = Data.task_event.Newtask_eventRow();
				foreach(DataColumn col in Data.task_event.Columns)
				{
					if (col.ColumnName == "record_id")
						continue;

					if (col.ColumnName == "market_plan_id")
					{
						newRow[col] = newID;
						continue;
					}

					newRow[col] = row[col];
				}

				Data.task_event.Addtask_eventRow(newRow);
			}
		}

		private void copyProdTaskData(int origID, int newID)
		{
			string query = "market_plan_id = " + origID;

			DataRow[] rows = Data.product_event.Select(query, "", DataViewRowState.CurrentRows);

			foreach(DataRow row in rows)
			{
				MrktSimDBSchema.product_eventRow newRow = Data.product_event.Newproduct_eventRow();
				foreach(DataColumn col in Data.product_event.Columns)
				{
					if (col.ColumnName == "record_id")
						continue;

					if (col.ColumnName == "market_plan_id")
					{
						newRow[col] = newID;
						continue;
					}

					newRow[col] = row[col];
				}

				Data.product_event.Addproduct_eventRow(newRow);
			}
		}


		#endregion

        #region model parameters
        public MrktSimDBSchema.model_parameterRow CreatePlanParameter(MrktSimDBSchema.market_planRow mrktPlan, int index, string userName)
        {
            // create parameters for plan these are predetermined
            string parmName = "parm" + index;

            // check if parameter already exists
            MrktSimDBSchema.model_parameterRow parm = ModelParameterExists(mrktPlan, parmName, "id");

            if (parm == null)
            {
                parm = CreateModelParameter(this.ModelID, mrktPlan, parmName, "id");
                parm.name = userName;
            }

            return parm;
        }

        #endregion

     
    }
}