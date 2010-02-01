using System;
using System.Data;
using System.Windows.Forms;
using MrktSimDb;
// using Common.Utilities;
namespace MarketSimUtilities.MsTree
{
	/// <summary>
	/// Summary description for ModelTree.
	/// </summary>
	//base class for Model View Nodes

	public abstract class MsControlNode : MrktSimTreeNode
	{
		private MrktSimControl control = null;

		public MrktSimControl Control
		{
			get
			{
				return control;
			}

			set
			{
				control = value;
			}
		}

		// might need to query the db
		public void DbUpdate()
		{
			if (Control != null)
				Control.DbUpate();

			foreach(MsControlNode node in Nodes)
				node.DbUpdate();
		}


		public void Reset()
		{
			if (Control != null)
				Control.Reset();

			Initialize();

			foreach(MsControlNode node in Nodes)
				node.Reset();
		}

		public void Flush()
		{
			if (Control != null)
				Control.Flush();

			foreach(MsControlNode node in Nodes)
				node.Flush();
		}

		public void Refresh()
		{
			if (Control != null)
				Control.Refresh();

			Initialize();

			foreach(MsControlNode node in Nodes)
				node.Refresh();
		}

		public bool Suspend
		{
			set
			{
				if (Control != null)
					Control.Suspend = value;

				foreach(MsControlNode node in Nodes)
					node.Suspend = value;
			}
		}

		private ModelDb theDb;

		public ModelDb Db
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

			
		public override bool ReadOnly()
		{
			return true;
		}

		public override bool CreateNewItem()
		{
			return false;
		}

		public override void DeleteItem()
		{
		}
	}

	public class MsControlLeafNode : MsControlNode
	{
		// does not need to initialize
		public override void Initialize() {}
	}


	// nodes for displaying Model Tree
	

	
	// top level node that display project data
	public class MsTopModelNode : MsControlNode
	{
       
		private SegmentNode segmentNode;
		
		private MsControlLeafNode channelNode;
		private MsControlLeafNode taskNode;
		private MsControlLeafNode attributeNode;
		// private MsControlLeafNode optNode;
		private MsControlLeafNode initialConditionNode;
		// private MsControlLeafNode scenarioNode;

		private MsControlLeafNode modelParameterNode;
        private MsControlLeafNode externalDataNode;		

		// not a leaf
		private ProductNode productNode;
		// private MarketingDataNode mrktDataNode;

		public MsTopModelNode()
		{
			NodeType = MsNodeType.ModelNodeType;
			this.Tag = null;
			this.Text = "Model Summary";

          
			// products
			productNode = new ProductNode();

			initialConditionNode = new MsControlLeafNode();
			initialConditionNode.NodeType= MsNodeType.InitialConditionNodeType;
			initialConditionNode.Text = "Initial Conditions";
			
			// segments
			segmentNode = new SegmentNode();
			
			//channels
			channelNode = new MsControlLeafNode();
			channelNode.NodeType = MsNodeType.ChannelNodeType;
			channelNode.Text = "Channels";

			taskNode = new MsControlLeafNode();
			taskNode.NodeType = MsNodeType.TaskNodeType;
			taskNode.Text = "Tasks";
			
			// attributes
			attributeNode = new MsControlLeafNode();
			attributeNode.NodeType = MsNodeType.AttributeNodeType;
			attributeNode.Text = "Attributes";

			// parameters
			modelParameterNode = new MsControlLeafNode();
			modelParameterNode.NodeType = MsNodeType.ModelParameterType;
			modelParameterNode.Text = "Parameters";

            // external data
            externalDataNode = new MsControlLeafNode();
            externalDataNode.NodeType = MsNodeType.ExternalDataType;
            externalDataNode.Text = "Real Sales";
			
			
//			// marketing data
//			mrktDataNode = new MarketingDataNode();
//			mrktDataNode.NodeType = MsNodeType.MarketDataNodeType;
//				
//			// scenarions
//			scenarioNode = new MsControlLeafNode();
//			scenarioNode.NodeType = MsNodeType.ScenarioNodeType;
//			scenarioNode.Text = "Scenarios";
//
//			// Optimization
//			optNode = new MsControlLeafNode();
//			optNode.NodeType = MsNodeType.OptNodeType;
//			optNode.Text = "Optimization";
            this.Expand();
        }

