using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace Utilities
{
	/// <summary>
	/// Summary description for LinkButton.
	/// </summary>
	public class LinkButton : System.Windows.Forms.Label
	{	
		public LinkButton()
		{
		}

		public void Deselect()
		{
			this.Image = unSelImage;
			this.BackColor = unSelBackColor;
			this.ForeColor = unSelForeColor;
		}

		new public void Select()
		{
			this.Image = selImage;
			this.BackColor = selBackColor;
			this.ForeColor = selForeColor;
		}

		/*public void Selectable(bool state)
		{
			if(state)
			{
				this.Enabled = true;
				this.BackColor = Color.White;
				this.Font = new Font(this.Font, this.Font.Style | FontStyle.Bold);
				this.Cursor = Cursors.Hand;
			}
			else
			{
				this.Enabled = false;
				this.BackColor = Color.White;
				this.Font = new Font(this.Font, this.Font.Style | FontStyle.Regular);
				this.Cursor = Cursors.Arrow;
			}
		}*/

		public System.Windows.Forms.Label SelectState
		{
			set
			{
				selImage = value.Image;
				selBackColor = value.BackColor;
				selForeColor = value.ForeColor;
			}
		}

		public System.Windows.Forms.Label UnSelectState
		{
			set
			{
				unSelImage = value.Image;
				unSelBackColor = value.BackColor;
				unSelForeColor = value.ForeColor;
			}

		}

		System.Drawing.Image selImage;
		System.Drawing.Image unSelImage;
		System.Drawing.Color selBackColor;
		System.Drawing.Color unSelBackColor;
		System.Drawing.Color selForeColor;
		System.Drawing.Color unSelForeColor;
	}
}
