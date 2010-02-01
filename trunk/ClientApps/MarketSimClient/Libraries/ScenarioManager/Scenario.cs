using System;
using System.Collections;
using System.Text;

using MrktSimDb;

namespace DecisionPower.MarketSim.ScenarioManagerLibrary
{
    public class Scenario
    {
        private Model model;

        private ArrayList marketPlansList;

        private MrktSimDBSchema.scenarioRow scenarioRow;

        private ModelDb theDb;

        /// <summary>
        /// The name of this scenario
        /// </summary>
        public string Name {
            get {
                return scenarioRow.name;
            }
           set {
                scenarioRow.name = value;
            }
        }

        /// <summary>
        /// The description of this scenario
        /// </summary>
        public string Description {
            get {
                return scenarioRow.descr;
            }
            set {
                scenarioRow.descr = value;
            }
        }

        /// <summary>
        /// The internal ID of this scenario
        /// </summary>
        public int ID {
            get {
                return scenarioRow.scenario_id;
            }
        }

        /// <summary>
        /// The internal data row corresponding to this scenario
        /// </summary>
        public MrktSimDBSchema.scenarioRow ScenarioRow {
            get {
                return scenarioRow;
            }
        }

        /// <summary>
        /// The market plans that are used by this scenario
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
        /// The market plans that are used by this scenario, sorted by market plan name
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
        /// The model that contains this scenario
        /// </summary>
        public Model Model {
            get {
                return model;
            }
        }

        /// <summary>
        /// Constructs a new Scenario object.  For framework usage only.
        /// </summary>
        /// <param name="database">Model that contains the scenario</param>
        /// <param name="scenarioRow">Internal representation of the scenario</param>
        public Scenario( Model model, MrktSimDBSchema.scenarioRow scenarioRow ) {
            this.model = model;
            this.theDb = model.ModelDb;
            this.scenarioRow = scenarioRow;
            marketPlansList = new ArrayList();
        }

        /// <summary>
        /// Creates a new empty market plan that is part of this scenario.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public MarketPlan NewMarketPlan( string name, string description, Product product, Channel channel, Segment segment ) {
            MarketPlan newPlan = null;
            MrktSimDBSchema.market_planRow newRow = theDb.CreateMarketPlan( name, MrktSimDb.Database.PlanType.MarketPlan );
            if( newRow != null ) {
                newRow.descr = description;
                newRow.product_id = product.ID;
                newRow.segment_id = segment.ID;
                newRow.channel_id = channel.ID;
                newRow.start_date = model.ModelRow.start_date;
                newRow.end_date = model.ModelRow.end_date;
                theDb.AddMarketPlanToScenario( scenarioRow, newRow );

                newPlan = new MarketPlan( this.model, newRow );
                this.model.RefreshAllUserItems();
            }
            return newPlan;
        }

        /// <summary>
        /// Creates a new empty market plan that is part of this scenario.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public MarketPlan NewMarketPlan( string name, string description, Product product ) {
            MarketPlan newPlan = null;
            MrktSimDBSchema.market_planRow newRow = theDb.CreateMarketPlan( name, MrktSimDb.Database.PlanType.MarketPlan );
            if( newRow != null ) {
                newRow.descr = description;
                newRow.product_id = product.ID;
                newRow.start_date = model.ModelRow.start_date;
                newRow.end_date = model.ModelRow.end_date;
                theDb.AddMarketPlanToScenario( scenarioRow, newRow );

                newPlan = new MarketPlan( this.model, newRow );
                this.model.RefreshAllUserItems();
            }
            return newPlan;
        }

        /// <summary>
        /// Adds a market plan to the scenario
        /// </summary>
        /// <param name="planToAdd"></param>
        public void AddMarketPlan( MarketPlan planToAdd ){
            MrktSimDBSchema.scenario_market_planRow addedRow = this.model.ModelDb.AddMarketPlanToScenario( this.scenarioRow, planToAdd.PlanRow );
            if( addedRow != null ) {
                this.model.RefreshAllUserItems();      
            }
        }

        /// <summary>
        /// Removes a market plan from the scenario
        /// </summary>
        /// <param name="planToRemove"></param>
        public void RemoveMarketPlan( MarketPlan planToRemove ) {
            bool removed = this.model.ModelDb.RemovePlanFromScenario( this.scenarioRow, planToRemove.PlanRow );
            if( removed ) {
                this.model.RefreshAllUserItems();     
            }
        }

        /// <summary>
        /// Makes a shallow copy of the scenario (copies scenario, but not contents).
        /// </summary>
        /// <param name="newScenarioName"></param>
        /// <param name="newDescription"></param>
        /// <returns></returns>
        /// <remarks>The copied scenario references the same market plans as the original scenario.  The copied scenario is automatically added to the model </remarks>
        public Scenario ShallowCopy( string newScenarioName, string newDescription ){
            MrktSimDBSchema.scenarioRow copiedScenarioRow = this.model.ModelDb.CopyScenario( this.scenarioRow, newScenarioName );
            copiedScenarioRow.descr = newDescription;
            Scenario copiedScenario = new Scenario( this.model, copiedScenarioRow );
            this.model.RefreshAllUserItems();
            return copiedScenario;
        }

        //copied scenario contains copies of market plans and assocuated components
        //copied scenario is automatically added to the model
        /// <summary>
        /// Makes a deep copy of the scenario - copies scenario, market plans, and (depending on args) the component data.
        /// </summary>
        /// <returns></returns>
        /// <remarks>The copied scenario references market plans which are copies of the market plans in the original scenario,
        /// and the copied market plans reference copies of components in the original scenario.  
        /// The copied scenario is automatically added to the model. </remarks>
        public Scenario Copy( string newScenarioName, string newDescription, bool copyComponents ) {

            MrktSimDBSchema.scenarioRow copiedScenarioRow = this.model.ModelDb.DeepCopyScenario( this.scenarioRow, newScenarioName, copyComponents );
            copiedScenarioRow.descr = newDescription;
            Scenario copiedScenario = new Scenario( this.model, copiedScenarioRow );
            this.model.RefreshAllUserItems();
            return copiedScenario;
        }
    }
}
