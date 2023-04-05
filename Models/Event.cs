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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n\t(´º_º`) {race.carOnTheRace?.Name?.ToUpper()} experienced {EventName} and needs to wait {PenaltyTime} seconds...");
            await Task.Delay(PenaltyTime);
            if (EventName != "Engine Problem")
            {
                Console.WriteLine($"\n( ^_^) Look who's back and ready to roll - it's {race.carOnTheRace?.Name?.ToUpper()}! Watch out, everyone else, this car is coming in hot!");
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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n\t(._.) {race.carOnTheRace?.Name?.ToUpper()} experienced {EventName} and reduced his speed's power...");
            await Task.Delay(penaltyTime);
        }
        public static TimeSpan SpeedReductionPenalty(Race race)
        {
            string carName = race.carOnTheRace.Name;
            if (race.Speed > 5)
            {
                race.Speed -= 5;
            }

            if (race.TimeRemaining.TotalSeconds <= double.MaxValue)
            {
                double speedInSec = (double)race.Speed / (double)Race.HourInSeconds;
                double newDistance = speedInSec * race.TimeRemaining.TotalSeconds;
                //int newDistanceInKm = (int)Math.Round(newDistance);
                if (newDistance < 0)
                {
                    newDistance = 1; //1km takes 30sec
                }
                race.Distance = (int)Math.Round(newDistance);
            }
            else
            {
                Console.WriteLine("Error: the time remaining for this race is too long.");
            }

            double timeInSec = ((double)race.Distance / race.Speed) * 3600;
            TimeSpan time = TimeSpan.FromSeconds(timeInSec);
            race.TimeRemaining = time;

            Console.WriteLine($"\n\t{carName.ToUpper()} have extra time to finish the race at a speed of {race.Speed:F} km/h?\n\t New RemainingTime:{race.TimeRemaining.ToString("hh\\:mm\\:ss")}");
            return race.TimeRemaining;

        }

        //public static TimeSpan DistanceTakeInSec(int speed, int distance)
        //{
        //    //calculate how long takes total distance in seconds
        //    double timeInDouble = (double)distance / speed;
        //    TimeSpan time = TimeSpan.FromSeconds(timeInDouble * HourInSeconds);
        //    return time;
        //}
    }
}
