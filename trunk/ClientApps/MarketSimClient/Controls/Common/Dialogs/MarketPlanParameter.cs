using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MrktSimDb;
using System.Data;
using Utilities;

namespace Common.Dialogs
{
	/// <summary>
	/// Summary description for MarketPlanParameter.
	/// </summary>
	public class MarketPlanParameter : System.Windows.Forms.Form
    {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        private string helpTag = "MarketPlanParameters";

        private string connectorString = " .  .  .  .  .  .  .  .  .  .  .  . ";

        private int numParms = -1;
        private int maxParms = 6;
        private int parmRowHeight = 18;
        private CheckBox[] parmCheckBoxes;
        private CheckBox checkBox6;
        private TextBox textBox6;
        private TextBox[] valueTextBoxes;
        private bool[] parmWasInitiallyOn;

		public MarketPlanParameter(MrktSimDBSchema.market_planRow mrktPln)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            parmCheckBoxes = new CheckBox[ maxParms ];
            parmCheckBoxes[ 0 ] = this.checkBox1;
            parmCheckBoxes[ 1 ] = this.checkBox2;
            parmCheckBoxes[ 2 ] = this.checkBox3;
            parmCheckBoxes[ 3 ] = this.checkBox4;
            parmCheckBoxes[ 4 ] = this.checkBox5;
            parmCheckBoxes[ 5 ] = this.checkBox6;

            valueTextBoxes = new TextBox[ maxParms ];
            valueTextBoxes[ 0 ] = this.textBox1;
            valueTextBoxes[ 1 ] = this.textBox2;
            valueTextBoxes[ 2 ] = this.textBox3;
            valueTextBoxes[ 3 ] = this.textBox4;
            valueTextBoxes[ 4 ] = this.textBox5;
            valueTextBoxes[ 5 ] = this.textBox6;

            parmWasInitiallyOn = new bool[ maxParms ];
            for( int k = 0; k < maxParms; k++ ) {
                parmWasInitiallyOn[ k ] = false;
            }          

            parmRowHeight = this.textBox2.Location.Y - this.textBox1.Location.Y;

            for( int k = 0; k < maxParms; k++ ) {
                parmCheckBoxes[ k ].CheckedChanged += new EventHandler( CheckBoxCheckedChanged );
            }
			
			mrktPlan = mrktPln;

			this.Text = "Parameters for " + mrktPlan.name;    // set title

            string[] parms = (string[]) Common.ModelParameter.MarketPlanParamNames[ (ModelDb.PlanType)mrktPlan.type ];
            //parms should never be null!

            //// the available parameters depend on type of market plan
            //switch((ModelDb.PlanType) mrktPlan.type)
            //{
            //    case ModelDb.PlanType.Display:
            //        parms = new string[] {"Awareness Scale", "Persuasion Scale", "Distribution Scale", "Apply to ALL Display Plans"};
            //        break;

            //    case ModelDb.PlanType.Distribution:
            //        parms = new string[] { "Awareness Scale", "Persuasion Scale", "Distribution Scale", "Apply to ALL Distribution Plans" };
            //        break;

            //    case ModelDb.PlanType.Market_Utility:
            //        parms = new string[] { "Awareness Scale", "Persuasion Scale", "Utility Scale", "Distribution Scale", "Apply to ALL Market Utility Plans" };
            //        break;

            //    case ModelDb.PlanType.Media:
            //        parms = new string[] { "Awareness Scale", "Persuasion Scale", "GRP Scale", "Apply to ALL Media Plans" };
            //        break;

            //    case ModelDb.PlanType.Coupons:
            //        parms = new string[] { "Awareness Scale", "Persuasion Scale", "Percent Population Scale", "Redemption Scale", "Apply to ALL Coupon Plans" };
            //        break;

            //    case ModelDb.PlanType.Price:
            //        parms = new string[] { "Price Scale", "Markup Scale", "Periodic Price Scale", "Percent Distribution Scale", "Apply to ALL Price Plans" };
            //        break;

            //    case ModelDb.PlanType.ProdEvent:
            //        parms = new string[] { "Modification Scale", "Apply to ALL External Factor Plans" };
            //        break;

            //    case ModelDb.PlanType.TaskEvent:
            //        parms = new string[] { "Modification Scale", "Apply to ALL Task Factor Plans" };
            //        break;
            //}

            this.numParms = parms.Length;

            string[] displayParams = new string[ parms.Length ];
            for( int k = 0; k < this.numParms - 1; k++ ) {
                displayParams[ k ] = parms[ k ] + connectorString;
            }

