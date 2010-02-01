using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace MrktSimDb
{
    partial class Database
    {

        public struct grpData
        {
            public double val;
            public double persuasion;
            public double awareness;
            public DateTime start;
            public DateTime end;
        }

        public struct probData
        {
            public double share;
            public double aware;
        }

        public enum PlanType
        {
            MarketPlan = 0,
            Price = 1,
            Distribution = 2,
            Display = 3,
            Media = 4,
            ProdEvent = 5,
            TaskEvent = 6, // no longer used
            Market_Utility = 7,
            Coupons = 8
        }

        private MrktSimDBSchema.Model_infoRow getModel()
        {
            if (Data.Model_info.Rows.Count != 1)
                return null;

            // get model

            return (MrktSimDBSchema.Model_infoRow)Data.Model_info.Rows[0];
        }



        public MrktSimDBSchema.market_planRow CreateMarketPlan(string name, PlanType type)
        {
            return CreateMarketPlan(name, AllID, AllID, AllID, AllID, type);
        }

        public MrktSimDBSchema.market_planRow CreateMarketPlan(string name, int product_id, int segment_id, int channel_id, int task_id, PlanType planType)
        {
            MrktSimDBSchema.Model_infoRow model = getModel();

            if (model == null)
                return null;

            // create a new brand
            MrktSimDBSchema.market_planRow row = Data.market_plan.Newmarket_planRow();

            row.model_id = model.model_id;

            byte type = (byte)planType;

            string filter = "type = " + type;

            if (name != null)
                row.name = CreateUniqueName(Data.market_plan, "name", name, filter);
            else
                row.name = "";

            row.descr = "";
            row.start_date = model.start_date;
            row.end_date = model.end_date;
            row.interval = 0;
            row.product_id = product_id;
            row.segment_id = segment_id;
            row.channel_id = channel_id;
            row.task_id = task_id;
            row.type = type; // unknown type

            row.parm1 = 1;
            row.parm2 = 1;
            row.parm3 = 1;
            row.parm4 = 1;
            row.parm5 = 1;
            row.parm6 = 1;

            row.user_name = "admin";

            Data.market_plan.Addmarket_planRow(row);
            return row;
        }

        public MrktSimDBSchema.product_eventRow CreateProductEvent(int planID, int productID, int channelID, int segmentID)
        {

            MrktSimDBSchema.Model_infoRow model = getModel();

            if (model == null)
                return null;

            // create new ditribution row
            MrktSimDBSchema.product_eventRow row = Data.product_event.Newproduct_eventRow();

            row.market_plan_id = planID;
            row.model_id = model.model_id;
            row.segment_id = segmentID;
            row.product_id = productID;
            row.channel_id = channelID;

            row.type = 0;

            row.demand_modification = 0.0;

            // the default is the model start end
            row.start_date = model.start_date;
            row.end_date = model.end_date;

            Data.product_event.Addproduct_eventRow(row);

            return row;
        }






        #region Market Plan and Scenario Methods

        //JimJ - same as next method but using just the ID of the scenario
        // SSN - have one call the other
        // 6/18/2007
        public MrktSimDBSchema.scenario_market_planRow ScenarioHasPlan(int scenarioID, MrktSimDBSchema.market_planRow market_plan)
        {
            foreach (MrktSimDBSchema.scenario_market_planRow smref in market_plan.Getscenario_market_planRows())
            {
                if (smref.scenarioRow.scenario_id == scenarioID)
                    return smref;
            }

            return null;
        }

        public MrktSimDBSchema.scenario_market_planRow ScenarioHasPlan(MrktSimDBSchema.scenarioRow scenario,
            MrktSimDBSchema.market_planRow market_plan)
        {
            return ScenarioHasPlan(scenario.scenario_id, market_plan);
        }

        public bool RemovePlanFromScenario(MrktSimDBSchema.scenarioRow scenario,
            MrktSimDBSchema.market_planRow market_plan)
        {
            MrktSimDBSchema.scenario_market_planRow scenarioPlanRef = ScenarioHasPlan(scenario, market_plan);

            if (scenarioPlanRef != null)
            {
                scenarioPlanRef.Delete();

                return true;
            }

            return false;
        }

        //JimJ - same as next method but using just the ID of the scenario
        // SSN changed so next method calls this one
        // 6/18/2007
        public MrktSimDBSchema.scenario_market_planRow AddMarketPlanToScenario(int scenarioID,
            MrktSimDBSchema.market_planRow market_plan)
        {

            MrktSimDBSchema.Model_infoRow model = getModel();

            if (model == null)
                return null;

            MrktSimDBSchema.scenario_market_planRow row = ScenarioHasPlan(scenarioID, market_plan);

            if (row != null)
                return row;

            row = Data.scenario_market_plan.Newscenario_market_planRow();

            row.model_id = model.model_id;
            row.scenario_id = scenarioID;
            row.market_plan_id = market_plan.id;

            DataRow[] rows = Data.scenario.Select();
            
            Data.scenario_market_plan.Addscenario_market_planRow(row);

            return row;
        }

        public MrktSimDBSchema.scenario_market_planRow AddMarketPlanToScenario(MrktSimDBSchema.scenarioRow scenario,
            MrktSimDBSchema.market_planRow market_plan)
        {
            return AddMarketPlanToScenario(scenario.scenario_id, market_plan);

            //MrktSimDBSchema.Model_infoRow model = getModel();

            //if (model == null)
            //    return null;

            //MrktSimDBSchema.scenario_market_planRow row = ScenarioHasPlan(scenario, market_plan);

            //if (row != null)
            //    return row;

            //row = Data.scenario_market_plan.Newscenario_market_planRow();

            //row.model_id = model.model_id;
            //row.scenario_id = scenario.scenario_id;
            //row.market_plan_id = market_plan.id;

            //Data.scenario_market_plan.Addscenario_market_planRow(row);

            //return row;
        }

        /// <summary>
        /// Ensure that Market Plan start and end dates are allinged with the data.
        /// </summary>
        /// <returns> true if change is made </returns>
        public bool AllignPlansWithData()
        {
            bool rval = false;

            // do components first
            string query = "type <> 0";
            foreach (MrktSimDBSchema.market_planRow plan in Data.market_plan.Select(query, "", DataViewRowState.CurrentRows))
            {
                if (AllignPlanWithData(plan))
                {
                    rval = true;
                }
            }

            query = "type = 0 ";
            foreach (MrktSimDBSchema.market_planRow plan in Data.market_plan.Select(query, "", DataViewRowState.Added))
            {
                if (AllignPlanWithData(plan))
                {
                    rval = true;
                }
            }

            return rval;
        }

        private bool AllignPlanWithData(MrktSimDBSchema.market_planRow plan)
        {
            bool rval = false;

            DateTime start = plan.start_date;
            DateTime end = plan.end_date;

            if (plan.type == 0)
            {
                // this is a top level plan
                MrktSimDBSchema.market_plan_treeRow[] trows = plan.Getmarket_plan_treeRowsBymarket_planmarket_plan_tree_parent();

                if (trows.Length == 0)
                {
                    return rval;
                }


                start = trows[0].market_planRowBymarket_planmarket_plan_tree_child.start_date;
                end = trows[0].market_planRowBymarket_planmarket_plan_tree_child.end_date;


                foreach (MrktSimDBSchema.market_plan_treeRow trow in trows)
                {
                    MrktSimDBSchema.market_planRow comp = trow.market_planRowBymarket_planmarket_plan_tree_child;

                    DateTime curStart = comp.start_date;
                    DateTime curEnd = comp.end_date;

                    if (start > curStart)
                        start = curStart;

                    if (end < curEnd)
                        end = curEnd;
                }

                return rval;
            }
            else
            {

                string query = "market_plan_id = " + plan.id;

                DataRow[] rows = null;

                switch ((PlanType)plan.type)
                {
                    case PlanType.Media:

                        rows = Data.mass_media.Select(query, "", DataViewRowState.CurrentRows);

                        break;
                    case PlanType.Display:

                        rows = Data.display.Select(query, "", DataViewRowState.CurrentRows);

                        break;
                    case PlanType.Distribution:

                        rows = Data.distribution.Select(query, "", DataViewRowState.CurrentRows);

                        break;
                    case PlanType.Market_Utility:

                        rows = Data.market_utility.Select(query, "", DataViewRowState.CurrentRows);

                        break;
                    case PlanType.Price:

                        rows = Data.product_channel.Select(query, "", DataViewRowState.CurrentRows);

                        break;
                    case PlanType.Coupons:

                        rows = Data.mass_media.Select(query, "", DataViewRowState.CurrentRows);

                        break;

                    case PlanType.ProdEvent:        //JimJ
                        // ignore external factors in aligning (???)
                        return rval;
                }

                // find max min of dates

                if (rows.Length == 0)
                {
                    return rval;
                }

                start = (DateTime)rows[0]["start_date"];
                end = (DateTime)rows[0]["end_date"];

                foreach (DataRow row in rows)
                {
                    DateTime curStart = (DateTime)row["start_date"];
                    DateTime curEnd = (DateTime)row["end_date"];

                    if (start > curStart)
                        start = curStart;


                    if (end < curEnd)
                        end = curEnd;
                }
            }

            if (plan.start_date != start)
            {
                plan.start_date = start;
                rval = true;
            }

            if (plan.end_date != end)
            {
                plan.end_date = end;
                rval = true;
            }

            return rval;
        }


        // deprecated
        public void KillScenarioAndMarketPlans(MrktSimDb.MrktSimDBSchema.scenarioRow scenario)
        {
            // this not only deletes the scenario but deletes the market plan and
            // also deletes market plans components owned by this user

            // start at the top level plans

            string query = "";
            string userName = scenario.user_name;
            foreach (MrktSimDBSchema.scenario_market_planRow planRef in scenario.Getscenario_market_planRows())
            {
                MrktSimDBSchema.market_planRow topPlan = planRef.market_planRow;
                if (topPlan.user_name == userName)
                {
                    query = "market_plan_id = " + topPlan.id + " AND scenario_id <> " + scenario.scenario_id;
                    DataRow[] rows = this.Data.scenario_market_plan.Select(query, "", DataViewRowState.CurrentRows);
                    if (rows.Length == 0)
                    {
                        topPlan.Delete();
                    }
                }
            }

            scenario.Delete();

            // cleanup
            // delete all market plan components that are no longer referenced
            if (userName.Length > 1)
            {
                query = "user_name = '" + userName + "'";
                DataRow[] plans = this.Data.market_plan.Select(query, "", DataViewRowState.CurrentRows);

                foreach (MrktSimDBSchema.market_planRow plan in plans)
                {
                    // if this is not in a top level plan it needs to be deleted

                    if (plan.type != 0)
                    {
                        // not a top level plan then if it does not have a reference in the market plan tree
                        // delete it

                        if (plan.Getmarket_plan_treeRowsBymarket_planmarket_plan_tree_child().Length == 0)
                        {
                            // need to read in plan just so we can delete it
                            //ReadPlanData(plan);

                            plan.Delete();
                        }
                    }
                }
            }
        }

        public MrktSimDBSchema.scenario_market_planRow ScenarioHasProductPlan(int scenario_id, int product_id)
        {
            string query = "scenario_id = " + scenario_id;
            DataRow[] rows = Data.scenario_market_plan.Select(query, "", DataViewRowState.CurrentRows);

            foreach (DataRow row in rows)
            {
                MrktSimDBSchema.scenario_market_planRow assoc = (MrktSimDBSchema.scenario_market_planRow)row;

                MrktSimDBSchema.market_planRow scenarioPlan = (MrktSimDBSchema.market_planRow)
                    assoc.GetParentRow("market_planscenario_market_plan", DataRowVersion.Current);

                // only concerned about MarketPlans - Not external factors
                if (scenarioPlan.type != (byte)PlanType.MarketPlan)
                    continue;

                if (scenarioPlan.product_id == product_id)
                    return assoc;
            }

            return null;
        }

        /// <summary>
        /// Returns the list of all market plans (MrktSimDBSchema.market_planRow objects) not used in any scenario
        /// </summary>
        /// <returns></returns>
        public ArrayList AllMarketPlansNotInAnyScenario()
        {
            ArrayList allUnassignedPlans = new ArrayList();

            string query = String.Format("type = {0}", (int)ModelDb.PlanType.MarketPlan);
            DataRow[] plans = this.Data.market_plan.Select(query, "", DataViewRowState.CurrentRows);

            foreach (MrktSimDBSchema.market_planRow plan in plans)
            {

                DataRow[] assocs = plan.GetChildRows("market_planscenario_market_plan", DataRowVersion.Current);

                if (assocs == null || assocs.Length == 0)
                {
                    allUnassignedPlans.Add(plan);
                }
            }

            return allUnassignedPlans;
        }

        /// <summary>
        /// Returns the list of all components (MrktSimDBSchema.market_planRow objects) not used in any market plan
        /// </summary>
        /// <returns></returns>
        public ArrayList AllComponentsNotInAnyPlan()
        {
            ArrayList allUnassignedComponents = new ArrayList();

            string query = String.Format("type <> {0}", (int)ModelDb.PlanType.MarketPlan);
            DataRow[] components = this.Data.market_plan.Select(query, "", DataViewRowState.CurrentRows);

            foreach (MrktSimDBSchema.market_planRow component in components)
            {

                DataRow[] assocs = component.GetChildRows("market_planmarket_plan_tree_child", DataRowVersion.Current);

                if (assocs == null || assocs.Length == 0)
                {
                    allUnassignedComponents.Add(component);
                }
            }

            return allUnassignedComponents;
        }

        #endregion

        #region Calibration methods
     
        public Dictionary<int, Dictionary<int, string>> SimulationMediaPlans( int sim_id ) {


            MrktSimDBSchema.simulationRow sim = Data.simulation.FindByid( sim_id );

            // file a table with media data
            genericCommand.CommandText = "Select id, name, product_id" +
                " from market_plan " +
                " where model_id = " + sim.scenarioRow.model_id +
                " AND type = 4 ";

            Dictionary<int, Dictionary<int, string>>  planNames = new Dictionary<int, Dictionary<int, string>> ();

            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            object[] planRead = new object[ 3 ];

            while( dataReader.Read() ) {
                dataReader.GetValues( planRead );

                int planId = (int)planRead[ 0 ];
                string name = (string)planRead[ 1 ];
                int prodId = (int)planRead[ 2 ];

                if( !planNames.ContainsKey( prodId ) ) {
                    planNames[prodId] = new Dictionary<int,string>();
                }

                planNames[ prodId ][ planId ] = name;
            }

            dataReader.Close();

            return planNames;
        }

       
        public Dictionary<int, string> MarketPlanNames( int run_id ) {
            MrktSimDBSchema.sim_queueRow run = Data.sim_queue.FindByrun_id( run_id );

            // file a table with media data
            genericCommand.CommandText = "Select id, name" +
                " from market_plan " +
                " where model_id = " + run.model_id;

            Dictionary<int, string> planNames = new Dictionary<int, string>();

            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            object[] planRead = new object[ 2 ];

            while( dataReader.Read() ) {
                dataReader.GetValues( planRead );

                int planId = (int)planRead[ 0 ];
                string name = (string)planRead[ 1 ];

                planNames[ planId ] = name;
            }

            dataReader.Close();

            return planNames;
        }


        // dispEGRPS <seg ,<chan ,<date ,<prod ,<plan, grps>>>>>
        public void DistDispEGRP(int run_id, out SortedDictionary<int, SortedDictionary<int , SortedDictionary<DateTime, SortedDictionary<int, SortedDictionary<int, double>>>>> dispEGRPS)
        {
            dispEGRPS = new SortedDictionary<int, SortedDictionary<int, SortedDictionary<DateTime, SortedDictionary<int, SortedDictionary<int, double>>>>>();
        }

        /// <summary>
        /// Turn sorted lists into the proper list expected by solver
        /// never used this but am leaving it in as reference
        /// </summary>
        /// <param name="prodPlanEGrps"> product, plan, perusaion by date</param>
        /// <param name="prodPersuasion"> product perusion by date</param>
        /// <param name="Attrs"> value at date, prod, attr is persuasion for plan</param>
        /// <param name="totalVals">value at date, prod is total persuasion</param>
        public void TransformToLists( SortedDictionary<int, SortedDictionary<int, List<double>>> prodPlanEGrps, SortedDictionary<int, List<double>> prodPersuasion,
            out List<List<List<double>>> Attrs, out List<List<double>> totalVals ) {
            Attrs = new List<List<List<double>>>();
            totalVals = new List<List<double>>();

            int prodIndex = 0;
            foreach( List<double> persuasionVals in prodPersuasion.Values ) {

                int dateIndex = 0;
                foreach( double persuasion in persuasionVals ) {

                    if( dateIndex == totalVals.Count ) {
                        totalVals.Add( new List<double>() );
                    }

                    // we are always adding to list
                    totalVals[ dateIndex ].Add( persuasion );
                
                    dateIndex++;
                }

                prodIndex++;
            }


            prodIndex = 0;
            foreach( SortedDictionary<int, List<double>> prodPlans in prodPlanEGrps.Values ) {

                int attrIndex = 0;
                foreach( List<double> planVals in prodPlans.Values ) {

                    int dateIndex = 0;
                    foreach( double persuasion in planVals ) {

                        if( dateIndex == Attrs.Count ) {
                            Attrs.Add(new List<List<double>>());
                        }

                        if( prodIndex == Attrs[dateIndex].Count) {
                            Attrs[dateIndex].Add(new List<double>());
                        }

                          if( attrIndex == Attrs[dateIndex][prodIndex].Count) {
                            Attrs[dateIndex][prodIndex].Add(0);
                        }

                        Attrs[dateIndex][prodIndex][attrIndex] = persuasion;

                        dateIndex++;
                    }

                    attrIndex++;
                }

                prodIndex++;
            }
        }

        /// <summary>
        /// Computes effective impact of GRP on consumer per plan per product 
        /// segment variables impacting this calcuation are averaged
        /// </summary>
        /// <param name="run_id"></param>
        /// <param name="plans"></param>
        /// <param name="prodPlanEGrps"> prodPlanEGrps[prod_id][plan_id][date_index] = effective grps on this day</param>
        /// <param name="prodPersuasion">prodPersuasion[prod_id][date_index] = actual perusasion on this day</param>
        /// <param name="dates">dates[date_index] = date on this day</param>
        public void MediaEGRP(int run_id, List<int> plans, 
            out SortedDictionary<int, SortedDictionary<int, List<double>>> prodPlanEGrps, 
            out SortedDictionary<int, List<double>> prodPersuasion, out List<DateTime> dates)
        {

            MrktSimDBSchema.sim_queueRow run = Data.sim_queue.FindByrun_id(run_id);
            MrktSimDBSchema.simulationRow sim = run.simulationRow;
            int scenario_id = sim.scenario_id;

            // segment variables to average
            double persuasion_scaling = 0;
            double pre_use_persuasion_decay = 0;
            double post_use_persuasion_decay = 0;

            int num_segs = 0;
            foreach( MrktSimDBSchema.segmentRow seg in Data.segment.Select() ) {
                post_use_persuasion_decay +=  seg.persuasion_decay_rate_post;
                pre_use_persuasion_decay += seg.persuasion_decay_rate_pre;
                persuasion_scaling += seg.persuasion_scaling;
                num_segs++;
            }

            pre_use_persuasion_decay /= 100 * num_segs;
            post_use_persuasion_decay /= 100 * num_segs;


            // fill a table with media data
            genericCommand.CommandText = "Select product_id, market_plan_id, attr_value_G, " +
                " message_persuation_probability, message_awareness_probability, start_date, end_date " +
                " from scenario_mass_media " +
                " where media_type = 'V' " +
                " and scenario_id = " + scenario_id;

            
            // by prod by plan we have a list of grpData(num, persuasion awareness, start, end)
            SortedDictionary<int, SortedDictionary<int, List<grpData>>> prodPlanGRPs = new SortedDictionary<int, SortedDictionary<int, List<grpData>>>();

            genericCommand.Connection.Open();
            System.Data.OleDb.OleDbDataReader dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            object[] grpRead = new object[ 7 ];

            SortedDictionary<int, List<grpData>> planGRPs = null;

            List<grpData> grps = null;

            while( dataReader.Read() ) {
                dataReader.GetValues( grpRead );

                int prodId = (int)grpRead[ 0 ];
                int planId = (int)grpRead[ 1 ];

               

                grpData grp = new grpData();

                grp.val = (double)grpRead[ 2 ];
                grp.persuasion = (double)grpRead[ 3 ];
                grp.awareness = (double)grpRead[ 4 ];
                grp.start = (DateTime)grpRead[ 5 ];
                grp.end = (DateTime)grpRead[ 6 ];

                // add to proper list
                if( prodPlanGRPs.ContainsKey( prodId ) ) {

                    planGRPs = prodPlanGRPs[ prodId ];
                }
                else {
                    planGRPs = new SortedDictionary<int, List<grpData>>();
                    prodPlanGRPs.Add( prodId, planGRPs );
                }

                if( plans != null && !plans.Contains( planId ) ) {
                    continue;
                }
                
                // add to proper list
                if( planGRPs.ContainsKey( planId ) ) {
                    grps = planGRPs[ planId ];
                }
                else {
                    grps = new List<grpData>();
                    planGRPs.Add( planId, grps );
                }

                grps.Add( grp );
            }

            dataReader.Close();

            // we get the actual product persuasion at any given time
            // as well as the share and awareness information

            // share and awareness are used to estimate the effectivenss of the grp
            // product persuasion is returned to the caller
            // in models where saturation is a factor then the product perusasion is needed

            //Grab the units data from the database to fill the sim_units dictionary
            genericCommand.CommandText = "select results_std.product_id, results_std.calendar_date, " +
                " SUM(results_std.num_sku_bought) as units, " +
                 " AVG(results_std.percent_aware_sku_cum) as aware, " +
                 " AVG(results_std.persuasion_sku) as persuasion " +
                " FROM results_std, product " +
                " WHERE results_std.product_id = product.product_id   " +
                " and results_std.run_id = " + run_id +
                " GROUP By results_std.product_id, results_std.calendar_date " +
                " ORDER BY results_std.product_id, results_std.calendar_date";


            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );


            prodPersuasion = new SortedDictionary<int, List<double>>();
            SortedDictionary<int, List<probData>> prodProbs = new SortedDictionary<int, List<probData>>();

            List<probData> probs = null;
            List<double> persuasionVals = null;

            object[] shareData = new object[ 5 ];
         
            //Grab data from database and store in a multimap
            while( dataReader.Read() ) {
                dataReader.GetValues( shareData );

                int prod_id = (int)shareData[ 0 ];

                //if (!prodPlanGRPs.ContainsKey(prod_id))
                //{
                //    continue;
                //}

                probData prob = new probData();

                prob.share = (double) shareData[ 2 ] ;
                prob.aware = (double) shareData[ 3 ];

                double persuasion = (double) shareData[4];

                persuasion /= sim.access_time;

                prob.aware /= 100 * sim.access_time;

                // used to estimate persuasion
                if( prodProbs.ContainsKey( prod_id ) ) {

                    probs = prodProbs[ prod_id ];
                }
                else {
                    probs = new List<probData>();
                    prodProbs.Add( prod_id, probs );
                }

                // this is used by solver
                if( prodPersuasion.ContainsKey( prod_id ) ) {

                    persuasionVals = prodPersuasion[ prod_id ];
                }
                else {
                    persuasionVals = new List<double>();
                    prodPersuasion.Add( prod_id, persuasionVals );
                }

                probs.Add( prob );
                persuasionVals.Add( persuasion );
            }

            dataReader.Close();

            
            // grab dates
            genericCommand.CommandText = "select DISTINCT calendar_date " +
                " FROM results_std" +
                " WHERE results_std.run_id = " + run_id +
                " ORDER BY calendar_date";


            genericCommand.Connection.Open();
            dataReader = genericCommand.ExecuteReader( CommandBehavior.CloseConnection );

            dates = new List<DateTime>();
          
            object[] dateData = new object[ 1 ];
      

            //Grab data from database and store in a multimap
            while( dataReader.Read() ) {
                dataReader.GetValues( dateData );
                dates.Add( dataReader.GetDateTime( 0 ) );
            }

            dataReader.Close();

            for( int ii = 0; ii < dates.Count; ++ii ) {

                double total = 0;

                foreach( List<probData> prodProb in prodProbs.Values ) {

                    total += prodProb[ ii ].share;
                }

                if( total > 0 ) {
                    foreach( List<probData> prodProb in prodProbs.Values ) {

                        probData probD = prodProb[ ii ];

                        probD.share = prodProb[ ii ].share / total;
                        probD.aware = prodProb[ ii ].aware;

                        prodProb[ ii ] = probD;
                    }
                }
            }

            // now turn GRPs into effective GRPS
            int sim_id = (int)Data.sim_queue.Select("run_id = " + run_id, "", DataViewRowState.CurrentRows)[0]["sim_id"];
            DateTime start_date = MrktSimDb.Metrics.MetricMan.Convert2DateTime( Data.simulation.Select( "id = " + sim_id, "", DataViewRowState.CurrentRows )[ 0 ][ "start_date" ] );

            prodPlanEGrps = new SortedDictionary<int, SortedDictionary<int, List<double>>>();

            SortedDictionary<int, List<double>> planEGRPs = null;
            foreach( int prodId in prodPlanGRPs.Keys ) {

                planGRPs = prodPlanGRPs[ prodId ];

                planEGRPs = new SortedDictionary<int, List<double>>();
                prodPlanEGrps[ prodId ] = planEGRPs;


                foreach( int planId in planGRPs.Keys ) {

                    grps = planGRPs[ planId ];

                    // translate grps into egrps
                    planEGRPs[ planId ] = EffectiveGrps( post_use_persuasion_decay, pre_use_persuasion_decay, dates, grps, prodProbs[ prodId ], start_date );
                }
            }
        }

        // Effective takes decay into account
        public List<double> EffectiveGrps(double post_use_persusion_decay, double pre_use_persuasion_decay, 
            List<DateTime> dates, List<grpData> grps, List<probData> probs, DateTime simStart)
        {
            List<double> egrps = new List<double>();

            bool post_decay = true;
            bool pre_decay = true;

            double post_scale = 1 - post_use_persusion_decay;
            double pre_scale = 1 - pre_use_persuasion_decay;

            if( post_use_persusion_decay < 0.0000001 )
            {
                post_decay = false;
            }

            if( pre_use_persuasion_decay < 0.0000001 )
            {
                pre_decay = false;
            }

            DateTime simEnd = new DateTime();
            
            simEnd = dates[dates.Count - 1];

            TimeSpan date_span = dates[0] - simStart;


            DateTime day = simStart;
            double egrp = 0.0;
            int dateDex = 0;
            foreach( DateTime date in dates ) 
            {
                probData prob = probs[ dateDex ];

                egrp = 0.0;
                while (day <= date)
                {
                    // figure out how the grp contribution on this day from each effective grp

                    foreach (grpData grp in grps)
                    {
                        if (grp.end <= simStart || grp.start > day)
                        {
                            continue;
                        }

                        // Reminder
                        // S = 1 + a + ... a^(n -1)
                        // a*S = a + ... a^n
                        // S = (1 - a^n)/(1 - a)

                        TimeSpan span = grp.end - grp.start;
                        int numDays = span.Days + 1;
                        double dailyGrp = grp.val / (100 * numDays);

                        dailyGrp *= grp.persuasion * (grp.awareness + (1 - grp.awareness) * prob.aware);

                        TimeSpan spanToDay = day - grp.end;
                        int length = spanToDay.Days;


                        // some cases
                        if (grp.start <= simStart)
                        {
                            // this is the case where the initial grps straddle the sim start

                            TimeSpan delta = simStart - grp.start;
                            int deltaDays = delta.Days + 1;

                            numDays -= deltaDays;

                        }
                        else if (length < 0)
                        {
                            numDays += length;
                            length = 0;

                            // NOTE if numDays <= 0 after this then
                            // grp.end - grp.start + 1 <= grp.end - day
                            // => day  + 1 <= grp.start
                            // => day < grp.start

                            if (numDays <= 0)
                            {
                                string oops = "day: " + day.ToShortDateString() + " start: " + grp.start.ToShortDateString() + " end: " + grp.end.ToShortDateString();
                                throw (new Exception("Error in effective grp calculation " + oops));
                            }
                        }

                        // algorithm given grp from start to end (inclusive)
                        // daily grp or dailyGrp = grp/numDays for each day d in range
                        // and numDays = end - start + 1 is number of days in span
                        // let d be offset from end so d = 0, 1, ... numDays - 1
                        // for a given d the effective grp on this day is
                        // egrp(d) = dailyGrp * scale^(day - (end - d)) 
                        // egrp(d) = dailyGrp * scale^(length + d)
                        // Sum from d = 0,.. numDays - 1 we get
                        // egrp = dailyGrp * scale^length * (1 - scale^numDays)/decay

                        if (post_decay) 
                        {
                            egrp += dailyGrp * prob.share * Math.Pow( post_scale, length ) * (1 - Math.Pow( post_scale, numDays )) / post_use_persusion_decay;
                        }
                        else
                        {
                            egrp += prob.share * dailyGrp * numDays;
                        }

                        if(pre_decay)
                        {
                            egrp += dailyGrp *(1 - prob.share) * Math.Pow( pre_scale, length ) * (1 - Math.Pow( pre_scale, numDays )) / pre_use_persuasion_decay;
                        }
                        
                        else
                        {
                            egrp += (1.0 - prob.share) * dailyGrp * numDays;
                        }
                    }
                    day = day.AddDays(1);
                }

                egrps.Add( egrp / date_span.Days );
            }

            return egrps;
        }
        #endregion
    }
}
