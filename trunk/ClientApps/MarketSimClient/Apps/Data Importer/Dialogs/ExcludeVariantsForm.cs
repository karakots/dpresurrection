using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataImporter.Dialogs
{
    public partial class ExcludeVariantsForm : Form
    {
        public ArrayList ExcludeItems {
            get {
                ArrayList list = new ArrayList();
                for( int i = 0; i < excludeListBox.Items.Count; i++ ) {
                    list.Add( (string)excludeListBox.Items[ i ] );
                }
                return list;
            }
        }

        public ExcludeVariantsForm( ArrayList excludeItems ) {
            InitializeComponent();

            foreach( string xs in excludeItems ) {
                this.excludeListBox.Items.Add( xs );
            }
        }

        private void addButton_Click( object sender, EventArgs e ) {
            AddItemForm addForm = new AddItemForm();
            DialogResult resp = addForm.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            this.excludeListBox.Items.Add( addForm.ItemValue );
        }

        private void removeButton_Click( object sender, EventArgs e ) {
            if( this.excludeListBox.SelectedIndex >= 0 ) {
                this.excludeListBox.Items.RemoveAt( this.excludeListBox.SelectedIndex );
            }
        }

        private void excludeListBox_SelectedIndexChanged( object sender, EventArgs e ) {
            if( excludeListBox.SelectedIndex != -1 ) {
                this.removeButton.Enabled = true;
            }
            else {
                this.removeButton.Enabled = false;
            }
        }
    }
}