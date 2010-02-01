using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace DemographicLibrary
{
    [Serializable]
    public class DemographicType<T> where T : DemoType, new() {
        public uint bits = 0;

        /// <summary>
        /// Constructor generates the type from the uint
        /// </summary>
        /// <param name="bitsIn"></param>
        protected DemographicType( uint bitsIn ) {
            bits = bitsIn;
        }

        /// <summary>
        /// Builds the NA type
        /// </summary>
        public DemographicType() {
            bits = 0;
        }

        /// <summary>
        /// Clears bits from type
        /// </summary>
        public void Clear() {
            bits = 0;
        }

        /// <summary>
        /// Adds the argument type to the current type
        /// </summary>
        /// <param name="addIn"></param>
        public void Add(DemographicType<T> addIn) {
            
            bits |= addIn.bits;
        }

        #region bitwise operations
        public static implicit operator bool( DemographicType<T> a ) {
            return a.bits > 0;
        }

        static public bool operator <( DemographicType<T> a, DemographicType<T> b ) {
            return (~a.bits & b.bits) > 0;
        }

        static public bool operator >( DemographicType<T> a, DemographicType<T> b ) {
            return (a.bits & ~b.bits) > 0;
        }

        static public bool operator >=( DemographicType<T> a, DemographicType<T> b ) {
            return (~a.bits & b.bits) == 0;
        }

        static public bool operator <=( DemographicType<T> a, DemographicType<T> b ) {
            return (a.bits & ~b.bits) == 0;
        }

        static public DemographicType<T> operator &( DemographicType<T> a, DemographicType<T> b ) {
            return new DemographicType<T>( a.bits & b.bits );
        }

        static public DemographicType<T> operator -( DemographicType<T> a, DemographicType<T> b ) {
            return new DemographicType<T>( a.bits & ~b.bits );
        }

        static public DemographicType<T> operator |( DemographicType<T> a, DemographicType<T> b ) {
            return new DemographicType<T>( a.bits | b.bits );
        }
        #endregion

        #region basic types
        // This is the ANY type that can be used
       
        static public DemographicType<T> ANY {
            get {
                return ComputeAny();
            }
        }

        // in case you need it this represents NO type
        static public DemographicType<T> NULL
        {
            get
            {
                return new DemographicType<T>();
            }
        }


        private static uint mask = 0;
        private static DemographicType<T> ComputeAny() {

            if( mask > 0 ) {
                return new DemographicType<T>( mask );
            }

            T tmp = new T();

            int num = tmp.GetNames().Length;

            mask = (uint)Math.Pow( 2, num ) - 1;

            return new DemographicType<T>( mask );
        }
        #endregion

        #region conversions

        /// <summary>
        /// Not really conversion per se
        /// </summary>
        /// <returns></returns>
        public override string ToString() {

            if( this >= ANY ) {
                return "ANY";
            }

            if( bits == 0 ) {
                return "NULL";
            }

            T tmp = new T();

            // check for values
            int[] vals = tmp.GetValues();

            if( vals != null ) {
                return Min().ToString() + "to" + Max().ToString();
            }

            string[] names = tmp.GetNames();

            if( names == null || names.Length == 0 ) {
                return "Error in Type";
            }

            string rval = "";

            uint mask = 1;
            for( int index = 0; index < names.Length; ++index, mask = mask << 1 ) {
                if( (bits & mask) > 0 ) {
                    if( rval != "" ) {
                        rval += ", ";
                    }

                    rval += names[ index ];
                }
            }

            return rval;
        }

        public string PCString()
        {
            string rval = this.ToString();

            if( rval == "ANY" )
            {
                rval = "NA";
            }
            else if (rval == "NULL")
            {
                rval = "NA";
            }

            rval = rval.Replace( "BLACK", "AFRICAN AMERICAN" );


            return rval;
        }

        /// <summary>
        /// Convert bits to a type
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static implicit operator DemographicType<T>( uint  bits) {

            return new DemographicType<T>( bits);
        }

        /// <summary>
        /// Convert type to bits
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static implicit operator uint (DemographicType<T> val) {

            return val.bits;
        }


        // convert from string
        static public implicit operator DemographicType<T>( string val ) {

            if( val.CompareTo( "NA" ) == 0 ) {
                return ANY;
            }

            if( val.CompareTo( "ANY" ) == 0 ) {
                return ANY;
            }

            T tmp = new T();

            string[] names = tmp.GetNames();

            uint bits = 1;
            foreach( string name in names ) {
                if( name.CompareTo( val ) == 0 ) {
                    return new DemographicType<T>( bits );
                }
                bits *= 2;
            }

            return new DemographicType<T>( 0 );
        }
        #endregion

        #region value computations

        /// <summary>
        /// For types that have actual values
        /// Adds the range of values to the type
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void AddRange( int min, int max ) {

            if( max < min )
            {
                return;
            }

            T tmp = new T();

            int[] vals = tmp.GetValues();

            if( min < vals[0] )
            {
                min = vals[0];
            }

            if( max > vals[vals.Length - 1] )
            {
                max = vals[vals.Length - 1];
            }

            int val = min;
            while( val <= max ) {
                DemographicType<T> type = DemographicType<T>.FromValue( val );
                Add( type );

                val = type.Max() + 1;
            }
        }

        public int Min() {

            T tmp = new T();

            int[] vals = tmp.GetValues();

            if( vals == null || vals.Length < 2 ) {
                throw new Exception( "No values for type" );
            }

            if( bits == 0 )
            {
                return vals[0];
            }

            uint mask = 1;
            int index = 1;
            while( (bits & mask) == 0 && index < vals.Length )
            {
                mask = mask << 1;
                index++;
            }

            if( index > 1 && index < vals.Length ) {
                return vals[ index - 1 ] + 1;
            }

            return vals[ 0 ];
        }

        /// <summary>
        /// returns first value >= all encoded values (LUB)
        /// </summary>
        /// <returns></returns>
        public int Max() {
            T tmp = new T();

            int[] vals = tmp.GetValues();

            if( vals == null || vals.Length < 2 ) {
                throw new Exception( "No values for type" );
            }

            if( bits == 0 )
            {
                return vals[vals.Length - 1];
            }

            // note we do not count the lowest value as an actual type value
            int index = vals.Length - 1;
            uint mask = (uint)Math.Pow( 2, index - 1 );
            while( (bits & mask) == 0 && index > 0) {
                mask = mask >> 1;
                index--;
            }

            if( index > 0 ) {
                return vals[ index ];
            }

            return vals[ vals.Length - 1 ];
        }

        /// <summary>
        /// only works on types that have a value array
        /// return the first type greater or equal to value
        /// if val is < min or > max then returns the NULL type
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DemographicType<T> FromValue( int val ) {
            T tmp = new T();

            int[] values = tmp.GetValues();

            // no values or we are out of range -- just return no type
            if( values == null  || values.Length < 2 ) {
                return new DemographicType<T>(0);
            }

            if( val < values[0] )
            {
                return new DemographicType<T>( 1 );
            }

            if( val > values[values.Length - 2] )
            {
                return new DemographicType<T>( (uint)Math.Pow( 2, values.Length - 2 ) );
            }

            uint bits = 1;
            for( int index = 1; index < values.Length - 1; ++index )
            {
                if( val <= values[index] )
                {
                    return new DemographicType<T>( bits );
                }

                bits *= 2;
            }

            return new DemographicType<T>(0);
        }

        #endregion


    }

    [Serializable]
    abstract public class DemoType
    { 
        abstract public string[] GetNames(); // { return names; }

        /// <summary>
        /// Values should have length one more then names
        /// the first item in array is the minimum possible value
        /// The last is the highest possible value
        /// Each value above the lowest should correspond to a name
        /// and represent the maximum value for that name
        /// </summary>
        /// <returns></returns>
        virtual public int[] GetValues() {
            return null;
        }
    }

    [Serializable]
    public class Gender : DemoType
    {
        static string[] names = new string[] { "MALE", "FEMALE" };
        public override string[] GetNames() {
            return names;
        }
    }

    [Serializable]
    public class Race : DemoType
    {
        static string[] names = new string[] {
                "WHITE",
                "BLACK",
                "LATINO",
                "ASIAN",
                "OTHER"
            };

        public override string[] GetNames() {
            return names;
        }
    }

    [Serializable]
    public class HomeOwner : DemoType
    {
        static string[] names = new string[] {
            "OWNER", 
            "RENTER"
            };

        public override string[] GetNames() {
            return names;
        }
    }

    [Serializable]
    public class ChildStatus : DemoType
    {
        static string[] names = new string[] {
                "YES",
                "NO"
            };
        public override string[] GetNames() {
            return names;
        }
    }

    [Serializable]
    public class HouseholdStatus : DemoType
    {
        static string[] names = new string[] {
                "FAMILY",
                "NONFAMILY"
            };
        public override string[] GetNames() {
            return names;
        }
    }

    [Serializable]
    public class Age : DemoType
    {
        static string[] names = new string[] { "Under18", "18to25", "26to35", "36to45", "46to55", "56to65", "Over65" };
        static int[] vals = new int[] { 0, 17, 25, 35, 45, 55, 65, 100};

        public override string[] GetNames() {
            return names;
        }

        public override int[] GetValues() {
            return vals;
        }
    }

    [Serializable]
    public class Income : DemoType
    {
        static string[] names = new string[] {
                "Under50K",
                "50Kto75K",
                "75Kto100K",
                "100Kto150K",
                "Over150K"};
        static int[] vals = new int[] { 0, 50000, 75000, 100000, 150000, 1000000 };


        public override string[] GetNames() {
            return names;
        }

        public override int[] GetValues() {
            return vals;
        }
    }
}