using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DataImporter.Library;

namespace DataImporter.Dialogs
{
    /// <summary>
    /// Dialog for selecting worksheets in a file where the worksheet names represent data types, as is
    /// the case for Nitro data files.
    /// </summary>
    public partial class SelectWorksheetsForm : Form, IWorksheetSelectingForm
    {
        private DataTable table;

        public DataTable SheetNamesTable {
            get {
                return table;
            }
        }

        public SelectWorksheetsForm( string fileName, DataTable worksheetNamesTable ) {
            InitializeComponent();

            fileLabel.Text = fileName;

            this.table = worksheetNamesTable;
            dataGridView1.DataSource = worksheetNamesTable;
        }
    }
}