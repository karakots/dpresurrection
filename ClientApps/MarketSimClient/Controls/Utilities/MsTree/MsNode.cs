using System;
using System.Windows.Forms;
using MrktSimDb;

namespace MarketSimUtilities.MsTree
{
	/// <summary>
	/// Used as the Tag object in tree
	/// supports database interface
	/// </summary>

	public enum MsNodeType 
	{
		// project Types
		topProjectNodeType,

		topProjectModelNodeType,
		projectModelNodeType,

		// model Types
		ModelNodeType,

		// product tree
		ProductNodeType,

		InitialConditionNodeType,
		ProductDependType,
		ProductTypeNodeType,
        ProductPackSizeType,

        PriceType,

		// segments
		SegmentNodeType,

		SocialNetworkType,
		NetworkModelType,

		// channel
		ChannelNodeType,

		// Tasks
		TaskNodeType,

		//attributes
		AttributeNodeType,

		// Marketing Data
		MarketDataNodeType,
		PriceNodeType,
		DisplayNodeType,
		MarketUtilityNodeType,
		DistributionNodeType,
		MediaNodeType,
		EventNodeType,

		CouponNodeType,

		ScenarioNodeType,

		ModelParameterType,
        ExternalDataType,

        CalibrationNodeType,
	};


	// Node for display and traversing
	public abstract class MrktSimTreeNode : TreeNode
	{
		private MsNodeType nodeType;

		public MsNodeType NodeType
		{
			get
			{
				return nodeType;
			}

			set
			{
				nodeType = value;
			}
		}
		
		// context switching
		public virtual string NewString
		{
			get
			{
				return null;
			}
		}

		public virtual string DeleteString
		{
			get
			{
				return null;
			}
		}


		// some nodes are static
		public abstract bool ReadOnly();

		// create a new item 
		public abstract bool CreateNewItem();

		// deletes an item
		public abstract void DeleteItem();

		// Must implement this
		public abstract void Initialize();
	}
}

