#define ZEDGRAPH
using System;

using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Common.Dialogs;
using Common;

using ModelView;
using ModelView.Dialogs;

using MarketSimUtilities;
using MrktSimDb;
using Utilities;
using Utilities.Graphing;

namespace MrktSimClient.Controls.MarketPlans
{
    /// <summary>
    /// A control for showing a generic graph of market plan data.  This class acts as the interface to the rest of the
    /// application and contains the actual graphing control in the graphPanel.  
    /// </summary>
    class MarketPlanGraphControl : MrktSimControl
    {
        private Panel controlPanel;
        private PlotControl plotControl1;
        private Splitter splitter1;
        private ListBox legendListBox;
        private Panel graphPanel;
        private Button moveToBackButton;
        private CheckBox fillCheckBox;
        private NumericUpDown curveScale;
        private Label label12;
        private Label label1;
        private Panel panel1;
        private Panel panel2;
        private Panel colorPanel;
        private Button colorButton;
        private PictureBox pictureBox1;
        private ArrayList dataCurves;

        public PlotControl Plot {
            get {
                return this.plotControl1;
            }
        }

        public Button Send {
            get {
                return this.plotControl1.Send;
            }
        }

        public string GraphTitle {
            set {
                plotControl1.Title = value;
            }

            get
            {
                return plotControl1.Title;
            }
        }

        public string XAxisTitle {
            set {
                plotControl1.XAxis = value;
            }
        }

        public string YAxisTitle {
            set {
                plotControl1.YAxis = value;
            }
        }

        public DateTime Start {
            set {
                plotControl1.Start = value;
            }
        }

        public DateTime End {
            set {
                plotControl1.End = value;
            }
        }

        public double Max {
            set {
                plotControl1.Max = value;
            }
        }

        public void AddCurve( DataCurve dataCurve ) {
            plotControl1.Data = dataCurve;
            dataCurves.Add( dataCurve );
            legendListBox.Items.Add( dataCurve.Label );
            if( legendListBox.Items.Count == 1 ) {
                legendListBox.SelectedIndex = 0;        // select the first item by default
            }
            plotControl1.Refresh();
        }

        /// <summary>
        /// Removes common heads (up to a ':' ) and tails (in [] ) from multiple legend strings
        /// </summary>
        public void CondenseLegends() {
            if( legendListBox.Items.Count < 2 ) {
                return;
            }

            string head = null;
            bool headsAllSame = true;
            string tail = null;
            bool tailsAllSame = true;

            foreach( string legendItem in legendListBox.Items ) {
                // check the head of the string (ends with a colon)
                if( legendItem.IndexOf( ":" ) != -1 ) {
                    string thisHead = legendItem.Substring( 0, legendItem.IndexOf( ":" ) );
                    if( head == null ) {
                        head = thisHead;
                    }
                    else {
                        if( thisHead != head ){
                            headsAllSame = false;
                        }
                    }
                }

                // check the end (channel name) of the string (in square brackets)
                if( legendItem.IndexOf( "[" ) != -1 ) {
                    string thisTail = legendItem.Substring( legendItem.IndexOf( "[" ) );
                    if( tail == null ) {
                        tail = thisTail;
                    }
                    else {
                        if( thisTail != tail ) {
                            tailsAllSame = false;
                        }
                    }
                }
            }

            // remove common heads and/or tails of the legends
            for( int i = 0; i < legendListBox.Items.Count; i++ ) {
                string s = (string)legendListBox.Items[ i ];
                if( headsAllSame == true ) {
                    s = s.Substring( head.Length + 1 ).Trim();
                }
                if( tailsAllSame == true ) {
                    s = s.Substring( 0, s.Length - tail.Length ).Trim();
                }
                legendListBox.Items[ i ] = s;
            }
        }

        public void ClearCurves() {
            plotControl1.ClearAllGraphs();
            legendListBox.Items.Clear();
            DisableCurveModificationControls();
            dataCurves = new ArrayList();
        }


        /// <summary>
        /// Creates a new MarketPlanScenarioPicker object.
        /// </summary>
        public MarketPlanGraphControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            plotControl1.Font = new Font( "Arial", 9 );
            plotControl1.CurveSelected += new PlotControl.SelectedCurve( plotControl1_CurveSelected );
            dataCurves = new ArrayList();
        }

        public void DataChanged()
        {
            plotControl1.DataChanged();
            ////plotControl1.Scale();
            plotControl1.Refresh();
        }

