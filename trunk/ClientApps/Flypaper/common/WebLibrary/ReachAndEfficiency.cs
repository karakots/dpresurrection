using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace WebLibrary
{
    /// <summary>
    /// Class used in the accumulation of score for certain media types
    /// </summary>
    [Serializable]
    public class ReachAndEfficiency
    {
        /// <summary>
        /// ID of the associated Media_Info object
        /// </summary>
        public Guid? ItemID;

        /// <summary>
        /// ID of the associated demographic segment
        /// </summary>
        public Guid? SegmentID;

        public string ItemName;
        public string ItemGroup;
        public double Reach;
        public double Efficiency;

        public ReachAndEfficiency() {
        }

        public ReachAndEfficiency( string itemName, Guid? itemID, Guid? segmentID ) {
            this.ItemName = itemName;
            this.ItemID = itemID;
            this.SegmentID = segmentID;
            this.Reach = 0.0;
            this.Efficiency = 0.0;
        }

        public ReachAndEfficiency( ReachAndEfficiency itemToCopy ) {
            this.ItemID = itemToCopy.ItemID;
            this.SegmentID = itemToCopy.SegmentID;
            this.ItemName = itemToCopy.ItemName;
            this.ItemGroup = itemToCopy.ItemGroup;
            this.Reach = itemToCopy.Reach;
            this.Efficiency = itemToCopy.Efficiency;
        }
    }
}