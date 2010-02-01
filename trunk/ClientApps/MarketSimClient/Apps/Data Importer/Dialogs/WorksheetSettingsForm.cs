using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DataImporter.Library;
using DataImporter.ImportSettings;

namespace DataImporter.Dialogs
{
    public partial class WorksheetSettingsForm : Form
    {
        private WorksheetSettings worksheetSettings;

        private bool dateHeaderValid = false;
        private bool skuHeaderValid = false;
        private bool setIntervalrValid = false;
        //private bool chanHeaderValid = false;

        private XPoint datePoint;
        private XPoint skuPoint;
        private XPoint chanPoint;
        private string dateHeader;
        private string skuHeader;
        private string chanHeader;
        private TimeSpan timeStep;
        private int setTimeStepDays;

        public WorksheetSettings WorksheetSettings {
            get { return worksheetSettings; }
        }

        public WorksheetSettingsForm( string filePath, WorksheetSettings existingSettings ) {
            InitializeComponent();

            this.worksheetSettings = existingSettings;
            this.sheetNameLabel.Text = worksheetSettings.SheetName;
            this.datePoint = this.worksheetSettings.FirstDateHeaderCell;
            this.skuPoint = this.worksheetSettings.FirstSkuHeaderCell;
            this.chanPoint = this.worksheetSettings.FirstChanHeaderCell;

            UpdateUI();
        }

        private void UpdateUI() {
            this.dateHeadingLabel.Text = worksheetSettings.FirstDateHeader;
            this.dhRowTextBox.TextChanged -= new EventHandler( dateRowColTextBox_TextChanged );
            this.dhColTextBox.TextChanged -= new EventHandler( dateRowColTextBox_TextChanged );

            this.dhRowTextBox.Text = this.worksheetSettings.FirstDateHeaderCell.Row.ToString();
            this.dhColTextBox.Text = this.worksheetSettings.FirstDateHeaderCell.Col.ToString();

            this.dhRowTextBox.TextChanged += new EventHandler( dateRowColTextBox_TextChanged );
            this.dhColTextBox.TextChanged += new EventHandler( dateRowColTextBox_TextChanged );

            if( this.worksheetSettings.DatesAreIntervalEnd ) {
                startEndComboBox.SelectedItem = "end";
            }
            else {
                startEndComboBox.SelectedItem = "start";
            }

            this.hvComboBox.SelectedIndexChanged -= new EventHandler( dateRowColTextBox_TextChanged );
            if( this.worksheetSettings.Horizontal ) {
                hvComboBox.SelectedItem = "Horizontal";
            }
            else {
                hvComboBox.SelectedItem = "Vertical";
            }
            this.hvComboBox.SelectedIndexChanged += new EventHandler( dateRowColTextBox_TextChanged );

            this.setStepCountTextBox.TextChanged -=new EventHandler(setStepCountTextBox_TextChanged);
            this.setStepTypeComboBox.SelectedIndexChanged -= new EventHandler( setStepCountTextBox_TextChanged );

            if( this.worksheetSettings.TimeStep.Days > 0 ) {
                this.setStepTypeComboBox.SelectedItem = "days";
                this.setStepCountTextBox.Text = this.worksheetSettings.TimeStep.Days.ToString();
            }
            else {
                this.setStepTypeComboBox.SelectedItem = "month(s)";
                int nmonths = -this.worksheetSettings.TimeStep.Days;
                this.setStepCountTextBox.Text = nmonths.ToString();
            }
            this.setStepCountTextBox.TextChanged += new EventHandler( setStepCountTextBox_TextChanged );
            this.setStepTypeComboBox.SelectedIndexChanged += new EventHandler( setStepCountTextBox_TextChanged );

            this.timeStepLabel.Text = TimeSpanString( worksheetSettings.TimeStep );

            // display the first SKU cell
            this.skuRowTextBox.TextChanged -= new EventHandler( skuRowColTextBox_TextChanged );
            this.skuColTextBox.TextChanged -= new EventHandler( skuRowColTextBox_TextChanged );

            this.skuRowTextBox.Text = this.worksheetSettings.FirstSkuHeaderCell.Row.ToString();
            this.skuColTextBox.Text = this.worksheetSettings.FirstSkuHeaderCell.Col.ToString();

            this.skuRowTextBox.TextChanged += new EventHandler( skuRowColTextBox_TextChanged );
            this.skuColTextBox.TextChanged += new EventHandler( skuRowColTextBox_TextChanged );

            this.skuHeadingLabel.Text = worksheetSettings.FirstSkuHeader;


            // display the first Channel cell
            this.chanRowTextBox.TextChanged -= new EventHandler( chanRowColTextBox_TextChanged );
            this.chanColTextBox.TextChanged -= new EventHandler( chanRowColTextBox_TextChanged );

            this.chanRowTextBox.Text = this.worksheetSettings.FirstChanHeaderCell.Row.ToString();
            this.chanColTextBox.Text = this.worksheetSettings.FirstChanHeaderCell.Col.ToString();

            this.chanRowTextBox.TextChanged += new EventHandler( chanRowColTextBox_TextChanged );
            this.chanColTextBox.TextChanged += new EventHandler( chanRowColTextBox_TextChanged );

            this.chanHeadingLabel.Text = worksheetSettings.FirstChanHeader;

            string isNitro = null;
            if( worksheetSettings.Validated ) {
                isNitro = CheckForNitroSettings();
            }

            errorLabel.Visible = !worksheetSettings.Validated;
            errorLabel.ForeColor = Color.Red;
            errorLabel.BackColor = Color.White;
            if( worksheetSettings.Validated && isNitro != null ) {
                errorLabel.Visible = true;
                errorLabel.BackColor = Color.Yellow;
                errorLabel.ForeColor = Color.Green;
                errorLabel.Text = isNitro;
            }
        }

