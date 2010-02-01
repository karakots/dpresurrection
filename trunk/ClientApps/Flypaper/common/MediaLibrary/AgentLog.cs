using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using HouseholdLibrary;
using System.IO;

namespace MediaLibrary
{
    [Serializable]
    public class DailyLog
    {
        public int Day = 0;
        public double Persuasion = 0.0;

        public bool Aware = false;

        public double Recency = 0.0;

        public bool in_consideration = false;

        public double ActionTaken = 0.0;
        public bool MadeChoice = false;
        public int NumImpressions = 0;

        public List<MediaRecord> media = new List<MediaRecord>();

        public DailyLog(int day)
        {
            Day = day;
        }

        public bool Changed( AgentData data )
        {
            if( Aware == data.Aware &&
                in_consideration == data.InConsideration &&
                ActionTaken == data.ActionTaken &&
                MadeChoice == data.MadeChoice &&
                NumImpressions == data.NumImpressions() )
            {
                return false;
            }

            return true;
        }

        public void Update( AgentData data )
        {
            Persuasion = data.Persuasion;
            Aware = data.Aware;
            Recency = data.Recency;
            in_consideration = data.InConsideration;
            ActionTaken = data.ActionTaken;
            MadeChoice = data.MadeChoice;
            NumImpressions = data.NumImpressions();
        }

        public void Update(MediaRecord record )
        {
            media.Add( record );
        }

        public void Print( TextWriter writer, string prefix )
        {  
            writer.WriteLine( "DAY: " + Day.ToString() + " Media Details" );
            foreach(MediaRecord record in media)
            {
                record.Print(writer, prefix);
            }

            writer.WriteLine( "DAY: " + Day.ToString() + " Final State" );
            writer.WriteLine( prefix + "Persuasion: " + Persuasion.ToString());
            writer.WriteLine( prefix + "Recency: " + Recency.ToString() );
            writer.WriteLine( prefix + "Aware: " + Aware.ToString());
            writer.WriteLine( prefix + "MadeChoice: " + MadeChoice.ToString() );
            writer.WriteLine( prefix + "NumImpressions: " + NumImpressions.ToString() );

            writer.WriteLine( prefix );

        }
    }

    [Serializable]
    public class AgentLog
    {
        [Serializable]
        public class Summary
        {
            public Agent Agent { get; set; }
            public double Persuasion { get; set; }

            public double Aware { get; set; }

            public double Recency { get; set; }

            public double Consideration { get; set; }

            public double ActionTaken { get; set; }
            public double MadeChoice { get; set; }
            public double NumImpressions { get; set; }

            public Summary()
            {
                Agent = null;
                Persuasion = 0.0;
                Aware = 0.0;
                Recency = 0.0;
                Consideration = 0.0;
                ActionTaken = 0.0;
                MadeChoice = 0.0;
                NumImpressions = 0.0;
            }
        }

        public Agent agent = null;
        public Dictionary<int, DailyLog> Logs = new Dictionary<int, DailyLog>();

        public AgentLog(Agent agentIn)
        {
            agent = agentIn;
        }

        [NonSerialized]
        private DailyLog lastLog = new DailyLog( 0 );

        private DailyLog createLog( int day )
        {
            if( Logs.ContainsKey( day ) )
            {
                return Logs[day];
            }

            lastLog = new DailyLog( day );

            Logs.Add( day, lastLog );

            return lastLog;
        }

        public void AddRecord(int day, MediaRecord record)
        {
            DailyLog log = createLog( day );

            log.Update( record );
        }

        public void UpdateState(int day, AgentData data)
        {
            if( lastLog.Day == day || lastLog.Changed( data ) )
            {
                DailyLog log = createLog( day );

                log.Update( data );
            }
            else
            {
                lastLog.Day = day;
            }
        }

        public Summary ComputeSummary()
        {
           Summary summary = new Summary();

            double last_day = 0;

            foreach( int day in Logs.Keys )
            {
                last_day = Logs[day].Day;
                double span = (1 + last_day - day);

                summary.Persuasion += span * Logs[day].Persuasion;
                summary.Aware += span * (Logs[day].Aware ? 1 : 0);
                summary.Recency += span * Logs[day].Recency;
                summary.Consideration += span *(Logs[day].in_consideration ? 1 : 0);
                summary.NumImpressions += span * Logs[day].NumImpressions;

                // happens only on one day
                summary.ActionTaken += Logs[day].ActionTaken;
                summary.MadeChoice += (Logs[day].MadeChoice ? 1 : 0);
                
            }

            if( last_day > 0 )
            {
                double scale = 1.0 / last_day;

                summary.Persuasion *= scale;
                summary.Aware *= scale;
                summary.Recency *= scale;
                summary.Consideration *= scale;
              
                summary.NumImpressions *= scale;
            }

            if( summary.MadeChoice > 0 )
            {
                // action is taken when a consumer make a choice
                double scale = 1.0 / summary.MadeChoice;
                summary.ActionTaken *= scale;
            }

            summary.Agent = agent;

            return summary;
        }

        public void PrintAgentData( TextWriter writer, string prefix )
        {
            agent.Print( writer, prefix );
        }

        public void PrintDiary( TextWriter writer, string prefix )
        {
            writer.WriteLine( "AGENT DIARY" );

            foreach( DailyLog log in Logs.Values )
            {
                log.Print( writer, prefix );
            }
        }

        public void Print(TextWriter writer)
        {
            PrintAgentData( writer, "" );
            PrintDiary( writer, "" );
        }
    }

    [Serializable]
    public class MediaRecord
    {
        public int day = 0;
        public Guid vehicle = new Guid();
        public AdOption option = null;
        public bool MadeAware = false;
        public double PersuasionConferred = 0.0;
        public bool MadeRecent = false;
        public bool Clicked = false;
        public bool Action = false;

        public void Print(TextWriter writer, string prefix)
        {
            writer.WriteLine(prefix + "VEHICLE DETAILS");
            writer.WriteLine(prefix + "ID, " + vehicle.ToString());
            //writer.WriteLine(prefix + "VEHICLE, " + vehicle.Vehicle);
            writer.WriteLine(prefix + "AD OPTION DETAILS");
            writer.WriteLine(prefix + "ID, " + option.ID);
            writer.WriteLine(prefix + "NAME, " + option.Name);
            writer.WriteLine(prefix + "INTERACTION DETAILS");
            writer.WriteLine(prefix + "MADE AWARE, " + MadeAware.ToString());
            if (MadeAware)
            {
                //if(vehicle.Type == MediaVehicle.MediaType.Internet)
                //{
                //    writer.WriteLine(prefix + "CLICKED, " + Clicked.ToString());
                //    writer.WriteLine(prefix + "ACTION, " + Action.ToString());
                //}
                writer.WriteLine(prefix + "PERSUASION, " + PersuasionConferred.ToString());
                writer.WriteLine(prefix + "MADE RECENT, " + MadeRecent.ToString());
            }

        }
    }
}
