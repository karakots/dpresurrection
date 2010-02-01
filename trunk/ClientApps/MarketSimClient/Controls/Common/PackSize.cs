using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


using MrktSimDb;
using Common.Utilities;
using MarketSimUtilities.MsTree;
using MarketSimUtilities;

using ExcelInterface;
using ErrorInterface;

using Common.Dialogs;

namespace Common
{
    public partial class PackSize :  MrktSimControl
    {

        public override void Refresh()
        {
            base.Refresh();

            this.updateList();
            this.packValuesGrid.Refresh();
        }


        // edits are written into dabatase
        public override void Flush()
        {
          
            packValuesGrid.Flush();
        }

        public override bool Suspend
        {
            set
            {
                base.Suspend = value;
                packValuesGrid.Suspend = value;

                if( !value )
                {
                    // update list
                    updateList();
                }
            }
        }

        public override void Reset()
        {
            base.Reset();

            this.updateList();
            packValuesGrid.Reset();

            this.createTableStyle();
        }

        public PackSize( ModelDb db ) : base(db)
        {
            InitializeComponent();

            //packSizeGrid.Table = db.Data.pack_size;
            //packSizeGrid.RowFilter = "id <> " + Database.AllID;
            //packSizeGrid.DescriptionWindow = false;

            packValuesGrid.Table = db.Data.pack_size_dist;
            packValuesGrid.DescriptionWindow = false;
            packValuesGrid.AllowDelete = false;

            this.updateList();

            createTableStyle();
        }


        private void createTableStyle()
        {
            this.SuspendLayout();

            packValuesGrid.Clear();

           // packValuesGrid.AddComboBoxColumn( "pack_size_id", theDb.Data.pack_size, "name", "id", true );
            packValuesGrid.AddNumericColumn( "size", true );
            packValuesGrid.AddNumericColumn( "dist", "% Probability" );

            packValuesGrid.Reset();

            this.ResumeLayout( false );
        }

        private void updateList()
        {
            packSizeList.Items.Clear();
         

            DataRow[] rows = theDb.Data.pack_size.Select("","", DataViewRowState.CurrentRows);

            foreach( MrktSimDBSchema.pack_sizeRow pack in rows )
            {
                if (pack.id == Database.AllID)
                {
                    continue;
                }

                packSizeList.Items.Add( pack );
            }

            packSizeList.DisplayMember = "name";

            if( packSizeList.Items.Count == 0 )
            {
                enableMenuItems( false );
            }
            else
            {
                enableMenuItems( true );
                packSizeList.SelectedIndex = 0;
            }
        }

        private void createPackSize_Click( object sender, EventArgs e )
        {
            EditPackSize dlg = new EditPackSize(theDb, null);

            DialogResult rslt = dlg.ShowDialog( this);

            if( rslt == DialogResult.OK )
            {
                // create pack size

                MrktSimDBSchema.pack_sizeRow pack = dlg.Pack;

                packSizeList.Items.Add( pack );

                enableMenuItems( true );
                packSizeList.SelectedItem = pack;
            }
        }

        private void editToolStripMenuItem_Click( object sender, EventArgs e )
        {
            MrktSimDBSchema.pack_sizeRow pack = (MrktSimDBSchema.pack_sizeRow) this.packSizeList.SelectedItem;

            if( pack == null )
            {
                return;
            }

            EditPackSize dlg = new EditPackSize(theDb, pack);

            DialogResult rslt = dlg.ShowDialog();

            if( rslt == DialogResult.OK )
            {
                updateList();

                packSizeList.SelectedItem = pack;
            }
        }

        private void deleteToolStripMenuItem_Click( object sender, EventArgs e )
        {
            MrktSimDBSchema.pack_sizeRow pack = (MrktSimDBSchema.pack_sizeRow)this.packSizeList.SelectedItem;

            if( pack == null )
            {
                return;
            }

            int index = packSizeList.SelectedIndex;
            packSizeList.Items.Remove( pack );

            pack.Delete();

            if( packSizeList.Items.Count > 0 )
            {
                if( index > 0 )
                {
                    packSizeList.SelectedIndex = index - 1;
                }
                else
                {
                    packSizeList.SelectedIndex = 0;
                }

                enableMenuItems( true );
            }
        }

        private void packSizeList_SelectedIndexChanged( object sender, EventArgs e )
        {
            MrktSimDBSchema.pack_sizeRow pack = (MrktSimDBSchema.pack_sizeRow)this.packSizeList.SelectedItem;

            if( pack == null )
            {
                packValuesGrid.RowFilter = "";
                
                enableMenuItems( false );

                return;
            }

            // filter the grid

            string query = "pack_size_id = " + pack.id;
            packValuesGrid.RowFilter = query;
        }

        private void enableMenuItems( bool trueORfalse )
        {
            this.editBut.Enabled = trueORfalse;
            this.deleteBut.Enabled = trueORfalse;
        }
    }
}
