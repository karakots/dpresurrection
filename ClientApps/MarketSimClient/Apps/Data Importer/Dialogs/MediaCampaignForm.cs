using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataImporter.Dialogs
{

    public partial class MediaCampaignForm : Form
    {
        public string Campaign {
            get {
                return this.campaignTextBox.Text.Trim();
            }
        }
    
        public MediaCampaignForm( string existingCampaign ) {
            InitializeComponent();

            this.campaignTextBox.Text = existingCampaign;
        }
    }
}