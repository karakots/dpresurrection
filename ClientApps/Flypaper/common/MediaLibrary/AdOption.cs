using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MediaLibrary
{

    /// <summary>
    /// Helper class to maintain the AdOption database for the different types of AdOptions
    /// </summary>
    public class AdOptionDb
    {
        static public DpMediaDb MediaDb = null;
        static public void Initdb(string connectionStr)
        {

            if (MediaDb == null)
            {
                MediaDb = new DpMediaDb(connectionStr);
            }

            MediaDb.RefreshAdOptions();
        }
    }

    /// <summary>
    /// Base class for doing the actual selection and updating of the db
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AdOptionReader<T> where T : class, AdOption
    {
        public List<T> Select()
        {
            return AdOptionDb.MediaDb.ReadOptions<T>();
        }

        public void Update(T option)
        {
            AdOptionDb.MediaDb.UpdateAdOption(option);
        }
    }

    /// <summary>
    /// If you want to create a business object
    /// For a particular AdOption class derive it from the AdOptionReader
    /// </summary>
    public class SimpleAdOptionDb : AdOptionReader<SimpleOption> { }
    public class OnlineAdOptionDb : AdOptionReader<OnlineOption> { }
    public class SearchAdOptionDb : AdOptionReader<SearchOption> { }


    public interface AdOption
    {
        string MediaType { get; set; }
        string Name { get; }
        int ID { get; }

        double prob_of_ad(AgentData data, double hours, MediaComp media_comp);

        void modify_agent(AgentData data, double hours, MediaComp media_comp);

        void modify_agent(AgentData data, double hours, MediaComp media_comp, out MediaRecord record);

        //void set_id(int id);
    }

    [Serializable]
    public class SimpleOption : AdOption
    {
        public static Random rand = new Random();

        private string name;
        private string id;
        private double persuasion;
        private double recency;
        private double awareness;
        private double prob_per_hour;
        private double cost_modifier;
        private double consideration_prob_scalar;
        private double consideration_persuasion_scalar;
        private double consideration_awareness_scalar;
        private int db_id;

        [NonSerialized]
        private string type = null;

        // must inlcude default constructor for datasource
        public SimpleOption()
        {
            persuasion = 0.0;
            recency = 0.0;
            awareness = 0.0;
            prob_per_hour = 0.0;
            db_id = -1;
            id = "NOID";
            consideration_prob_scalar = 1.0;
            consideration_persuasion_scalar = 1.0;
            consideration_awareness_scalar = 1.0;
        }

        public SimpleOption(string Name, int ID, double Prob_Per_Hour, double Awareness, double Persuasion, double Recency, double Cost_Modifier, double Consideration_Prob_Scalar, double Consideration_Persuasion_Scalar, double Consideration_Awareness_Scalar)
        {
            name = Name;
            id = "NOID";
            db_id = ID;
            prob_per_hour = Prob_Per_Hour;
            awareness = Awareness;
            persuasion = Persuasion;
            recency = Recency;
            cost_modifier = Cost_Modifier;
            consideration_persuasion_scalar = Consideration_Persuasion_Scalar;
            consideration_awareness_scalar = Consideration_Awareness_Scalar;
            consideration_prob_scalar = Consideration_Prob_Scalar;
        }

        public override string ToString()
        {
            return Name;
        }

        public double Cost_Modifier
        {
            get
            {
                return cost_modifier;
            }
            set
            {
                cost_modifier = value;
            }
        }

        public double Prob_Per_Hour
        {
            get
            {
                return prob_per_hour;
            }
            set
            {
                prob_per_hour = value;
            }
        }

        public double Persuasion
        {
            get
            {
                return persuasion;
            }
            set
            {
                persuasion = value;
            }
        }

        public double Awareness
        {
            get
            {
                return awareness;
            }
            set
            {
                awareness = value;
            }
        }

        public double Recency
        {
            get
            {
                return recency;
            }
            set
            {
                recency = value;
            }
        }

        public double ConsiderationProbScalar
        {
            get
            {
                return consideration_prob_scalar;
            }
            set
            {
                consideration_prob_scalar = value;
            }
        }

        public double ConsiderationPersuasionScalar
        {
            get
            {
                return consideration_persuasion_scalar;
            }
            set
            {
                consideration_persuasion_scalar = value;
            }
        }

        public double ConsiderationAwarenessScalar
        {
            get
            {
                return consideration_awareness_scalar;
            }
            set
            {
                consideration_awareness_scalar = value;
            }
        }

        #region AdOption Members

        public string MediaType
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int ID
        {
            get
            {
                return db_id;
            }

            set
            {
                db_id = value;
            }
        }

        public string Tag
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }


        //public void set_id(int id)
        //{
        //    db_id = id;
        //}

        public virtual double prob_of_ad(AgentData data, double hours, MediaComp media_comp)
        {
            double q = 1 - EffectiveProb(data);
            return 1 - Math.Pow(q, hours);
        }

        public virtual void modify_agent(AgentData data, double hours, MediaComp media_comp)
        {
            data.AddImpression(media_comp.Type);
            if (data.Aware || rand.NextDouble() < EffectiveAwareness(data))
            {
                data.Aware = true;
                double effective_persuasion = EffectivePersuasion(data);
                data.Persuasion += effective_persuasion;
                if (rand.NextDouble() < Recency)
                {
                    data.Recency = 1;
                }
            }
        }

        public virtual void modify_agent(AgentData data, double hours, MediaComp media_comp, out MediaRecord record)
        {
            record = new MediaRecord();
            record.option = this;
            record.vehicle = media_comp.Guid;


            data.AddImpression(media_comp.Type);
            if (data.Aware || rand.NextDouble() < EffectiveAwareness(data))
            {
                record.MadeAware = true;
                data.Aware = true;
                double effective_persuasion = EffectivePersuasion(data);
                data.Persuasion += effective_persuasion;
                record.PersuasionConferred = effective_persuasion;
                if (rand.NextDouble() < Recency)
                {
                    data.Recency = 1;
                    record.MadeRecent = true;
                }
            }
        }

        #endregion

        public double EffectiveProb(AgentData data)
        {
            if (data.InConsideration)
            {
                return prob_per_hour;
            }
            else
            {
                return prob_per_hour * consideration_prob_scalar;
            }
        }

        public double EffectivePersuasion(AgentData data)
        {
            if (data.InConsideration)
            {
                return persuasion;
            }
            else
            {
                return persuasion * consideration_persuasion_scalar;
            }
        }

        public double EffectiveAwareness(AgentData data)
        {
            if (data.InConsideration)
            {
                return awareness;
            }
            else
            {
                return awareness * consideration_awareness_scalar;
            }
        }

    }

    [Serializable]
    public class OnlineOption : SimpleOption
    {
        public double Num_Impressions { get; set; }
        public double Click_Awaneness { get; set; }
        public double Click_Persuasion { get; set; }
        public double Action_Awareness { get; set; }
        public double Action_Persuasion { get; set; }
        public double Chance_Click { get; set; }
        public double Chance_Action { get; set; }

        public bool IsClick { get; set; }

        private int num_ads;


        // must inlcude default constructor for datasource
        public OnlineOption()
            : base()
        {
            Click_Awaneness = 0;
            Click_Persuasion = 0;
            Action_Awareness = 0;
            Action_Persuasion = 0;
            Chance_Click = 0;
            Chance_Action = 0;
            Num_Impressions = 0;
            IsClick = false;

            num_ads = 0;
        }

        public OnlineOption(string Name, int ID, double Awareness, double Persuasion, double Recency, double ClickAwareness, double ClickPersuasion, double ActionAwareness, double ActionPersuasion, double ChanceClick, double ChanceAction, double Cost_Modifier, double Consideration_Prob_Scalar, double Consideration_Persuasion_Scalar, double Consideration_Awareness_Scalar)
            : base(Name, ID, 0.0, Awareness, Persuasion, Recency, Cost_Modifier, Consideration_Prob_Scalar, Consideration_Persuasion_Scalar, Consideration_Awareness_Scalar)
        {
            Click_Awaneness = ClickAwareness;
            Click_Persuasion = ClickPersuasion;
            Action_Awareness = ActionAwareness;
            Action_Persuasion = ActionPersuasion;
            Chance_Click = ChanceClick;
            Chance_Action = ChanceAction;
            Num_Impressions = 0;
            IsClick = false;

            num_ads = 0;
        }

        public override void modify_agent(AgentData data, double hours, MediaComp media_comp)
        {
            for (double i = 0; i < num_ads; i++)
            {
                data.AddImpression(media_comp.Type);

                if (!IsClick)
                {
                    media_comp.Impressions--;
                }

                if (data.Aware || rand.NextDouble() < EffectiveAwareness(data))
                {
                    data.Aware = true;
                    data.Persuasion += EffectivePersuasion(data);
                    if (rand.NextDouble() < Recency)
                    {
                        data.Recency = 1;
                    }
                    if (rand.NextDouble() < Chance_Click)
                    {
                        data.Persuasion += Click_Persuasion;
                        if (rand.NextDouble() < Chance_Action)
                        {
                            data.Persuasion += Action_Persuasion;
                            i = data.num_online_impressions;
                        }
                        if (IsClick)
                        {
                            media_comp.Impressions--;
                        }
                    }
                }
            }
        }


        public override void modify_agent(AgentData data, double hours, MediaComp media_comp, out MediaRecord record)
        {
            record = new MediaRecord();
            record.option = this;
            record.vehicle = media_comp.Guid;

            for (int i = 0; i < num_ads; i++)
            {
                data.AddImpression(media_comp.Type);

                if (!IsClick)
                {
                    media_comp.Impressions--;
                }


                if (rand.NextDouble() < EffectiveAwareness(data))
                {
                    data.Aware = true;
                    record.MadeAware = true;
                }

                if (data.Aware)
                {
                    double effective_persuasion = EffectivePersuasion(data);
                    data.Persuasion += effective_persuasion;
                    record.PersuasionConferred = effective_persuasion;
                    if (rand.NextDouble() < Recency)
                    {
                        data.Recency = 1;
                        record.MadeRecent = true;
                    }
                    if (rand.NextDouble() < Chance_Click)
                    {

                        record.Clicked = true;
                        double click_persuasion = Click_Persuasion;
                        data.Persuasion += click_persuasion;
                        record.PersuasionConferred += click_persuasion;
                        if (rand.NextDouble() < Chance_Action)
                        {
                            record.Action = true;
                            double action_persuasion = Action_Persuasion;
                            data.Persuasion += action_persuasion;
                            record.PersuasionConferred += action_persuasion;
                            i = data.num_online_impressions;
                        }
                        if (IsClick)
                        {
                            media_comp.Impressions--;
                        }
                    }
                }
            }

        }

        public override double prob_of_ad(AgentData data, double hours, MediaComp media_comp)
        {
            //Replace this with actual ads per hour
            double ads_per_hour = 2;


            double ads = PoissonRandomNumber(hours * ads_per_hour);
            ads = Math.Min(ads, media_comp.Impressions);

            num_ads = (int)ads;

            return ads;
        }

        public static double PoissonRandomNumber(double lambda)
        {
            double k = 0;                          //Counter
            int max_k = 1;           //k upper limit
            double p = rand.NextDouble(); //uniform random number
            double P = Math.Exp(-lambda);          //probability
            double sum = P;                     //cumulant
            if (sum >= p)
            {
                return 0;             //done allready
            }
            for (k = 1; k < max_k; ++k)
            {
                //Loop over all k:s
                P *= lambda / k;            //Calc next prob
                sum += P;                   //Increase cumulant
                if (sum >= p)
                {
                    break;                  //Leave loop
                }
            }

            return k;                         //return random number
        }
    }

    public class SearchOption : OnlineOption
    {

        // must inlcude default constructor for datasource
        public SearchOption()
            : base()
        {
        }

        public SearchOption(string Name, int ID, double Awareness, double Persuasion, double Recency, double ClickAwareness, double ClickPersuasion, double ActionAwareness, double ActionPersuasion, double ChanceClick, double ChanceAction, double Cost_Modifier, double Consideration_Prob_Scalar, double Consideration_Persuasion_Scalar, double Consideration_Awareness_Scalar) :
            base(Name, ID, Awareness, Persuasion, Recency, ClickAwareness, ClickPersuasion, ActionAwareness, ActionPersuasion, ChanceClick, ChanceAction, Cost_Modifier, Consideration_Prob_Scalar, Consideration_Persuasion_Scalar, Consideration_Awareness_Scalar)
        {

        }

        public override double prob_of_ad(AgentData data, double hours, MediaComp media_comp)
        {
            double q = 1 - EffectiveProb( data );
            return 1 - Math.Pow( q, hours );
        }

    }
}
