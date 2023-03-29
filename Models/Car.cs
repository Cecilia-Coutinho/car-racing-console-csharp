using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRacingSimulator.Models
{
    internal class Car
    {
        public string? CarName { get; set; }
        public int StartSpeed { get; set; }
        public int DefaultSpeed { get; set; }

        public Car(string carName)
        {
            CarName = carName;
            StartSpeed = 0;
            DefaultSpeed = 120; //km/h
        }
    }
}
