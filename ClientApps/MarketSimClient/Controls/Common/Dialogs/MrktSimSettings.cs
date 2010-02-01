using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MrktSimDb;
using Utilities;

namespace Common.Dialogs
{
    public partial class MrktSimSettings : Form
    {
        private string helpTag = "MrktSimSettings";
        private string confirmCalTitle = "Confirm Showing Calibration Controls";
        private string confirmCalMsg = "WARNING: Calibration Controls can affect and potentially invalidate your Model. \r\n" +
            "Enabling this option is not recommended unless you require Calibration features. \r\n\r\nEnable Calibration Controls?";

        public bool EnableCalibrationControls {
            get {
                return this.showCalibrationControlsCheckBox.Checked;
            }
        }
        
        /// <summary>
        /// Creates a new dialog for setting overall MarketSim configuration items.
        /// </summary>
        public MrktSimSettings( bool enableCalibrationControls, bool enableCalibrationSettingControl ) {
            InitializeComponent();

            this.showCalibrationControlsCheckBox.Enabled = enableCalibrationSettingControl;

            this.showCalibrationControlsCheckBox.CheckedChanged -=new EventHandler(showCalibrationControlsCheckBox_CheckedChanged);
            this.showCalibrationControlsCheckBox.Checked = enableCalibrationControls;
            this.showCalibrationControlsCheckBox.CheckedChanged += new EventHandler( showCalibrationControlsCheckBox_CheckedChanged );
        }

        private void helpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, this.helpTag );
        }

        /// <summary>
        /// Confirms before letting the user turn on the calibration controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showCalibrationControlsCheckBox_CheckedChanged( object sender, EventArgs e ) {
            if( this.showCalibrationControlsCheckBox.Checked == true ) {
                ConfirmDialog cdlg = new ConfirmDialog( confirmCalMsg, confirmCalTitle );
                cdlg.Height += 60;
                cdlg.SetOkCancelButtonStyle();
                cdlg.SelectWarningIcon();
                DialogResult resp = cdlg.ShowDialog();
                if( resp != DialogResult.OK ) {
                    this.showCalibrationControlsCheckBox.Checked = false;
                }
            }
        }
    }
}