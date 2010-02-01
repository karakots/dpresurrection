using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NitroReader.Dialogs
{
    /// <summary>
    /// Form class for getting the name for a new group.
    /// </summary>
    public partial class NewGroup : Form
    {
        private MarketPlan marketPlan;
        private Settings settings;

        private ArrayList allNames;

        /// <summary>
        /// Returns the group name.
        /// </summary>
        public string GroupName {
            get {
                return nameTextBox.Text.Trim();
            }
        }

        /// <summary>
        /// Returns the correlation factor for the group.
        /// </summary>
        public double Correlation {
            get {
                return (double)this.trackBar1.Value / (double)this.trackBar1.Maximum;
            }
        }

        /// <summary>
        /// Creates a new NewGroup object.
        /// </summary>
        /// <param name="marketPlan"></param>
        /// <param name="settings"></param>
        public NewGroup( MarketPlan marketPlan, Settings settings ) {
            InitializeComponent();

            this.settings = settings;
            this.marketPlan = marketPlan;

            // accumulate the list of all names already used in the output
            allNames = new ArrayList();
            foreach( MarketPlan.VariantInfo vinfo in marketPlan.Variants ) {
                allNames.Add( vinfo.Name );
                string msName = settings.GetMarketSimName( vinfo.Name );
                if( allNames.Contains( msName ) == false ) {
                    allNames.Add( msName );
                }
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

            // generate the default name for the new group
            string groupName = "Group ?";
            for( int i = 1; i < 999; i++ ) {
                groupName = String.Format( "Group{0}", i );
                if( allNames.Contains( groupName ) == false ) {
                    break;
                }
            }
            this.nameTextBox.Text = groupName;
        }

        /// <summary>
        /// Closes the form if the name entered is a legal (new) name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click( object sender, EventArgs e ) {
            string newName = nameTextBox.Text.Trim();
            okButton.DialogResult = DialogResult.OK;
            if( allNames.Contains( newName ) ) {
                // error - trying to re-use a name
                okButton.DialogResult = DialogResult.None;
                this.DialogResult = DialogResult.None;
                string msg = String.Format( "The name \"{0}\" has been used already.  Choose a different name.", newName );
                ConfirmForm cform = new ConfirmForm( msg, "Name Used Already" );
                cform.HideCancel();
                cform.ShowDialog( this );
            }
        }

        /// <summary>
        /// Disables the ok button if the name is blank
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nameTextBox_TextChanged( object sender, EventArgs e ) {
            okButton.Enabled = (nameTextBox.Text.Trim().Length > 0);
        }
    }
}