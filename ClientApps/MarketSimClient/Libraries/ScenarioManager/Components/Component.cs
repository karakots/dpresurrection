using System;
using System.Collections;
using System.Text;
using System.Data;

using MrktSimDb;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary.Components
{
    public class Component
    {
        protected Model model;

        protected ArrayList marketPlansList;

        protected MrktSimDBSchema.market_planRow componentRow;

        protected Product product;
        protected Segment segment;
        protected Channel channel;

        protected DataTable dataTable;

        protected ModelDb theDb;

        protected static double defaultAwareness = 0.2;       // these values are reset in the speicific subclasses
        protected static double defaultPersuasion = 0.3;

        /// <summary>
        /// Default awareness for this component
        /// </summary>
        public static double DefaultAwareness {
            get {
                return defaultAwareness;
            }
        }

        /// <summary>
        /// Default persuasion for this component
        /// </summary>
        public static double DefaultPersuasion {
            get {
                return defaultPersuasion;
            }
        }

        /// <summary>
        /// The name of this component
        /// </summary>
        public string Name {
            get {
                return componentRow.name;
            }
            set {
                componentRow.name = value;
            }
        }

        /// <summary>
        /// The description of this component
        /// </summary>
        public string Description {
            get {
                return componentRow.descr;
            }
            set {
                componentRow.descr = value;
            }
        }

        /// <summary>
        /// The product for this component
        /// </summary>
        public Product Product {
            get {
                return model.ProductForID( componentRow.product_id );
            }
            set {
                componentRow.product_id = value.ID;
            }
        }

        /// <summary>
        /// The segment for this component
        /// </summary>
        public Segment Segment {
            get {
                return segment;
            }
            set {
                this.segment = value;
                this.componentRow.segment_id = this.segment.ID;
                theDb.UpdateMarketPlan( this.componentRow, 0 );
            }
        }

        /// <summary>
        /// The channel for this component
        /// </summary>
        public Channel Channel {
            get {
                return channel;
            }
            set {
                this.channel = value;
                this.componentRow.channel_id = this.channel.ID;
                theDb.UpdateMarketPlan( this.componentRow, 0 );
            }
        }

        /// <summary>
        /// Start date for this component.  Reflects the data items currently in the component.
        /// </summary>
        public DateTime StartDate {
            get {
                return componentRow.start_date;
            }
            set {
                componentRow.start_date = value;
            }
        }

        /// <summary>
        /// End date for this component.  Reflects the data items currently in the component.
        /// </summary>
        public DateTime EndDate {
            get {
                return componentRow.end_date;
            }
            set {
                componentRow.end_date = value;
            }
        }

        /// <summary>
        /// The model that contains this component
        /// </summary>
        public Model Model {
            get {
                return model;
            }
        }

        /// <summary>
        /// The internal ID of this component.  For framework usage only.
        /// </summary>
        public int ID {
            get {
                return componentRow.id;
            }
        }

        /// <summary>
        /// The database row associated with this component.  For framework usage only.
        /// </summary>
        public MrktSimDBSchema.market_planRow ComponentRow {
            get {
                return componentRow;
            }
        }

        /// <summary>
        /// The market plans that are using  this component
        /// </summary>
        public ArrayList MarketPlansList {
            get {
                return marketPlansList;
            }
            //set {
            //    marketPlansList = value;
            //}
        }

        /// <summary>
        /// The market plans that are using  this component
        /// </summary>
        public MarketPlan[] MarketPlans {
            get {
                // copy from the master list
                MarketPlan[] plansArray = new MarketPlan[ marketPlansList.Count ];
                marketPlansList.CopyTo( plansArray );

                // put the array in alphabetic order by name
                string[] planNames = new string[ plansArray.Length ];
                for( int i = 0; i < plansArray.Length; i++ ) {
                    planNames[ i ] = plansArray[ i ].Name;
                }
                Array.Sort( planNames, plansArray );

                return plansArray;
            }
        }

        /// <summary>
        /// The table that contains the data for this component
        /// </summary>
        public DataTable DataTable {
            get {
                return dataTable;
            }
            set {
                dataTable = value;
            }
        }

        /// <summary>
        /// Constructs a new Component object.  For framework usage only.
        /// </summary>
        /// <param name="marketPlan">Market plan that contains the component</param>
        /// <param name="componentRow">Internal representation of the component</param>
        public Component( Model model, MrktSimDBSchema.market_planRow componentRow ) {
            this.componentRow = componentRow;
            this.model = model;
            this.theDb = model.ModelDb;

            if( componentRow != null ) {
                this.product = Model.ProductForID( componentRow.product_id );
                this.channel = Model.ChannelForID( componentRow.channel_id );
                this.segment = Model.SegmentForID( componentRow.segment_id );
            }

            this.marketPlansList = new ArrayList();
        }

        /// <summary>
        /// Deletes all of the data for the component.
        /// </summary>
        /// <returns></returns>
        public int DeleteAllData() {
            string deleteQuery = String.Format( "model_id = {0} AND market_plan_id = {1}", this.model.ID, this.ID );

            DataRow[] rowsToDelete = this.dataTable.Select( deleteQuery );
            int delCount = rowsToDelete.Length;
            for( int i = 0; i < delCount; i++ ) {
                rowsToDelete[ i ].Delete();
            }
            return delCount;
        }

        /// <summary>
        /// Deletes all component data in the given time span.  The dateCheckType controls whether the start point, end point, or full time range of a data item
        /// must be in the given range for that item to be deleted.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>The number of data component entries (database rows) that were deleted</returns>
        public int DeleteData( DateTime start, DateTime end, Enums.DataIntervalCheckType dataCheckType ) {
            return DeleteData( start, end, dataCheckType, Channel.All );
        }

        /// <summary>
        /// Deletes all component data in the given time span.  The dateCheckType controls whether the start point, end point, or full time range of a data item
        /// must be in the given range for that item to be deleted.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>The number of data component entries (database rows) that were deleted</returns>
        public int DeleteData( DateTime start, DateTime end, Enums.DataIntervalCheckType dataCheckType, Channel channel ) {

            string deleteQuery = DataQuery( start, end, dataCheckType, channel );

            DataRow[] rowsToDelete = this.dataTable.Select( deleteQuery );
            int delCount = rowsToDelete.Length;
            for( int i = 0; i < delCount; i++ ) {
                rowsToDelete[ i ].Delete();
            }
            return delCount;
        }

        /// <summary>
        /// Returns a copy of the component.
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="newDescription"></param>
        /// <returns></returns>
        public Component Copy( string newComponentName, string newDescription ) {
            MrktSimDBSchema.market_planRow copyRow = this.model.ModelDb.CopyPlan( this.componentRow );
            copyRow.name = newComponentName;
            copyRow.descr = newDescription;

            Component copyComp = new Component( this.model, copyRow );

            this.model.RefreshAllUserItems();

            return copyComp;
        }

        /// <summary>
        /// Changes the product of the component.  
        /// </summary>
        /// <param name="newProduct"></param>
        public void ChangeProduct( Product newProduct ) {
            this.componentRow.product_id = newProduct.ID;

            this.model.ModelDb.UpdateMarketPlan( this.componentRow, 0 );

            this.model.RefreshAllUserItems();
        }

        /// <summary>
        /// The query (filter) string that is used for getting this component's data.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="dataCheckType"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        protected string DataQuery( DateTime start, DateTime end, Enums.DataIntervalCheckType dataCheckType, Channel channel ) {
            string query = String.Format( "model_id = {0} AND market_plan_id = {1}", model.ID, this.componentRow.id );

            if( dataCheckType == Enums.DataIntervalCheckType.DataEndsInInterval || dataCheckType == Enums.DataIntervalCheckType.DataEntirelyWithinInterval ) {
                query += String.Format( " AND end_date >= '{0}' AND end_date <= '{1}'", start.ToShortDateString(), end.ToShortDateString() );
            }

            if( dataCheckType == Enums.DataIntervalCheckType.DataStartsInInterval || dataCheckType == Enums.DataIntervalCheckType.DataEntirelyWithinInterval ) {
                query += String.Format( " AND start_date >= '{0}' AND start_date <= '{1}'", start.ToShortDateString(), end.ToShortDateString() );
            }

            if( channel.ID != -1 ) {
                query += String.Format( " AND channel_id = {0}", channel.ID );
            }
            return query;
        }
    }
}