		public override void Initialize()
		{
			Nodes.Clear();

			productNode.Db = Db;
			productNode.Initialize();
			this.Nodes.Add(productNode);

			segmentNode.Db = Db;
			segmentNode.Initialize();
			this.Nodes.Add(segmentNode);

			initialConditionNode.Db = Db;
			initialConditionNode.Initialize();
			this.Nodes.Add(initialConditionNode);

			channelNode.Db = Db;
			channelNode.Initialize();
			this.Nodes.Add(channelNode);
			
			// tasks
			if (Db.Model.task_based)
			{
				taskNode.Db = Db;
				taskNode.Initialize();
				this.Nodes.Add(taskNode);
			}

			attributeNode.Db = Db;
			attributeNode.Initialize();
			this.Nodes.Add(attributeNode);

          

			modelParameterNode.Db = Db;
			modelParameterNode.Initialize();
			this.Nodes.Add(modelParameterNode);

            externalDataNode.Db = Db;
            externalDataNode.Initialize();
            this.Nodes.Add( externalDataNode );

//			mrktDataNode.Db = Db;
//			mrktDataNode.Initialize();
//			this.Nodes.Add(mrktDataNode);
//
//			scenarioNode.Db = Db;
//			scenarioNode.Initialize();
//			this.Nodes.Add(scenarioNode);
//
//
//			optNode.Db = Db;
//			optNode.Initialize();
			//			this.Nodes.Add(optNode);
		}
	}

	public class SegmentNode : MsControlNode
	{
		private MsControlLeafNode socialNetworkNode;
		private MsControlLeafNode networkModelNode;

		public SegmentNode()
		{
			NodeType = MsNodeType.SegmentNodeType;
			Text = "Segments";

			socialNetworkNode = new MsControlLeafNode();
			socialNetworkNode.NodeType = MsNodeType.SocialNetworkType;
			socialNetworkNode.Text = "Social Networks";

			networkModelNode = new MsControlLeafNode();
			networkModelNode.NodeType = MsNodeType.NetworkModelType;
			networkModelNode.Text = "Network Models";
		}

		public override void Initialize()
		{
			Nodes.Clear();

			if (Db.Model.social_network)
			{
				socialNetworkNode.Db = Db;
				socialNetworkNode.Initialize();
				this.Nodes.Add(socialNetworkNode);

				networkModelNode.Db = Db;
				networkModelNode.Initialize();
				this.Nodes.Add(networkModelNode);
			}
		}
	}


	public class ProductNode : MsControlNode
	{
		private MsControlLeafNode productDependNode;
        private MsControlLeafNode productPackSizeNode;
        private MsControlLeafNode priceTypeNode;

		private System.Collections.ArrayList productTypeNodes = new System.Collections.ArrayList();

		public ProductNode()
		{
			NodeType = MsNodeType.ProductNodeType;
			Text = "Products";
		}

		public override void Initialize()
		{

			productDependNode = new MsControlLeafNode();
			productDependNode.NodeType = MsNodeType.ProductDependType;
			productDependNode.Text = "Product Dependencies";

       

			MsControlLeafNode aTypeNode;
			string query = "";
			System.Data.DataRow[] product_types = Db.Data.product_type.Select(query,"", DataViewRowState.CurrentRows);

			productTypeNodes.Clear();

			foreach(MrktSimDBSchema.product_typeRow row in product_types)
			{
                if (row.id != Database.AllID)
                {
                    aTypeNode = new MsControlLeafNode();
                    aTypeNode.NodeType = MsNodeType.ProductTypeNodeType;
                    aTypeNode.Text = row.type_name;
                    aTypeNode.Tag = row;
                    productTypeNodes.Add(aTypeNode);
                }
			}


			Nodes.Clear();

			foreach( MsControlLeafNode node in productTypeNodes )
			{
				node.Db = Db;
				node.Initialize();
				this.Nodes.Add(node);
			}


			if (Db.Model.product_dependency)
			{
				productDependNode.Db = Db;
				productDependNode.Initialize();
				this.Nodes.Add(productDependNode);
			}

            if(Database.Nimo )
            {
                productPackSizeNode = new MsControlLeafNode();
                productPackSizeNode.NodeType = MsNodeType.ProductPackSizeType;
                productPackSizeNode.Text = "Pack Size";
                productPackSizeNode.Db = Db;
                productPackSizeNode.Initialize();
                this.Nodes.Add( productPackSizeNode );
            }

            // price type
            priceTypeNode = new MsControlLeafNode();
            priceTypeNode.NodeType = MsNodeType.PriceNodeType;
            priceTypeNode.Text = "Price Types";
            priceTypeNode.Db = Db;
            priceTypeNode.Initialize();
            this.Nodes.Add( priceTypeNode );

		}
	}

