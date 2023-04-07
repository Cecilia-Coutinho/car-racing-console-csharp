using System.Diagnostics;

namespace CarRacingSimulator.Models
{
    namespace CarRacingSimulator.Models
    {
        public class Race
        {
            public int Distance { get; set; } = 3; //km
            public int Speed { get; set; } = 120; //km/h
            public static readonly int HourInSeconds = 3600; //1h
            public readonly int StartSpeed = 0; //always starts with 0
            public TimeSpan TimeElapsed { get; set; }
            public TimeSpan TimeRemaining { get; set; }

            public List<IEvent> RandEvents = new()
        {
            new OutOfGasEvent(),
            new FlatTireEvent(),
            new BirdInWindshieldEvent(),
            new EngineProblemEvent()
        };

            public Car CarOnTheRace { get; set; }

            public Race(Car car)
            {
                CarOnTheRace = car;
            }

            public static TimeSpan DistanceTakeInSec(int speed, double distance)
            {
                //calculate how long takes total distance in seconds
                double timeInHours = distance / speed;
                TimeSpan time = TimeSpan.FromSeconds(timeInHours * HourInSeconds);
                return time;
            }
            public static int UpdateDistance(Race race)
            {
                double speed = (double)race.Speed;
                double secondsPerHour = (double)Race.HourInSeconds;
                TimeSpan timeRemaining = race.TimeRemaining;

                //update distance
                double speedInSec = speed / secondsPerHour;
                double newDistance = speedInSec * timeRemaining.TotalSeconds;
                race.Distance = (int)Math.Round(newDistance);
                return race.Distance;
            }
            public static void UpdateRemainingTime(Race race)
            {
                double distance = UpdateDistance(race);
                double timeInSec = (distance / race.Speed) * HourInSeconds;
                int newRemainingTime = (int)Math.Round(timeInSec);
                TimeSpan time = TimeSpan.FromSeconds(newRemainingTime);
                race.TimeRemaining = time;
            }
        }

        /*
         * average speed = total distance / total time
         * average time = total distance / total speed
         * average distance = total speed * total time
         */
    }
}
