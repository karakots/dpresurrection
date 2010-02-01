using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SimControl.Dialogues
{
    public partial class LicenseDialog : Form
    {
        public LicenseDialog()
        {
            InitializeComponent();
        }

        public string UserName
        {
            get
            {
                return UserNameBox.Text;
            }
        }

        public string LicenseKey
        {
            get
            {
                return LicenseKeyBox.Text;
            }
        }
    }
}