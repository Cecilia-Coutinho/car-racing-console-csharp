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
        public double Probability { get; protected set; }
        protected EventBase(string eventName, int penaltyTime)
        {
            EventName = eventName;
            PenaltyTime = penaltyTime;
        }

        public virtual async Task Apply(Car car)
        {
            Console.WriteLine($"{car?.Name?.ToUpper()} experienced {EventName} and needs to wait {PenaltyTime} seconds...");
            await Task.Delay(TimeSpan.FromSeconds(PenaltyTime));
        }
    }

    public class OutOfGasEvent : EventBase
    {
        static int penaltyTime = 30;
        public OutOfGasEvent() : base("Out of gas", penaltyTime) { }
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
        public BirdInWindshieldEvent() : base("Bird in the windshield", penaltyTime) { }
        public override string ToString()
        {
            return $"{EventName} {penaltyTime}";
        }
    }

    public class EngineProblemEvent : EventBase
    {
        static Race race = new Race();
        static int speedReduction = race.DefaultSpeed - 1;
        static int additionalTime = Race.DistanceTakeInSec(speedReduction, race.TimeRemaining) - race.TimeRemaining;
        public EngineProblemEvent() : base("Engine problem", additionalTime) { }
        public override async Task Apply(Car car)
        {
            Console.WriteLine($"{car?.Name?.ToUpper()} experienced {EventName} and reduced his speed's power...");
            await Task.Delay(TimeSpan.FromSeconds(additionalTime));
        }
    }

    //public readonly Random Random = new Random();
    //public bool IsPenalty { get; set; }
    //public TimeSpan PenaltyTime { get; set; }

    //public double Probability { get; set; }

    //public Event(bool isPenalty, TimeSpan newTimeRemaining, double probability)
    //{
    //    IsPenalty = isPenalty;
    //    PenaltyTime = newTimeRemaining;
    //    Probability = probability;
    //}
    //OutOfGas() 30s
    //FlatTire() 20s
    //BirdInWindshield() 10s
    //EngineProblem() -=1km

    //public string OutOfGas(Car car)
    //{
    //    return $"Car {car?.Name?.ToUpper()} is out of gas and needs to refuel...";
    //}


}