        private string CheckForNitroSettings() {
            string isNitro = null;
            string fdh = worksheetSettings.FirstDateHeader.ToLower();
            if( fdh.StartsWith( "week ending" ) || fdh.StartsWith( "week starting" ) || fdh.StartsWith( "4 weeks ending" ) || fdh.StartsWith( "4 weeks starting" ) ) {
                isNitro = "NITRO Format";
            }

            if( this.worksheetSettings.Horizontal == false ) {
                isNitro = null;
            }

            if( isNitro != null ) {
                if( this.worksheetSettings.FirstSkuHeaderCell.Row == 4 && this.worksheetSettings.FirstSkuHeaderCell.Col == 2 &&
                    this.worksheetSettings.FirstDateHeaderCell.Row == 2 && this.worksheetSettings.FirstDateHeaderCell.Col == 3 ) {
                    isNitro = "Standard NITRO Format";

                    if( this.worksheetSettings.FirstChanHeaderCell.Row == 3 && this.worksheetSettings.FirstChanHeaderCell.Col == 1 ) {
                        isNitro = "Std NITRO Format w/Chans";
                    }
                }
            }
            return isNitro;
        }

        private void skuRowColTextBox_TextChanged( object sender, EventArgs e ) {
            int r = -1;
            int c = -1;
            string cellStr = null;
            try {
                r = int.Parse( skuRowTextBox.Text.Trim() );
                c = int.Parse( skuColTextBox.Text.Trim() );
                object cellObj = ScanManager.Reader.GetValue( r, c );
                if( cellObj is string ) {
                    cellStr = (string)cellObj;
                    if( cellStr.Trim().Length == 0 ) {
                        cellStr = null;
                    }
                }
            }
            catch( Exception ) {
            }

            this.worksheetSettings.FirstSkuHeaderCell = new XPoint( r, c );
            if( cellStr != null ) {
                this.skuHeaderValid = true;
                this.skuHeadingLabel.Text = cellStr;
                this.skuHeadingLabel.ForeColor = Color.Black;
                this.errorLabel.Visible = false;

                this.skuHeader = cellStr;
                this.skuPoint = new XPoint( r, c );
            }
            else {
                this.skuHeaderValid = false;
                this.skuHeadingLabel.Text = "???";
                this.skuHeadingLabel.ForeColor = Color.Red;
                this.errorLabel.Visible = true;

                this.skuHeader = null;
                this.skuPoint = new XPoint( -1, -1 );
            }
        }

        private void chanRowColTextBox_TextChanged( object sender, EventArgs e ) {
            int r = -1;
            int c = -1;
            string cellStr = null;
            try {
                r = int.Parse( chanRowTextBox.Text.Trim() );
                c = int.Parse( chanColTextBox.Text.Trim() );
                object cellObj = ScanManager.Reader.GetValue( r, c );
                if( cellObj is string ) {
                    cellStr = (string)cellObj;
                    if( cellStr.Trim().Length == 0 ) {
                        cellStr = null;
                    }
                }
            }
            catch( Exception ) {
            }

            this.worksheetSettings.FirstChanHeaderCell = new XPoint( r, c );
            if( cellStr != null ) {
                //this.chanHeaderValid = true;
                this.chanHeadingLabel.Text = cellStr;
                this.chanHeadingLabel.ForeColor = Color.Black;
                this.errorLabel.Visible = false;

                this.chanHeader = cellStr;
                this.chanPoint = new XPoint( r, c );
            }
            else {
                //this.chanHeaderValid = false;
                this.chanHeadingLabel.Text = "???";
                this.chanHeadingLabel.ForeColor = Color.Red;
                this.errorLabel.Visible = true;

                this.chanHeader = null;
                this.chanPoint = new XPoint( -1, -1 );
            }
        }

