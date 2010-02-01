using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataImporter.Dialogs
{
    public partial class AwarenessPersuasionForm : Form
    {
        public double DisplayAwareness {
            get {
                return Convert.ToDouble( displayAwareness.Text );
            }
            set {
                displayAwareness.Text = String.Format( "{0:f2}", value );
            }
        }

        public double DistributionAwareness {
            get {
                return Convert.ToDouble( distAwareness.Text );
            }
            set {
                distAwareness.Text = String.Format( "{0:f2}", value );
            }
        }

        public double MediaAwareness {
            get {
                return Convert.ToDouble( mediaAwareness.Text );
            }
            set {
                mediaAwareness.Text = String.Format( "{0:f2}", value );
            }
        }

        public double CouponsAwareness {
            get {
                return Convert.ToDouble( couponsAwareness.Text );
            }
            set {
                couponsAwareness.Text = String.Format( "{0:f2}", value );
            }
        }

        public double DisplayPersuasion {
            get {
                return Convert.ToDouble( displayPersuasion.Text );
            }
            set {
                displayPersuasion.Text = String.Format( "{0:f2}", value );
            }
        }

        public double DistributionPersuasion {
            get {
                return Convert.ToDouble( distPersuasion.Text );
            }
            set {
                distPersuasion.Text = String.Format( "{0:f2}", value );
            }
        }

        public double MediaPersuasion {
            get {
                return Convert.ToDouble( mediaPersuasion.Text );
            }
            set {
                mediaPersuasion.Text = String.Format( "{0:f2}", value );
            }
        }

        public double CouponsPersuasion {
            get {
                return Convert.ToDouble( couponsPersuasion.Text );
            }
            set {
                couponsPersuasion.Text = String.Format( "{0:f2}", value );
            }
        }

        public AwarenessPersuasionForm() {
            InitializeComponent();
        }

        private void textboxTextChanged( object sender, EventArgs e ) {
            TextBox snd = (TextBox)sender;
            try {
                double d = double.Parse( snd.Text );
                okButton.Enabled = true;
            }
            catch( Exception ) {
                okButton.Enabled = false;
            }
        }
    }
}