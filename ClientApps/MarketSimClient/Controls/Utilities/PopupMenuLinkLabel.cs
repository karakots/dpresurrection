using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Utilities
{
    /// <summary>
    /// This control shows a LinkLabel which pops up a list of LinkLabels when the mouse is moved over it.
    /// </summary>
    public partial class PopupMenuLinkLabel : UserControl
    {
        /// <summary>
        /// Signature for callback method called when a popup menu item is clicked.
        /// </summary>
        public delegate void LinkClicked();

        public delegate void OnBeforeActivate();
        public OnBeforeActivate BeforeActivate;

        #region Member Variables
        private int leftMargin = 7;
        private int topMargin = 9;
        private int bottomMargin = 4;
        private int rightMargin = 5;
        private int tabMargin = 15;
        private int menuItemSpacing = 5;

        private int popupParentLevelsAbove = 2;              // the number of parent levels to account for when computing popup location
        private bool popUpToLeft = false;
        private bool popUpAbove = false;
        private int popupXAdjust = 0;

        private Color highlightColor = Color.LightSalmon;
        private Color textColor = SystemColors.ControlText;
        private Color popupBackColor = Color.White;
        private Font popupFont = UIConfigSettings.Fonts.PopupMenuItemFont;

        private Timer autoHideTimer;
        private int autoHideTimerInterval = 500;                  // the duration that a menu stays visible after the mouse leaves the menu
        private bool ignoreNextMouseLeave = false;              // true only after the user has clicked a main link to immediately dismiss a menu
        private Timer leaveCheckTimer;
        private bool mouseInControl;

        private Timer callbackTimer;                                 // fire a callback after a very short timer delay so the UI can stay caught up
        private LinkClicked activeCallback;

        private Panel popupMenuPanel;

        private ArrayList popupLinkLabels = new ArrayList();
        private ArrayList disableLinks = new ArrayList();
        private float minimumWidth = UIConfigSettings.PopupMenuMinimumWidth;

        private bool highlightOn;

        private static ArrayList allLinks = new ArrayList();

        #endregion

        #region Constructor
        private static PopupMenuLinkLabel ActiveMenuLink;

        /// <summary>
        /// Constructs a new PopupMenuLinkLabel control.
        /// </summary>
        public PopupMenuLinkLabel() {
            InitializeComponent();

            this.BackColor = Color.Transparent;

            this.autoHideTimer = new Timer();
            this.autoHideTimer.Tick += new System.EventHandler( this.AutoHideTimerCallback );
            this.autoHideTimer.Interval = autoHideTimerInterval;
            this.autoHideTimer.Tag = this;

            this.leaveCheckTimer = new Timer();
            this.leaveCheckTimer.Tick += new System.EventHandler( this.LeaveCheckTimerCallback );
            this.leaveCheckTimer.Interval = 100;

            allLinks.Add( this );
        }
        #endregion

        #region Properties
        /// <summary>
        /// Sets the panel that will be used to display the popup menu
        /// </summary>
        /// <remarks>This is often owned by a higher-level control so the popup is not clipped at the boundary of the direct owner</remarks>
        public Panel PopupMenuPanel {
            set {
                this.popupMenuPanel = value;
            }
        }

        /// <summary>
        /// Get or set a timer interval used for auto-closing the menu when the mouse is moved out of bounds.
        /// </summary>
        [Category( "Behavior" ),
         DefaultValue( 500 ),
         Description( "The timer interval in milliseconds for auto-closing a menu." )]
        public int AutoHideTimerInterval {
            get { return this.autoHideTimerInterval; }
            set { this.autoHideTimerInterval = value; }
        }

        /// <summary>
        /// Get or set an offset in pixels from the left side of the menu control for drawing the text
        /// of the first menu item.
        /// </summary>
        [Category( "Appearance" ),
         DefaultValue( 7 ),
         Description( "Specify the x-offset in pixels to begin drawing text for the first menu item." )]
        public int LeftMargin {
            get { return this.leftMargin; }
            set { this.leftMargin = value; }
        }

        /// <summary>
        /// Get or set x-offset in pixels to adjust the popup menu location.
        /// </summary>
        [Category( "Appearance" ),
         DefaultValue( 0 ),
         Description( "Specify the x-offset in pixels to adjust the popup menu location." )]
        public int PopupXAdjust {
            get { return this.popupXAdjust; }
            set { this.popupXAdjust = value; }
        }

        /// <summary>
        /// Get or set the space to put to the right of the longest menu item.
        /// </summary>
        [Category( "Appearance" ),
         DefaultValue( 7 ),
        Description( "Specify the space to put to the right of the longest menu item." )]
        public int RightMargin {
            get { return this.rightMargin; }
            set { this.rightMargin = value; }
        }

        /// <summary>
        /// Get or set an offset in pixels from the top of the menu control for drawing the text of the
        /// first menu item.
        /// </summary>
        [Category( "Appearance" ),
         DefaultValue( 3 ),
         Description( "Specify the y-offset in pixels to begin drawing text for the first menu item." )]
        public int TopMargin {
            get { return this.topMargin; }
            set { this.topMargin = value; }
        }

        /// <summary>
        /// Get or set the minimum amount the popup menu is wider than its triggering control.
        /// </summary>
        [Category( "Appearance" ),
         DefaultValue( 3 ),
         Description( "Specify the minimum amount the popup menu is wider than its triggering control." )]
        public int TabMargin {
            get { return this.tabMargin; }
            set { this.tabMargin = value; }
        }

        /// <summary>
        /// Get or set a value in pixels to be used for the spacing between two menu items.
        /// </summary>
        [Category( "Appearance" ),
         DefaultValue( 4 ),
         Description( "Specify the spacing between two menu items in pixels." )]
        public int MenuItemSpacing {
            get { return this.menuItemSpacing; }
            set { this.menuItemSpacing = value; }
        }

        /// <summary>
        /// Get or set the space to put below the last menu item.
        /// </summary>
        [Category( "Appearance" ),
         DefaultValue( 7 ),
        Description( "Specify the space to put below the last menu item." )]
        public int BottomMargin {
            get { return this.bottomMargin; }
            set { this.bottomMargin = value; }
        }
      
        /// <summary>
        /// Get or set the main link text
        /// </summary>
        [Category( "Appearance" ),
         Description( "Text for the main link." )]
        public string LinkText {
            get { return this.linkLabel.Text; }
            set { this.linkLabel.Text = value; }
        }

        /// <summary>
        /// Get or set the text color for a highlighted menu item.
        /// </summary>
        [Category( "Appearance" ),
         Description( "The background color for an activated menu." )]
        public Color HighlightColor {
            get { return this.highlightColor; }
            set { this.highlightColor = value; }
        }

        /// <summary>
        /// Get or set the background color of the popup menu.
        /// </summary>
        [Category( "Appearance" ),
         Description( "The background color for the popup menu." )]
        public Color PopupBackColor {
            get { return this.popupBackColor; }
            set { this.popupBackColor = value; }
        }

        /// <summary>
        /// Get or set the font for popup menu items.
        /// </summary>
        [Category( "Appearance" ),
         Description( "The font for items in the popup menu." )]
        public Font PopupFont {
            get { return this.popupFont; }
            set { this.popupFont = value; }
        }

        /// <summary>
        /// Get or set he number of parent levels the Control that owns the popup menu is above the Control using this item.
        /// </summary>
        [Category( "Behavior" ),
         Description( "Specifies the number of parent levels the Control that owns the popup menu is above the Control using this item." )]
        public int PopupParentLevelsAbove {
            get { return this.popupParentLevelsAbove; }
            set { this.popupParentLevelsAbove = value; }
        }

        [Category( "Behavior" ),
         DefaultValue( false ),
         Description( "If true, the menu extends to the left of the triggering link, rather than the right as usual." )]
        public bool PopUpToLeft {
            get { return this.popUpToLeft; }
            set { this.popUpToLeft = value; }
        }

        [Category( "Behavior" ),
         DefaultValue( false ),
         Description( "If true, the menu extends is shown the triggering link, rather than below as usual." )]
        public bool PopUpAbove {
            get { return this.popUpAbove; }
            set { this.popUpAbove = value; }
        }
        #endregion

        #region Mouse Event Handlers
        /// <summary>
        /// Handles mouse entry into the menu control.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnMouseEnter( EventArgs e ) {
            base.OnMouseEnter( e );
            this.mouseInControl = true;
            StopAutoHideTimer();

            if( this.ignoreNextMouseLeave == false ) {
                // activate the menu 
                HighlightMainLink( true );
                ShowPopupMenu();
            }
        }

        /// <summary>
        /// Override mouse exit from the menu control.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnMouseLeave( EventArgs e ) {
            base.OnMouseLeave( e );
            this.mouseInControl = false;

            if( this.ignoreNextMouseLeave == false ) {
                // start the timer that will hide the menu
                StartAutoHideTimer();
            }
            else {
                // start the timer that determines whether we have totally left the control or just moved into the main link
                this.leaveCheckTimer.Stop();
                this.leaveCheckTimer.Start();
            }
        }

        /// <summary>
        /// Handles mouse-enter events for the main link label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_MouseEnter( object sender, EventArgs e ) {
           this.mouseInControl = true;
           StopAutoHideTimer();

           if( this.ignoreNextMouseLeave == false ) {
               // activate the menu 
               HighlightMainLink( true );
               ShowPopupMenu();
           }
        }

        /// <summary>
        /// Passes on mouseEnter events from the main link to the overall control's mouse-leave handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_MouseLeave( object sender, EventArgs e ) {
            // handled the same as for the overall control
            this.OnMouseLeave( e );
        }

        /// <summary>
        /// Handles mouse entry into the popup menu panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupMenu_MouseEnter( object sender, EventArgs e ) {
            // keep the menu viisble as long as the mouse is in it
            StopAutoHideTimer();
        }

        /// <summary>
        /// Handles mouse exiting the popup menu panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupMenu_MouseLeave( object sender, EventArgs e ) {
            // start the timer that will hide the menu
            StartAutoHideTimer();
        }

        /// <summary>
        /// Handles a clcik on a link item in the popup menu by hiding the menu and calling the appropriate callback method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e ) {
            StopAutoHideTimer();
            HidePopupMenu();
            HighlightMainLink( false );

            LinkLabel clickedLink = (LinkLabel)sender;

            // perform the callback associated with this link
            LinkClicked callback = (LinkClicked)clickedLink.Tag;
            if( callback != null ) {
                Launch( callback );
            }
        }

        /// <summary>
        /// Launch the given callback, after a very brief return to the main event loop so the menu can disappear
        /// </summary>
        /// <param name="callback"></param>
        private void Launch( LinkClicked callback ) {
            if( this.callbackTimer != null ) {
                this.callbackTimer.Stop();
            }
            this.callbackTimer = new Timer();
            this.callbackTimer.Interval = 1;
            this.callbackTimer.Tick += new EventHandler( LaunchCallback );
            this.activeCallback = callback;

            this.callbackTimer.Start();
        }

        /// <summary>
        /// Actually laucnh the callback method corresponding to the clicked link.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LaunchCallback( object sender, EventArgs e ) {
            this.callbackTimer.Stop();

            if( this.activeCallback != null ) {
                this.activeCallback();
            }
        }
        #endregion Mouse event handlers

        #region Timer Methods
        /// <summary>
        /// Start the timer which continues to shows the menu for a while after the user has moved the
        /// mouse out of the menu control.
        /// </summary>
        protected void StartAutoHideTimer() {
            this.autoHideTimer.Stop();
            this.autoHideTimer.Start();
        }

        /// <summary>
        /// Stop the menu auto-hide timer.
        /// </summary>
        protected void StopAutoHideTimer() {
            this.autoHideTimer.Stop();
        }

        /// <summary>
        /// Menu timer callback function.
        /// </summary>
        /// <param name="obj">Timer callback object.</param>
        /// <param name="e">Timer callback event argument.</param>
        protected void AutoHideTimerCallback( object obj, EventArgs e ) {
            StopAutoHideTimer();

            // turn off the item highlight
            HighlightMainLink( false );

            // hide the popup, provided this object is still the active PopupMenuLinkLabel
            if( this.autoHideTimer.Tag == PopupMenuLinkLabel.ActiveMenuLink ) {
                HidePopupMenu();
            }
        }
 
        /// <summary>
        /// Verifies that the mouse truly left this control.
        /// </summary>
        /// <remarks>
        /// This timer callback function verifies a mouseLeave event from the overall control wasn't (almost) 
        /// immediately followed by a mouseEnter into the main link control.  It does so by checking the mouseInControl
        /// variable, which is set to true by mouseEnter for either the overall control or the main link control.
        /// </remarks>
        /// <param name="obj">Timer callback object.</param>
        /// <param name="e">Timer callback event argument.</param>
        protected void LeaveCheckTimerCallback( object obj, EventArgs e ) {
            this.leaveCheckTimer.Stop();
            if( mouseInControl == false ) {
                this.ignoreNextMouseLeave = false;
            }
        }
        #endregion

        #region Popup Menu Methods
        /// <summary>
        /// Adds an item to the popup links menu.
        /// </summary>
        /// <param name="text">Link text</param>
        /// <param name="click">Callback method to invoke wen this link is clicked</param>
        /// <returns></returns>
        public LinkLabel AddItem( string text, LinkClicked click ) {
            // create a new link
            LinkLabel link = new LinkLabel();

            link.AutoSize = false;
            link.Text = text;
            link.Font = this.popupFont;
            link.Tag = click;
            link.BackColor = Color.Transparent;

            link.MouseEnter += new EventHandler( popupMenu_MouseEnter );
            link.MouseLeave += new EventHandler( popupMenu_MouseLeave );
            link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( linkLabel_LinkClicked );

            // add the new link label to the list
            popupLinkLabels.Add( link );

            return link;
        }

        /// <summary>
        /// Removes all items from the popup menu.
        /// </summary>
        public void ClearAllItems() {
            popupLinkLabels = new ArrayList();
        }

        /// <summary>
        /// Disables all menu items with the specified values
        /// </summary>
        /// <param name="disable"></param>
        public void EnableAllLinks() {
            disableLinks = new ArrayList();
        }

        /// <summary>
        /// Disables (only) all menu items with the specified values
        /// </summary>
        /// <param name="disable"></param>
        public void DisableLinks( params string[] disable ) {
            disableLinks = new ArrayList();
            for( int i = 0; i < disable.Length; i++ ) {
                disableLinks.Add( disable[ i ] );
            }
        }

        /// <summary>
        /// Disables the menu item with the specified text. Does not affect other items.
        /// </summary>
        /// <param name="disable"></param>
        public void DisableLink( string disableItemText ) {
            disableLinks.Add( disableItemText );
        }

        /// <summary>
        /// Re-enables the menu item with the specified vtextalue. Does not affect other items.
        /// </summary>
        /// <param name="disable"></param>
        public void EnableLink( string disableItemText ) {
            disableLinks.Remove( disableItemText );
        }

        /// <summary>
        /// Shows the popup links menu
        /// </summary>
        public void ShowPopupMenu() {
            if( popupMenuPanel.Visible && (popupMenuPanel.Tag == this) ) {
                // we are already showing the appropriate menu
                return;   
            }

            if( popupLinkLabels == null || popupLinkLabels.Count == 0 ) {
                // just in case
                return;
            }

            if( BeforeActivate != null ) {
                BeforeActivate();
            }

            PopupMenuLinkLabel.ActiveMenuLink = this;

            popupMenuPanel.SuspendLayout();
            popupMenuPanel.Controls.Clear();
            popupMenuPanel.Font = this.popupFont;
            popupMenuPanel.BackColor = this.popupBackColor;

            popupMenuPanel.MouseEnter += new EventHandler( popupMenu_MouseEnter );
            popupMenuPanel.MouseLeave += new EventHandler( popupMenu_MouseLeave );

            // determine the needed overall size and component layout
            Graphics g = Graphics.FromHwnd( popupMenuPanel.Handle );
            float totHt = this.topMargin;
            float minWid = minimumWidth;
            int nLinks = popupLinkLabels.Count;

            SizeF sz0 = g.MeasureString( this.linkLabel.Text, this.linkLabel.Font );
            minWid = (float)Math.Max( minWid, sz0.Width + this.tabMargin );

            for( int i = 0; i < nLinks; i++ ) {
                LinkLabel subLink = (LinkLabel)popupLinkLabels[ i ];
                popupMenuPanel.Controls.Add( subLink );
                string s = subLink.Text;
                SizeF sz = g.MeasureString( s, popupMenuPanel.Font );
                minWid = (float)Math.Max( minWid, sz.Width );
                subLink.Location = new Point( this.leftMargin, (int)Math.Round( totHt ) );
                subLink.Size = new Size( (int)Math.Ceiling( sz.Width ), (int)Math.Ceiling( sz.Height ) );
                totHt += sz.Height + menuItemSpacing;

                bool enabled = !(bool)disableLinks.Contains( ((LinkLabel)popupLinkLabels[ i ]).Text ); 
                if( enabled ) {
                    subLink.LinkColor = UIConfigSettings.Colors.PopupMenuLinkColor;
                    subLink.LinkBehavior = LinkBehavior.SystemDefault;
                    subLink.Enabled = true;
                }
                else {
                    subLink.LinkColor = UIConfigSettings.Colors.PopupMenuDisabledLinkColor;
                    subLink.LinkBehavior = LinkBehavior.NeverUnderline;
                    subLink.Enabled = false;
                }
            }

            minWid += this.leftMargin + this.rightMargin;
            totHt += this.bottomMargin;

            // determine the location of the popup menu
            int popupWidth = (int)Math.Round( minWid );
            int popupHeight = (int)Math.Round( totHt );

            Point loc = new Point( this.Bounds.Left, this.Bounds.Bottom );

            if( this.popUpToLeft ) {
                loc.X = this.Bounds.Right - popupWidth - 2;
            }
            else {
                loc.X += 2;
            }

            if( this.popUpAbove ) {
                loc.Y -= (this.Bounds.Height + popupHeight - 2);
            }
            else {
                loc.Y -= 3;
            }

            Control parent = this;
            for( int i = 0; i < this.popupParentLevelsAbove; i++ ) {    
                if( parent.Parent != null ) {
                    parent = parent.Parent;
                    loc.X += parent.Location.X;
                    loc.Y += parent.Location.Y;
                }
            }

            loc.X += popupXAdjust;

            popupMenuPanel.Location = loc;
            popupMenuPanel.Size = new Size( popupWidth, popupHeight );

            popupMenuPanel.ResumeLayout();

            if( popupMenuPanel.Tag != this ) {
                // be sure the popup background repaints if it is changing location
                popupMenuPanel.Invalidate();
            }

            // actually show the popup menu
            popupMenuPanel.Tag = this;
            //popupMenuPanel.BringToFront();

            foreach( PopupMenuLinkLabel lnk in allLinks ) {
                if( lnk.popupMenuPanel.Tag != this ) {
                    lnk.Reset();
                }
            }
            popupMenuPanel.Visible = true;
        }

        /// <summary>
        /// Hides the popup menu.
        /// </summary>
        private void HidePopupMenu() {
            popupMenuPanel.Visible = false;
        }

        public void Reset() {
            HighlightMainLink( false );
            popupMenuPanel.Visible = false;
        }
        #endregion

        #region Paint Methids
        /// <summary>
        /// Activates or clears the highlighted state of the main link item
        /// </summary>
        /// <param name="highlightOn"></param>
        private void HighlightMainLink( bool highlightOn ) {

            if( highlightOn ) {
                this.linkLabel.BackColor = this.popupBackColor;
            }
            else {
                this.linkLabel.BackColor = this.BackColor;
            }
            this.highlightOn = highlightOn;
            this.Invalidate();
        }

        /// <summary>
        /// Paint the background for the main link label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PopupMenuLinkLabel_Paint( object sender, PaintEventArgs e ) {
            if( this.highlightOn ) {

                SizeF lblSize = e.Graphics.MeasureString( this.linkLabel.Text, this.linkLabel.Font );
                int lblWid = (int)Math.Round( lblSize.Width );

                int border = 2;
                int x1 = border;
                int x2 = x1 + lblWid + (2 * border);

                int y1 = border;
                int y2 = this.Height - border;

                int arcSize = 4;
                ArrayList pointList = new ArrayList();
                pointList.Add( new Point( x1, y2 ) );

                pointList.Add( PointForArc( x1 + arcSize, y1 + arcSize, arcSize, 180 ) );
                pointList.Add( PointForArc( x1 + arcSize, y1 + arcSize, arcSize, 225 ) );
                pointList.Add( PointForArc( x1 + arcSize, y1 + arcSize, arcSize, 270 ) );

                pointList.Add( PointForArc( x2 - arcSize, y1 + arcSize, arcSize, 270 ) );
                pointList.Add( PointForArc( x2 - arcSize, y1 + arcSize, arcSize, 315 ) );
                pointList.Add( PointForArc( x2 - arcSize, y1 + arcSize, arcSize, 0 ) );

                pointList.Add( new Point( x2, y2 ) );

                Point[] points = new Point[ pointList.Count ];
                pointList.CopyTo( points );

                Pen p = new Pen( Color.Black );
                Brush b = new SolidBrush( this.popupBackColor );
 //               e.Graphics.FillRectangle( b, border, border, x2 - border, y2 - border );
                //Console.WriteLine( "PopupMenuLinkLabel_Paint() - FillRectangle( {0} )", new Rectangle( border, border, x2 - border, y2 - border ) );
                e.Graphics.DrawLines( p, points );
            }
        }
        
        /// <summary>
        /// Paints a nice-looking border on the popup menu.
        /// </summary>
        /// <param name="menuPanel"></param>
        /// <param name="g"></param>
        public static void PaintMenuPanelBackground( Panel menuPanel, Graphics g, int ownerWid ) {

            PopupMenuLinkLabel activeLink = PopupMenuLinkLabel.ActiveMenuLink;
            SizeF lblSize = g.MeasureString( activeLink.linkLabel.Text, activeLink.linkLabel.Font );
            int lblWid = (int)Math.Round( lblSize.Width );

            int w = menuPanel.Width;
            int h = menuPanel.Height;
            Point[] points = new Point[ 7 ];
            Point[] points2 = new Point[ 4 ];

            //Console.WriteLine( "PaintMenuPanelBackground() width = {0}", w );

            if( PopupMenuLinkLabel.ActiveMenuLink.popUpAbove == false ) {
                if( PopupMenuLinkLabel.ActiveMenuLink.popUpToLeft == false ) {
                    // popup is below and to right (default)
                    int p = 0;
                    points[ p++ ] = new Point( 0, 0 );
                    points[ p++ ] = new Point( 0, h - 3 );
                    points[ p++ ] = new Point( 2, h - 1 );
                    points[ p++ ] = new Point( w - 3, h - 1 );
                    points[ p++ ] = new Point( w - 1, h - 3 );
                    points[ p++ ] = new Point( w - 1, 0 );
                    points[ p++ ] = new Point( lblWid + 4, 0 );

                    int xofst = -2;
                    int yofst = -2;
                    for( int i = 2; i <= 5; i++ ) {
                        int i2 = i - 2;
                        points2[ i2 ] = new Point( points[ i ].X + xofst, points[ i ].Y + yofst );
                    }
                }
                else {
                    // popup is below and to left
                    points[ 0 ] = new Point( w - PopupMenuLinkLabel.ActiveMenuLink.Width + 4, 0 );
                    points[ 1 ] = new Point( 0, 0 );
                    points[ 2 ] = new Point( 0, h - 1 );
                    points[ 3 ] = new Point( w - 1, h - 1 );
                    points[ 4 ] = new Point( w - 1, 0 );
                }
            }
            else {
                // popup is above and to right
                if( ActiveMenuLink.popUpToLeft == false ) {
                    points[ 0 ] = new Point( 0, h - 1 );
                    points[ 1 ] = new Point( 0, 0 );
                    points[ 2 ] = new Point( w - 1, 0 );
                    points[ 3 ] = new Point( w - 1, h - 1 );
                    points[ 4 ] = new Point( PopupMenuLinkLabel.ActiveMenuLink.Width - 4, h - 1 );
                }
                else {
                    // popup is above and to left
                    points[ 0 ] = new Point( w - PopupMenuLinkLabel.ActiveMenuLink.Width + 4, h - 1 );
                    points[ 1 ] = new Point( 0, h - 1 );
                    points[ 2 ] = new Point( 0, 0 );
                    points[ 3 ] = new Point( w - 1, 0 );
                    points[ 4 ] = new Point( w - 1, h - 1 );
                }
            }

            Rectangle grect = new Rectangle( 0, 0, w, h );
            Brush gbrush = new LinearGradientBrush( grect, 
                UIConfigSettings.Colors.PopupMenuFadeStart, UIConfigSettings.Colors.PopupMenuFadeEnd, LinearGradientMode.ForwardDiagonal );
//            Brush gbrush = new LinearGradientBrush( grect, Color.White, Color.FromArgb( 244, 244, 244 ), LinearGradientMode.Vertical );
            g.FillRectangle( gbrush, grect );
            g.DrawLines( new Pen( Color.DarkBlue, 2 ), points );
 //           g.DrawLines( new Pen ( Color.Black ), points2 );
        }


        private Point PointForArc( int x0, int y0, int radius, float angleDegrees ) {
            int x = (int)Math.Round( x0 + radius * Math.Cos( angleDegrees * Math.PI / 180 ) );
            int y = (int)Math.Round( y0 + radius * Math.Sin( angleDegrees * Math.PI / 180 ) );
            return new Point( x, y );
        }
        #endregion

        private void mainLinkLabel_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e ) {
            if( this.highlightOn ) {
                // the user wants to hide the menu and has requested it with a click on the main link
                StopAutoHideTimer();

                HighlightMainLink( false );
                HidePopupMenu();
                this.ignoreNextMouseLeave = true;         //this is the only point this gets set to true
            }
            else {
                // the user hid the menu with a click on the main link and now wants to show it again
                StopAutoHideTimer();

                HighlightMainLink( true );
                ShowPopupMenu();
                this.ignoreNextMouseLeave = false;
            }
        }
    }
}
