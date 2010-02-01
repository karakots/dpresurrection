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

namespace Common
{
    /// <summary>
    /// This class handles getting lists of items that are used by, or make use of, a given item.   
    /// Items include scenarios, market plans, and plan components.
    /// </summary>
    public class MarketPlanControlRelater
    {
        private ModelDb theDb;

        /// <summary>
        /// Creates a new MarketPlanControlRelater object.
        /// </summary>
        public MarketPlanControlRelater( ModelDb db ) {
            this.theDb = db;
        }

        /// <summary>
        /// Returns true if the given scenario contains the given market plan
        /// </summary>
        /// <param name="marketPlanID"></param>
        /// <param name="scenarioID"></param>
        /// <returns></returns>
        public bool ScenarioContainsPlan( int marketPlanID, int scenarioID ) {
            ArrayList planScenarios = GetScenariosForPlan( marketPlanID );
            bool contains = false;
            foreach( Item item in planScenarios ) {
                if( item.Selected && ((item.ID == scenarioID) || (scenarioID == ModelDb.AllID)) ) {
                    contains = true;
                    break;
                }
            }
            return contains;
        }

        /// <summary>
        /// Returns true if the given market plan contains the given component
        /// </summary>
        /// <param name="componentPlanID"></param>
        /// <param name="marketPlanID"></param>
        /// <returns></returns>
        public bool PlanContainsComponent( int componentPlanID, int marketPlanID ) {
            ArrayList componentPlans = GetPlansForComponent( componentPlanID, -1 );
            bool contains = false;
            foreach( Item item in componentPlans ) {
                if( item.Selected && (item.ID == marketPlanID) ) {
                    contains = true;
                    break;
                }
            }
            return contains;
        }

        /// <summary>
        /// Returns an array of all market plan IDs that are in the specified scenario.
        /// </summary>
        /// <param name="scenarioID"></param>
        /// <returns></returns>
        public int[] GetPlanIDsInScenario( int scenarioID ) {
            ArrayList allPlans = GetPlansForScenario( scenarioID );
            ArrayList usedIDList = new ArrayList();
            foreach( Item item in allPlans ) {
                if( item.Selected ) {
                    usedIDList.Add( item.ID );
                }
            }

            int[] usedIDs = null;
            if( usedIDList.Count > 0 ) {
                usedIDs = new int[ usedIDList.Count ];
                usedIDList.CopyTo( usedIDs );
            }
            return usedIDs;
        }

        /// <summary>
        /// Returns the list of all scenarios - selected items are the ones using the specified market plan.
        /// </summary>
        /// <param name="marketPlanID"></param>
        /// <returns>All scenarios, with none selected, if the value is -1</returns>
        public ArrayList GetScenariosForPlan( int marketPlanID ) {

            DataTable scenarioTable = theDb.Data.scenario;
            MrktSimDBSchema.scenarioRow[] allRows = (MrktSimDBSchema.scenarioRow[])scenarioTable.Select();

            DataTable treeTable = theDb.Data.scenario_market_plan;
            string treeQuery = "";
            if( marketPlanID >= 0 ) {
                treeQuery = String.Format( "market_plan_id={0}", marketPlanID );
            }
            MrktSimDBSchema.scenario_market_planRow[] selectedRows = (MrktSimDBSchema.scenario_market_planRow[])treeTable.Select( treeQuery );

            ArrayList items = new ArrayList();
            // add all of the market plan rows
            foreach( MrktSimDBSchema.scenarioRow row in allRows ) {

                // get the ID if the scenario to add
                int itemID = row.scenario_id;

                // ignore the row if we added the ID already
                if( ItemListContainsID( itemID, items ) ) {
                    continue;
                }

                // get the name of the scenario plan we are adding
                string itemName = row.name;

                // see if the item is selected
                bool selected = false;
                foreach( MrktSimDBSchema.scenario_market_planRow selectedRow in selectedRows ) {
                    if( (selectedRow.scenario_id == itemID) && (marketPlanID >= 0) ) {
                        selected = true;
                        break;
                    }
                }

                // create the item and add it to the list
                Item item = new Item( itemID, itemName, selected );
                items.Add( item );
            }
            return items;
        }

