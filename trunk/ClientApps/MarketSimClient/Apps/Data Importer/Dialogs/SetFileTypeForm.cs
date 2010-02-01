using System;
using System.Collections;
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
    /// <summary>
    /// Dialog for specifying the data type for a file that contains only one type of data.  This type of file
    /// uses the worksheet names to represent a parameter other than type (for instance brands).
    /// </summary>
    public partial class SetFileTypeForm : Form
    {
        public ProjectSettings.DataType DataType {
            get {
                switch( (string)comboBox1.SelectedItem ) {
                    case "Display":
                        return ProjectSettings.DataType.Display;
                    case "Distribution":
                        return ProjectSettings.DataType.Distribution;
                    case "Media":
                        return ProjectSettings.DataType.Media;
                    case "Price (Absolute)":
                        return ProjectSettings.DataType.PriceAbsolute;
                    case "Price (Promo)":
                        return ProjectSettings.DataType.PricePromo;
                    case "Price (Regular)":
                        return ProjectSettings.DataType.PriceRegular;
                    case "Price (% Promo)":
                        return ProjectSettings.DataType.PromoPricePct;
                    case "Price (% Absolute)":
                        return ProjectSettings.DataType.AbsolutePricePct;
                    case "Real Sales":
                        return ProjectSettings.DataType.RealSales;
                }
                return ProjectSettings.DataType.Unknown;      // should never get here
            }
        }

        public SetFileTypeForm( string fileName ) {
            InitializeComponent();

            fileLabel.Text = fileName;
            comboBox1.SelectedIndex = 0;

            if( fileName.ToLower().IndexOf( "media" ) != -1 ) {
                comboBox1.SelectedItem = "Media";
            }
            else if( fileName.ToLower().IndexOf( "display" ) != -1 ) {
                comboBox1.SelectedItem = "Display";
            }
            else if( fileName.ToLower().IndexOf( "distribution" ) != -1 ) {
                comboBox1.SelectedItem = "Distribution";
            }
        }
    }
}