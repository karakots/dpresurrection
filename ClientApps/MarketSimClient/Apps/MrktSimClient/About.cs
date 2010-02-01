using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MrktSimClient
{
    public partial class About : Form
    {
        public About( string version, string AppName ) {
            InitializeComponent();

            this.versionLabel.Text = version;
            appNameLabel.Text = AppName;
        }
    }
}