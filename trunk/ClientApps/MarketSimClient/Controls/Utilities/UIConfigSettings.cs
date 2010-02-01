using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Utilities
{
    public class UIConfigSettings
    {
        public class Colors
        {
            public static Color DPBlue = Color.FromArgb( 0, 105, 170 );
            public static Color DPGreen = Color.FromArgb( 174, 18, 40 );

            public static Color LightDPBlue = Color.FromArgb( 228, 239, 246 );
            public static Color LightDPGreen = Color.FromArgb( 232, 236, 194 );

            public static Color DarkDPBlue = Color.FromArgb( 0, 55, 89 );
            public static Color DarkDPGreen = Color.FromArgb( 137, 148, 32 );

            public static Color OrangeAccent = Color.FromArgb( 239, 118, 0 );
            public static Color DarkOrangeAccent = Color.FromArgb( 189, 86, 0 );

            //public static Color GreenFadeStart = Color.FromArgb( 232, 236, 194 );
            public static Color GreenFadeStart = Color.FromArgb( 242, 246, 194 );
            public static Color BlueFadeStart = Color.FromArgb( 195, 239, 235 );

            public static Color PopupMenuFadeStart = Color.White;
            //public static Color PopupMenuFadeEnd = Color.FromArgb( 244, 244, 244 );
 //           public static Color PopupMenuFadeEnd = Color.FromArgb( 228, 239, 246 );
            public static Color PopupMenuFadeEnd = Color.FromArgb( 242, 247, 251 );
            //public static Color PopupMenuFadeEnd = Color.FromArgb( 239, 118, 0 );

            public static Color PopupMenuLinkColor = Color.Blue;
            public static Color PopupMenuDisabledLinkColor = Color.Gray;

            public static Color MainNavigatorPanelColor = LightDPBlue;

            public static Color EditDialoglColor = Color.FromArgb( 200, 220, 200 );
        }

        public class Fonts
        {
            public static Font PopupMenuLinkFont = new Font( "Arlal", 9, FontStyle.Bold );
            public static Font PopupMenuItemFont = new Font( "Arlal", 8 );
            public static Font NavPaneTitleFont = new Font( "Arlal", 12, FontStyle.Bold );
        }

        public const float PopupMenuMinimumWidth = 95;

        ////public static void PaintFadeBackground1( Control c, Graphics g ){
        ////    Rectangle r1 = new Rectangle( 0, 0, c.Bounds.Width, c.Bounds.Height / 2 );
        ////    Rectangle r2 = new Rectangle( 0, r1.Height - 1, c.Bounds.Width, 1 + c.Bounds.Height / 2 );

        ////    Brush gbrush1 = new LinearGradientBrush( r1,
        ////                Colors.GreenFadeStart, Color.White, LinearGradientMode.Vertical );
        ////    Brush gbrush2 = new LinearGradientBrush( r2,
        ////                Color.White, Colors.GreenFadeStart, LinearGradientMode.Vertical );

        ////    g.FillRectangle( gbrush1, r1 );
        ////    g.FillRectangle( gbrush2, r2 );

            
        ////}
    }
}
