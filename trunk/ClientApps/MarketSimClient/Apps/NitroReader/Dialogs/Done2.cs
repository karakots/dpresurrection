using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NitroReader.Dialogs
{
    public partial class Done2 : Form
    {
        private GeneralSettingsValues settings;

        public Done2( string planName, string planPath, int variantCount, TimeSpan time, GeneralSettingsValues settings ) {
            InitializeComponent();

            this.settings = settings;

            this.planNameLabel.Text = planName;
            this.planPathLabel.Text = planPath;
            this.variantCountLabel.Text = variantCount.ToString();

            if( MarketPlan.errors != null ) {
                if( (MarketPlan.errors.Count + MarketPlan.warnings.Count) > 0 ) {
                    this.warningsButton.Visible = true;
                }
            }

            this.timeLabel.Text = String.Format( "{0:f2} s", time.TotalSeconds );
            this.showReportCheckBox.Checked = settings.ShowReportOnCompletion;
        }

        private void warningsButton_Click( object sender, EventArgs e ) {
            if( MarketPlan.errors != null ) {
                string name = "Errors";
                string title = "Errors:";
                string errs = "";

                // accumulate errors
                if( MarketPlan.errors.Count > 0 ) {
                    if( MarketPlan.warnings.Count > 0 ) {
                        name = "Errors/Warnings";
                        title = "Errors and Warnings:";
                        errs = "Errors:\r\n";
                    }
                    foreach( string s in MarketPlan.errors ) {
                        errs += s + "\r\n";
                    }
                }

                // accumulate warnings
                if( MarketPlan.warnings.Count > 0 ) {
                    if( MarketPlan.errors.Count > 0 ) {
                        errs += "\r\nWarnings:\r\n";
                    }
                    else {
                        // warnings only
                        name = "Warnings";
                        title = "Warnings:";
                    }
                    foreach( string s in MarketPlan.warnings ) {
                        errs += s + "\r\n";
                    }
                }
                InfoForm warningForm = new InfoForm( name, title, errs );
                warningForm.ShowDialog( this );
            }
        }

        private void showReportCheckBox_CheckedChanged( object sender, EventArgs e ) {
            this.settings.ShowReportOnCompletion = showReportCheckBox.Checked;
        }
    }
}