    public class MarketingDataNode : MsControlNode
    {
        private MsControlLeafNode display;
        private MsControlLeafNode market_utility;
        private MsControlLeafNode distribution;
        private MsControlLeafNode price;
        private MsControlLeafNode media;
        private MsControlLeafNode specialEvent;
        private MsControlLeafNode coupons;

        public MarketingDataNode() {
            NodeType = MsNodeType.MarketDataNodeType;
            this.Text = "Marketing Plans";

            price = new MsControlLeafNode();
            price.NodeType = MsNodeType.PriceNodeType;
            price.Text = "Price";

            display = new MsControlLeafNode();
            display.NodeType = MsNodeType.DisplayNodeType;
            display.Text = "Display";

            market_utility = new MsControlLeafNode();
            market_utility.NodeType = MsNodeType.MarketUtilityNodeType;
            market_utility.Text = "Market Utility";

            distribution = new MsControlLeafNode();
            distribution.NodeType = MsNodeType.DistributionNodeType;
            distribution.Text = "Distribution";

            media = new MsControlLeafNode();
            media.NodeType = MsNodeType.MediaNodeType;
            media.Text = "Media";

            specialEvent = new MsControlLeafNode();
            specialEvent.NodeType = MsNodeType.EventNodeType;
            specialEvent.Text = "External Factors";

            coupons = new MsControlLeafNode();
            coupons.NodeType = MsNodeType.CouponNodeType;
            coupons.Text = "Coupons";
        }

        public override void Initialize() {
            this.Nodes.Clear();

            this.Nodes.Add( price );

            if( Db.Model.display ) {
                this.Nodes.Add( display );
            }

            if( Db.Model.distribution ) {
                this.Nodes.Add( distribution );
            }

            this.Nodes.Add( media );

            this.Nodes.Add( coupons );

            if( Db.Model.market_utility ) {
                this.Nodes.Add( market_utility );
            }

            this.Expand();
            //         this.Nodes.Add( specialEvent );
        }
    }

    public class CalibrationNode : MsControlNode
    {
        private MsControlLeafNode display;
        private MsControlLeafNode market_utility;
        private MsControlLeafNode distribution;
        private MsControlLeafNode price;
        private MsControlLeafNode media;
        private MsControlLeafNode specialEvent;
        private MsControlLeafNode coupons;

        public CalibrationNode() {
            NodeType = MsNodeType.CalibrationNodeType;
            this.Text = "Calibration";

            price = new MsControlLeafNode();
            price.NodeType = MsNodeType.PriceNodeType;
            price.Text = "Price";

            display = new MsControlLeafNode();
            display.NodeType = MsNodeType.DisplayNodeType;
            display.Text = "Display";

            market_utility = new MsControlLeafNode();
            market_utility.NodeType = MsNodeType.MarketUtilityNodeType;
            market_utility.Text = "Market Utility";

            distribution = new MsControlLeafNode();
            distribution.NodeType = MsNodeType.DistributionNodeType;
            distribution.Text = "Distribution";

            media = new MsControlLeafNode();
            media.NodeType = MsNodeType.MediaNodeType;
            media.Text = "Media";

            specialEvent = new MsControlLeafNode();
            specialEvent.NodeType = MsNodeType.EventNodeType;
            specialEvent.Text = "External Factors";

            coupons = new MsControlLeafNode();
            coupons.NodeType = MsNodeType.CouponNodeType;
            coupons.Text = "Coupons";
        }

        public override void Initialize() {
            this.Nodes.Clear();

            this.Nodes.Add( price );

            if( Db.Model.display ) {
                this.Nodes.Add( display );
            }

            if( Db.Model.distribution ) {
                this.Nodes.Add( distribution );
            }

            this.Nodes.Add( media );

            this.Nodes.Add( coupons );

            if( Db.Model.market_utility ) {
                this.Nodes.Add( market_utility );
            }

            this.Nodes.Add( specialEvent );
        }
    }
}

