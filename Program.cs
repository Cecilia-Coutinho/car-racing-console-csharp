using CarRacingSimulator.Models;
using System;

namespace CarRacingSimulator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Car velocityTurtle = new Car("Velocity Turtle");
            Car speedRacer = new Car("Speed Racer");
            Car slowBunny = new Car("Slow Bunny");

            Race race1 = new Race();
            Race race2 = new Race();
            Race race3 = new Race();

            Console.WriteLine($"Starting race...");

            // Start both races concurrently using Task.WhenAll
            await Task.WhenAll(
                StartRace(velocityTurtle, race1),
                StartRace(speedRacer, race2),
                StartRace(slowBunny, race3)
            );

            // Determine the winner and print the result
            await DefineWinner(race1, race2, race3);

            // Print the end of the race
            Console.WriteLine("Done!");
        }

        public static async Task StartRace(Car car, Race race)
        {
            //Race race = new Race();
            int timeRemaining = Race.DistanceTakeInSec(race.DefaultSpeed, race.DefaultDistance); //How long will take to finish the race
            int timeElapsed = race.StartSpeed; //How long it took to finish

            // Loop until the race is finished
            while (timeRemaining > 0)
            {
                // Wait for 30 seconds before the next move
                int timeToWait = 30;
                await WaitForEvent(timeToWait, car);
                timeRemaining -= timeToWait;
                timeElapsed += timeToWait;

                // Determine the probability of each event occurring
                double outOfGasProbability = 1 / 50 * (100);
                double flatTireProbability = 2 / 50 * (100);
                double birdInWindshieldProbability = 5 / 50 * (100);
                double engineProblemProbability = 6 / 50 * (100); //set right number after tests 10/50
                int rand = new Random().Next(0, 12); //set right number after tests (0,50)

                //randon method to call event
                var events = race.RandEvents;
                var randomEventIndex = new Random().Next(0, events.Count);
                var randomEvent = events[randomEventIndex];
                if (
                    rand <= outOfGasProbability &&
                    randomEvent == events.Find(e => e is OutOfGasEvent)
                    ||
                    rand <= flatTireProbability &&
                    randomEvent == events.Find(e => e is FlatTireEvent)
                    ||
                    rand <= birdInWindshieldProbability
                    && randomEvent == events.Find(e => e is BirdInWindshieldEvent)
                    )
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    await randomEvent.Apply(car);
                    // Add the random event penalty to both the elapsed and remaining time
                    timeRemaining += randomEvent.PenaltyTime;
                    timeElapsed += randomEvent.PenaltyTime;
                }
                if (
                    rand <= engineProblemProbability
                    && randomEvent == events.Find(e => e is EngineProblemEvent)
                )
                {
                    race.TimeRemaining = timeRemaining;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    await randomEvent.Apply(car);
                    // Add the random event penalty to both the elapsed and remaining time
                    timeRemaining += randomEvent.PenaltyTime;
                    timeElapsed += randomEvent.PenaltyTime;
                }
                Console.ResetColor();
            };

            // Set the finish time for the car
            race.SecondsToFinish = timeElapsed;
            Console.WriteLine($"{car.Name} finished the race and took {timeElapsed} seconds to complete it.");
        }

        private static async Task DefineWinner(Race race1, Race race2, Race race3)
        {
            // Determine which car finished first
            int timeSpentRace1 = race1.SecondsToFinish;
            int timeSpentRace2 = race2.SecondsToFinish;
            int timeSpentRace3 = race3.SecondsToFinish;
            var winner = timeSpentRace1 < timeSpentRace2 || timeSpentRace1 < timeSpentRace3 ? "Velocity Turtle" : (timeSpentRace2 < timeSpentRace1 || timeSpentRace2 < timeSpentRace3 ? "Speed Racer" : "Slow Bunny");

            await Task.Delay(TimeSpan.FromSeconds(2));
            if (timeSpentRace1 == timeSpentRace2)
            {
                Console.WriteLine($"Tie! No winners!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{winner} won the race! CONGRATS!!");
                Console.ResetColor();
            }
        }

        private static async Task WaitForEvent(int timeToWait, Car car)
        {
            Console.ForegroundColor = ConsoleColor.White;
            await Task.Delay(TimeSpan.FromSeconds(timeToWait));
            Console.WriteLine($"{car?.Name?.ToUpper()} took the {timeToWait}s turn smoothly without losing too much momentum.");
            Console.ResetColor();
        }
    }
}