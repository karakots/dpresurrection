using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MediaManager
{
    public partial class MediaSelector : Form
    {
        public MediaSelector(List<string> location_names)
        {
            InitializeComponent();

            foreach (string name in location_names)
            {
                MediaListCombo.Items.Add(name);
            }

            MediaListCombo.SelectedIndex = 0;
        }

        public string MissingLocation
        {
            set
            {
                LocNameLabel.Text = value;
            }
        }

        public string NewLocation
        {
            get
            {
                return MediaListCombo.SelectedItem.ToString();
            }
        }
    }
}
