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
	/// Summary description for MarketPlanView.
	/// </summary>
	public class MarketPlanView : ModelViewForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		// private System.ComponentModel.Container components = null;

		public MarketPlanView() : base()
		{
			this.Size = new Size(800, 500);
			this.MinimumSize = this.Size;

			// only the baseModel view can save as...
			this.SaveAs = false;

			Context = new MarketingDataNode();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            //if( disposing )
            //{
            //    if(components != null)
            //    {
            //        components.Dispose();
            //    }
            //}
			base.Dispose( disposing );
		}

		public override void menuOptions_Click(object sender, System.EventArgs e)
		{	
			MarketOptionsDialog dlg = new MarketOptionsDialog(this.Db);

			DialogResult rslt = dlg.ShowDialog();

			if (rslt == DialogResult.Cancel)
				return;

			update_nodes();
		}
	}
}
