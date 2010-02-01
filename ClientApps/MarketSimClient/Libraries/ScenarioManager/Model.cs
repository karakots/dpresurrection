using System;
using System.Collections;
using System.Text;

using MrktSimDb;
using DecisionPower.MarketSim.ScenarioManagerLibrary.Components;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary
{
    public class Model
    {
        public class PriceType
        {
            public PriceType( MrktSimDBSchema.price_typeRow aRow )
            {
                row = aRow;
            }

            public string Name
            {
                get
                {
                    return row.name;
                }
            }

            public int BOGN
            {
                get
                {
                    return row.BOGN;
                }
            }

            public bool Relative
            {
                get
                {
                    return row.relative;
                }
            }

            public bool Absolute
            {
                get
                {
                    return !row.relative;
                }
            }

            public int Id
            {
                get
                {
                    return row.id;
                }
            }

            private MrktSimDBSchema.price_typeRow row;
        }

        // the project that contains this model
        private Project project;

        private bool isOpen;

        // these items are exposed for read only
        private Product[] products;
        private ProductType[] productTypes;
        private Segment[] segments;
        private Channel[] channels;
        private PriceType[] priceTypes = null;

        private MrktSimDBSchema.Model_infoRow modelRow;

        private ArrayList scenariosList;

        // we maintain these master lists of items here since they have dynamic (potentially empty) sets of owners in the main hierarchy
        private ArrayList allMarketPlansList;
        private ArrayList allComponentsList;

        private ModelDb thelDb;

        /// <summary>
        /// The name of this model
        /// </summary>
        public string Name {
            get {
                return modelRow.model_name;
            }
        }

        /// <summary>
        /// The description of this scenario
        /// </summary>
        public string Description {
            get {
                return modelRow.descr;
            }
        }

        /// <summary>
        /// True if the model is already open by another process or user.  Model can be opened but for read only.
        /// </summary>
        public bool Locked {
            get {
                return modelRow.locked;
            }
        }

        /// <summary>
        /// If true, the model is open for read only
        /// </summary>
        public bool ReadOnly {
            get {
                return modelRow.read_only;
            }
        }

        /// <summary>
        /// The products in this model
        /// </summary>
        public Product[] Products {
            get {
                if( isOpen ) {
                    return products;
                }
                else {
                    throw new MrktSimException( "Model not open: " + Name, "Model must be open for editing when accessing the Products property." );
                }
            }
        }

        public PriceType[] PriceTypes
        {
            get
            {
                if( isOpen )
                {
                    if (priceTypes == null)
                    {
                        MrktSimDBSchema.price_typeRow[] rows = (MrktSimDBSchema.price_typeRow[]) thelDb.Data.price_type.Select();

                        priceTypes = new PriceType[rows.Length];

                        int index = 0;
                        foreach(MrktSimDBSchema.price_typeRow row in rows)
                        {
                            priceTypes[index++] = new PriceType(row);
                        }
                    }

                    return priceTypes;
                }
                else
                {
                    throw new MrktSimException( "Model not open: " + Name, "Model must be open for editing when accessing the PriceType property." );
                }
            }
        }

        /// <summary>
        /// Products of the given type
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        public Product[] GetProducts( ProductType productType ) {
            ArrayList plist = new ArrayList();
            foreach( Product p in products ) {
                if( p.ProductType.ID == productType.ID ) {
                    plist.Add( p );
                }
            }
            Product[] retItems = new Product[ plist.Count ];
            plist.CopyTo( retItems );
            return retItems;
        }

        /// <summary>
        /// The leaf (SKU/variant) products (does not include brands, companies, etc.)
        /// </summary>
        /// <returns></returns>
        public Product[] GetLeafProducts() {
            ArrayList plist = new ArrayList();
            foreach( Product p in products ) {
                if( p.IsLeaf ) {
                    plist.Add( p );
                }
            }
            Product[] retItems = new Product[ plist.Count ];
            plist.CopyTo( retItems );
            return retItems;
        }

        /// <summary>
        /// The channels in this model
        /// </summary>
        public Channel[] Channels {
            get {
                if( isOpen ) {
                    return channels;
                }
                else {
                    throw new MrktSimException( "Model not open: " + Name, "Model must be open for editing when accessing the Channels property." );
                }
            }
        }

        /// <summary>
        /// The segments in this model
        /// </summary>
        public Segment[] Segments {
            get {
                if( isOpen ) {
                    return segments;
                }
                else {
                    throw new MrktSimException( "Model not open: " + Name, "Model must be open for editing when accessing the Segments property." );
                }
            }
        }

        /// <summary>
        /// The Price Typesin this model
        /// </summary>
        public ProductType[] ProductTypes {
            get {
                if( isOpen ) {
                    return productTypes;
                }
                else {
                    throw new MrktSimException( "Model not open: " + Name, "Model must be open for editing when accessing the ProductTypes property." );
                }
            }
        }

        /// <summary>
        /// The segments in this model
        /// </summary>
        public Scenario[] Scenarios {
            get {
                if( isOpen ) {
                    // copy from the master list
                    Scenario[] scenariosArray = new Scenario[ scenariosList.Count ];
                    scenariosList.CopyTo( scenariosArray );

                    // put the array in alphabetic order by name
                    string[] scenarioNames = new string[ scenariosArray.Length ];
                    for( int i = 0; i < scenariosArray.Length; i++ ) {
                        scenarioNames[ i ] = scenariosArray[ i ].Name;
                    }
                    Array.Sort( scenarioNames, scenariosArray );

                    return scenariosArray;
                }
                else {
                    throw new MrktSimException( "Model not open: " + Name, "Model must be open for editing when accessing the Scenarios property." );
                }
            }
        }

        /// <summary>
        /// The Project that this model belongs to. 
        /// </summary>
        public Project Project {
            get {
                return project;
            }
        }

        /// <summary>
        /// The ModelDb instance that this model belongs to.  For framework usage only.
        /// </summary>
        public ModelDb ModelDb {
            get {
                return thelDb;
            }
        }

        /// <summary>
        /// The internal ID of this object.  For framework usage only.
        /// </summary>
        public int ID {
            get {
                return modelRow.model_id; 
            }
        }

        /// <summary>
        /// The database row that this object represents.  For framework usage only.
        /// </summary>
        public MrktSimDBSchema.Model_infoRow ModelRow {
            get {
                return modelRow;
            }
        }

        /// <summary>
        /// Constructs a new Model object.  For framework usage only.
        /// </summary>
        /// <param name="database">Project that contains the model</param>
        /// <param name="modelRow">Internal representation of the model</param>
        public Model( Project project, MrktSimDBSchema.Model_infoRow modelRow ) {
            this.project = project;
            this.modelRow = modelRow;

            this.scenariosList = new ArrayList();
            this.allMarketPlansList = new ArrayList();
            this.allComponentsList = new ArrayList();
        }

        /// <summary>
        /// Creates a new empty Scenario in the model.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public Scenario NewScenario( string name, string description ){
            Scenario newScenario = null;
            MrktSimDBSchema.scenarioRow newRow = thelDb.CreateStandardScenario( name );
            if( newRow != null ) {
                newRow.descr = description;
                newScenario = new Scenario( this, newRow );
            }
            scenariosList.Add( newScenario );   //there are no subitems so, we don't need to call RefreshSubItems()
            return newScenario;
        }

        /// <summary>
        /// Opens the model for editing (makes an in-memory copy of the database for the model).  
        /// Use SaveAllChanges() to commit changes to the master database.
        /// </summary>
        public void OpenForEditing() {
            thelDb = new ModelDb();
            thelDb.Connection = project.Database.DBConnection;
            thelDb.ModelID = this.modelRow.model_id;

            string who;
            thelDb.Open(out who);
            thelDb.Refresh();

            RefreshReadOnlyItems();
            RefreshAllUserItems();

            this.isOpen = true;
        }

        /// <summary>
        /// Commits any changes made to the model to the master database.
        /// </summary>
        public void SaveAllChanges() {
            if( isOpen == false ) {
                throw new MrktSimException( "Model not open: " + Name, "Model must be open for editing when calliing SaveAllChanges()." );
            }

            thelDb.Update();
        }

        /// <summary>
        /// Closes the model.  If saveChanges is true, commits any changes made to the model to the master database.
        /// </summary>
        public void Close( bool saveChanges ) {
            if( isOpen == false ) {
                return;
            }
            
            if( saveChanges ) {
                SaveAllChanges();
            }
            thelDb.Close();

            this.isOpen = false;
        }

        /// <summary>
        /// Returns the channel with the given ID.   For framework usage only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Channel ChannelForID( int id ) {
            foreach( Channel testChan in this.channels ) {
                if( testChan.ID == id ) {
                    return testChan;
                }
            }
            // ID not found
            return null;
        }

        /// <summary>
        /// Returns the segment with the given ID.   For framework usage only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Segment SegmentForID( int id ) {
            foreach( Segment testSeg in this.segments ) {
                if( testSeg.ID == id ) {
                    return testSeg;
                }
            }
            // ID not found
            return null;
        }

        public PriceType PriceTypeForID( int id )
        {
            foreach( PriceType priceType in priceTypes )
            {
                if( priceType.Id == id )
                {
                    return priceType;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the product with the given ID.   For framework usage only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product ProductForID( int id ) {
            foreach( Product testProd in this.products ) {
                if( testProd.ID == id ) {
                    return testProd;
                }
            }
            // ID not found
            return null;
        }

        /// <summary>
        /// Returns the product type with the given ID.   For framework usage only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductType ProductTypeForID( int id ) {
            foreach( ProductType testProdTyp in this.productTypes ) {
                if( testProdTyp.ID == id ) {
                    return testProdTyp;
                }
            }
            // ID not found
            return null;
        }

        /// <summary>
        /// Returns the product type with the given name.   For framework usage only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductType ProductTypeForName( string name ) {
            foreach( ProductType testProdTyp in this.productTypes ) {
                if( testProdTyp.Name == name ) {
                    return testProdTyp;
                }
            }
            // ID not found
            return null;
        }

        /// <summary>
        /// Returns the scenario with the given ID.   For framework usage only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Scenario ScenarioForID( int id ) {
            foreach( Scenario testScenario in this.scenariosList ) {
                if( testScenario.ID == id ) {
                    return testScenario;
                }
            }
            // ID not found
            return null;
        }

        /// <summary>
        /// Returns the market plan with the given ID.   For framework usage only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MarketPlan MarketPlanForID( int id ) {
            foreach( MarketPlan testPlan in this.allMarketPlansList ) {
                if( testPlan.ID == id ) {
                    return testPlan;
                }
            }
            // ID not found
            return null;
        }

        /// <summary>
        /// Returns the component with the given ID.   For framework usage only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Component ComponentForID( int id ) {
            foreach( Component testComp in this.allComponentsList ) {
                if( testComp.ID == id ) {
                    return testComp;
                }
            }
            // ID not found
            return null;
        }

        /// <summary>
        /// Adds the market plan to the master list of plans in this model.
        /// </summary>
        /// <param name="planToAdd"></param>
        /// <returns>true if this was a new irtem; false if the model already has this plan (ID)</returns>
        /// <remarks>Normally you need to call RefreshAllUserItems() just after this method.</remarks>
        public bool AddPlanToMasterList( MarketPlan planToAdd ) {
            if( MarketPlanForID( planToAdd.ID ) != null ) {
                this.allMarketPlansList.Add( planToAdd );
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Adds the component to the master list of components in this model.
        /// </summary>
        /// <param name="planToAdd"></param>
        /// <returns>true if this was a new irtem; false if the model already has this component (ID)</returns>
        /// <remarks>Normally you need to call RefreshAllUserItems() just after this method.</remarks>
        public bool AddComponentToMasterList( MarketPlan planToAdd ) {
            if( MarketPlanForID( planToAdd.ID ) != null ) {
                this.allMarketPlansList.Add( planToAdd );
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Deletes this scenario from the model.  Note that this does not delete the associated market plans, as they may be used by other scenarios.
        /// </summary>
        public void DeleteScenario( Scenario scenarioToDelete ) {

            for( int i = 0; i < this.scenariosList.Count; i++ ) {
                Scenario testScen = (Scenario)scenariosList[ i ];
                if( testScen.ID == scenarioToDelete.ID ) {
                    this.scenariosList.RemoveAt( i );
                    break;
                }
            }

            scenarioToDelete.ScenarioRow.Delete();
            RefreshAllUserItems();
        }

        /// <summary>
        /// Deletes this market plan from the model.  If deleteComponentsAlso is true, the components of this market plan
        /// will be deleted regardless of their possible utilization by other market plans.
        /// </summary>
        /// <param name="marketPlanToDelete"></param>
        /// <param name="deleteComponentsAlso"></param>
        public void DeleteMarketPlan( MarketPlan marketPlanToDelete, bool deleteComponentsAlso ) {
            if( deleteComponentsAlso ) {
                foreach( Component compToDelete in marketPlanToDelete.Components ) {
                    DeleteComponent( compToDelete );
                }
            }

            // delete the tree-link items pertaining to this plan as well as the plan
            string scenarioTreeQuery = String.Format( "market_plan_id = {0}", marketPlanToDelete.ID );
            MrktSimDBSchema.scenario_market_planRow[] scenarioTreeRows = (MrktSimDBSchema.scenario_market_planRow[])this.ModelDb.Data.scenario_market_plan.Select( scenarioTreeQuery );
            string planTtreeQuery = String.Format( "parent_id = {0}", marketPlanToDelete.ID );
            MrktSimDBSchema.market_plan_treeRow[] planTreeRows = (MrktSimDBSchema.market_plan_treeRow[])this.ModelDb.Data.market_plan_tree.Select( planTtreeQuery );

            foreach( MrktSimDBSchema.scenario_market_planRow scenarioTreeRow in scenarioTreeRows ) {
                scenarioTreeRow.Delete();
            }
            foreach( MrktSimDBSchema.market_plan_treeRow planTreeRow in planTreeRows ) {
                planTreeRow.Delete();
            }

            for( int i = 0; i < this.allMarketPlansList.Count; i++ ) {
                MarketPlan testPlan = (MarketPlan)allMarketPlansList[ i ];
                if( testPlan.ID == marketPlanToDelete.ID ) {
                    this.allMarketPlansList.RemoveAt( i );
                    break;
                }
            }

            marketPlanToDelete.PlanRow.Delete();
            RefreshAllUserItems();
        }

        /// <summary>
        /// Deletes the component form the model.
        /// </summary>
        /// <param name="componentToDelete"></param>
        public void DeleteComponent( Component componentToDelete ) {
            componentToDelete.DeleteAllData();

            // delete the tree-link items pertaining to this plan as well as the plan
            string planTtreeQuery = String.Format( "child_id = {0}", componentToDelete.ID );
            MrktSimDBSchema.market_plan_treeRow[] planTreeRows = (MrktSimDBSchema.market_plan_treeRow[])this.ModelDb.Data.market_plan_tree.Select( planTtreeQuery );
            foreach( MrktSimDBSchema.market_plan_treeRow planTreeRow in planTreeRows ) {
                planTreeRow.Delete();
            }

            for( int i = 0; i < this.allComponentsList.Count; i++ ) {
                Component testComp = (Component)allComponentsList[ i ];
                if( testComp.ID == componentToDelete.ID ) {
                    this.allComponentsList.RemoveAt( i );
                    break;
                }
            }

            componentToDelete.ComponentRow.Delete();
            RefreshAllUserItems();
        }

        /// <summary>
        /// Updates the Scenarios for this model, the MarketPlans for each scenario, and the Components for each of the market plans
        /// so that all user items reflect the current state of the ModelDb data.
        /// </summary>
        /// <remarks>This isn't as simple as just clearing and reloading the lists -- the same objects need to be preserved unless there
        /// are really items added/removed in the DB.  Otherwise an app using the objects could lose references.</remarks>
        public void RefreshAllUserItems() {

            RefreshScenariosList();
            RefreshMasterMarketPlansList();
            RefreshMasterComponentsList();

            RefreshLinkages();
        }

        /// <summary>
        /// Refreshes scenariosList to reflect the items currently in the ModelDb
        /// </summary>
        private void RefreshScenariosList() {
            string scenarioQuery = "";
            // get the current scenarios form the ModelDb
            MrktSimDBSchema.scenarioRow[] scenarioRows = (MrktSimDBSchema.scenarioRow[])thelDb.Data.scenario.Select( scenarioQuery,
                                                                                                                          null, System.Data.DataViewRowState.CurrentRows );
            bool[] rowIsCurrent = new bool[ scenarioRows.Length ];

            for( int r = 0; r < scenarioRows.Length; r++ ) {
                rowIsCurrent[ r ] = false;
            }
            int okRows = 0;

            // check each scenario 
            for( int s = this.scenariosList.Count - 1; s >= 0; s-- ) {

                int id = ((Scenario)scenariosList[ s ]).ID;

                // see if we have a matching row for this object
                bool foundMatch = false;
                for( int r = 0; r < scenarioRows.Length; r++ ) {
                    if( scenarioRows[ r ].scenario_id == id ) {
                        rowIsCurrent[ r ] = true;
                        okRows += 1;
                        foundMatch = true;
                        break;
                    }
                }

                if( foundMatch == false ) {
                    // this scenario no longer has a row in the model
                    scenariosList.RemoveAt( s );
                }
            }

            // items to add?
            if( scenarioRows.Length != okRows ) {
                for( int r = 0; r < scenarioRows.Length; r++ ) {
                    if( rowIsCurrent[ r ] == false ) {
                        Scenario addedScenario = new Scenario( this, scenarioRows[ r ] );
                        scenariosList.Add( addedScenario );
                    }
                }
            }
        }

        /// <summary>
        /// Refreshes allMarketPlansList to reflect the items currently in the ModelDb
        /// </summary>
        private void RefreshMasterMarketPlansList() {
            RefreshMasterPlansList( "type = 0", allMarketPlansList );
        }

        /// <summary>
        /// Refreshes allComponentsList to reflect the items currently in the ModelDb
        /// </summary>
        private void RefreshMasterComponentsList() {
            RefreshMasterPlansList( "type <> 0", allComponentsList );
        }

        /// <summary>
        /// Refreshes the given list to reflect the items currently in the ModelDb market plans table
        /// </summary>
        private void RefreshMasterPlansList( string query, ArrayList list ) {

            // get the current plans form the ModelDb
            MrktSimDBSchema.market_planRow[] planRows = (MrktSimDBSchema.market_planRow[])thelDb.Data.market_plan.Select( query, 
                                                                                                        null, System.Data.DataViewRowState.CurrentRows );
            bool[] rowIsCurrent = new bool[ planRows.Length ];

            for( int r = 0; r < planRows.Length; r++ ) {
                rowIsCurrent[ r ] = false;
            }
            int okRows = 0;

            // check each scenario 
            for( int m = list.Count - 1; m >= 0; m-- ) {

                int id = -1;
                if( list[ m ] is MarketPlan ) {
                    id = ((MarketPlan)list[ m ]).ID;
                }
                else {
                    id = ((Component)list[ m ]).ID;
                }

                // see if we have a matching row for this object
                bool foundMatch = false;
                for( int r = 0; r < planRows.Length; r++ ) {
                    if( planRows[ r ].id == id ) {
                        rowIsCurrent[ r ] = true;
                        okRows += 1;
                        foundMatch = true;
                        break;
                    }
                }

                if( foundMatch == false ) {
                    // this scenario no longer has a row in the model
                    list.RemoveAt( m );
                }
            }

            // items to add?
            if( planRows.Length != okRows ) {
                for( int r = 0; r < planRows.Length; r++ ) {
                    if( rowIsCurrent[ r ] == false ) {
                        // this could be either a plan or a component
                        if( planRows[ r ].type == 0 ) {
                            MarketPlan addedPlan = new MarketPlan( this, planRows[ r ] );
                            list.Add( addedPlan );
                        }
                        else {
                            // add the appropriate type of component
                            switch( (ModelDb.PlanType)planRows[ r ].type ) {
                                case MrktSimDb.Database.PlanType.Coupons:
                                    CouponsComponent addedCComp = new CouponsComponent( this, planRows[ r ] );
                                    list.Add( addedCComp );
                                    break;
                                case MrktSimDb.Database.PlanType.Display:
                                    DisplayComponent addedDyComp = new DisplayComponent( this, planRows[ r ] );
                                    list.Add( addedDyComp );
                                    break;
                                case MrktSimDb.Database.PlanType.Distribution:
                                    DistributionComponent addedDComp = new DistributionComponent( this, planRows[ r ] );
                                    list.Add( addedDComp );
                                    break;
                                case MrktSimDb.Database.PlanType.Market_Utility:
                                    MarketUtilityComponent addedMUComp = new MarketUtilityComponent( this, planRows[ r ] );
                                    list.Add( addedMUComp );
                                    break;
                                case MrktSimDb.Database.PlanType.Media:
                                    MediaComponent addedMComp = new MediaComponent( this, planRows[ r ] );
                                    list.Add( addedMComp );
                                    break;
                                case MrktSimDb.Database.PlanType.Price:
                                    PriceComponent addedPComp = new PriceComponent( this, planRows[ r ] );
                                    list.Add( addedPComp );
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method updates the lists of related items: market plans for each scenario, scenarios and components for each plan, and plans for each component
        /// </summary>
        private void RefreshLinkages() {
            string treeQuery = "";
            // get the current linkages form the ModelDb
            MrktSimDBSchema.scenario_market_planRow[] tree1Rows = (MrktSimDBSchema.scenario_market_planRow[])thelDb.Data.scenario_market_plan.Select( treeQuery,
                                                                                                                          null, System.Data.DataViewRowState.CurrentRows );

            MrktSimDBSchema.market_plan_treeRow[] tree2Rows = (MrktSimDBSchema.market_plan_treeRow[])thelDb.Data.market_plan_tree.Select( treeQuery,
                                                                                                                          null, System.Data.DataViewRowState.CurrentRows );

            // clear all existing links
            foreach( Scenario scenario in this.scenariosList ) {
                scenario.MarketPlansList.Clear();
            }
            foreach( MarketPlan marketPlan in this.allMarketPlansList ) {
                marketPlan.ScenariosList.Clear();
                marketPlan.ComponentsList.Clear();
            }
            foreach( Component component in this.allComponentsList ) {
                component.MarketPlansList.Clear();
            }

            // process the scenario-to-market-plan links
            foreach( MrktSimDBSchema.scenario_market_planRow tree1Row in tree1Rows ) {
                int parentID = tree1Row.scenario_id;
                int childID = tree1Row.market_plan_id;

                Scenario parent = ScenarioForID( parentID );
                MarketPlan child = MarketPlanForID( childID );

                // ignore obsolete links
                if( parent == null || child == null ) {
                    continue;
                }

                if( parent.MarketPlansList.Contains( child ) == false ) {
                    parent.MarketPlansList.Add( child );
                }
                if( child.ScenariosList.Contains( parent ) == false ) {
                    child.ScenariosList.Add( parent );
                }
            }

            //process the market-plan-to-component links
            foreach( MrktSimDBSchema.market_plan_treeRow tree2Row in tree2Rows ) {
                int parentID = tree2Row.parent_id;
                int childID = tree2Row.child_id;

                MarketPlan parent = MarketPlanForID( parentID );
                Component child = ComponentForID( childID );

                // ignore obsolete links
                if( parent == null || child == null ) {
                    continue;
                }

                if( parent.ComponentsList.Contains( child ) == false ) {
                    parent.ComponentsList.Add( child );
                }
                if( child.MarketPlansList.Contains( parent ) == false ) {
                    child.MarketPlansList.Add( parent );
                }
            }
        }

        /// <summary>
        /// Refreshes the arrays of products, channels, and segments in the model.
        /// </summary>
        private void RefreshReadOnlyItems(){
            string prodTypeQuery = "";
            MrktSimDBSchema.product_typeRow[] prodTypeRows = (MrktSimDBSchema.product_typeRow[])thelDb.Data.product_type.Select( prodTypeQuery );
            productTypes = new ProductType[ prodTypeRows.Length ];
            for( int i = 0; i < prodTypeRows.Length; i++ ) {
                productTypes[ i ] = new ProductType( prodTypeRows[ i ].type_name, prodTypeRows[ i ].id );
            }

            string prodQuery = "";
            MrktSimDBSchema.productRow[] prodRows = (MrktSimDBSchema.productRow[])thelDb.Data.product.Select( prodQuery );
            products = new Product[ prodRows.Length ];
            string[] prodNames = new string[ prodRows.Length ];
            for( int i = 0; i < prodRows.Length; i++ ) {
                prodNames[ i ] = prodRows[ i ].product_name;
                ProductType ptype = ProductTypeForID( prodRows[ i ].product_type );
                bool isLeaf = (((int)prodRows[ i ].brand_id) == 1 );
                products[ i ] = new Product( prodRows[ i ].product_name, prodRows[ i ].product_id, ptype, isLeaf );
            }
            Array.Sort( prodNames, products );

            string chanQuery = "";
            MrktSimDBSchema.channelRow[] chanRows = (MrktSimDBSchema.channelRow[])thelDb.Data.channel.Select( chanQuery );
            channels = new Channel[ chanRows.Length ];
            string[] chanNames = new string[ chanRows.Length ];
            for( int i = 0; i < chanRows.Length; i++ ) {
                chanNames[ i ] = chanRows[ i ].channel_name;
                channels[ i ] = new Channel( chanRows[ i ].channel_name, chanRows[ i ].channel_id );
            }
            Array.Sort( chanNames, channels );

            string segmentQuery = "";
            MrktSimDBSchema.segmentRow[] segRows = (MrktSimDBSchema.segmentRow[])thelDb.Data.segment.Select( segmentQuery );
            segments = new Segment[ segRows.Length ];
            string[] segNames = new string[ segRows.Length ];
            for( int i = 0; i < segRows.Length; i++ ) {
                segNames[ i ] = segRows[ i ].segment_name;
                segments[ i ] = new Segment( segRows[ i ].segment_name, segRows[ i ].segment_id );
            }
            Array.Sort( segNames, segments );
        }
    }
}

