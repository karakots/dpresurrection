using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using ModelView.Dialogs;
using MarketSimUtilities.MsTree;

namespace ModelView
{
	/// <summary>
	/// Summary description for BaseModel.
	/// </summary>
	public class BaseModel : ModelViewForm
	{
		public override MrktSimDb.ModelDb Db
		{
			get
			{
				return base.Db;
			}
			set
			{
				base.Db = value;

				// scenarions
				MsTopModelNode modelNode = new MsTopModelNode();
				
				Context = modelNode; 
			}
		}

		public BaseModel()
		{
		}

		public override void menuOptions_Click(object sender, System.EventArgs e)
		{
			

			
			ModelOptionsDialog dlg = new ModelOptionsDialog(this.Db);

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.Cancel)
				return;

			this.update_nodes();
		}
	}
}
