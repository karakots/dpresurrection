using System;
using System.Collections.Generic;
using System.Text;

namespace GeoLibrary
{
    [Serializable]
    public struct BoundingBox
    {
        public Coordinate Max;
        public Coordinate Min;

        public void Reset()
        {
            Max.x = double.MinValue;
            Max.y = double.MinValue;
            Min.x = double.MaxValue;
            Min.y = double.MaxValue;
        }

        public void Add( BoundingBox next )
        {

            Min.x = Math.Min( Min.x, next.Min.x );
            Min.y = Math.Min( Min.y, next.Min.y );

            Max.x = Math.Max( Max.x, next.Max.x );
            Max.y = Math.Max( Max.y, next.Max.y );
        }

        public void Add( Coordinate point )
        {

            if( Min.x > point.x )
            {
                Min.x = point.x;
            }
            if( Max.x < point.x )
            {
                Max.x = point.x;
            }
            if( Min.y > point.y )
            {
                Min.y = point.y;
            }
            if( Max.y < point.y )
            {
                Max.y = point.y;
            }
        }
    }

    [Serializable]
    abstract public class Shape
    {
        public static double maxX = 1;
        public static double maxY = 1;
        public static double minX = 1;
        public static double minY = 1;

        public GeoRegion Region = null;

        public string Name;

        public BoundingBox Box = new BoundingBox();
      
        public abstract void ResetBoundaries();
    }


    /// <summary>
    /// This should be used only for visualization
    /// TBD We need to marry this with the
    /// geo structures
    /// </summary>
    [Serializable]
    public class Country : Shape
    {
        public static object MapObj = null;
       

        public Dictionary<string, State> States;

        public Country(string name)
        {
            Name = name;

            States = new Dictionary<string, State>();
        }

        public override void ResetBoundaries()
        {
            Box.Reset();

            foreach( State state in States.Values )
            {

                state.ResetBoundaries();


                Box.Add( state.Box );
            } 
        }
    }

    [Serializable]
    public class State : Shape
    {
        public string ID;

        public Dictionary<string, County> Counties;

        public State( string name, string id )
        {
            this.Name = name;
            this.ID = id;

            Counties = new Dictionary<string, County>();
        }

        public override void ResetBoundaries()
        {
            Box.Reset();

            foreach( County county in Counties.Values )
            {
                county.ResetBoundaries();

                Box.Add( county.Box );
            }
        }
    }

    [Serializable]
    public class County : Shape
    {
        public string ID;

        public string Type;

        public Dictionary<int, Section> Sections;

        public County(string name, string type, string id)
        {
            Sections = new Dictionary<int, Section>();
            this.Name = name;
            this.Type = type;
            this.ID = id;
        }

        public override void ResetBoundaries()
        {
            Box.Reset();

            foreach( Section section in Sections.Values )
            {
                foreach( Coordinate point in section.Coordinates )
                {
                    Box.Add( point );
                }
            }
        }
    }

    [Serializable]
    public class Section
    {
        public int ID;

        public List<Coordinate> Coordinates;

        public Section(int id)
        {
            Coordinates = new List<Coordinate>();
            ID = id;
        }
    }

    [Serializable]
    public struct Coordinate
    {
        public double x;
        public double y;

        public Coordinate(double x_arg, double y_arg)
        {
            x = x_arg;
            y = y_arg;
        }

        public void Offset(double x_off, double y_off)
        {
            x += x_off;
            y += y_off;
        }

        public double Dist( Coordinate p )
        {
            return Math.Sqrt( (p.x - x) * (p.x - x) + (p.y - y) * (p.y - y) );
        }
    }
}
