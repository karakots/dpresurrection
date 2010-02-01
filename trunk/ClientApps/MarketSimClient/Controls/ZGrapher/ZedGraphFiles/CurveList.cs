//============================================================================
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
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

using System;
using System.Drawing;
using System.Collections;

namespace ZedGraph
{
	/// <summary>
	/// A collection class containing a list of <see cref="CurveItem"/> objects
	/// that define the set of curves to be displayed on the graph.
	/// </summary>
	/// 
	/// <author> John Champion
	/// modified by Jerry Vos</author>
	/// <version> $Revision: 1.2 $ $Date: 2006/06/22 21:05:17 $ </version>
	public class CurveList : CollectionPlus, ICloneable
	{
	#region Properties
		// internal temporary value that keeps
		// the max number of points for any curve
		// associated with this curveList
		private int	maxPts;

		/// <summary>
		/// Read only value for the maximum number of points in any of the curves
		/// in the list.
		/// </summary>
		public int MaxPts
		{
			get { return maxPts; }
		}

		/// <summary>
		/// Read only property that returns the number of curves in the list that are of
		/// type <see cref="Bar"/>.
		/// </summary>
		public int NumBars
		{
			get
			{
				int count = 0;
				foreach ( CurveItem curve in this )
				{
					if ( curve.IsBar )
						count++;
				}

				return count;
			}
		}

		/// <summary>
		/// Default constructor for the collection class
		/// </summary>
		public CurveList()
		{
			maxPts = 1;
		}

		/// <summary>
		/// Determine if there is any data in any of the <see cref="CurveItem"/>
		/// objects for this graph.  This method does not verify valid data, it
		/// only checks to see if <see cref="CurveItem.NPts"/> > 0.
		/// </summary>
		/// <returns>true if there is any data, false otherwise</returns>
		public bool HasData()
		{
			foreach( CurveItem curve in this )
			{
				if ( curve.Points.Count > 0 )
					return true;
			}
			return false;
		}
	#endregion
	
	#region Constructors
		/// <summary>
		/// Deep-copy clone routine
		/// </summary>
		/// <returns>A new, independent copy of the CurveList</returns>
		public object Clone()
		{ 
			return new CurveList( this ); 
		}
		
		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The XAxis object from which to copy</param>
		public CurveList( CurveList rhs )
		{
			this.maxPts = rhs.maxPts;

			foreach ( CurveItem item in rhs )
			{
				this.Add( (CurveItem) item.Clone() );
			}
		}
		
	#endregion
	
	#region List Methods
		/// <summary>
		/// Indexer to access the specified <see cref="CurveItem"/> object by
		/// its ordinal position in the list.
		/// </summary>
		/// <param name="index">The ordinal position (zero-based) of the
		/// <see cref="CurveItem"/> object to be accessed.</param>
		/// <value>A <see cref="CurveItem"/> object reference.</value>
		public CurveItem this[ int index ]  
		{
			get { return( (CurveItem) List[index] ); }
			set { List[index] = value; }
		}

		/// <summary>
		/// Indexer to access the specified <see cref="CurveItem"/> object by
		/// its <see cref="CurveItem.Label"/> string.
		/// </summary>
		/// <param name="label">The string label of the
		/// <see cref="CurveItem"/> object to be accessed.</param>
		/// <value>A <see cref="CurveItem"/> object reference.</value>
		public CurveItem this[ string label ]  
		{
			get
			{
				int index = IndexOf( label );
				if ( index >= 0 )
					return( (CurveItem) List[index]  );
				else
					return null;
			}
		}

		/// <summary>
		/// Add a <see cref="CurveItem"/> object to the collection at the end of the list.
		/// </summary>
		/// <param name="curve">A reference to the <see cref="CurveItem"/> object to
		/// be added</param>
		/// <seealso cref="IList.Add"/>
		public void Add( CurveItem curve )
		{
			List.Add( curve );
		}

		/// <summary>
		/// Remove a <see cref="CurveItem"/> object from the collection based on an object reference.
		/// </summary>
		/// <param name="curve">A reference to the <see cref="CurveItem"/> object that is to be
		/// removed.</param>
		/// <seealso cref="IList.Remove"/>
		public void Remove( CurveItem curve )
		{
			List.Remove( curve );
		}

