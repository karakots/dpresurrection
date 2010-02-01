using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;
using Utilities;

namespace Common.Dialogs
{
    public partial class MrktSimConfig : Form
    {
        private string helpTag = "MrktSimConfig";

        public string CustomConfigCode {
            get {
                return configurationCodeTextBox.Text.Trim();
            }
        }
        
        /// <summary>
        /// Creates a new dialog for setting overall MarketSim configuration items.
        /// </summary>
        public MrktSimConfig( string customConfigCode ) {
            InitializeComponent();

            this.configurationCodeTextBox.Text = customConfigCode;
        }

        private void helpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, this.helpTag );
        }
    }
}