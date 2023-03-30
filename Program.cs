using CarRacingSimulator.Models;

namespace CarRacingSimulator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Race race = new Race();
            //Console.WriteLine(Race.DistanceTakeInSec(race.DefaultSpeed, race.DefaultDistance));
            Car velocityTurttle = new Car("Velocity Turttle");
            Car speedRacer = new Car("Speed Racer");

            Race race1 = new Race();
            Race race2 = new Race();

            Console.WriteLine($"Starting race...");

            // Start both races concurrently using Task.WhenAll
            await Task.WhenAll(
                StartRace(velocityTurttle, race1),
                StartRace(speedRacer, race2)
            );

            // Determine the winner and print the result
            await DefineWinner(race1, race2);

            // Print the end of the race
            Console.WriteLine("Done!");

        }

        public static async Task StartRace(Car car, Race race)
        {
            //Race race = new Race();
            int timeRemaining = Race.DistanceTakeInSec(race.DefaultSpeed, race.DefaultDistance); //How long will take to finish the race
            int timeElapsed = race.StartSpeed; //How long it took to finish

            while (timeRemaining > 0)
            {
                await WaitForEvent(30);
                timeRemaining -= 30;
                timeElapsed += 30;

                //randon method to call event
                var events = race.RandEvents;
                var randomEventIndex = new Random().Next(0, events.Count);
                var randomEvent = events[randomEventIndex];
                await randomEvent.Apply(car);
                timeRemaining += randomEvent.PenaltyTime;
                timeElapsed += randomEvent.PenaltyTime;
            };

            // Set the finish time for the car
            race.SecondsToFinish = timeElapsed;

            Console.WriteLine($"{car.Name} took {timeElapsed} seconds to complete the race.");
            await Task.Delay(TimeSpan.FromSeconds(3));
            Console.WriteLine("Race completed");
        }

        private static async Task DefineWinner(Race race1, Race race2)
        {
            // Determine which car finished first
            var winner = race1.SecondsToFinish < race2.SecondsToFinish ? "Velocity Turttle" : "Speed Racer";
            await Task.Delay(TimeSpan.FromSeconds(3));
            Console.WriteLine($"{winner} won the race!");
        }

        private static async Task WaitForEvent(int timeToWait)
        {
            await Task.Delay(TimeSpan.FromSeconds(timeToWait));
            Console.WriteLine("\nwait Completed\n");
        }

        // Generate a random number
        // Create cars
        // Convert speed from km/h to km/s
        // Start race
        // Method simulate car's movement
        // Move each car
        // Print the status
        // Wait for 30 seconds before the next move
        // Check if a problem occurs
        // The car runs out of gas
        // The car has a flat tire
        // A bird hits the car's windshield
        // The car has a mechanical problem
        // Update the car's distance
        // Check if the car has finished the race
        // Print the winner
    }
}