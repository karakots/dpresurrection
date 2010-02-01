using System;

using System.Drawing;

using System.Collections;

using System.ComponentModel;

using System.Windows.Forms;

using Utilities.Graphing;

using ZedGraph;



namespace Results

{

	/// <summary>

	/// Summary description for Grapher.

	/// </summary>

	public class Grapher : System.Windows.Forms.Form

	{

		public DataCurve Data

		{

			set

			{

				LineItem myCurve = zedGraphControl.GraphPane.AddCurve( value.Label, value.X, value.Y, value.Color, SymbolType.None);



				if (value.Units != null && value.Units.Length > 0)

					YAxis = value.Units;

			}

		}



		public double Start

		{

			set

			{

				zedGraphControl.GraphPane.XAxis.Min = value;

			}

		}



		public double End

		{

			set

			{

				zedGraphControl.GraphPane.XAxis.Max = value;

			}

		}



		public void AutoScaleVals()

		{

		}



		public string Title

		{

			set

			{

				zedGraphControl.GraphPane.Title = value;

				titleBox.Text = value;

			}

		}



		public string YAxis

		{

			set

			{

				zedGraphControl.GraphPane.YAxis.Title = value;

				yAxisBox.Text = value;



			}

		}



		public string XAxis

		{

			set

			{

				zedGraphControl.GraphPane.XAxis.Title = value;

			}



			get

			{

				return zedGraphControl.GraphPane.XAxis.Title;

			}

		}



		public void DataChanged()

		{

			zedGraphControl.AxisChange();

			zedGraphControl.Invalidate();

		}



		private ZedGraph.ZedGraphControl zedGraphControl;

		private System.Windows.Forms.GroupBox controlBox;

		private System.Windows.Forms.MainMenu mainMenu;

		private System.Windows.Forms.MenuItem menuItem1;

		private System.Windows.Forms.MenuItem imageItem;

		private System.Windows.Forms.MenuItem excelItem;

		private System.Windows.Forms.MenuItem csvItem;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.TextBox yAxisBox;

        private System.Windows.Forms.TextBox titleBox;
        private IContainer components;



		public Grapher()

