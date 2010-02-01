using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

using DataImporter.ImportSettings;

namespace DataImporter.Dialogs
{
    public partial class FileCombinerForm : Form
    {
        private ProjectSettings.ProjectSection currentSection;
        private ArrayList correspondingFilesList;
        private string projectRoot;

        private string DentsuRulesName = "Dentsu Price from Yen and Volume Sales";
        private string defaultSource2Dir = @"C:\Documents and Settings\jim\My Documents\DecisionPower\Dentsu Project\Sales Volume";
        private string defaultOutputDir = @"C:\Documents and Settings\jim\My Documents\DecisionPower\Dentsu Project\Computed Price Data";

        public ArrayList FilesList {
            get {
                return correspondingFilesList;
            }
        }

        public int CombiningRuleIndex {
            get {
                return ruleComboBox.SelectedIndex;
            }
        }

        public FileCombinerForm( string projectFilePath,  ProjectSettings.ProjectSection currentSection ) {
            InitializeComponent();

            this.projectRoot = projectFilePath.Substring( 0, projectFilePath.LastIndexOf( "\\" ) );
            this.currentSection = currentSection;

            sourceLabel.Text = currentSection.FleSetFolder;
            ruleComboBox.Items.Clear();
            ruleComboBox.Items.Add( DentsuRulesName );
            ruleComboBox.SelectedItem = DentsuRulesName;

            src2TextBox.Text = defaultSource2Dir;
            outTextBox.Text = defaultOutputDir;
        }

        private void browseButton_Click( object sender, EventArgs e ) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyDocuments;
            fbd.Description = "Select a folder containing a similarly-structured set of files as those in the source folder.";
            DialogResult resp = fbd.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
            src2TextBox.Text = fbd.SelectedPath;
            src2TextBox.SelectAll();
        }

