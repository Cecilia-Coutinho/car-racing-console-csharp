namespace CarRacingSimulator.Models
{
    public class Race
    {
        public int DefaultDistance { get; set; } = 10; //km
        public int DefaultSpeed { get; set; } = 120; //km/h
        public static int HourInSeconds { get; } = 3600; //1h
        public int StartSpeed { get; } = 0; //always starts with 0
        public decimal SecondsToFinish { get; set; }
        public decimal TimeRemaining { get; set; }

        public List<IEvent> RandEvents = new()
        {
            new OutOfGasEvent(),
            new FlatTireEvent(),
            new BirdInWindshieldEvent(),
            new EngineProblemEvent()
        };

        public Car carOnTheRace { get; set; }

        public Race(Car car)
        {
            carOnTheRace = car;
        }

        public static decimal DistanceTakeInSec(decimal speed, decimal distance)
        {
            //calculate how long takes total distance in seconds
            decimal time = distance / speed * (HourInSeconds);
            return time;
        }
    }
}