		{

			//

			// Required for Windows Form Designer support

			//

			InitializeComponent();



			zedGraphControl.IsShowPointValues = true;

			

			zedGraphControl.GraphPane.XAxis.Type = AxisType.Linear;

			zedGraphControl.GraphPane.XAxis.Title = "";

//

//			symbolBox.Items.Add(SymbolType.None);

//			symbolBox.Items.Add(SymbolType.Circle);

//			symbolBox.Items.Add(SymbolType.Star);

//			symbolBox.Items.Add(SymbolType.Triangle);

//

//			symbolBox.SelectedIndex = 0;

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
            this.components = new System.ComponentModel.Container();
            this.controlBox = new System.Windows.Forms.GroupBox();
            this.titleBox = new System.Windows.Forms.TextBox();
            this.yAxisBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mainMenu = new System.Windows.Forms.MainMenu( this.components );
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.imageItem = new System.Windows.Forms.MenuItem();
            this.excelItem = new System.Windows.Forms.MenuItem();
            this.csvItem = new System.Windows.Forms.MenuItem();
            this.zedGraphControl = new ZedGraph.ZedGraphControl();
            this.controlBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlBox
            // 
            this.controlBox.Controls.Add( this.titleBox );
            this.controlBox.Controls.Add( this.yAxisBox );
            this.controlBox.Controls.Add( this.label2 );
            this.controlBox.Controls.Add( this.label1 );
            this.controlBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlBox.Location = new System.Drawing.Point( 0, 28 );
            this.controlBox.Name = "controlBox";
            this.controlBox.Size = new System.Drawing.Size( 671, 48 );
            this.controlBox.TabIndex = 1;
            this.controlBox.TabStop = false;
            // 
            // titleBox
            // 
            this.titleBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.titleBox.Location = new System.Drawing.Point( 344, 16 );
            this.titleBox.Name = "titleBox";
            this.titleBox.Size = new System.Drawing.Size( 240, 20 );
            this.titleBox.TabIndex = 6;
            this.titleBox.TextChanged += new System.EventHandler( this.titleBox_TextChanged );
            // 
            // yAxisBox
            // 
            this.yAxisBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.yAxisBox.Location = new System.Drawing.Point( 56, 16 );
            this.yAxisBox.Name = "yAxisBox";
            this.yAxisBox.Size = new System.Drawing.Size( 240, 20 );
            this.yAxisBox.TabIndex = 5;
            this.yAxisBox.TextChanged += new System.EventHandler( this.yAxisBox_TextChanged );
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point( 304, 16 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 32, 16 );
            this.label2.TabIndex = 4;
            this.label2.Text = "Title";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point( 8, 16 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 40, 16 );
            this.label1.TabIndex = 3;
            this.label1.Text = "Y-Axis";
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.menuItem1} );
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.imageItem,
            this.excelItem} );
            this.menuItem1.Text = "File";
            // 
            // imageItem
            // 
            this.imageItem.Index = 0;
            this.imageItem.Text = "Save Image";
            this.imageItem.Click += new System.EventHandler( this.imageItem_Click );
            // 
            // excelItem
            // 
            this.excelItem.Index = 1;
            this.excelItem.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.csvItem} );
            this.excelItem.Text = "export";
            // 
            // csvItem
            // 
            this.csvItem.Index = 0;
            this.csvItem.Text = "comma delimited format (csv)";
            this.csvItem.Click += new System.EventHandler( this.csvItem_Click );
            // 
            // zedGraphControl
            // 
            this.zedGraphControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl.IsShowPointValues = true;
            this.zedGraphControl.Location = new System.Drawing.Point( 0, 0 );
            this.zedGraphControl.Name = "zedGraphControl";
            this.zedGraphControl.PointValueFormat = "G";
            this.zedGraphControl.Size = new System.Drawing.Size( 671, 28 );
            this.zedGraphControl.TabIndex = 0;
            // 
            // Grapher
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.ClientSize = new System.Drawing.Size( 671, 76 );
            this.Controls.Add( this.zedGraphControl );
            this.Controls.Add( this.controlBox );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size( 608, 100 );
            this.Name = "Grapher";
            this.Text = "Grapher";
            this.controlBox.ResumeLayout( false );
            this.controlBox.PerformLayout();
            this.ResumeLayout( false );

		}

		#endregion



		private void csvItem_Click(object sender, System.EventArgs e)

		{

			const double largeFloat = 1000000000000;



			//System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			System.Windows.Forms.SaveFileDialog saveFileDlg = new SaveFileDialog();



			saveFileDlg.DefaultExt = ".csv";

			saveFileDlg.Filter = "CSV File (*.csv)|*.csv";



			saveFileDlg.CheckFileExists = false;

			//saveFileDlg.ReadOnlyChecked = false;



			DialogResult rslt = saveFileDlg.ShowDialog();



			if (rslt == DialogResult.OK)

			{

				string fileName = saveFileDlg.FileName;

				

				System.IO.StreamWriter writer;



				// any open erros are due to file being in use

				try

				{

					writer = new System.IO.StreamWriter(fileName);

				}

				catch(System.IO.IOException oops)

				{

					MessageBox.Show(oops.Message);

					return;

				}



				// write out header

				char comma = ',';

				string header = XAxis;



				foreach(CurveItem crv in zedGraphControl.GraphPane.CurveList )

				{

					header += crv.Label.Replace(",","-") + ",";

				}



				char[] trim = {comma};

				header.TrimEnd(trim);



				writer.WriteLine(header);



				int[] index = new int[zedGraphControl.GraphPane.CurveList.Count];

				double[] curYVal = new double[zedGraphControl.GraphPane.CurveList.Count];



				int ii;

				double xVal = largeFloat;



				for(ii = 0; ii < index.Length; ii++)

				{

					// initialize - probably done automatically - but what the hey

					index[ii] = 0;

					curYVal[ii] = 0.0;



					if (zedGraphControl.GraphPane.CurveList[ii].Points.Count == 0)

						continue;



					// find lowest value for curves

					double val = zedGraphControl.GraphPane.CurveList[ii].Points[0].X;



					if (val < xVal)

						xVal = val;

				}

					

				// check for all data written out

				bool done = false;

				while (!done)

				{

					string values = xVal.ToString() + comma;

					

					// check if there is still data to write

					done = true;

					

					// use this to track the xVal

				



					// write out current values

					for(ii = 0; ii < index.Length; ii++)

					{

						CurveItem crv = zedGraphControl.GraphPane.CurveList[ii];



						if (index[ii] < crv.Points.Count)

						{

							done = false;



							if (crv.Points[index[ii]].X <= xVal)

							{

								curYVal[ii] = crv.Points[index[ii]].Y;

								index[ii]++;

							}

						}

						else

						{

							// past end of crv set value to 0

							curYVal[ii] = 0.0;

						}



					

						values += curYVal[ii].ToString() + comma;

					}



					if (!done)

					{

						values.TrimEnd(trim);

						writer.WriteLine(values);

					



						// make sure curXVal is large enough

						double newXVal = largeFloat;

						for(ii = 0; ii < index.Length; ii++)

						{

							CurveItem crv = zedGraphControl.GraphPane.CurveList[ii];



							if (index[ii] < crv.Points.Count && crv.Points[index[ii]].X < newXVal)

							{

								newXVal = crv.Points[index[ii]].X ;

							}

						}



						xVal = newXVal;

					}

				}



				writer.Flush();

				writer.Close();

			}

		}



	



		private void titleBox_TextChanged(object sender, System.EventArgs e)

		{

			if (titleBox.Text.Length == 0)				

				zedGraphControl.GraphPane.Title = "   ";

			else

				zedGraphControl.GraphPane.Title = titleBox.Text;



			if (this.Visible)

				DataChanged();

		}



		private void yAxisBox_TextChanged(object sender, System.EventArgs e)

		{

			if (yAxisBox.Text.Length == 0)

				zedGraphControl.GraphPane.YAxis.Title = "   ";

			else

				zedGraphControl.GraphPane.YAxis.Title = yAxisBox.Text;



			if (this.Visible)

				DataChanged();

		}

	



		private void imageItem_Click(object sender, System.EventArgs e)

		{

			//System.Windows.Forms.OpenFileDialog openFileDlg = new OpenFileDialog();

			System.Windows.Forms.SaveFileDialog saveFileDlg = new SaveFileDialog();



			saveFileDlg.DefaultExt = ".bmp";

			saveFileDlg.Filter = "bitmap (*.bmp)|*.bmp|jpeg (*.jpg)|*.jpg";



			saveFileDlg.CheckFileExists = false;



			DialogResult rslt = saveFileDlg.ShowDialog();



			if (rslt == DialogResult.OK)

			{

				string fileName = saveFileDlg.FileName;



				System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Bmp;



				if (fileName.EndsWith(".jpg"))

					format = System.Drawing.Imaging.ImageFormat.Jpeg;



				Bitmap image = zedGraphControl.GraphPane.Image;



				image.Save(fileName, format);

			}

		}

	}

}

