using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MediaLibrary;

namespace Calibration.dialogs
{
    public partial class SelectAdOption : Form
    {
        public SelectAdOption(Dictionary<int, AdOption> options)
        {
            InitializeComponent();

            foreach (int option_id in options.Keys)
            {
                OptionCombo.Items.Add(options[option_id]);
            }

            OptionCombo.SelectedIndex = 0;
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

        public AdOption Option
        {
            get
            {
                return (AdOption)OptionCombo.SelectedItem;
            }
        }
    }
}
