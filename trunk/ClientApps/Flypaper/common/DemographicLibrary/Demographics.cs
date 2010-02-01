using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemographicLibrary
{
    [Serializable]
    public class Demographic
    {
        // primitive demographics
        public static DemoType[] DemoTypes = new DemoType[] { new Gender(), new Race(), new HomeOwner(), new ChildStatus(), new Age(), new Income() };

        public static List<Demographic> PrimitiveTypes = CreatePrimitiveTypes();
        private static List<Demographic> CreatePrimitiveTypes()
        {
            List<Demographic> rval = new List<Demographic>();

            DemoType[] demoTypes = Demographic.DemoTypes;

            int numDex = demoTypes.Length;
            int[] index = new int[numDex];
            string[] types = new string[numDex];

            // multi index scheme - selecting one from each possibility

            // initialize
            for( int ii = 0; ii < numDex; ++ii )
            {
                index[ii] = 0;
            }

            bool done = false;

            while( !done )
            {
                // normally one might check that we can do the initial computation as in a for loop
                // but we know we can so we forego the pleasure


                // compute
                for( int ii = 0; ii < numDex; ++ii )
                {
                    types[ii] = demoTypes[ii].GetNames()[index[ii]];
                }

                Demographic prim = new Demographic( types );
                rval.Add( prim );

                // increment
                // I have used htis algorithm before and it is somewhat suprising that it works
                done = true;
                for( int ii = 0; ii < numDex; ++ii )
                {
                    ++index[ii];

                    if( index[ii] < demoTypes[ii].GetNames().Length )
                    {
                        done = false;
                        break;
                    }
                    else
                    {
                        index[ii] = 0;
                    }
                }
            }

            return rval;
        }

        private static List<double> zeroSize = null;
        private static List<double> newPrimitiveSize()
        {
            if (zeroSize == null)
            {
                zeroSize = new List<double>(PrimitiveTypes.Count);
                // initialize zeros
                for( int ii = 0; ii < PrimitiveTypes.Count; ++ii )
                {
                    zeroSize.Add( 0.0 );
                }
            }

            return new List<double>(zeroSize);
        }

        [Serializable]
        public class PrimVector : List<double>
        {
            public double Any { get; set; }

            public PrimVector() : base(newPrimitiveSize())
            {
                Any = 0;
            }

            public void Normalize()
            {
                double total = 0;

                for( int ii = 0; ii < Count; ++ii )
                {
                    total += this[ii];
                }

                for( int ii = 0; ii < Count; ++ii )
                {
                    this[ii] *= this.Any / total;
                }
            }

            /// <summary>
            /// Takes Dot product
            /// Does not take into account Any
            /// </summary>
            /// <param name="A"></param>
            /// <param name="B"></param>
            /// <returns></returns>
            static public double operator *( PrimVector A, PrimVector B )
            {
                double rval = 0;
                int num = Math.Min( A.Count, B.Count );

                for( int ii = 0; ii < num; ++ii )
                {
                    rval += A[ii] * B[ii];
                }

                return rval;
            }

        }

        private DemographicType<Age> age = new DemographicType<Age>();
        private DemographicType<Income> income = new DemographicType<Income>();
        private DemographicType<Gender> gender = new DemographicType<Gender>();
        private DemographicType<Race> race = new DemographicType<Race>();
        private DemographicType<ChildStatus> kids = new DemographicType<ChildStatus>();
        private DemographicType<HomeOwner> homeowner = new DemographicType<HomeOwner>();

        public string Name { get; set; }

        public string Region = "";


        public uint[] GetDemographicBase()
        {
            uint[] rval = new uint[Demographic.DemoTypes.Count()];

            rval[0] = gender;
            rval[1] = age;
            rval[2] = race;
            rval[3] = income;
            rval[4] = kids;
            rval[5] = income;

            return rval;
        }

        #region operations on type

        public DemographicType<Gender> Gender
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
            }
        }
        public DemographicType<Race> Race
        {
            get
            {
                return race;
            }
            set
            {
                race = value;
            }
        }
        public DemographicType<HomeOwner> Homeowner
        {
            get
            {
                return homeowner;
            }
            set
            {
                homeowner = value;
            }
        }

        public DemographicType<ChildStatus> Kids
        {
            get
            {
                return kids;
            }
            set
            {
                kids = value;
            }
        }

        public DemographicType<Age> Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }

        public DemographicType<Income> Income
        {
            get
            {
                return income;
            }
            set
            {
                income = value;
            }
        }

        #endregion


        public void AddAge(int anAge)
        {
            age.AddRange(anAge, anAge);
        }

        public void AddAge(int minAge, int maxAge)
        {
            age.AddRange(minAge, maxAge);
        }


        public void AddIncome(int anIncome)
        {
            income.AddRange(anIncome, anIncome);
        }
        public void AddIncome(int minIncome, int maxIncome)
        {
            income.AddRange(minIncome, maxIncome);
        }

        public int AgeMin
        {
            get
            {
                return age.Min();
            }
        }


        public int AgeMax
        {
            get
            {
                return age.Max();
            }
        }

        public int IncomeMin
        {
            get
            {
                return income.Min();
            }
        }

        public int IncomeMax
        {
            get
            {
                return income.Max();
            }
        }

       
        private void init(string[] types)
        {
            if (types.Length != DemoTypes.Length)
            {
                throw new Exception("wrong initilizer to demographic");
            }

            // this assumed to be in the same order as above
            Name = types[0] + ":" + types[1] + ":" + types[2] + ":" + types[3] + ":" + types[4] + ":" + types[5];

            Gender = types[0];
            Race = types[1];
            Homeowner = types[2];
            Kids = types[3];
            Age = types[4];
            Income = types[5];
        }


        public Demographic(string[] types)
        {

            init(types);

        }

        public Demographic(string basicType)
        {
            string[] types = basicType.Split(':');

            init(types);
        }

        public Demographic()
        {
            Name = "Everybody";
            Age = DemographicType<Age>.ANY;
            Income = DemographicType<Income>.ANY;
            Gender = DemographicType<Gender>.ANY;
            Race = DemographicType<Race>.ANY;
            Homeowner = DemographicType<HomeOwner>.ANY;
            Kids = DemographicType<ChildStatus>.ANY;
        }

        public Demographic(Demographic dem)
        {
            Name = dem.Name;
            Age = dem.age;
            income = dem.income;
            Gender = dem.gender;
            Race = dem.race;
            Homeowner = dem.homeowner;
            Kids = dem.kids;
        }

        

        /// <summary>
        /// Is this demographic a subset of the given demographic
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public bool IsSubsetOf(Demographic demographic)
        {
            if (Race > demographic.Race)
            {
                return false;
            }

            if (Gender > demographic.Gender)
            {
                return false;
            }

            if (Age > demographic.Age)
            {
                return false;
            }

            if (Income > demographic.Income)
            {
                return false;
            }

            if (Homeowner > demographic.Homeowner)
            {
                return false;
            }

            if (Kids > demographic.Kids)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            string ret_val = Gender.ToString() + ":";
            ret_val += Race.ToString() + ":";
            ret_val += Age.ToString() + ":";
            ret_val += Income.ToString() + ":";
            ret_val += Homeowner.ToString() + ":";
            ret_val += Kids.ToString();

            return ret_val;
        }

        /// <summary>
        /// Generates a zero - one primitive vector
        /// </summary>
        /// <returns></returns>
        public PrimVector ToPrimVector()
        {
            PrimVector rval = new PrimVector();

            for( int ii = 0; ii < PrimitiveTypes.Count; ++ii )
            {
                if( Intersects(PrimitiveTypes[ii], this) )
                {
                    rval[ii] = 1;
                }
            }


            return rval;
        }

        #region Demographic Operators
        // OR of demographics
        static public Demographic operator |( Demographic a, Demographic b )
        {
            Demographic rval = new Demographic();

            rval.age = a.age | b.age;
            rval.income = a.income | b.income;
            rval.gender = a.gender | b.gender;
            rval.race = a.race | b.race;
            rval.homeowner = a.homeowner | b.homeowner;
            rval.kids = a.kids | b.kids;

            return rval;
        }

        static public bool Intersects( Demographic a, Demographic b )
        {
            if ( !(a.age & b.age) ) return false;
            if ( !( a.income & b.income) ) return false;
            if ( !( a.gender & b.gender) ) return false;
            if ( !( a.race & b.race) ) return false;
            if ( !( a.homeowner & b.homeowner) ) return false;
            if ( !( a.kids & b.kids) ) return false;

            return true;
        }

        #endregion
    }
}