		/// <summary>
		/// Insert a <see cref="CurveItem"/> object into the collection at the specified
		/// zero-based index location.
		/// </summary>
		/// <param name="index">The zero-based index location for insertion.</param>
		/// <param name="curve">A reference to the <see cref="CurveItem"/> object that is to be
		/// inserted.</param>
		/// <seealso cref="IList.Insert"/>
		public void Insert( int index, CurveItem curve )
		{
			List.Insert( index, curve );
		}

		/// <summary>
		/// Return the zero-based position index of the
		/// <see cref="CurveItem"/> with the specified <see cref="CurveItem.Label"/>.
		/// </summary>
		/// <param name="label">The <see cref="String"/> label that is in the
		/// <see cref="CurveItem.Label"/> attribute of the item to be found.
		/// </param>
		/// <returns>The zero-based index of the specified <see cref="CurveItem"/>,
		/// or -1 if the <see cref="CurveItem"/> is not in the list</returns>
		/// <seealso cref="IList.IndexOf"/>
		/// <seealso cref="IndexOfTag"/>
		public int IndexOf( string label )
		{
			int index = 0;
			foreach ( CurveItem p in this )
			{
				if ( String.Compare( p.Label, label, true ) == 0 )
					return index;
				index++;
			}

			return -1;
		}

		/// <summary>
		/// Return the zero-based position index of the
		/// <see cref="CurveItem"/> with the specified <see cref="CurveItem.Tag"/>.
		/// </summary>
		/// <remarks>In order for this method to work, the <see cref="CurveItem.Tag"/>
		/// property must be of type <see cref="String"/>.</remarks>
		/// <param name="label">The <see cref="String"/> label that is in the
		/// <see cref="CurveItem.Tag"/> attribute of the item to be found.
		/// </param>
		/// <returns>The zero-based index of the specified <see cref="CurveItem"/>,
		/// or -1 if the <see cref="CurveItem"/> is not in the list</returns>
		/// <seealso cref="IList.IndexOf"/>
		/// <seealso cref="IndexOf"/>
		public int IndexOfTag( string label )
		{
			int index = 0;
			foreach ( CurveItem p in this )
			{
				if ( p.Tag is string &&
							String.Compare( (string) p.Tag, label, true ) == 0 )
					return index;
				index++;
			}

			return -1;
		}

		/// <summary>
		/// Sorts the list according to the point values at the specified index and
		/// for the specified axis.
		/// </summary>
		public void Sort( SortType type, int index )
		{
			InnerList.Sort( new CurveItem.Comparer( type, index ) );
		}
		
