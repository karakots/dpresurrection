//============================================================================
//PointPairList Class
//Copyright (C) 2004  Jerry Vos
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
	/// A collection class containing a list of <see cref="PointPair"/> objects
	/// that define the set of points to be displayed on the curve.
	/// </summary>
	/// 
	/// <author> Jerry Vos based on code by John Champion
	/// modified by John Champion</author>
	/// <version> $Revision: 1.2 $ $Date: 2006/06/22 21:05:17 $ </version>
	public class PointPairList : CollectionPlus, ICloneable
	{
	#region Fields
		/// <summary>Private field to maintain the sort status of this
		/// <see cref="PointPairList"/>.  Use the public property
		/// <see cref="Sorted"/> to access this value.
		/// </summary>
		protected bool sorted = true;
	#endregion

	#region Properties
		/// <summary>
		/// Indexer to access the specified <see cref="PointPair"/> object by
		/// its ordinal position in the list.
		/// </summary>
		/// <param name="index">The ordinal position (zero-based) of the
		/// <see cref="PointPair"/> object to be accessed.</param>
		/// <value>A <see cref="PointPair"/> object reference.</value>
		public PointPair this[ int index ]  
		{
			get { return (PointPair) List[index]; }
			set { List[index] = value; }
		}

		/// <summary>
		/// true if the list is currently sorted.
		/// </summary>
		/// <seealso cref="Sort()"/>
		public bool Sorted
		{
			get { return sorted; }
		}
	#endregion

	#region Constructors
		/// <summary>
		/// Default constructor for the collection class
		/// </summary>
		public PointPairList()
		{
		}

		/// <summary>
		/// Constructor to initialize the PointPairList from two arrays of
		/// type double.
		/// </summary>
		public PointPairList( double[] x, double[] y )
		{
			Add( x, y );
			
			sorted = false;
		}

		/// <summary>
		/// Constructor to initialize the PointPairList from three arrays of
		/// type double.
		/// </summary>
		public PointPairList( double[] x, double[] y, double[] baseVal )
		{
			Add( x, y, baseVal );
			
			sorted = false;
		}

		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The PointPairList from which to copy</param>
		public PointPairList( PointPairList rhs )
		{
			Add( rhs );

			sorted = false;
		}

		/// <summary>
		/// Deep-copy clone routine
		/// </summary>
		/// <returns>A new, independent copy of the PointPairList</returns>
		virtual public object Clone()
		{ 
			return new PointPairList( this ); 
		}
		
	#endregion

	#region Methods
		/// <summary>
		/// Add a <see cref="PointPair"/> object to the collection at the end of the list.
		/// </summary>
		/// <param name="point">A reference to the <see cref="PointPair"/> object to
		/// be added</param>
		/// <returns>The zero-based ordinal index where the point was added in the list.</returns>
		/// <seealso cref="IList.Add"/>
		public int Add( PointPair point )
		{
			sorted = false;
			return List.Add( point );
		}

		/// <summary>
		/// Add a <see cref="PointPairList"/> object to the collection at the end of the list.
		/// </summary>
		/// <param name="pointList">A reference to the <see cref="PointPairList"/> object to
		/// be added</param>
		/// <returns>The zero-based ordinal index where the last point was added in the list,
		/// or -1 if no points were added.</returns>
		/// <seealso cref="IList.Add"/>
		public int Add( PointPairList pointList )
		{
			int rv = -1;
			
			foreach ( PointPair point in pointList )
				rv = this.Add( point );
				
			sorted = false;
			return rv;
		}

		/// <summary>
		/// Add a set of points to the PointPairList from two arrays of type double.
		/// If either array is null, then a set of ordinal values is automatically
		/// generated in its place (see <see cref="AxisType.Ordinal"/>.
		/// If the arrays are of different size, then the larger array prevails and the
		/// smaller array is padded with <see cref="PointPair.Missing"/> values.
		/// </summary>
		/// <param name="x">A double[] array of X values</param>
		/// <param name="y">A double[] array of Y values</param>
		/// <returns>The zero-based ordinal index where the last point was added in the list,
		/// or -1 if no points were added.</returns>
		/// <seealso cref="IList.Add"/>
		public int Add( double[] x, double[] y )
		{
			PointPair	point = new PointPair( 0, 0, 0 );
			int 		len = 0,
						rv = -1;
			
			if ( x != null )
				len = x.Length;
			if ( y != null && y.Length > len )
				len = y.Length;
			
			for ( int i=0; i<len; i++ )
			{
				if ( x == null )
					point.X = (double) i + 1.0;
				else if ( i < x.Length )
					point.X = x[i];
				else
					point.X = PointPair.Missing;
					
				if ( y == null )
					point.Y = (double) i + 1.0;
				else if ( i < y.Length )
					point.Y = y[i];
				else
					point.Y = PointPair.Missing;
					
				rv = List.Add( point );
			}
			
			sorted = false;
			return rv;
		}

		/// <summary>
		/// Add a set of points to the <see cref="PointPairList"/> from three arrays of type double.
		/// If the X or Y array is null, then a set of ordinal values is automatically
		/// generated in its place (see <see cref="AxisType.Ordinal"/>.  If the <see paramref="baseVal"/>
		/// is null, then it is set to zero.
		/// If the arrays are of different size, then the larger array prevails and the
		/// smaller array is padded with <see cref="PointPair.Missing"/> values.
		/// </summary>
		/// <param name="x">A double[] array of X values</param>
		/// <param name="y">A double[] array of Y values</param>
		/// <param name="z">A double[] array of Z or lower-dependent axis values</param>
		/// <returns>The zero-based ordinal index where the last point was added in the list,
		/// or -1 if no points were added.</returns>
		/// <seealso cref="IList.Add"/>
		public int Add( double[] x, double[] y, double[] z )
		{
			PointPair	point = new PointPair();
			int 		len = 0,
						rv = -1;
			
			if ( x != null )
				len = x.Length;
			if ( y != null && y.Length > len )
				len = y.Length;
			if ( z != null && z.Length > len )
				len = z.Length;
						
			for ( int i=0; i<len; i++ )
			{
				if ( x == null )
					point.X = (double) i + 1.0;
				else if ( i < x.Length )
					point.X = x[i];
				else
					point.X = PointPair.Missing;
					
				if ( y == null )
					point.Y = (double) i + 1.0;
				else if ( i < y.Length )
					point.Y = y[i];
				else
					point.Y = PointPair.Missing;
					
				if ( z == null )
					point.Z = (double) i + 1.0;
				else if ( i < z.Length )
					point.Z = z[i];
				else
					point.Z = PointPair.Missing;
					
				rv = List.Add( point );
			}
			
			sorted = false;
			return rv;
		}

		/// <summary>
		/// Add a single point to the <see cref="PointPairList"/> from values of type double.
		/// </summary>
		/// <param name="x">The X value</param>
		/// <param name="y">The Y value</param>
		/// <returns>The zero-based ordinal index where the point was added in the list.</returns>
		/// <seealso cref="IList.Add"/>
		public int Add( double x, double y )
		{
			sorted = false;
			PointPair	point = new PointPair( x, y );
			return List.Add( point );
		}

		/// <summary>
		/// Add a single point to the <see cref="PointPairList"/> from values of type double.
		/// </summary>
		/// <param name="x">The X value</param>
		/// <param name="y">The Y value</param>
		/// <param name="z">The Z or lower dependent axis value</param>
		/// <returns>The zero-based ordinal index where the point was added
		/// in the list.</returns>
		/// <seealso cref="IList.Add"/>
		public int Add( double x, double y, double z )
		{
			sorted = false;
			PointPair point = new PointPair( x, y, z );
			return List.Add( point );
		}

		/// <summary>
		/// Remove the specified <see cref="PointPair"/> object from the collection based
		/// the point values (must match exactly).
		/// </summary>
		/// <param name="pt">
		/// A <see cref="PointPair"/> that is to be removed by value.
		/// </param>
		/// <seealso cref="IList.Remove"/>
		public void Remove( PointPair pt )
		{
			List.Remove( pt );
		}

		/// <summary>
		/// Return the zero-based position index of the specified
		/// <see cref="PointPair"/> in the collection.
		/// </summary>
		/// <param name="pt">The <see cref="PointPair"/> object that is to be found.
		/// </param>
		/// <returns>The zero-based index of the specified <see cref="PointPair"/>, or -1 if the <see cref="PointPair"/>
		/// is not in the list</returns>
		/// <seealso cref="IList.IndexOf"/>
		public int IndexOf( PointPair pt )
		{
			return List.IndexOf( pt );
		}

		/// <summary>
		/// Return the zero-based position index of the
		/// <see cref="PointPair"/> with the specified label <see cref="PointPair.Tag"/>.
		/// </summary>
		/// <remarks>The <see cref="PointPair.Tag"/> object must be of type <see cref="String"/>
		/// for this method to find it.</remarks>
		/// <param name="label">The <see cref="String"/> label that is in the
		/// <see cref="PointPair.Tag"/> attribute of the item to be found.
		/// </param>
		/// <returns>The zero-based index of the specified <see cref="PointPair"/>,
		/// or -1 if the <see cref="PointPair"/> is not in the list</returns>
		/// <seealso cref="IList.IndexOf"/>
		public int IndexOfTag( string label )
		{
			int iPt = 0;
			foreach ( PointPair p in this )
			{
				if ( p.Tag is string && String.Compare( (string) p.Tag, label, true ) == 0 )
					return iPt;
				iPt++;
			}

			return -1;
		}

		/// <summary>
		/// Sorts the list according to the point x values. Will not sort the 
		/// list if the list is already sorted.
		/// </summary>
		/// <returns>If the list was sorted before sort was called</returns>
		public bool Sort()
		{
			// if it is already sorted we don't have to sort again
			if ( sorted )
				return true;

			InnerList.Sort( new PointPair.PointPairComparer( SortType.XValues ) );
			return false;
		}
		
		/// <summary>
		/// Sorts the list according to the point values . Will not sort the 
		/// list if the list is already sorted.
		/// </summary>
		/// <param name="type"></param>  The <see cref = "SortType"/>
		///used to determine whether the X or Y values will be used to sort
		///the list
		/// <returns>If the list was sorted before sort was called</returns>
		public bool Sort( SortType type)
		{
			// if it is already sorted we don't have to sort again
			if ( sorted )
				return true;
				
			InnerList.Sort( new PointPair.PointPairComparer( type ) );
			
			return false;
		}
		
		/// <summary>
		/// Add the Y values from the specified <see cref="PointPairList"/> object to this
		/// <see cref="PointPairList"/>.  If <see paramref="sumList"/> has more values than
		/// this list, then the extra values will be ignored.  If <see paramref="sumList"/>
		/// has less values, the missing values are assumed to be zero.
		/// </summary>
		/// <param name="sumList">A reference to the <see cref="PointPairList"/> object to
		/// be summed into the this <see cref="PointPairList"/>.</param>
		public void SumY( PointPairList sumList )
		{
			for ( int i=0; i<this.Count; i++ )
			{
				if ( i < sumList.Count )
				{
					PointPair point = this[i];
					point.Y += sumList[i].Y;
					this[i] = point;
				}
			}
				
			//sorted = false;
		}

		/// <summary>
		/// Add the X values from the specified <see cref="PointPairList"/> object to this
		/// <see cref="PointPairList"/>.  If <see paramref="sumList"/> has more values than
		/// this list, then the extra values will be ignored.  If <see paramref="sumList"/>
		/// has less values, the missing values are assumed to be zero.
		/// </summary>
		/// <param name="sumList">A reference to the <see cref="PointPairList"/> object to
		/// be summed into the this <see cref="PointPairList"/>.</param>
		public void SumX( PointPairList sumList )
		{
			for ( int i=0; i<this.Count; i++ )
			{
				if ( i < sumList.Count )
				{
					PointPair point = this[i];
					point.X += sumList[i].X;
					this[i] = point;
				}
			}
				
			sorted = false;
		}

		/// <summary>
		/// Go through the list of <see cref="PointPair"/> data values
		/// and determine the minimum and maximum values in the data.
		/// </summary>
		/// <param name="xMin">The minimum X value in the range of data</param>
		/// <param name="xMax">The maximum X value in the range of data</param>
		/// <param name="yMin">The minimum Y value in the range of data</param>
		/// <param name="yMax">The maximum Y value in the range of data</param>
		/// <param name="ignoreInitial">ignoreInitial is a boolean value that
		/// affects the data range that is considered for the automatic scale
		/// ranging (see <see cref="GraphPane.IsIgnoreInitial"/>).  If true, then initial
		/// data points where the Y value is zero are not included when
		/// automatically determining the scale <see cref="Axis.Min"/>,
		/// <see cref="Axis.Max"/>, and <see cref="Axis.Step"/> size.  All data after
		/// the first non-zero Y value are included.
		/// </param>
		/// <param name="isZIncluded">true to include the Z data in the data range for
		/// the dependent axis</param>
		/// <param name="isXIndependent">true if X is the independent axis, false
		/// if Y or Y2 is the independent axis.</param>
		virtual public void GetRange(	ref double xMin, ref double xMax,
										ref double yMin, ref double yMax,
										bool ignoreInitial,
										bool isZIncluded,
										bool isXIndependent )
		{
			// initialize the values to outrageous ones to start
			xMin = yMin = Double.MaxValue;
			xMax = yMax = Double.MinValue;
			
			// Loop over each point in the arrays
			foreach ( PointPair point in this )
			{
				double curX = point.X;
				double curY = point.Y;
				double curZ = point.Z;
				
				// ignoreInitial becomes false at the first non-zero
				// Y value
				if (	ignoreInitial && curY != 0 &&
						curY != PointPair.Missing )
					ignoreInitial = false;
				
				if ( 	!ignoreInitial &&
						curX != PointPair.Missing &&
						curY != PointPair.Missing )
				{
					if ( curX < xMin )
						xMin = curX;
					if ( curX > xMax )
						xMax = curX;
					if ( curY < yMin )
						yMin = curY;
					if ( curY > yMax )
						yMax = curY;

					if ( isZIncluded && isXIndependent && curZ != PointPair.Missing )
					{
						if ( curZ < yMin )
							yMin = curZ;
						if ( curZ > yMax )
							yMax = curZ;
					}
					else if ( isZIncluded && curZ != PointPair.Missing )
					{
						if ( curZ < xMin )
							xMin = curZ;
						if ( curZ > xMax )
							xMax = curZ;
					}
				}
			}	
		}
	#endregion
	}
}