        private void browseOutButton_Click( object sender, EventArgs e ) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyDocuments;
            fbd.Description = "Select the folder for the computed result files.";
            fbd.ShowNewFolderButton = true;
            DialogResult resp = fbd.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }
            outTextBox.Text = fbd.SelectedPath;
            outTextBox.SelectAll();
        }

        private void okButton_Click( object sender, EventArgs e ) {

            string src1Dir = currentSection.FleSetFolder;
            string[] allSourceFiles1 = Directory.GetFiles( src1Dir, DataImporter.InputFilePattern, SearchOption.AllDirectories );
            string[] excludeFilesContaining = new string[] { "Total", "All Channels" };

            // check secondary sources
            if( Directory.Exists( src2TextBox.Text ) == false ) {
                MessageBox.Show( "\r\n    Error: The specified output folder does not exist.    \r\n", "Directory Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Error );
                this.DialogResult = DialogResult.None;
                return;
            }
            string src2Dir = src2TextBox.Text;
            string[] allSourceFiles2 = Directory.GetFiles( src2Dir, DataImporter.InputFilePattern, SearchOption.AllDirectories );

            ArrayList sourceFilesList1 = new ArrayList();
            ArrayList sourceFilesList2 = new ArrayList();
            foreach( string f1 in allSourceFiles1 ) {
                if( f1.IndexOf( "Total" ) == -1 && f1.IndexOf( "All Channels" ) == -1 ) {
                    sourceFilesList1.Add( projectRoot + "\\" + f1 );
                }
            }
            foreach( string f2 in allSourceFiles2 ) {
                if( f2.IndexOf( "Total" ) == -1 && f2.IndexOf( "All Channels" ) == -1 ) {
                    sourceFilesList2.Add( f2 );
                }
            }
            string[] sourceFiles1 = new string[ sourceFilesList1.Count ];
            string[] sourceFiles2 = new string[ sourceFilesList2.Count ];
            sourceFilesList1.CopyTo( sourceFiles1 );
            sourceFilesList2.CopyTo( sourceFiles2 );

            bool[] sourceFile1Matched = new bool[ sourceFiles1.Length ];
            int[] sourceFile2List1Indexes = new int[ sourceFiles2.Length ];

            for( int i = 0; i < sourceFiles1.Length; i++ ) {
                sourceFile1Matched[ i ] = false;
            }
            for( int i = 0; i < sourceFiles2.Length; i++ ) {
                sourceFile2List1Indexes[ i ] = -1;
            }

            // generate the matched pair list of files
            for( int i1 = 0; i1 < sourceFiles1.Length; i1++ ) {
                for( int i2 = 0; i2 < sourceFiles2.Length; i2++ ) {
                    if( sourceFile2List1Indexes[ i2 ] == -1 ) {
                        // see if the files correspond
                        if( FilesCorrespond( sourceFiles1[ i1 ].Substring( currentSection.FleSetFolder.Length + 1 ),
                                                   sourceFiles2[ i2 ].Substring( src2Dir.Length + 1 ), 
                                                   (string)ruleComboBox.SelectedItem ) ) {
                            sourceFile2List1Indexes[ i2 ] = i1;
                            sourceFile1Matched[ i1 ] = true;
                            break;   // break the s2 loop since we've found the matching item
                        }
                    }
                }
            }

            int unmatched1 = 0;
            foreach( bool isMatched in sourceFile1Matched ) {
                if( isMatched == false ) {
                    unmatched1 += 1;
                }
            }
            int unmatched2 = 0;
            foreach( int matchIndex in sourceFile2List1Indexes ) {
                if( matchIndex == -1 ) {
                    unmatched2 += 1;
                }
            }

            if( unmatched1 != 0 || unmatched2 != 0 ) {
                string msg = String.Format( "\r\n    Error: The two sets of input files did not match.     \r\n\r\n    Unused Source 1 Files: {0}    \r\n    " +
                    "    Unmatched Source 2 Files: {1}", unmatched1, unmatched2 );


                Console.WriteLine( "Unmatched Files:\n\n" );
                for( int i = 0; i < sourceFile1Matched.Length; i++ ) {
                    if( sourceFile1Matched[ i ] == false && (sourceFiles1[ i ].IndexOf( "Total" ) == -1) && (sourceFiles1[ i ].IndexOf( "All Channels" ) == -1 ) ) {
                        Console.WriteLine( sourceFiles1[ i ] );
                    }
                }

                MessageBox.Show( msg, "Invalid Inputs", MessageBoxButtons.OK, MessageBoxIcon.Error );
                this.DialogResult = DialogResult.None;
                return;
            }

            if( Directory.Exists( outTextBox.Text ) == false ) {
                Directory.CreateDirectory( outTextBox.Text );
            }

            string outDir = outTextBox.Text;

            correspondingFilesList = new ArrayList();
            for( int i2 = 0; i2 < sourceFiles2.Length; i2++ ){
                string[] data = new string[ 3 ];
                string inFile = sourceFiles1[ sourceFile2List1Indexes[ i2 ] ];
                data[ 0 ] = inFile;
                data[ 1 ] = sourceFiles2[ i2 ];
                data[ 2 ] = OutputFileFor( inFile.Substring( currentSection.FleSetFolder.Length + 1 ), outDir );
                correspondingFilesList.Add( data );
            }
            this.DialogResult = DialogResult.OK;
        }

        private string OutputFileFor( string inFileRelPath, string outFolder ) {
            string outFile = outFolder + "\\" + inFileRelPath;
            return outFile;
        }

        private bool FilesCorrespond( string path1, string path2, string ruleID ) {
            if( ruleID != DentsuRulesName ) {
                string msg  = String.Format(  "\r\n    Error: Unknown combining rule: \"{0}\"     \r\n", ruleID );
                MessageBox.Show( msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return false;
            }

            path1 = path1.Replace( "Souken", "Soken" );

            string dir1 = "";
            string file1 = "";
            string dir2 = "";
            string file2 = "";

            if( path1.IndexOf( "\\" ) != -1 ) {
                file1 = path1.Substring( path1.LastIndexOf( "\\" ) + 1 );
                path1 = path1.Substring( 0, path1.LastIndexOf( "\\" ) );

                if( path1.IndexOf( "\\" ) != -1 ) {
                    dir1 = path1.Substring( path1.LastIndexOf( "\\" ) + 1 );
                }
                else {
                    dir1 = path1;
                }
            }
            else {
                file1 = path1;
                dir1 = null;
            }

            if( path2.IndexOf( "\\" ) != -1 ) {
                file2 = path2.Substring( path2.LastIndexOf( "\\" ) + 1 );
                path2 = path2.Substring( 0, path2.LastIndexOf( "\\" ) );

                if( path2.IndexOf( "\\" ) != -1 ) {
                    dir2 = path2.Substring( path2.LastIndexOf( "\\" ) + 1 );
                }
                else {
                    dir2 = path2;
                }
            }
            else {
                file2 = path2;
                dir2 = null;
            }

            if( (dir1 == dir2 || dir1 == null || dir2 == null) && file1 == file2 ) {
                // an exact match!
                return true;
            }

            // check correspondences for Dentsu situation

            string dsfx = " Sales Volume Data";      // volume dirs end with "" Sales Volume Data"
            if( dir2.EndsWith( dsfx ) ) {
                dir2 = dir2.Substring( 0, dir2.Length - dsfx.Length );
            }

            file1 = file1.Replace( ". ", "." );       // volume dirs don't have a space after the . in the number 

            // volume dirs use different channel notation
            file2 = file2.Replace( "(CVS)", "CVS" );  
            file2 = file2.Replace( "(GMS)", "GMS" );
            file2 = file2.Replace( "(SM-S)", "SMS" );
            file2 = file2.Replace( "(SM-L)", "SML" );
            file2 = file2.Replace( "(Mini-SM)", "MSM" );
            file2 = file2.Replace( " with Oriental Style", "" );
            file2 = file2.Replace( "6-1", "6" );

            file1 = file1.Replace( "Suntoru", "Suntory" );
            file1 = file1.Replace( "SM-L", "SML" );
            file1 = file1.Replace( "SM-S", "SMS" );
            file1 = file1.Replace( "Mini-SM", "MSM" );
            file1 = file1.Replace( "Mini SM", "MSM" );

            if( dir2.IndexOf( "6-2" ) != -1 ) {
                dir2 = dir2.Replace( " Series", "" );
            }
            if( file1.IndexOf( "17-4" ) != -1 ) {
                file1 = file1.Replace( "Rokujyo", "Rokujyo Mugicha" );
            }
            file1 = file1.Replace( "2-3 ", "2-3." );
            file1 = file1.Replace( " ", "" );
            file2 = file2.Replace( " ", "" ); 

            // check again
            if( dir1 == dir2 ) {
                if( file1 == file2 ) {
                    return true;
                }
                // perhaps the src2 name has a leadiung zero
                if( file2.StartsWith( "0" ) ) {
                    file2 = file2.Substring( 1 );
                }

                if( file1 == file2 ) {
                    return true;
                }
            }

            return false;
        }
    }
}