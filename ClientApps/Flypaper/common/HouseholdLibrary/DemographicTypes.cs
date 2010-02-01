using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace HouseholdLibrary
{

    [Serializable]
    public class DemographicType<T> where T : DemoType, new() {
        private uint bits = 0;

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

            uint mask = 1;
            int index = 1;
            while( (bits & mask) == 0 ) {
                mask = mask << 1;
                index++;
            }

            if( index > 1 && index < vals.Length ) {
                return vals[ index - 1 ] + 1;
            }

            return vals[ 0 ];
        }


        public int Max() {
            T tmp = new T();

            int[] vals = tmp.GetValues();

            if( vals == null || vals.Length < 2 ) {
                throw new Exception( "No values for type" );
            }

            int index = vals.Length - 1;
            uint mask = (uint)Math.Pow( 2, index - 1 );
            while( (bits & mask) == 0 ) {
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

            uint bits = 0;
            for( int index = 0; index < values.Length; ++index ) {
                if( index == 0 ) {
                    if( val < values[ index ] ) {
                        return new DemographicType<T>( 0 );
                    }

                    bits = 1;
                }
                else { 
                    if( val <= values[ index ] ) {
                        return new DemographicType<T>( bits );
                    }

                    bits *= 2;
                }
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
        static int[] vals = new int[] { 0, 17, 25, 35, 45, 55, 65, 100 };

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
                "Income50Kto75K",
                "Incone75Kto100K",
                "Income100Kto150K",
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