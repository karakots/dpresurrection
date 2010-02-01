using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Utilities;

namespace Common.Dialogs
{
    public partial class ImportNameAndDescr : Form
    {
        private OpenFileDialog fileDialog;
        private bool fileDialogInfoValid = false;

        private string specificItemName = null;
        private string helpTag = null;

        private string noNameMessage = "You must enter a name.";
        private string noNameMessageSpecif = "You must enter a name for the {0}.";
        private string illegalCharsMessage = "The name contains illegal characters.";
        private string illegalCharsMessageSpecif = "The {0} name contains illegal characters.";
        private string noFileMessage = "You must specify a file to import.";
        private string noFileMessageSpecif = "You must specify a {0} file to import.";
        private string noFileMultiMessage = "You must specify at least one file to import.";
        private string noFileMultiMessageSpecif = "You must specify at least one {0} file to import.";

        private ArrayList preselectedFiles = null;

        // for Visual Studio only
        public ImportNameAndDescr() {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a dialog for importing one or more Excel files.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="notes"></param>
        /// <param name="filePickerPath"></param>
        public ImportNameAndDescr( string title, string notes, string filePickerPath, bool multiSelect, Color color, string helpTag ) {
            InitializeComponent();

            this.titleLabel.Text = title;
            this.infoLabel.Text = notes;
            this.helpTag = helpTag;
            this.Color = color;

            fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = multiSelect;
            fileDialog.DefaultExt = ".xls";
            fileDialog.Filter = "Excel Files (*.xls)|*.xls";
            fileDialog.InitialDirectory = filePickerPath;

            if( multiSelect == false ) {
                this.fileLabel.Text = "Source File";
            }
            preselectedFiles = null;
        }

        public Color Color {
            set {
                this.BackColor = value;
                this.panel1.BackColor = value;
            }
        }

        public string SpecificItemName {
            set {
                this.specificItemName = value;
            }
        }

        /// <summary>
        /// Sets the main title text
        /// </summary>
        public string TitleText {
            set { this.titleLabel.Text = value; }
        }

        /// <summary>
        /// Sets the secondary (informational) text
        /// </summary>
        public string InfoText {
            set { this.infoLabel.Text = value; }
        }

        /// <summary>
        /// Sets the title for the Name textfield
        /// </summary>
        public string NameText {
            set { this.nameLabel.Text = value; }
        }

        /// <summary>
        /// Sets or gets the flag indicating that multiple files can be selected
        /// </summary>
        public bool MultiSelect {
            set { this.fileDialog.Multiselect = value; }
            get { return this.fileDialog.Multiselect; }
        }

        /// <summary>
        /// Returns the string in the File field if MultiSelect is false.  Returns null if MultiSelect is true.
        /// </summary>
        public string ObjDataFile {
            get {
                if( this.fileDialog.Multiselect == false ) {
                    return this.fileTextBox.Text.Trim();
                }
                else {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns the files selected by the file picker if MultiSelect is true.  Returns null if MultiSelect is false.
        /// </summary>
        public string[] ObjDataFiles {
            get {
                if( this.preselectedFiles == null ) {
                    if( this.fileDialog.Multiselect == true && this.fileDialogInfoValid == true ) {
                        return this.fileDialog.FileNames;
                    }
                    else {
                        return null;
                    }
                }
                else {
                    // the files were preselected
                    string[] files = new string[ preselectedFiles.Count ];
                    preselectedFiles.CopyTo( files );
                    return files;
                }
            }
        }

        /// <summary>
        /// Returns the value in the Name text field
        /// </summary>
        public string ObjName {
            get { return this.nameTextBox.Text.Trim(); }
        }

        /// <summary>
        /// Returns the value in the Descriiption text field
        /// </summary>
        public string ObjDescription {
            get { return this.descriptionTextBox.Text.Trim(); }
        }

        public void SetPreselectedFiles( ArrayList pathList ) {
            if( pathList != null) {
                this.preselectedFiles = pathList;

                this.browseButton.Visible = false;
                this.fileTextBox.Text = String.Format( "( {0} files selected )", pathList.Count );
                this.fileTextBox.Enabled = false;
                this.fileDialog.Multiselect = true;
                this.fileDialogInfoValid = true;
            }
        }

        /// <summary>
        /// Validates the entries of file (nonblank; exists) and name (nonblank; no illegal chars) fields.  Sets
        /// DialogResullt to OK if validation succeeds.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click( object sender, EventArgs e ) {

            if( this.fileTextBox.Text.Trim().Length == 0 ) {
                string msg = noFileMessage;
                if( this.specificItemName != null ) {
                    msg = String.Format( noFileMessageSpecif, this.specificItemName );
                }
                if( this.fileDialog.Multiselect == true ) {
                    msg = noFileMultiMessage;
                    if( this.specificItemName != null ) {
                        msg = String.Format( noFileMultiMessageSpecif, this.specificItemName );
                    }
                }

                ConfirmDialog cdlg = new ConfirmDialog( msg, "Source File Required" );
                cdlg.ShowDialog();
                return;
            }

            if( this.nameTextBox.Text.Trim().Length == 0 ) {
                string msg = noNameMessage;
                if( this.specificItemName != null ) {
                    msg = String.Format( noNameMessageSpecif, this.specificItemName );
                }
                ConfirmDialog cdlg = new ConfirmDialog( msg, "Name Required" );
                cdlg.ShowDialog();
                return;
            }

            // validate the name
            char[] illegal = { ',', '\'', '"', '.', ';' };
            int chkIndex = nameTextBox.Text.IndexOfAny( illegal );
            if( chkIndex >= 0 ) {
                string msg = illegalCharsMessage;
                if( this.specificItemName != null ) {
                    msg = String.Format( illegalCharsMessageSpecif, this.specificItemName );
                }
                ConfirmDialog cdlg = new ConfirmDialog( msg, "Illegal characters are:", " . , \' \" ;", "Name Invalid" );
                cdlg.SetOkButtonOnlyStyle();
                cdlg.ShowDialog();
                return;
            }

            if( this.fileDialogInfoValid == false ) {
                // validate the file
                if( File.Exists( fileTextBox.Text ) == false ) {
                    string msg = "\r\n         The specified data file does not exist.       \r\n\r\n       " + ObjDataFile + "\r\n\r\n";

                    MessageBox.Show( msg, "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                    return;
                }
            }

		    this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Shows a file picker for Excel files.  Fills the file field with the file path, and the name field with the name only.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseButton_Click( object sender, EventArgs e ) {

            DialogResult resp = fileDialog.ShowDialog();
            if( resp == DialogResult.OK ) {

                this.fileDialogInfoValid = true;

                fileTextBox.TextChanged -= new EventHandler( fileTextBox_TextChanged );
                if( fileDialog.Multiselect == false ) {
                    this.fileTextBox.Text = fileDialog.FileName;
                    this.fileTextBox.SelectAll();
                    this.nameTextBox.Text = BasicName( fileDialog.FileName, false );
                }
                else {
                    if( fileDialog.FileNames.Length == 1 ) {
                        this.fileTextBox.Text = fileDialog.FileNames[ 0 ];
                        this.nameTextBox.Text = BasicName( fileDialog.FileNames[ 0 ], false );
                    }
                    else if( fileDialog.FileNames.Length > 1 ) {
                        string info = String.Format( "({0} files selected) {1}, ...", fileDialog.FileNames.Length, BasicName( fileDialog.FileNames[ 0 ], true ) );
                        this.fileTextBox.Text = info;
                        this.nameTextBox.Text = BasicName( fileDialog.FileNames[ 0 ], false );
                    }
                    else {
                        // 0 files selected
                        this.fileTextBox.Text = "";
                        this.nameTextBox.Text = "";
                    }
                }
                fileTextBox.TextChanged += new EventHandler( fileTextBox_TextChanged );
            }
        }

        /// <summary>
        /// Returns the basic file name (no dir or extension) for the given file path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string BasicName( string path, bool includeExt ) {
            string theName = path;
            if( theName.IndexOf( "\\" ) != -1 ) {
                theName = theName.Substring( theName.LastIndexOf( "\\" ) + 1 );
            }
            if( includeExt == false ) {
                if( theName.IndexOf( "." ) != -1 ) {
                    theName = theName.Substring( 0, theName.LastIndexOf( "." ) );
                }
            }
            return theName;
        }

        private void fileTextBox_TextChanged( object sender, EventArgs e ) {
            this.fileDialogInfoValid = false;
        }

        private void helpButton_Click( object sender, EventArgs e ) {
            HelpManager.ShowHelp( this, this.helpTag );
        }
    }
}