using System;
using POGOProtos.Enums;

namespace Website.Service.Models
{
    public class Critter
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public TimeSpan TimeTillHidden { get; set; }
        public string TimeTillHiddenString { get; set; }
        public DateTime ServerTime { get; set; }
    }

    public class CritterIV
    {
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Stamina { get; set; }
    }
}