using System.Xml.Linq;

namespace CarRacingSimulator.Models
{
    public interface IEvent
    {
        //each event must have:
        string EventName { get; }
        double PenaltyTime { get; }
        Task Apply(Race race);
    }
    public abstract class EventBase : IEvent
    {
        // default implementations for common methods to reduce duplication 
        public string EventName { get; protected set; }
        public double PenaltyTime { get; protected set; }
        protected EventBase(string eventName, double penaltyTime)
        {
            EventName = eventName;
            PenaltyTime = penaltyTime;
        }

        public virtual async Task Apply(Race race)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n\t(´･_･`) {race.carOnTheRace?.Name?.ToUpper()} experienced {EventName} and needs to wait {PenaltyTime} seconds...");
            await Task.Delay(TimeSpan.FromSeconds(PenaltyTime));
            if (EventName != "Engine Problem")
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"\n( ఠൠఠ )ﾉ Look who's back and ready to roll - it's {race.carOnTheRace?.Name?.ToUpper()}! Watch out, everyone else, this car is coming in hot!");
            }
            Console.ResetColor();
        }
    }

    public class OutOfGasEvent : EventBase
    {
        static double penaltyTime = 30.30;
        public OutOfGasEvent() : base("Out Of Gas", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    public class FlatTireEvent : EventBase
    {
        static double penaltyTime = 20;
        public FlatTireEvent() : base("Flat Tire", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    public class BirdInWindshieldEvent : EventBase
    {
        static double penaltyTime = 10;
        public BirdInWindshieldEvent() : base("Bird In The Windshield", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    //TODO: REVIEW THIS LOGIC
    public class EngineProblemEvent : EventBase
    {
        static double penaltyTime;
        public EngineProblemEvent() : base("Engine Problem", penaltyTime) { }
        public override async Task Apply(Race race)
        {
            double speedReduction = 1;
            double reducedSpeed = race.DefaultSpeed - speedReduction;
            double newDistanceInSec = Race.DistanceTakeInSec(reducedSpeed, race.DefaultDistance);
            penaltyTime = newDistanceInSec - race.TimeRemaining;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n\t(._.) {race.carOnTheRace?.Name?.ToUpper()} experienced {EventName} and reduced his speed's power...");
            await Task.Delay(TimeSpan.FromSeconds(penaltyTime));
        }
    }
}