        /// <summary>
        /// Returns the list of all market plans - selected items are the ones used by the specified scenario.
        /// </summary>
        /// <param name="marketPlanID"></param>
        /// <returns></returns>
        public ArrayList GetPlansForScenario( int scenarioID ) {

            DataTable plansTable = theDb.Data.market_plan;
            MrktSimDBSchema.market_planRow[] allRows = (MrktSimDBSchema.market_planRow[])plansTable.Select( "type = 0 OR type = 5" );

            DataTable treeTable = theDb.Data.scenario_market_plan;
            string treeQuery = String.Format( "scenario_id={0}", scenarioID );
            MrktSimDBSchema.scenario_market_planRow[] selectedRows = (MrktSimDBSchema.scenario_market_planRow[])treeTable.Select( treeQuery );

            ArrayList items = new ArrayList();
            // add all of the market plan rows
            foreach( MrktSimDBSchema.market_planRow row in allRows ) {

                int itemID = row.id;

                // ignore the row if we added the ID already
                if( ItemListContainsID( itemID, items ) ) {
                    continue;
                }
                
                string itemName = row.name;

                // see if the item is selected
                bool selected = false;
                foreach( MrktSimDBSchema.scenario_market_planRow selectedRow in selectedRows ) {
                    if( selectedRow.market_plan_id == itemID ) {
                        selected = true;
                        break;
                    }
                }

                // create the item and add it to the list
                Item item = new Item( itemID, itemName, (ModelDb.PlanType)row.type, selected, row.product_id );
                items.Add( item );
            }
            return items;
        }

        /// <summary>
        /// Returns the list of all market plans - selected items are the ones using the specified component.
        /// </summary>
        /// <param name="marketPlanID"></param>
        /// <returns>All market plans, with none selected, if the argument is -1</returns>
        public ArrayList GetPlansForComponent( int marketPlanComponentID, int productID ) {

            DataTable table = theDb.Data.market_plan;
            string tableQuery = "type=0";                          // select only top-level plans
            if( productID >= 0 ) {
                tableQuery = String.Format( "type=0 AND product_id={0}", productID );
            }
            MrktSimDBSchema.market_planRow[] allRows = (MrktSimDBSchema.market_planRow[])table.Select( tableQuery );

            DataTable treeTable = theDb.Data.market_plan_tree;
            string treeQuery = "";
            if( marketPlanComponentID >= 0 ) {
                treeQuery = String.Format( "child_id={0}", marketPlanComponentID );
            }
            MrktSimDBSchema.market_plan_treeRow[] selectedTreeRows = (MrktSimDBSchema.market_plan_treeRow[])treeTable.Select( treeQuery );

            ArrayList items = new ArrayList();
            // add all of the market plan rows
            foreach( MrktSimDBSchema.market_planRow row in allRows ) {

                int itemID = row.id; 

                // ignore the row if we added the ID already
                if( ItemListContainsID( itemID, items ) ) {
                    continue;
                }

                // get the name of the market plan we are adding
                string itemName = row.name;

                // see if the item is selected
                bool selected = false;
                if( marketPlanComponentID >= 0 ) {
                    foreach( MrktSimDBSchema.market_plan_treeRow selectedRow in selectedTreeRows ) {
                        if( selectedRow.parent_id == itemID ) {
                            selected = true;
                            break;
                        }
                    }
                }

                // create the item and add it to the list
                Item item = new Item( itemID, itemName, (ModelDb.PlanType)row.type, selected, row.product_id );
                items.Add( item );
            }
            return items;
        }

