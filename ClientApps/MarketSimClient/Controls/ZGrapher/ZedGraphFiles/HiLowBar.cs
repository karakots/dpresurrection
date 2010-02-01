﻿//============================================================================
//ZedGraph Class Library - A Flexible Line Graph/Bar Graph Library in C#
//Copyright (C) 2004  John Champion
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

#endregion

namespace ZedGraph
{
	/// <summary>
	/// This class handles the drawing of the curve <see cref="HiLowBar"/> objects.
	/// The Hi-Low Bars are the "floating" bars that have a lower and upper value and
	/// appear at each defined point.
	/// </summary>
	/// 
	/// <author> John Champion </author>
	/// <version> $Revision: 1.2 $ $Date: 2006/06/22 21:05:17 $ </version>
	public class HiLowBar : Bar, ICloneable
	{
	#region Fields
		/// <summary>
		/// Private field that stores the size (width) of this
        /// <see cref="HiLowBar"/> in points (1/72 inch).  Use the public
        /// property <see cref="Size"/> to access this value.
		/// </summary>
		private float		size;

		/// <summary>
		/// Private field that determines whether the bar width will be based on
		/// the <see cref="Size"/> value, or it will be based on available
		/// space similar to <see cref="BarItem"/> objects.  Use the public property
		/// <see cref="IsMaximumWidth"/> to access this value.
		/// </summary>
		private bool isMaximumWidth;
	#endregion

	#region Properties
		/// <summary>
		/// A simple struct that defines the
		/// default property values for the <see cref="ZedGraph.HiLowBar"/> class.
		/// </summary>
		new public struct Default
		{
			// Default HiLowBar properties
			/// <summary>
			/// The default size (width) for the bars (<see cref="HiLowBar.Size"/> property),
			/// in units of points.
			/// </summary>
			public static float Size = 7;
		}
	#endregion

	#region Constructors
		/// <summary>
		/// Default constructor that sets all <see cref="HiLowBar"/> properties to default
		/// values as defined in the <see cref="Bar.Default"/> class.
		/// </summary>
		public HiLowBar() : this( Color.Empty )
		{
		}

		/// <summary>
		/// Default constructor that sets the 
		/// <see cref="Color"/> as specified, and the remaining
		/// <see cref="HiLowBar"/> properties to default
		/// values as defined in the <see cref="Bar.Default"/> class.
		/// The specified color is only applied to the
		/// <see cref="ZedGraph.Fill.Color"/>, and the <see cref="ZedGraph.Border.Color"/>
		/// will be defaulted.
		/// </summary>
		/// <param name="color">A <see cref="Color"/> value indicating
		/// the <see cref="ZedGraph.Fill.Color"/>
		/// of the Bar.
		/// </param>
		public HiLowBar( Color color ) : this( color, Default.Size )
		{
		}

		/// <summary>
		/// Default constructor that sets the 
		/// <see cref="Color"/> and <see cref="Size"/> as specified, and the remaining
		/// <see cref="HiLowBar"/> properties to default
		/// values as defined in the <see cref="Bar.Default"/> class.
		/// The specified color is only applied to the
		/// <see cref="ZedGraph.Fill.Color"/>, and the <see cref="ZedGraph.Border.Color"/>
		/// will be defaulted.
		/// </summary>
		/// <param name="color">A <see cref="Color"/> value indicating
		/// the <see cref="ZedGraph.Fill.Color"/>
		/// of the Bar.
		/// </param>
		/// <param name="size">The size (width) of the <see cref="HiLowBar"/>'s, in points
		/// (1/72nd inch)</param>
		public HiLowBar( Color color, float size ) : base( color )
		{
			this.size = size;
		}

		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The <see cref="HiLowBar"/> object from which to copy</param>
		public HiLowBar( HiLowBar rhs ) : base( rhs )
		{
		}

		/// <summary>
		/// Deep-copy clone routine
		/// </summary>
		/// <returns>A new, independent copy of the <see cref="HiLowBar"/></returns>
		new public object Clone()
		{ 
			return new HiLowBar( this ); 
		}
	#endregion

	#region Properties
		/// <summary>
		/// Gets or sets the size of the <see cref="HiLowBar"/>
		/// </summary>
		/// <remarks>The size of the bars can be set by this value, which
		/// is then scaled according to the scaleFactor (see
		/// <see cref="GraphPane.CalcScaleFactor"/>).  Alternatively,
		/// if <see cref="IsMaximumWidth"/> is true, the bar width will
		/// be set according to the maximum available cluster width less
		/// the cluster gap (see <see cref="GraphPane.GetClusterWidth"/>
		/// and <see cref="GraphPane.MinClusterGap"/>).  That is, if
		/// <see cref="IsMaximumWidth"/> is true, then the value of
		/// <see cref="Size"/> will be ignored.
		/// </remarks>
        /// <value>Size in points (1/72 inch)</value>
        /// <seealso cref="Default.Size"/>
		public float Size
		{
			get { return size; }
			set { size = value; }
		}

