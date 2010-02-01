using System;
using System.Windows.Forms;
using System.Collections;

using MrktSimDb;
using ModelView;

namespace ModelView.MsTree
{
	/// <summary>
	/// This hierarchy is used in the Project Tree
	/// Project nodes use the ProjectDb schema
	/// </summary>
	/// 
	
   
	public abstract class MsProjectNode : TreeNode
	{
        public enum Type
        {
            Project,
            RootModel,
            Model
        }

		// the ModelDb 
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

        public MsProjectNode.Type NodeType;

        public abstract void Initialize();
	}

	// top level node that display project data
	public class MsTopProjectNode : MsProjectNode
	{
		public MsTopProjectNode()
		{
            NodeType = MsProjectNode.Type.Project;
			this.Text = ProjectDb.NoProjectName;
			this.Tag = null; 
		}

		public override void Initialize()
		{
			// set name correctly
			this.Text = Db.ProjectName;

			this.Nodes.Clear();

			MsProjectModelNode node = new MsProjectModelNode();
			node.Db = Db;

			this.Nodes.Add(node);

			node.Initialize();
		}

		public  void Refresh()
		{
			foreach(MsProjectModelNode node in Nodes)
			{
				node.Refresh();
			}
		}

		public bool HasChanges()
		{
			foreach(MsProjectModelNode node in Nodes)
			{
				if (node.HasChanges())
					return true;
			}

			return false;
		}

		public void Close()
		{
			foreach(MsProjectModelNode node in Nodes)
			{
				node.Close();
			}
		}

		public void Save()
		{
			foreach(MsProjectModelNode node in Nodes)
			{
				node.Save();
			}
		}

		public bool ReadOnly()
		{
			return true;
		}

		// create a new item 
		public bool CreateNewItem()
		{
			foreach(MsProjectModelNode node in Nodes)
			{
				if (node.CreateNewItem())
					return true;
			}

			return false;
		}

		// cannot be deleted
		public void DeleteItem()
		{}
	}


	// displays model tree in project pane
	public class MsProjectModelNode : MsProjectNode
	{
		// where we display the model
		private ArrayList forms;
		public int model_id;

		public void AddModelForm(ModelViewInterface form)
		{
			forms.Add(form);

			form.Closed += new EventHandler(this.FormClosed);
		}

		public bool HasForms
		{
			get
			{
				if (forms.Count == 0)
					return false;

				return true;
			}
		}

		public void FormClosed(Object sender, System.EventArgs ignore)
		{
			forms.Remove(sender);
		}

		public MsProjectModelNode()
		{
            NodeType = MsProjectNode.Type.RootModel;
			this.Text = "Models";
			this.Tag = null;  // decalres this as top level node
			forms = new ArrayList();

			model_id = -1;
		}

		public bool HasChanges()
		{
			if (this.Tag == null)
			{
				foreach(MsProjectModelNode node in Nodes)
				{
					if (node.HasChanges())
						return true;
				}
			}
			else 
			{
				foreach( ModelViewInterface form in forms)
				{	
					if (form.HasChanges())
						return true;
				}
			}

			return false;
		}

		public void Close()
		{
			if (this.Tag == null)
			{
				foreach(MsProjectModelNode node in Nodes)
				{
					node.Close();
				}
			}
			else 
			{
				while(forms.Count > 0)
				{
					ModelViewInterface form = (ModelViewInterface) forms[0];

					form.Close();
				}
			}
		}

		public void Save()
		{
			if (this.Tag == null)
			{
				foreach(MsProjectModelNode node in Nodes)
				{
					node.Save();
				}
			}
			else 
			{
				foreach( ModelViewInterface form in forms)
				{	
					form.SaveModel();
				}	
			}
		}

		public bool ReadOnly()
		{
			return true;
		}

