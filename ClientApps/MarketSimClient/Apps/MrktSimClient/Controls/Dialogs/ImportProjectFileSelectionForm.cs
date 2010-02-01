using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MrktSimClient.Controls.Dialogs
{
    public partial class ImportProjectFileSelectionForm : Form
    {
        public ArrayList GetSelectedFiles( string rootDir ) {
            ArrayList vals = new ArrayList();
            for( int i = 0; i < checkedListBox.CheckedItems.Count; i++ ) {
                vals.Add( rootDir + "\\" + (string)checkedListBox.CheckedItems[ i ] );
            }
            return vals;
        }

        public ImportProjectFileSelectionForm() {
            InitializeComponent();
        }

        public ImportProjectFileSelectionForm( ArrayList fileRelativePathList ): this() {
            for( int i = 0; i < fileRelativePathList.Count; i++ ) {
                checkedListBox.Items.Add( (string)fileRelativePathList[ i ] );
            }
            CheckAll( true );
        }

        private void button4_Click( object sender, EventArgs e ) {
            CheckAll( true );
        }

        private void CheckAll( bool chk ) {
            for( int i = 0; i < checkedListBox.Items.Count; i++ ) {
                checkedListBox.SetItemChecked( i, chk );
            }
        }

        private void uncheckAllButton_Click( object sender, EventArgs e ) {
            CheckAll( false );
        }

        private void CheckOnly( string prefix ) {
            for( int i = 0; i < checkedListBox.Items.Count; i++ ) {
                string item = (string)checkedListBox.Items[ i ];
                bool doCheck = item.StartsWith( prefix );
                checkedListBox.SetItemChecked( i, doCheck );
            }
        }

        private void button1_Click( object sender, EventArgs e ) {
            CheckOnly( "Distribution\\" );
        }

        private void button2_Click( object sender, EventArgs e ) {
            CheckOnly( "Display\\" );
        }

        private void button3_Click( object sender, EventArgs e ) {
            CheckOnly( "Media\\" );
        }

        private void button5_Click( object sender, EventArgs e ) {
            CheckOnly( "Price\\" );
        }
    }
}