using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NitroReader.Dialogs
{
    public partial class Group2 : Form
    {
        private ListView listView;
        private MarketPlan marketPlan;
        private Settings settings;

        private string[] grpNames;
        private int[] grpIndexes;
        private double[] grpCorrs;

        public Group2( ListView listView, MarketPlan marketPlan, Settings settings ) {
            InitializeComponent();

            this.listView = listView;
            this.settings = settings;
            this.marketPlan = marketPlan;

            int nsel = listView.SelectedIndices.Count;
            string info = String.Format( "There are {0} items selected to add", nsel );
            this.infoLabel.Text = info;

            UpdateGroupsList();
        }

        private void UpdateGroupsList() {
            grpNames = new string[ settings.Groups.Count ];
            string[] grpNames2 = new string[ settings.Groups.Count ];
            grpIndexes = new int[ settings.Groups.Count ];
            grpCorrs = new double[ settings.Groups.Count ];

            for( int g = 0; g < settings.Groups.Count; g++ ) {
                Settings.GroupInfo ginfo = (Settings.GroupInfo)settings.Groups[ g ];
                grpNames[ g ] = ginfo.Name;
                grpNames2[ g ] = ginfo.Name;
                grpIndexes[ g ] = g;
                grpCorrs[ g ] = ginfo.Correlation;
            }

            // sort the names
            Array.Sort( grpNames, grpIndexes );
            Array.Sort( grpNames2, grpCorrs );
            groupsComboBox.Items.Clear();
            groupsComboBox.Items.AddRange( grpNames );
        }

        private void Group2_Load( object sender, EventArgs e ) {
            if( groupsComboBox.Items.Count > 0 ) {
                groupsComboBox.SelectedIndex = 0;
            }
        }

        private void okButton_Click( object sender, EventArgs e ) {
            int selItem = groupsComboBox.SelectedIndex;
            if( selItem >= 0 ) {
                int grpIndex = grpIndexes[ selItem ];
                Settings.GroupInfo groupInfo = (Settings.GroupInfo)settings.Groups[ grpIndex ];
                 //add the items to the group
                foreach( ListViewItem litem in listView.SelectedItems ) {
                    MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)litem.Tag;
                    int itemToAddIndx = vinfo.RowNumber;
                    Console.WriteLine( "Adding item {0} ({2}) to group {1}", itemToAddIndx, grpIndex, vinfo.Name );
                    groupInfo.ItemIndexes.Add( itemToAddIndx );
                    groupInfo.itemNitroNames.Add( vinfo.Name );
                }
            }
            this.settings.SetEdited();
        }

        private void newButton_Click( object sender, EventArgs e ) {
            NewGroup ngDlg = new NewGroup( this.marketPlan, this.settings );
            DialogResult resp = ngDlg.ShowDialog( this );
            if( resp == DialogResult.OK ) {
                Settings.GroupInfo newGroupInfo = new Settings.GroupInfo();
                newGroupInfo.Name = ngDlg.GroupName;
                newGroupInfo.Correlation = ngDlg.Correlation;
                settings.Groups.Add( newGroupInfo );
                UpdateGroupsList();
                groupsComboBox.SelectedItem = newGroupInfo.Name;
            }
        }

        private void groupsComboBox_SelectedIndexChanged( object sender, EventArgs e ) {
            int indx = groupsComboBox.SelectedIndex;
            if( indx >= 0 ) {
                double corr = grpCorrs[ indx ];
                correlationLabel.Text = String.Format( "Correlation: {0:f0}%", corr * 100 );
            }
            else {
                correlationLabel.Text = "";
            }
        }
    }
}