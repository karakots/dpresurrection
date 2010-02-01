using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DataImporter;
using DataImporter.Library;
using DataImporter.ImportSettings;

namespace DataImporter.Dialogs
{
    public partial class VariantNamingAidForm : Form
    {
        private ProjectSettings project;
        private ArrayList allMarketSimNames;
        private ArrayList inputNames;

        private string[] inNames;
        private string[] outNames;

        //private ProjectSettings.ProductInfo selectedInfo;

        public VariantNamingAidForm( ProjectSettings project ) {
            InitializeComponent();
            this.project = project;
            allMarketSimNames = new ArrayList();
            inputNames = new ArrayList();

            UpdateInputUI();
        }

         private void loadNamesButton_Click( object sender, EventArgs e ) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files (*.xls)|*.xls";
            ofd.Title = "Select MarketSim Names File";
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = project.InputRootDirectory;
            DialogResult resp = ofd.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            ExcelWriter2 namesReader = new ExcelWriter2();
            namesReader.Open( ofd.FileName, "Products" );
            allMarketSimNames = new ArrayList();
            int row = 2;
            int col = 3;
            do {
                object obj = namesReader.GetValue( row, col );
                if( obj as string == null ) {
                    break;
                }
                string nStr = (string)obj;
                bool addedAlready = false;
                foreach( string added in allMarketSimNames ) {
                    if( added == nStr ) {
                        addedAlready = true;
                        break;
                    }
                }
                if( addedAlready == false ) {
                    allMarketSimNames.Add( nStr );
                }
                row += 1;
            }
            while( true );
            namesReader.Kill();

            UpdateOutputUI();
        }

        private void UpdateOutputUI() {
            outputListBox.BeginUpdate();
            outputListBox.Items.Clear();

            string[] names = new string[ allMarketSimNames.Count ];
            allMarketSimNames.CopyTo( names );
            Array.Sort( names );
            for( int i = 0; i < names.Length; i++ ) {
                outputListBox.Items.Add( names[ i ] );
            }
            outputListBox.EndUpdate();
        }

        private void UpdateInputUI() {
            inputListBox.BeginUpdate();
            inputListBox.Items.Clear();

            inNames = new string[ project.Products.Count ];
            outNames = new string[ project.Products.Count ];
            for( int i = 0; i < project.Products.Count; i++ ) {
                inNames[ i ] = ((ProjectSettings.ProductInfo)project.Products[ i ]).ImportName;
                outNames[ i ] = ((ProjectSettings.ProductInfo)project.Products[ i ]).MarketSimName;
            }
            Array.Sort( inNames, outNames );
            for( int i = 0; i < inNames.Length; i++ ) {
                string inName = inNames[ i ];
                inName = AddExtraInfo( inName );
                inputListBox.Items.Add( inName );
            }
            inputListBox.EndUpdate();
        }

        private void okButton_Click( object sender, EventArgs e ) {
            bool changed = false;
            for( int i = 0; i < inNames.Length; i++ ) {
                ProjectSettings.ProductInfo info = project.GetProduct( inNames[ i ] );
                if( info.MarketSimName != outNames[ i ] ) {
                    project.GetProduct( inNames[ i ] ).MarketSimName = outNames[ i ];
                    changed = true;
                }
            }
            if( changed ) {
                project.SetEdited();
            }
        }

        private void inputListBox_SelectedIndexChanged( object sender, EventArgs e ) {
            outputListBox.SelectedIndexChanged -= new EventHandler(outputListBox_SelectedIndexChanged);
            marketSimNameTextBox.TextChanged -= new EventHandler( marketSimNameTextBox_TextChanged );
            outputListBox.SelectedIndex = -1;
            if( inputListBox.SelectedIndex >= 0 ) {
                string selItem = (string)inputListBox.SelectedItem;
                if( selItem.IndexOf( "[" ) != -1 ) {
                    selItem = selItem.Substring( 0, selItem.IndexOf( "[" ) ).Trim();        // remove any additional info in [] brackets
                }
                string[] inWords = selItem.Split( ' ' );

                outputListBox.Items.Clear();
                outputListBox.BeginUpdate();
                if( showAllCheckBox.Checked == false ) {
                    for( int i = 0; i < allMarketSimNames.Count; i++ ) {
                        string msname = (string)allMarketSimNames[ i ];
                        for( int j = 0; j < inWords.Length; j++ ) {
                            if( Char.IsLetter( inWords[ j ][ 0 ] ) ) {
                                if( msname.IndexOf( inWords[ j ] ) != -1 ) {
                                    outputListBox.Items.Add( msname );
                                    break;
                                }
                            }
                        }
                    }
                }
                else {
                    for( int i = 0; i < allMarketSimNames.Count; i++ ) {
                        string msname = (string)allMarketSimNames[ i ];
                        outputListBox.Items.Add( msname );
                    }
               }
                outputListBox.EndUpdate();
   
                marketSimNameTextBox.Text = outNames[ inputListBox.SelectedIndex ];
                for( int i = 0; i < outputListBox.Items.Count; i++ ){
                    if( (string)outputListBox.Items[ i ] == marketSimNameTextBox.Text ) {
                        outputListBox.SelectedIndex = i;
                        break;
                    }
                }
            }
            else {
                marketSimNameTextBox.Text = "";
            }
            marketSimNameTextBox.TextChanged += new EventHandler( marketSimNameTextBox_TextChanged );
            outputListBox.SelectedIndexChanged += new EventHandler( outputListBox_SelectedIndexChanged );
        }