		/// <summary>
		/// Go through each <see cref="CurveItem"/> object in the collection,
		/// calling the <see cref="PointPairList.GetRange"/> member to 
		/// determine the minimum and maximum values in the
		/// <see cref="CurveItem.Points"/> list of data value pairs.  If the curves include 
		/// a stack bar, handle within the current GetRange method. In the event that no
		/// data are available, a default range of min=0.0 and max=1.0 are returned.
		/// If the Y axis has a valid data range and the Y2 axis not, then the Y2
		/// range will be a duplicate of the Y range.  Vice-versa for the Y2 axis
		/// having valid data when the Y axis does not.
		/// If any <see cref="CurveItem"/> in the list has a missing
		/// <see cref="PointPairList"/>, a new empty one will be generated.
		/// </summary>
		/// <param name="xMinVal">The minimun X value in the data range for all curves
		/// in this collection</param>
		/// <param name="xMaxVal">The maximun X value in the data range for all curves
		/// in this collection</param>
		/// <param name="yMinVal">The minimun Y (left Y axis) value in the data range
		/// for all curves in this collection</param>
		/// <param name="yMaxVal">The maximun Y (left Y axis) value in the data range
		/// for all curves in this collection</param>
		/// <param name="y2MinVal">The minimun Y2 (right Y axis) value in the data range
		/// for all curves in this collection</param>
		/// <param name="y2MaxVal">The maximun Y2 (right Y axis) value in the data range
		/// for all curves in this collection</param>
		/// <param name="bIgnoreInitial">ignoreInitial is a boolean value that
		/// affects the data range that is considered for the automatic scale
		/// ranging (see <see cref="GraphPane.IsIgnoreInitial"/>).  If true, then initial
		/// data points where the Y value is zero are not included when
		/// automatically determining the scale <see cref="Axis.Min"/>,
		/// <see cref="Axis.Max"/>, and <see cref="Axis.Step"/> size.  All data after
		/// the first non-zero Y value are included.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see cref="GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		public void GetRange( 	out double xMinVal, out double xMaxVal,
								out double yMinVal, out double yMaxVal,
								out double y2MinVal, out double y2MaxVal,
								bool bIgnoreInitial, GraphPane pane )
		{
			double	tXMinVal,
					tXMaxVal,
					tYMinVal,
					tYMaxVal;

			// initialize the values to outrageous ones to start
			xMinVal = yMinVal = y2MinVal = tXMinVal = tYMinVal = Double.MaxValue;
			xMaxVal = yMaxVal = y2MaxVal = tXMaxVal = tYMaxVal = Double.MinValue;
			maxPts = 1;			
			
			PointPairList sumList = null;

			// Loop over each curve in the collection and examine any that are not a stack bar
			foreach( CurveItem curve in this )
			{
				if ( curve.IsBar && pane.BarType == BarType.Stack )
				{
					if ( sumList == null )
						sumList = (PointPairList) curve.Points.Clone();
					else if ( pane.BarBase == BarBase.X )
						sumList.SumY( curve.Points );
					else
						sumList.SumX( curve.Points );
					
					sumList.GetRange( ref tXMinVal, ref tXMaxVal,
						ref tYMinVal, ref tYMaxVal, bIgnoreInitial,
						false, true );
				}
				else
					// Call the GetRange() member function for the current
					// curve to get the min and max values
					curve.GetRange( ref tXMinVal, ref tXMaxVal,
						ref tYMinVal, ref tYMaxVal, bIgnoreInitial, pane );
   			
				bool isYOrd = ( ( pane.Y2Axis.IsOrdinal || pane.Y2Axis.IsText ) && curve.IsY2Axis ) ||
								( ( pane.YAxis.IsOrdinal || pane.YAxis.IsText ) && ! curve.IsY2Axis );
				bool isXOrd = pane.XAxis.IsOrdinal || pane.XAxis.IsText;
   							
				// For ordinal Axes, the data range is just 1 to Npts
				if ( isYOrd )
				{
					tYMinVal = 1.0;
					tYMaxVal = curve.NPts;
				}
				if ( isXOrd )
				{
					tXMinVal = 1.0;
					tXMaxVal = curve.NPts;
				}

				// Bar types always include the Y=0 value
				if ( curve.IsBar )
				{
					if ( pane.BarBase == BarBase.X )
					{
						if ( tYMinVal > 0 )
							tYMinVal = 0;
						else if ( tYMaxVal < 0 )
							tYMaxVal = 0;
   					
						if ( !isXOrd )
						{
							tXMinVal -= pane.ClusterScaleWidth / 2.0;
							tXMaxVal += pane.ClusterScaleWidth / 2.0;
						}
					}
					else
					{
						if ( tXMinVal > 0 )
							tXMinVal = 0;
						else if ( tXMaxVal < 0 )
							tXMaxVal = 0;
   						
						if ( !isYOrd )
						{
							tYMinVal -= pane.ClusterScaleWidth / 2.0;
							tYMaxVal += pane.ClusterScaleWidth / 2.0;
						}
					}
				}

				// determine which curve has the maximum number of points
				if ( curve.NPts > maxPts )
					maxPts = curve.NPts;

				// If the min and/or max values from the current curve
				// are the absolute min and/or max, then save the values
				// Also, differentiate between Y and Y2 values		
				if ( curve.IsY2Axis )
				{
					if ( tYMinVal < y2MinVal )
						y2MinVal = tYMinVal;
					if ( tYMaxVal > y2MaxVal )
						y2MaxVal = tYMaxVal;
				}
				else
				{
					if ( tYMinVal < yMinVal )
						yMinVal = tYMinVal;
					if ( tYMaxVal > yMaxVal )
						yMaxVal = tYMaxVal;
				}
   			
				if ( tXMinVal < xMinVal )
					xMinVal = tXMinVal;
				if ( tXMaxVal > xMaxVal )
					xMaxVal = tXMaxVal;
			
			}
		
			// Define suitable default ranges in the event that
			// no data were available
			if ( xMinVal >= Double.MaxValue || xMaxVal <= Double.MinValue )
			{
				xMinVal = 0;
				xMaxVal = 1;
			}
		
			if ( yMinVal >= Double.MaxValue || yMaxVal <= Double.MinValue )
			{
				if ( y2MinVal < Double.MaxValue && y2MaxVal > Double.MinValue )
				{
					yMinVal = y2MinVal;
					yMaxVal = y2MaxVal;
				}
				else
				{
					yMinVal = 0;
					yMaxVal = 1;
				}
			}
		
			if ( y2MinVal >= Double.MaxValue || y2MaxVal <= Double.MinValue )
			{
				if ( yMinVal < Double.MaxValue && yMaxVal > Double.MinValue )
				{
					y2MinVal = yMinVal;
					y2MaxVal = yMaxVal;
				}
				else
				{
					y2MinVal = 0;
					y2MaxVal = 1;
				}
			}
			//check for single values which may	be	larger than the	totals,	because totals have been        
			// reduced by	negative values.  If any	are found, adjust max/min values accordingly		   rpk
			if	( pane.BarType == BarType.Stack)
				foreach(	CurveItem curve	in this	)
				{	  
					if	( curve.IsBar 	)
					{
						curve.Points.GetRange( ref tXMinVal, ref tXMaxVal,
										ref tYMinVal, ref tYMaxVal, bIgnoreInitial, false, true);
										
						if	( curve.IsY2Axis )
						{
							if	( tYMinVal < y2MinVal )
								y2MinVal =	tYMinVal;
							if	( tYMaxVal > y2MaxVal )
								y2MaxVal	=	tYMaxVal;
						}
						else
						{
							if	( tYMinVal < yMinVal )
								yMinVal	=	tYMinVal;
							if	( tYMaxVal > yMaxVal	)
								yMaxVal	= tYMaxVal;
						}
						
						if	( tXMinVal < xMinVal )
							xMinVal	=	tXMinVal;
						if	( tXMaxVal > xMaxVal	)
							xMaxVal	= tXMaxVal;
					}
				}		
			
			//create reasonable min/max values for bartype = percentStack 
			bool hasNegative = false ;
			if ( pane.BarType == BarType.PercentStack)
			{
				foreach (CurveItem curve in this) 
				{
					if ( curve.IsBar)
						foreach ( PointPair point in curve.Points )
							if ( point.X < 0 || point.Y < 0 )
							{
								hasNegative = true ;		
								if ( pane.BarBase == BarBase.X )
								{
									yMinVal = -100 ;
									yMaxVal = 100 ;
								}
								else
								{
									xMinVal = -100 ;
									xMaxVal = 100 ;
								}
								break ;			
							}
						if ( hasNegative )
							break ;
					}
				if ( !hasNegative )
					if ( pane.BarBase == BarBase.X )
					{
						yMinVal = 0 ;
						yMaxVal = 100 ;
					}
					else
					{
						xMinVal = 0 ;
						xMaxVal = 100 ;
					}					
			}
						
		}
		
