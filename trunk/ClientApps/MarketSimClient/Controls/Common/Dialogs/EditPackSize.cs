using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;

namespace Common.Dialogs
{
    public partial class EditPackSize : Form
    {
        const string uniformMessage = "A uniform distribution\r\nwill be created";
        const string standardMessage = "Probabilities are divided\r\n" +
            "by total to sum to 100%\r\n" +
            "If total = 0 a uniform\r\n" +
            "distribution is created.";

        public EditPackSize( ModelDb db, MrktSimDBSchema.pack_sizeRow packIn )
        {
            InitializeComponent();

            theDb = db;

            pack = packIn;

            init();
        }

        private ModelDb theDb;
        private MrktSimDBSchema.pack_sizeRow pack;

        public MrktSimDBSchema.pack_sizeRow Pack
        {
            get
            {
                return pack;
            }
        }

        public string PackName
        {
            get
            {
                return this.nameBox.Text;
            }

            set
            {
                this.nameBox.Text = value;
            }
        }

        public int NumPoints
        {
            get
            {
                return (int) this.numDistPoints.Value;
            }

            set
            {
                if (value == 0)
                {
                    this.numDistPoints.Value = 1;
                }
                else
                {
                    this.numDistPoints.Value = value;
                }
            }
        }

        private void init()
        {
            if( pack == null )
            {
                // we are creating a pack
                PackName = "<Enter Name For Pack Size>";
                NumPoints = 5;

                updateStats();

                applyBut.Enabled = false;
                infoBox.Text = uniformMessage;
            }
            else
            {
                PackName = pack.name;
                NumPoints = pack.Getpack_size_distRows().Length;

                updateStats();
            }
        }

        const double eps = 0.01;
        private void updateStats()
        {
            this.totalBox.Text = "--";
            this.meanBox.Text = "--";
            this.devBox.Text = "--";

            this.devBox.Enabled = false;
            this.normalizeBox.Enabled = true;
            this.normalizeBox.Checked = true;
            this.infoBox.Visible = true;
            infoBox.Text = standardMessage;

            this.totalBox.Enabled = false;
            this.meanBox.Enabled = false;

            if( pack == null )
            {
                return;
            }

            double total = 0;
            double ave = 0;
            double sigma2 = 0;

            foreach( MrktSimDBSchema.pack_size_distRow row in pack.Getpack_size_distRows() )
            {
                total += row.dist;

                ave += row.size * row.dist / 100.0;
                sigma2 += row.size * row.size * row.dist / 100.0;

            }

            sigma2 -= ave * ave;

            if( sigma2 > 0 )
            {
                sigma2 = Math.Sqrt( sigma2 );
            }
            else
            {
                sigma2 = 0;
            }


            // now display state
            this.totalBox.Text = total.ToString( "N2" );
            this.totalBox.Enabled = true;

            if( total <= 100 + eps )
            {
                this.meanBox.Text = ave.ToString( "N2" );
                this.meanBox.Enabled = true;

                this.devBox.Text = sigma2.ToString( "N2" );
                this.devBox.Enabled = true;
            }

            if( Math.Abs( total - 100 ) <= eps )
            {
                this.normalizeBox.Checked = false;
            }
            else if( total < eps )
            {
                infoBox.Text = uniformMessage;
            }
        }

        private void normalize()
        {
            if( pack == null )
            {
                return;
            }

            int num = pack.Getpack_size_distRows().Length;

            if( num == 0 )
            {
                return;
            }

            double total = 0;
            foreach( MrktSimDBSchema.pack_size_distRow row in pack.Getpack_size_distRows() )
            {
                total += row.dist / 100.0;
            }

            // two cases
            if( total > eps )
            {
                foreach( MrktSimDBSchema.pack_size_distRow row in pack.Getpack_size_distRows() )
                {
                    row.dist /= total;
                }
            }
            else
            {
                double scale = 100.0 / num;
                foreach( MrktSimDBSchema.pack_size_distRow row in pack.Getpack_size_distRows() )
                {
                    row.dist = scale;
                }
            }
        }

        #region UI control
      
        public bool apply()
        {
            // check name
            if( !Database.LegalName( this.PackName ) )
            {
                MessageBox.Show( this, "Please enter a legal name" );
                return false;
            }

            if( pack == null )
            {
                // create a new pack size
                pack = theDb.CreatePackSize( PackName, NumPoints );
            }
            else
            {
                pack.name = PackName;
                theDb.EditPackSize( pack, NumPoints );
            }

            if( normalizeBox.Checked )
            {
                normalize();
            }

            return true;
        }

        private void okBut_Click( object sender, EventArgs e )
        {
            if( !apply() )
            {
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void cancelBut_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.Cancel;
        }


        private void applyBut_Click( object sender, EventArgs e )
        {
            apply();

            updateStats();
        }

        #endregion

    }
}
