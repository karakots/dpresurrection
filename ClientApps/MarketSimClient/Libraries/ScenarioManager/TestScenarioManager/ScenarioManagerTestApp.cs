using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;

// Build Intructions

// You will need to add the following references
// from the ScenarioManagerLibrary folder
// MarketSimScenarioManagerLibrary.dll
// MrktSimDb.dll
// Math Utility.dll

// Referencing a database connection

// The program references a file
// default.udl
// this file should be edited to reference a valid NIMO database
// the working directory should reference the folder this file is in.
// A sample default.udl file is included in the
// ScenarioManagerTestApp solution folder

using DecisionPower.MarketSim.ScenarioManagerLibrary;
using DecisionPower.MarketSim.ScenarioManagerLibrary.Components;
using DecisionPower.MarketSim.ScenarioManagerLibrary.Data;


namespace TestScenarioManager
{
    public partial class ScenarioManagerTestApp : Form
    {
        private string connectionFile = "default.udl";

        private DecisionPower.MarketSim.ScenarioManagerLibrary.Database database;
        private int curProjectIndex = 0;
        private int curModelIndex = 0;
        private int curScenariolndex = 0;
        private int curMarketPlanIndex = 0;
        private int curComponentIndex = 0;

        private Model testModel;
        private Scenario testScenario;
        private MarketPlan testMarketPlan;
        private Component testComponent;

        public ScenarioManagerTestApp() {
            InitializeComponent();
        }