		/// <summary>
		/// Render all the <see cref="CurveItem"/> objects in the list to the
		/// specified <see cref="Graphics"/>
		/// device by calling the <see cref="CurveItem.Draw"/> member function of
		/// each <see cref="CurveItem"/> object.
		/// </summary>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see cref="GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see cref="GraphPane"/> object using the
		/// <see cref="GraphPane.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		public void Draw( Graphics g, GraphPane pane, double scaleFactor )
		{
			// Configure the accumulator for stacked bars
			//Bar.ResetBarStack();

			// Count the number of BarItems in the curvelist
			int pos = this.NumBars;
			
			if ( pane.BarType == BarType.SortedOverlay )
			{
				// First, create a new curveList with references (not clones) of the curves
				CurveList tempList = new CurveList();
				foreach ( CurveItem curve in this )
					if ( curve.IsBar )
						tempList.Add( (CurveItem) curve );
				
				for ( int i=0; i<this.maxPts; i++ )
				{
					tempList.Sort( pane.BarBase == BarBase.X ? SortType.YValues : SortType.XValues, i );
					foreach ( BarItem barItem in tempList )
						barItem.Bar.DrawSingleBar( g, pane, barItem,
							((BarItem)barItem).BaseAxis(pane),
							((BarItem)barItem).ValueAxis(pane, barItem.IsY2Axis),
							0, i, scaleFactor );
				}
			}

			//	Loop for each curve in reverse order to pick up the remaining bartypes
			for ( int i=this.Count-1; i>=0; i-- )
			{
				CurveItem curve = this[i];
				
				if ( curve.IsBar )
					pos--;
					
				// Render the curve
				//	if	it's a bar type or a sorted overlay or a percentstacked bar, it's already been	done above
				if	( !(pane.BarType == BarType.SortedOverlay) || !curve.IsBar  ) 
					curve.Draw( g, pane, pos, scaleFactor );
			}
		}
	#endregion

	}
}


