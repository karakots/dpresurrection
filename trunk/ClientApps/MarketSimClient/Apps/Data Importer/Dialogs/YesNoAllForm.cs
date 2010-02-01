using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataImporter.Dialogs
{
    public partial class YesNoAllForm : Form
    {
        public YesNoAllForm( string info, string title ) {
            InitializeComponent();

            infoLabel.Text = info;
            this.Text = title;
        }
    }
}