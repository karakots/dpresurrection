using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MrktSimDb;
using Common;
using Common.Utilities;

using System.Data;

using ModelView.Dialogs;

using MarketSimUtilities;
using MarketSimUtilities.MsTree;

namespace ModelView
{
	/// <summary>
	/// Summary description for ModelViewForm.
	/// </summary>
	public class ModelViewForm : System.Windows.Forms.Form, ModelViewInterface
	{
		public event EventHandler DatabaseChanged;
		
		public void SaveModel()
		{
			Cursor temp = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			this.currentControl.Flush();

            foreach( MsControlNode topModelNode in topModelNodes ) {
                topModelNode.Suspend = true;
            }

			theDb.Update();

            foreach( MsControlNode topModelNode in topModelNodes ) {
                topModelNode.Suspend = false;
 			    topModelNode.DbUpdate();
           }
			
			if (DatabaseChanged != null)
			{
				DatabaseChanged(this, new System.EventArgs());
			}

			this.Cursor = temp;
		}

		
		virtual public ModelDb Db
		{
			set
			{
				theDb = value;

				// VdM
				//theOptInfoDB = new OptInfoDB();
				//theOptInfoDB.Connection = theDb.Connection;

				//theOptInfoDB.Refresh();  

				if (topModelNodes != null)
				{
                    foreach( MsControlNode topModelNode in topModelNodes ) {
                        topModelNode.Db = theDb;
                        topModelNode.Initialize();
                    }
				}

				if (theDb.ReadOnly)
				{
					saveModelMenu.Visible = false;
				}
			}

			get
			{
				return theDb;
			}
		}

		public bool HasChanges()
		{
			this.currentControl.Flush();

			bool hasFocus = currentControl.Focused;

			return theDb.HasChanges();
		}

		public bool SaveAs
		{
			set
			{
				this.saveAsMenu.Visible = value;
			}
		}

		protected ModelDb theDb = null;
        private ArrayList topModelNodes = null;
 //       private MsControlNode topModelNode = null;      

		protected MsControlNode Context
		{
			set
			{
                topModelNodes = new ArrayList();
                MsControlNode topModelNode = value;
                topModelNodes.Add( topModelNode );

				if (theDb != null)
				{
					topModelNode.Db = theDb;
					topModelNode.Initialize();
				}

				navigatePane.Nodes.Clear();
				navigatePane.Nodes.Add(topModelNode);
			}
		}

        /// <summary>
        /// Adds a list of root nodes to the tree panel.
        /// </summary>
        /// <param name="rootNodes">A list of MsControlNode objects</param>
        protected void SetContextNodes( ArrayList rootNodes ) {
            topModelNodes = rootNodes;
            foreach( MsControlNode topModelNode in topModelNodes ) {
                if( theDb != null ) {
                    topModelNode.Db = theDb;
                    topModelNode.Initialize();
                }
            }
            navigatePane.Nodes.Clear();
            foreach( MsControlNode topModelNode in topModelNodes ) {
                navigatePane.Nodes.Add( topModelNode );
            }
        }

        protected MrktSimControl currentControl;
 //       private MarketPlanControl2 marketPlanControl; 

		// VdM 
		// private OptInfoDB theOptInfoDB;
		private System.Windows.Forms.MainMenu mainModelMenu;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem saveModelMenu;
		private System.Windows.Forms.MenuItem saveAsMenu;
		private System.Windows.Forms.MenuItem refreshModelMenu;
		private System.Windows.Forms.MenuItem closeModelMenu;
		protected System.Windows.Forms.TreeView navigatePane;
		private System.Windows.Forms.MenuItem dataMenu;
		private System.Windows.Forms.MenuItem CreateMenu;
		private System.Windows.Forms.MenuItem pasteMenu;
		private System.Windows.Forms.MenuItem excelImpotMenu;
		private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuOptions;
        private IContainer components;

		public ModelViewForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


