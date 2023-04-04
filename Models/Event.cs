using System.Diagnostics;
using System.Xml.Linq;

namespace CarRacingSimulator.Models
{
    public interface IEvent
    {
        //each event must have:
        string EventName { get; }
        decimal PenaltyTime { get; }
        Task Apply(Race race);
    }
    public abstract class EventBase : IEvent
    {
        // default implementations for common methods to reduce duplication 
        public string EventName { get; protected set; }
        public decimal PenaltyTime { get; protected set; }
        protected EventBase(string eventName, decimal penaltyTime)
        {
            EventName = eventName;
            PenaltyTime = penaltyTime;
        }

        public virtual async Task Apply(Race race)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n\t(´º_º`) {race.carOnTheRace?.Name?.ToUpper()} experienced {EventName} and needs to wait {PenaltyTime} seconds...");
            await Task.Delay(TimeSpan.FromSeconds((double)PenaltyTime));
            if (EventName != "Engine Problem")
            {
                Console.WriteLine($"\n( ^_^) Look who's back and ready to roll - it's {race.carOnTheRace?.Name?.ToUpper()}! Watch out, everyone else, this car is coming in hot!");
            }
        }
    }

    public class OutOfGasEvent : EventBase
    {
        static decimal penaltyTime = 30;
        public OutOfGasEvent() : base("Out Of Gas", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    public class FlatTireEvent : EventBase
    {
        static decimal penaltyTime = 20;
        public FlatTireEvent() : base("Flat Tire", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    public class BirdInWindshieldEvent : EventBase
    {
        static decimal penaltyTime = 10;
        public BirdInWindshieldEvent() : base("Bird In The Windshield", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    public class EngineProblemEvent : EventBase
    {
        static decimal penaltyTime = 0;
        public EngineProblemEvent() : base("Engine Problem", penaltyTime) { }
        public override async Task Apply(Race race)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n\t(._.) {race.carOnTheRace?.Name?.ToUpper()} experienced {EventName} and reduced his speed's power...");
            await Task.Delay(TimeSpan.FromSeconds((double)penaltyTime));
        }
        public static decimal SpeedReductionPenalty(Race race)
        {
            /*
             * average speed = total distance / total time
             * average time = total distance / total speed
             * average distance = total speed * total time
             */

            decimal speedReduction = 1;
            decimal reducedSpeed = race.DefaultSpeed - speedReduction;
            decimal newDistance = reducedSpeed * (race.TimeRemaining / Race.HourInSeconds);
            decimal timeInSeconds = newDistance / reducedSpeed * (Race.HourInSeconds);
            Console.WriteLine($"Time to cover {newDistance:F2}meters at {reducedSpeed} km/h: {timeInSeconds:F2} seconds");
            return timeInSeconds;
        }
    }
}