		/// <summary>
		/// Determines whether the bar width will be based on
		/// the <see cref="Size"/> value, or it will be based on available
		/// space similar to <see cref="BarItem"/> objects.
		/// </summary>
		/// <remarks>If true, then the value
		/// of <see cref="Size"/> is ignored.  If this value is true, then
		/// <see cref="GraphPane.MinClusterGap"/> will be used to determine the total space between each bar.
		/// </remarks>
		public bool IsMaximumWidth
		{
			get { return isMaximumWidth; }
			set { isMaximumWidth = value; }
		}
	#endregion

	#region Methods
		/// <summary>
		/// Protected internal routine that draws the specified single bar (an individual "point")
		/// of this series to the specified <see cref="Graphics"/> device.
		/// </summary>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see cref="GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="curve">A <see cref="CurveItem"/> object representing the
		/// <see cref="Bar"/>'s to be drawn.</param>
		/// <param name="index">
		/// The zero-based index number for the single bar to be drawn.
		/// </param>
		/// <param name="pos">
		/// The ordinal position of the this bar series (0=first bar, 1=second bar, etc.)
		/// in the cluster of bars.
		/// </param>
		/// <param name="baseAxis">The <see cref="Axis"/> class instance that defines the base (independent)
		/// axis for the <see cref="Bar"/></param>
		/// <param name="valueAxis">The <see cref="Axis"/> class instance that defines the value (dependent)
		/// axis for the <see cref="Bar"/></param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see cref="GraphPane"/> object using the
		/// <see cref="GraphPane.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		override protected void DrawSingleBar( Graphics g, GraphPane pane,
							CurveItem curve,
							int index, int pos, Axis baseAxis, Axis valueAxis,
							double scaleFactor )
		{
			float	scaledSize = GetBarWidth( pane, baseAxis, scaleFactor );

			// pixBase = pixel value for the bar center on the base axis
			// pixValue = pixel value for the bar top on the value axis
			// pixLow = pixel value for the bar bottom on the value axis
			float pixBase, pixHiVal, pixLowVal;

			// curBase = the scale value on the base axis of the current bar
			// curValue = the scale value on the value axis of the current bar
//			double curBase = ( baseAxis is XAxis ) ? points[index].X : points[index].Y;
//			double curValue = ( baseAxis is XAxis ) ? points[index].Y : points[index].X;

			double curBase, curLowVal, curHiVal;
			BarValueHandler valueHandler = new BarValueHandler( pane );
			valueHandler.GetBarValues( curve, index, out curBase,
					out curLowVal, out curHiVal );


			// curLow = the scale value on the value axis for the bottom of the current bar
			// Get a "low" value for the bottom of the bar and verify validity
			//double curLow = points[index].LowValue;

			if (	curLowVal == PointPair.Missing ||
					System.Double.IsNaN( curLowVal ) ||
					System.Double.IsInfinity( curLowVal ) )
				curLowVal = 0;

			// Any value set to double max is invalid and should be skipped
			// This is used for calculated values that are out of range, divide
			//   by zero, etc.
			// Also, any value <= zero on a log scale is invalid

			if ( !curve.Points[index].IsInvalid )
			{
				// calculate a pixel value for the top of the bar on value axis
				pixHiVal = valueAxis.Transform( index, curHiVal );
				// calculate a pixel value for the center of the bar on the base axis
				pixBase = baseAxis.Transform( index, curBase );

				pixLowVal = valueAxis.Transform( index, curLowVal );

				// Calculate the pixel location for the side of the bar (on the base axis)
				float pixSide = pixBase - scaledSize / 2.0F;

				// Draw the bar
				if ( baseAxis is XAxis )
					this.Draw( g, pane, pixSide, pixSide + scaledSize, pixLowVal,
								pixHiVal, scaleFactor, true );
				else
					this.Draw( g, pane, pixLowVal, pixHiVal, pixSide, pixSide + scaledSize,
								scaleFactor, true );
		   }
	   }

		/// <summary>
		/// Returns the width of the bar, in pixels, based on the settings for
		/// <see cref="Size"/> and <see cref="IsMaximumWidth"/>.
		/// </summary>
		/// <param name="pane">The parent <see cref="GraphPane"/> object.</param>
		/// <param name="baseAxis">The <see cref="Axis"/> object that
		/// represents the bar base (independent axis).</param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see cref="GraphPane"/> object using the
		/// <see cref="GraphPane.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		/// <returns>The width of each bar, in pixel units</returns>
		public float GetBarWidth( GraphPane pane, Axis baseAxis, double scaleFactor )
		{
			if ( isMaximumWidth )
				return baseAxis.GetClusterWidth( pane ) / ( 1.0F + pane.MinClusterGap );
			else
				return (float) ( this.size * scaleFactor );
		}
	#endregion

	}
}
