using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Results
{
    public partial class GroupingDialog : Form
    {
        public GroupingDialog(string initial_name)
        {
            InitializeComponent();

            GroupNameBox.Text = initial_name;
        }

        public String GroupName
        {
            get
            {
                return GroupNameBox.Text;
            }
        }
    }
}