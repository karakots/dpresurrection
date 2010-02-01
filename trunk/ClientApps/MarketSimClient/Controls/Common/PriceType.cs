using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;
using Common.Utilities;
using MarketSimUtilities;
using ExcelInterface;
using ErrorInterface;

namespace Common
{
    public partial class PriceType : MarketSimUtilities.MrktSimControl
    {
        public override void Refresh()
        {
            base.Refresh();

            this.priceTypeGrid.Refresh();
        }


        // edits are written into dabatase
        public override void Flush()
        {
            priceTypeGrid.Flush();
        }

        public override bool Suspend
        {
            set
            {
                base.Suspend = value;
                priceTypeGrid.Suspend = value;
            }
        }

        public override void Reset()
        {
            base.Reset();

            priceTypeGrid.Reset();

            this.createTableStyle();
        }

        public PriceType( ModelDb db ) : base( db )
        {
            InitializeComponent();


            priceTypeGrid.Table = db.Data.price_type;
            priceTypeGrid.DescriptionWindow = false;

            priceTypeGrid.RowFilter = "id <> " + Database.AllID;
          
            createTableStyle();
        }

        private void createTableStyle()
        {
            this.SuspendLayout();

            priceTypeGrid.Clear();

            priceTypeGrid.AddTextColumn( "name" );
            priceTypeGrid.AddCheckBoxColumn( "relative", "Relative Pricing");
            priceTypeGrid.AddNumericColumn( "awareness");
            priceTypeGrid.AddNumericColumn( "persuasion" );
            priceTypeGrid.AddNumericColumn( "BOGN" );

            priceTypeGrid.Reset();

            this.ResumeLayout( false );

        }

        private void addNewPriceTypeToolStripMenuItem_Click( object sender, EventArgs e )
        {
            InputString dlg = new InputString();

            dlg.Text = "Enter Name for Price Type";
            dlg.InputText = "";

            DialogResult rslt = dlg.ShowDialog();

            if( rslt != DialogResult.OK )
            {
                return;
            }

            string token = dlg.InputText;

            this.theDb.CreatePriceType( token );
        }
    }
}
