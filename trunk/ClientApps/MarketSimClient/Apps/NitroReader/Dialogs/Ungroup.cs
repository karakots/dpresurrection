using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NitroReader.Dialogs
{
    public partial class Ungroup : Form
    {
        private ListView listView;
        private Settings settings;

        public Ungroup( ListView listView, Settings settings ) {
            InitializeComponent();

            this.listView = listView;
            this.settings = settings;

            bool multipleGroups = false;
            string itemName = "";
            int group = -1;
            foreach( ListViewItem litem in listView.SelectedItems ) {
                MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)litem.Tag;
                int itemGroup = vinfo.GroupIndex;
                if( group == -1 ) {
                    group = itemGroup;
                    itemName = vinfo.Name;
                }
                else {
                    if( itemGroup != group ) {
                        multipleGroups = true;
                        break;
                    }
                }
            }

            int nItems = listView.SelectedIndices.Count;
            string groupName = null;
            if( multipleGroups == false ) {
                Settings.GroupInfo ginfo = (Settings.GroupInfo)settings.Groups[ group ];
                groupName =ginfo.Name;
            }

            if( nItems == 1 ) {
                infoLabel.Text = String.Format( "OK to remove \"{0}\" from the \"{1}\" group?", itemName, groupName );
            }
            else if( multipleGroups == false ) {
                infoLabel.Text = String.Format( "OK to remove the {0} selected variants from the \"{1}\" group?", nItems, groupName );
            }
            else {
                 infoLabel.Text = String.Format( "OK to remove the {0} selected variants from their groups?", nItems );
           }
        }

        private void okButton_Click( object sender, EventArgs e ) {
            foreach( ListViewItem litem in listView.SelectedItems ) {
                MarketPlan.VariantInfo vinfo = (MarketPlan.VariantInfo)litem.Tag;
                int itemToUngroupIndx = vinfo.RowNumber;
                int currentGroup = vinfo.GroupIndex;
                //change the master settings 
                ((Settings.GroupInfo)(this.settings.Groups[ currentGroup ])).ItemIndexes.Remove( itemToUngroupIndx );
                ((Settings.GroupInfo)(this.settings.Groups[ currentGroup ])).itemNitroNames.Remove( itemToUngroupIndx );

                //update the display (vinfo points to the marketPlan.Variants list)
                vinfo.GroupIndex = -1;
                vinfo.BackColor = Color.White;
            }
            this.settings.SetEdited();
        }
    }
}