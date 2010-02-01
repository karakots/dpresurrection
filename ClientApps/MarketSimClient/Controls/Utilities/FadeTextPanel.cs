using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Utilities
{
    public partial class FadeTextPanel : UserControl
    {
        private int styleNumber = 0;

        private class FadeStyle {
            public Color TopColor;
            public Color MiddleColor;
            public Color BottomColor;
            public double MiddleFraction;
            public int MiddleHeight;

            public FadeStyle( Color top, Color mid, Color bottom, double frac, int middleHeight ) {
                this.TopColor = top;
                this.MiddleColor = mid;
                this.BottomColor = bottom;
                this.MiddleFraction = frac;
                this.MiddleHeight = middleHeight;
            }
        }

        private static FadeStyle[] fadeStyles = new FadeStyle[] {
            new FadeStyle( UIConfigSettings.Colors.GreenFadeStart, Color.White, UIConfigSettings.Colors.BlueFadeStart, 0.3, -1 ),
            new FadeStyle( UIConfigSettings.Colors.GreenFadeStart, Color.White, UIConfigSettings.Colors.GreenFadeStart, 0.3, -1 ),
            new FadeStyle( UIConfigSettings.Colors.MainNavigatorPanelColor, Color.White, UIConfigSettings.Colors.MainNavigatorPanelColor, 0.4, 5 )
        };

        private FadeStyle fadeStyle;
        private string labelText = "?";

        public FadeTextPanel() {
            InitializeComponent();

            fadeStyle = fadeStyles[ 0 ];
        }

        [Category( "Appearance" ),
        DefaultValue( 0 ),
        Description( "The style selection value (0=green; 1=blue)." )]
        public int StyleNumber {
            set {
                this.styleNumber = value;
                fadeStyle = fadeStyles[ this.styleNumber ];
            }
            get { return this.styleNumber; }
        }

        /// <summary>
        /// Sets the text of this control
        /// </summary>
        [Category( "Appearance" ),
        Description( "Text to show in the control" )]
        public override string Text {
            set {
                this.labelText = value;
                if( this.labelText == null ) {
                    this.labelText = "?";
                }
                SetText( this.labelText ); 
            }
            get { return this.labelText; }
        }

        public override Font Font {
            set { label.Font = value; }
            get { return label.Font; }
        }

        /// <summary>
        /// Sets the displayed text to the given value, automatically inserting line breaks as necessary to fit into the control's width.
        /// </summary>
        /// <param name="txt"></param>
        private void SetText( string txt ) {
            string[] lines = txt.Split( '\r', '\n' );
            string outtxt = "";
            for( int i = 0; i < lines.Length; i++ ) {
                if( lines[ i ].Trim().Length == 0 ) {
                    continue;
                }
                if( i == 0 ) {
                    outtxt = FitLine( lines[ i ] );
                }
                else {
                    outtxt += "\r\n" + FitLine( lines[ i ] );
               }
            }
            this.label.Text = outtxt;
        }

        /// <summary>
        /// Breaks the given line of text into multiple lines that fit the width of the control.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private string FitLine( string txt ) {
            int maxw = this.Width - 28;
            Graphics g = Graphics.FromHwnd( this.Handle );
            SizeF txtsz = g.MeasureString( txt, label.Font );
            if( txtsz.Width <= maxw ) {
                return txt;
            }
            if( txt.Trim().IndexOf( ' ' ) == -1 ) {
                return txt;
            }

            // the string needs to be broken into multiple lines
            string[] words = txt.Split( ' ' );
            string line = "";
            string t = "";
            for( int i = 0; i < words.Length; i++ ) {
                string w = words[ i ].Trim();
                // the last word
                if( i == words.Length - 1 ) {
                    line += " " + w;
                    t += "\r\n" + line;
                    break;
                }

                if( line.Length == 0 ) {
                    line = w;
                }
                else {
                    line += " " + w;
                }

                // see if the next one would put us over
                string test = line + " " + words[ i + 1 ];
                SizeF sz = g.MeasureString( test, label.Font );
                if( sz.Width > maxw ) {
                    // we can't add the next one to the current line
                    if( t.Length == 0 ) {
                        t = line;
                    }
                    else {
                        t += "\r\n" + line;
                    }
                    line = "";
                }
            }
            return t;
        }

        /// <summary>
        /// Paints the fade background colors.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fadePanel_Paint( object sender, PaintEventArgs e ) {
            SetText( this.labelText );
            //Console.WriteLine( "PAINT label {0}", this.Size.ToString() );

            Rectangle paintRect = this.Bounds;
            paintRect = new Rectangle( this.Padding.Left, this.Padding.Top,
                paintRect.Width - this.Padding.Horizontal, paintRect.Height - this.Padding.Vertical );

            //int y1 = (int)Math.Round( this.Bounds.Height * this.fadeStyle.MiddleFraction );
            //int y2 = y1;

            //Rectangle rtop = new Rectangle( 0, 0, this.Bounds.Width, y1 );
            //Rectangle rtop2 = new Rectangle( 0, 0, this.Bounds.Width, y1 + 1 );
            //Rectangle rmid = new Rectangle( 0, y1 + 1, this.Bounds.Width, y2 - y1 );
            //Rectangle rbot = new Rectangle( 0, y2, this.Bounds.Width, this.Bounds.Height - y2 - 1 );
            //Rectangle rbot2 = new Rectangle( 0, y2 - 1, this.Bounds.Width, this.Bounds.Height - y2 + 2 );
            int effHeight = paintRect.Height;
            if( this.fadeStyle.MiddleHeight > 0 ) {
                effHeight -= this.fadeStyle.MiddleHeight;
            }
            int y1 = (int)Math.Round( effHeight * this.fadeStyle.MiddleFraction );
            int y2 = y1;
            int midHt = 0;
            if( this.fadeStyle.MiddleHeight > 0 ) {
                y2 += this.fadeStyle.MiddleHeight;
                midHt = this.fadeStyle.MiddleHeight;
            }

            Rectangle rtop = new Rectangle( paintRect.X, paintRect.Y, paintRect.Width, y1 );
            Rectangle rtop2 = new Rectangle( paintRect.X, paintRect.Y, paintRect.Width, y1 );
            Rectangle rmid = new Rectangle( paintRect.X, y1, paintRect.Width, y2 - y1 + 1 );
            Rectangle rbot = new Rectangle( paintRect.X, y2, paintRect.Width, paintRect.Height - y2 - midHt - 1 );
            Rectangle rbot2 = new Rectangle( paintRect.X, y2 - 1, paintRect.Width, paintRect.Height - y2 - midHt + 2 );

            Brush gbrush1 = new LinearGradientBrush( rtop2,
                        this.fadeStyle.TopColor, this.fadeStyle.MiddleColor, LinearGradientMode.Vertical );
            Brush midbrush = new SolidBrush( this.fadeStyle.MiddleColor );
            Brush gbrush2 = new LinearGradientBrush( rbot2,
                        this.fadeStyle.MiddleColor, this.fadeStyle.BottomColor, LinearGradientMode.Vertical );

            e.Graphics.FillRectangle( gbrush1, rtop );
            e.Graphics.FillRectangle( gbrush2, rbot );
            e.Graphics.FillRectangle( midbrush, rmid );
        }

        private void fadePanel_Resize( object sender, EventArgs e ) {
            this.fadePanel.Invalidate();
        }

        private void FadeTextPanel_Resize( object sender, EventArgs e ) {
            this.fadePanel.Invalidate();
        }
    }
}
