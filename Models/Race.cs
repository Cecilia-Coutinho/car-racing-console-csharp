﻿namespace CarRacingSimulator.Models
{
    namespace CarRacingSimulator.Models
    {
        public class Race
        {
            public int Distance { get; set; } = 10; //km
            public int Speed { get; set; } = 120; //km/h
            public static int HourInSeconds { get; } = 3600; //1h
            public int StartSpeed { get; } = 0; //always starts with 0
            public TimeSpan TimeElapsed { get; set; }
            public TimeSpan TimeRemaining { get; set; }

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

            public static TimeSpan DistanceTakeInSec(int speed, int distance)
            {
                //calculate how long takes total distance in seconds
                double timeInHours = (double)distance / speed;
                TimeSpan time = TimeSpan.FromSeconds(timeInHours * HourInSeconds);
                return time;
            }
        }

        /*
         * average speed = total distance / total time
         * average time = total distance / total speed
         * average distance = total speed * total time
         */
    }
}