		// creates tree from database
		public override void Initialize()
		{
			// create Models
			if( this.Tag == null )
			{
				this.Nodes.Clear();
				// this is the top level model node node

				foreach(MrktSimDBSchema.Model_infoRow model in Db.Data.Model_info.Rows)
				{
					if(!ProjectDb.Nimo || model.app_code == ProjectDb.NimoAppCode)
					{
						MsProjectModelNode aModelNode = new MsProjectModelNode();
				
						aModelNode.Text = model.model_name;
						aModelNode.Db = Db;
						aModelNode.Model = model;
                        aModelNode.NodeType = MsProjectNode.Type.Model;

						this.Nodes.Add(aModelNode);

						aModelNode.Initialize();
					}
				}
			}
			else
			{
				// Nothing to do at present
			}
		}


		public string ModelName
		{
			set
			{
				if (Model == null)
					return;

				Model.model_name = value;
				this.Text = value;
			}

			get
			{
				if (Model == null)
					return null;

				return Model.model_name;
			}
		}

		private bool findModel(int id)
		{
			foreach(MsProjectModelNode node in Nodes)
			{
				if (node.Model.model_id == id)
					return true;
			}

			return false;
		}

		public  void Refresh()
		{
			// create Models
			if( this.Tag == null )
			{
				// this is the top level model node node

				// find obsolete nodes
				bool deleting = true;
				while(deleting)
				{
					deleting = false;

					foreach(MsProjectModelNode node in Nodes)
					{
						if (node.Model.RowState == System.Data.DataRowState.Detached)
						{
							MrktSimDBSchema.Model_infoRow model = Db.Data.Model_info.FindBymodel_id(node.model_id);

							if (model != null)
								node.Model = model;
							else
							{
								Nodes.Remove(node);
								deleting = true;
								break;
							}
						}
					}
				}

				// refresh exising nodes
				foreach(MsProjectModelNode node in Nodes)
				{
					node.Text = node.Model.model_name;
				}

				// add new nodes
				foreach(MrktSimDBSchema.Model_infoRow row in Db.Data.Model_info.Rows)
				{
					if(!ProjectDb.Nimo || row.app_code == ProjectDb.NimoAppCode)
					{
						// find row in current nodes
						if (!findModel(row.model_id))
						{
						

							MsProjectModelNode aModelNode = new MsProjectModelNode();
				
							aModelNode.Text = row.model_name;
							aModelNode.Db = Db;
							aModelNode.Model = row;
                            aModelNode.NodeType = MsProjectNode.Type.Model;
	
							this.Nodes.Add(aModelNode);

							aModelNode.Initialize();
						}
					}
				}
			}
			else
			{
				// Nothing to do at present
			}
		}

		// create a new item
		public bool CreateNewItem()
		{
			return false;
//			// only top level can create item
//			if (Model != null)
//				return false;
//
//			// create a new model
//			MrktSimDBSchema.Model_infoRow model = Db.CreateModel();
//
//			// create node to hold model
//			MsProjectModelNode aModelNode = new MsProjectModelNode();
//				
//			aModelNode.Text = model.model_name;
//			aModelNode.Tag = model;
//			aModelNode.Model = model;
//			aModelNode.Db = Db;
//			aModelNode.NodeType = MsNodeType.projectModelNodeType;
//
//			this.Nodes.Add(aModelNode);
//
//			aModelNode.Initialize();
//
//			return true;
		}

		// delete item
		public void DeleteItem()
		{
			// cannot delete top level node
			if( Model == null)
				return;

			Model.Delete();

			if( this.Parent != null)
				this.Parent.Nodes.Remove(this);
		}


		// return row in database corresponding to this node
		public MrktSimDBSchema.Model_infoRow Model
		{
			get
			{
				if( this.Tag == null)
					return null;

				return (MrktSimDBSchema.Model_infoRow) this.Tag;
			}

			set
			{
				this.Tag = value;
				model_id = value.model_id;
			}
		}
	}
}