			navigatePane.ShowLines = false;
			navigatePane.HotTracking = true;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ModelViewForm ) );
            this.mainModelMenu = new System.Windows.Forms.MainMenu( this.components );
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.saveModelMenu = new System.Windows.Forms.MenuItem();
            this.saveAsMenu = new System.Windows.Forms.MenuItem();
            this.refreshModelMenu = new System.Windows.Forms.MenuItem();
            this.closeModelMenu = new System.Windows.Forms.MenuItem();
            this.dataMenu = new System.Windows.Forms.MenuItem();
            this.CreateMenu = new System.Windows.Forms.MenuItem();
            this.pasteMenu = new System.Windows.Forms.MenuItem();
            this.excelImpotMenu = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuOptions = new System.Windows.Forms.MenuItem();
            this.navigatePane = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // mainModelMenu
            // 
            this.mainModelMenu.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.menuItem3,
            this.dataMenu} );
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 0;
            this.menuItem3.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.saveModelMenu,
            this.saveAsMenu,
            this.refreshModelMenu,
            this.closeModelMenu} );
            this.menuItem3.Text = "File";
            // 
            // saveModelMenu
            // 
            this.saveModelMenu.Index = 0;
            this.saveModelMenu.Text = "Save";
            this.saveModelMenu.Click += new System.EventHandler( this.saveModelMenu_Click );
            // 
            // saveAsMenu
            // 
            this.saveAsMenu.Index = 1;
            this.saveAsMenu.Text = "Save as...";
            this.saveAsMenu.Click += new System.EventHandler( this.saveAsMenu_Click );
            // 
            // refreshModelMenu
            // 
            this.refreshModelMenu.Index = 2;
            this.refreshModelMenu.Text = "Refresh";
            this.refreshModelMenu.Click += new System.EventHandler( this.refreshModelMenu_Click );
            // 
            // closeModelMenu
            // 
            this.closeModelMenu.Index = 3;
            this.closeModelMenu.Text = "Close";
            this.closeModelMenu.Click += new System.EventHandler( this.closeModelMenu_Click );
            // 
            // dataMenu
            // 
            this.dataMenu.Index = 1;
            this.dataMenu.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.CreateMenu,
            this.pasteMenu,
            this.excelImpotMenu,
            this.menuItem1,
            this.menuOptions} );
            this.dataMenu.Text = "Tools";
            this.dataMenu.Select += new System.EventHandler( this.dataMenu_Select );
            // 
            // CreateMenu
            // 
            this.CreateMenu.Index = 0;
            this.CreateMenu.Text = "Create Data...";
            this.CreateMenu.Click += new System.EventHandler( this.CreateMenu_Click );
            // 
            // pasteMenu
            // 
            this.pasteMenu.Index = 1;
            this.pasteMenu.Text = "Paste Data";
            this.pasteMenu.Click += new System.EventHandler( this.pasteMenu_Click );
            // 
            // excelImpotMenu
            // 
            this.excelImpotMenu.Index = 2;
            this.excelImpotMenu.Text = "Import From Excel";
            this.excelImpotMenu.Click += new System.EventHandler( this.excelImpotMenu_Click );
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "-";
            // 
            // menuOptions
            // 
            this.menuOptions.Index = 4;
            this.menuOptions.Text = "Options";
            this.menuOptions.Click += new System.EventHandler( this.menuOptions_Click );
            // 
            // navigatePane
            // 
            this.navigatePane.Dock = System.Windows.Forms.DockStyle.Left;
            this.navigatePane.HideSelection = false;
            this.navigatePane.HotTracking = true;
            this.navigatePane.Location = new System.Drawing.Point( 0, 0 );
            this.navigatePane.Name = "navigatePane";
            this.navigatePane.Scrollable = false;
            this.navigatePane.ShowLines = false;
            this.navigatePane.Size = new System.Drawing.Size( 168, 445 );
            this.navigatePane.TabIndex = 0;
            this.navigatePane.MouseDown += new System.Windows.Forms.MouseEventHandler( this.navigatePane_MouseDown );
            // 
            // ModelViewForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size( 792, 445 );
            this.Controls.Add( this.navigatePane );
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.Menu = this.mainModelMenu;
            this.MinimumSize = new System.Drawing.Size( 800, 400 );
            this.Name = "ModelViewForm";
            this.Text = " new model";
            this.Closing += new System.ComponentModel.CancelEventHandler( this.ModelViewForm_Closing );
            this.Load += new System.EventHandler( this.ModelViewForm_Load );
            this.ResumeLayout( false );

		}
		#endregion

        /// <summary>
        /// Loads the current model from the database.
        /// </summary>
		private void readFromDatabase()
		{
            foreach( MsControlNode topModelNode in topModelNodes ) {
                topModelNode.Suspend = true;
            }

			theDb.Refresh();

            foreach( MsControlNode topModelNode in topModelNodes ) {
                topModelNode.Suspend = false;
                topModelNode.Reset();
            }
		}

        /// <summary>
        /// Creates the appropriate control for the right-side panel for a specified node in the left-panel tree.
        /// </summary>
        /// <param name="node"></param>
		private void createControl(MsControlNode node)
		{
            if( node == null ) {
                return;
            }

			if (node.Control != null)
			{
				node.Control.Dispose();
				node.Control = null;
			}

            bool layoutNeeded = SetNodeControl( node ); 

            if( layoutNeeded ) { 
                initializeControlLayout( node.Control );
            }
		}

        /// <summary>
        /// Sets the correct node.Control value for the given left-panel tree node type.
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns>true if the control needs a layout call</returns>
        public virtual bool SetNodeControl( MsControlNode node ) {
			// set control for this node
            switch( node.NodeType ) {
                case MsNodeType.ModelNodeType:
                    node.Control = new ModelSummary( theDb );
                    break;

                case MsNodeType.ProductNodeType:
                    node.Control = new ProductGridControl( theDb );
                    break;

                case MsNodeType.SegmentNodeType:
                    node.Control = new SegmentGridControl( theDb );
                    break;

                case MsNodeType.ChannelNodeType:
                    node.Control = new ChannelControl( theDb );
                    break;

                // initial conditions
                case MsNodeType.InitialConditionNodeType:
                    node.Control = new InitialConditionControl( theDb );
                    break;

                case MsNodeType.TaskNodeType:
                    node.Control = new TaskControl( theDb );
                    break;
                case MsNodeType.AttributeNodeType:
                    node.Control = new AttributeControl( theDb );
                    break;

                case MsNodeType.ProductDependType:
                    node.Control = new ProductMatrixControl( theDb );
                    break;

                case MsNodeType.ProductTypeNodeType:
                    node.Control = new ProductTypeGridControl( theDb, (MrktSimDBSchema.product_typeRow)node.Tag );
                    break;

                case MsNodeType.SocialNetworkType:
                    node.Control = new SocialNetwork();
                    node.Control.Db = theDb;
                    break;

                case MsNodeType.NetworkModelType:
                    node.Control = new NetworkModels();
                    node.Control.Db = theDb;
                    break;

                case MsNodeType.ModelParameterType:
                    node.Control = new ModelParameter();
                    node.Control.Db = theDb;
                    break;

                case MsNodeType.ExternalDataType:
                    node.Control = new ExternalDataGrid();
                    node.Control.Db = theDb;
                    break;
            }
            return true;
        }

        /// <summary>
        /// Initializes the layout of a newly-created control.
        /// </summary>
        /// <param name="control"></param>
        private void initializeControlLayout( MrktSimControl control ) {
            // set the database
            control.SuspendLayout();

            control.Dock = System.Windows.Forms.DockStyle.Fill;
            control.Location = new System.Drawing.Point( 129, 0 );
            control.Size = new System.Drawing.Size( 215, 270 );
            control.TabIndex = 2;

            this.Controls.Add( control );

            control.ResumeLayout( false );
        }

        /// <summary>
        /// Makes the specified control active in the right-side panel.
        /// </summary>
        /// <param name="control"></param>
		private void activateControl(MrktSimControl control)
		{
			control.Refresh();
			control.BringToFront();
			currentControl = control;
		}

		/// <summary>
		/// action to take after a node is actually selected
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
	
        /// <summary>
        /// Save the current model to the ModelDb.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void saveModelMenu_Click(object sender, System.EventArgs e)
		{
			SaveModel();
		}

        /// <summary>
        /// Refresh the model from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void refreshModelMenu_Click(object sender, System.EventArgs e)
		{
			readFromDatabase();
		}

		private bool checkDataQueryUser()
		{
			bool rval = true;

			if (HasChanges())
			{
				string msg = "Do you wish to save " + theDb.Model.model_name + "?";
				string caption = "Closing " + theDb.Model.model_name;

				DialogResult res = MessageBox.Show(this, msg, "Saving Model",MessageBoxButtons.YesNoCancel);

				if( res == DialogResult.Cancel)
				{
					rval =  false;
				}
				else if( res == DialogResult.Yes)
				{
					// save model
					SaveModel();
				}
			}

			return rval;
		}


		private void closeModelMenu_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ModelViewForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!checkDataQueryUser())
			{
				e.Cancel = true;
			}
			else
			{
				if (theDb != null)
					theDb.Close();
			}
		}

		// writes out xml
		private void writeXML_Click(object sender, System.EventArgs e)
		{
			theDb.Data.WriteXml("saveit.dpi");
		}

		// reads in xml
		// TODO before this is viable - we must add proper relations to table
		private void readXML_Click(object sender, System.EventArgs e)
		{
			theDb.Data.Clear();
			theDb.Data.ReadXml("saveit.dpi");
		}

		// save as
		private void saveAsMenu_Click(object sender, System.EventArgs e)
		{
			SaveModelAs dlg = new SaveModelAs();

			dlg.ModelName = theDb.Model.model_name;

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.Cancel)
				return;

			ModelDb dbCopy = theDb.Copy();

			dbCopy.Model.model_name = dlg.ModelName;
			dbCopy.Model.descr = "Copy of " + theDb.Model.model_name;

			dbCopy.Update();

			dlg.Dispose();

			if (DatabaseChanged != null)
				DatabaseChanged(this, new System.EventArgs());
		}


		private void ModelViewForm_Load(object sender, System.EventArgs e)
		{
            if( topModelNodes == null ) {
                return;
            }
			this.SuspendLayout();

            foreach( MsControlNode topModelNode in topModelNodes ) {
                createControl( topModelNode );
            }
            activateControl( ((MsControlNode)topModelNodes[ 0 ]).Control );         // activate the first root node

			this.ResumeLayout(false);

			// create event handlers after everything is loaded up  ??? does this do anything ???
            foreach( MsControlNode topModelNode in topModelNodes ) {
                ModelSummary form = topModelNode.Control as ModelSummary;
            }
		}

		public override void Refresh()
		{
			base.Refresh();

			update_nodes();
		}

		public void update_nodes()
		{
			this.SuspendLayout();

            foreach( MsControlNode topModelNode in topModelNodes ) {
                topModelNode.Reset();
            }

			this.ResumeLayout();
		}

        protected virtual void navigatePane_MouseDown( object sender, System.Windows.Forms.MouseEventArgs e ) {
            MsControlNode node = (MsControlNode)navigatePane.GetNodeAt( e.X, e.Y );

            if( node == null ){
                return;
            }

            navigatePane.SelectedNode = node;

            if( node.Control == null ) {
                //instantiate node control
                createControl( node );
            }

            if( currentControl != node.Control ) {
                this.Cursor = Cursors.WaitCursor;
                currentControl.Flush();
                activateControl( node.Control );
                this.Cursor = Cursors.Default;
            }
            //else {
            //    MarketPlanControl2 mpControl = currentControl as MarketPlanControl2;
            //    if( mpControl != null ) {
            //        // switch control mode as appropriate for this node
            //        switch( node.NodeType ) {
            //            case MsNodeType.MarketDataNodeType:
            //                mpControl.SetViewFor( ModelDb.PlanType.MarketPlan );
            //                break;

            //            case MsNodeType.PriceNodeType:
            //                mpControl.SetViewFor( ModelDb.PlanType.Price );
            //                break;

            //            case MsNodeType.DistributionNodeType:
            //                mpControl.SetViewFor( ModelDb.PlanType.Distribution );
            //                break;

            //            case MsNodeType.DisplayNodeType:
            //                mpControl.SetViewFor( ModelDb.PlanType.Display );
            //                break;

            //            case MsNodeType.MediaNodeType:
            //                mpControl.SetViewFor( ModelDb.PlanType.Media );
            //                break;

            //            case MsNodeType.CouponNodeType:
            //                mpControl.SetViewFor( ModelDb.PlanType.Coupons );
            //                break;

            //            case MsNodeType.MarketUtilityNodeType:
            //                mpControl.SetViewFor( ModelDb.PlanType.Market_Utility );
            //                break;

            //            case MsNodeType.EventNodeType:
            //                mpControl.SetViewFor( ModelDb.PlanType.ProdEvent );
            //                break;
            //        }
            //    }
            //}
        }

		
		private void dataMenu_Select(object sender, System.EventArgs e)
		{
			if (currentControl.AllowPaste())
			{
				// check if we should allow pasting data

				IDataObject data = Clipboard.GetDataObject();
				if (data.GetDataPresent(DataFormats.Text))
				{
					this.pasteMenu.Enabled = true;
				}
				else
				{
					this.pasteMenu.Enabled = false;
				}
			}
			else
			{
				this.pasteMenu.Enabled = false;
			}

			if (currentControl.AllowCreate())
			{
				this.CreateMenu.Enabled = true;
			}
			else
			{
				this.CreateMenu.Enabled = false;
			}

			
			if (currentControl.AllowExcelImport())
			{
				this.excelImpotMenu.Enabled = true;
			}
			else
			{
				this.excelImpotMenu.Enabled = false;
			}

		}

		private void CreateMenu_Click(object sender, System.EventArgs e)
		{
			currentControl.Create();
		}

		private void pasteMenu_Click(object sender, System.EventArgs e)
		{
			currentControl.Paste();
		}

		private void excelImpotMenu_Click(object sender, System.EventArgs e)
		{
			currentControl.ExcelImport();
		}

		public virtual void menuOptions_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
