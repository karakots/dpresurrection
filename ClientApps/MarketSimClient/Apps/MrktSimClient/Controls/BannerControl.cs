using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MrktSimDb;

namespace MrktSimClient.Controls
{
    public partial class BannerControl : UserControl
    {
        public BannerControl()
        {
            InitializeComponent();

            if( Database.Application != Database.AppType.MarketSim )
            {
                AppLabel.Text = Database.AppName + " Edition";
            }
            else
            {
                AppLabel.Text = "";
            }
        }
    }
}
