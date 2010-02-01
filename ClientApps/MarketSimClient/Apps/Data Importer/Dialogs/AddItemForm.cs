using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataImporter.Dialogs
{
    public partial class AddItemForm : Form
    {
        public string ItemValue {
            get { return this.textBox1.Text.Trim(); }
       }

        public AddItemForm() {
            InitializeComponent();
        }
    }
}