using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRacingSimulator.Models
{
    internal class Race
    {
        public int Distance { get; set; }
        public bool IsFinished { get; set; }
        public Race()
        {
            Distance = 10; //km
            IsFinished = false;
        }
    }
}
