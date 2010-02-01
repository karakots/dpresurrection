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
    /// Dialog for selecting worksheets in a file that has a worksheet-per-brand structure.  The data type is not specified
    /// in this dialog; it must be set somewhere else. Normally each file has a specific data type set by SetFileTypeForm.
    /// </summary>
    public partial class SelectBrandWorksheetsForm : Form, IWorksheetSelectingForm
    {
        private DataTable table;

        public DataTable SheetNamesTable {
            get {
                return table;
            }
        }

        public SelectBrandWorksheetsForm( string fileName, DataTable worksheetNamesTable ) {
            InitializeComponent();

            fileLabel.Text = fileName;

            this.table = worksheetNamesTable;
            dataGridView1.DataSource = worksheetNamesTable;
        }
    }
}