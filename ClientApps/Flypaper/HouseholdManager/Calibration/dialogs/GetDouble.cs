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
    public partial class GetDouble : Form
    {
        public GetDouble()
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

        public double Value
        {
            get
            {
                return (double)DoubleValue.Value;
            }
        }
    }
}
