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

            Console.WriteLine($"\nStarting race...\n");

            // Start both races concurrently using Task.WhenAll
            await Task.WhenAll(
                StartRace(velocityTurtle, race1),
                StartRace(speedRacer, race2),
                StartRace(slowBunny, race3)
            );

            // Determine the winner and print the result
            await DefineWinner(race1, race2, race3, velocityTurtle, speedRacer, slowBunny);

            // Print the end of the race
            Console.WriteLine("\nRace Finished!");
            //
        }

        public static async Task StartRace(Car car, Race race)
        {
            race.TimeRemaining = Race.DistanceTakeInSec(race.DefaultSpeed, race.DefaultDistance); //How long will take to finish the race
            int timeElapsed = race.StartSpeed; //How long it took to finish

            // Loop until the race is finished
            while (race.TimeRemaining > 0)
            {
                // Wait for 30 seconds before the next move
                int timeToWait = 30;
                await WaitForEvent(timeToWait, car);
                race.TimeRemaining -= timeToWait;
                timeElapsed += timeToWait;

                // Determine the probability of each event occurring
                double outOfGasProbability = 35 / 50 * (100); // to update: 1/50
                double flatTireProbability = 40 / 50 * (100); // to update: 2/50
                double birdInWindshieldProbability = 45 / 50 * (100); //to update:  5/50
                double engineProblemProbability = 50 / 50 * (100); // to update: 10/50
                int rand = new Random().Next(0, 100); //set right number after tests (0,100)

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
                    await randomEvent.Apply(car);
                    // Add the random event penalty to the elapsed time
                    //race.TimeRemaining += randomEvent.PenaltyTime;
                    timeElapsed += randomEvent.PenaltyTime;
                }

                if (
                    rand <= engineProblemProbability
                    && randomEvent == events.Find(e => e is EngineProblemEvent)
                )
                {
                    await randomEvent.Apply(car);
                    // Add the random event penalty to both the elapsed and remaining time
                    //race.TimeRemaining += randomEvent.PenaltyTime;
                    timeElapsed += randomEvent.PenaltyTime;
                }
            };

            // Set the finish time for the car
            race.SecondsToFinish = timeElapsed;
            Console.WriteLine($"\n\t{car?.Name?.ToUpper()} finished the race and took {timeElapsed} seconds to complete it.");
        }

        private static async Task DefineWinner(Race race1, Race race2, Race race3, Car car1, Car car2, Car car3)
        {
            // Determine which car finished first
            int timeSpentRace1 = race1.SecondsToFinish;
            int timeSpentRace2 = race2.SecondsToFinish;
            int timeSpentRace3 = race3.SecondsToFinish;

            // Determine the winner of the race
            string? winner;
            if (timeSpentRace1 < timeSpentRace2 && timeSpentRace1 < timeSpentRace3)
            {
                winner = car1.Name;
            }
            else if (timeSpentRace2 < timeSpentRace1 && timeSpentRace2 < timeSpentRace3)
            {
                winner = car2.Name;
            }
            else
            {
                winner = car3.Name;
            }

            await Task.Delay(TimeSpan.FromSeconds(2));
            if (timeSpentRace1 == timeSpentRace2 ||
                timeSpentRace1 == timeSpentRace3 ||
                timeSpentRace2 == timeSpentRace3)
            {
                var tieMessage = "";

                if (timeSpentRace1 == timeSpentRace2 && timeSpentRace1 < timeSpentRace3)
                {
                    tieMessage = $"{car1.Name} and {car2.Name}. They finished in {timeSpentRace1} seconds";
                }
                else if (timeSpentRace1 == timeSpentRace3 && timeSpentRace1 < timeSpentRace2)
                {
                    tieMessage = $"{car1.Name} and {car3.Name}. They finished in {timeSpentRace1} seconds";
                }
                else if (timeSpentRace2 == timeSpentRace3 && timeSpentRace2 < timeSpentRace1)
                {
                    tieMessage = $"{car2.Name} and {car3.Name}. They finished in {timeSpentRace2} seconds";
                }
                else
                {
                    tieMessage = $"{car1.Name}, {car2.Name} and {car3.Name}. They finished in {timeSpentRace1} seconds";
                }

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n\tTie between {tieMessage} !");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n\t{winner} won the race! CONGRATS!!");
                Console.ResetColor();
            }
        }

        private static async Task WaitForEvent(int timeToWait, Car car)
        {
            await Task.Delay(TimeSpan.FromSeconds(timeToWait));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n{car?.Name?.ToUpper()} took the turn smoothly without losing too much momentum.");
            Console.ResetColor();
        }
    }
}