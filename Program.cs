using CarRacingSimulator.Models;
using CarRacingSimulator.Models.CarRacingSimulator.Models;
using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;

namespace CarRacingSimulator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await RunRace();
            Console.WriteLine("Do you want start a new Car Racing?");
            //to do: add option to choose: restart Y/N
            //
            //
        }

        public static async Task RunRace()
        {
            Car velocityTurtle = new Car("Velocity Turtle");
            Car speedRacer = new Car("Speed Racer");
            Car slowBunny = new Car("Slow Bunny");

            List<Race> races = new List<Race>();
            Race race1 = new Race(velocityTurtle);
            Race race2 = new Race(speedRacer);
            Race race3 = new Race(slowBunny);
            races.Add(race1);
            races.Add(race2);
            races.Add(race3);

            Console.WriteLine($"\nStarting race...\n");

            // Start all races concurrently using Task.WhenAll
            await Task.WhenAll(
                StartRace(race1),
                StartRace(race2),
                StartRace(race3)
            );

            // Determine the winner and print the result
            await DefineWinner(races);

            // Print the end of the race
            Console.WriteLine("\nRace Finished!");
        }

        public static async Task StartRace(Race race)
        {
            race.TimeRemaining = Race.DistanceTakeInSec(race.Speed, race.Distance); //How long will take to finish the race
            TimeSpan timeElapsed = TimeSpan.FromSeconds((double)race.StartSpeed); //How long it took to finish
            var car = race.carOnTheRace;
            bool isTimeRemaining = race.TimeRemaining.TotalSeconds > 0;
            Console.WriteLine($"Time remaining {car.Name}: {race.TimeRemaining.ToString("hh\\:mm\\:ss")}");

            // Loop until the race is finished
            while (isTimeRemaining)
            {
                // Wait for 30 seconds before the next move
                TimeSpan timeToWait = TimeSpan.FromSeconds(30);
                await WaitForEvent(timeToWait, car);
                race.TimeRemaining -= timeToWait;
                timeElapsed += timeToWait;

                Console.WriteLine($"Time remaining {car.Name}: {race.TimeRemaining.ToString("hh\\:mm\\:ss")}");

                // Determine the probability of each event occurring
                decimal outOfGasProbability = 45 / 50 * (100); // to update: 1/50
                decimal flatTireProbability = 40 / 50 * (100); // to update: 2/50
                decimal birdInWindshieldProbability = 40 / 50 * (100); //to update:  5/50
                decimal engineProblemProbability = 50 / 50 * (100); // to update: 10/50
                int rand = new Random().Next(0, 100);

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
                    await randomEvent.Apply(race);
                    timeElapsed += randomEvent.PenaltyTime;
                }

                if (
                    rand <= engineProblemProbability
                    && randomEvent == events.Find(e => e is EngineProblemEvent)
                )
                {
                    if (race.TimeRemaining >= timeToWait)
                    {
                        await randomEvent.Apply(race);
                        // Update remaining time
                        race.TimeRemaining = EngineProblemEvent.SpeedReductionPenalty(race);
                    }
                }

                if (race.TimeRemaining < timeToWait)
                {
                    timeElapsed += race.TimeRemaining;
                    await Task.Delay(TimeSpan.FromSeconds(race.TimeRemaining.TotalSeconds));
                    isTimeRemaining = false;
                }


            };

            // Set the finish time for the car
            race.TimeElapsed = timeElapsed;
            Console.WriteLine($"\n\t|> |> |> {car?.Name?.ToUpper()} finished the race and took {race.TimeElapsed.ToString("hh\\:mm\\:ss")} to complete it.");
        }

        private static async Task DefineWinner(List<Race> races)
        {
            List<string> winners = new();
            string winnerMessage = $"";
            double minTime = double.MaxValue;

            // Find the minimum time across all races and add winners
            foreach (var race in races)
            {
                TimeSpan timeSpentRace = race.TimeElapsed;

                if (timeSpentRace.TotalSeconds < minTime)
                {
                    minTime = timeSpentRace.TotalSeconds;
                    winners.Clear();
                }

                if (race.TimeElapsed.TotalSeconds == minTime)
                {
                    winners.Add(race.carOnTheRace.Name);
                }
            }

            // Determine winner and format message
            for (int i = 0; i < winners.Count; i++)
            {
                if (winners.Count >= 2)
                {
                    // If there is a tie, concatenate the winners into a string
                    string winnersTie = string.Join(", ", winners);
                    winnerMessage = $"Tie between: {winnersTie.ToUpper()}. They finished in {TimeSpan.FromSeconds(minTime)}!";
                }
                else if (winners.Count == 1)
                {
                    winnerMessage = $"{winners[0].ToUpper()} won the race and took {TimeSpan.FromSeconds(minTime)}! CONGRATS!!";
                }
                else
                {
                    // If there are no winners, something went wrong
                    winnerMessage = "No winner found.";
                }
            }

            // Print the winner message and delay for a configurable amount of time
            await Task.Delay(TimeSpan.FromSeconds(2));
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n\t{winnerMessage}");
            Console.ResetColor();
        }

        private static async Task WaitForEvent(TimeSpan timeToWait, Car car)
        {
            await Task.Delay(timeToWait);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n{car?.Name?.ToUpper()} took the turn smoothly without losing too much momentum.");
            Console.ResetColor();
        }
    }
}