using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

using MrktSimDb;
using ModelView;

namespace MrktSimClient.Controls
{
    public abstract class MsProjectNode : TreeNode
    {
        public enum Type
        {
            Db,
            Project,
            Model,
            Scenario,
            Simulation
        }

        #region Fields and Properties

        public bool ActiveNode = false;

        public MsProjectNode.Type NodeType;

        public override string ToString()
        {
            return this.Text;
        }

        // the database
        private ProjectDb theDb;
        public ProjectDb Db
        {
            set
            {
                theDb = value;
            }

            get
            {
                return theDb;
            }
        }

        protected int id = Database.AllID;
        public int Id
        {
            get { return id; }
        }

        protected UserControl control = null;
        public UserControl Control
        {
            get
            {
                if (control == null)
                {
                    control = createControl();

                    if (ParentNode != null)
                        control.Enabled = ParentNode.Control.Enabled;
                }

                return control;
            }

            //set
            //{
            //    control = value;
            //}
        }

        public MsProjectNode ParentNode
        {
            get
            {
                return (MsProjectNode)Parent;
            }
        }

        public bool Suspend
        {
            set
            {
                if (control != null)
                {
                    if (value)
                    {
                        control.SuspendLayout();
                    }
                    else
                    {
                        control.ResumeLayout();
                    }

                    foreach (MsProjectNode node in Nodes)
                    {
                        node.Suspend = value;
                    }
                }
            }
        }

        #endregion


        #region Interface
        protected abstract UserControl createControl();
        protected abstract DataRow findChildDataById(int id);
        protected abstract DataRow[] getChildData(DataViewRowState state);
        protected abstract string childKey
        {
            get;
        }

        protected abstract void add(DataRow row);
        #endregion


        #region Methods
        public MsProjectNode(DataRow row, ProjectDb db)
        {
            this.Tag = row;
            Db = db;
        }

        private MsProjectNode findChildNodeById(int id)
        {
            foreach(MsProjectNode node in Nodes)
            {
                if (node.id == id)
                    return node;
            }

            return null;
        }

        // creates tree from database
        public void Initialize()
        {
            DataRow[] rows = getChildData(DataViewRowState.CurrentRows);

            foreach (DataRow row in rows)
            {
                add(row);
            }
        }

        /// <summary>
        /// refreshes the entire tree
        /// do this after a db refresh
        /// </summary>
        public void RefreshTree()
        {

            if (this.Parent == null)
            {
                // at the top begin the refresh
                Refresh();
            }
            else
            {
                this.ParentNode.RefreshTree();
            }
        }

        public void Refresh()
        {
            // find obsolete nodes
            bool deleting = true;
            while (deleting)
            {
                deleting = false;

                foreach (MsProjectNode node in Nodes)
                {
                    if (((DataRow)node.Tag).RowState == System.Data.DataRowState.Detached)
                    {
                        DataRow row = this.findChildDataById(node.Id);

                        if (row != null)
                            node.Tag = row;
                        else
                        {
                            Nodes.Remove(node);
                            deleting = true;
                            break;
                        }
                    }
                }
            }

            // my name

            this.Text = this.ToString();

            // refresh exising nodes
            foreach (MsProjectNode node in Nodes)
            {
                node.Refresh();
            }

            // add new nodes

            DataRow[] rows = getChildData(DataViewRowState.CurrentRows);

            foreach (DataRow row in rows)
            {
                int rowID = (int) row[childKey];

                // find row in current nodes
                if (findChildNodeById(rowID) == null)
                {
                    add(row);
                }
            }
        }

        public bool Enabled
        {
            set
            {
                // nothing doing
                if (control == null)
                    return;

                Control.Enabled = value;

                foreach (MsProjectNode node in Nodes)
                {
                    node.Enabled = value;
                }
            }

            get
            {
                if (control != null)
                {
                    return Control.Enabled;
                }

                return false;
            }
        }

        // delete item
        public void DeleteItem()
        {
            ((DataRow) this.Tag).Delete();

            if (this.Parent != null)
                this.Parent.Nodes.Remove(this);
        }


