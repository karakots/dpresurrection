using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MrktSimClient.Controls.Dialogs
{
    public partial class PlanAwarenessPersuasionForm : Form
    {
        public double Awareness {
            get {
                return double.Parse( awarenessTextBox.Text.Trim() );
            }
        }

        public double Persuasion {
            get {
                return double.Parse( persuasionTextBox.Text.Trim() );
            }
        }

        public bool ForceSpecificValues {
            get {
                return setSpecificCheckBox.Checked;
            }
        }

        public PlanAwarenessPersuasionForm() {
            InitializeComponent();
        }

        public PlanAwarenessPersuasionForm( string label, double awareness, bool awarenessIsScaling, double perssuasion, bool persuasionIsScaling ) : this() {
            this.planNameLabel.Text = label;
            awarenessTextBox.Text = String.Format( "{0:f2}", awareness );
            persuasionTextBox.Text = String.Format( "{0:f2}", perssuasion );

            if( awarenessIsScaling ) {
                awLabel.Text = "Scaling for Awareness";
                awarenessTextBox.Text = "1.0";
                awAvgLabel.Text = String.Format( "Mean Awareness = {0:f2}", awareness );
                awAvgLabel.Visible = true;
                setSpecificCheckBox.Visible = true;
            }

            if( persuasionIsScaling ) {
                perLabel.Text = "Scaling for Persuasion";
                persuasionTextBox.Text = "1.0";
                perAvgLabel.Text = String.Format( "Mean Persuasion = {0:f2}", perssuasion );
                perAvgLabel.Visible = true;
                setSpecificCheckBox.Visible = true;
            }
        }

        private void okButton_Click( object sender, EventArgs e ) {
            try {
                double dum1 = this.Awareness;
                double dum2 = this.Persuasion;
                this.DialogResult = DialogResult.OK;
            }
            catch( Exception ) {
                MessageBox.Show( "\r\n     ERROR: The entries must be numeric values.    \r\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                this.DialogResult = DialogResult.None;
            }
        }

        private void setSpecificCheckBox_CheckedChanged( object sender, EventArgs e ) {
            if( setSpecificCheckBox.Checked ) {
                awLabel.Text = "Awareness";
                perLabel.Text = "Persuasion";
                if( perAvgLabel.Visible == true ) {
                    persuasionTextBox.Text = "";
                }
                if( awAvgLabel.Visible == true ) {
                    awarenessTextBox.Text = "";
                }
                perAvgLabel.Visible = false;
                awAvgLabel.Visible = false;
            }
        }
    }
}