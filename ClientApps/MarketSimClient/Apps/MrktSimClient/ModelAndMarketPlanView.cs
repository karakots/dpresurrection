using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MarketSimUtilities.MsTree;
using MrktSimDb;
using MarketSimSettings;

using MrktSimClient.Controls;
using ModelView.Dialogs;

namespace MrktSimClient
{

    public partial class ModelAndMarketPlanView : ModelView.ModelViewForm
    {
        private MarketPlanControl2 marketPlanControl;             // the control that shows all market plan views

        //private MrktSimDb.ProjectDb projectDb;

        //public MrktSimDb.ProjectDb ProjectDb {
        //    set { projectDb = value; }
        //}

        public ModelAndMarketPlanView()
            : base() {
            //           InitializeComponent();            
            this.Size = new Size( 1100, 600 );
            this.MinimumSize = this.Size;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.ModelAndMarketPlanView_FormClosing );
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.SizeChanged += new EventHandler(ModelAndMarketPlanView_SizeChanged);
            this.LocationChanged += new EventHandler(ModelAndMarketPlanView_LocationChanged);
            this.Load += new EventHandler(ModelAndMarketPlanView_Load);
            // only the baseModel view can save as...
            this.SaveAs = false;

            // create models root node
            MsTopModelNode modelNode = new MsTopModelNode();

            // create market plans root node
            MarketingDataNode marketingDataNode = new MarketingDataNode();

            // SSN 3-8-2007
            // we won't create one just yet until we know what we need
            // create calibration root node
           //  CalibrationNode calibrationNode = new CalibrationNode();

            ArrayList rootNodes = new ArrayList();
            rootNodes.Add( modelNode );
            rootNodes.Add( marketingDataNode );

            // see cooment above
            // rootNodes.Add( calibrationNode );
            this.SetContextNodes( rootNodes );

            int wid = Settings<MrktSim.ClientSettings>.Value.ModelFrameBounds.Width;
            int ht = Settings<MrktSim.ClientSettings>.Value.ModelFrameBounds.Height;
            int x = Settings<MrktSim.ClientSettings>.Value.ModelFrameBounds.X;
            int y = Settings<MrktSim.ClientSettings>.Value.ModelFrameBounds.Y;
            if( x < 2 && y < 2 ) {
                x = 200;                     // reasonable defaults
                y = 150;
            }
            this.Location = new Point( x, y );
            if( wid >= this.MinimumSize.Width && ht >= this.MinimumSize.Height &&
                ((x + wid) < SystemInformation.WorkingArea.Width) &&
                ((y + ht) < SystemInformation.WorkingArea.Height) ) {
                this.Size = Settings<MrktSim.ClientSettings>.Value.ModelFrameBounds.Size;
            }
            else {
                // saved bounds won't fit on the screen!
                x = 30;
                y = 30;
                this.Size = this.MinimumSize;
            }
            ////if( Settings<MrktSim.ClientSettings>.Value.ModelFrameMaximized == true ) {
            ////    this.WindowState = FormWindowState.Maximized;
            ////}
        }


        /// <summary>
        /// Sets the correct node.Control value for the given node type.
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns>true if the control needs a layout call</returns>
        public override bool SetNodeControl( MsControlNode node ) {

            bool layoutNeeded = base.SetNodeControl( node );

            switch( node.NodeType ) {
                case MsNodeType.PriceNodeType:
                    node.Control = marketPlanControl;
                    layoutNeeded = false;
                    break;

                case MsNodeType.DistributionNodeType:
                    node.Control = marketPlanControl;
                    layoutNeeded = false;
                    break;

                case MsNodeType.DisplayNodeType:
                    node.Control = marketPlanControl;
                    layoutNeeded = false;
                    break;

                case MsNodeType.MediaNodeType:
                    node.Control = marketPlanControl;
                    layoutNeeded = false;
                    break;

                case MsNodeType.CouponNodeType:
                    node.Control = marketPlanControl;
                    layoutNeeded = false;
                    break;

                case MsNodeType.MarketUtilityNodeType:
                    node.Control = marketPlanControl;
                    layoutNeeded = false;
                    break;

                case MsNodeType.EventNodeType:
                    node.Control = marketPlanControl;
                    layoutNeeded = false;
                    break;

                case MsNodeType.CalibrationNodeType:
                    node.Control = marketPlanControl;
                    layoutNeeded = false;
                    break;

                case MsNodeType.MarketDataNodeType:
                    marketPlanControl = new MarketPlanControl2( theDb, ModelDb.PlanType.MarketPlan );
                    node.Control = marketPlanControl;
                    layoutNeeded = true;
                    break;
            }
            return layoutNeeded;
        }