        /// <summary>
        /// Find node of given id and type amonst children
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public MsProjectNode Find(int anId, Type aType)
        {
            if (Id == anId && NodeType == aType)
                return this;

            foreach (MsProjectNode child in Nodes)
            {
                MsProjectNode found = child.Find(anId, aType);

                if (found != null)
                    return found;
            }

            return null;
        }

        #endregion
    }

    public class MsDbNode : MsProjectNode
    {
        public override string ToString()
        {
            if (Db != null && Db.Connection != null)
                return Db.Connection.Database;

            return null;
        }

        protected override UserControl createControl()
        {
            DbControl control = new DbControl();

            control.Node = this;

            return control;
        }

        protected override DataRow findChildDataById(int id)
        {
            return Db.Data.project.FindByid(id);
        }

        protected override string childKey
        {
            get { return "id"; }
        }

    
        protected override DataRow[] getChildData(DataViewRowState state)
        {
            return Db.Data.project.Select("", "", state);
        }

        protected override void add(DataRow row)
        {
            MsTopProjectNode aNode = new MsTopProjectNode(row, Db);

            this.Nodes.Add(aNode);

        }


        public MsDbNode(ProjectDb db)
            : base(null, db)
        {
            NodeType = MsProjectNode.Type.Db;
            this.Text = db.Connection.Database;
            this.id = Database.AllID;

            Initialize();
        }

        public bool OpenModel()
        {
            foreach (MsTopProjectNode node in Nodes)
            {
                if (node.OpenModel())
                    return true;
            }

            return false;
        }
    }

    // top level node that display project data
    public class MsTopProjectNode : MsProjectNode
    {
        public override string ToString()
        {
            if (Project.RowState != DataRowState.Detached &&
                Project.RowState != DataRowState.Deleted)
                return Project.name;

            return null;
        }

        protected override UserControl createControl()
        {
            ProjectControl control = new ProjectControl();

            control.Node = this;

            return control;
        }

        protected override DataRow findChildDataById(int id)
        {
            return Db.Data.Model_info.FindBymodel_id(id);
        }

        protected override string childKey
        {
            get { return "model_id"; }
        }

    

        protected override DataRow[] getChildData(DataViewRowState state)
        {

            string query = "project_id = " + this.Id;

            if( ProjectDb.AppCode != "" ) {
                query += " AND app_code = '" + ProjectDb.AppCode + "'";
            }

            return Db.Data.Model_info.Select(query, "", state);
        }

        protected override void add(DataRow row)
        {
            MsModelNode aModelNode = new MsModelNode(row, Db);

            this.Nodes.Add(aModelNode);
        }

        // return row in database corresponding to this node
        public MrktSimDBSchema.projectRow Project
        {
            get
            {
                return (MrktSimDBSchema.projectRow)this.Tag;
            }
        }


        public MsTopProjectNode(DataRow row, ProjectDb db)
            : base(row, db)
        {
            NodeType = MsProjectNode.Type.Project;
            this.Text = row["name"].ToString();
            this.id = (int) row["id"];

            Initialize();
        }

        public bool OpenModel()
        {
            foreach (MsModelNode node in Nodes)
            {
                if (node.OpenModel())
                    return true;
            }

            return false;
        }
    }


    // displays model tree in project pane
    public class MsModelNode : MsProjectNode
    {
        public override string ToString()
        {
            if (Model.RowState != DataRowState.Detached &&
                Model.RowState != DataRowState.Deleted)
                return Model.model_name;

            return null;
        }

        protected override UserControl createControl()
        {
            ModelControl control = new ModelControl();

            control.Node = this;

            return control;
        }

        protected override DataRow findChildDataById(int id)
        {
            return Db.Data.scenario.FindByscenario_id(id);
        }

        protected override DataRow[] getChildData(DataViewRowState state)
        {
            string query = "model_id = " + id;
            return Db.Data.scenario.Select(query, "", state);
        }

