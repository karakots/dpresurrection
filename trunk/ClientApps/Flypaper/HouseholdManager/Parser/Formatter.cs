using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Parser
{
    public partial class Formatter : Form
    {
        public Formatter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            FileName.Text = dialog.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader(FileName.Text);

            StreamWriter writer = new StreamWriter(FileName.Text.Insert(FileName.Text.Length-4,"_parsed"));

            List<String> top_lines = new List<string>();

            for (int i = 0; i < TopUpDown.Value; i++)
            {
                top_lines.Add(reader.ReadLine());
            }

            List<List<string>> top_values = new List<List<string>>();

            foreach (String line in top_lines)
            {
                top_values.Add(new List<string>(line.Split('\t')));
            }

            while (!reader.EndOfStream)
            {
                String[] values = reader.ReadLine().Split('\t');

                string header = "";

                int i = 0;
                for (; i < LeftUpDown.Value; i++)
                {
                    header += values[i] + "\t";
                }

                for (; i < values.Length; i++)
                {
                    string output = "";
                    foreach (List<string> tops in top_values)
                    {
                        output += tops[i] + "\t";
                    }
                    if (values[i].Length == 0)
                    {
                        values[i] = "0";
                    }
                    if (checkBox1.Checked && Double.Parse(values[i]) == 0)
                    {
                        continue;
                    }
                    output += values[i];
                    writer.WriteLine(header + output);
                }
            }


            writer.Close();

            reader.Close();

            MessageBox.Show("Done!");
        }
    }
}
