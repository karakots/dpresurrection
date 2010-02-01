using System;
using System.Collections;
using System.Text;

using MrktSimDb;
using DecisionPower.MarketSim.ScenarioManagerLibrary.Components;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary
{
    public class MarketPlan
    {
        private Model model;

        private ArrayList scenariosList;
        private ArrayList componentsList;

        private MrktSimDBSchema.market_planRow planRow;

        private Product product;
        private Segment segment;
        private Channel channel;

        private ModelDb theDb;

        /// <summary>
        /// The name of this market plan
        /// </summary>
        public string Name {
            get {
                return planRow.name;
            }
            set {
                planRow.name = value;
            }
        }

        /// <summary>
        /// The description of this market plan
        /// </summary>
        public string Description {
            get {
                return planRow.descr;
            }
            set {
                planRow.descr = value;
            }
        }

        /// <summary>
        /// The internal database row representing this market plan
        /// </summary>
        public MrktSimDBSchema.market_planRow PlanRow {
            get {
                return planRow;
            }
        }

        /// <summary>
        /// The internal database row representing this market plan
        /// </summary>
        public int ID {
            get {
                return planRow.id;
            }
        }

        /// <summary>
        /// The product for this market plan
        /// </summary>
        public Product Product {
            get {
                return product;
            }
        }

        /// <summary>
        /// The segment for this market plan
        /// </summary>
        public Segment Segment {
            get {
                return segment;
            }
        }

        /// <summary>
        /// The channel for this market plan
        /// </summary>
        public Channel Channel {
            get {
                return channel;
            }
        }

        /// <summary>
        /// Start date for this market plan.  Reflects the data items currently in the plan.
        /// </summary>
        public DateTime StartDate {
            get {
                return planRow.start_date;
            }
        }

        /// <summary>
        /// End date for this market plan.  Reflects the data items currently in the plan.
        /// </summary>
        public DateTime EndDate {
            get {
                return planRow.end_date;
            }
        }
       
        /// <summary>
        /// The scenarios that are using  this market plan
        /// </summary>
        public ArrayList ScenariosList {
            get {
                return scenariosList;
            }
            //set {
            //    scenariosList = value;
            //}
        }

        /// <summary>
        /// The scenarios that are using  this market plan, sorted by scenario name
        /// </summary>
        public Scenario[] Scenarios {
            get {
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
        }
 
        /// <summary>
        /// The components that are used by this scenario
        /// </summary>
        public ArrayList ComponentsList {
            get {
                return componentsList;
            }
        }

        /// <summary>
        /// The components that are used by this scenario, sorted by component name
        /// </summary>
        public Component[] Components {
            get {
                // copy from the master list
                Component[] compsArray = new Component[ componentsList.Count ];
                componentsList.CopyTo( compsArray );

                // put the array in alphabetic order by name
                string[] compNames = new string[ compsArray.Length ];
                for( int i = 0; i < compsArray.Length; i++ ) {
                    compNames[ i ] = compsArray[ i ].Name;
                }
                Array.Sort( compNames, compsArray );

                return compsArray;
            }
        }

        /// <summary>
        /// The Model that this market plan belongs to
        /// </summary>
        public Model Model {
            get {
                return model;
            }
        }

        /// <summary>
        /// The ModelDb instance that this market plan belongs to.  For framework usage only.
        /// </summary>
        public ModelDb ModelDb {
            get {
                return model.ModelDb;
            }
        }

        /// <summary>
        /// Constructs a new MarketPlan object.  For framework usage only.
        /// </summary>
        /// <param name="scenario">Scenario that should contains the market plan</param>
        /// <param name="planRow">Internal representation of the market plan</param>
        public MarketPlan( Model model, MrktSimDBSchema.market_planRow planRow ) {
            this.model = model;
            this.theDb = model.ModelDb;
            this.planRow = planRow;

            if( planRow != null ){
                this.product = Model.ProductForID( planRow.product_id );
                this.segment = Model.SegmentForID( planRow.segment_id );
                this.channel = Model.ChannelForID( planRow.channel_id );
            }

            this.componentsList = new ArrayList();
            this.scenariosList = new ArrayList();
        }

        /// <summary>
        /// Adds the given component to this market plan.
        /// </summary>
        /// <param name="componentToAdd"></param>
        public void AddComponent( Component componentToAdd ){

            theDb.CreatePlanRelation( this.ID, componentToAdd.ID );

            this.model.RefreshAllUserItems();
        }

        /// <summary>
        /// Removes the given component from the market plan.  Does not actually delete trhe component.
        /// </summary>
        /// <param name="componentToRemove"></param>
        public void RemoveComponent( Component componentToRemove ) {

            // remove the compoennt from the plan by removing the plan-tree item pertaining to this component
            string planTtreeQuery = String.Format( "parent_id = {0} AND child_id = {1}", this.ID, componentToRemove.ID );
            MrktSimDBSchema.market_plan_treeRow[] planTreeRows = (MrktSimDBSchema.market_plan_treeRow[])theDb.Data.market_plan_tree.Select( planTtreeQuery );

            foreach( MrktSimDBSchema.market_plan_treeRow planTreeRow in planTreeRows ) {       // should only be 1 row
                planTreeRow.Delete();
            }

            this.model.RefreshAllUserItems();
        }

        /// <summary>
        /// Makes a shallow copy of the market plan (copies market plan, but not components).
        /// </summary>
        /// <param name="newMarketPlanName"></param>
        /// <param name="newDescription"></param>
        /// <returns></returns>
        private MarketPlan DoShallowCopy( string newMarketPlanName, string newDescription ) {

            MrktSimDBSchema.market_planRow copyRow = this.model.ModelDb.CopyPlan( this.planRow );
            copyRow.name = newMarketPlanName;
            copyRow.descr = newDescription;

            MarketPlan copyPlan = new MarketPlan( this.model, copyRow );

            this.model.RefreshAllUserItems();

            return copyPlan;
        }

        /// <summary>
        /// Makes a shallow copy of the market plan (copies market plan, but not components).
        /// </summary>
        /// <param name="newMarketPlanName"></param>
        /// <param name="newDescription"></param>
        /// <returns></returns>
        public MarketPlan ShallowCopy( string newMarketPlanName, string newDescription ) {
            return DoShallowCopy( newMarketPlanName, newDescription );
        }
        
        /// <summary>
        /// Makes a deep copy of the market plan (copies market plan and all components).
        /// </summary>
        /// <param name="newScenarioName"></param>
        /// <param name="newDescription"></param>
        /// <returns></returns>
        /// <remarks>The copied market plan references components which are copies of the components in the original market plan.  
        /// The copied market plan is automatically added to the same scenarios that reference the original.</remarks>
        public MarketPlan Copy( string newMarketPlanName, string newDescription ) {
            return this.Copy( newMarketPlanName, newDescription, this.product );
        }

        /// <summary>
        /// Makes a deep copy of the market plan (copies market plan and all components), using the given new product for all copied items.
        /// </summary>
        /// <param name="newScenarioName"></param>
        /// <param name="newDescription"></param>
        /// <param name="newProduct"></param>
        /// <returns></returns>
        /// <remarks>The copied market plan references components which are copies of the components in the original market plan.  
        /// The copied market plan is automatically added to the same scenarios that reference the original.</remarks>
        public MarketPlan Copy( string newMarketPlanName, string newDescription, Product newProduct ) {

            MarketPlan copyPlan = DoShallowCopy( newMarketPlanName, newDescription );

            foreach( Component comp in copyPlan.componentsList ) {
                // copy each component
                MrktSimDBSchema.market_planRow copiedCompRow = theDb.CopyPlan( comp.ComponentRow );
                Component copiedComponent = new Component( this.model, copiedCompRow );
                copiedComponent.Product = newProduct;

                // remove original component, ...
                copyPlan.RemoveComponent( comp );
                // ...replace with copied component
                copyPlan.AddComponent( copiedComponent );
            }
            this.model.RefreshAllUserItems();

            return copyPlan;
        }

        /// <summary>
        /// Depricated -- use Copy( newName, newDescription, newProduct ) instead.
        /// Changes the product of the market plan and all referenced components.  The new-product components are COPIES of the originals, which still exist.
        /// </summary>
        /// <param name="newProduct"></param>
        public void ChangeProduct( Product newProduct ) {

            if( this.planRow.product_id != newProduct.ID ) {
                this.planRow.product_id = newProduct.ID;
                theDb.UpdateMarketPlan( this.planRow, 0 );            // copies the components before changing the names
            }

            this.model.RefreshAllUserItems();
        }
    }
}