        protected override string childKey
        {
            get { return "scenario_id"; }
        }

        protected override void add(DataRow row)
        {
            MsScenarioNode aScenarioNode = new MsScenarioNode(row, Db);

            this.Nodes.Add(aScenarioNode);
        }


        public MsModelNode(DataRow row, ProjectDb db)
            : base(row, db)
        {
            NodeType = MsProjectNode.Type.Model;

            this.Text = row["model_name"].ToString();
            this.id = (int) row["model_id"];

            this.Initialize();
           
        }


        public bool OpenModel()
        {
            if (control == null)
            {
                return false;
            }

            ModelEditor form = ((ModelControl)Control).ModelEditorForm;

            if (form != null)
            {
                return true;
            }

            // now check if we are editing a sim
            foreach( MsScenarioNode node in Nodes ) {
                if( node.OpenModel() )
                    return true;
            }

            return false;
        }

        // return row in database corresponding to this node
        public MrktSimDBSchema.Model_infoRow Model
        {
            get
            {
                return (MrktSimDBSchema.Model_infoRow)this.Tag;
            } 
        }
    }

    // displays model tree in project pane
    public class MsScenarioNode : MsProjectNode
    {
        public override string ToString()
        {
            if (Scenario.RowState != DataRowState.Detached &&
                Scenario.RowState != DataRowState.Deleted)
                return Scenario.name;

            return null;
        }

        protected override string childKey
        {
            get { return "id"; }
        }

        protected override UserControl createControl()
        {
            ScenarioControl control = new ScenarioControl();

            control.Node = this;

            return control;
        }

        protected override DataRow findChildDataById(int id)
        {
            return Db.Data.simulation.FindByid(id);
        }

        protected override DataRow[] getChildData(DataViewRowState state)
        {
            string query = "scenario_id = " + id;
            return Db.Data.simulation.Select(query, "", state);
        }

        protected override void add(DataRow row)
        {
            MsSimulationNode node = new MsSimulationNode(row, Db);

            this.Nodes.Add(node);
        }

        public MsScenarioNode(DataRow row, ProjectDb db)
            : base(row, db)
        {
            NodeType = MsProjectNode.Type.Scenario;

            this.Text = row["name"].ToString(); ;
            id = (int) row["scenario_id"];

            this.Initialize();
        }

        // return row in database corresponding to this node
        public MrktSimDBSchema.scenarioRow Scenario
        {
            get
            {
                return (MrktSimDBSchema.scenarioRow)this.Tag;
            }
        }

        public bool OpenModel() {
            if( control == null ) {
                return false;
            }

            ScenarioControl scenarioControl = (ScenarioControl) this.Control;

            if( scenarioControl.NumOpenSims > 0 ) {
                return true;
            }

            return false;
        }
    }

    public class MsSimulationNode : MsProjectNode
    {
        public override string ToString()
        {
            if (Simulation.RowState != DataRowState.Detached &&
                Simulation.RowState != DataRowState.Deleted)
                return Simulation.name;

            return null;
        }

        protected override string childKey
        {
            get { return "id"; }
        }

        protected override UserControl createControl()
        {
            SimulationControl control = new SimulationControl();

            control.Node = this;

            return control;
        }

        protected override DataRow findChildDataById(int id)
        {
            return null;
        }

        protected override DataRow[] getChildData(DataViewRowState state)
        {
            return new DataRow[0];
        }

        protected override void add(DataRow row)
        {
        }



        public MsSimulationNode(DataRow row, ProjectDb db)
            : base(row, db)
        {
            NodeType = MsProjectNode.Type.Simulation;


            this.Text = row["name"].ToString(); ;
            id = (int) row["id"];

            this.Initialize();
        }

        // return row in database corresponding to this node
        public MrktSimDBSchema.simulationRow Simulation
        {
            get
            {
                return (MrktSimDBSchema.simulationRow)this.Tag;
            }

            set
            {
                this.Tag = value;
                this.Text = value.name;
                id = value.id;
            }
        }
    }
}
