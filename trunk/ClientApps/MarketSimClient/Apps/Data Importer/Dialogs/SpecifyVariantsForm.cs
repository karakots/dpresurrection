using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataImporter.Dialogs
{
    public partial class SpecifyVariantsForm : Form
    {
        public ArrayList SpecifiedItems {
            get {
                ArrayList list = new ArrayList();
                for( int i = 0; i < specifyListBox.Items.Count; i++ ) {
                    list.Add( (string)specifyListBox.Items[ i ] );
                }
                return list;
            }
        }

        public SpecifyVariantsForm( ArrayList specifiedItems ) {
            InitializeComponent();

            foreach( string xs in specifiedItems ) {
                this.specifyListBox.Items.Add( xs );
            }
        }
        
        private void addButton_Click( object sender, EventArgs e ) {
            AddItemForm addForm = new AddItemForm();
            DialogResult resp = addForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            this.specifyListBox.Items.Add( addForm.ItemValue );
        }

        private void removeButton_Click( object sender, EventArgs e ) {
            if( this.specifyListBox.SelectedIndex >= 0 ) {
                this.specifyListBox.Items.RemoveAt( this.specifyListBox.SelectedIndex );
            }
        }

        private void excludeListBox_SelectedIndexChanged( object sender, EventArgs e ) {
            if( specifyListBox.SelectedIndex != -1 ) {
                this.removeButton.Enabled = true;
            }
            else {
                this.removeButton.Enabled = false;
            }
        }
    }
}