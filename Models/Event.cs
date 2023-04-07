using CarRacingSimulator.Models.CarRacingSimulator.Models;
using System.Diagnostics;
using System.Xml.Linq;

namespace CarRacingSimulator.Models
{
    public interface IEvent
    {
        //each event must have:
        string EventName { get; }
        TimeSpan PenaltyTime { get; }
        Task Apply(Race race);
    }
    public abstract class EventBase : IEvent
    {
        // default implementations for common methods to reduce duplication 
        public string EventName { get; protected set; }
        public TimeSpan PenaltyTime { get; protected set; }
        protected EventBase(string eventName, TimeSpan penaltyTime)
        {
            EventName = eventName;
            PenaltyTime = penaltyTime;
        }

        public virtual async Task Apply(Race race)
        {
            Console.WriteLine($"\n\t(´º_º`) {race.CarOnTheRace?.Name?.ToUpper()} experienced {EventName} and needs to wait {PenaltyTime} seconds...");
            await Task.Delay(PenaltyTime);
            if (EventName != "Engine Problem")
            {
                Console.WriteLine($"\n( ^_^) Look who's back and ready to roll - it's {race.CarOnTheRace?.Name?.ToUpper()}! Watch out, everyone else, this car is coming in hot!");
            }
        }
    }

    public class OutOfGasEvent : EventBase
    {
        static TimeSpan penaltyTime = TimeSpan.FromSeconds(30);
        public OutOfGasEvent() : base("Out Of Gas", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    public class FlatTireEvent : EventBase
    {
        static TimeSpan penaltyTime = TimeSpan.FromSeconds(20);
        public FlatTireEvent() : base("Flat Tire", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    public class BirdInWindshieldEvent : EventBase
    {
        static TimeSpan penaltyTime = TimeSpan.FromSeconds(10);
        public BirdInWindshieldEvent() : base("Bird In The Windshield", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    public class EngineProblemEvent : EventBase
    {
        static TimeSpan penaltyTime = TimeSpan.FromSeconds(0);
        public EngineProblemEvent() : base("Engine Problem", penaltyTime) { }
        public override async Task Apply(Race race)
        {
            Console.WriteLine($"\n\t(._.) {race.CarOnTheRace?.Name?.ToUpper()} experienced {EventName} and reduced his speed's power...");
            SpeedReductionPenalty(race);
            await Task.Delay(penaltyTime);
        }
        public static void SpeedReductionPenalty(Race race)
        {
            string carName = race.CarOnTheRace.Name;
            int speedReduction = 5;

            if (race.Speed > speedReduction)
            {
                race.Speed -= speedReduction;
            }

            //update remaining time
            Race.UpdateRemainingTime(race);

            Console.WriteLine($"\n¯\\_('_')_/¯ {carName.ToUpper()} has extra time to finish the race at a speed of {race.Speed} km/h.");
        }
    }
}
