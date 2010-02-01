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
    public partial class DbUtils : Form
    {
        private string confirmDeleteTitle = "Confirm Deteting All Results";
        private string confirmDeleteMsg = "CAUTION: This will delete ALL results data for ALL MODELS in the {0} Database.\r\n\r\n" +
            "This action is FINAL - once deleted, there is no way to recover the results data.\r\n\r\n" +
            "Clear all results now?";
        private const string versionFormat = "{0}.{1}.{2}";

        private string shrinkMessage = "Shrink the database?\n\n(This operation will eliminate unused space from the overall database size).\n\nProceeed?";
        private string shrinkTitle = "Confirm Shrinking Database";
        private string shrinkHelpTag = "ShrinkDatabase";
        private string shrinkOkMessage = "Database Shrink Completed";
        private string shrinkErrTitle = "Error Shrinking Database";
        private string shrinkErrMessage = "Error: Shrink was not completed successfully.";

        private string helpTag = "DbUtils";
        private ProjectDb theDb;
        private string theDbName;
        
        /// <summary>
        /// Creates a new dialog for setting overall MarketSim configuration items.
        /// </summary>
        public DbUtils( ProjectDb db, string dbName, bool enableDeleteResults ) {
            InitializeComponent();

            this.theDb = db;
            this.theDbName = dbName;

            int major;
            int minor;
            int release;
            ProjectDb.GetCurrentDbVersion( out major, out minor, out release );
            versionLabel.Text = String.Format( versionFormat, major, minor, release );

            this.truncateResultsButton.Enabled = enableDeleteResults;

            GetDbSizeInfo();


            if( theDb.LockedModels() ) {
                lockedModelsLabel.Enabled = true;
                unLockModelsButton.Enabled = true;
            }
            else {
                lockedModelsLabel.Enabled = false;
                unLockModelsButton.Enabled = false;
            }

            if( theDb.NumQueuedOrRunningSims() > 0 )
            {
                stopSimBut.Enabled = true;
                stopSimLabel.Enabled = true;
            }
            else
            {
                stopSimBut.Enabled = false;
                stopSimLabel.Enabled = false;
            }

            this.groupBox2.SendToBack();
        }

        private void helpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, this.helpTag );
        }

        /// <summary>
        /// Confirms before letting the user turn on the calibration controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteResultsButton_Click( object sender, EventArgs e ) {
            string msg = String.Format( confirmDeleteMsg, this.theDbName );
            ConfirmDialog cdlg = new ConfirmDialog( msg, confirmDeleteTitle );
            cdlg.Height += 60;
            cdlg.Width += 30;
            cdlg.SetOkCancelButtonStyle();
            cdlg.SelectWarningIcon();
            DialogResult resp = cdlg.ShowDialog();
            if( resp == DialogResult.OK ) {
                // delete ALL results now!
                this.Cursor = Cursors.WaitCursor;

                theDb.ClearAllResults();
                GetDbSizeInfo();

                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Confirms with the user, then shrinks the database (eliminates unused space).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shrinkButton_Click( object sender, EventArgs e ) {
            ConfirmDialog cdlg = new ConfirmDialog( shrinkMessage, shrinkTitle, shrinkHelpTag );
            cdlg.SelectQuestionIcon();
            cdlg.SetOkCancelButtonStyle();
            cdlg.Height += 60;
            DialogResult resp = cdlg.ShowDialog();
            if( resp == DialogResult.OK ) {
                this.Cursor = Cursors.WaitCursor;

                bool shrinkOk = theDb.ShrinkDatabase( 0 );
                if( shrinkOk ) {
                    GetDbSizeInfo();
                }

                this.Cursor = Cursors.Default;
                if( shrinkOk ) {
                    CompletionDialog cmpDlg = new CompletionDialog( shrinkOkMessage );
                    cmpDlg.ShowDialog();
                }
                else {
                    ConfirmDialog errDlg = new ConfirmDialog( shrinkErrMessage, shrinkErrTitle );
                    errDlg.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Updates the DB size displays.
        /// </summary>
        private void GetDbSizeInfo() {
            this.sizeLabel.Text = theDb.DatabaseSizeInfo( null );
            this.resultsSizeLabel.Text = theDb.DatabaseSizeInfo( "results_std" );
        }

        private void unLockModelsButton_Click( object sender, EventArgs e ) {

            string message = "Unlocking the models or simulations will allow them to be edited. \r\n" +
                "If they are currently being edited then there is a risk of losing data\r\n" +
                "Are you sure you want to unlock the models or simulations?";

           
                 DialogResult rslt = MessageBox.Show( message, "Unlock models and scenarios", MessageBoxButtons.YesNo );
                 if( rslt == DialogResult.Yes )
                     theDb.UnLockModels();
            
        }

        private void stopSimBut_Click( object sender, EventArgs e )
        {
            string message = "This will reset the status of all simulations\r\n" +
               "If they are currently running then there is a risk of losing data\r\n" +
               "Are you sure you want to change the status of simulations?";


            DialogResult rslt = MessageBox.Show( message, "Reset Simulations", MessageBoxButtons.YesNo );
            if( rslt == DialogResult.Yes )
                theDb.DequeueAllSims();

        }
    }
}