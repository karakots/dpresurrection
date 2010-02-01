using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GeoLibrary;

namespace Calibration.dialogs
{
    public partial class SelectRegion : Form
    {
        public SelectRegion(List<string> regions)
        {
            InitializeComponent();

            foreach (string region in regions)
            {
                RegionCombo.Items.Add(region);
            }

            RegionCombo.SelectedIndex = 0;
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

        public string RegionName
        {
            get
            {
                return (string)RegionCombo.SelectedItem;
            }
        }
    }
}