        private void dateRowColTextBox_TextChanged( object sender, EventArgs e ) {
            int r = -1;
            int c = -1;
            string cellStr = null;
            DateTime date = DateTime.MinValue;
            DateTime nextDate = DateTime.MinValue;
            timeStep = new TimeSpan();
            bool valid = false;

            try {
                r = int.Parse( dhRowTextBox.Text.Trim() );
                c = int.Parse( dhColTextBox.Text.Trim() );
                int nextR = r;
                int nextC = c;
                object cellObj = ScanManager.Reader.GetValue( r, c );
                if( WorksheetScanner.ContainsDate( cellObj, out date ) ) {

                    cellStr = (string)cellObj;

                    if( (string)hvComboBox.SelectedItem == "Horizontal" ) {
                        nextC = c + 1;
                    }
                    else {
                        nextR = r + 1;
                    }

                    object nextCellObj = ScanManager.Reader.GetValue( nextR, nextC );
                    if( WorksheetScanner.ContainsDate( nextCellObj, out nextDate ) ) {
                        timeStep = nextDate - date;
                        valid = true;
                    }
                }
            }
            catch( Exception ) {
            }

            if( valid ) {
                this.dateHeadingLabel.Text = cellStr;
                this.dateHeadingLabel.ForeColor = Color.Black;
                this.timeStepLabel.Text = TimeSpanString( timeStep );
                this.timeStepLabel.ForeColor = Color.Black;
                this.errorLabel.Visible = false;

                this.dateHeader = cellStr;
                this.datePoint = new XPoint( r, c );

                if( (string)this.hvComboBox.SelectedItem == "Horizontal" ) {
                    if( this.skuPoint.Row <= this.datePoint.Row ) {
                        this.skuPoint = new XPoint( this.datePoint.Row + 1, this.skuPoint.Col );
                        this.skuRowTextBox.Text = this.skuPoint.Row.ToString();
                    }
                }
                else {
                }
            }
            else {
                if( cellStr == null ) {
                    this.dateHeadingLabel.Text = "???";
                    this.dateHeadingLabel.ForeColor = Color.Red;
                }
                this.timeStepLabel.Text = "???";
                this.timeStepLabel.ForeColor = Color.Red;
                this.errorLabel.Visible = true;
            }
            this.dateHeaderValid = valid;
        }

        private string TimeSpanString( TimeSpan span ) {
            int nDays = (int)span.TotalDays;
            string timeStepString = null;
            if( nDays % 365 == 0 ) {
                if( nDays == 365 ) {
                    timeStepString = "1 year";
                }
                else {
                    timeStepString = String.Format( "{0} years", nDays / 365 );
                }
            }
            else if( nDays % 7 == 0 ) {
                if( nDays == 7 ) {
                    timeStepString = "1 week";
                }
                else {
                    timeStepString = String.Format( "{0} weeks", nDays / 7 );
                }
            }
            else {
                if( nDays == 1 ) {
                    timeStepString = "1 day";
                }
                else {
                    timeStepString = String.Format( "{0} days", nDays );
                }
            }
            return timeStepString;
        }

        private void okButton_Click( object sender, EventArgs e ) {

            dateRowColTextBox_TextChanged( null, null );
            skuRowColTextBox_TextChanged( null, null );

            if( this.skuHeaderValid && this.dateHeaderValid ) {
                worksheetSettings.FirstDateHeaderCell = this.datePoint;
                worksheetSettings.FirstSkuHeaderCell = this.skuPoint;
                worksheetSettings.FirstDateHeader = this.dateHeader;
                worksheetSettings.FirstSkuHeader = this.skuHeader;
                worksheetSettings.ScanAllDateHeaders = this.scanAllDatesCheckBox.Checked;

                if( (string)hvComboBox.SelectedItem == "Horizontal" ) {
                    worksheetSettings.Horizontal = true;
                }
                else {
                    worksheetSettings.Horizontal = false;
                }

                if( (string)startEndComboBox.SelectedItem == "start" ) {
                    worksheetSettings.DatesAreIntervalEnd = false;
                }
                else {
                    worksheetSettings.DatesAreIntervalEnd = true;
                }

                if( worksheetSettings.ScanAllDateHeaders ) {
                    worksheetSettings.TimeStepDays = this.setTimeStepDays;
                }
                else {
                    worksheetSettings.TimeStepDays = (int)timeStep.TotalDays;
                }
            }
        }

        private void scanAllDatesCheckBox_CheckedChanged( object sender, EventArgs e ) {
            if( scanAllDatesCheckBox.Checked ) {
                timeStepLabel.Visible = false;
                setStepCountTextBox.Visible = true;
                setStepTypeComboBox.Visible = true;
            }
            else {
                timeStepLabel.Visible = true;
                setStepCountTextBox.Visible = false;
                setStepTypeComboBox.Visible = false;
            }
        }

        private void setStepCountTextBox_TextChanged( object sender, EventArgs e ) {
            try {
                int n = int.Parse( setStepCountTextBox.Text.Trim() );
                string stype = (string)setStepTypeComboBox.SelectedItem;
                if( stype == "days" ) {
                    this.setTimeStepDays = n;
                }
                else {
                    // the only other choice is months -- since a month isn't a set interval, we encode them using negative days = months span
                    this.setTimeStepDays = -n;
                }
                setStepCountTextBox.ForeColor = Color.Black;
                setStepTypeComboBox.ForeColor = Color.Black;
                this.setIntervalrValid = true;
            }
            catch( Exception ) {
                setStepCountTextBox.ForeColor = Color.Red;
                setStepTypeComboBox.ForeColor = Color.Red;
                this.setIntervalrValid = false;
            }
        }
    }
}