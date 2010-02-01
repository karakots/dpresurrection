using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdPlanitSimController
{
    public partial class PopSizeDialogue : Form
    {
        public PopSizeDialogue()
        {
            InitializeComponent();
        }

        public int NumAgents
        {
            get
            {
                return 10000 * (int) popSize.Value;
            }
        }

        private void OKBut_Click( object sender, EventArgs e )
        {
            DialogResult = DialogResult.OK;
        }

        private void CancelBut_Click( object sender, EventArgs e )
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
