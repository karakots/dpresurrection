using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calibration.dialogs
{
    public partial class GetString : Form
    {
        public GetString()
        {
            InitializeComponent();
        }

        public string Title
        {
            set
            {
                Text = value;
            }
        }

        public string Info
        {
            set
            {
                InfoLabel.Text = value;
            }
        }

        public string Type
        {
            set
            {
                ValueTypeLabel.Text = value;
            }
        }

        public string Value
        {
            get
            {
                return StringValue.Text;
            }
        }
    }
}
