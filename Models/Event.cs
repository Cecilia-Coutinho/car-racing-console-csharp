using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRacingSimulator.Models
{
    internal class Event
    {
        public readonly Random Random = new Random();
        public bool IsPenalty { get; set; }
        public TimeSpan PenaltyTime { get; set; }

        public double Probability { get; set; }

        public Event(bool isPenalty, TimeSpan penaltyTime, double probability)
        {
            IsPenalty = isPenalty;
            PenaltyTime = penaltyTime;
            Probability = probability;
        }
        //OutOfGas()
        //FlatTire()
        //BirdInWindshield()
        //EngineProblem()

    }
}
