using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NitroReader.Dialogs
{
    public partial class MarketSimName : Form
    {
        private MarketPlan marketPlan;
        private Settings settings;

        private ArrayList allNames;

        public string NameForMarketSim {
            get {
                return nameTextBox.Text.Trim();
            }
        }

        public static bool NameIsAvailable( string newMrktSimName, MarketPlan marketPlan, Settings settings ) {

            ArrayList allNames = new ArrayList();
            foreach( MarketPlan.VariantInfo vinfo in marketPlan.Variants ) {
                string msName = settings.GetMarketSimName( vinfo.Name );
                allNames.Add( msName );
            }
            foreach( Settings.GroupInfo ginfo in settings.Groups ) {
                allNames.Add( ginfo.Name );
            }
            foreach( Settings.GroupInfo ginfo in settings.Groups ) {
                string msName = settings.GetMarketSimName( ginfo.Name );
                if( allNames.Contains( msName ) == false ) {
                    allNames.Add( msName );
                }
            }
            return (allNames.Contains( newMrktSimName ) == false);
        }

        public MarketSimName( string nitroName, string marketSimName, MarketPlan marketPlan, Settings settings ) {
            InitializeComponent();

            this.nitroNameLabel.Text = nitroName;
            this.nameTextBox.Text = marketSimName;

            this.settings = settings;
            this.marketPlan = marketPlan;

            allNames = new ArrayList();
            foreach( MarketPlan.VariantInfo vinfo in marketPlan.Variants ) {
                string msName = settings.GetMarketSimName( vinfo.Name );
                allNames.Add( msName );
            }
            foreach( Settings.GroupInfo ginfo in settings.Groups ) {
                allNames.Add( ginfo.Name );
            }
            foreach( Settings.GroupInfo ginfo in settings.Groups ) {
                string msName = settings.GetMarketSimName( ginfo.Name );
                if( allNames.Contains( msName ) == false ) {
                    allNames.Add( msName );
                }
            }
        }

        private void okButton_Click( object sender, EventArgs e ) {
            string newName = nameTextBox.Text.Trim();
            okButton.DialogResult = DialogResult.OK;
            if( (newName != this.nitroNameLabel.Text) && allNames.Contains( newName ) ) {
                // error - trying to enter a new name, but it has been used already
                okButton.DialogResult = DialogResult.None;
                this.DialogResult = DialogResult.None;
                string msg = String.Format( "The name \"{0}\" has been used already.  Choose a different name.", newName );
                ConfirmForm cform = new ConfirmForm( msg, "Name Used Already" );
                cform.HideCancel();
                cform.ShowDialog( this );
            }
        }

        private void nameTextBox_TextChanged( object sender, EventArgs e ) {
            okButton.Enabled = (nameTextBox.Text.Trim().Length > 0);
        }
    }
}