        /// <summary>
        /// Returns the list of all market plan components of the specified type - selected items are the ones used by the market plan (use -1 for all plans).
        /// </summary>
        /// <param name="marketPlanID"></param>
        /// <returns>All market plans components of the  currently active type, with none selected, if the argument is -1</returns>
        public ArrayList GetComponentsForPlan( int marketPlanID, int productID ) {

            DataTable table = theDb.Data.market_plan;
            string tableQuery = String.Format( "type<>{0}", 0 );                          // select only component types
            if( productID >= 0 ) {
                tableQuery = String.Format( "type<>{0} AND product_id={1}", 0, productID );                          // select only component types
            }
            MrktSimDBSchema.market_planRow[] allRows = (MrktSimDBSchema.market_planRow[])table.Select( tableQuery );

            DataTable treeTable = theDb.Data.market_plan_tree;
            string treeQuery = "";           // marketPlanID == -1 gives all components
            if( marketPlanID !=  -1 ) {
                treeQuery = String.Format( "parent_id={0}", marketPlanID );
            }
            MrktSimDBSchema.market_plan_treeRow[] selectedTreeRows = (MrktSimDBSchema.market_plan_treeRow[])treeTable.Select( treeQuery );

            ArrayList items = new ArrayList();
            // add all of the market plan rows
            foreach( MrktSimDBSchema.market_planRow row in allRows ) {

                int itemID = row.id;

                // ignore the row if we added the ID already
                if( ItemListContainsID( itemID, items ) ) {
                    continue;
                }

                // get the name of the market plan we are adding
                string itemName = row.name;

                // see if the item is selected
                bool selected = false;
                foreach( MrktSimDBSchema.market_plan_treeRow selectedRow in selectedTreeRows ) {
                    if( (selectedRow.child_id == itemID) && (marketPlanID != -1) ) {
                        selected = true;
                        break;
                    }
                }

                // create the item and add it to the list
                Item item = new Item( itemID, itemName, (ModelDb.PlanType)row.type, selected, row.product_id );
                items.Add( item );
            }
            return items;
        }

        /// <summary>
        /// Returns true if any of the (Item) objects in the given list have the given ID value.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        private bool ItemListContainsID( int id, ArrayList items ) {
            bool contains = false;
            foreach( Item item in items ){
                if( item.ID == id ) {
                    contains = true;
                    break;
                }
            }
            return contains;
        }

        /// <summary>
        /// Encapsulates a set of result item values for MarketPlanControlRelater methods.
        /// </summary>
        public class Item
        {
            public int ID;
            public string Name;
            public ModelDb.PlanType ItemType;
            public bool Selected;
            public bool Visible;
            public int ProductID;

            // an item that represents a scenario
            public Item( int id, string name, bool selected ) {
                this.ID = id;
                this.Name = name;
                this.ItemType = ModelDb.PlanType.MarketPlan;         // will be ignored
                this.Selected = selected;
                this.Visible = true;
                this.ProductID = ModelDb.AllID;
            }

            // an item that represents a market plan or component
            public Item( int id, string name, ModelDb.PlanType type, bool selected ) {
                this.ID = id;
                this.Name = name;
                this.ItemType = type;
                this.Selected = selected;
                this.Visible = true;
                this.ProductID = ModelDb.AllID;
            }

            // an item that represents a market plan or component
            public Item( int id, string name, ModelDb.PlanType type, bool selected, int productId ) {
                this.ID = id;
                this.Name = name;
                this.ItemType = type;
                this.Selected = selected;
                this.Visible = true;
                this.ProductID = productId;
            }

            // an item that represents a market plan or component
            public Item( int id, string name, ModelDb.PlanType type, bool selected, bool visible ) {
                this.ID = id;
                this.Name = name;
                this.ItemType = type;
                this.Selected = selected;
                this.Visible = visible;
                this.ProductID = ModelDb.AllID;
            }
        }
    }
}
