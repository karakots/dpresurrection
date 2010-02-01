using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HouseholdManager
{
    public partial class Settings : Form
    {
        #region private members
        private DirectoryInfo rawdata_dir;
        private DirectoryInfo hhinput_dir;
        private DirectoryInfo output_dir;

        #endregion

        #region public properties
        public DirectoryInfo RawDataDirectory
        {
            get
            {
                return rawdata_dir;
            }
            set
            {
                rawdata_dir = value;
                if (rawdata_dir != null && rawdata_dir.Exists)
                {
                    rawdata_path.Text = rawdata_dir.FullName;
                    if (hhinput_dir != null && rawdata_dir != null && output_path != null)
                    {
                        OKButton.Enabled = true;
                    }
                }
            }
        }

        public DirectoryInfo HHInputDirectory
        {
            get
            {
                return hhinput_dir;
            }
            set
            {
                hhinput_dir = value;

                if (hhinput_dir != null && hhinput_dir.Exists)
                {
                    hhinput_path.Text = hhinput_dir.FullName;
                    if (hhinput_dir != null && rawdata_dir != null  && output_path != null)
                    {
                        OKButton.Enabled = true;
                    }
                }
            }
        }

        public DirectoryInfo OutputDirectory
        {
            get
            {
                return output_dir;
            }
            set
            {
                output_dir = value;

                if (output_dir != null && output_dir.Exists)
                {
                    output_path.Text = output_dir.FullName;
                    if (hhinput_dir != null && rawdata_dir != null  && output_path != null)
                    {
                        OKButton.Enabled = true;
                    }
                }
            }
        }
        #endregion

        #region initialization
        public Settings()
        {
            InitializeComponent();
        }
        public Settings( DirectoryInfo rawdata_dir, DirectoryInfo hh_dir, DirectoryInfo output_dir )
        {
            InitializeComponent();

            RawDataDirectory = rawdata_dir;
            HHInputDirectory = hh_dir;
            OutputDirectory = output_dir;
        }
        #endregion

        #region UI_Controls
        private void HHBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browse = new FolderBrowserDialog();
            browse.ShowNewFolderButton = false;

            if (browse.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            RawDataDirectory = new DirectoryInfo(browse.SelectedPath);
        }

        private void MediaBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browse = new FolderBrowserDialog();
            browse.ShowNewFolderButton = false;

            if (browse.ShowDialog() != DialogResult.OK)
            {
                return;
            }
        }
        #endregion

        private void AgentBrowse_Click( object sender, EventArgs e )
        {

            FolderBrowserDialog browse = new FolderBrowserDialog();
            browse.ShowNewFolderButton = false;

            if( browse.ShowDialog() != DialogResult.OK )
            {
                return;
            }

            HHInputDirectory = new DirectoryInfo( browse.SelectedPath );
        }

        private void OutputBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browse = new FolderBrowserDialog();
            browse.ShowNewFolderButton = false;

            if (browse.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            OutputDirectory = new DirectoryInfo(browse.SelectedPath);
        }



    }
}