            displayParams[ this.numParms - 1 ] = parms[ this.numParms - 1 ];
            int i = 0;
            for( i = 0; i < this.numParms; i++ ) {
                AddParm( displayParams[ i ], i );              // add the parameter control
            }
            for( ; i < maxParms; i++ ) {
                HideParm( i );                                     // hide each unused parameter contol
            }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point( 187, 8 );
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size( 74, 20 );
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point( 187, 33 );
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size( 74, 20 );
            this.textBox2.TabIndex = 1;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point( 187, 58 );
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size( 74, 20 );
            this.textBox3.TabIndex = 2;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point( 187, 83 );
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size( 74, 20 );
            this.textBox4.TabIndex = 3;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point( 187, 108 );
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size( 74, 20 );
            this.textBox5.TabIndex = 4;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point( 13, 10 );
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size( 80, 17 );
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point( 13, 35 );
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size( 80, 17 );
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point( 13, 60 );
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size( 80, 17 );
            this.checkBox3.TabIndex = 7;
            this.checkBox3.Text = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point( 13, 85 );
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size( 80, 17 );
            this.checkBox4.TabIndex = 8;
            this.checkBox4.Text = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point( 13, 110 );
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size( 80, 17 );
            this.checkBox5.TabIndex = 9;
            this.checkBox5.Text = "checkBox5";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cancelButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point( 164, 171 );
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.okButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point( 74, 171 );
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size( 75, 23 );
            this.okButton.TabIndex = 10;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler( this.okButton_Click );
            // 
            // helpButton
            // 
            this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.helpButton.Font = new System.Drawing.Font( "Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.helpButton.Location = new System.Drawing.Point( 286, 3 );
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size( 24, 21 );
            this.helpButton.TabIndex = 14;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = false;
            this.helpButton.Click += new System.EventHandler( this.helpButton_Click );
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point( 13, 135 );
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size( 80, 17 );
            this.checkBox6.TabIndex = 15;
            this.checkBox6.Text = "checkBox6";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point( 187, 133 );
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size( 74, 20 );
            this.textBox6.TabIndex = 16;
            // 
            // MarketPlanParameter
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.ClientSize = new System.Drawing.Size( 313, 199 );
            this.Controls.Add( this.textBox6 );
            this.Controls.Add( this.textBox5 );
            this.Controls.Add( this.textBox4 );
            this.Controls.Add( this.textBox3 );
            this.Controls.Add( this.textBox2 );
            this.Controls.Add( this.textBox1 );
            this.Controls.Add( this.checkBox6 );
            this.Controls.Add( this.helpButton );
            this.Controls.Add( this.cancelButton );
            this.Controls.Add( this.okButton );
            this.Controls.Add( this.checkBox5 );
            this.Controls.Add( this.checkBox4 );
            this.Controls.Add( this.checkBox3 );
            this.Controls.Add( this.checkBox2 );
            this.Controls.Add( this.checkBox1 );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MarketPlanParameter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MarketPlanParameter";
            this.Load += new System.EventHandler( this.MarketPlanParameter_Load );
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

		#region Private Fields
		
		private MrktSimDBSchema.market_planRow mrktPlan;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private Button cancelButton;
        private Button okButton;
        private Button helpButton;
		private ModelDb theDb;

		#endregion

        /// <summary>
        /// Updates the checkbox states and value text boxes according to the data in the given database.
        /// </summary>
		public  ModelDb Db
		{
			set
			{
				theDb = value;

				UpdateSelection();
			}
		}

        /// <summary>
        /// Sets the text for the specified param checkbox.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="indx"></param>
        private void AddParm( string name, int indx ) {

            this.parmCheckBoxes[ indx ].Text = name;
            this.parmCheckBoxes[ indx ].Visible = true;

            if( indx > 0 ) {
                this.valueTextBoxes[ indx - 1 ].Visible = true;
                this.valueTextBoxes[ indx ].Visible = false;
            }
        }

        /// <summary>
        /// Hides the specified param checkbox.
        /// </summary>
        /// <param name="indx"></param>
        private void HideParm( int indx ) {
            this.parmCheckBoxes[ indx ].Visible = false;
            this.valueTextBoxes[ indx ].Visible = false;
        }

        /// <summary>
        /// Updates the display to reflect the current state of theDb.
        /// </summary>
		private void UpdateSelection()
		{
            for( int i = 0; i < this.numParms; i++ ){

                int parmNum = i + 1;
                string parmName = "parm" + parmNum;

                MrktSimDBSchema.model_parameterRow prow = theDb.ModelParameterExists( this.mrktPlan, parmName, "id" );
                if( prow == null ){	
					this.parmCheckBoxes[ i ].Checked = false;
					this.valueTextBoxes[ i ].Enabled = false;
  //                   this.valueTextBoxes[ i ].Text = "1.0";
                   UpdateParmValueTextBox( this.mrktPlan, parmNum );  //should always be 1.0 - display the actual value to be sure
				}
				else {
                    this.parmCheckBoxes[ i ].Checked = true;
                    this.valueTextBoxes[ i ].Enabled = true;
                    parmWasInitiallyOn[ i ] = true;                  

                    UpdateParmValueTextBox( this.mrktPlan, parmNum );
                }
			}
		}

        /// <summary>
        /// Save the UI state to the database
        /// </summary>
        private void SaveChanges() {
            // the final checkbox is always "apply to all plans"
            bool allPlans = this.parmCheckBoxes[ this.numParms - 1 ].Checked;

            // do the value controls
            for( int i = 0; i < this.numParms - 1; i++ ) {
                if( this.parmCheckBoxes[ i ].Checked == true || parmWasInitiallyOn[ i ] == true ) {
                    int parmNum = i + 1;
                    string parmName = "parm" + parmNum;
                    string parmTitle = this.mrktPlan.name + " " + this.parmCheckBoxes[ i ].Text;
                    if( parmTitle.EndsWith( connectorString ) ) {
                        parmTitle = parmTitle.Substring( 0, parmTitle.Length - connectorString.Length );
                    }

                    if( allPlans == false ) {
                        // create for just this market plan
                        theDb.CreatePlanParameter( this.mrktPlan, parmNum, parmTitle );
                        SetParmValue( this.mrktPlan, parmNum );
                    }
                    else {
                        // create for all market plans of the same type as this one
                        string query = "type = " + this.mrktPlan.type;
                        foreach( MrktSimDBSchema.market_planRow plan in theDb.Data.market_plan.Select( query, "", System.Data.DataViewRowState.CurrentRows ) ) {
                            parmTitle = plan.name + " " + this.parmCheckBoxes[ i ].Text;
                            theDb.CreatePlanParameter( plan, parmNum, parmTitle );
                            SetParmValue( plan, parmNum );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Transfers a value from one of the UI textboxes to the given market plan
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="parmNum"></param>
        private void SetParmValue( MrktSimDBSchema.market_planRow plan, int parmNum ) {
            string strVal = "1";
            switch( parmNum ) {
                case 1:
                    strVal = this.textBox1.Text.Trim();
                    break;
                case 2:
                    strVal = this.textBox2.Text.Trim();
                    break;
                case 3:
                    strVal = this.textBox3.Text.Trim();
                    break;
                case 4:
                    strVal = this.textBox4.Text.Trim();
                    break;
                case 5:
                    strVal = this.textBox5.Text.Trim();
                    break;
                case 6:
                    strVal = this.textBox6.Text.Trim();
                    break;
            }

            // parse the string into the value
            double parmValue = 1.0;
            try {
                parmValue = double.Parse( strVal );
            }
            catch( Exception ) {
            }

            // set the parm in the market plan itself
            switch( parmNum ) {
                case 1:
                    plan.parm1 = parmValue;
                    break;
                case 2:
                    plan.parm2 = parmValue;
                    break;
                case 3:
                    plan.parm3 = parmValue;
                    break;
                case 4:
                    plan.parm4 = parmValue;
                    break;
                case 5:
                    plan.parm5 = parmValue;
                    break;
                case 6:
                    plan.parm6 = parmValue;
                    break;
            }
        }

        /// <summary>
        /// Transfers a parm value from the given market plan to the 
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="parmNum"></param>
        private void UpdateParmValueTextBox( MrktSimDBSchema.market_planRow plan, int parmNum ) {
            double value = 1.0;
            switch( parmNum ) {
                case 1:
                    value = this.mrktPlan.parm1;
                    break;
                case 2:
                    value = this.mrktPlan.parm2;
                    break;
                case 3:
                    value = this.mrktPlan.parm3;
                    break;
                case 4:
                    value = this.mrktPlan.parm4;
                    break;
                case 5:
                    value = this.mrktPlan.parm5;
                    break;
                case 6:
                    value = this.mrktPlan.parm6;
                    break;
            }

            int displayRow = parmNum - 1;

            string valStr = value.ToString();
            if( value == 1 ) {
                valStr = "1.0";
            }
            this.valueTextBoxes[ displayRow ].Text = valStr;
        }

        // adjusts the size to be appropriate for the param count
        private void MarketPlanParameter_Load( object sender, EventArgs e ) {
            int newHeight = this.Size.Height - (this.parmRowHeight * (this.maxParms - this.numParms));
            this.Size = new Size( this.Size.Width, newHeight );
        }

        // enables/disables a value textbox when the correspondiong checkbox is chnaged
        private void CheckBoxCheckedChanged( object sender, EventArgs e ) {
            CheckBox cb = (CheckBox)sender;
            bool chkd = cb.Checked;

            if( cb == this.checkBox1 ) {
                EnableValueTextBox( this.textBox1, chkd );
            }
            else if( cb == this.checkBox2 ) {
                EnableValueTextBox( this.textBox2, chkd );
            }
            else if( cb == this.checkBox3 ) {
                EnableValueTextBox( this.textBox3, chkd );
            }
            else if( cb == this.checkBox4 ) {
                EnableValueTextBox( this.textBox4, chkd );
            }
            else if( cb == this.checkBox5 ) {
                EnableValueTextBox( this.textBox5, chkd );
            }
            else if( cb == this.checkBox6 ) {
                EnableValueTextBox( this.textBox6, chkd );
            }
        }

        // disabliing a text box includes resetting  value to 1.0
        private void EnableValueTextBox( TextBox textBox, bool enable ) {
            textBox.Enabled = enable;
            if( enable == false ) {
                textBox.Text = "1.0";
            }
        }

        private void okButton_Click( object sender, EventArgs e ) {
            SaveChanges();
        }

        private void helpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, this.helpTag );
        }
    }
}
