namespace CarRacingSimulator.Models
{
    public class Race
    {
        public int DefaultDistance { get; set; } = 3; //km
        public int DefaultSpeed { get; set; } = 120; //km/h
        static int HourInSeconds { get; } = 3600; //1h
        public int StartSpeed { get; } = 0; //always starts with 0
        public double SecondsToFinish { get; set; }
        public double TimeRemaining { get; set; }

        public List<IEvent> RandEvents = new List<IEvent>()
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

        public static double DistanceTakeInSec(double speed, double distance)
        {
            //calculate how long takes total distance in seconds
            double time = distance / speed * (HourInSeconds);
            return time;
        }
    }
}
