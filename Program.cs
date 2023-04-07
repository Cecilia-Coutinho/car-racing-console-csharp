using CarRacingSimulator.Models;
using CarRacingSimulator.Models.CarRacingSimulator.Models;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ConstrainedExecution;

namespace CarRacingSimulator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await RunRace();
            Console.WriteLine("Would you like to start a new Car Racing?");
            //to do: add option to choose: restart Y/N
        }

        public static async Task RunRace()
        {
            Console.WriteLine("\nCar Racing Console Simulator 1.0" +
                "\nOnce the Race starts, press any key to see its status.");

            Car velocityTurtle = new Car("Velocity Turtle");
            Car speedRacer = new Car("Speed Racer");
            Car slowBunny = new Car("Slow Bunny");

            List<Race> races = new List<Race>();
            Race race1 = new(velocityTurtle);
            Race race2 = new(speedRacer);
            Race race3 = new(slowBunny);
            races.Add(race1);
            races.Add(race2);
            races.Add(race3);

            string startMessage = "Race Starting...";
            string finishMessage = "Race Finished!";

            // Print the start of the race
            ASCIICarRacingMessage(startMessage);

            // Launch the task to wait for a keypress
            Task consoleKeyTask = Task.Run(() => { _ = RaceStatus(races); });

            // ... Launch other async tasks ...
            await Task.WhenAll(
                StartRace(race1),
                StartRace(race2),
                StartRace(race3)
            );

            // Waits for the keypress to end it all
            await consoleKeyTask;

            // Determine the winner and print the result
            await DefineWinner(races);

            // Print the end of the race
            ASCIICarRacingMessage(finishMessage);
        }

        public static async Task StartRace(Race race)
        {
            race.TimeRemaining = await Race.DistanceTakeInSec(race.Speed, (double)race.Distance); //How long will take to finish the race
            race.TimeElapsed = TimeSpan.FromSeconds((double)race.StartSpeed); //How long it took to finish
            var car = race.CarOnTheRace;
            bool isTimeRemaining = race.TimeRemaining.TotalSeconds > 0;

            // Loop until the race is finished
            while (isTimeRemaining)
            {
                // Wait for 30 seconds before the next move
                TimeSpan timeToWait = TimeSpan.FromSeconds(30);
                await WaitForEvent(timeToWait, car);
                race.TimeRemaining -= timeToWait;
                race.TimeElapsed += timeToWait;

                // Determine the probability of each event occurring
                decimal outOfGasProbability = (30 / 50) * 100;
                decimal flatTireProbability = (40 / 50) * 100;
                decimal birdInWindshieldProbability = (50 / 50) * 100;
                decimal engineProblemProbability = (50 / 50) * 100; ;
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
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    await randomEvent.Apply(race);
                    Console.ResetColor();
                    race.TimeElapsed += randomEvent.PenaltyTime;
                }

                if (
                    rand <= engineProblemProbability
                    && randomEvent == events.Find(e => e is EngineProblemEvent)
                )
                {
                    if (race.TimeRemaining >= timeToWait)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        await randomEvent.Apply(race);
                        Console.ResetColor();
                    }
                }

                if (race.TimeRemaining < timeToWait)
                {
                    race.TimeElapsed += race.TimeRemaining;
                    await Task.Delay(TimeSpan.FromSeconds(race.TimeRemaining.TotalSeconds));
                    race.TimeRemaining = TimeSpan.FromSeconds(0);
                    isTimeRemaining = false;
                }
            };

            // Set the finish time for the car
            Console.WriteLine($"\n\t|> |> |> {car?.Name?.ToUpper()} finished the race and took {race.TimeElapsed.ToString("hh\\:mm\\:ss")} to complete it.");
        }

        public static async Task RaceStatus(List<Race> races)
        {
            bool isRaceRunning = true;

            // status of all cars
            while (isRaceRunning)
            {
                Console.ReadKey(true);
                await Task.Delay(TimeSpan.FromSeconds(1));
                races.ForEach(async race =>
                {
                    string carName = race.CarOnTheRace.Name;
                    TimeSpan elapsedTime = race.TimeElapsed;
                    TimeSpan remainingTime = race.TimeRemaining;
                    int currentSpeed = race.Speed;
                    int distanceLeft = await Race.UpdateDistance(race);

                    Console.WriteLine($"\n{carName} has an elapsed time of {elapsedTime} and has an average time to finish in {remainingTime}" +
                        $"\nCurrent Speed: {currentSpeed} km/h. " +
                        $"\nDistance left: {distanceLeft} km." +
                        $"\n----------------------------------");
                });

                var totalRemaining = races.Select(race => race.TimeRemaining.TotalSeconds).Sum();

                if (totalRemaining == 0)
                {
                    isRaceRunning = false;
                }
            }
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
                    winners.Add(race.CarOnTheRace.Name);
                }
            }

            // Determine winner and format message
            for (int i = 0; i < winners.Count; i++)
            {
                if (winners.Count >= 2)
                {
                    // If there is a tie, concatenate the winners into a string
                    string winnersTie = string.Join(", ", winners);
                    winnerMessage = $"Tie between: {winnersTie.ToUpper()}. They finished in {TimeSpan.FromSeconds(minTime)}! \\(^ᴗ^)/";
                }
                else if (winners.Count == 1)
                {
                    winnerMessage = $" \\(^ᴗ^)/ {winners[0].ToUpper()} won the race and took {TimeSpan.FromSeconds(minTime)} to finish it! CONGRATS!!";
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
            Console.WriteLine($"\n{car?.Name?.ToUpper()} took the turn smoothly without losing too much momentum.");
        }

        private static void ASCIICarRacingMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"" +
                $"\n__" +
                $"\n.-'--`-._" +
                $"\n'-O---O--'  {message}\n");
            Console.ResetColor();
        }
    }
}