        private void marketSimNameTextBox_TextChanged( object sender, EventArgs e ) {
            if( inputListBox.SelectedIndex >= 0 ) {
                outNames[ inputListBox.SelectedIndex ] = marketSimNameTextBox.Text;
            }
        }

        private void outputListBox_SelectedIndexChanged( object sender, EventArgs e ) {
            if( outputListBox.SelectedIndex >= 0 ) {
                marketSimNameTextBox.Text = (string)outputListBox.SelectedItem;
            }
        }

        private string AddExtraInfo( string info ) {

            string extra = "";
            for( int i = 0; i < info.Length; i++ ) {
                char c = info[ i ];
                if( Char.IsWhiteSpace( c ) || Char.IsPunctuation( c ) || Char.IsLetterOrDigit( c ) ){
                    continue;
                }
                // a bad char!!
                int charVal = (int)c;

                if( extra.Length == 0 ) {
                    extra += "        [";
                }

                // see if these are the special numeral-in-a-circle-chars
                if( charVal >= 9312 && charVal <= 9342 ) {
                    int num = charVal - 9311;
                    extra += String.Format( "  < {0} >  ", num );
                }
                else {
                    extra += String.Format( "{0:d}", charVal );
                }
                extra += ", ";
            }
            string s = info;
            if( extra.Length > 0 ) {
                extra = extra.Substring( 0, extra.Length - 2 ) + "]";
                s += extra;
            }
            return s;
        }

        private void showAllCheckBox_CheckedChanged( object sender, EventArgs e ) {
            UpdateOutputUI();
        }

        private void autoMatchButton_Click_1( object sender, EventArgs e ) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files (*.xls)|*.xls";
            ofd.Title = "Select Name List Pair File";
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = project.InputRootDirectory;
            DialogResult resp = ofd.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            ExcelWriter2 namesReader = new ExcelWriter2();
            namesReader.Open( ofd.FileName, "Products" );
            allMarketSimNames = new ArrayList();
            int row = 2;
            int col = 1;
            do {
                object obj = namesReader.GetValue( row, col );
                object obj2 = namesReader.GetValue( row, col + 1 );
                if( obj as string == null || obj2 as string == null ) {
                    // hit the end of the list
                    break;
                }
                string nStr = (string)obj;
                string nStr2 = (string)obj2;
                for( int i = 0; i < inputListBox.Items.Count; i++ ) {
                    string inp = (string)inputListBox.Items[ i ];
                    if( inp == nStr || inp.EndsWith( nStr ) ) {
                        // we found a matching item in the input list
                        outNames[ i ] = nStr2;
                    }
                }
                row += 1;
            }
            while( true );
            namesReader.Kill();

            UpdateOutputUI();
        }

        private void addButton_Click( object sender, EventArgs e ) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files (*.xls)|*.xls";
            ofd.Title = "Select Name List Pair File";
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = project.InputRootDirectory;
            DialogResult resp = ofd.ShowDialog();
            if( resp != DialogResult.OK ) {
                return;
            }

            ExcelWriter2 namesReader = new ExcelWriter2();
            namesReader.Open( ofd.FileName, "Products" );
            allMarketSimNames = new ArrayList();
            int row = 2;
            int col = 1;
            do {
                object obj = namesReader.GetValue( row, col );
                object obj2 = namesReader.GetValue( row, col + 1 );
                if( obj as string == null || obj2 as string == null ) {
                    // hit the end of the list
                    break;
                }
                string nStr = (string)obj;
                string nStr2 = (string)obj2;

                if( project.GetProduct( nStr ) == null ) {
                    project.AddProduct( nStr, nStr2 );
                }

                row += 1;
            }
            while( true );
            namesReader.Kill();

            UpdateOutputUI();
        }
    }
}