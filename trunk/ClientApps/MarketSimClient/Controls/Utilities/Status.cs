using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Utilities
{
    /// <summary>
    /// Status is a class that is useful for getting user status messages to show on the status strip, 
    /// and includes support doing for time-consuming processing in a background thread.
    /// </summary>
    public class Status
    {
        public delegate void OnNextStep();
        public delegate void BackgroundProcess();

        public delegate void StatusUtility( ContainerControl actionControl );

        private StatusShower shower;
        private OnNextStep nextStep;
        private Timer nextStepTimer;
        private System.Threading.Thread workerThread;
        private int waitCount;
        private int minCyclingProgress;
        private int maxCyclingProgress;
        private string busyStr;
        private string doneStr;

        /// <summary>
        /// Requests that the given controls parent Form switch to the hourglass cursor.  Keeps a level count so ithe cursor doesn't
        /// return to regular until as many calls to ClearWaitCursor() have been made as were made to SetWaitCursor() before.
        /// </summary>
        /// <param name="actionControl"></param>
        public static void SetWaitCursor( ContainerControl actionControl ) {
            if( actionControl.InvokeRequired )
            {
                StatusUtility setCursor = new StatusUtility( SetWaitCursor );
                actionControl.Invoke( setCursor, new object[] { actionControl } );
            }
            else
            {
                StatusShower shower = GetShower( actionControl );
                if( shower == null )
                {
                    throw new Exception( "Call to SetWaitCursor() where actionControl's ParentForm is not a StatusShower" );
                }

                if( actionControl.ParentForm != null )
                {
                    actionControl.ParentForm.UseWaitCursor = true;
                    actionControl.ParentForm.Cursor = Cursors.WaitCursor;
                    ////Console.WriteLine( "+ + + Set Wait Cursor for {0}", actionControl.ParentForm.Name );
                }
                else
                {
                    actionControl.Cursor = Cursors.WaitCursor;
                    actionControl.UseWaitCursor = true;
                    ////Console.WriteLine( "+ + + Set Wait Cursor for {0} -  non-parent!", actionControl.Name );
                }
                shower.WaitCursorLevel += 1;
            }
        }

        /// <summary>
        /// Restores the cursor to normal if there is only one SetWaitCursor() call on the stack.  Otherwise reduces the stack count by 1.
        /// </summary>
        /// <param name="actionControl"></param>
        public static void ClearWaitCursor( ContainerControl actionControl ) {

            if( actionControl.InvokeRequired )
            {
                StatusUtility clearCursor = new StatusUtility( ClearWaitCursor );
                actionControl.Invoke( clearCursor, new object[] { actionControl } );
            }
            else
            {
                StatusShower shower = GetShower( actionControl );
                if( shower == null )
                {
                    throw new Exception( "Call to ClearWaitCursor() where actionControl's ParentForm is not a StatusShower" );
                }
                if( shower.WaitCursorLevel > 0 )
                {
                    shower.WaitCursorLevel -= 1;
                }
                if( shower.WaitCursorLevel == 0 )
                {
                    if( actionControl.ParentForm != null )
                    {
                        actionControl.ParentForm.Cursor = Cursors.Default;
                        actionControl.ParentForm.UseWaitCursor = false;
                        ////Console.WriteLine( "- - -  Clear Wait Cursor for {0}", actionControl.ParentForm.Name );
                    }
                    else
                    {
                        actionControl.Cursor = Cursors.Default;
                        actionControl.UseWaitCursor = false;
                        ////Console.WriteLine( "- - -  Clear Wait Cursor for {0} -  non-parent!", actionControl.Name );
                    }
                }
            }
        }

        public static StatusShower GetShower( ContainerControl actionControl )
        {
            if( actionControl.ParentForm != null && actionControl.ParentForm is StatusShower )
            {
                return actionControl.ParentForm as StatusShower;
            }
            else if( actionControl is StatusShower )
            {
                return actionControl as StatusShower;
            }

            return null;
        }

        /// <summary>
        /// Creates a status object.  Each StatusShower (typically a Frame) should have its own Status object.
        /// </summary>
        /// <param name="statusShower"></param>
        public Status( ContainerControl actionControl  ) {
            this.shower = GetShower( actionControl );
        }

        /// <summary>
        /// Updates the status UI while waiting for a worker thread to finish its processing.
        /// </summary>
        /// <param name="nextStepMethod">The OnNextStep method to call when the thread is finished.</param>
        /// <param name="workerThreadToWaitFor">The worker thread to wait for</param>
        /// <param name="checkInterval">How often to check the worker thread status</param>
        /// <param name="thisControl">Must live in a form that implements Status.StatusShower</param>
        /// <param name="busySatusTxt">Status text to display during the thread processing</param>
        /// <param name="minProgress">Minimal progress-bar amount to cycle through during thread processing</param>
        /// <param name="maxProgress">Maximal progress-bar amount to cycle through during thread processing</param>
        /// <param name="doneStatiusTxt">Status text to display after the thread processing finishes (seen after nextStepMethod completes) </param>
        public void UpdateUIAndContinue( OnNextStep nextStepMethod, System.Threading.Thread workerThreadToWaitFor, 
            int checkInterval, string busySatusTxt, int minProgress, int maxProgress, string doneStatiusTxt ) {

            busyStr = busySatusTxt;
            doneStr = doneStatiusTxt;

            nextStep = nextStepMethod;
            workerThread = workerThreadToWaitFor;
            waitCount = 0;
            minCyclingProgress = minProgress;
            maxCyclingProgress = maxProgress;

            nextStepTimer = new Timer();
            nextStepTimer.Interval = checkInterval;
            nextStepTimer.Tick += new EventHandler( NextStepCheckHandler );
            nextStepTimer.Start();
        }

        /// <summary>
        /// Checks to see if the worker thread is still alive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextStepCheckHandler( object sender, EventArgs e ) {
            waitCount++;
            if( workerThread.IsAlive ) {

                int spinPct = ((10 * waitCount) % (maxCyclingProgress - minCyclingProgress)) + minCyclingProgress;
                shower.SetStatus( busyStr, spinPct );
                return;
            }
            shower.SetStatus( doneStr, maxCyclingProgress );
            nextStepTimer.Stop();
            nextStep();
        }

        /// <summary>
        /// Updates the status UI elements.  Should be the last line in a method; processing will continue with a call to nextStepMethod.
        /// </summary>
        /// <param name="nextStepMethod"></param>
        /// <param name="thisControl"></param>
        /// <param name="statusTxt"></param>
        /// <param name="statusPercent"></param>
        public void UpdateUIAndContinue( OnNextStep nextStepMethod, string statusTxt, int statusPercent ) {
            //if( nextStepTimer != null ) {
            //    if( nextStep != null && nextStepMethod != nextStep ) {
            //        Console.WriteLine( "Error: Attempting to start two different timers simultaneously!\n{0} vs. {1}", nextStepMethod.ToString(), nextStep.ToString() );
            //    }
            //    return;
            //}
            //Console.WriteLine( "UpdateUIAndContinue....  " + statusTxt );
            nextStep = nextStepMethod;

            curStat = statusTxt;

            SetStatus( statusTxt, statusPercent );

            nextStepTimer = new Timer();
            nextStepTimer.Interval = 1;
            nextStepTimer.Tick += new EventHandler( NextStepHandler );
            nextStepTimer.Start();
        }

        private string curStat;

        /// <summary>
        /// Carries on processing after UpdateUIAndContinue()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextStepHandler( object sender, EventArgs e ) {
 //           Console.WriteLine( "...NextStepHandler: " + curStat );
            nextStepTimer.Stop();
            nextStep();
        }

        /// <summary>
        /// Sets the text and progres bar value of the status strip of the control's owning form, which must implement StatusShower
        /// </summary>
        /// <param name="statusTxt"></param>
        /// <param name="statusPercent"></param>
        /// <param name="thisControl"></param>
        public void SetStatus( string statusTxt, int statusPercent ) {
            if( this.shower != null ) {
                if( statusTxt != null ) {
                    this.shower.SetStatus( statusTxt, statusPercent );
                }
                else {
                    this.shower.ClearStatus();
                }
            }
        }

        /// <summary>
        /// Clears the status strip of the control's owning form, which must implement StatusShower
        /// </summary>
        /// <param name="statusTxt"></param>
        /// <param name="statusPercent"></param>
        /// <param name="thisControl"></param>
        public void ClearStatus() {
            if( shower != null ) {
                shower.ClearStatus();
            }
        }

        public static void ClearStatus( ContainerControl actionControl )
        {
            if( actionControl.InvokeRequired )
            {
                StatusUtility clearStatus = new StatusUtility( ClearStatus );
                actionControl.Invoke( clearStatus, new object[] { actionControl } );
            }
            else
            {

                if( actionControl.ParentForm != null )
                {
                    actionControl.ParentForm.Cursor = Cursors.Default;
                    actionControl.ParentForm.UseWaitCursor = false;
                    ////Console.WriteLine( "- - -  Clear Wait Cursor for {0}", actionControl.ParentForm.Name );
                }
                else
                {
                    actionControl.Cursor = Cursors.Default;
                    actionControl.UseWaitCursor = false;
                    ////Console.WriteLine( "- - -  Clear Wait Cursor for {0} -  non-parent!", actionControl.Name );
                }

                StatusShower shower = GetShower( actionControl );

                if( shower == null )
                {
                   // throw new Exception( "Call to ClearStatus() " + actionControl.Name + "  is not a StatusShower" );
                }
                else
                {

                    shower.WaitCursorLevel = 0;

                    shower.ClearStatus();
                }
            }
        }

        /// <summary>
        /// Starts the given method in a background thread.
        /// </summary>
        /// <param name="backgroundProcess"></param>
        /// <returns></returns>
        public System.Threading.Thread StartBackgroundThread( System.Threading.ThreadStart backgroundProcess ) {
            System.Threading.Thread backgroundThread = new System.Threading.Thread( backgroundProcess );
            backgroundThread.Start();
            // wait for the thread to start
            while( !backgroundThread.IsAlive ) ;
            return backgroundThread;
        }

        ///// <summary>
        ///// Returns the Status object that the given object should use.
        ///// </summary>
        ///// <param name="actionControl"></param>
        ///// <returns></returns>
        //public static Status GetStatusHandler( ContainerControl actionControl ) {
        //    StatusShower shower = actionControl.ParentForm as StatusShower;
        //    if( shower != null ) {
        //        return shower.GetStatusHandler();
        //    }
        //    else {
        //        return null;
        //    }
        //}
   }


    /// <summary>
    /// Methods that a Form must implement to work with the Status class.
    /// </summary>
    public interface StatusShower
    {
        int WaitCursorLevel { set; get; }
        void SetStatus( string txt, int percent );
        void ClearStatus();
    }
}
