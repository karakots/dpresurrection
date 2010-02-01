using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

using DemographicLibrary;

namespace HouseholdLibrary
{
    [Serializable]
    public class Person
    {
        private DemographicType<Gender> gender;
        private DemographicType<Race> race;
        private int age;

        public DemographicType<Gender> Gender { get { return gender; } }
        public DemographicType<Race> Race { get { return race; } }
        public int Age { get{return age;}}

        public Person(string gender, string race, int age)
        {
            this.gender = gender;
            
            this.age = age;
            try {
                this.race = race;
            }
            catch( Exception ) {
                this.race = "OTHER";
            }
        }

        public Person( DemographicType<Gender> gender, DemographicType<Race> race, int age ) {
              
            this.gender = gender;
            this.race = race;
            this.age = age;
        }

        public Person()
        {
            gender = "MALE";
            race = "WHITE";
            age = 25;
        }

        /// <summary>
        /// demographic match to a person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public bool Match(Demographic demographic)
        {
            if (!(Gender & demographic.Gender))
            {
                return false;
            }

            if (!(Race & demographic.Race))
            {
                return false;
            }

            if (Age < 18 && !(demographic.Kids & "YES"))
            {
                return false;
            }

            DemographicType<Age> personAge = DemographicType<Age>.FromValue(Age);
            if (!(personAge & demographic.Age))
            {
                return false;
            }

            return true;
        }

        
    }

    [Serializable]
    public class HouseholdMedia
    {
        private Guid guid;
        private Double rate;

        public Guid Guid { get { return guid; } }
        public Double Rate { get { return rate; } set { rate = value; } }

        public HouseholdMedia(Guid guid, Double rate)
        {
            this.guid = guid;
            this.rate = rate;
        }

    }

    [Serializable]
    public class Household
    {
        private int income;
        private int size;
        private DemographicType<HouseholdStatus> type;
        private DemographicType<Race> race;
        private List<Person> occupants;
        private Dictionary<int, List<HouseholdMedia>> mediaDict;
        private Guid guid;
        private int geo_id;

        public DemographicType<Race> Race { get { return race; } }
        public int Size { get { return size; } }
        public int Income { get { return income; } }
        public DemographicType<HouseholdStatus> Type { get { return type; } }
        public Guid Guid {get{return guid;}}
        public int GeoID {get{return geo_id;}}
        public List<Person> People { get { return occupants; } }

        public bool HasMedia( int media_type )
        {

            if( mediaDict == null || !mediaDict.ContainsKey( media_type ) )
            {
                return false;
            }

            return true;
        }


        public List<HouseholdMedia> Media( int media_type )
        {
            if( !HasMedia( media_type ) )
            {
                return new List<HouseholdMedia>();
            }

            return mediaDict[media_type];
        }

        public void ClearMedia( int media_type )
        {
            if( mediaDict == null )
            {
                return;
            }

            if( mediaDict.ContainsKey( media_type ) )
            {
                mediaDict[media_type].Clear();
            }
        }

        public void AddMedia( int type, HouseholdMedia hhMedia )
        {
            if( mediaDict == null )
            {
                mediaDict = new Dictionary<int, List<HouseholdMedia>>();
            }

            if( !mediaDict.ContainsKey( type ) )
            {
                mediaDict.Add( type, new List<HouseholdMedia>() );
            }

            mediaDict[type].Add( hhMedia );
        }

        public Household(int geo_id, string race, int size, string type, int income)
        {
            guid = Guid.NewGuid();
            this.geo_id = geo_id;
            this.income = income;
            this.type = type;
            try
            {
                this.race = race;
            }
            catch
            {
                this.race = "OTHER";
            }
            this.size = size;
            occupants = new List<Person>();
            mediaDict = new Dictionary<int, List<HouseholdMedia>>();
        }

        public Household( int geo_id, DemographicType<Race> race, int size, DemographicType<HouseholdStatus> type, int income ) {
            guid = Guid.NewGuid();
            this.geo_id = geo_id;
            this.income = income;
            this.type = type;
            this.race = race;
            this.size = size;
            occupants = new List<Person>();
            mediaDict = new Dictionary<int, List<HouseholdMedia>>();
        }

        public void AddPerson(Person person)
        {
            occupants.Add(person);
        }

        public int NumOccupants()
        {
            return occupants.Count;
        }

        /// <summary>
        /// Is anyone in this house contained in this demographic
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public bool Match(Demographic demographic)
        {
            bool match = false;

            foreach (Person person in People)
            {
                if (person.Match(demographic))
                {
                    match = true;
                    break;
                }
            }

            if (!match)
            {
                return false;
            }

            // if we must have kids we need to check if this is true
            if (demographic.Kids > 0 && demographic.Kids <= "YES")
            {
                match = false;
                foreach (Person person in People)
                {
                    if (person.Age < 18)
                    {
                        match = true;
                        break;
                    }
                }
            }

            if (!match)
            {
                return false;
            }

            match = false;
            if ((Type & "NONFAMILY") && (demographic.Homeowner & "RENTER"))
            {
                match = true;
            }
            if ((Type & "FAMILY") && (demographic.Homeowner & "OWNER"))
            {
                match = true;
            }

            if (!match)
            {
                return false;
            }

            DemographicType<Income> houseIncome = DemographicType<Income>.FromValue(Income);

            if (!(houseIncome & demographic.Income))
            {
                return false;
            }

   
            if (!match)
            {
                return false;
            }

            // so close...
            return match;
        }

        public void Print( TextWriter writer, string prefix )
        {
            writer.WriteLine( prefix + "House Details" );
            writer.WriteLine( prefix + "ID, " + Guid.ToString() );
            writer.WriteLine( prefix + "RACE, " + Race.ToString() );
            writer.WriteLine( prefix + "INCOME, " + Income.ToString() );
            writer.WriteLine( prefix + "TYPE, " + Type.ToString() );
            writer.WriteLine( prefix + "SIZE, " + Size.ToString() );
            for( int i = 0; i < People.Count; i++ )
            {
                writer.WriteLine( prefix + "PERSON " + (i + 1).ToString() + " DETAILS" );
                writer.WriteLine( prefix + "RACE, " + People[i].Race.ToString() );
                writer.WriteLine( prefix + "AGE, " + People[i].Age.ToString() );
                writer.WriteLine( prefix + "GENDER, " + People[i].Gender.ToString() );
            }

            writer.WriteLine( prefix + "Media Details" );
            foreach(int type in mediaDict.Keys )
            {
                 writer.WriteLine( prefix + "Media Type, " + type.ToString());
                foreach(HouseholdMedia media in mediaDict[type])
                {
                    writer.WriteLine( prefix + "VehicleID, " + media.Guid.ToString());
                    writer.WriteLine( prefix + "Rate, " + media.Rate.ToString() );
                }
            }
        }
    }
   
}