        public void plotControl1_CurveSelected( int selectedIndex ) {
            legendListBox.SelectedIndex = selectedIndex;
        }

        		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MarketPlanGraphControl ) );
            this.graphPanel = new System.Windows.Forms.Panel();
            this.plotControl1 = new Utilities.Graphing.PlotControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.colorPanel = new System.Windows.Forms.Panel();
            this.colorButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.curveScale = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.moveToBackButton = new System.Windows.Forms.Button();
            this.fillCheckBox = new System.Windows.Forms.CheckBox();
            this.legendListBox = new System.Windows.Forms.ListBox();
            this.graphPanel.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.curveScale)).BeginInit();
            this.SuspendLayout();
            // 
            // graphPanel
            // 
            this.graphPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.graphPanel.BackColor = System.Drawing.Color.LightGray;
            this.graphPanel.Controls.Add( this.plotControl1 );
            this.graphPanel.Controls.Add( this.splitter1 );
            this.graphPanel.Controls.Add( this.controlPanel );
            this.graphPanel.Location = new System.Drawing.Point( 5, 5 );
            this.graphPanel.Name = "graphPanel";
            this.graphPanel.Size = new System.Drawing.Size( 682, 290 );
            this.graphPanel.TabIndex = 0;
            // 
            // plotControl1
            // 
            this.plotControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotControl1.End = new System.DateTime( 1899, 12, 31, 4, 48, 0, 0 );
            this.plotControl1.Location = new System.Drawing.Point( 0, 0 );
            this.plotControl1.Max = 1.2;
            this.plotControl1.MaxX = 1.2;
            this.plotControl1.Min = 0;
            this.plotControl1.MinimumSize = new System.Drawing.Size( 200, 100 );
            this.plotControl1.MinX = 0;
            this.plotControl1.Name = "plotControl1";
            this.plotControl1.PercentMax = 100;
            this.plotControl1.PercentMin = 0;
            this.plotControl1.ScatterPlot = false;
            this.plotControl1.Size = new System.Drawing.Size( 682, 222 );
            this.plotControl1.Start = new System.DateTime( 1899, 12, 30, 0, 0, 0, 0 );
            this.plotControl1.TabIndex = 2;
            this.plotControl1.TimeSeries = true;
            this.plotControl1.Title = "Title";
            this.plotControl1.XAxis = "Date";
            this.plotControl1.YAxis = "Y-Axis";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point( 0, 222 );
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size( 682, 3 );
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(54)))), ((int)(((byte)(137)))), ((int)(((byte)(188)))) );
            this.controlPanel.Controls.Add( this.panel2 );
            this.controlPanel.Controls.Add( this.panel1 );
            this.controlPanel.Controls.Add( this.curveScale );
            this.controlPanel.Controls.Add( this.label1 );
            this.controlPanel.Controls.Add( this.label12 );
            this.controlPanel.Controls.Add( this.moveToBackButton );
            this.controlPanel.Controls.Add( this.fillCheckBox );
            this.controlPanel.Controls.Add( this.legendListBox );
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlPanel.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.controlPanel.Location = new System.Drawing.Point( 0, 225 );
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size( 682, 65 );
            this.controlPanel.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add( this.colorPanel );
            this.panel2.Controls.Add( this.colorButton );
            this.panel2.Location = new System.Drawing.Point( 312, 4 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size( 57, 60 );
            this.panel2.TabIndex = 46;
            // 
            // colorPanel
            // 
            this.colorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPanel.BackColor = System.Drawing.Color.White;
            this.colorPanel.Enabled = false;
            this.colorPanel.Location = new System.Drawing.Point( 6, 5 );
            this.colorPanel.Name = "colorPanel";
            this.colorPanel.Size = new System.Drawing.Size( 45, 26 );
            this.colorPanel.TabIndex = 45;
            this.colorPanel.Click += new System.EventHandler( this.colorPanel_Click );
            // 
            // colorButton
            // 
            this.colorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.colorButton.BackColor = System.Drawing.Color.White;
            this.colorButton.Enabled = false;
            this.colorButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.colorButton.Location = new System.Drawing.Point( 6, 35 );
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size( 45, 23 );
            this.colorButton.TabIndex = 44;
            this.colorButton.Text = "Color";
            this.colorButton.UseVisualStyleBackColor = false;
            this.colorButton.Click += new System.EventHandler( this.colorButton_Click );
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add( this.pictureBox1 );
            this.panel1.Location = new System.Drawing.Point( 2, 6 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size( 55, 55 );
            this.panel1.TabIndex = 45;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject( "pictureBox1.Image" )));
            this.pictureBox1.Location = new System.Drawing.Point( 3, 4 );
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size( 48, 48 );
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // curveScale
            // 
            this.curveScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.curveScale.Enabled = false;
            this.curveScale.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.curveScale.Location = new System.Drawing.Point( 182, 8 );
            this.curveScale.Maximum = new decimal( new int[] {
            9,
            0,
            0,
            0} );
            this.curveScale.Minimum = new decimal( new int[] {
            9,
            0,
            0,
            -2147483648} );
            this.curveScale.Name = "curveScale";
            this.curveScale.Size = new System.Drawing.Size( 40, 20 );
            this.curveScale.TabIndex = 41;
            this.curveScale.ThousandsSeparator = true;
            this.curveScale.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.curveScale.ValueChanged += new System.EventHandler( this.curveScale_ValueChanged );
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label1.Location = new System.Drawing.Point( 157, 16 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 48, 16 );
            this.label1.TabIndex = 44;
            this.label1.Text = "10^";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.label12.Location = new System.Drawing.Point( 122, 16 );
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size( 48, 16 );
            this.label12.TabIndex = 40;
            this.label12.Text = "Scale";
            // 
            // moveToBackButton
            // 
            this.moveToBackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveToBackButton.BackColor = System.Drawing.Color.White;
            this.moveToBackButton.Enabled = false;
            this.moveToBackButton.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.moveToBackButton.Location = new System.Drawing.Point( 122, 38 );
            this.moveToBackButton.Name = "moveToBackButton";
            this.moveToBackButton.Size = new System.Drawing.Size( 100, 21 );
            this.moveToBackButton.TabIndex = 2;
            this.moveToBackButton.Text = "Move to Back";
            this.moveToBackButton.UseVisualStyleBackColor = false;
            this.moveToBackButton.Click += new System.EventHandler( this.moveToBackButton_Click );
            // 
            // fillCheckBox
            // 
            this.fillCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fillCheckBox.AutoSize = true;
            this.fillCheckBox.Enabled = false;
            this.fillCheckBox.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.fillCheckBox.Location = new System.Drawing.Point( 255, 24 );
            this.fillCheckBox.Name = "fillCheckBox";
            this.fillCheckBox.Size = new System.Drawing.Size( 41, 18 );
            this.fillCheckBox.TabIndex = 1;
            this.fillCheckBox.Text = "Fill";
            this.fillCheckBox.UseVisualStyleBackColor = true;
            this.fillCheckBox.CheckedChanged += new System.EventHandler( this.fillCheckBox_CheckedChanged );
            // 
            // legendListBox
            // 
            this.legendListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.legendListBox.Font = new System.Drawing.Font( "Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)) );
            this.legendListBox.FormattingEnabled = true;
            this.legendListBox.ItemHeight = 14;
            this.legendListBox.Items.AddRange( new object[] {
            "Curve Legend 1",
            "Curve Legend 2",
            "Curve Legend 3",
            "Curve Legend 4",
            "Curve Legend 5"} );
            this.legendListBox.Location = new System.Drawing.Point( 368, 4 );
            this.legendListBox.Name = "legendListBox";
            this.legendListBox.ScrollAlwaysVisible = true;
            this.legendListBox.Size = new System.Drawing.Size( 311, 60 );
            this.legendListBox.TabIndex = 0;
            this.legendListBox.SelectedIndexChanged += new System.EventHandler( this.legendListBox_SelectedIndexChanged );
            // 
            // MarketPlanGraphControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb( ((int)(((byte)(200)))), ((int)(((byte)(219)))), ((int)(((byte)(108)))) );
            this.Controls.Add( this.graphPanel );
            this.Margin = new System.Windows.Forms.Padding( 0 );
            this.Name = "MarketPlanGraphControl";
            this.Size = new System.Drawing.Size( 692, 300 );
            this.graphPanel.ResumeLayout( false );
            this.controlPanel.ResumeLayout( false );
            this.controlPanel.PerformLayout();
            this.panel2.ResumeLayout( false );
            this.panel1.ResumeLayout( false );
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.curveScale)).EndInit();
            this.ResumeLayout( false );

        }
                #endregion

        private void legendListBox_SelectedIndexChanged( object sender, EventArgs e ) {
            int selIndx = legendListBox.SelectedIndex;
            Console.WriteLine( "new index = {0}", selIndx );
            if( selIndx >= 0 && selIndx < this.plotControl1.Curves.Count ) {
                ZedGraph.LineItem lineItem = (ZedGraph.LineItem)this.plotControl1.Curves[ selIndx ];
                fillCheckBox.Checked = (lineItem.Line.Fill.Type != ZedGraph.FillType.None);
                fillCheckBox.Enabled = true;
                colorPanel.BackColor = lineItem.Line.Color;
                colorPanel.Enabled = true;
                colorButton.Enabled = true;
                curveScale.Value = plotControl1.CrvScale( selIndx );
                curveScale.Enabled = true;
                moveToBackButton.Enabled = true;

                //JimJ work in progress...!!!
                //DataCurve dc = (DataCurve)this.dataCurves[ selIndx ];
                //plotControl1.YAxis = dc.Units;
            }
            else {
                // no curve selected
                DisableCurveModificationControls();
            }

            plotControl1.Refresh();
        }

        private void DisableCurveModificationControls() {
            fillCheckBox.Checked = false;
            fillCheckBox.Enabled = false;
            colorPanel.BackColor = Color.White;
            colorPanel.Enabled = false;
            colorButton.Enabled = false;
            curveScale.Value = 0;
            curveScale.Enabled = false;
            moveToBackButton.Enabled = false;
        }

        private void moveToBackButton_Click( object sender, EventArgs e ) {
            int selIndx = legendListBox.SelectedIndex;
            if( selIndx < 0 ) {
                return;
            }
            ZedGraph.LineItem lineItem = (ZedGraph.LineItem)this.plotControl1.Curves[ selIndx ];
            DataCurve bdc = (DataCurve)dataCurves[ selIndx ];
            plotControl1.PushToBack( lineItem );
            dataCurves.RemoveAt( selIndx );
            dataCurves.Add( bdc );
            plotControl1.Refresh();
            legendListBox.SelectedIndexChanged -=new EventHandler( legendListBox_SelectedIndexChanged );
            string item = (string)legendListBox.Items[ legendListBox.SelectedIndex ];
            legendListBox.Items.RemoveAt( legendListBox.SelectedIndex );
            legendListBox.Items.Add( item );
            legendListBox.SelectedIndex = legendListBox.Items.Count - 1;
            legendListBox.SelectedIndexChanged +=new EventHandler( legendListBox_SelectedIndexChanged );
        }

        private void curveScale_ValueChanged( object sender, EventArgs e ) {
            int selIndx = legendListBox.SelectedIndex;
            if( selIndx < 0 ) {
                return;
            }
            plotControl1.ScaleCrv( selIndx, (int)curveScale.Value );
            plotControl1.Refresh();
        }

        private void fillCheckBox_CheckedChanged( object sender, EventArgs e ) {
            int selIndx = legendListBox.SelectedIndex;
            Console.WriteLine( "FILL index = {0}", selIndx );
            if( selIndx < 0 ) {
                return;
            }
            ZedGraph.LineItem lineItem = (ZedGraph.LineItem)this.plotControl1.Curves[ selIndx ];
            if( fillCheckBox.Checked == true ) {
                lineItem.Line.Fill = new ZedGraph.Fill( Color.White, lineItem.Color, 45 );
            }
            else {
                lineItem.Line.Fill.Type = ZedGraph.FillType.None;
            }

            plotControl1.Refresh();
        }

        private void colorPanel_Click( object sender, EventArgs e ) {
            SetCurrentCurveColor();
        }

        private void colorButton_Click( object sender, EventArgs e ) {
            SetCurrentCurveColor();
        }

        private void SetCurrentCurveColor() {
            int selIndx = legendListBox.SelectedIndex;
            if( selIndx < 0 ) {
                return;
            }
            ZedGraph.LineItem lineItem = (ZedGraph.LineItem)this.plotControl1.Curves[ selIndx ];
            ColorDialog cdg = new ColorDialog();
            cdg.Color = lineItem.Line.Color;
            DialogResult resp = cdg.ShowDialog();
            if( resp == DialogResult.OK ) {
                colorPanel.BackColor = cdg.Color;
                lineItem.Line.Color = cdg.Color;
                plotControl1.Refresh();
            }
        }
    }
}
