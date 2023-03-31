using System.Xml.Linq;

namespace CarRacingSimulator.Models
{
    public interface IEvent
    {
        //each event must have:
        string EventName { get; }
        int PenaltyTime { get; }
        Task Apply(Car car);
    }
    public abstract class EventBase : IEvent
    {
        // default implementations for common methods to reduce duplication 
        public string EventName { get; protected set; }
        public int PenaltyTime { get; protected set; }
        protected EventBase(string eventName, int penaltyTime)
        {
            EventName = eventName;
            PenaltyTime = penaltyTime;
        }

        public virtual async Task Apply(Car car)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n\t{car?.Name?.ToUpper()} experienced {EventName} and needs to wait {PenaltyTime} seconds...");
            await Task.Delay(TimeSpan.FromSeconds(PenaltyTime));
            if (EventName != "Engine Problem")
            {
                Console.WriteLine($"\nLook who's back and ready to roll - it's {car?.Name?.ToUpper()}! Watch out, everyone else, this car is coming in hot!");
            }
            Console.ResetColor();
        }
    }

    public class OutOfGasEvent : EventBase
    {
        static int penaltyTime = 30;
        public OutOfGasEvent() : base("Out Of Gas", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    public class FlatTireEvent : EventBase
    {
        static int penaltyTime = 20;
        public FlatTireEvent() : base("Flat Tire", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    public class BirdInWindshieldEvent : EventBase
    {
        static int penaltyTime = 10;
        public BirdInWindshieldEvent() : base("Bird In The Windshield", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    public class EngineProblemEvent : EventBase
    {
        static Race race = new Race();
        static int speedReduction = race.DefaultSpeed - 1;
        static int newDistanceInSec = Race.DistanceTakeInSec(speedReduction, race.DefaultDistance);
        static int penaltyTime = newDistanceInSec - race.TimeRemaining;
        public EngineProblemEvent() : base("Engine Problem", penaltyTime) { }
        public override async Task Apply(Car car)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n\t{car?.Name?.ToUpper()} experienced {EventName} and reduced his speed's power...");
            await Task.Delay(TimeSpan.FromSeconds(penaltyTime));
        }
    }
}