        private void testConnectionButton_Click( object sender, EventArgs e ) {
            ErrorInfo errorInfo = null;
            this.database = null;

            string connectionFilePath = Application.StartupPath + "\\" + connectionFile;

            this.Cursor = Cursors.WaitCursor;
            this.database = DecisionPower.MarketSim.ScenarioManagerLibrary.Database.ValidateDBConnection( connectionFilePath, out errorInfo );
            this.Cursor = Cursors.Default;

            if( database == null ) {
                // there was an error!

                string errorMsg = String.Format( "\r\n\r\n    Error: {0}    \r\n    {1}    \r\n\r\n", errorInfo.Summary, errorInfo.Details );
                MessageBox.Show( errorMsg, "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                this.databaseLabel.Text = "Database: Connection error";
                return;
            }

            // database connection is OK
            testGroup.Enabled = true;
            testConnectionButton.Enabled = false;
            this.databaseLabel.Text = "Database: " + database.DBConnection.Database;
            Project project = this.database.Projects[ curProjectIndex ];
            DisplayCurrentProject();
            MessageBox.Show( "\r\n\r\n    MarketSim Database Connection OK    \r\n\r\n", "Database Connection OK", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }

        private void projectsButton_Click( object sender, EventArgs e ) {
            Project[] projects = this.database.Projects;
            if( projects.Length == 0 ) {
                MessageBox.Show( "\r\n    There are no projects in the database    \r\n", "Projects", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
            else {
                string s = "\r\n    Projects:    \r\n";
                for( int p = 0; p < projects.Length; p++ ) {
                    s += "    " + projects[ p ].Name + "    \r\n";
                }
                MessageBox.Show( s, "Projects", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
        }

        private void modelsButton_Click( object sender, EventArgs e ) {
            Project[] projects = this.database.Projects;
            if( projects.Length == 0 ) {
                MessageBox.Show( "\r\n    There are no projects in the database    \r\n", "Projects", MessageBoxButtons.OK, MessageBoxIcon.Information );
                return;
            }

            Project project = projects[ curProjectIndex ];

            Model[] models = project.Models;
            string s = String.Format( "\r\n    Models -- Project: \"{0}\":    \r\n\r\n", project.Name );
            if( models.Length == 0 ) {
                s += "\r\n    0 models in project    \r\n";
                MessageBox.Show( s, "Models", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
            else {
                testModel = models[ 0 ];
                testModelLabel.Text = testModel.Name;

                for( int m = 0; m < models.Length; m++ ) {
                    s += "    " + models[ m ].Name + "    \r\n";
                }
                s += "\r\n";
                MessageBox.Show( s, "Models", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
        }

        private void openModelButton_Click( object sender, EventArgs e ) {
            if( testModel == null ) {
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            testModel.OpenForEditing();
            this.Cursor = Cursors.Default;

            MessageBox.Show( "Opened model for editing!" );

            Scenario[] scenarios = testModel.Scenarios;

            testModelLabel.Text = String.Format( "{0} -- open ({1} scenarios)", testModel.Name, scenarios.Length );
            modelGroupBox.Enabled = true;
            openModelButton.Enabled = false;
            nextProjectButton.Enabled = false;
            nextModelButton.Enabled = false;
            modelGroupBox.Text = "Model - " + testModel.Name;

            if( scenarios.Length > 0 ) {
                testScenario = scenarios[ curScenariolndex ];
                if( testScenario.MarketPlans.Length > 0 ) {
                    testMarketPlan = testScenario.MarketPlans[ curMarketPlanIndex ];
                    if( testMarketPlan.Components.Length > 0 ) {
                        testComponent = testMarketPlan.Components[ curComponentIndex ];
                    }
                }
            }
            DisplayCurrentScenario();
        }

        private void scenariosButton_Click( object sender, EventArgs e ) {
            Scenario[] scenarios = testModel.Scenarios;

            string s = "";
            for( int i = 0; i < scenarios.Length; i++ ) {
                s += "    " + scenarios[ i ].Name + "    \r\n";
            }

            MessageBox.Show( s, "Scenarios", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }

        private void plansButton_Click( object sender, EventArgs e ) {
            Scenario[] scenarios = testModel.Scenarios;
            if( scenarios.Length == 0 ) {
                MessageBox.Show( "\r\n    There are no scenarios in the model    \r\n", "Scenarios", MessageBoxButtons.OK, MessageBoxIcon.Information );
                return;
            }

            // just in case there are less than before
            if( curScenariolndex > scenarios.Length - 1 ) {
                curScenariolndex = 0;
            }

            curScenariolndex++;
            testScenario = testModel.Scenarios[ curScenariolndex ];

            MarketPlan[] marketPlans = testScenario.MarketPlans;

            string s = "";
            for( int i = 0; i < marketPlans.Length; i++ ) {
                s += "    " + marketPlans[ i ].Name + "    \r\n";
            }

            MessageBox.Show( s, "Market Plans", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }

        private void componentsButton_Click( object sender, EventArgs e ) {
            MarketPlan[] marketPlans = testScenario.MarketPlans;
            if( marketPlans.Length == 0 ) {
                MessageBox.Show( "\r\n    There are no market plans in the scenario    \r\n", "Market Plans", MessageBoxButtons.OK, MessageBoxIcon.Information );
                return;
            }

            testMarketPlan = marketPlans[ curMarketPlanIndex ];

            Component[] components = testMarketPlan.Components;

            string s = "Components:  \r\n\r\n";
            for( int i = 0; i < components.Length; i++ ) {
                s += "    " + components[ i ].Name + "    \r\n";
            }

            MessageBox.Show( s, "Components", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }

        private void nextPlanButton_Click( object sender, EventArgs e ) {
            curScenariolndex++;
            if( curScenariolndex >= testModel.Scenarios.Length ) {
                curScenariolndex = 0;
            }

            if( testModel.Scenarios.Length > 0 ) {
                testScenario = testModel.Scenarios[ curScenariolndex ];
            }
            else {
                testScenario = null;
            }

            DisplayCurrentScenario();
        }

        private void nextCompButton_Click( object sender, EventArgs e ) {
            curMarketPlanIndex++;
            if( curMarketPlanIndex >= testScenario.MarketPlans.Length ) {
                curMarketPlanIndex = 0;
            }

            if( testScenario.MarketPlans.Length > 0 ) {
                testMarketPlan = testScenario.MarketPlans[ curMarketPlanIndex ];
            }
            else {
                testMarketPlan = null;
            }

            DisplayCurrentMarketPlan();
        }

        private void nextProjectButton_Click( object sender, EventArgs e ) {
            curProjectIndex++;
            if( curProjectIndex >= this.database.Projects.Length ) {
                curProjectIndex = 0;
            }
            Project project = this.database.Projects[ curProjectIndex ];

            Model[] models = project.Models;
            if( models.Length > 0 ) {
                testModel = models[ 0 ];
                testModelLabel.Text = String.Format( "Model: {0}", testModel.Name );
            }
            DisplayCurrentProject();
        }

        private void bextDataButton_Click( object sender, EventArgs e ) {
            curComponentIndex++;
            if( curComponentIndex >= testMarketPlan.Components.Length ) {
                curComponentIndex = 0;
            }

            testComponent = testMarketPlan.Components[ curComponentIndex ];

            int dataRowCount = 0;
            if( testComponent is CouponsComponent ) {
                CouponsComponent cComp = (CouponsComponent)testComponent;
                CouponsComponentData[] cData = cComp.GetData( DateTime.MinValue, DateTime.MaxValue, Enums.DataIntervalCheckType.DataEndsInInterval );
                dataRowCount = cData.Length;
            }
            else if( testComponent is DisplayComponent ) {
                DisplayComponent dyComp = (DisplayComponent)testComponent;
                DisplayComponentData[] dyData = dyComp.GetData( DateTime.MinValue, DateTime.MaxValue, Enums.DataIntervalCheckType.DataEndsInInterval );
                dataRowCount = dyData.Length;
            }
            else if( testComponent is DistributionComponent ) {
                DistributionComponent dComp = (DistributionComponent)testComponent;
                DistributionComponentData[] dData = dComp.GetData( DateTime.MinValue, DateTime.MaxValue, Enums.DataIntervalCheckType.DataEndsInInterval );
                dataRowCount = dData.Length;
            }
            else if( testComponent is MediaComponent ) {
                MediaComponent mComp = (MediaComponent)testComponent;
                MediaComponentData[] mData = mComp.GetData( DateTime.MinValue, DateTime.MaxValue, Enums.DataIntervalCheckType.DataEndsInInterval );
                dataRowCount = mData.Length;
            }
            else if( testComponent is PriceComponent ) {
                PriceComponent pComp = (PriceComponent)testComponent;
                PriceComponentData[] pData = pComp.GetData( DateTime.MinValue, DateTime.MaxValue, Enums.DataIntervalCheckType.DataEndsInInterval );
                dataRowCount = pData.Length;
            }
            else if( testComponent is MarketUtilityComponent ) {
                MarketUtilityComponent muComp = (MarketUtilityComponent)testComponent;
                MarketUtilityComponentData[] muData = muComp.GetData( DateTime.MinValue, DateTime.MaxValue, Enums.DataIntervalCheckType.DataEndsInInterval );
                dataRowCount = muData.Length;
            }

            DisplayCurrentComponent();
        }

        private void copyCompButton_Click( object sender, EventArgs e ) {
            Component copyComponent = this.testComponent.Copy( "Copied Component", "CDesc" );
            this.testMarketPlan.AddComponent( copyComponent );
            DisplayCurrentComponent();

            MessageBox.Show( "\r\n     Component Copied OK    \r\n", "Copy Ok" );
        }

        private void copyPlanButton_Click( object sender, EventArgs e ) {
            MarketPlan copyPlan = this.testMarketPlan.ShallowCopy( "Copied Plan", "Copy descr" );
            this.testScenario.AddMarketPlan( copyPlan );
            DisplayCurrentMarketPlan();

            MessageBox.Show( "\r\n     Market Plan Copied OK    \r\n", "Copy Ok" );
        }

        private void copyScenarioButton_Click( object sender, EventArgs e ) {
            Scenario copyScenario = this.testScenario.ShallowCopy( "Copied Scenario", "Copy Descr" );
            DisplayCurrentScenario();

            MessageBox.Show( "\r\n     Scenario Copied OK    \r\n", "Copy Ok" );
        }

        private void saveButton_Click( object sender, EventArgs e ) {
            testModel.SaveAllChanges();
            MessageBox.Show( "Data Saved" );
        }

        private void dataButton_Click( object sender, EventArgs e ) {
            if( this.testComponent == null ) {
                return;
            }

            DataForm dataForm = new DataForm( this.testComponent );
            DialogResult resp = dataForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
        }

        private void AddDataButton_Click( object sender, EventArgs e ) {
            //if( testComponent is CouponsComponent ) {
            //    CouponsComponentData[] newItems = new CouponsComponentData[ 2 ];
            //    Product p = testComponent.Product;
            //    newItems[ 0 ] = new CouponsComponentData( testModel, testComponent.Product, testComponent.Channel, DateTime.Now, DateTime.Now, 100, 10 );
            //    newItems[ 1 ] = new CouponsComponentData( testModel, testComponent.Product, testComponent.Channel, DateTime.Now, DateTime.Now, 100, 10 );
            //}
            if( testComponent is DisplayComponent ) {
                DisplayComponentData[] newItems = new DisplayComponentData[ 2 ];
                Product p = testComponent.Product;
                newItems[ 0 ] = new DisplayComponentData( testModel, testComponent.Product, testComponent.Channel, DateTime.Now, DateTime.Now, 100 );
                newItems[ 1 ] = new DisplayComponentData( testModel, testComponent.Product, testComponent.Channel, DateTime.Now, DateTime.Now, 100 );

                newItems[ 0 ].Awareness = 50;
                newItems[ 0 ].Persuasion = 50;
                newItems[ 1 ].Awareness = 50;
                newItems[ 1 ].Persuasion = 50;

                DisplayComponent dc = (DisplayComponent)testComponent;
                dc.AddData( newItems );
            }
            else if( testComponent is MediaComponent ) {
                MediaComponentData[] newItems = new MediaComponentData[ 2 ];
                Product p = testComponent.Product;
                newItems[ 0 ] = new MediaComponentData( testModel, testComponent.Product, testComponent.Channel, DateTime.Now, DateTime.Now, 100 );
                newItems[ 1 ] = new MediaComponentData( testModel, testComponent.Product, testComponent.Channel, DateTime.Now, DateTime.Now, 100 );

                newItems[ 0 ].Awareness = 50;
                newItems[ 0 ].Persuasion = 50;
                newItems[ 1 ].Awareness = 50;
                newItems[ 1 ].Persuasion = 50;

                MediaComponent mc = (MediaComponent)testComponent;
                mc.AddData( newItems );
            }
            else {
                MessageBox.Show( "Error: Selected component must be a Display or Media component!" );
            }
        }

        private void productsButton_Click( object sender, EventArgs e ) {
            Product[] products = testModel.Products;
            ProductType[] productTypes = testModel.ProductTypes;
            string s = String.Format( "\r\n    Products in Model ( {0} ):    \r\n", products.Length );
            s += "    -------------------------    \r\n";
            foreach( ProductType ptype in productTypes ) {
                s += String.Format( " Product Type: {0}    \r\n", ptype.Name );
                products = testModel.GetProducts( ptype );
                foreach( Product prod in products ) {
                    s += String.Format( "      {0}    \r\n", prod.Name );
                }
                s += "\r\n";
            }

            MessageBox.Show( s, "Products" );
        }

        private void channelsButton_Click( object sender, EventArgs e ) {
            Channel[] channels = testModel.Channels;
            string s = String.Format( "\r\n    Channels in Model ( {0} ):    \r\n", channels.Length );
            s += "    -------------------------    \r\n";
            foreach( Channel chan in channels ) {
                s += String.Format( "      {0}    \r\n", chan.Name );
            }
            s += "\r\n"; 

            MessageBox.Show( s, "Channels" );
        }

        private void segmentsButton_Click( object sender, EventArgs e ) {
            Segment[] segments = testModel.Segments;
            string s = String.Format( "\r\n    Segments in Model ( {0} ):    \r\n", segments.Length );
            s += "    -------------------------    \r\n";
            foreach( Segment segment in segments ) {
                s += String.Format( "      {0}    \r\n", segment.Name );
            }
            s += "\r\n"; 

            MessageBox.Show( s, "Segments" );
        }

        private void deleteScenarioButton_Click( object sender, EventArgs e ) {
            DialogResult resp = MessageBox.Show( "\r\n    Ok to delete scenario?    \r\n", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
            if( resp == DialogResult.OK ) {
                this.testModel.DeleteScenario( this.testScenario );
            }

            DisplayCurrentScenario();
        }

        private void deletePlanButton_Click( object sender, EventArgs e ) {
            DialogResult resp = MessageBox.Show( "\r\n    Ok to delete market plan?    \r\n", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
            if( resp == DialogResult.OK ) {
                this.testModel.DeleteMarketPlan( this.testMarketPlan, false );
            }

            DisplayCurrentMarketPlan();
        }

        private void deleteCompButton_Click( object sender, EventArgs e ) {
            DialogResult resp = MessageBox.Show( "\r\n    Ok to delete component?    \r\n", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
            if( resp == DialogResult.OK ) {
                this.testModel.DeleteComponent( this.testComponent );
            }

            DisplayCurrentComponent();
        }

        private void renameScenarioButton_Click( object sender, EventArgs e ) {
            NewNameForm nameForm = new NewNameForm( "Set name and description for scenario", testScenario.Name, testScenario.Description );
            DialogResult resp = nameForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            if( nameForm.ItemName != "" ) {
                testScenario.Name = nameForm.ItemName;
            }
            testScenario.Description = nameForm.ItemDescription;

            DisplayCurrentScenario();
        }

        private void renamePlanButton_Click( object sender, EventArgs e ) {
            NewNameForm nameForm = new NewNameForm( "Set name and description for market plan", testMarketPlan.Name, testMarketPlan.Description );
            DialogResult resp = nameForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            if( nameForm.ItemName != "" ) {
                testMarketPlan.Name = nameForm.ItemName;
            }
            testMarketPlan.Description = nameForm.ItemDescription;

            DisplayCurrentMarketPlan();
        }

        private void renameCompButton_Click( object sender, EventArgs e ) {
            NewNameForm nameForm = new NewNameForm( "Set name and description for component", testComponent.Name, testComponent.Description );
            DialogResult resp = nameForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            if( nameForm.ItemName != "" ) {
                testComponent.Name = nameForm.ItemName;
            }
            testComponent.Description = nameForm.ItemDescription;

            DisplayCurrentComponent();
        }

        private void DisplayCurrentProject() {
            Project project = this.database.Projects[ curProjectIndex ];
            projectLabel.Text = String.Format( "Project[{2} of {3}]: {0} -- {1} models", project.Name, project.Models.Length, curProjectIndex + 1,
                this.database.Projects.Length );
            DisplayCurrentModel();
        }

        private void DisplayCurrentModel() {
            Project project = this.database.Projects[ curProjectIndex ];

            Model[] models = project.Models;
            if( models.Length > 0 ) {

                testModel = project.Models[ curModelIndex ];
                testModelLabel.Text = String.Format( "Model[{1} of {2}]: {0}", testModel.Name, curModelIndex + 1, models.Length );
            }
            else {
                testModelLabel.Text = "0 models in project";
            }
        }

        private void DisplayCurrentScenario() {
            testScenario = testModel.Scenarios[ this.curScenariolndex ];

            if( testScenario != null ) {
                plansLabel.Text = String.Format( "Scenario[{2} of {3}]: {0} -- {1} plans", testScenario.Name, testScenario.MarketPlans.Length,
                   curScenariolndex + 1, testModel.Scenarios.Length );
            }
            else {
                plansLabel.Text = "Scenario: null";
            }
            DisplayCurrentMarketPlan();
        }

        private void DisplayCurrentMarketPlan() {
            if( testMarketPlan != null ) {
                componentsLabel.Text = String.Format( "Market Plan[{2} of {3}]: {0} -- {1} components", testMarketPlan.Name, testMarketPlan.Components.Length,
                    curMarketPlanIndex + 1, testScenario.MarketPlans.Length );
            }
            else {
                componentsLabel.Text = "Market Plan: null";
            }
            DisplayCurrentComponent();
        }

        private void DisplayCurrentComponent() {
            if( testComponent != null ) {
                dataLabel.Text = String.Format( "Component[{0} of {3}]: {1} -- {2} data rows", curComponentIndex + 1, testComponent.Name,
                   testComponent.DataTable.Rows.Count,
                   testMarketPlan.Components.Length );
            }
            else {
                dataLabel.Text = "Component: null";
            }
        }

        private void copyPlanDeepButton_Click( object sender, EventArgs e ) {
            MarketPlan copyPlan = this.testMarketPlan.Copy( "Copied Plan", "Copy descr" );
            this.testScenario.AddMarketPlan( copyPlan );
            DisplayCurrentMarketPlan();
        }

        private void copyScenarioDeepButton_Click( object sender, EventArgs e ) {
            Scenario copyScenario = this.testScenario.Copy( "Copied Scenario", "Copy Descr", false );
            DisplayCurrentScenario();
        }

        private void nextModelButton_Click( object sender, EventArgs e ) {
             Project project = this.database.Projects[ curProjectIndex ];

            Model[] models = project.Models;
            if( models.Length > 0 ) {

                curModelIndex++;
                if( curModelIndex >= models.Length ) {
                    curModelIndex = 0;
                }

                testModel = models[ curModelIndex ];
            }
            else {
                testModel = null;
            }
            DisplayCurrentProject();
        }

        private void button1_Click( object sender, EventArgs e ) {
            if( testComponent == null || testMarketPlan == null ) {
                return;
            }
            bool inPlan = false;
            foreach( Component comp in testMarketPlan.Components ) {
                if( comp.ID == testComponent.ID ) {
                    inPlan = true;
                }
            }
            if( inPlan ) {
                DialogResult resp = MessageBox.Show( "\r\n    Remove component from market plan?    \r\n", "Confirm Remove",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
                if( resp != DialogResult.OK ) {
                    return;
                }
                testMarketPlan.RemoveComponent( testComponent );

                MessageBox.Show( "\r\n    Component Removed    \r\n" );
            }
            else {
                DialogResult resp = MessageBox.Show( "\r\n    Add component to market plan?    \r\n", "Confirm Add",
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
                if( resp != DialogResult.OK ) {
                    return;
                }
                testMarketPlan.AddComponent( testComponent );

                MessageBox.Show( "\r\n    Component Added    \r\n" );
            }
        }

        private void planInOutButton_Click( object sender, EventArgs e ) {
            if( testScenario == null || testMarketPlan == null ) {
                return;
            }
            bool inPlan = false;
            foreach( MarketPlan plan in testScenario.MarketPlans ) {
                if( plan.ID == testMarketPlan.ID ) {
                    inPlan = true;
                }
            }
            if( inPlan ) {
                DialogResult resp = MessageBox.Show( "\r\n    Remove market plan from scenario?    \r\n", "Confirm Remove", 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
                if( resp != DialogResult.OK ) {
                    return;
                }

                testScenario.RemoveMarketPlan( testMarketPlan );

                MessageBox.Show( "\r\n    Plan Removed    \r\n" );
            }
            else {
                DialogResult resp = MessageBox.Show( "\r\n    Add market plan to scenario?    \r\n", "Confirm Add",
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question );
                if( resp != DialogResult.OK ) {
                    return;
                }
                testScenario.AddMarketPlan( testMarketPlan );

                MessageBox.Show( "\r\n    Plan Added    \r\n" );
            }
        }

        private void ScenarioManagerTestApp_FormClosing( object sender, FormClosingEventArgs e ) {
            if( testModel != null ) {
                testModel.Close( false );
            }
        }

        private void button1_Click_1( object sender, EventArgs e ) {
            Scenario copyScenario = this.testScenario.Copy( "Copied Scenario", "Copy Descr", true );
            DisplayCurrentScenario();
        }

        private void planPropsButton_Click( object sender, EventArgs e ) {
            string msg = null;
            if( this.testMarketPlan != null ) {
                Product p = this.testMarketPlan.Product;
                if( p != null ) {
                    msg = "Product for Plan = " + p.Name;
                }
                else {
                    msg = "The current Market Plan has a NULL Product.";
                }
            }
            else {
                msg = "The current Market Plan is null.";
            }

            MessageBox.Show( msg );
        }

        private void compPropsButton_Click( object sender, EventArgs e ) {
            string msg = null;
            if( this.testComponent != null ) {
                Channel c = this.testComponent.Channel;
                Segment s = this.testComponent.Segment;
                string cname = "NULL";
                string sname = "NULL";
                if( c != null ) {
                    cname = c.Name;
                }
                if( s != null ) {
                    sname = s.Name;
                }
                msg = String.Format( "Current component segment={0}, channel={1}", sname, cname );
            }
            else {
                msg = "The current Component is null.";
            }

            MessageBox.Show( msg );
        }

        private void priceTypesBut_Click( object sender, EventArgs e )
        {
            Model.PriceType[] priceTypes = testModel.PriceTypes;

            string s = String.Format( "\r\n    Price Types in Model ( {0} ):    \r\n", priceTypes.Length);
            s += "    -------------------------    \r\n";
            foreach( Model.PriceType priceType in priceTypes )
            {
                string rel = "relative";

                if( priceType.Absolute )
                {
                    rel = "absolute";
                }

                string bogn = "";

                if( priceType.BOGN > 0 )
                {
                    bogn = String.Format( " Buy One get {0} free", priceType.BOGN );
                }

                s += String.Format( "      {0} ({1}) : {2}  \r\n", priceType.Name, rel, bogn );
            }
            s += "\r\n";

            MessageBox.Show( s, "Price Types" );
        }
    }
}