        protected override void navigatePane_MouseDown( object sender, System.Windows.Forms.MouseEventArgs e ) {
          
            base.navigatePane_MouseDown( sender, e );

            MsControlNode node = (MsControlNode)navigatePane.GetNodeAt( e.X, e.Y );

            if( node == null ) {
                return;
            }

            // Only a click on one of the market plan tree items will re-activate the same control.
            if( currentControl == node.Control ) {
                MarketPlanControl2 mpControl = currentControl as MarketPlanControl2;
                if( mpControl != null ) {
                    // switch control mode as appropriate for this node
                    switch( node.NodeType ) {
                        case MsNodeType.MarketDataNodeType:
                            mpControl.SetViewFor( ModelDb.PlanType.MarketPlan );
                            break;

                        case MsNodeType.CalibrationNodeType:
                            mpControl.SetViewFor( ModelDb.PlanType.MarketPlan );
                            break;

                        case MsNodeType.PriceNodeType:
                            mpControl.SetViewFor( ModelDb.PlanType.Price );
                            break;

                        case MsNodeType.DistributionNodeType:
                            mpControl.SetViewFor( ModelDb.PlanType.Distribution );
                            break;

                        case MsNodeType.DisplayNodeType:
                            mpControl.SetViewFor( ModelDb.PlanType.Display );
                            break;

                        case MsNodeType.MediaNodeType:
                            mpControl.SetViewFor( ModelDb.PlanType.Media );
                            break;

                        case MsNodeType.CouponNodeType:
                            mpControl.SetViewFor( ModelDb.PlanType.Coupons );
                            break;

                        case MsNodeType.MarketUtilityNodeType:
                            mpControl.SetViewFor( ModelDb.PlanType.Market_Utility );
                            break;

                        case MsNodeType.EventNodeType:
                            mpControl.SetViewFor( ModelDb.PlanType.ProdEvent );
                            break;
                    }
                }
            }
        }


        public override void menuOptions_Click( object sender, System.EventArgs e ) {

            // determine if the model or the market plan view is active
            TreeNode node = navigatePane.SelectedNode;
            if( node != null ) {
                while( node.Parent != null ) {
                    node = node.Parent;
                }
            }
            // now the node is the root node for the selected item's tree

            // show the model or market plans options dialog, as appropriate
            if( node is MarketingDataNode ) {
                // market plans options
                MarketPlanOptionsDialog dlg = new MarketPlanOptionsDialog( this.Db );

                DialogResult rslt = dlg.ShowDialog();

                if( rslt == DialogResult.Cancel ) {
                    return;
                }

                update_nodes();
            }
            else {
                //model options
                ModelOptionsDialog dlg = new ModelOptionsDialog( this.Db );

                DialogResult rslt = dlg.ShowDialog();

                if( rslt == DialogResult.Cancel ) {
                    return;
                }

                update_nodes();
            }
        }

        private void UpdateBoundsSettings() {
            int menuHeight = SystemInformation.MenuHeight;
            Rectangle newBounds = new Rectangle( this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height - menuHeight );

            if( (this.WindowState != FormWindowState.Maximized) && (this.WindowState != FormWindowState.Minimized) ) {
                Settings<MrktSim.ClientSettings>.Value.ModelFrameBounds = newBounds;
            }
        }

        private void ModelAndMarketPlanView_FormClosing( object sender, FormClosingEventArgs e ) {
            UpdateBoundsSettings();

            if( this.WindowState == FormWindowState.Maximized ) {
                Settings<MrktSim.ClientSettings>.Value.ModelFrameMaximized = true;
            }
            else {
                Settings<MrktSim.ClientSettings>.Value.ModelFrameMaximized = false;
            }
        }

        private void ModelAndMarketPlanView_LocationChanged( object sender, EventArgs e ) {
            UpdateBoundsSettings();
        }

        private void ModelAndMarketPlanView_SizeChanged( object sender, EventArgs e ) {
            UpdateBoundsSettings();
        }

        private void ModelAndMarketPlanView_Load( object sender, EventArgs e ) {
            if( Settings<MrktSim.ClientSettings>.Value.ModelFrameMaximized == true ) {
                this.WindowState = FormWindowState.Maximized;
            }
        }
    }
}