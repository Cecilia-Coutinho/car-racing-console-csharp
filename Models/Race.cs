namespace CarRacingSimulator.Models
{
    public class Race
    {
        public int DefaultDistance { get; set; } = 3; //km
        public int DefaultSpeed { get; set; } = 120; //km/h
        static int HourInSeconds { get; } = 3600; //1h
        public int StartSpeed { get; } = 0; //always starts with 0
        public int SecondsToFinish { get; set; }
        public int TimeRemaining { get; set; }

        public List<IEvent> RandEvents = new List<IEvent>()
        {
            new OutOfGasEvent(),
            new FlatTireEvent(),
            new BirdInWindshieldEvent(),
            new EngineProblemEvent()
        };

        public static int DistanceTakeInSec(int speed, int distance)
        {
            //calculate how long takes total distance in seconds
            double time = (double)distance / (double)speed * (HourInSeconds);
            return (int)time;
        }
    }
}
